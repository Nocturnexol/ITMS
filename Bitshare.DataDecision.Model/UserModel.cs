using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bitshare.DataDecision.Model
{
    /// <summary>
    /// 用户对象
    /// </summary>
    public class UserModel
    {
        public tblUser_Sys User { get; set; }
        public List<sys_role> Roles { get; set; }
    }
}
