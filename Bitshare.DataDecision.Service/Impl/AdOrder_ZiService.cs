using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Service.Enum;
using Bitshare.PTMM.Model;
using Bitshare.PTMM.Common;
using System.Data;
using System.Data.SqlClient;
namespace Bitshare.PTMM.Service.Impl
{
    #region 工厂定义
    /// <summary>
    /// wangyj
    /// 2016/6/29
    /// </summary>
    public class AdOrder_ZiServiceFactory
    {
        static IAdOrder_ZiService Instance;
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public IAdOrder_ZiService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AdOrder_ZiService();
            }
            return Instance;
        }
    }

    #endregion
    #region 实现方法
    /// <summary>
    /// 媒体制作-自备车制作-媒体代理
    /// wangyj
    /// 2016/6/29
    /// </summary>
    internal class AdOrder_ZiService : IAdOrder_ZiService
    {
        /// <summary>
        /// 获取媒体制作列表数据
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="loginName">登陆名</param>
        /// <returns></returns>
        public SubPageResult<tblAdOrder> GetZAdOrderList(PageInfo pager, string loginName, string title)
        {
            SubPageResult<tblAdOrder> result = new SubPageResult<tblAdOrder>();
            result = CommonHelper.ListSubPageResult<tblAdOrder>(pager);
            return result;
        }
        /// <summary>
        /// 根据不同页面条件不同
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string GetAdOrderCondtion(string title)
        {
            string condition = "";
            //通过模块来控制的权限
            switch (title)
            {
                case "媒体制作":
                    condition += " AND ContractType = '媒体制作' and adorderid not in (SELECT adorderid " + "FROM tblProcesslist " + "WHERE  (ProcessName =  '媒体制作合同') AND (NodeName ='合同确认' and PassFlag=1))";
                    break;
                case "自备车制作":
                    condition += " AND ContractType = '自备车制作' and adorderid not in (SELECT  adorderid " + "FROM tblProcesslist " + "WHERE  (ProcessName =  '自备车制作合同') AND (NodeName ='合同确认' and PassFlag=1))";
                    break;
                case "媒体代理":
                    condition += " AND ContractType = '媒体代理' and adorderid not in (SELECT  adorderid " + "FROM tblProcesslist " + "WHERE (ProcessName =  '代理合同') AND (NodeName ='合同确认' and PassFlag=1))";
                    break;
                default:
                    break;
            }
            return condition;
        }
        /// <summary>
        /// 保存之前生成新订单号和生成新媒体明细
        /// </summary>
        /// <param name="UlWebGrid"></param>
        /// <returns></returns>
        public string MainGridBeforeSave(string title, string AdOrderId, string MediaTypeName)
        {
            string AdOrderIdNew = "";
            if (title.Contains("媒体制作"))
            {
                int DtOrderId = 0;
                string sql = "select top 1 AdOrderId from tblAdOrder where AdOrderId like '%" + "Z" + AdOrderId + "%' ORDER BY AdOrderId DESC";
                SqlDataReader reader = DBContext.PTMMHZ.GetReader(sql);
                if (reader.Read())
                {
                    string OrderId = Convert.ToString(reader["AdOrderId"]);
                    string[] OrderArr = OrderId.Split('-');
                    if (OrderArr.Length > 1)
                    {
                        DtOrderId = Convert.ToInt32(OrderArr[1]);
                    }
                }
                reader.Close();
                if (DtOrderId == 0)
                {
                    AdOrderIdNew = "Z" + AdOrderId + "-1";
                    LoadAdMediaList(AdOrderId, MediaTypeName, "Z" + AdOrderId + "-1", title);
                }
                else
                {
                    DtOrderId = DtOrderId + 1;
                    AdOrderIdNew = "Z" + AdOrderId + "-" + DtOrderId;
                    LoadAdMediaList(AdOrderId, MediaTypeName, "Z" + AdOrderId + "-" + DtOrderId, title);
                }
            }
            else if (title.Contains("自备车制作"))
            {
                AdOrderIdNew = AutoGenerationOrderIDbOrd("B");
            }
            else
            {
                AdOrderIdNew = AutoGenerationOrderIDbOrd("D");
            }

            return AdOrderIdNew;
        }
        /// <summary>
        /// 添加明细
        /// </summary>
        /// <param name="AdOrderId"></param>
        /// <param name="MediaType"></param>
        /// <param name="NewAdOrderId"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        public string LoadAdMediaList(string AdOrderId, string MediaType, string NewAdOrderId, string Title)
        {
            string msg = "添加明细失败";
            List<string> sqlList = new List<string>();
            //if (MediaType == "车身")
            //{
                string sql = "select * from tblAdFixingList where AdOrderId = '" + AdOrderId + "'";
                DataTable dt_FixingList = DBContext.PTMMHZ.GetDataTable(sql);
                sql = "select AdContent,MEDIANAME,ROADSORTNAME,VEHICLETYPENAME,MEIDANUM,INSUREISSUEDATE,INSUREENDDATE,MEDIAUNITPRICE,FACTUREPRICE,REMARK from tblAdOrderMediaList where AdOrderId = '" + AdOrderId + "'";
                DataTable dt_AdOrderList = DBContext.PTMMHZ.GetDataTable(sql);
                sql = "select AdOrderId from tblAdFixinglist_Zi where AdOrderId = '" + NewAdOrderId + "'";
                SqlDataReader reader = DBContext.PTMMHZ.GetReader(sql);
                if (!reader.Read())
                {
                    foreach (DataRow ds_row in dt_FixingList.Rows)
                    {
                        string AdContent = ds_row["AdContent"] == DBNull.Value ? "" : Convert.ToString(ds_row["AdContent"]);
                        string MediaName = ds_row["MediaName"] == DBNull.Value ? "" : Convert.ToString(ds_row["MediaName"]);
                        string RoadSortName = ds_row["RoadSortName"] == DBNull.Value ? "" : Convert.ToString(ds_row["RoadSortName"]);
                        string VehicleTypeName = ds_row["VehicleTypeName"] == DBNull.Value ? "" : Convert.ToString(ds_row["VehicleTypeName"]);
                        string FixDate = ds_row["FixDate"] == DBNull.Value ? "" : Convert.ToString(ds_row["FixDate"]);
                        string IssueDate = ds_row["InsureIssueDate"] == DBNull.Value ? "" : Convert.ToString(ds_row["InsureIssueDate"]);
                        string EndDate = ds_row["InsureEndDate"] == DBNull.Value ? "" : Convert.ToString(ds_row["InsureEndDate"]);
                        string VehicleNumbering = ds_row["VehicleNumbering"] == DBNull.Value ? "" : Convert.ToString(ds_row["VehicleNumbering"]);
                        string ProcessID = ds_row["ProcessID"] == DBNull.Value ? "" : Convert.ToString(ds_row["ProcessID"]);
                        string MakeState = ds_row["MakeState"] == DBNull.Value ? "" : Convert.ToString(ds_row["MakeState"]);
                        string StickersDate = ds_row["StickersDate"] == DBNull.Value ? "" : Convert.ToString(ds_row["StickersDate"]);
                        string SetDate = ds_row["SetDate"] == DBNull.Value ? "" : Convert.ToString(ds_row["SetDate"]);
                        string DesignType = ds_row["DesignType"] == DBNull.Value ? "" : Convert.ToString(ds_row["DesignType"]);
                        string MakeMode = ds_row["MakeMode"] == DBNull.Value ? "" : Convert.ToString(ds_row["MakeMode"]);
                        string Remark = ds_row["Remark"] == DBNull.Value ? "" : Convert.ToString(ds_row["Remark"]);
                        if (Title.Contains("媒体制作"))
                        {
                            sql = "Insert into tblAdFixinglist_Zi (ProcessId,AdOrderId,AdContent,MediaName,RoadSortName,VehicleTypeName,InsureIssueDate,InsureEndDate,VehicleNumbering,FixDate,MakeState,SetDate,DesignType,MakeMode,Remark) values('" + ProcessID + "','" + NewAdOrderId + "','" + AdContent + "','" + MediaName + "','" + RoadSortName + "','" + VehicleTypeName + "','" + IssueDate + "','" + EndDate + "','" + VehicleNumbering + "','" + FixDate + "','" + MakeState + "','" + SetDate + "','" + DesignType + "','" + MakeMode + "','" + Remark + "')";
                            if (string.IsNullOrEmpty(SetDate) || string.IsNullOrEmpty(FixDate))
                            {
                                if (string.IsNullOrEmpty(SetDate) && string.IsNullOrEmpty(FixDate))
                                {
                                    sql = "Insert into tblAdFixinglist_Zi (ProcessId,AdOrderId,AdContent,MediaName,RoadSortName,VehicleTypeName,InsureIssueDate,InsureEndDate,VehicleNumbering,MakeState,DesignType,MakeMode,Remark) values('" + ProcessID + "','" + NewAdOrderId + "','" + AdContent + "','" + MediaName + "','" + RoadSortName + "','" + VehicleTypeName + "','" + IssueDate + "','" + EndDate + "','" + VehicleNumbering + "','" + MakeState + "','" + DesignType + "','" + MakeMode + "','" + Remark + "')";
                                }
                                else if (string.IsNullOrEmpty(SetDate) && !string.IsNullOrEmpty(FixDate))
                                {
                                    sql = "Insert into tblAdFixinglist_Zi (ProcessId,AdOrderId,AdContent,MediaName,RoadSortName,VehicleTypeName,InsureIssueDate,InsureEndDate,VehicleNumbering,FixDate,MakeState,DesignType,MakeMode,Remark) values('" + ProcessID + "','" + NewAdOrderId + "','" + AdContent + "','" + MediaName + "','" + RoadSortName + "','" + VehicleTypeName + "','" + IssueDate + "','" + EndDate + "','" + VehicleNumbering + "','" + FixDate + "','" + MakeState + "','" + DesignType + "','" + MakeMode + "','" + Remark + "')";
                                }
                                if (string.IsNullOrEmpty(FixDate) && !string.IsNullOrEmpty(SetDate))
                                {
                                    sql = "Insert into tblAdFixinglist_Zi (ProcessId,AdOrderId,AdContent,MediaName,RoadSortName,VehicleTypeName,InsureIssueDate,InsureEndDate,VehicleNumbering,MakeState,SetDate,DesignType,MakeMode,Remark) values('" + ProcessID + "','" + NewAdOrderId + "','" + AdContent + "','" + MediaName + "','" + RoadSortName + "','" + VehicleTypeName + "','" + IssueDate + "','" + EndDate + "','" + VehicleNumbering + "','" + MakeState + "','" + SetDate + "','" + DesignType + "','" + MakeMode + "','" + Remark + "')";
                                }
                            }

                        }
                        else
                        {
                            sql = "Insert into tblAdFixinglist_Zi (ProcessId,AdOrderId,AdContent,MediaName,RoadSortName,VehicleTypeName,InsureIssueDate,InsureEndDate,VehicleNumbering,FixDate,MakeState,SetDate,DesignType,MakeMode,Remark) values('" + ProcessID + "','" + "D" + AdOrderId + "','" + AdContent + "','" + MediaName + "','" + RoadSortName + "','" + VehicleTypeName + "','" + IssueDate + "','" + EndDate + "','" + VehicleNumbering + "','" + FixDate + "','" + MakeState + "','" + SetDate + "','" + DesignType + "','" + MakeMode + "','" + Remark + "')";
                            if (string.IsNullOrEmpty(SetDate) || string.IsNullOrEmpty(FixDate))
                            {
                                if (string.IsNullOrEmpty(SetDate) && string.IsNullOrEmpty(FixDate))
                                {
                                    sql = "Insert into tblAdFixinglist_Zi (ProcessId,AdOrderId,AdContent,MediaName,RoadSortName,VehicleTypeName,InsureIssueDate,InsureEndDate,VehicleNumbering,MakeState,DesignType,MakeMode,Remark) values('" + ProcessID + "','" + "D" + AdOrderId + "','" + AdContent + "','" + MediaName + "','" + RoadSortName + "','" + VehicleTypeName + "','" + IssueDate + "','" + EndDate + "','" + VehicleNumbering + "','" + MakeState + "','" + DesignType + "','" + MakeMode + "','" + Remark + "')";
                                }
                                else if (string.IsNullOrEmpty(SetDate) && !string.IsNullOrEmpty(FixDate))
                                {
                                    sql = "Insert into tblAdFixinglist_Zi (ProcessId,AdOrderId,AdContent,MediaName,RoadSortName,VehicleTypeName,InsureIssueDate,InsureEndDate,VehicleNumbering,FixDate,MakeState,DesignType,MakeMode,Remark) values('" + ProcessID + "','" + "D" + AdOrderId + "','" + AdContent + "','" + MediaName + "','" + RoadSortName + "','" + VehicleTypeName + "','" + IssueDate + "','" + EndDate + "','" + VehicleNumbering + "','" + FixDate + "','" + MakeState + "','" + DesignType + "','" + MakeMode + "','" + Remark + "')";
                                }
                                if (string.IsNullOrEmpty(FixDate) && !string.IsNullOrEmpty(SetDate))
                                {
                                    sql = "Insert into tblAdFixinglist_Zi (ProcessId,AdOrderId,AdContent,MediaName,RoadSortName,VehicleTypeName,InsureIssueDate,InsureEndDate,VehicleNumbering,MakeState,SetDate,DesignType,MakeMode,Remark) values('" + ProcessID + "','" + "D" + AdOrderId + "','" + AdContent + "','" + MediaName + "','" + RoadSortName + "','" + VehicleTypeName + "','" + IssueDate + "','" + EndDate + "','" + VehicleNumbering + "','" + MakeState + "','" + SetDate + "','" + DesignType + "','" + MakeMode + "','" + Remark + "')";
                                }
                            }
                        }
                        sqlList.Add(sql);
                    }
                }
                sql = "select AdOrderId from tblAdOrderMediaList_Zi where AdOrderId = '" + NewAdOrderId + "'";
                reader = DBContext.PTMMHZ.GetReader(sql);
                if (!reader.Read())
                {
                    foreach (DataRow ds_row in dt_AdOrderList.Rows)
                    {
                        string AdContent = ds_row["AdContent"] == DBNull.Value ? "" : Convert.ToString(ds_row["AdContent"]);
                        string MediaName = ds_row["MediaName"] == DBNull.Value ? "" : Convert.ToString(ds_row["MediaName"]);
                        string RoadSortName = ds_row["RoadSortName"] == DBNull.Value ? "" : Convert.ToString(ds_row["RoadSortName"]);
                        string VehicleTypeName = ds_row["VehicleTypeName"] == DBNull.Value ? "" : Convert.ToString(ds_row["VehicleTypeName"]);
                        string MEIDANUM = ds_row["MEIDANUM"] == DBNull.Value ? "0" : Convert.ToString(ds_row["MEIDANUM"]);
                        string InsureIssueDate = ds_row["InsureIssueDate"] == DBNull.Value ? "" : Convert.ToString(ds_row["InsureIssueDate"]);
                        string InsureEndDate = ds_row["InsureEndDate"] == DBNull.Value ? "" : Convert.ToString(ds_row["InsureEndDate"]);
                        string MEDIAUNITPRICE = ds_row["MEDIAUNITPRICE"] == DBNull.Value ? "0" : Convert.ToString(ds_row["MEDIAUNITPRICE"]);
                        string FACTUREPRICE = ds_row["FACTUREPRICE"] == DBNull.Value ? "0" : Convert.ToString(ds_row["FACTUREPRICE"]);
                        string Remark = ds_row["Remark"] == DBNull.Value ? "" : Convert.ToString(ds_row["Remark"]);
                        if (Title .Contains("媒体制作"))
                        {
                            sql = "Insert into tblAdOrderMediaList_Zi (AdContent,MediaName,RoadSortName,VehicleTypeName,MEIDANUM,InsureIssueDate,InsureEndDate,MEDIAUNITPRICE,FACTUREPRICE,Remark,AdOrderId) values('" + AdContent + "','" + MediaName + "','" + RoadSortName + "','" + VehicleTypeName + "'," + MEIDANUM + ",'" + InsureIssueDate + "','" + InsureEndDate + "'," + MEDIAUNITPRICE + "," + FACTUREPRICE + ",'" + Remark + "','" + NewAdOrderId + "')";
                        }
                        else
                        {
                            sql = "Insert into tblAdOrderMediaList_Zi (AdContent,MediaName,RoadSortName,VehicleTypeName,MEIDANUM,InsureIssueDate,InsureEndDate,MEDIAUNITPRICE,FACTUREPRICE,Remark,AdOrderId) values('" + AdContent + "','" + MediaName + "','" + RoadSortName + "','" + VehicleTypeName + "'," + MEIDANUM + ",'" + InsureIssueDate + "','" + InsureEndDate + "'," + MEDIAUNITPRICE + "," + FACTUREPRICE + ",'" + Remark + "','" + "D" + AdOrderId + "')";
                        }
                        sqlList.Add(sql);
                    }
                }
            //}
            //else
            //{
                sql = "select * from tblOutdoorAdOrderMedia where AdOrderId = '" + AdOrderId + "'";
                DataTable dt_OutdoorList = DBContext.PTMMHZ.GetDataTable(sql);
                sql = "select AdOrderId from tblOutdoorAdOrderMedia_Zi where AdOrderId = '" + NewAdOrderId + "'";
                reader = DBContext.PTMMHZ.GetReader(sql);
                if (!reader.Read())
                {
                    foreach (DataRow ds_row in dt_OutdoorList.Rows)
                    {
                        string AdContent = ds_row["AdContent"] == DBNull.Value ? "" : Convert.ToString(ds_row["AdContent"]);
                        string OutDoorMediaName = ds_row["OutDoorMediaName"] == DBNull.Value ? "" : Convert.ToString(ds_row["OutDoorMediaName"]);
                        string Area = ds_row["Area"] == DBNull.Value ? "" : Convert.ToString(ds_row["Area"]);
                        string RoadName = ds_row["RoadName"] == DBNull.Value ? "" : Convert.ToString(ds_row["RoadName"]);
                        string PtPlatForm = ds_row["PtPlatForm"] == DBNull.Value ? "" : Convert.ToString(ds_row["PtPlatForm"]);
                        string Azimuth = ds_row["Azimuth"] == DBNull.Value ? "" : Convert.ToString(ds_row["Azimuth"]);
                        string OutDoorMediaNumbering = ds_row["OutDoorMediaNumbering"] == DBNull.Value ? "" : Convert.ToString(ds_row["OutDoorMediaNumbering"]);
                        string IssueDate = ds_row["IssueDate"] == DBNull.Value ? "" : Convert.ToString(ds_row["IssueDate"]);
                        string endDate = ds_row["endDate"] == DBNull.Value ? "" : Convert.ToString(ds_row["endDate"]);
                        string Facility = ds_row["Facility"] == DBNull.Value ? "0" : Convert.ToString(ds_row["Facility"]);
                        string Lighting = ds_row["Lighting"] == DBNull.Value ? "0" : (Convert.ToString(ds_row["Lighting"]) == "True" ? "1" : "0");
                        string MediaSize = ds_row["MediaSize"] == DBNull.Value ? "" : Convert.ToString(ds_row["MediaSize"]);
                        string MediaUnitPrice = ds_row["MediaUnitPrice"] == DBNull.Value ? "0" : Convert.ToString(ds_row["MediaUnitPrice"]);
                        string FacturePrice = ds_row["FacturePrice"] == DBNull.Value ? "0" : Convert.ToString(ds_row["FacturePrice"]);
                        string MakeState = ds_row["MakeState"] == DBNull.Value ? "" : Convert.ToString(ds_row["MakeState"]);
                        string ProcessID = ds_row["ProcessID"] == DBNull.Value ? "" : Convert.ToString(ds_row["ProcessID"]);
                        string StickersDate = ds_row["StickersDate"] == DBNull.Value ? "" : Convert.ToString(ds_row["StickersDate"]);
                        string MakeMode = ds_row["MakeMode"] == DBNull.Value ? "" : Convert.ToString(ds_row["MakeMode"]);
                        string PackageNum = ds_row["PackageNum"] == DBNull.Value ? "" : Convert.ToString(ds_row["PackageNum"]);
                        string Remark = ds_row["Remark"] == DBNull.Value ? "" : Convert.ToString(ds_row["Remark"]);
                        int contractFlag = ds_row["contractFlag"] == DBNull.Value ? 0 : Convert.ToInt32(ds_row["contractFlag"]);
                        if (Title.Contains("媒体制作"))
                        {
                            sql = "Insert into tblOutdoorAdOrderMedia_Zi (AdContent,OutDoorMediaName,Area,RoadName,PtPlatForm,Azimuth,OutDoorMediaNumbering,IssueDate,endDate,Facility,Lighting,MediaSize,MediaUnitPrice,FacturePrice,MakeState,ProcessID,MakeMode,PackageNum,Remark,contractFlag,AdOrderId) values('" + AdContent + "','" + OutDoorMediaName + "','" + Area + "','" + RoadName + "','" + PtPlatForm + "','" + Azimuth + "','" + OutDoorMediaNumbering + "','" + IssueDate + "','" + endDate + "'," + Facility + "," + Lighting + ",'" + MediaSize + "'," + MediaUnitPrice + "," + FacturePrice + ",'" + MakeState + "','" + ProcessID + "','" + MakeMode + "','" + PackageNum + "','" + Remark + "'," + contractFlag + ",'" + NewAdOrderId + "')";
                        }
                        else
                        {
                            sql = "Insert into tblOutdoorAdOrderMedia_Zi (AdContent,OutDoorMediaName,Area,RoadName,PtPlatForm,Azimuth,OutDoorMediaNumbering,IssueDate,endDate,Facility,Lighting,MediaSize,MediaUnitPrice,FacturePrice,MakeState,ProcessID,MakeMode,PackageNum,Remark,contractFlag,AdOrderId) values('" + AdContent + "','" + OutDoorMediaName + "','" + Area + "','" + RoadName + "','" + PtPlatForm + "','" + Azimuth + "','" + OutDoorMediaNumbering + "','" + IssueDate + "','" + endDate + "'," + Facility + "," + Lighting + ",'" + MediaSize + "'," + MediaUnitPrice + "," + FacturePrice + ",'" + MakeState + "','" + ProcessID + "','" + MakeMode + "','" + PackageNum + "','" + Remark + "'," + contractFlag + ",'" + "D" + AdOrderId + "')";
                        }
                        sqlList.Add(sql);
                    }
                }
            //}
            if (sqlList.Count > 0)
            {
                if (DBContext.PTMMHZ.ExecTrans(sqlList.ToArray()))
                {
                    msg = "添加明细成功";
                }
            }
            return msg;
        }
        /// <summary>
        /// 用来产生自备车制作合同订单号的方法(订单号)
        /// </summary>
        /// <returns></returns>
        public static string AutoGenerationOrderIDbOrd(string bOrd)
        {
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            string YearStr = Year.ToString();
            string MonthStr = Month.ToString();
            string AdOrderId = "";
            if (MonthStr.Length == 1)
            {
                MonthStr = "0" + MonthStr;
            }
            AdOrderId = bOrd + YearStr + MonthStr;
            string StrQuery = "SELECT TOP 1 AdOrderId FROM tblAdorder WHERE AdOrderId LIKE '" + bOrd + YearStr + MonthStr + "%' AND OldData<>1 ORDER BY AdOrderId DESC";
            DataTable dt = DBContext.PTMMHZ.GetDataTable(StrQuery);
            if (dt.Rows.Count > 0)
            {
                string DtAdOrderId = dt.Rows[0]["AdOrderId"].ToString();
                string MAdOrderId = DtAdOrderId.Remove(0, 7);
                int num = Int32.Parse(MAdOrderId);
                string Number = "";
                switch (MAdOrderId.Length)
                {

                    case 1:
                        num += 1;
                        if (num > 10)
                        {
                            Number = "00" + num.ToString();
                            if (Number.Length == 1)
                            {
                                Number = "00" + Number;
                            }
                            else if (Number.Length == 2)
                            {
                                Number = "0" + Number;
                            }
                        }
                        else
                        {
                            Number = "0" + num.ToString();
                            if (Number.Length == 1)
                            {
                                Number = "00" + Number;
                            }
                            else if (Number.Length == 2)
                            {
                                Number = "0" + Number;
                            }
                        }
                        break;
                    case 2:
                        num += 1;
                        if (num > 100)
                        {
                            Number = "0" + num.ToString();
                            if (Number.Length == 1)
                            {
                                Number = "00" + Number;
                            }
                            else if (Number.Length == 2)
                            {
                                Number = "0" + Number;
                            }
                        }
                        else
                        {
                            Number = num.ToString();
                            if (Number.Length == 1)
                            {
                                Number = "00" + Number;
                            }
                            else if (Number.Length == 2)
                            {
                                Number = "0" + Number;
                            }
                        }
                        break;

                    default:
                        num += 1;
                        Number = num.ToString();
                        if (Number.Length == 1)
                        {
                            Number = "00" + Number;
                        }
                        else if (Number.Length == 2)
                        {
                            Number = "0" + Number;
                        }
                        break;
                }
                AdOrderId += Number;
            }
            else
            {
                AdOrderId += "001";
            }

            return AdOrderId;
        }
        #region 媒体明细

        /// <summary>
        /// 根据订单单号获取媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblAdOrderMedialist_Zi> GettblAdOrderMedia_ZilistPageResult(string orderId, PageInfo page)
        {
            SubPageResult<tblAdOrderMedialist_Zi> result = new SubPageResult<tblAdOrderMedialist_Zi>();
            try
            {
                page.Where = " AdorderId='" + orderId + "'";
                result = CommonHelper.ListSubPageResult<tblAdOrderMedialist_Zi>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblAdOrderMedialistPageResult", ex);
            }
            return result;

        }

        #endregion
        #region 车位明细

        /// <summary>
        /// 根据订单单号获取媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblAdFixingList_Zi> GettblAdFixinglist_ZiPageResult(string orderId, PageInfo page)
        {
            SubPageResult<tblAdFixingList_Zi> result = new SubPageResult<tblAdFixingList_Zi>();
            try
            {
                page.Where = " AdorderId='" + orderId + "'";
                result = CommonHelper.ListSubPageResult<tblAdFixingList_Zi>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblAdOrderMedialistPageResult", ex);
            }
            return result;

        }

        #endregion
        #region 户外明细

        /// <summary>
        /// 根据订单单号获取媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblOutdoorAdOrderMedia_Zi> GettblOutdoorAdOrderMedia_ZiPageDate(string orderId, PageInfo page)
        {
            SubPageResult<tblOutdoorAdOrderMedia_Zi> result = new SubPageResult<tblOutdoorAdOrderMedia_Zi>();
            try
            {
                page.Where = " AdorderId='" + orderId + "'";
                result = CommonHelper.ListSubPageResult<tblOutdoorAdOrderMedia_Zi>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblOutdoorAdOrderMedia_ZiPageDate", ex);
            }
            return result;

        }

        #endregion
    }
     #endregion
}
