using Bitshare.DataDecision.DBUtility;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Bitshare.DBUtility
{
    public class TransactionInfo
    {
        public List<CommandInfo> CmdList;

        public TransactionInfo()
        {
            CmdList = new List<CommandInfo>();
        }

        public void AddCmd(string sql, SqlParameter[] parameters)
        {
            CmdList.Add(new CommandInfo(sql, parameters));
        }
    }
}
