using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Service.Impl;
using Bitshare.PTMM.Service;
using System.Data;
using Bitshare.PTMM.Model;
using Bitshare.PTMM.Service.Enum;

namespace Bitshare.PTMM.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMediaService
    {
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
        PageData GetVehicle_MediaPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);
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
        PageData GetExcludedMediaPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);
        #endregion
        #region 线路
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
        PageData GetPtRoadPathPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);
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
        PageData GetPtPassRegionPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);
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
        PageData GetPtPlatFormPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);
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
        PageData GetPtRoadSortPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);
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
        PageData GetPtRoadPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);
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
        PageData GetSelectVehiclePhotoPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);
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
        PageData GetBicycleStopInfoPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string sWhere2 = " 1=1 ");
        /// <summary>
        /// 分页获取自行车站点统计列表统计数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="StopId">查询条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        PageData GetBicycleListPageListResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string StopId = null, string sWhere = null);
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
        PageData GetStopListPageListResult(string loginName = null, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string StopId = null, string sWhere = null);
             /// <summary>
        /// 导出
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sWhere"></param>
        /// <param name="sWhere2"></param>
        /// <returns></returns>
        DataTable GetExportDataBicycleMedia(int pageIndex = 1, int pageSize = 50, string sWhere = null, string sWhere2 = null);
        #endregion
        #region 站点统计
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
        PageData GetStopInfoPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string MediaName = "");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        DataTable GetExportDataOutMedia(int pageIndex = 1, int pageSize = 50, string sWhere = null, string sWhere1 = null, string MediaName = "");
        #endregion

        #region 车辆管理
        /// <summary>
        /// 获取车辆列表数据
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="loginName">登陆名</param>
        /// <returns></returns>
        SubPageResult<View_PtVehicleReg> GetPtVehicleRegList(PageInfo pager, string loginName);
        /// <summary>
        /// 获取车型列表
        /// </summary>
        /// <returns></returns>
        List<string> GetVehicleTypeList();

        /// <summary>
        /// 获取线路列表
        /// </summary>
        /// <returns></returns>
        List<string> GetRoadLineList(string company = "");

        /// <summary>
        /// 获取行车公司列表
        /// </summary>
        /// <returns></returns>
        List<string> GetPtCompanyList();

        /// <summary>
        /// 获取线路等级列表
        /// </summary>
        /// <returns></returns>
        List<string> GetRoadClassList(string roadName = null);

        /// <summary>
        /// 获取车身媒体列表
        /// </summary>
        /// <returns></returns>
        List<string> GetVehicleBodyMediaNameList();


        /// <summary>
        /// 获取车内媒体列表
        /// </summary>
        /// <returns></returns>
        List<string> GetVehicleInnerMediaNameList();
        /// <summary>
        /// 报废车辆
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        ReturnMessageModel DiscardAction(List<View_PtVehicleReg> list, string userName);
        /// <summary>
        /// 车辆调线
        /// </summary>
        /// <param name="list"></param>
        /// <param name="user">用户对象</param>
        /// <param name="roadName">新的线路名称</param>
        /// <returns></returns>
        ReturnMessageModel ChangeVehicleRoad(List<View_PtVehicleReg> list, tblUser_Sys user, string roadName);


        #endregion

        #region 车辆修理
        /// <summary>
        /// 批量执行车辆修理车辆媒体状态
        /// </summary>
        /// <param name="tblRcdIdList"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ReturnMessageModel VehicleRepairState(List<int> tblRcdIdList, VehicleRepairEnum type);

        /// <summary>
        /// 车辆修理通知
        /// </summary>
        /// <param name="tblRcdIdList"></param>
        /// <param name="userName">发送人</param>
        /// <returns></returns>
        ReturnMessageModel VehicleRepairNotice(List<int> tblRcdIdList, string userName);

         /// <summary>
        /// 车身广告复原
        /// </summary>
        /// <param name="tblRcdIdList"></param>
        /// <param name="userName">发送人</param>
        /// <returns></returns>
        ReturnMessageModel VehicleRestore(List<int> tblRcdIdList, string userName);

         /// <summary>
        /// 广告更新
        /// </summary>
        /// <param name="tblRcdIdList"></param>
        /// <param name="userName">发送人</param>
        /// <returns></returns>
        ReturnMessageModel VehicleMediaRefresh(List<int> tblRcdIdList, string userName);   
        /// <summary>
        /// 户外广告复原
        /// </summary>
        /// <param name="tblRcdIdList"></param>
        /// <param name="userName">发送人</param>
        /// <returns></returns>
        ReturnMessageModel OutDoorAdRestore(List<int> tblRcdIdList, string userName);

        #endregion

    }
}
