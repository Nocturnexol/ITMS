using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;

namespace Bitshare.PTMM.Service
{
    public interface  IAdDesignService
    {
        #region 明细
        /// <summary>
        /// 详细页面明细数据获取
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">条数</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序</param>
        /// <param name="sWhere">条件</param>
        /// <param name="type">判断是哪一个明细</param>
        /// <returns></returns>
        PageResult GetAllPageResult(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string type = null);

        #endregion


        List<string> GetAdDesignList(string loginName, string authority);


        /// <summary>
        /// 根据传递的订单号获取订单信息
        /// </summary>
        /// <param name="adOrderId">订单id:yct-2015-4-29-1</param>
        /// <returns>eg：tblAdOrder</returns>
        tblAdOrder GetTblAdOrder(string adOrderId);



    }
}
