using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;
using Bitshare.PTMM.Model;
using System.Data.SqlClient;
using System.Collections;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Common;
namespace Bitshare.PTMM.Service.Helper
{
    /// <summary>
    /// 流程管理中具体执行方法的类
    /// </summary>
    public class ProcessDataHelp
    {
        /// <summary>
        /// 获取用户名是否为该流程的有效用户
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="LoginName">用户名称</param>
        /// <returns>true/false</returns>
        public static bool GetProcessIsUser(string ProcessName, string LoginName)
        {
            bool IsUser = false;

            #region 先判断当前用户是否包含在该流程中，未包含则只可查看。
            //获取流程所需的所有角色名称
            var vExecutorRole = BusinessContext.tblProcessManage.GetModelList("ProcessName='"+ProcessName+"'").Where(a => a.ProcessName == ProcessName).Select(a => a.ExecutorRole).Distinct().ToList();
            if (vExecutorRole != null && vExecutorRole.Count > 0)
            {
                ///获取当前用户所对应的角色id 
                var vRoleID = BusinessContext.tblUser_Roles.GetModelList("LoginName='" + LoginName + "'").Select(a => a.Role_Id).ToList();
                foreach (int s in vRoleID.ToList())
                {
                    foreach (string d in vExecutorRole.ToList())
                    {
                        var vTemp = BusinessContext.sys_role.GetModelList("role_name='" + d + "'").Where(p => p.role_name == d).Select(a => a.TblRcdId);
                        if (vTemp != null && vTemp.Count() > 0 && vTemp.ToList()[0] == s)
                        {
                            IsUser = true; break;
                        }
                    }
                    if (IsUser)
                        break;
                }
            }
            #endregion
            return IsUser;
        }
        /// <summary>
        /// 根据流程名称获取流程节点列表
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <returns></returns>
        public static List<string> listnodes(string ProcessName)
        {
            List<string> listnode = new List<string>();
            DataTable ProcessName_ds = DBContext.PTMMHZ.GetDataTable("select distinct nodeName,Nodenum from dbo.tblProcessManage where processName='" + ProcessName + "'  order by Nodenum");
            for (int i = 0; i < ProcessName_ds.Rows.Count; i++)
            {

                listnode.Add(Convert.ToString(ProcessName_ds.Rows[i]["nodeName"]));
            }
            return listnode;
        }
        #region 媒体确认
        /// <summary>
        /// 户外媒体检查
        /// </summary>
        /// <param name="AdOrderId"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string OutMediaCheck(string AdOrderId, string TableName)
        {
            DataTable OrderMeidaAll = DBContext.PTMMHZ.GetDataTable("select AdOrderID,OutDoorMediaName,RoadName,Azimuth,PtPlatForm,OutDoorMediaNumbering,IssueDate,endDate from View_OutDoorMediaList where AdOrderId<>'" + AdOrderId + "'");

            DataTable OrderMeida = DBContext.PTMMHZ.GetDataTable("select AdOrderID,Azimuth,OutDoorMediaName,RoadName,PtPlatForm,OutDoorMediaNumbering,IssueDate,endDate from tblOutdoorAdOrderMedia where AdOrderId='" + AdOrderId + "'");
            string mediaLog = "";
            foreach (DataRow Dr in OrderMeida.Rows)
            {
                string RoadName = Dr["RoadName"] == null ? "" : Dr["RoadName"].ToString().TrimEnd();
                string PtPlatForm = Dr["PtPlatForm"] == null ? "" : Dr["PtPlatForm"].ToString().TrimEnd();
                string Azimuth = Dr["Azimuth"] == null ? "" : Dr["Azimuth"].ToString().TrimEnd();
                string Tag_AdOrderId = Dr["AdOrderId"] == null ? "" : Dr["AdOrderId"].ToString().TrimEnd();
                string MediaName = Dr["OutDoorMediaName"] == null ? "" : Dr["OutDoorMediaName"].ToString().TrimEnd();
                string OutDoorMediaNumbering = Dr["OutDoorMediaNumbering"] == null ? "" : Dr["OutDoorMediaNumbering"].ToString().TrimEnd();
                DateTime DtBegin = DateTime.Parse(DateTime.Parse(Dr["IssueDate"].ToString()).ToShortDateString());
                DateTime DtEnd = DateTime.Parse(DateTime.Parse(Dr["endDate"].ToString()).ToShortDateString());

                string AdOrderids = "";
                DataRow[] DtRows = OrderMeidaAll.Select("RoadName='" + RoadName + "' AND PtPlatForm ='" + PtPlatForm + "' AND Azimuth='" + Azimuth + "' AND OutDoorMediaName='" + MediaName + "'AND OutDoorMediaNumbering='" + OutDoorMediaNumbering + "' and  endDate>='" + DateTime.Now.ToShortDateString() + "'  and AdOrderId<>'" + AdOrderId + "'");
                if (DtRows.Length > 0)
                {
                    foreach (DataRow Dt_Row in DtRows)
                    {
                        if (AdOrderids == AdOrderId)
                        {
                            AdOrderids = Convert.ToString(Dt_Row["AdOrderID"]);
                        }
                        else
                        {
                            AdOrderids += "," + Convert.ToString(Dt_Row["AdOrderID"]);
                        }
                    }
                    mediaLog = "路段:" + RoadName + " 站台:" + PtPlatForm + " 方位:" + Azimuth + " 媒体名称:" + MediaName + " 编号:" + OutDoorMediaNumbering + " 在定单" + AdOrderids + "中重复使用。";
                }
            }
            return mediaLog;
        }
        /// <summary>
        /// 自行车媒体检查
        /// </summary>
        /// <param name="AdOrderId"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string BicycleMediaCheck(string AdOrderId, string TableName)
        {
            DataTable OrderMeidaAll = DBContext.PTMMHZ.GetDataTable("select AdOrderID,OutDoorMediaName,RoadName,OutDoorMediaNumbering,IssueDate,endDate from  View_BicycleMadia where AdOrderId<>'" + AdOrderId + "'");
            DataTable OrderMeida = DBContext.PTMMHZ.GetDataTable("select AdOrderID,OutDoorMediaName,RoadName,OutDoorMediaNumbering,IssueDate,endDate from tblBicycleMadialist where AdOrderId='" + AdOrderId + "'");
            string mediaLog = "";
            foreach (DataRow Dr in OrderMeida.Rows)
            {
                string RoadName = Dr["RoadName"] == null ? "" : Dr["RoadName"].ToString().TrimEnd();

                string Tag_AdOrderId = Dr["AdOrderId"] == null ? "" : Dr["AdOrderId"].ToString().TrimEnd();
                string MediaName = Dr["OutDoorMediaName"] == null ? "" : Dr["OutDoorMediaName"].ToString().TrimEnd();
                string OutDoorMediaNumbering = Dr["OutDoorMediaNumbering"] == null ? "" : Dr["OutDoorMediaNumbering"].ToString().TrimEnd();
                DateTime DtBegin = DateTime.Parse(DateTime.Parse(Dr["IssueDate"].ToString()).ToShortDateString());
                DateTime DtEnd = DateTime.Parse(DateTime.Parse(Dr["endDate"].ToString()).ToShortDateString());

                string AdOrderids = "";
                DataRow[] DtRows = OrderMeidaAll.Select("RoadName='" + RoadName + "' AND OutDoorMediaName='" + MediaName + "'AND OutDoorMediaNumbering='" + OutDoorMediaNumbering + "' and  IssueDate<='" + DateTime.Now.ToShortDateString() + "' and endDate>='" + DateTime.Now.ToShortDateString() + "' and AdOrderId<>'" + AdOrderId + "'");
                if (DtRows.Count() > 1)
                {
                    foreach (DataRow Dt_Row in DtRows)
                    {
                        if (AdOrderids == AdOrderId)
                        {
                            AdOrderids = Convert.ToString(Dt_Row["AdOrderID"]);
                        }
                        else
                        {
                            AdOrderids += "," + Convert.ToString(Dt_Row["AdOrderID"]);
                        }
                    }
                    mediaLog = "路段:" + RoadName + "媒体名称:" + MediaName + " 编号:" + OutDoorMediaNumbering + " 在定单" + AdOrderids + "中重复使用。";
                }
            }
            return mediaLog;
        }
        /// <summary>
        /// 车位媒体检查
        /// </summary>
        /// <param name="AdOrderId"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string AdFixingListCheck(string AdOrderId)
        {
            DataTable OrderMeidaAll = DBContext.PTMMHZ.GetDataTable("select RoadSortName,AdOrderId,VehicleNumbering,MediaName,InsureIssueDate,InsureEndDate,MediaState from tblAdFixinglist where (MediaState=2 Or MediaState=3) and (datediff(d,getdate(),tblAdFixingList.InsureEndDate)>=0) and AdOrderId<>'" + AdOrderId + "'");
            DataTable OrderMeida = DBContext.PTMMHZ.GetDataTable("select AdOrderID,MediaName,RoadSortName,VehicleNumbering,InsureIssueDate,InsureEndDate from tblAdFixingList where AdOrderId='" + AdOrderId + "'");
            string mediaLog = "";
            foreach (DataRow Dr in OrderMeida.Rows)
            {
                string RoadName = Dr["RoadSortName"] == null ? "" : Dr["RoadSortName"].ToString().TrimEnd();

                string Tag_AdOrderId = Dr["AdOrderId"] == null ? "" : Dr["AdOrderId"].ToString().TrimEnd();
                string MediaName = Dr["MediaName"] == null ? "" : Dr["MediaName"].ToString().TrimEnd();
                string OutDoorMediaNumbering = Dr["VehicleNumbering"] == null ? "" : Dr["VehicleNumbering"].ToString().TrimEnd();
                DateTime DtBegin = DateTime.Parse(DateTime.Parse(Dr["InsureIssueDate"].ToString()).ToShortDateString());
                DateTime DtEnd = DateTime.Parse(DateTime.Parse(Dr["InsureEndDate"].ToString()).ToShortDateString());

                string AdOrderids = "";//RoadSortName  MediaName   VehicleNumbering  InsureIssueDate  InsureEndDate
                DataRow[] DtRows = OrderMeidaAll.Select(" MediaName='" + MediaName + "'AND VehicleNumbering='" + OutDoorMediaNumbering + "' and  InsureIssueDate<='" + DateTime.Now.ToShortDateString() + "' and InsureEndDate>='" + DateTime.Now.ToShortDateString() + "' and AdOrderId<>'" + AdOrderId + "'");
                if (DtRows.Count() > 1)
                {
                    foreach (DataRow Dt_Row in DtRows)
                    {
                        if (AdOrderids == AdOrderId)
                        {
                            AdOrderids = Convert.ToString(Dt_Row["AdOrderID"]);
                        }
                        else
                        {
                            AdOrderids += "," + Convert.ToString(Dt_Row["AdOrderID"]);
                        }
                    }
                    mediaLog = "媒体名称:" + MediaName + " 编号:" + OutDoorMediaNumbering + " 在定单" + AdOrderids + "中重复使用。";
                }
            }
            return mediaLog;
        }
        //车身生成车位，或者判断空位
        public static string MediaSure(string AdOrderId, string TableName,string NodeName,string  ProcessName)
        {
            try
            {
                string mediaLog = "";
                if (TableName == "tblBicycleMadialist")
                {
                    //自行车媒体检查
                    mediaLog = BicycleMediaCheck(AdOrderId, TableName);
                    if (mediaLog != "")
                    {
                        return mediaLog;
                    }
                }
                else
                {
                    if (TableName == "tblOutdoorAdOrderMedia")
                    {
                        //户外媒体检查
                        mediaLog = OutMediaCheck(AdOrderId, TableName);
                        if (mediaLog != "")
                        {
                            return mediaLog;
                        }
                    }
                    else
                    {
                        //检查车身媒体空位情况
                        ReturnMessageModel RM=StartVehicleMediaCheck(AdOrderId, NodeName, ProcessName);
                        if (RM.IsSuccess)
                        {
                            
                        }
                        else
                        {
                            mediaLog = RM.Message;
                            return mediaLog;
                        }

                    }
                }
                return "MeidaOk";
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("流程管理MediaSure出错：", ex);
                return "过程出错！";
            }
        }
        /// <summary>
        /// 车辆在发起预定的时候的判断
        /// </summary>
        /// <param name="AdOrderId"></param>
        /// <param name="NodeName"></param>
        /// <returns></returns>
        public static ReturnMessageModel StartVehicleMediaCheck(string AdOrderId, string NodeName, string ProcessName)//预定提交时 空位判断
        {
            ReturnMessageModel Msg = new ReturnMessageModel();

            Msg.IsSuccess = false;
            Msg.Message = "媒体检查通过！";
            try
            {
                Boolean IsAllow = true;
                string mediaLog = "";
                if (ProcessName=="媒体预定")
                {
                    DataTable OrderVehicle = DBContext.PTMMHZ.GetDataTable("select RoadSortName,VehicleTypeName,AdOrderId,MediaName,MeidaNum,InsureIssueDate,InsureEndDate from tblAdOrderMedialist where AdOrderId='" + AdOrderId + "'");
                    //获得所有被定单本身占用的车辆
                    DataTable GetVehicleAd = DBContext.PTMMHZ.GetDataTable("Select AF.AdOrderId,AF.RoadSortName,AF.VehicleTypeName,AF.MediaName,AF.InsureIssueDate,AF.InsureEndDate,A.AdOrderId from tblAdFixingList as AF,TblAdOrder AS A where A.AdOrderId=AF.AdOrderId and (A.ContractSureFlag=1 Or( A.ContractSureFlag=0 and A.EffectDate>=GETDATE())) and AF.InsureEndDate>=getdate()");
                    //获取所有车辆信息where Reserve<>1
                    DataTable GetVehicle = DBContext.PTMMHZ.GetDataTable("Select A.VehicleNumbering,A.RoadSortName,A.VehicleTypeName,tblBodyWorkMedia_List.VehicleMediaName from View_PtVehicleReg as A LEFT OUTER JOIN   dbo.tblBodyWorkMedia_List ON A.VehicleNumbering=dbo.tblBodyWorkMedia_List.VehicleNumberinge  WHERE     (A.VehicleNumbering IS NOT NULL) AND (dbo.tblBodyWorkMedia_List.VehicleMediaName IS NOT NULL) ");
                    int CwNum = 0;
                    foreach (DataRow Dr in OrderVehicle.Rows)
                    {
                        string roadsort = Dr["RoadSortName"] == DBNull.Value ? "" : Dr["RoadSortName"].ToString().TrimEnd();
                        string VehicleType = Dr["VehicleTypeName"] == DBNull.Value ? "" : Dr["VehicleTypeName"].ToString().TrimEnd();
                        string Tag_AdOrderId = Dr["AdOrderId"] == DBNull.Value ? "" : Dr["AdOrderId"].ToString().TrimEnd();
                        string MediaName = Dr["MediaName"] == DBNull.Value ? "" : Dr["MediaName"].ToString().TrimEnd();
                        int MediaNum = (int)Dr["MeidaNum"];
                        DateTime DtBegin = DateTime.Parse(DateTime.Parse(Dr["InsureIssueDate"].ToString()).ToShortDateString());
                        DateTime DtEnd = DateTime.Parse(DateTime.Parse(Dr["InsureEndDate"].ToString()).ToShortDateString());
                        int iNum = 0;

                        if (VehicleType == "-")
                        {
                            VehicleType = "";
                        }
                        string StrWhere = " 1=1 ";
                        //车尾张贴不考虑线路名称
                        if (MediaName == "车尾张贴")
                        {
                            CwNum += MediaNum;
                            MediaNum = CwNum;
                        }
                        else
                        {
                            StrWhere = "RoadSortName='" + roadsort + "'";
                        }
                        DataRow[] drrVehicleAd;
                        DataRow[] drrGetVehicle;
                        if (string.IsNullOrWhiteSpace(VehicleType))
                        {
                            drrVehicleAd = GetVehicleAd.Select(StrWhere+" and AdOrderId<>'" + AdOrderId + "' and InsureIssueDate<='" + DtEnd + "' and InsureEndDate>='" + DtBegin + "' and MediaName='" + MediaName + "'");
                            drrGetVehicle = GetVehicle.Select(StrWhere + " and VehicleMediaName='" + MediaName + "'");
                        }
                        else
                        {
                            drrVehicleAd = GetVehicleAd.Select(StrWhere+" and VehicleTypeName='" + VehicleType + "' and AdOrderId<>'" + AdOrderId + "' and InsureIssueDate<='" + DtEnd + "' and InsureEndDate>='" + DtBegin + "' and MediaName='" + MediaName + "'");
                            drrGetVehicle = GetVehicle.Select(StrWhere + " and VehicleMediaName='" + MediaName + "' and VehicleTypeName='" + VehicleType + "'");

                        }
                        if (drrGetVehicle != null)
                        {
                            iNum = drrGetVehicle.Count();
                            if (drrVehicleAd != null)
                            {
                                iNum = iNum - drrVehicleAd.Count();
                            }

                        }
                        if (iNum < MediaNum)
                        {
                            if (MediaName == "车尾张贴")
                            {
                                mediaLog += MediaName + ";差" + (MediaNum - iNum).ToString() + "辆. ";
                            }
                            else
                            {
                                mediaLog += MediaName + "-" + roadsort + "- " + VehicleType + ";差 " + (MediaNum - iNum).ToString() + "辆. ";
                            }
                            IsAllow = false;
                        }
                    }
                }
                Msg.IsSuccess = IsAllow;
                Msg.Message = mediaLog;
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("流程管理StartVehicleMediaCheck出错：", ex);
                Msg.IsSuccess = false;
                Msg.Message = "检查过程遇到错误！";
            }
            return Msg;
        }
        /// <summary>
        /// 根据车型备注分配车辆自编号
        /// </summary>
        /// <param name="AdOrderId"></param>
        /// <returns></returns>
        public static List<string> GetSqlListByNodeName(string AdOrderId)
        {
            //媒体预订明细
            string strSql = "select *from tblAdOrderMedialist where  AdOrderId='" + AdOrderId + "'";
            DataTable ds_AdOrderMedia = DBContext.PTMMHZ.GetDataTable(strSql);

            //互斥媒体
            strSql = "select Excluded,Opponent from tblExcludedMedia ";
            DataTable ds_ExcludedMedia = DBContext.PTMMHZ.GetDataTable(strSql);
            List<string> sqlList = new List<string>();
            strSql = "delete from tblAdFixingList  where AdOrderId='" + AdOrderId + "'";
            sqlList.Add(strSql);
            try
            {
                //暂存数据表
                DataTable VehicleNumberingds = new DataTable();
                VehicleNumberingds.Columns.Add("RoadSortName", Type.GetType("System.String"));//线路
                VehicleNumberingds.Columns.Add("VehicleTypeName", Type.GetType("System.String"));//车型
                VehicleNumberingds.Columns.Add("MediaName", Type.GetType("System.String")); //媒体
                VehicleNumberingds.Columns.Add("VehicleNumbering", Type.GetType("System.String")); //车辆自编号
                VehicleNumberingds.Columns.Add("InsureIssueDate", typeof(DateTime)); //开始日期
                VehicleNumberingds.Columns.Add("InsureEndDate", typeof(DateTime));//结束日期
                //车辆数据
                strSql = "SELECT  A.VehicleNumbering,A.RoadSortName,A.VehicleTypeName,A.VehiclePark,A.Mediasize,A.RoadClass,tblBodyWorkMedia_List.VehicleMediaName,a.Tblrcdid from View_PtVehicleReg as A LEFT OUTER JOIN   dbo.tblBodyWorkMedia_List ON A.VehicleNumbering=dbo.tblBodyWorkMedia_List.VehicleNumberinge  WHERE     (A.VehicleNumbering IS NOT NULL) AND (dbo.tblBodyWorkMedia_List.VehicleMediaName IS NOT NULL) ";
                DataTable ds_PtVehicleReg = DBContext.PTMMHZ.GetDataTable(strSql);
                if (ds_AdOrderMedia != null && ds_AdOrderMedia.Rows.Count > 0)
                {
                    foreach (DataRow ds_row in ds_AdOrderMedia.Rows)
                    {
                        string AdContent = Convert.ToString(ds_row["AdContent"]);
                        string RoadSortName = Convert.ToString(ds_row["RoadSortName"]);
                        string VehicleId = Convert.ToString(ds_row["Tblrcdid"]);
                        string FormatAss = Convert.ToString(ds_row["FormatAss"]);
                        string VehicleTypeName = Convert.ToString(ds_row["VehicleTypeName"]);
                        string MediaName = Convert.ToString(ds_row["MediaName"]);
                        string RoadClass = Convert.ToString(ds_row["RoadClass"]);
                        string InsureIssueDate = Convert.ToString(ds_row["InsureIssueDate"]);
                        string InsureEndDate = Convert.ToString(ds_row["InsureEndDate"]);
                        int MeidaNum = Convert.ToInt32(ds_row["MeidaNum"]);
                        int BChange = Convert.ToInt32(ds_row["BChange"]);
                        string Remark = Convert.ToString(ds_row["Remark"]);
                        int CwNum = 0;
                        if (MediaName == "车尾张贴")
                        {
                            CwNum += MeidaNum;
                            MeidaNum = CwNum;
                        }
                        //得到互斥媒体
                        List<string> MediaNameList = new List<string>();
                        MediaNameList.Add(MediaName);
                        DataRow[] Opponent_row = ds_ExcludedMedia.Select("Excluded='" + MediaName + "'");
                        for (int i = 0; i < Opponent_row.Length; i++)
                        {
                            if (!MediaNameList.Contains(Opponent_row[i]["Opponent"]))
                            {
                                MediaNameList.Add(Convert.ToString(Opponent_row[i]["Opponent"]));
                            }
                        }
                        //string sWhere = "";
                        for (int k = 0; k < MeidaNum; k++)
                        {
                            //if (!string.IsNullOrWhiteSpace(VehicleTypeName))
                            //{
                            //    sWhere += " and tblPtVehicleReg.VehicleTypeName='" + VehicleTypeName + "' ";
                            //}
                            if (MediaName == "车尾张贴")
                            {
                                strSql = " select VehicleTypeName,count(VehicleTypeName) as VehicleTypeNameNum from  (select C.VehicleNumbering,tblBodyWorkMedia_List.VehicleMediaName,C.RoadSortName, C.VehicleTypeName  from View_PtVehicleReg AS C  LEFT OUTER JOIN   dbo.tblBodyWorkMedia_List ON C.VehicleNumbering = dbo.tblBodyWorkMedia_List.VehicleNumberinge where C.VehicleNumbering not in (select DISTINCT A.VehicleNumbering from dbo.tblAdFixingList as A where  A.MediaState in(2,3) and A.VehicleNumbering is not null  and A.MediaName in " + DBContext.PTMMHZ.AssemblyInCondition(MediaNameList)
                                + " and A.InsureIssueDate<='" + InsureEndDate + "' and A.InsureEndDate>='" + InsureIssueDate + "' ) "
                                + " and VehicleMediaName='" + MediaName + "') as PtVehicleRegds"
                                + " GROUP BY VehicleTypeName order by VehicleTypeNameNum desc";
                            }
                            else
                            {
                                strSql = " select VehicleTypeName,count(VehicleTypeName) as VehicleTypeNameNum from  (select C.VehicleNumbering,tblBodyWorkMedia_List.VehicleMediaName,C.RoadSortName, C.VehicleTypeName  from View_PtVehicleReg AS C  LEFT OUTER JOIN   dbo.tblBodyWorkMedia_List ON C.VehicleNumbering = dbo.tblBodyWorkMedia_List.VehicleNumberinge where C.VehicleNumbering not in (select DISTINCT A.VehicleNumbering from dbo.tblAdFixingList as A where A.MediaState in(2,3) and A.VehicleNumbering is not null  and A.RoadSortName='" + RoadSortName + "' and A.MediaName in " + DBContext.PTMMHZ.AssemblyInCondition(MediaNameList)
                                  + " and A.InsureIssueDate<='" + InsureEndDate + "' and A.InsureEndDate>='" + InsureIssueDate + "' ) "
                                  + " and RoadSortName='" + RoadSortName + "' and VehicleMediaName='" + MediaName + "') as PtVehicleRegds"
                                  + " GROUP BY VehicleTypeName order by VehicleTypeNameNum desc";
                            }
                            DataTable ds_VehicleTypeName = DBContext.PTMMHZ.GetDataTable(strSql);

                            for (int j = 0; j < ds_VehicleTypeName.Rows.Count; j++)
                            {
                                VehicleTypeName = Convert.ToString(ds_VehicleTypeName.Rows[j]["VehicleTypeName"]);
                                int VehicleTypeNameNum = Convert.ToInt32(ds_VehicleTypeName.Rows[j]["VehicleTypeNameNum"]);
                                DataRow[] PtVehicleReg_row = null;
                                if (MediaName == "车尾张贴")
                                {
                                    PtVehicleReg_row = ds_PtVehicleReg.Select("VehicleTypeName='" + VehicleTypeName + "' and VehicleMediaName='" + MediaName + "'");
                                }
                                else
                                {
                                    PtVehicleReg_row = ds_PtVehicleReg.Select("VehicleTypeName='" + VehicleTypeName + "' and VehicleMediaName='" + MediaName + "' and RoadSortName='" + RoadSortName + "'");
                                }
                                for (int a = 0; a < PtVehicleReg_row.Length; a++)
                                {
                                    string VehicleNumbering = Convert.ToString(PtVehicleReg_row[a]["VehicleNumbering"]);
                                    string VehiclePark = Convert.ToString(PtVehicleReg_row[a]["VehiclePark"]);
                                    string Mediasize = Convert.ToString(PtVehicleReg_row[a]["Mediasize"]);
                                    DataRow[] NotVehicleNumbering = VehicleNumberingds.Select("VehicleNumbering='" + VehicleNumbering + "' and  InsureIssueDate<='" + InsureEndDate + "' and InsureEndDate>='" + InsureIssueDate + "' and RoadSortName='" + RoadSortName + "'  and MediaName in " + DBContext.PTMMHZ.AssemblyInCondition(MediaNameList));
                                    if (NotVehicleNumbering.Length == 0)
                                    {
                                        DataRow RowTotal = VehicleNumberingds.Rows.Add();
                                        RowTotal["RoadSortName"] = RoadSortName;
                                        RowTotal["VehicleTypeName"] = VehicleTypeName;
                                        RowTotal["MediaName"] = MediaName;
                                        RowTotal["VehicleNumbering"] = VehicleNumbering;
                                        RowTotal["InsureIssueDate"] = InsureIssueDate;
                                        RowTotal["InsureEndDate"] = InsureEndDate;
                                        strSql = "INSERT INTO tblAdFixingList (AdOrderId,AdContent,RoadSortName,FormatAss,VehicleTypeName,MediaName,InsureIssueDate,InsureEndDate,MeidaNum,FixSure,BChange,Remark,VehicleNumbering,RoadClass,VehiclePark,Mediasize,MediaState,VehicleId)"
                                                 + " values ('" + AdOrderId + "','" + AdContent + "','" + RoadSortName + "','" + FormatAss + "','" + VehicleTypeName + "','" + MediaName + "','" + InsureIssueDate + "','" + InsureEndDate + "',1,0," + BChange + ",'" + Remark + "','" + VehicleNumbering + "','" + RoadClass + "','" + VehiclePark + "','" + Mediasize + "',2," + VehicleId + ")";

                                        sqlList.Add(strSql);
                                        j = ds_VehicleTypeName.Rows.Count;
                                        a = PtVehicleReg_row.Length;
                                    }


                                    if (MeidaNum < 1)
                                    {
                                        a = PtVehicleReg_row.Length;
                                    }

                                }
                                if (MeidaNum < 1)
                                {
                                    j = ds_VehicleTypeName.Rows.Count;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            { 

            }
            return sqlList;
        }
        #endregion
        #region  制作确认
        public static ReturnMessageModel CheckMakingByFun(string ProcessId, string ProcessName, string NodeName)
        {
            ReturnMessageModel RM = new ReturnMessageModel();
            try
            {
                string TableName="";
                string Sql = "";
                DataTable dtInfo = new DataTable();
                if(ProcessId.Contains("VPG"))
                {
                    TableName="tblAdFixingList_Making";
                    Sql = "select RoadSortName,MediaName,VehicleNumbering from " + TableName + " where ProcessId='" + ProcessId + "'";
                    dtInfo = DBContext.PTMMHZ.GetDataTable(Sql);
                }
                else if (ProcessId.Contains("OPG"))
                {
                    TableName = "tblOutdoorAdOrderMedia_Making";
                    Sql = "select RoadName,MediaName,OutDoorMediaNumbering,PlatFormName  from " + TableName + " where ProcessId='" + ProcessId + "'";
                    dtInfo = DBContext.PTMMHZ.GetDataTable(Sql);
                }
                else if (ProcessId.Contains("BPG"))
                {
                    TableName = "tblBicycleMadialist_Making";
                    Sql = "select MediaName,RoadName,OutDoorMediaNumbering,ServiceName from " + TableName + " where ProcessId='" + ProcessId + "'";
                    dtInfo = DBContext.PTMMHZ.GetDataTable(Sql);
                }
                else if (ProcessId.Contains("TPG"))
                {
                    Sql = Sql = "select RoadSortName,MediaName,VehicleNumbering from tblAdFixingList_Making  where ProcessId='" + ProcessId + "'";
                    dtInfo = DBContext.PTMMHZ.GetDataTable(Sql);
                    if (dtInfo == null || dtInfo.Rows.Count == 0)
                    {
                        Sql = "select RoadName,MediaName,OutDoorMediaNumbering,PlatFormName  from tblOutdoorAdOrderMedia_Making  where ProcessId='" + ProcessId + "'";
                        dtInfo = DBContext.PTMMHZ.GetDataTable(Sql);
                        if (dtInfo == null || dtInfo.Rows.Count == 0)
                        {
                            Sql = "select MediaName,RoadName,OutDoorMediaNumbering,ServiceName from tblBicycleMadialist_Making where ProcessId='" + ProcessId + "'";
                            dtInfo = DBContext.PTMMHZ.GetDataTable(Sql);
                        }
                    }
                    
                }
                if (dtInfo == null || dtInfo.Rows.Count == 0)
                {
                    RM.IsSuccess = false;
                    RM.Message = "没有明细，清先添加明细！";
                    return RM;
                }
                else
                {
                    RM.IsSuccess = true;
                    RM.Message = "";
                }
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("流程管理CheckMakingByFun出错：", ex);
                RM.IsSuccess = false;
                RM.Message = "检查过程遇到错误！";
            }
            return RM;
        }
        #endregion
        #region 车身空位判断（制作）
        /// <summary>
        /// 车辆在车身制作发稿时候的空位检查
        /// </summary>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        public static ReturnMessageModel VehicleMediaCheckByMake(string ProcessId, string AdOrderId,  List<tblAdFixingList_Making> listFixing)
        {
            ReturnMessageModel RM = new ReturnMessageModel();
            try
            {

                string mediaLog = "";
                Boolean IsAllow = true;
                List<string> MediaNameList = new List<string>();
                string strSql = "select Excluded,Opponent from tblExcludedMedia ";
                DataTable ds_ExcludedMedia = DBContext.PTMMHZ.GetDataTable(strSql);
                DateTime now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                List<tblAdFixingList> OrderMeidaCanUse = BusinessContext.tblAdFixingList.GetModelList(" MediaState in (2,3) and AdOrderId<>'" + AdOrderId + "'").Where(p => p.AdOrderId != (AdOrderId) && (p.MediaState == 2 || p.MediaState == 3) && p.InsureEndDate >= now).ToList();
                foreach (tblAdFixingList_Making AdOrderFixing in listFixing)
                {
                    string MediaName = AdOrderFixing.MediaName;
                    string VehicleNumbering = AdOrderFixing.VehicleNumbering;
                    DateTime DtBegin = DateTime.Parse(AdOrderFixing.IssueDate.ToString());
                    DateTime DtEnd = DateTime.Parse(AdOrderFixing.EndDate.ToString());
                    List<string> BookOrders = new List<string>();//由于预定而重复的结果 
                    List<string> ContractOrders = new List<string>();//在发布儿照成的冲突 
                    DataRow[] Opponent_row = ds_ExcludedMedia.Select("Excluded='" + MediaName + "'");
                    for (int i = 0; i < Opponent_row.Length; i++)
                    {
                        if (!MediaNameList.Contains(Opponent_row[i]["Opponent"]))
                        {
                            MediaNameList.Add(Convert.ToString(Opponent_row[i]["Opponent"]));
                        }
                    }
                    //MdoPublic.MediaClnModel 
                    string[] ExclusionMedia = MediaNameList.ToArray();


                    List<tblAdFixingList> DtRowsUse = OrderMeidaCanUse.Where(p => p.VehicleNumbering == VehicleNumbering && p.InsureIssueDate <= DtEnd && p.InsureEndDate >= DtBegin && ExclusionMedia.Contains(p.MediaName)).ToList();

                    if (DtRowsUse.Count() > 0)
                    {
                        foreach (tblAdFixingList DtRowsUselist in DtRowsUse)
                        {
                            string AllAdOrderId = DtRowsUselist.AdOrderId;
                            int MediaState = Convert.ToInt32(DtRowsUselist.MediaState);
                            //添加由于预定导致重复的
                            if (MediaState == 2)
                            {

                                if (!BookOrders.Contains(AllAdOrderId))
                                {
                                    BookOrders.Add(AllAdOrderId);
                                }
                            }
                            else if (MediaState == 3)
                            {
                                if (!ContractOrders.Contains(AllAdOrderId))
                                {
                                    ContractOrders.Add(AllAdOrderId);
                                }
                            }
                        }
                    }
                    if (BookOrders.Count > 0)
                    {
                        IsAllow = false;
                        string AdOrders = "";
                        foreach (string OrderId in BookOrders)
                        {
                            if (string.IsNullOrWhiteSpace(AdOrders))
                            {
                                AdOrders += OrderId;
                            }
                            else
                            {
                                AdOrders += "," + OrderId;
                            }
                        }
                        mediaLog += "车辆自编号:" + VehicleNumbering + " 媒体名称:" + MediaName + " 在定单" + AdOrders + "中重复预定。";
                    }
                    if (ContractOrders.Count > 0)
                    {
                        // 不允许通过的条件
                        IsAllow = false;
                        string AdOrders = "";
                        foreach (string OrderId in ContractOrders)
                        {
                            if (string.IsNullOrWhiteSpace(AdOrders))
                            {
                                AdOrders += OrderId;
                            }
                            else
                            {
                                AdOrders += "," + OrderId;
                            }
                        }
                        mediaLog += "车辆自编号:" + VehicleNumbering + " 媒体名称:" + MediaName + " 在合同" + AdOrders + "中重复预定。";
                    }
                }
                RM.IsSuccess = IsAllow;
                RM.Message = mediaLog;

            }
            catch (Exception ex)
            {
                RM.IsSuccess = false;
                RM.Message = "检查过程遇到错误！";
                Bitshare.PTMM.Common.LogManager.Error("VehicleMediaCheckByMake", ex);
            }
            return RM;
        }
        #endregion

        #region  户外媒体检查
        /// <summary>
        /// 户外媒体检查
        /// </summary>
        /// <param name="AdOrderId"></param>
        /// <returns></returns>
        public static ReturnMessageModel OutMediaCheck(string AdOrderId, List<tblOutdoorAdOrderMedia_Making> OutdoorMediaList)
        {
            ReturnMessageModel RM = new ReturnMessageModel();

            try
            {
                string mediaLog = "";

                DateTime now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                List<tblOutdoorAdOrderMedia> OrderMeidaCanUse = BusinessContext.tblOutdoorAdOrderMedia.GetModelList("AdOrderId<>'" + AdOrderId + "' and MediaState in (2,3)").Where(p => p.AdOrderId != (AdOrderId) && (p.MediaState == 2 || p.MediaState == 3) && p.endDate >= now).ToList(); 
                Boolean IsAllow = true;

                foreach (tblOutdoorAdOrderMedia_Making OutdoorMedia in OutdoorMediaList)
                {

                    string MediaName = OutdoorMedia.MediaName;
                    string PlatFormName = OutdoorMedia.PlatFormName;
                    string RoadName = OutdoorMedia.RoadName;
                    string MediaNumber = OutdoorMedia.OutDoorMediaNumbering;
                    DateTime DtBegin = Convert.ToDateTime(OutdoorMedia.IssueDate);
                    DateTime DtEnd = Convert.ToDateTime(OutdoorMedia.EndDate);

                    List<string> BookOrders = new List<string>();//由于预定而重复的结果 
                    List<string> ContractOrders = new List<string>();//在发布儿照成的冲突 
                    //查出不等于当前订单并且有冲突的订单号

                    List<tblOutdoorAdOrderMedia> DtRowsUse = OrderMeidaCanUse.Where(p => p.OutDoorMediaNumbering == MediaNumber && p.IssueDate <= DtEnd && p.endDate >= DtBegin).ToList();


                    if (DtRowsUse.Count() > 0)
                    {
                        foreach (tblOutdoorAdOrderMedia DtRowsUselist in DtRowsUse)
                        {

                            string AllAdOrderId = DtRowsUselist.AdOrderId;
                            int MediaState = Convert.ToInt32(DtRowsUselist.MediaState);
                            //添加由于预定导致重复的
                            if (MediaState == 2)
                            {
                                if (!BookOrders.Contains(AllAdOrderId))
                                {
                                    BookOrders.Add(AllAdOrderId);
                                }

                            }
                            //合同重复
                            else if (MediaState == 3)
                            {
                                if (!ContractOrders.Contains(AllAdOrderId))
                                {
                                    ContractOrders.Add(AllAdOrderId);
                                }
                            }
                        }
                    }

                    if (BookOrders.Count > 0)
                    {
                        IsAllow = false;
                        string AdOrders = "";
                        foreach (string OrderId in BookOrders)
                        {
                            if (string.IsNullOrWhiteSpace(AdOrders))
                            {
                                AdOrders += OrderId;
                            }
                            else
                            {
                                AdOrders += "," + OrderId;
                            }
                        }

                        mediaLog += " 媒体名称:" + MediaName + " 线路:" + RoadName + " 编号:" + MediaNumber + " 在定单" + AdOrders + "中重复预定。";
                    }
                    if (ContractOrders.Count > 0)
                    {
                        // 不允许通过的条件
                        IsAllow = false;
                        string AdOrders = "";
                        foreach (string OrderId in ContractOrders)
                        {
                            if (string.IsNullOrWhiteSpace(AdOrders))
                            {
                                AdOrders += OrderId;
                            }
                            else
                            {
                                AdOrders += "," + OrderId;
                            }
                        }
                        mediaLog += " 媒体名称:" + MediaName + " 线路:" + RoadName + " 编号:" + MediaNumber + " 在合同" + AdOrders + "中重复。";
                    }
                }
                RM.IsSuccess = IsAllow;
                RM.Message = mediaLog;

            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("OutMediaCheck", ex);
                RM.IsSuccess = false;
                RM.Message = "检查过程遇到错误！";
            }
            return RM;
        }
        #endregion

        #region  自行车媒体检查
        /// <summary>
        /// 自行车媒体检查
        /// </summary>
        /// <param name="AdOrderId"></param>
        /// <returns></returns>
        public static ReturnMessageModel BicycleMediaCheck(string AdOrderId,  List<tblBicycleMadialist_Making> BicycleMediaList)
        {
            ReturnMessageModel RM = new ReturnMessageModel();

            try
            {
                string mediaLog = "";

                DateTime now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                List<tblBicycleMadialist> BicycleidaCanUse = BusinessContext.tblBicycleMadialist.GetModelList("AdOrderId<>'" + AdOrderId + "' and MediaState in (2,3)").Where(p => p.AdOrderID != (AdOrderId) && (p.MediaState == 2 || p.MediaState == 3) && p.endDate >= now).ToList();
                Boolean IsAllow = true;

                foreach (tblBicycleMadialist_Making OutdoorMedia in BicycleMediaList)
                {

                    string MediaName = OutdoorMedia.MediaName;
                    string ServiceName = OutdoorMedia.ServiceName;
                    string RoadName = OutdoorMedia.RoadName;
                    string MediaNumber = OutdoorMedia.OutDoorMediaNumbering;
                    DateTime DtBegin = Convert.ToDateTime(OutdoorMedia.IssueDate);
                    DateTime DtEnd = Convert.ToDateTime(OutdoorMedia.EndDate);

                    List<string> BookOrders = new List<string>();//由于预定而重复的结果 
                    List<string> ContractOrders = new List<string>();//在发布儿照成的冲突 
                    //查出不等于当前订单并且有冲突的订单号

                    List<tblBicycleMadialist> DtRowsUse = BicycleidaCanUse.Where(p => p.OutDoorMediaNumbering == MediaNumber && p.IssueDate <= DtEnd && p.endDate >= DtBegin).ToList();


                    if (DtRowsUse.Count() > 0)
                    {
                        foreach (tblBicycleMadialist DtRowsUselist in DtRowsUse)
                        {

                            string AllAdOrderId = DtRowsUselist.AdOrderID;
                            int MediaState = Convert.ToInt32(DtRowsUselist.MediaState);
                            //添加由于预定导致重复的
                            if (MediaState == 2)
                            {
                                if (!BookOrders.Contains(AllAdOrderId))
                                {
                                    BookOrders.Add(AllAdOrderId);
                                }

                            }
                            //合同重复
                            else if (MediaState == 3)
                            {
                                if (!ContractOrders.Contains(AllAdOrderId))
                                {
                                    ContractOrders.Add(AllAdOrderId);
                                }
                            }
                        }
                    }

                    if (BookOrders.Count > 0)
                    {
                        IsAllow = false;
                        string AdOrders = "";
                        foreach (string OrderId in BookOrders)
                        {
                            if (string.IsNullOrWhiteSpace(AdOrders))
                            {
                                AdOrders += OrderId;
                            }
                            else
                            {
                                AdOrders += "," + OrderId;
                            }
                        }

                        mediaLog += " 媒体名称:" + MediaName + " 线路:" + RoadName + " 编号:" + MediaNumber + " 在定单" + AdOrders + "中重复预定。";
                    }
                    if (ContractOrders.Count > 0)
                    {
                        // 不允许通过的条件
                        IsAllow = false;
                        string AdOrders = "";
                        foreach (string OrderId in ContractOrders)
                        {
                            if (string.IsNullOrWhiteSpace(AdOrders))
                            {
                                AdOrders += OrderId;
                            }
                            else
                            {
                                AdOrders += "," + OrderId;
                            }
                        }
                        mediaLog += " 媒体名称:" + MediaName + " 线路:" + RoadName + " 编号:" + MediaNumber + " 在合同" + AdOrders + "中重复。";
                    }
                }
                RM.IsSuccess = IsAllow;
                RM.Message = mediaLog;

            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("OutMediaCheck", ex);
                RM.IsSuccess = false;
                RM.Message = "检查过程遇到错误！";
            }
            return RM;
        }
        #endregion

        #region 车身发布确认检查是否与订单媒体数据相同
        //车身检查
        public static ReturnMessageModel VehicleMediaMaking(string ProcessId, string AdOrderId)
        {
            ReturnMessageModel RM = new ReturnMessageModel();
            try
            {

                List<string> sqlList = new List<string>();
                Dictionary<string, string> SellerMsg = new Dictionary<string, string>();
                string mediaLog = "";
                RM.IsSuccess = true;
                RM.Message = "媒体检查通过";
                //MediaState(加入媒体状态 来判断 0代表录入，1 代表提交，2 代表媒体确认  3 正式合同 
                //得到所有的在发布或者预定状态未到期的媒体
                DataTable OrderMeidaAll = DBContext.PTMMHZ.GetDataTable("Select AdOrderId,VehicleNumbering,MediaName,InsureIssueDate,InsureEndDate,MediaState from tblAdFixinglist where  AdOrderId='" + AdOrderId + "'");
                //得到当前订单的媒体明细
                DataTable OrderMeida = DBContext.PTMMHZ.GetDataTable("select AdOrderId,VehicleNumbering,MediaName,IssueDate,EndDate from tblAdFixingList_Making where ProcessId='" + ProcessId + "'");
                Boolean IsAllow = true;

                foreach (DataRow Dr in OrderMeida.Rows)
                {
                    string MediaName = Dr["MediaName"] == null ? "" : Dr["MediaName"].ToString().TrimEnd();
                    string VehicleNumbering = Dr["VehicleNumbering"] == null ? "" : Dr["VehicleNumbering"].ToString().TrimEnd();
                    DateTime DtBegin = DateTime.Parse(DateTime.Parse(Dr["IssueDate"].ToString()).ToShortDateString());
                    DateTime DtEnd = DateTime.Parse(DateTime.Parse(Dr["EndDate"].ToString()).ToShortDateString());
                    List<string> BookOrders = new List<string>();//由于预定而重复的结果 
                    List<string> ContractOrders = new List<string>();//在发布儿照成的冲突 
                    string strCdt = " And MediaName='" + MediaName + "' AND AdOrderId='" + AdOrderId + "'";
                    DataRow[] DtRows = OrderMeidaAll.Select(" VehicleNumbering='" + VehicleNumbering + "'" + strCdt);
                    if (DtRows.Count() == 0)
                    {
                        string AdMsgContent = "车辆自编号:" + VehicleNumbering + " 媒体名称:" + MediaName + ",";
                        mediaLog += AdMsgContent;
                    }
                }
                if (!string.IsNullOrWhiteSpace(mediaLog))
                {
                    IsAllow = false;
                    mediaLog += "在订单中数据不匹配";
                }
                RM.IsSuccess = IsAllow;
                RM.Message = mediaLog;
            }
            catch (Exception ex)
            {
                RM.IsSuccess = false;
                RM.Message = "检查过程遇到错误！";
                Bitshare.PTMM.Common.LogManager.Error("WebPublicCode-VehicleMediaMaking", ex);
                throw;
            }
            return RM;
        }
        #endregion

        #region 户外发布确认检查是否与订单媒体数据相同
        //户外检查
        public static ReturnMessageModel OutMediaMaking(string ProcessId, string AdOrderId)
        {
            ReturnMessageModel RM = new ReturnMessageModel();
            try
            {
                List<string> sqlList = new List<string>();
                Dictionary<string, string> SellerMsg = new Dictionary<string, string>();
                string mediaLog = "";
                RM.IsSuccess = true;
                RM.Message = "媒体检查通过";

                DataTable OrderMeidaAll = DBContext.PTMMHZ.GetDataTable("select AdOrderId,RoadName,OutDoorMediaName,OutDoorMediaNumbering,IssueDate,EndDate,MediaState from tblOutdoorAdOrderMedia where  AdOrderId='" + AdOrderId + "'");
                //得到当前订单的媒体明细
                DataTable OrderMeida = DBContext.PTMMHZ.GetDataTable("select AdOrderId,RoadName,MediaName,OutDoorMediaNumbering,IssueDate,EndDate from dbo.tblOutdoorAdOrderMedia_Making where ProcessId='" + ProcessId + "'");
                Boolean IsAllow = true;
                foreach (DataRow Dr in OrderMeida.Rows)
                {
                    string MediaName = Dr["MediaName"] == null ? "" : Dr["MediaName"].ToString().TrimEnd();
                    string RoadName = Dr["RoadName"] == null ? "" : Dr["RoadName"].ToString().TrimEnd();
                    string OutDoorMediaNumbering = Dr["OutDoorMediaNumbering"] == null ? "" : Dr["OutDoorMediaNumbering"].ToString().TrimEnd();
                    DateTime DtBegin = DateTime.Parse(DateTime.Parse(Dr["IssueDate"].ToString()).ToShortDateString());
                    DateTime DtEnd = DateTime.Parse(DateTime.Parse(Dr["EndDate"].ToString()).ToShortDateString());
                    DataRow[] DtRows = OrderMeidaAll.Select(" OutDoorMediaNumbering='" + OutDoorMediaNumbering + "' and AdOrderId='" + AdOrderId + "'");
                    if (DtRows.Count() == 0)
                    {
                        string AdMsgContent = " 媒体名称:" + MediaName + " 线路名称:" + RoadName + " 媒体编号:" + OutDoorMediaNumbering + ",";
                        mediaLog += AdMsgContent;
                    }
                }
                if (!string.IsNullOrWhiteSpace(mediaLog))
                {
                    IsAllow = false;
                    mediaLog += "在订单中数据不匹配";
                }
                RM.IsSuccess = IsAllow;
                RM.Message = mediaLog;
            }
            catch (Exception ex)
            {
                RM.IsSuccess = false;
                RM.Message = "检查过程遇到错误！";
                Bitshare.PTMM.Common.LogManager.Error("WebPublicCode-OutMediaMaking", ex);
            }
            return RM;
        }
        #endregion

        #region 自行车发布确认检查是否与订单媒体数据相同
        //户外检查
        public static ReturnMessageModel BicycleMediaMaking(string ProcessId, string AdOrderId)
        {
            ReturnMessageModel RM = new ReturnMessageModel();
            try
            {
                List<string> sqlList = new List<string>();
                Dictionary<string, string> SellerMsg = new Dictionary<string, string>();
                string mediaLog = "";
                RM.IsSuccess = true;
                RM.Message = "媒体检查通过";

                DataTable OrderMeidaAll = DBContext.PTMMHZ.GetDataTable("select AdOrderId,OutDoorMediaName,OutDoorMediaNumbering,ServiceName,IssueDate,EndDate,MediaState from tblBicycleMadialist where  AdOrderId='" + AdOrderId + "'");
                //得到当前订单的媒体明细
                DataTable OrderMeida = DBContext.PTMMHZ.GetDataTable("select AdOrderId, MediaName,OutDoorMediaNumbering,ServiceName,IssueDate,EndDate from dbo.tblBicycleMadialist_Making where ProcessId='" + ProcessId + "'");
                Boolean IsAllow = true;
                foreach (DataRow Dr in OrderMeida.Rows)
                {
                    string MediaName = Dr["MediaName"] == null ? "" : Dr["MediaName"].ToString().TrimEnd();
                    string ServiceName = Dr["ServiceName"] == null ? "" : Dr["ServiceName"].ToString().TrimEnd();
                    string OutDoorMediaNumbering = Dr["OutDoorMediaNumbering"] == null ? "" : Dr["OutDoorMediaNumbering"].ToString().TrimEnd();
                    DateTime DtBegin = DateTime.Parse(DateTime.Parse(Dr["IssueDate"].ToString()).ToShortDateString());
                    DateTime DtEnd = DateTime.Parse(DateTime.Parse(Dr["EndDate"].ToString()).ToShortDateString());
                    DataRow[] DtRows = OrderMeidaAll.Select(" OutDoorMediaNumbering='" + OutDoorMediaNumbering + "' and AdOrderId='" + AdOrderId + "'");
                    if (DtRows.Count() == 0)
                    {
                        string AdMsgContent = " 媒体名称:" + MediaName + " 服务名称名称:" + ServiceName + " 媒体编号:" + OutDoorMediaNumbering + ",";
                        mediaLog += AdMsgContent;
                    }
                }
                if (!string.IsNullOrWhiteSpace(mediaLog))
                {
                    IsAllow = false;
                    mediaLog += "在订单中数据不匹配";
                }
                RM.IsSuccess = IsAllow;
                RM.Message = mediaLog;
            }
            catch (Exception ex)
            {
                RM.IsSuccess = false;
                RM.Message = "检查过程遇到错误！";
                Bitshare.PTMM.Common.LogManager.Error("WebPublicCode-OutMediaMaking", ex);
            }
            return RM;
        }
        #endregion

        #region 车身派工 通过派工数据修改合同数据-车身
        //通过派工数据修改合同数据-车身
        public static Dictionary<bool, List<String>> VehicleMediaing(string ProcessId, string AdOrderId)
        {
            List<string> listsql = new List<string>();
            List<string> listError = new List<string>();
            Dictionary<bool, List<String>> dGetOutMedia = new Dictionary<bool, List<string>>();
            try
            {
                List<tblAdFixingList_Making> OrderMedia = BusinessContext.tblAdFixingList_Making.GetModelList("ProcessId='" + ProcessId + "'").Where(p => p.ProcessId == ProcessId).ToList();
                List<tblAdFixingList> FixingList = BusinessContext.tblAdFixingList.GetModelList("AdOrderId='" + AdOrderId + "'").Where(p => p.AdOrderId == AdOrderId).ToList();
                foreach (tblAdFixingList_Making item in OrderMedia)
                {
                    string MediaName = item.MediaName;
                    string VehicleNumbering = item.VehicleNumbering;
                    DateTime DtBegin = DateTime.Parse(item.IssueDate.ToString());
                    DateTime DtEnd = DateTime.Parse(item.EndDate.ToString());
                    DateTime FixDate = DateTime.Parse(item.FixDate.ToString());
                    string AdOrderid = item.AdOrderId;
                    var fix = FixingList.Where(p => p.VehicleNumbering == VehicleNumbering && p.MediaName == MediaName && p.InsureEndDate == DtEnd && p.InsureIssueDate == DtBegin).ToList();
                    //var fix = FixingList.Where(p => p.VehicleNumbering == VehicleNumbering && p.MediaName == MediaName && (p.FixDate == null || string.IsNullOrWhiteSpace(Convert.ToString(p.FixDate.Value)))).ToList();
                    if (fix.Count() == 1 && item.FixDate != null)
                    {
                        string strCdt = " where  VehicleNumbering='" + VehicleNumbering + "' and AdOrderId='" + AdOrderId + "' ";

                        string sql = "update tblAdFixinglist set FixDate='" + FixDate +"' "+ strCdt;
                        listsql.Add(sql);
                    }
                    else
                    {
                        listError.Add("车辆编号" + VehicleNumbering);
                    }

                }
                if (listsql.Count > 0)
                {
                    dGetOutMedia.Add(true, listsql);
                }
                if (listError.Count > 0)
                {
                    dGetOutMedia.Add(false, listError);
                }

            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("VehicleMediaing", ex);
            }
            return dGetOutMedia;
        }
        #endregion

        #region 户外派工 通过派工数据修改合同数据-户外
        public static Dictionary<bool, List<String>> OutMediaing(string ProcessId, string AdOrderId)
        {
            List<string> listsql = new List<string>();
            List<string> listError = new List<string>();
            Dictionary<bool, List<String>> dGetOutMedia = new Dictionary<bool, List<string>>();
            try
            {
                List<tblOutdoorAdOrderMedia_Making> OrderMedia = BusinessContext.tblOutdoorAdOrderMedia_Making.GetModelList("ProcessId='" + ProcessId + "'").Where(p => p.ProcessId == ProcessId).ToList();
                List<tblOutdoorAdOrderMedia> OutdoorList = BusinessContext.tblOutdoorAdOrderMedia.GetModelList("AdOrderId='" + AdOrderId + "'").Where(p => p.AdOrderId == AdOrderId).ToList();
                foreach (tblOutdoorAdOrderMedia_Making item in OrderMedia)
                {
                    string MediaName = item.MediaName;
                    string RoadName = item.RoadName;
                    string PlatFormName = item.PlatFormName;
                    string MediaNumber = item.OutDoorMediaNumbering;
                    DateTime DtBegin = Convert.ToDateTime(item.IssueDate);
                    DateTime DtEnd = Convert.ToDateTime(item.EndDate);
                    DateTime FixDate = Convert.ToDateTime(item.FixDate);
                    //var fix = OutdoorList.Where(p => p.OutDoorMediaNumbering == MediaNumber && p.OutDoorMediaName == MediaName && (p.FixDate == null || string.IsNullOrWhiteSpace(Convert.ToString(p.FixDate.Value)))).ToList();
                    var fix = OutdoorList.Where(p => p.OutDoorMediaNumbering == MediaNumber && p.OutDoorMediaName == MediaName && p.endDate == DtEnd && p.IssueDate == DtBegin).ToList();
                    if (fix.Count() == 1 && item.FixDate != null)
                    {
                        string sql = "update tblOutdoorAdOrderMedia set FixDate='" + FixDate + "' where OutDoorMediaNumbering='" + MediaNumber + "' and IssueDate='" + DtBegin + "' and EndDate='" + DtEnd + "' and AdOrderId='" + AdOrderId + "'";
                        listsql.Add(sql);
                    }
                    else
                    {
                        listError.Add("媒体编号" + MediaNumber);
                    }
                }
                if (listsql.Count > 0)
                {
                    dGetOutMedia.Add(true, listsql);
                }
                if (listError.Count > 0)
                {
                    dGetOutMedia.Add(false, listError);
                }
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("WebPublicCode-OutMediaing", ex);
            }
            return dGetOutMedia;
        }
        #endregion

        #region 自行车派工 通过派工数据修改合同数据-自行车
        public static Dictionary<bool, List<String>> BicycleMediaing(string ProcessId, string AdOrderId)
        {
            List<string> listsql = new List<string>();
            List<string> listError = new List<string>();
            Dictionary<bool, List<String>> dGetOutMedia = new Dictionary<bool, List<string>>();
            try
            {
                List<tblBicycleMadialist_Making> OrderMedia = BusinessContext.tblBicycleMadialist_Making.GetModelList("ProcessId='" + ProcessId + "'").Where(p => p.ProcessId == ProcessId).ToList();
                List<tblBicycleMadialist> OutdoorList = BusinessContext.tblBicycleMadialist.GetModelList("AdOrderId='" + AdOrderId + "'").Where(p => p.AdOrderID == AdOrderId).ToList();
                foreach (tblBicycleMadialist_Making item in OrderMedia)
                {
                    string MediaName = item.MediaName;
                    string RoadName = item.RoadName;
  
                    string MediaNumber = item.OutDoorMediaNumbering;
                    DateTime DtBegin = Convert.ToDateTime(item.IssueDate);
                    DateTime DtEnd = Convert.ToDateTime(item.EndDate);
                    DateTime FixDate = Convert.ToDateTime(item.FixDate);
                    var fix = OutdoorList.Where(p => p.OutDoorMediaNumbering == MediaNumber && p.OutDoorMediaName == MediaName && p.endDate == DtEnd && p.IssueDate == DtBegin).ToList();
                    //var fix = OutdoorList.Where(p => p.OutDoorMediaNumbering == MediaNumber && p.OutDoorMediaName == MediaName && (p.FixDate == null || string.IsNullOrWhiteSpace(Convert.ToString(p.FixDate.Value)))).ToList();
                    if (fix.Count() == 1 && item.FixDate!=null)
                    {
                        string sql = "update tblBicycleMadialist set FixDate='" + FixDate + "' where OutDoorMediaNumbering='" + MediaNumber + "' and IssueDate='" + DtBegin + "' and EndDate='" + DtEnd + "' and AdOrderId='" + AdOrderId + "'";
                        listsql.Add(sql);
                    }
                    else
                    {
                        listError.Add("媒体编号" + MediaNumber);
                    }
                }
                if (listsql.Count > 0)
                {
                    dGetOutMedia.Add(true, listsql);
                }
                if (listError.Count > 0)
                {
                    dGetOutMedia.Add(false, listError);
                }
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("WebPublicCode-OutMediaing", ex);
            }
            return dGetOutMedia;
        }
        #endregion
    }
}