using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;

namespace Bitshare.PTMM.Service
{
  public   interface IAdOrderFundListService
    {
      /// <summary>
      /// 获取款项收付款明细列表
      /// </summary>
      /// <param name="pager">分页</param>
      /// <param name="loginName">登录名</param>
      /// <returns></returns>
      SubPageResult<View_FinanceDetails> GetAdOrderFundList(PageInfo pager, string loginName);
    }
}
