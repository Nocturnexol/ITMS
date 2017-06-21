using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;

namespace Bitshare.PTMM.Service.Impl
{
    #region 工厂定义
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class MediaQueryServiceFactory
    {
        static IMediaQueryService Instance;
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public IMediaQueryService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new MediaQueryService();
            }
            return Instance;
        }
    }

    #endregion

    internal class MediaQueryService:IMediaQueryService
    {

        /// <summary>
        /// 获取车辆媒体广告查询列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public SubPageResult<View_VehicleMediaQuery> GetView_VehicleAdList(PageInfo pager, string loginName)
        {
            SubPageResult<View_VehicleMediaQuery> result = new SubPageResult<View_VehicleMediaQuery>();
            result = CommonHelper.ListSubPageResult<View_VehicleMediaQuery>(pager);
            return result;
        }



        /// <summary>
        /// 获取户外媒体广告查询列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public SubPageResult<View_OutDoorMediaQuery> GetView_OutDoorMediaAdList(PageInfo pager, string loginName)
        {
            SubPageResult<View_OutDoorMediaQuery> result = new SubPageResult<View_OutDoorMediaQuery>();
            result = CommonHelper.ListSubPageResult<View_OutDoorMediaQuery>(pager);
            return result;
        }


        /// <summary>
        /// 获取自行车广告媒体查询列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public SubPageResult<View_BicycleMediaQuery> GetView_BicycleMediaAdList(PageInfo pager, string loginName)
        {
            SubPageResult<View_BicycleMediaQuery> result = new SubPageResult<View_BicycleMediaQuery>();
            result = CommonHelper.ListSubPageResult<View_BicycleMediaQuery>(pager);
            return result;
        }
    
    }
}
