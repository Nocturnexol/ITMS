using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Service.Impl;
using Bitshare.PTMM.Service;
using System.Data;
using System.Reflection;
using Bitshare.PTMM.Common;
using Bitshare.PTMM.Model;
using Bitshare.PTMM.Service.Helper;
using Bitshare.PTMM.Service.Enum;
namespace Bitshare.PTMM.Service.Impl
{
    #region 工厂定义
    /// <summary>
    /// wyj
    /// 2016/6/15
    /// </summary>
    public class MediaServiceFactory
    {
        static IMediaService Instance;
        private string serviceName;
        /// <summary>
        /// 
        /// </summary>
        public MediaServiceFactory()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        public MediaServiceFactory(string serviceName)
        {
            this.serviceName = serviceName;
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public IMediaService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new MediaService();
            }
            return Instance;
        }
    }

    #endregion

    #region 实现类
    /// <summary>
    /// 
    /// </summary>
    public class MediaService : IMediaService
    {
        /// <summary>
        /// 公用工厂
        /// </summary>
        public CommonServiceFactory factory = new CommonServiceFactory();

        #region 媒体定义
        /// <summary>
        /// 分页获取媒体定义主列表统计数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public PageData GetVehicle_MediaPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
        {
            PageData result = new PageData();
            try
            {
                ICommonService service = factory.GetInstance();
                result = service.ListByPage<Bitshare.PTMM.Model.tblVehicle_Media>(pageIndex, pageSize, queryFields, sWhere, orderBy);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetVehicle_MediaPageResult", ex);
            }
            return result;
        }
        /// <summary>
        /// 分页获取媒体定义互斥媒体列表统计数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public PageData GetExcludedMediaPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
        {
            PageData result = new PageData();
            try
            {
                ICommonService service = factory.GetInstance();
                result = service.ListByPage<Bitshare.PTMM.Model.tblExcludedMedia>(pageIndex, pageSize, queryFields, sWhere, orderBy);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetExcludedMediaPageResult", ex);
            }
            return result;
        }
        #endregion
        #region 线路基本信息
        /// <summary>
        /// 分页获取线路列表统计数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public PageData GetPtRoadPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
        {
            PageData result = new PageData();
            try
            {
                ICommonService service = factory.GetInstance();
                result = service.ListByPage<Bitshare.PTMM.Model.tblPtRoad>(pageIndex, pageSize, queryFields, sWhere, orderBy);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetPtRoadPageResult", ex);
            }
            return result;
        }
        /// <summary>
        /// 分页获取线路分类列表统计数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public PageData GetPtRoadSortPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
        {
            PageData result = new PageData();
            try
            {
                ICommonService service = factory.GetInstance();
                result = service.ListByPage<Bitshare.PTMM.Model.tblPtRoadSort>(pageIndex, pageSize, queryFields, sWhere, orderBy);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetPtRoadSortPageResult", ex);
            }
            return result;
        }
        /// <summary>
        /// 分页获取公交停站列表统计数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public PageData GetPtPlatFormPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
        {
            PageData result = new PageData();
            try
            {
                ICommonService service = factory.GetInstance();
                result = service.ListByPage<Bitshare.PTMM.Model.tblPtPlatForm>(pageIndex, pageSize, queryFields, sWhere, orderBy);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetPtPlatFormPageResult", ex);
            }
            return result;
        }
        /// <summary>
        /// 分页获取景观区域列表统计数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public PageData GetPtPassRegionPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
        {
            PageData result = new PageData();
            try
            {
                ICommonService service = factory.GetInstance();
                result = service.ListByPage<Bitshare.PTMM.Model.tblPtPassRegion>(pageIndex, pageSize, queryFields, sWhere, orderBy);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetPtPassRegionPageResult", ex);
            }
            return result;
        }
        /// <summary>
        /// 分页获取路由列表统计数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public PageData GetPtRoadPathPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
        {
            PageData result = new PageData();
            try
            {
                ICommonService service = factory.GetInstance();
                result = service.ListByPage<Bitshare.PTMM.Model.tblPtRoadPath>(pageIndex, pageSize, queryFields, sWhere, orderBy);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetPtRoadPathPageResult", ex);
            }
            return result;
        }
        #endregion
        #region 车辆照片搜索
        /// <summary>
        /// 分页获取车辆照片搜索列表统计数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public PageData GetSelectVehiclePhotoPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
        {
            PageData result = new PageData();
            try
            {
                ICommonService service = factory.GetInstance();
                result = service.ListByPage<Bitshare.PTMM.Model.View_VehicleTotalPhotos>(pageIndex, pageSize, queryFields, sWhere, orderBy);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetSelectVehiclePhotoPageResult", ex);
            }
            return result;
        }
        #endregion
        #region 自行车站点统计
        /// <summary>
        /// 分页获取自行车站点统计列表统计数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public PageData GetBicycleStopInfoPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string sWhere2 = " 1=1 ")
        {
            PageData result = new PageData();
            try
            {
                ICommonService service = factory.GetInstance(); //View_BicycleStopInfo
                DataTable ds_BicycleStop = DBContext.PTMMHZ.GetDataTable("select * from tblBicycleStop where " + sWhere + " order by " + orderBy);
                DataTable companyname_ds = DBContext.PTMMHZ.GetDataTable(" select distinct  companyname from dbo.tblBicycle_Madia where " + sWhere2);
                DataTable outdoormadie_ds = DBContext.PTMMHZ.GetDataTable(" select distinct  outdoormadiename from dbo.tblBicycle_Madia where " + sWhere2);
                List<string> outdoormadieList = new List<string>();
                List<string> companynameList = new List<string>();
                foreach (DataRow dr in companyname_ds.Rows)
                {
                    string Scompanyname = Convert.ToString(dr["companyname"]);
                    if (!string.IsNullOrWhiteSpace(Scompanyname) && !companynameList.Contains(Scompanyname))
                    {
                        companynameList.Add(Scompanyname);
                    }
                }
                foreach (DataRow dr in outdoormadie_ds.Rows)
                {
                    string Soutdoormadiename = Convert.ToString(dr["outdoormadiename"]);
                    if (!string.IsNullOrWhiteSpace(Soutdoormadiename) && !companynameList.Contains(Soutdoormadiename))
                    {
                        outdoormadieList.Add(Soutdoormadiename);
                    }
                }
                int NCount = 0;
                DataTable BicycleStopInfo_ds = DBContext.PTMMHZ.GetDataTable("select COMPANYNAME,StopId,OUTDOORMADIENAME from View_BicycleStopInfo");
                if (ds_BicycleStop != null && ds_BicycleStop.Rows.Count > 0)
                {
                    int SumOutDoorMadieNum = 0;
                    int TOutDoorMadieNum = 0;
                    int OutDoorMadieNum = 0;
                    bool DCOMPANYNAME = false;
                    int nLength = outdoormadieList.Count();
                    int[] OutDoorMedia = new int[nLength];
                    for (int i = 0; i < ds_BicycleStop.Rows.Count; i++)
                    {
                        string StopId = Convert.ToString(ds_BicycleStop.Rows[i]["StopId"]);
                        DataRow[] datarow = BicycleStopInfo_ds.Select("StopId ='" + StopId + "'");
                        if (datarow != null && datarow.Length > 0)
                        {
                            OutDoorMadieNum = datarow.Count();//总数
                            if (!ds_BicycleStop.Columns.Contains("OutDoorMadieNum"))
                                ds_BicycleStop.Columns.Add("OutDoorMadieNum", Type.GetType("System.Int32"));
                            ds_BicycleStop.Rows[i]["OutDoorMadieNum"] = OutDoorMadieNum;
                            NCount = 0;
                            foreach (string Scompanyname in companynameList)
                            {
                                //已拆除数
                                DataRow[] datarow1 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and COMPANYNAME='" + Scompanyname + "'");
                                if (datarow1 != null && datarow1.Length > 0)
                                {
                                    DCOMPANYNAME = true;
                                    if (!ds_BicycleStop.Columns.Contains("COMPANYNAME" + NCount))
                                        ds_BicycleStop.Columns.Add("COMPANYNAME" + NCount, Type.GetType("System.Boolean"));
                                    ds_BicycleStop.Rows[i]["COMPANYNAME" + NCount] = DCOMPANYNAME;

                                }
                                else
                                {
                                    DCOMPANYNAME = false;
                                    if (!ds_BicycleStop.Columns.Contains("COMPANYNAME" + NCount))
                                        ds_BicycleStop.Columns.Add("COMPANYNAME" + NCount, Type.GetType("System.Boolean"));
                                    ds_BicycleStop.Rows[i]["COMPANYNAME" + NCount] = DCOMPANYNAME;
                                }
                                NCount++;
                            }
                            NCount = 0;
                            foreach (string Soutdoormadiename in outdoormadieList)
                            {
                                DataRow[] datarow1 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and OUTDOORMADIENAME='" + Soutdoormadiename + "'");
                                if (datarow1 != null && datarow1.Length > 0)
                                {
                                    TOutDoorMadieNum = datarow1.Count();
                                    OutDoorMedia[NCount] += TOutDoorMadieNum;
                                    if (!ds_BicycleStop.Columns.Contains("OutDoorMadieNum" + NCount))
                                        ds_BicycleStop.Columns.Add("OutDoorMadieNum" + NCount, Type.GetType("System.Int32"));
                                    ds_BicycleStop.Rows[i]["OutDoorMadieNum" + NCount] = TOutDoorMadieNum;
                                }
                                else
                                {
                                    TOutDoorMadieNum = 0;
                                    if (!ds_BicycleStop.Columns.Contains("OutDoorMadieNum" + NCount))
                                        ds_BicycleStop.Columns.Add("OutDoorMadieNum" + NCount, Type.GetType("System.Int32"));
                                    ds_BicycleStop.Rows[i]["OutDoorMadieNum" + NCount] = TOutDoorMadieNum;
                                }
                                NCount++;
                            }
                        }
                    }
                    DataRow dr = ds_BicycleStop.NewRow();
                    dr["TblRcdId"] = 0;
                    dr["StopId"] = "总计";
                    NCount = 0;
                    foreach (string Soutdoormadiename in outdoormadieList)
                    {
                        SumOutDoorMadieNum += OutDoorMedia[NCount];
                        dr["OutDoorMadieNum" + NCount] = OutDoorMedia[NCount];
                        NCount++;
                    }
                    dr["OutDoorMadieNum"] = SumOutDoorMadieNum;
                    ds_BicycleStop.Rows.Add(dr);

                }
                result.page = pageIndex;
                result.records = ds_BicycleStop != null ? ds_BicycleStop.Rows.Count : 0;
                ds_BicycleStop = SplitDataTable(ds_BicycleStop, pageIndex, pageSize);
                List<Model.tblBicycleStop> lis_BicycleStops = new List<Model.tblBicycleStop>();
                lis_BicycleStops = ds_BicycleStop.ToList<Model.tblBicycleStop>();
                result.total = (result.records + pageSize - 1) / pageSize;
                result.rows = lis_BicycleStops;
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetBicycleStopInfoPageResult", ex);
            }
            return result;
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sWhere"></param>
        /// <param name="sWhere2"></param>
        /// <returns></returns>
        public DataTable GetExportDataBicycleMedia(int pageIndex = 1, int pageSize = 50, string sWhere = null, string sWhere2 = null)
        {
            DataTable ds_BicycleStop = DBContext.PTMMHZ.GetDataTable("select StopId as '服务点编号',Area as '区域', ROADNAME as '路段名称',SpecificPosition as '具体位置',ServiceName as '服务点名称',Remark as '备注' " +
                " from tblBicycleStop where " + sWhere + " order by TblRcdId desc");
            DataTable companyname_ds = DBContext.PTMMHZ.GetDataTable(" select distinct  companyname from dbo.tblBicycle_Madia  ");
            DataTable outdoormadie_ds = DBContext.PTMMHZ.GetDataTable(" select distinct  outdoormadiename from dbo.tblBicycle_Madia  ");
            List<string> outdoormadieList = new List<string>();
            List<string> companynameList = new List<string>();
            foreach (DataRow dr in companyname_ds.Rows)
            {
                string Scompanyname = Convert.ToString(dr["companyname"]);
                if (!string.IsNullOrWhiteSpace(Scompanyname) && !companynameList.Contains(Scompanyname))
                {
                    companynameList.Add(Scompanyname);
                }
            }
            foreach (DataRow dr in outdoormadie_ds.Rows)
            {
                string Soutdoormadiename = Convert.ToString(dr["outdoormadiename"]);
                if (!string.IsNullOrWhiteSpace(Soutdoormadiename) && !outdoormadieList.Contains(Soutdoormadiename))
                {
                    outdoormadieList.Add(Soutdoormadiename);
                }
            }
            int NCount = 0;
            DataTable BicycleStopInfo_ds = DBContext.PTMMHZ.GetDataTable("select COMPANYNAME,StopId,OUTDOORMADIENAME from View_BicycleStopInfo");
            if (ds_BicycleStop != null && ds_BicycleStop.Rows.Count > 0)
            {
                int SumOutDoorMadieNum = 0;
                int TOutDoorMadieNum = 0;
                int OutDoorMadieNum = 0;
                int nLength = outdoormadieList.Count();
                int[] OutDoorMedia = new int[nLength];
                for (int i = 0; i < ds_BicycleStop.Rows.Count; i++)
                {
                    string StopId = Convert.ToString(ds_BicycleStop.Rows[i]["服务点编号"]);
                    DataRow[] datarow = BicycleStopInfo_ds.Select("StopId ='" + StopId + "'");
                    if (datarow != null && datarow.Length > 0)
                    {
                        OutDoorMadieNum = datarow.Count();//总数
                        if (!ds_BicycleStop.Columns.Contains("媒体数量"))
                            ds_BicycleStop.Columns.Add("媒体数量", Type.GetType("System.Int32"));
                        ds_BicycleStop.Rows[i]["媒体数量"] = OutDoorMadieNum;
                        NCount = 0;
                        foreach (string Soutdoormadiename in outdoormadieList)
                        {
                            DataRow[] datarow1 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and OUTDOORMADIENAME='" + Soutdoormadiename + "'");
                            if (datarow1 != null && datarow1.Length > 0)
                            {
                                TOutDoorMadieNum = datarow1.Count();
                                OutDoorMedia[NCount] += TOutDoorMadieNum;
                                if (!ds_BicycleStop.Columns.Contains(Soutdoormadiename))
                                    ds_BicycleStop.Columns.Add(Soutdoormadiename, Type.GetType("System.String"));
                                ds_BicycleStop.Rows[i][Soutdoormadiename] = TOutDoorMadieNum;
                            }
                            else
                            {
                                TOutDoorMadieNum = 0;
                                if (!ds_BicycleStop.Columns.Contains(Soutdoormadiename))
                                    ds_BicycleStop.Columns.Add(Soutdoormadiename, Type.GetType("System.String"));
                                ds_BicycleStop.Rows[i][Soutdoormadiename] = "";
                            }
                            NCount++;
                        }
                        NCount = 0;
                        foreach (string Scompanyname in companynameList)
                        {
                            //已拆除数
                            DataRow[] datarow1 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and COMPANYNAME='" + Scompanyname + "'");
                            if (datarow1 != null && datarow1.Length > 0)
                            {
                                if (!ds_BicycleStop.Columns.Contains(Scompanyname))
                                    ds_BicycleStop.Columns.Add(Scompanyname, Type.GetType("System.String"));
                                ds_BicycleStop.Rows[i][Scompanyname] = "是";

                            }
                            else
                            {
                                if (!ds_BicycleStop.Columns.Contains(Scompanyname))
                                    ds_BicycleStop.Columns.Add(Scompanyname, Type.GetType("System.String"));
                                ds_BicycleStop.Rows[i][Scompanyname] = "否";
                            }
                            NCount++;
                        }
                    }
                }
                DataRow dr = ds_BicycleStop.NewRow();
                //dr["TblRcdId"] = 0;
                dr["服务点编号"] = "总计";
                NCount = 0;
                foreach (string Soutdoormadiename in outdoormadieList)
                {
                    SumOutDoorMadieNum += OutDoorMedia[NCount];
                    dr[Soutdoormadiename] = OutDoorMedia[NCount];
                    NCount++;
                }
                dr["媒体数量"] = SumOutDoorMadieNum;
                ds_BicycleStop.Rows.Add(dr);

            }
            return ds_BicycleStop;
        }
        #region 暂时不用
        //public PageData GetBicycleStopInfoPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
        //{
        //    PageData result = new PageData();
        //    try
        //    {
        //        ICommonService service = factory.GetInstance(); //View_BicycleStopInfo
        //        DataTable ds_BicycleStop = DBContext.PTMMHZ.GetDataTable("select *,'' as 'OutDoorMadieNum','' as 'DOutDoorMadieNum','' as 'OOutDoorMadieNum','' as 'YOutDoorMadieNum','' as 'TOutDoorMadieNum','' as 'DCOMPANYNAME','' as 'SCOMPANYNAME','' as 'WCOMPANYNAME','' as 'VCOMPANYNAME' from tblBicycleStop where " + sWhere + " order by " + orderBy);
        //        result = service.ListByPage<Bitshare.PTMMHZ.Model.tblBicycleStop>(pageIndex, pageSize, queryFields, sWhere, orderBy);
        //        //DataTable BicycleStop_ds = DBContext.PTMMHZ.GetDataTable("select * from tblBicycleStop where " + sWhere + " order by " + orderBy);
        //        //List<Bitshare.PTMMHZ.Model.tblBicycleStop> ds = (List<Bitshare.PTMMHZ.Model.tblBicycleStop>)result.rows;
        //        DataTable BicycleStopInfo_ds = DBContext.PTMMHZ.GetDataTable("select COMPANYNAME,StopId,OUTDOORMADIENAME from View_BicycleStopInfo");
        //        if (ds_BicycleStop != null && ds_BicycleStop.Rows.Count > 0)
        //        {
        //            int SumDOutDoorMadieNum = 0;
        //            int SumOOutDoorMadieNum = 0;
        //            int SumYOutDoorMadieNum=0;
        //            int SumTOutDoorMadieNum = 0;
        //            for (int i = 0; i < ds_BicycleStop.Rows.Count; i++)
        //            {
        //                int OutDoorMadieNum = 0;
        //                int DOutDoorMadieNum = 0;
        //                int OOutDoorMadieNum = 0;
        //                int YOutDoorMadieNum = 0;
        //                int TOutDoorMadieNum = 0;
        //                bool DCOMPANYNAME = false;
        //                bool SCOMPANYNAME = false;
        //                bool WCOMPANYNAME = false;
        //                bool VCOMPANYNAME = false;
        //                string StopId = Convert.ToString(ds_BicycleStop.Rows[i]["StopId"]);
        //                DataRow[] datarow = BicycleStopInfo_ds.Select("StopId ='" + StopId + "'");
        //                ds_BicycleStop.Rows[i]["DCOMPANYNAME"] = DCOMPANYNAME;
        //                ds_BicycleStop.Rows[i]["SCOMPANYNAME"] = SCOMPANYNAME;
        //                ds_BicycleStop.Rows[i]["WCOMPANYNAME"] = WCOMPANYNAME;
        //                ds_BicycleStop.Rows[i]["VCOMPANYNAME"] = VCOMPANYNAME;
        //                ds_BicycleStop.Rows[i]["OutDoorMadieNum"] = 0;
        //                if (datarow != null && datarow.Length > 0)
        //                {
        //                    OutDoorMadieNum = datarow.Count();//总数
        //                    ds_BicycleStop.Rows[i]["OutDoorMadieNum"] = OutDoorMadieNum;
        //                    //已拆除数
        //                    DataRow[] datarow1 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and COMPANYNAME='已拆除'");
        //                    if (datarow1 != null && datarow1.Length > 0)
        //                    {
        //                        DCOMPANYNAME=true;
        //                        ds_BicycleStop.Rows[i]["DCOMPANYNAME"] = DCOMPANYNAME;
        //                    }
        //                    //自行车公司数
        //                    DataRow[] datarow2 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and COMPANYNAME='自行车公司'");
        //                    if (datarow2 != null && datarow2.Length > 0)
        //                    {
        //                        VCOMPANYNAME = true;
        //                        ds_BicycleStop.Rows[i]["VCOMPANYNAME"] = VCOMPANYNAME;
        //                    }
        //                    else
        //                    {
        //                        ds_BicycleStop.Rows[i]["VCOMPANYNAME"] = false;
        //                    }
        //                    //思美数
        //                    DataRow[] datarow3 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and COMPANYNAME='思美'");
        //                    if (datarow3 != null && datarow3.Length > 0)
        //                    {
        //                        SCOMPANYNAME = true;
        //                        ds_BicycleStop.Rows[i]["SCOMPANYNAME"] = SCOMPANYNAME;
        //                    }
        //                    else
        //                    {
        //                        ds_BicycleStop.Rows[i]["SCOMPANYNAME"] = false;
        //                    }
        //                    //文明办数
        //                    DataRow[] datarow4 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and COMPANYNAME='文明办'");
        //                    if (datarow4 != null && datarow4.Length > 0)
        //                    {
        //                        WCOMPANYNAME = true;
        //                        ds_BicycleStop.Rows[i]["WCOMPANYNAME"] = WCOMPANYNAME;
        //                    }
        //                    else
        //                    {
        //                        ds_BicycleStop.Rows[i]["WCOMPANYNAME"] = false;
        //                    }
        //                    //圆弧灯箱
        //                    datarow4 = null;
        //                    datarow4 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and OUTDOORMADIENAME='圆弧灯箱'");
        //                    if (datarow4 != null && datarow4.Length > 0)
        //                    {
        //                        YOutDoorMadieNum = datarow4.Count();
        //                        SumYOutDoorMadieNum += YOutDoorMadieNum;
        //                        ds_BicycleStop.Rows[i]["YOutDoorMadieNum"] = YOutDoorMadieNum;
        //                    }
        //                    else
        //                    {
        //                        ds_BicycleStop.Rows[i]["YOutDoorMadieNum"] = 0;
        //                    }
        //                    //双面灯箱
        //                    datarow4 = null;
        //                    datarow4 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and OUTDOORMADIENAME='双面灯箱'");
        //                    if (datarow4 != null && datarow4.Length > 0)
        //                    {
        //                        DOutDoorMadieNum = datarow4.Count();
        //                        SumDOutDoorMadieNum += DOutDoorMadieNum;
        //                        ds_BicycleStop.Rows[i]["DOutDoorMadieNum"] = DOutDoorMadieNum;
        //                    }
        //                    else
        //                    {
        //                        ds_BicycleStop.Rows[i]["DOutDoorMadieNum"] = 0;
        //                    }
        //                    //单面灯箱
        //                    datarow4 = null;
        //                    datarow4 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and OUTDOORMADIENAME='单面灯箱'");
        //                    if (datarow4 != null && datarow4.Length > 0)
        //                    {
        //                        OOutDoorMadieNum = datarow4.Count();
        //                        SumOOutDoorMadieNum += OOutDoorMadieNum;
        //                        ds_BicycleStop.Rows[i]["OOutDoorMadieNum"] = OOutDoorMadieNum;
        //                    }
        //                    else
        //                    {
        //                        ds_BicycleStop.Rows[i]["OOutDoorMadieNum"] = 0;
        //                    }
        //                    //亭背灯箱
        //                    datarow4 = null;
        //                    datarow4 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and OUTDOORMADIENAME='亭背灯箱'");
        //                    if (datarow4 != null && datarow4.Length > 0)
        //                    {
        //                        TOutDoorMadieNum = datarow4.Count();
        //                        SumTOutDoorMadieNum += TOutDoorMadieNum;
        //                        ds_BicycleStop.Rows[i]["TOutDoorMadieNum"] = TOutDoorMadieNum;
        //                    }
        //                    else
        //                    {
        //                        ds_BicycleStop.Rows[i]["TOutDoorMadieNum"] = 0;
        //                    }
        //                }
        //            }
        //            DataRow dr = ds_BicycleStop.NewRow();
        //            dr["TblRcdId"] = 0;
        //            dr["StopId"] = "总计";
        //            dr["OOutDoorMadieNum"] = SumOOutDoorMadieNum;
        //            dr["YOutDoorMadieNum"] = SumYOutDoorMadieNum;
        //            dr["DOutDoorMadieNum"] = SumDOutDoorMadieNum;
        //            dr["TOutDoorMadieNum"] = SumTOutDoorMadieNum;
        //            dr["OutDoorMadieNum"] = SumTOutDoorMadieNum + SumDOutDoorMadieNum + SumYOutDoorMadieNum + SumOOutDoorMadieNum;
        //            dr["SCOMPANYNAME"] = false;
        //            dr["DCOMPANYNAME"] = false;
        //            dr["WCOMPANYNAME"] = false;
        //            dr["VCOMPANYNAME"] = false;
        //            ds_BicycleStop.Rows.Add(dr);
        //            //Model.tblBicycleStop SumBicycle = new Model.tblBicycleStop();
        //            //SumBicycle.StopId = "总计";
        //            //SumBicycle.OOutDoorMadieNum = SumOOutDoorMadieNum;
        //            //SumBicycle.YOutDoorMadieNum = SumYOutDoorMadieNum;
        //            //SumBicycle.DOutDoorMadieNum = SumDOutDoorMadieNum;
        //            //SumBicycle.TOutDoorMadieNum = SumTOutDoorMadieNum;
        //            //SumBicycle.OutDoorMadieNum = SumTOutDoorMadieNum + SumDOutDoorMadieNum + SumYOutDoorMadieNum + SumOOutDoorMadieNum;
        //            //SumBicycle.TblRcdId = 0;
        //            //ds.Add(SumBicycle);

        //        }
        //        result.page = pageIndex;
        //        result.records = ds_BicycleStop != null ? ds_BicycleStop.Rows.Count : 0;
        //        //result.pageCount = pageSize;
        //        ds_BicycleStop=SplitDataTable(ds_BicycleStop, pageIndex, pageSize);
        //        List<Model.tblBicycleStop> lis_BicycleStops = new List<Model.tblBicycleStop>();
        //        foreach(DataRow dr in ds_BicycleStop.Rows)
        //        {
        //            Model.tblBicycleStop lis_BicycleStop = new Model.tblBicycleStop();
        //            lis_BicycleStop.Area = Convert.ToString(dr["Area"]);
        //            lis_BicycleStop.StopId = Convert.ToString(dr["StopId"]);
        //            lis_BicycleStop.ServiceName = Convert.ToString(dr["ServiceName"]);
        //            lis_BicycleStop.ROADNAME = Convert.ToString(dr["ROADNAME"]);
        //            lis_BicycleStop.SpecificPosition = Convert.ToString(dr["SpecificPosition"]);
        //            lis_BicycleStop.SpecificPosition = Convert.ToString(dr["SpecificPosition"]);
        //            lis_BicycleStop.TblRcdId = Convert.ToInt32(dr["TblRcdId"]);
        //            lis_BicycleStop.DCOMPANYNAME = Convert.ToBoolean(dr["DCOMPANYNAME"] == DBNull.Value ? false : dr["DCOMPANYNAME"]);
        //            lis_BicycleStop.VCOMPANYNAME = Convert.ToBoolean(dr["VCOMPANYNAME"] == DBNull.Value ? false : dr["VCOMPANYNAME"]);
        //            lis_BicycleStop.WCOMPANYNAME = Convert.ToBoolean(dr["WCOMPANYNAME"] == DBNull.Value ? false : dr["WCOMPANYNAME"]);
        //            lis_BicycleStop.SCOMPANYNAME = Convert.ToBoolean(dr["SCOMPANYNAME"] == DBNull.Value ? false : dr["SCOMPANYNAME"]);
        //            lis_BicycleStop.OutDoorMadieNum = Convert.ToInt32(dr["OutDoorMadieNum"] == DBNull.Value ? 0 : dr["OutDoorMadieNum"]);
        //            lis_BicycleStop.TOutDoorMadieNum = Convert.ToInt32((dr["TOutDoorMadieNum"] == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(dr["TOutDoorMadieNum"]))) ? 0 : dr["TOutDoorMadieNum"]);
        //            lis_BicycleStop.YOutDoorMadieNum = Convert.ToInt32((dr["YOutDoorMadieNum"]== DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(dr["YOutDoorMadieNum"]))) ? 0 : dr["YOutDoorMadieNum"]);
        //            lis_BicycleStop.DOutDoorMadieNum = Convert.ToInt32((dr["DOutDoorMadieNum"] == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(dr["DOutDoorMadieNum"]))) ? 0 : dr["DOutDoorMadieNum"]);
        //            lis_BicycleStop.OOutDoorMadieNum = Convert.ToInt32((dr["OOutDoorMadieNum"] == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(dr["OOutDoorMadieNum"]))) ? 0 : dr["OOutDoorMadieNum"]);
        //            lis_BicycleStops.Add(lis_BicycleStop);
        //        }
        //        result.total = (result.records + pageSize - 1) / pageSize;
        //        result.rows = lis_BicycleStops;// GetList<Model.tblBicycleStop>(ds_BicycleStop);
        //        //result.total = pageSize;
        //    }
        //    catch (Exception ex)
        //    {
        //        Bitshare.PTMMHZ.Common.LogManager.Error("GetBicycleStopInfoPageResult", ex);
        //    }
        //    return result;
        //}
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryFields"></param>
        /// <param name="orderBy"></param>
        /// <param name="StopId"></param>
        /// <returns></returns>
        public PageData GetBicycleListPageListResult(string loginName = null, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string StopId = null, string sWhere = null)
        {
            PageData result = new PageData();
            List<Model.View_BicycleStopInfo> lis_BicycleStops = new List<Model.View_BicycleStopInfo>();
            if (!string.IsNullOrEmpty(StopId))
            {
                string _vehicleSql = "select tblrcdid,StopId, COMPANYNAME, OUTDOORMADIENAME, Area, ROADNAME, ServiceName, SpecificPosition, OUTDOORMADIENUMBERING, LIGHTING, REMARK,'' as AdContent,'' as Seller  from dbo.View_BicycleStopInfo  where StopId='" + StopId + "' and " + sWhere;
                DataTable VehicleDT = DBContext.PTMMHZ.GetDataTable(_vehicleSql);
                _vehicleSql = "SELECT Seller, OutDoorMediaName, RoadName, ServiceName, OutDoorMediaNumbering, dbo.tblBicycleMadialist.AdContent FROM  dbo.tblBicycleMadialist LEFT OUTER JOIN dbo.tblAdOrder ON dbo.tblBicycleMadialist.AdOrderID = dbo.tblAdOrder.AdOrderId WHERE (dbo.tblBicycleMadialist.IssueDate <= GETDATE()) AND (dbo.tblBicycleMadialist.endDate >= GETDATE())";
                DataTable AdorderBicycle = DBContext.PTMMHZ.GetDataTable(_vehicleSql);
                for (int i = 0; i < VehicleDT.Rows.Count; i++)
                {
                    string OUTDOORMADIENAME = Convert.ToString(VehicleDT.Rows[i]["OUTDOORMADIENAME"]);
                    string ROADNAME = Convert.ToString(VehicleDT.Rows[i]["ROADNAME"]);
                    string ServiceName = Convert.ToString(VehicleDT.Rows[i]["ServiceName"]);
                    string OutDoorMediaNumbering = Convert.ToString(VehicleDT.Rows[i]["OUTDOORMADIENUMBERING"]);
                    _vehicleSql = "OutDoorMediaName='" + OUTDOORMADIENAME + "' and RoadName='" + ROADNAME + "' and ServiceName='" + ServiceName + "' and OutDoorMediaNumbering='" + OutDoorMediaNumbering + "'";
                    DataRow[] datarow = AdorderBicycle.Select(_vehicleSql);
                    for (int j = 0; j < datarow.Length; j++)
                    {
                        VehicleDT.Rows[i]["AdContent"] = Convert.ToString(datarow[j]["AdContent"]);
                        VehicleDT.Rows[i]["Seller"] = Convert.ToString(datarow[j]["Seller"]);
                    }
                }
                foreach (DataRow dr in VehicleDT.Rows)
                {
                    Model.View_BicycleStopInfo lis_BicycleStop = new Model.View_BicycleStopInfo();
                    lis_BicycleStop.StopId = Convert.ToString(dr["StopId"]);
                    lis_BicycleStop.COMPANYNAME = Convert.ToString(dr["COMPANYNAME"]);
                    lis_BicycleStop.OUTDOORMADIENAME = Convert.ToString(dr["OUTDOORMADIENAME"]);
                    lis_BicycleStop.Area = Convert.ToString(dr["Area"]);
                    lis_BicycleStop.ROADNAME = Convert.ToString(dr["ROADNAME"]);
                    lis_BicycleStop.SpecificPosition = Convert.ToString(dr["SpecificPosition"]);
                    lis_BicycleStop.ServiceName = Convert.ToString(dr["ServiceName"]);
                    lis_BicycleStop.REMARK = Convert.ToString(dr["REMARK"]);
                    lis_BicycleStop.OUTDOORMADIENUMBERING = Convert.ToString(dr["OUTDOORMADIENUMBERING"]);
                    lis_BicycleStop.LIGHTING = Convert.ToBoolean(dr["LIGHTING"] == DBNull.Value ? false : dr["LIGHTING"]);
                    lis_BicycleStop.TblRcdId = Convert.ToInt32(dr["TblRcdId"] == DBNull.Value ? 0 : dr["TblRcdId"]);
                    lis_BicycleStops.Add(lis_BicycleStop);
                }
            }
            result.total = (result.records + pageSize - 1) / pageSize;
            result.rows = lis_BicycleStops;
            return result;
        }
        public List<T> GetList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        if (!value.ToString().Equals(""))
                        {
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list.Count == 0 ? null : list;
        }
        /// <summary>
        /// 根据索引和pagesize返回记录
        /// </summary>
        /// <param name="dt">记录集 DataTable</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="pagesize">一页的记录数</param>
        /// <returns></returns>
        public static DataTable SplitDataTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0)
                return dt;
            DataTable newdt = dt.Clone();
            //newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }
        #endregion
        #region 户外
        /// <summary>
        /// 获取户外媒体统计
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryFields"></param>
        /// <param name="orderBy"></param>
        /// <param name="sWhere"></param>
        /// <param name="MediaName"></param>
        /// <returns></returns>
        public PageData GetStopInfoPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string MediaName = "")
        {
            PageData result = new PageData();
            try
            {
                string Select = "";
                ICommonService service = factory.GetInstance(); //View_BicycleStopInfo
                DataTable ds_BicycleStop = DBContext.PTMMHZ.GetDataTable("select * from tblStop where " + sWhere + " order by " + orderBy);
                DataTable OutDoorMadieName_ds = DBContext.PTMMHZ.GetDataTable(" select distinct  OutDoorMadieName from dbo.tblOutDoor_Media");
                List<string> OutDoorMadieNameList = new List<string>();
                foreach (DataRow dr in OutDoorMadieName_ds.Rows)
                {
                    string Scompanyname = Convert.ToString(dr["OutDoorMadieName"]);
                    if (!string.IsNullOrWhiteSpace(Scompanyname) && !OutDoorMadieNameList.Contains(Scompanyname))
                    {
                        OutDoorMadieNameList.Add(Scompanyname);
                    }
                }
                int NCount = 0;
                DataTable BicycleStopInfo_ds = DBContext.PTMMHZ.GetDataTable("select StopId,OutDoorMadieName from View_StopInfo where " + sWhere);
                if (ds_BicycleStop != null && ds_BicycleStop.Rows.Count > 0)
                {
                    int SumOutDoorMadieNum = 0;
                    int OutDoorMadieNum = 0;
                    int nLength = OutDoorMadieNameList.Count();
                    int[] OutDoorMedia = new int[nLength];
                    for (int i = 0; i < ds_BicycleStop.Rows.Count; i++)
                    {
                        int TOutDoorMadieNum = 0;
                        OutDoorMadieNum = 0;
                        string StopId = Convert.ToString(ds_BicycleStop.Rows[i]["StopId"]);
                        DataRow[] datarow = BicycleStopInfo_ds.Select("StopId ='" + StopId + "'");
                        if (datarow != null && datarow.Length > 0)
                        {
                            NCount = 0;
                            foreach (string Soutdoormadiename in OutDoorMadieNameList)
                            {
                                if (MediaName.Contains(Soutdoormadiename) && !Select.Contains("OutDoorMadieNum" + NCount))
                                {
                                    Select += "OutDoorMadieNum" + NCount + ",";
                                }
                                DataRow[] datarow1 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and OutDoorMadieName='" + Soutdoormadiename + "'");
                                if (datarow1 != null && datarow1.Length > 0)
                                {
                                    TOutDoorMadieNum = datarow1.Count();
                                    OutDoorMadieNum += TOutDoorMadieNum;
                                    OutDoorMedia[NCount] += TOutDoorMadieNum;
                                    if (!ds_BicycleStop.Columns.Contains("OutDoorMadieNum" + NCount))
                                        ds_BicycleStop.Columns.Add("OutDoorMadieNum" + NCount, Type.GetType("System.Int32"));
                                    ds_BicycleStop.Rows[i]["OutDoorMadieNum" + NCount] = TOutDoorMadieNum;
                                    SumOutDoorMadieNum += TOutDoorMadieNum;
                                }
                                else
                                {
                                    if (!ds_BicycleStop.Columns.Contains("OutDoorMadieNum" + NCount))
                                        ds_BicycleStop.Columns.Add("OutDoorMadieNum" + NCount, Type.GetType("System.Int32"));

                                }
                                NCount++;
                            }
                            if (!ds_BicycleStop.Columns.Contains("OutDoorMadieNum"))
                                ds_BicycleStop.Columns.Add("OutDoorMadieNum", Type.GetType("System.Int32"));
                            if (OutDoorMadieNum > 0)
                            {
                                ds_BicycleStop.Rows[i]["OutDoorMadieNum"] = OutDoorMadieNum;
                            }
                        }
                    }
                    DataRow dr = ds_BicycleStop.NewRow();
                    dr["TblRcdId"] = 0;
                    dr["StopId"] = "总计";
                    NCount = 0;
                    foreach (string Soutdoormadiename in OutDoorMadieNameList)
                    {
                        if (!ds_BicycleStop.Columns.Contains("OutDoorMadieNum" + NCount))
                            ds_BicycleStop.Columns.Add("OutDoorMadieNum" + NCount, Type.GetType("System.Int32"));
                        dr["OutDoorMadieNum" + NCount] = OutDoorMedia[NCount];
                        NCount++;
                    }
                    if (!ds_BicycleStop.Columns.Contains("OutDoorMadieNum"))
                        ds_BicycleStop.Columns.Add("OutDoorMadieNum", Type.GetType("System.Int32"));
                    dr["OutDoorMadieNum"] = SumOutDoorMadieNum;
                    ds_BicycleStop.Rows.Add(dr);

                }
                result.page = pageIndex;
                result.records = ds_BicycleStop != null ? ds_BicycleStop.Rows.Count : 0;
                if (!string.IsNullOrWhiteSpace(MediaName))
                {
                    string[] arr = Select.Trim(',').Split(',');
                    string RowFilter = " 1=1  and (";
                    for (int i = 0; i < arr.Count(); i++)
                    {
                        if (i == 0)
                        {
                            RowFilter += "  (" + arr[i] + ">0 ) ";
                        }
                        else
                        {
                            RowFilter += " or (" + arr[i] + ">0 ) ";
                        }
                    }
                    RowFilter += ")";
                    DataView dv = ds_BicycleStop.DefaultView;
                    dv.RowFilter = RowFilter;// MediaName + " >0  and " + MediaName + " is not null ";
                    ds_BicycleStop = dv.ToTable();
                }
                ds_BicycleStop = SplitDataTable(ds_BicycleStop, pageIndex, pageSize);
                List<Model.tblStop> lis_BicycleStops = ds_BicycleStop.ToList<Model.tblStop>();
                result.total = (result.records + pageSize - 1) / pageSize;
                result.rows = lis_BicycleStops;
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetBicycleStopInfoPageResult", ex);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sWhere"></param>
        /// <param name="sWhere1"></param>
        /// <param name="MediaName"></param>
        /// <returns></returns>
        public DataTable GetExportDataOutMedia(int pageIndex = 1, int pageSize = 50, string sWhere = null, string sWhere1 = null, string MediaName = "")
        {
            DataTable ds_BicycleStop = new DataTable();
            try
            {
                ds_BicycleStop = DBContext.PTMMHZ.GetDataTable("select StopId as '序列号',Area as '区域', ROADNAME as '路段名称',PLATFORMNAME as '站名',AZIMUTH as '方位',StyleType as '款式',Remark as '备注'  from tblStop where " + sWhere + "order by StopId ");
                if (!ds_BicycleStop.Columns.Contains("媒体数量"))
                    ds_BicycleStop.Columns.Add("媒体数量", Type.GetType("System.Int32"));
                DataTable OutDoorMadieName_ds = DBContext.PTMMHZ.GetDataTable(" select distinct  OutDoorMadieName from dbo.tblOutDoor_Media");
                List<string> OutDoorMadieNameList = new List<string>();
                foreach (DataRow dr in OutDoorMadieName_ds.Rows)
                {
                    string Scompanyname = Convert.ToString(dr["OutDoorMadieName"]);
                    if (!string.IsNullOrWhiteSpace(Scompanyname) && !OutDoorMadieNameList.Contains(Scompanyname))
                    {
                        OutDoorMadieNameList.Add(Scompanyname);
                    }
                }
                int NCount = 0;
                DataTable BicycleStopInfo_ds = DBContext.PTMMHZ.GetDataTable("select StopId,OutDoorMadieName from View_StopInfo where " + sWhere1);
                if (ds_BicycleStop != null && ds_BicycleStop.Rows.Count > 0)
                {
                    int SumOutDoorMadieNum = 0;
                    int OutDoorMadieNum = 0;
                    int nLength = OutDoorMadieNameList.Count();
                    int[] OutDoorMedia = new int[nLength];
                    for (int i = 0; i < ds_BicycleStop.Rows.Count; i++)
                    {
                        int TOutDoorMadieNum = 0;
                        OutDoorMadieNum = 0;
                        string StopId = Convert.ToString(ds_BicycleStop.Rows[i]["序列号"]);
                        DataRow[] datarow = BicycleStopInfo_ds.Select("StopId ='" + StopId + "'");
                        if (datarow != null && datarow.Length > 0)
                        {
                            NCount = 0;
                            foreach (string Soutdoormadiename in OutDoorMadieNameList)
                            {
                                DataRow[] datarow1 = BicycleStopInfo_ds.Select("StopId ='" + StopId + "' and OutDoorMadieName='" + Soutdoormadiename + "'");
                                if (datarow1 != null && datarow1.Length > 0)
                                {
                                    TOutDoorMadieNum = datarow1.Count();
                                    OutDoorMadieNum += TOutDoorMadieNum;
                                    OutDoorMedia[NCount] += TOutDoorMadieNum;
                                    if (!ds_BicycleStop.Columns.Contains(Soutdoormadiename))
                                        ds_BicycleStop.Columns.Add(Soutdoormadiename, Type.GetType("System.Int32"));
                                    ds_BicycleStop.Rows[i][Soutdoormadiename] = TOutDoorMadieNum;
                                    SumOutDoorMadieNum += TOutDoorMadieNum;
                                }
                                else
                                {
                                    if (!ds_BicycleStop.Columns.Contains(Soutdoormadiename))
                                        ds_BicycleStop.Columns.Add(Soutdoormadiename, Type.GetType("System.Int32"));

                                }
                                NCount++;
                            }
                            if (!ds_BicycleStop.Columns.Contains("媒体数量"))
                                ds_BicycleStop.Columns.Add("媒体数量", Type.GetType("System.Int32"));
                            if (OutDoorMadieNum > 0)
                            {
                                ds_BicycleStop.Rows[i]["媒体数量"] = OutDoorMadieNum;
                            }
                        }
                    }
                    DataRow dr = ds_BicycleStop.NewRow();
                    // dr["TblRcdId"] = 0;
                    dr["序列号"] = "总计";
                    NCount = 0;
                    foreach (string Soutdoormadiename in OutDoorMadieNameList)
                    {
                        if (!ds_BicycleStop.Columns.Contains(Soutdoormadiename))
                            ds_BicycleStop.Columns.Add(Soutdoormadiename, Type.GetType("System.Int32"));
                        dr[Soutdoormadiename] = OutDoorMedia[NCount];
                        NCount++;
                    }

                    dr["媒体数量"] = SumOutDoorMadieNum;
                    ds_BicycleStop.Rows.Add(dr);

                    if (!string.IsNullOrWhiteSpace(MediaName))
                    {
                        string[] arr = MediaName.Trim(',').Split(',');
                        string RowFilter = " 1=1  and (";
                        for (int i = 0; i < arr.Count(); i++)
                        {
                            if (i == 0)
                            {
                                RowFilter += "  (" + arr[i] + ">0 ) ";
                            }
                            else
                            {
                                RowFilter += " or (" + arr[i] + ">0 ) ";
                            }
                        }
                        RowFilter += ")";
                        DataView dv = ds_BicycleStop.DefaultView;
                        dv.RowFilter = RowFilter;
                        ds_BicycleStop = dv.ToTable();
                        //DataView dv = ds_BicycleStop.DefaultView;
                        //dv.RowFilter = MediaName + " >0  and " + MediaName + " is not null ";
                        //ds_BicycleStop = dv.ToTable();
                    }
                }

                // ds_BicycleStop = SplitDataTable(ds_BicycleStop, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetBicycleStopInfoPageResult", ex);
            }
            return ds_BicycleStop;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryFields"></param>
        /// <param name="orderBy"></param>
        /// <param name="StopId"></param>
        /// <returns></returns>
        public PageData GetStopListPageListResult(string loginName = null, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string StopId = null, string sWhere = null)
        {
            PageData result = new PageData();
            List<Model.View_StopInfo> lis_BicycleStops = new List<Model.View_StopInfo>();
            if (!string.IsNullOrEmpty(StopId))
            {
                string sql = "select " + queryFields + " from View_StopInfo where StopId = '" + StopId + "'";
                DataTable table = DBContext.PTMMHZ.GetDataTable(sql);
                table.Columns.Add("AdContent", Type.GetType("System.String"));
                sql = "select AdContent,OutDoorMediaName,RoadName,PtPlatForm,Azimuth,OutDoorMediaNumbering from tblOutdoorAdOrderMedia where endDate > getdate()";
                DataTable ad_table = DBContext.PTMMHZ.GetDataTable(sql);
                foreach (DataRow row in table.Rows)
                {
                    string OUTDOORMADIENAME = Convert.ToString(row["OutDoorMadieName"]);
                    string ROADNAME = Convert.ToString(row["RoadName"]);
                    string PLATFORMNAME = Convert.ToString(row["PlatFormName"]);
                    string AZIMUTH = Convert.ToString(row["Azimuth"]);
                    string OUTDOORMADIENUMBERING = Convert.ToString(row["OutDoorMadieNumbering"]);
                    string str_where = " OutDoorMediaName='" + OUTDOORMADIENAME + "' AND RoadName='" + ROADNAME + "' AND PtPlatForm='" + PLATFORMNAME + "' AND Azimuth='" + AZIMUTH + "' AND OutDoorMediaNumbering='" + OUTDOORMADIENUMBERING + "'";
                    DataRow[] ds_row_temp = ad_table.Select(str_where);
                    if (ds_row_temp.Length > 0)
                    {
                        row["AdContent"] = ds_row_temp[0]["AdContent"];
                    }
                }
                foreach (DataRow dr in table.Rows)
                {
                    Model.View_StopInfo lis_BicycleStop = new Model.View_StopInfo();
                    lis_BicycleStop.StopId = Convert.ToString(dr["StopId"]);
                    lis_BicycleStop.Azimuth = Convert.ToString(dr["Azimuth"]);
                    lis_BicycleStop.PlatFormName = Convert.ToString(dr["PlatFormName"]);
                    lis_BicycleStop.Area = Convert.ToString(dr["Area"]);
                    lis_BicycleStop.RoadName = Convert.ToString(dr["RoadName"]);
                    lis_BicycleStop.Companyname = Convert.ToString(dr["Companyname"]);
                    lis_BicycleStop.OutDoorMadieName = Convert.ToString(dr["OutDoorMadieName"]);
                    //lis_BicycleStop.SpecificPosition = Convert.ToString(dr["SpecificPosition"]);
                    lis_BicycleStop.OutDoorMadieNumbering = Convert.ToString(dr["OutDoorMadieNumbering"]);
                    lis_BicycleStop.MediaSize = Convert.ToString(dr["MediaSize"]);
                    lis_BicycleStop.Remark = Convert.ToString(dr["Remark"]);
                    lis_BicycleStop.Size = Convert.ToString(dr["Size"]);
                    lis_BicycleStop.PackageNum = Convert.ToString(dr["PackageNum"]);
                    lis_BicycleStop.Lighting = Convert.ToBoolean(dr["Lighting"] == DBNull.Value ? false : dr["Lighting"]);
                    lis_BicycleStop.TblRcdId = Convert.ToInt32(dr["TblRcdId"] == DBNull.Value ? 0 : dr["TblRcdId"]);
                    lis_BicycleStops.Add(lis_BicycleStop);
                }
            }
            result.total = (result.records + pageSize - 1) / pageSize;
            result.rows = lis_BicycleStops;
            return result;
        }
        #endregion

        #region 车辆管理
        /// <summary>
        /// 获取车辆列表数据
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="loginName">登陆名</param>
        /// <returns></returns>
        public SubPageResult<View_PtVehicleReg> GetPtVehicleRegList(PageInfo pager, string loginName)
        {
            SubPageResult<View_PtVehicleReg> result = new SubPageResult<View_PtVehicleReg>();
            result = CommonHelper.ListSubPageResult<View_PtVehicleReg>(pager);
            return result;
        }
        /// <summary>
        /// 获取车型列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetVehicleTypeList()
        {
            List<string> list = CommonHelper.ListDistinctField("tblVehicle_Type", "VehicleTypeName");
            return list ?? new List<string>();
        }

        /// <summary>
        /// 获取线路列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetRoadLineList(string company = "")
        {
            string where = "1=1 ";
            if (!string.IsNullOrWhiteSpace(company))
            {
                where += " and roadName in (select RoadName from tblPtRoad where  PtCompany='" + company + "')";
            }
            List<string> list = CommonHelper.ListDistinctField("tblPtRoadSort", "RoadSortName", where);
            return list ?? new List<string>();
        }

        /// <summary>
        /// 获取行车公司列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetPtCompanyList()
        {
            List<string> list = CommonHelper.ListDistinctField("tblPtCompany", "PtCompany");
            return list ?? new List<string>();
        }

        /// <summary>
        /// 获取线路等级列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetRoadClassList(string roadName = null)
        {
            string where = "1=1 ";
            if (!string.IsNullOrWhiteSpace(roadName))
            {
                where += String.Format(" and roadName in(select roadName from dbo.tblPtRoadSort where roadsortName='{0}')", roadName);
            }
            List<string> list = CommonHelper.ListDistinctField("tblPtRoad", "RoadClass", where);
            return list ?? new List<string>();
        }

        /// <summary>
        /// 获取车身媒体列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetVehicleBodyMediaNameList()
        {
            List<string> list = CommonHelper.ListDistinctField("tblVehicle_Media", "VehicleMediaName", " Remark='车身媒体'");
            return list ?? new List<string>();
        }


        /// <summary>
        /// 获取车内媒体列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetVehicleInnerMediaNameList()
        {
            List<string> list = CommonHelper.ListDistinctField("tblVehicle_Media", "VehicleMediaName", " Remark='车内媒体'");
            return list ?? new List<string>();
        }
        /// <summary>
        /// 报废车辆
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public ReturnMessageModel DiscardAction(List<View_PtVehicleReg> list, string userName)
        {
            ReturnMessageModel RM = new ReturnMessageModel(false);
            List<string> sqlList = new List<string>();
            if (list.Count > 0)
            {
                sqlList.Add("update tblPtVehicleReg Set DiscardAction=1,Reserve=1,State='报废',updateUser='" + userName + "',updateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where TblRcdId in " + DBContext.AssemblyInCondition(list.Select(p => p.TblRcdId).ToList()));

                sqlList.Add("update tblAdFixingList set RoadSortName='报废',updateUser='" + userName + "',updateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where  VehicleNumbering in" + DBContext.AssemblyInCondition(list.Select(p => p.VehicleNumbering).ToList()) + " and InsureEndDate>CONVERT(VARCHAR(10),GETDATE(),120)");


                string sql = @"select *  from VehicleAdInfo_Using 
where MediaName not in (select DISTINCT VehicleMediaName from dbo.tblVehicle_Media where Remark like '%车内媒体%')
 and  InsureEndDate>=convert(VARCHAR(10), getdate() ,120) and FixSure=1 and VehicleNumbering in" + DBContext.AssemblyInCondition(list.Select(p => p.VehicleNumbering).ToList());
                DataTable dt_InPublish = DBContext.PTMMHZ.GetDataTable(sql);


                List<VehicleAdInfo_Using> adPublishList = dt_InPublish.ToList<VehicleAdInfo_Using>();

                foreach (var item in list)
                {

                    var vehPublishList = adPublishList.Where(p => p.VehicleNumbering == item.VehicleNumbering).OrderBy(p => p.VehicleTypeName).ToList();
                    #region 排除相同媒体名称未上刊的
                    if (vehPublishList.Count > 1)
                    {
                        for (int ii = 0; ii < vehPublishList.Count; ii++)
                        {
                            if (vehPublishList[ii].MediaName == vehPublishList[ii + 1].MediaName)
                            {
                                if (vehPublishList[ii].InsureEndDate <= vehPublishList[ii + 1].InsureIssueDate)
                                {
                                    vehPublishList.Remove(vehPublishList[ii + 1]);
                                    adPublishList.Remove(vehPublishList[ii + 1]);
                                }
                                else
                                {
                                    vehPublishList.Remove(vehPublishList[ii]);
                                    adPublishList.Remove(vehPublishList[ii]);
                                }
                            }
                            if (vehPublishList.Count > 1)
                            {
                                ii = 0;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    #endregion
                    #region 循环在刊广告信息
                    foreach (var veh in vehPublishList)
                    {
                        string _saveField = "";
                        string _saveValue = "";
                        _saveField += "VehicleNumbering";
                        _saveValue += "'" + item.VehicleNumbering + "'";
                        if (veh.AdOrderId != null)
                        {
                            if (_saveField != "")
                            {
                                _saveField += ",AdOrderId";
                                _saveValue += ",'" + veh.AdOrderId + "'";
                            }
                            else
                            {
                                _saveField += "AdOrderId";
                                _saveValue += "'" + veh.AdOrderId + "'";

                            }

                            // string AdOrderId = Ad.AdOrderId;
                        }
                        //定单
                        if (veh.AdContent != null)
                        {
                            if (_saveField != "")
                            {
                                _saveField += ",AdContent";
                                _saveValue += ",'" + veh.AdContent + "'";
                            }
                            else
                            {
                                _saveField += "AdContent";
                                _saveValue += "'" + veh.AdContent + "'";

                            }

                            // string AdOrderId = Ad.AdOrderId;
                        }
                        //线路
                        if (veh.RoadSortName != null)
                        {
                            if (_saveField != "")
                            {
                                _saveField += ",RoadSortName";
                                _saveValue += ",'" + veh.RoadSortName + "'";
                            }
                            else
                            {
                                _saveField += "RoadSortName";
                                _saveValue += "'" + veh.RoadSortName + "'";

                            }

                            //   string RoadSortName = Ad.RoadSortName;
                        }
                        //媒体
                        if (veh.MediaName != null)
                        {
                            if (_saveField != "")
                            {
                                _saveField += ",MediaName";
                                _saveValue += ",'" + veh.MediaName + "'";
                            }
                            else
                            {
                                _saveField += "MediaName";
                                _saveValue += "'" + veh.MediaName + "'";

                            }

                            //   string MediaName = veh.MediaName;
                        }
                        //业务员
                        if (veh.Seller != null)
                        {
                            if (_saveField != "")
                            {
                                _saveField += ",Seller";
                                _saveValue += ",'" + veh.Seller + "'";
                            }
                            else
                            {
                                _saveField += "Seller";
                                _saveValue += "'" + veh.Seller + "'";

                            }

                            //      string Seller = Ad.Seller;
                        }
                        //业务员
                        if (veh.InsureEndDate != null)
                        {
                            if (_saveField != "")
                            {
                                _saveField += ",InsureEndDate";
                                _saveValue += ",'" + veh.InsureEndDate + "'";
                            }
                            else
                            {
                                _saveField += "InsureEndDate";
                                _saveValue += "'" + veh.InsureEndDate + "'";

                            }

                            //      string InsureEndDate = Ad.InsureEndDate;
                        }
                        //开始时间
                        if (veh.InsureIssueDate != null)
                        {
                            if (_saveField != "")
                            {
                                _saveField += ",InsureIssueDate";
                                _saveValue += ",'" + veh.InsureIssueDate + "'";
                            }
                            else
                            {
                                _saveField += "InsureIssueDate";
                                _saveValue += "'" + veh.InsureIssueDate + "'";

                            }

                        }
                        //营运公司
                        if (item.RepairCompany != null)
                        {
                            if (_saveField != "")
                            {
                                _saveField += ",OperateCompany";
                                _saveValue += ",'" + item.PtCompany + "'";
                            }
                            else
                            {
                                _saveField += "OperateCompany";
                                _saveValue += "'" + item.PtCompany + "'";

                            }

                            //        string OperateCompany = Ad.OperateCompany;
                        }
                        //修理公司
                        if (item.RoadSortName != null)
                        {
                            if (_saveField != "")
                            {
                                _saveField += ",RepairCompany";
                                _saveValue += ",'" + item.RepairCompany + "'";
                            }
                            else
                            {
                                _saveField += "RepairCompany";
                                _saveValue += "'" + item.RepairCompany + "'";

                            }


                        }
                        //车型
                        if (item.VehicleTypeName != null)
                        {
                            if (_saveField != "")
                            {
                                _saveField += ",VehicleTypeName";
                                _saveValue += ",'" + item.VehicleTypeName + "'";
                            }
                            else
                            {
                                _saveField += "VehicleTypeName";
                                _saveValue += "'" + item.VehicleTypeName + "'";

                            }

                        }

                        _saveField += ",PlanRepairType";
                        _saveValue += ",'报废'";
                        _saveField += ",PlanRepairDate";
                        _saveValue += ",'" + DateTime.Now + "'";
                        sqlList.Add("INSERT INTO  tblVehicleRepair (" + _saveField + ")  Values(" + _saveValue + ")");
                    }
                    #endregion
                }

                RM.IsSuccess = DBContext.PTMMHZ.ExecTrans(sqlList);

                RM.ReturnData = adPublishList;
            }
            else
            {
                RM.Message = "未选择需要报废的车辆";
            }

            return RM;
        }

        /// <summary>
        /// 车辆调线
        /// </summary>
        /// <param name="list"></param>
        /// <param name="user">用户对象</param>
        /// <param name="roadName">新的线路名称</param>
        /// <returns></returns>
        public ReturnMessageModel ChangeVehicleRoad(List<View_PtVehicleReg> list, tblUser_Sys user, string roadName)
        {
            ReturnMessageModel RM = new ReturnMessageModel(false);

            string ssql = string.Format(@"select tblPtRoad.RoadClass from tblPtRoad  left join tblPtRoadSort on tblPtRoad.roadname=tblPtRoadSort.roadname 
where tblPtRoadSort.RoadSortName='{0}'", roadName);

            DataTable dt_newRoadClass = DBContext.PTMMHZ.GetDataTable(ssql);
            string newRoadClass = string.Empty;
            if (dt_newRoadClass != null && dt_newRoadClass.Rows.Count > 0)
            {
                newRoadClass = Convert.ToString(dt_newRoadClass.Rows[0]["RoadClass"]);
            }
            List<int> tblrcdids = list.Select(p => p.TblRcdId).ToList();
            //得到未过期的广告订单以及订单内容
            string sql = "select AdOrderId,AdContent,VehicleId,MediaName,InsureIssueDate,InsureEndDate from tblAdFixingList where InsureEndDate>=Convert(varchar(10),getdate(),120) and VehicleId in " + DBContext.AssemblyInCondition(tblrcdids);
            DataTable dt_havaAdInfo = DBContext.PTMMHZ.GetDataTable(sql);
            //tblVehicleChangeRoad 
            List<string> sqlList = new List<string>();
            //更新车辆线路
            sqlList.Add(string.Format("update tblPtVehicleReg set RoadSortName='{0}',UpdateDate='{2}',UpdateUser='{3}' where  TblRcdId in{1}", roadName, DBContext.AssemblyInCondition(tblrcdids), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.UserName));
            //更新车位媒体未过期的广告车辆线路
            sqlList.Add(string.Format("update tblAdFixingList set RoadSortName='{0}' where  VehicleId in{1} and InsureEndDate>=Convert(varchar(10),getdate(),120)", roadName, DBContext.AssemblyInCondition(tblrcdids)));

            List<tblVehicleChangeRoad> changeList = new List<tblVehicleChangeRoad>();
            #region 循环所有车辆
            foreach (var item in list)
            {
                tblVehicleChangeRoad vehcile = new tblVehicleChangeRoad();
                vehcile.MediaName = item.VehicleBodyMediaName;
                vehcile.RoadSort = item.RoadSortName;//原线路
                vehcile.TagOfRoadSort = roadName;//新的线路
                vehcile.VehicleNumbering = item.VehicleNumbering;
                vehcile.VehicleTypeName = item.VehicleTypeName;
                vehcile.NewVehicleTypeName = item.VehicleTypeName;
                DataRow[] ddr = dt_havaAdInfo.Select("VehicleId=" + item.TblRcdId);
                List<string> adContentList = new List<string>();
                if (ddr != null && ddr.Count() > 0)
                {

                    foreach (DataRow row in ddr)
                    {
                        string adId = Convert.ToString(row["AdOrderId"]);
                        string adContent = Convert.ToString(row["adContent"]);
                        string MediaName = Convert.ToString(row["MediaName"]);
                        string sDateTime = row["InsureIssueDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["InsureIssueDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        string eDateTime = row["InsureEndDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["InsureEndDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        adContentList.Add(string.Format("{0}/{1}/{2}/{3}/{4}", adId, adContent, MediaName, sDateTime, eDateTime));
                    }
                }
                vehcile.RoadClass = item.RoadClass;
                vehcile.NewRoadClass = newRoadClass;
                vehcile.AdContent = string.Join(";", adContentList);
                vehcile.OprMode = "直接调线";
                vehcile.Remark = item.Remark;
                vehcile.ChangeDate = DateTime.Now;
                changeList.Add(vehcile);
                sqlList.Add(string.Format(@"insert into tblVehicleChangeRoad(MediaName,RoadSort,VehicleNumbering,VehicleTypeName,NewVehicleTypeName,AdOrderId,AdContent,OprMode,TagOfRoadSort,ChangeDate,Remark,RoadClass,NewRoadClass)values(
                                                                             '{0}','{1}','{2}', '{3}','{4}','{5}', '{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                                                                           vehcile.MediaName, vehcile.RoadSort, vehcile.VehicleNumbering, vehcile.VehicleTypeName, vehcile.NewVehicleTypeName, vehcile.AdOrderId, vehcile.AdContent, vehcile.OprMode, vehcile.TagOfRoadSort, vehcile.ChangeDate, vehcile.Remark, vehcile.RoadClass, vehcile.NewRoadClass));
            }
            #endregion

            List<View_PtVehicleReg> newList = new List<View_PtVehicleReg>();
            newList = list.Clone() as List<View_PtVehicleReg>;

            foreach (var item in newList)
            {

                item.RoadSortName = roadName;
                item.RoadName = roadName;
                item.RoadClass = newRoadClass;
            }
            if (DBContext.PTMMHZ.ExecTrans(sqlList))
            {
                OperateLogHelper.Edit<View_PtVehicleReg>(list, newList, user);//车辆表更新日志
                OperateLogHelper.Create<tblVehicleChangeRoad>(changeList, user);//车辆调线记录
                RM.IsSuccess = true;
            }
            else
            {
                RM.IsSuccess = false;
            }

            return RM;
        }


        #endregion



        #region 车辆修理
        /// <summary>
        /// 批量执行车辆修理车辆媒体状态
        /// </summary>
        /// <param name="tblRcdIdList"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ReturnMessageModel VehicleRepairState(List<int> tblRcdIdList, VehicleRepairEnum type)
        {
            ReturnMessageModel RM = new ReturnMessageModel(false);
            List<string> sqlList = new List<string>();
            List<tblVehicleRepairList> vehicleRepairList = BusinessContext.tblVehicleRepairList.GetModelList("tblrcdid in " + DBContext.AssemblyInCondition(tblRcdIdList));
            List<string> vehicleStateList = vehicleRepairList.Select(p => p.ProductionState).ToList();
            if (vehicleStateList.Contains("终止发布"))
            {
                RM.Message = "当前状态含有终止发布,不可修改";
            }
            else if (vehicleStateList.Contains("已下单"))
            {
                RM.Message = "当前状态含有已下单,不可修改";
            }
            else if (vehicleStateList.Contains("不重新制作"))
            {
                RM.Message = "当前状态含有不重新制作,不可修改";
            }
            else
            {
                int mainkey = vehicleRepairList.FirstOrDefault().MainKey;
                int rows = BusinessContext.tblVehicleRepairList.GetModelList("mainkey =" + mainkey).Count;
                #region 枚举类型
                switch (type)
                {

                    case VehicleRepairEnum.StopPublish:
                        {
                            string sql = "update dbo.tblVehicleRepairlist  set InsureEndDate=getdate(),ProductionState='终止发布' where TblRcdId in" + DBContext.AssemblyInCondition(tblRcdIdList);
                            sqlList.Add(sql);
                            if (rows == 1)
                            {
                                sql = "update dbo.tblVehicleRepair  set DataState='历史',ProductionState='终止发布' where TblRcdId=" + mainkey;
                                sqlList.Add(sql);
                            }

                        } break;
                    case VehicleRepairEnum.UnderOrder:
                        {
                            string sql = "update dbo.tblVehicleRepairlist  set ProductionState='已下单' where TblRcdId in" + DBContext.AssemblyInCondition(tblRcdIdList);
                            sqlList.Add(sql);
                            if (rows == 1)
                            {
                                sql = "update dbo.tblVehicleRepair  set DataState='历史',ProductionState='已下单' where TblRcdId=" + mainkey;
                                sqlList.Add(sql);
                            }
                        } break;
                    case VehicleRepairEnum.NotReMake:
                        {
                            string sql = "update dbo.tblVehicleRepairlist  set ProductionState='不重新制作' where TblRcdId in" + DBContext.AssemblyInCondition(tblRcdIdList);
                            sqlList.Add(sql);
                            if (rows == 1)
                            {
                                sql = "update dbo.tblVehicleRepair  set DataState='历史',ProductionState='不重新制作' where TblRcdId=" + mainkey;
                                sqlList.Add(sql);
                            }
                        } break;
                }
                #endregion
                List<int> adFixingList = vehicleRepairList.Select(p => p.AdFixingId).ToList();
                if (type == VehicleRepairEnum.UnderOrder)
                {
                    string sql = "update  dbo.tblAdFixingList  set  State=1 where TblRcdId in" + DBContext.AssemblyInCondition(adFixingList);
                    sqlList.Add(sql);
                }
                else if (type == VehicleRepairEnum.StopPublish)
                {
                    string sql = "update  dbo.tblAdFixingList   set InsureEndDate=getdate() where TblRcdId in" + DBContext.AssemblyInCondition(adFixingList);
                    sqlList.Add(sql);
                }

                if (DBContext.PTMMHZ.ExecTrans(sqlList))
                {
                    RM.IsSuccess = true;
                }
            }
            return RM;
        }

        /// <summary>
        /// 车辆修理通知
        /// </summary>
        /// <param name="tblRcdIdList"></param>
        /// <param name="userName">发送人</param>
        /// <returns></returns>
        public ReturnMessageModel VehicleRepairNotice(List<int> tblRcdIdList, string userName)
        {
            ReturnMessageModel RM = new ReturnMessageModel(false);
            if (tblRcdIdList.Count == 0)
            {
                RM.Message = "未选择操作数据";
            }
            else
            {

                List<tblVehicleRepair> listRepair = BusinessContext.tblVehicleRepair.GetModelList("Informed=1 and tblrcdid in" + DBContext.AssemblyInCondition(tblRcdIdList));

                if (listRepair.Count > 0)
                {
                    RM.Message = "存在已修理通知的数据,无法重复修理通知";
                }
                else
                {
                    List<string> sqlList = new List<string>();
                    List<tblVehicleRepairList> list = BusinessContext.tblVehicleRepairList.GetModelList("mainkey in " + DBContext.AssemblyInCondition(tblRcdIdList));
                    foreach (var item in list)
                    {
                        string Opinion = item.VehicleNumbering + "," + item.MediaName + "," + item.RoadSortName + "," + item.AdContent;
                        string sql = "insert INTO tblMessage (Sender,Accepter,MsgType,MsgTitle,MsgContent,SendDate,AcceptDate,AdOrderId) values ('" + userName + "','" + item.Seller + "','车辆修理','车辆修理通知','" + Opinion + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + item.AdOrderId + "')";
                        sqlList.Add(sql);
                    }

                    string strSql = "update  tblVehicleRepair set InformedDate=GETDATE(),Informed=1 where TblRcdId in" + DBContext.AssemblyInCondition(tblRcdIdList);
                    sqlList.Add(strSql);
                    if (DBContext.PTMMHZ.ExecTrans(sqlList))
                    {
                        RM.IsSuccess = true;
                    }
                }


            }
            return RM;
        }

        /// <summary>
        /// 车身广告复原
        /// </summary>
        /// <param name="tblRcdIdList"></param>
        /// <param name="userName">发送人</param>
        /// <returns></returns>
        public ReturnMessageModel VehicleRestore(List<int> tblRcdIdList, string userName)
        {
            ReturnMessageModel RM = new ReturnMessageModel(false);
            if (tblRcdIdList.Count == 0)
            {
                RM.Message = "未选择操作数据";
            }
            else
            {
                List<tblVehicleRepair> vehicleRepairList = BusinessContext.tblVehicleRepair.GetModelList("tblrcdid in " + DBContext.AssemblyInCondition(tblRcdIdList));
                List<int> vehIdList = vehicleRepairList.Where(p => p.VehicleId > 0).Select(p => p.VehicleId).ToList();

                string sql = "select *  from tblAdFixinglist where (CoveredFlag<>1 or CoveredFlag is null) and InsureEndDate<=CONVERT(VARCHAR(10),GETDATE(),120)  and VehicleId in" + DBContext.AssemblyInCondition(vehIdList);
                DataTable dt = DBContext.PTMMHZ.GetDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    RM.Message = "无需复原";
                }
                else
                {
                    List<string> sqlList = new List<string>();
                    sqlList.Add("Update tblAdFixinglist set CoveredFlag=1,CoveredDate='" + DateTime.Now.ToShortDateString() + "' where (CoveredFlag<>1 or CoveredFlag is null) and InsureEndDate<=CONVERT(VARCHAR(10),GETDATE(),120)  and VehicleId in" + DBContext.AssemblyInCondition(vehIdList));
                    sqlList.Add("delete tblExpireVehicle  where   mainkey in" + DBContext.AssemblyInCondition(tblRcdIdList));
                    if (DBContext.PTMMHZ.ExecTrans(sqlList))
                    {
                        RM.IsSuccess = true;
                    }
                }

            }
            return RM;
        }


        /// <summary>
        /// 户外广告复原
        /// </summary>
        /// <param name="tblRcdIdList"></param>
        /// <param name="userName">发送人</param>
        /// <returns></returns>
        public ReturnMessageModel OutDoorAdRestore(List<int> tblRcdIdList, string userName)
        {
            ReturnMessageModel RM = new ReturnMessageModel(false);
            if (tblRcdIdList.Count == 0)
            {
                RM.Message = "未选择操作数据";
            }
            else
            {
                List<tblOutDoor_Media> outDoorList = BusinessContext.tblOutDoor_Media.GetModelList("tblrcdid in " + DBContext.AssemblyInCondition(tblRcdIdList));
                List<string> outDoorMediaNumList = outDoorList.Select(p => p.OutDoorMadieNumbering).Distinct().ToList();

                string sql = "select CoveredFlag  from tblOutdoorAdOrderMedia where mediaState=3 and (CoveredFlag<>1 or CoveredFlag is null) and EndDate<=CONVERT(VARCHAR(10),GETDATE(),120)  and OutDoorMediaNumbering in" + DBContext.AssemblyInCondition(outDoorMediaNumList);
                DataTable dt = DBContext.PTMMHZ.GetDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    RM.Message = "无需复原";
                }
                else
                {
                   
                    List<string> sqlList = new List<string>();
                    sqlList.Add("Update tblOutdoorAdOrderMedia set CoveredFlag=1,CoveredDate='" + DateTime.Now.ToShortDateString() + "' where mediaState=3 and (CoveredFlag<>1 or CoveredFlag is null) and EndDate<=CONVERT(VARCHAR(10),GETDATE(),120)  and OutDoorMediaNumbering in" + DBContext.AssemblyInCondition(outDoorMediaNumList));
                   
                    if (DBContext.PTMMHZ.ExecTrans(sqlList))
                    {
                        RM.IsSuccess = true;
                    }
                }

            }
            return RM;
        }

        /// <summary>
        /// 广告更新
        /// </summary>
        /// <param name="tblRcdIdList"></param>
        /// <param name="userName">发送人</param>
        /// <returns></returns>
        public ReturnMessageModel VehicleMediaRefresh(List<int> tblRcdIdList, string userName)
        {
            ReturnMessageModel RM = new ReturnMessageModel(true);
            if (tblRcdIdList.Count == 0)
            {
                RM.Message = "未选择操作数据";
            }
            else
            {
                List<tblVehicleRepair> vehicleRepairList = BusinessContext.tblVehicleRepair.GetModelList("tblrcdid in " + DBContext.AssemblyInCondition(tblRcdIdList));
                List<int> vehIdList = vehicleRepairList.Where(p => p.VehicleId > 0).Select(p => p.VehicleId).ToList();

                List<string> sqlList = new List<string>();
                sqlList.Add("delete from tblExpireVehicle where Mainkey in" + DBContext.AssemblyInCondition(tblRcdIdList));
                sqlList.Add("delete from tblVehicleRepairList where Mainkey in" + DBContext.AssemblyInCondition(tblRcdIdList));
                //获取未过期的车位媒体信息
                List<tblAdFixingList> adAllFixList = BusinessContext.tblAdFixingList.GetModelList("VehicleId in " + DBContext.AssemblyInCondition(vehIdList) + "  and InsureIssueDate<=getdate() and InsureEndDate>=getdate()");
                List<string> listAdOrderId = adAllFixList.GroupBy(p => p.AdOrderId).Select(p => p.Key).ToList();
                

                List<tblAdFixingList> adExpireFixList = BusinessContext.tblAdFixingList.GetModelList("(CoveredFlag=0 or  CoveredFlag is null) and VehicleId in " + DBContext.AssemblyInCondition(vehIdList) + "  and  InsureEndDate<getdate()");
                listAdOrderId.AddRange(adExpireFixList.Select(p=>p.AdOrderId).ToList());
                List<tblAdOrder> listAdOrder = new List<tblAdOrder>();
                if (listAdOrderId.Count > 0)
                {
                    listAdOrder=BusinessContext.tblAdOrder.GetModelList("AdOrderId in " + DBContext.AssemblyInCondition(listAdOrderId));
                }
                foreach (var model in vehicleRepairList)
                {
                    List<tblAdFixingList> adFixList = adAllFixList.Where(p => p.VehicleId == model.VehicleId).ToList();
                    #region 未过期的广告
                    foreach (var item in adFixList)
                    {
                        int RcdId = item.TblRcdId;
                        string seller = string.Empty;
                        tblAdOrder adOrder = listAdOrder.Where(p => p.AdOrderId == item.AdOrderId).FirstOrDefault();
                        if (adOrder != null)
                        {
                            seller = adOrder.Seller;
                        }
                        int print = 0;
                        if (item.PrintSample)
                        {
                            print = 1;
                        }
                        string strSql = string.Format("update tblAdFixingList  set State=1 where TblRcdId=" + RcdId + "");
                        sqlList.Add(strSql);

                        strSql = string.Format("Insert into dbo.tblVehicleRepairList(MainKey,AdorderId,AdContent,MEDIANAME,ROADSORTNAME,VEHICLETYPENAME,VehicleNumbering,InsureIssueDate,InsureEndDate,Remark,MeidaNum,MakeState,StickersDate,MakeMode,SampleSure,PrintSample,CustomProvideInfo,FixDate,Seller,AdFixingId,VehicleId) values( "
                                                + @"{0},'{1}','{2}','{3}','{4}','{5}','{6}',
                                        " + (item.InsureIssueDate == null ? "null" : ("'" + item.InsureIssueDate.Value.ToString("yyyy-MM-dd") + "'"))
                                                 + @"," + (item.InsureEndDate == null ? "null" : ("'" + item.InsureEndDate.ToString("yyyy-MM-dd") + "'"))
                                                 + @",'{9}',{10},'{11}',
                                        " + (item.StickersDate == null ? "null" : ("'" + item.StickersDate.Value.ToString("yyyy-MM-dd") + "'"))
                                                 + @",'{13}','{14}','{15}','{16}'," +
                                                 (item.FixDate == null ? "null" : ("'" + item.FixDate.Value.ToString("yyyy-MM-dd") + "'"))
                                                 + ",'{18}',{19},{20})",
                                                 model.TblRcdId, item.AdOrderId, item.AdContent, item.MediaName, item.RoadSortName, item.VehicleTypeName, item.VehicleNumbering,
                                                 item.InsureIssueDate, item.InsureEndDate, item.Remark, item.MeidaNum, item.MakeState,
                                                 item.StickersDate, item.MakeMode, item.SampleSure, print, item.CustomProvideInfo, item.FixDate, seller, item.TblRcdId, item.VehicleId);
                        sqlList.Add(strSql);
                    }
                    #endregion

                    List<tblAdFixingList> adexpList = adExpireFixList.Where(p => p.VehicleId == model.VehicleId).ToList();
                    #region 过期广告
                    foreach (var item in adexpList)
                    {
                        int RcdId = item.TblRcdId;
                        string seller = string.Empty;
                        tblAdOrder adOrder = listAdOrder.Where(p => p.AdOrderId == item.AdOrderId).FirstOrDefault();
                        if (adOrder != null)
                        {
                            seller = adOrder.Seller;
                        }
                        int print = 0;
                        if (item.PrintSample)
                        {
                            print = 1;
                        }


                        string strSql = string.Format("Insert into dbo.tblExpireVehicle(MainKey,AdorderId,AdContent,MEDIANAME,ROADSORTNAME,VEHICLETYPENAME,VehicleNumbering,InsureIssueDate,InsureEndDate,Remark,MeidaNum,FixDate,Seller) values( "
                                                + @"{0},'{1}','{2}','{3}','{4}','{5}','{6}',
                                        " + (item.InsureIssueDate == null ? "null" : ("'" + item.InsureIssueDate.Value.ToString("yyyy-MM-dd") + "'"))
                                                 + @"," + (item.InsureEndDate == null ? "null" : ("'" + item.InsureEndDate.ToString("yyyy-MM-dd") + "'"))
                                                 + @",'{9}',{10}"
                                                 + @"," +
                                                 (item.FixDate == null ? "null" : ("'" + item.FixDate.Value.ToString("yyyy-MM-dd") + "'"))
                                                 + ",'{12}')",
                                                 model.TblRcdId, item.AdOrderId, item.AdContent, item.MediaName, item.RoadSortName, item.VehicleTypeName, item.VehicleNumbering,
                                                 item.InsureIssueDate, item.InsureEndDate, item.Remark, item.MeidaNum, item.FixDate, seller);
                        sqlList.Add(strSql);
                    }
                    #endregion
                }

                if (!DBContext.PTMMHZ.ExecTrans(sqlList))
                {
                    RM.IsSuccess = false;
                    RM.Message = "数据异常";
                }
            }
            return RM;
        }




        #endregion


    }
    #endregion
}
