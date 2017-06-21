using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;

namespace Bitshare.PTMM.Service.Impl
{
     public   class AdOrderFundListServiceFactory
    {
         static IAdOrderFundListService Instance;
         public IAdOrderFundListService GetInstance()
         {
             if (Instance == null)
             {
                 Instance = new AdOrderFundListService();
             }
             return Instance;
         }
         
    }

     internal class AdOrderFundListService : IAdOrderFundListService
     {
         /// <summary>
         /// 获取款项收付款明细列表
         /// </summary>
         /// <param name="pager">分页</param>
         /// <param name="loginName">登陆名</param>
         /// <returns></returns>
         public SubPageResult<View_FinanceDetails> GetAdOrderFundList(PageInfo pager, string loginName)
         {
             SubPageResult<View_FinanceDetails> result = new SubPageResult<View_FinanceDetails>();
             result = CommonHelper.ListSubPageResult<View_FinanceDetails>(pager);
             return result;
         }
     }
}
