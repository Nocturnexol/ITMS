using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;

namespace Bitshare.PTMM.Service
{
  public   interface IMediaQueryService
    {
      /// <summary>
      /// 获取车辆广告查询列表
      /// </summary>
      /// <param name="pager">分页</param>
      /// <param name="loginName">登录名</param>
      /// <returns></returns>
      SubPageResult<View_VehicleMediaQuery> GetView_VehicleAdList(PageInfo pager, string loginName);

      /// <summary>
      /// 获取户外媒体广告查询列表
      /// </summary>
      /// <param name="pager">分页</param>
      /// <param name="loginName">登录名</param>
      /// <returns></returns>
      SubPageResult<View_OutDoorMediaQuery> GetView_OutDoorMediaAdList(PageInfo pager, string loginName);

      /// <summary>
      /// 获取自行车广告查询列表
      /// </summary>
      /// <param name="pager">分页</param>
      /// <param name="loginName">登录名</param>
      /// <returns></returns>
      SubPageResult<View_BicycleMediaQuery> GetView_BicycleMediaAdList(PageInfo pager, string loginName);

    }
}
