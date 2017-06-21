using Bitshare.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// 数据库访问上下文
    /// </summary>
    public class DBContext
    {
        /// <summary>
        /// DataDecision_Bus数据库访问对象
        /// </summary>
        public static SQLDBVisitor DataDecision { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        static DBContext()
        {
            InitConfig();
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void InitConfig()
        {
            try
            {
                string connstr_DataDecision = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                DataDecision = new SQLDBVisitor(connstr_DataDecision);
            }
            catch (Exception ex)
            {
                LogManager.Error("SQLDBContext()", ex);
            }
        }

        /// <summary>
        /// 组合SQL语句用于 in 条件的查询,如：('a','b',...)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string AssemblyInCondition(List<string> values)
        {
            string str = string.Empty;
            if (values != null && values.Count > 0)
            {
                // 过滤空值和重复项
                var list = values.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().Select(p => "'" + p + "'").ToList();
                if (list.Count > 0)
                {
                    str = "(" + string.Join(",", list) + ")";
                }
            }
            return str;
        }

        /// <summary>
        /// 组合SQL语句用于 in 条件的查询,如(a,b,...)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string AssemblyInCondition(List<int> values)
        {
           
            
            string str = string.Empty;
            if (values != null && values.Count > 0)
            {
                str = "(" + string.Join(",", values.Distinct()) + ")";
            }
            return str;
        }
    }
}