using Bitshare.DataDecision.DBUtility;
using Bitshare.DBUtility;

namespace Bitshare.DataDecision.DAL
{
    public class BaseDAL
    {
        /// <summary>
        /// 执行事务操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ExecTransaction(TransactionInfo model)
        {
            return DbHelperSQL.ExecuteSqlTran(model.CmdList) > 0;
        }

    }
}
