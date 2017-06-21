using Bitshare.Common;
using Bitshare.DataDecision.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Bitshare.DataDecision.BLL;
using MongoDB.Driver.Builders;
using FunctionalAuthority = Bitshare.DataDecision.Model.FunctionalAuthority;
using tblUser_Sys = Bitshare.DataDecision.Model.tblUser_Sys;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// 流程处理相关帮助类
    /// </summary>
    public static class FlowHelper
    {
        public static List<SelectListItem> BasicTypeList { get; set; }

        public static void InitBasicType()
        {
            var typeArr = ConfigurationManager.AppSettings["BasicTypes"].Split(new[] { ',' },
                StringSplitOptions.RemoveEmptyEntries);
            BasicTypeList = typeArr.Select((t, i) => new SelectListItem
            {
                Text = t,
                Value = (i + 1).ToString()
            }).ToList();
        }
        public static IList<SelectListItem> GetTypeSelectList(string typeName)
        {
            if (!BasicTypeList.Select(t => t.Text).Contains(typeName))
            {
                throw new ArgumentException("无效的类型名称");
            }
            var type = BasicTypeList.First(t => t.Text == typeName);
            long count;
            var list = new MongoBll<BasicType>().GetList(out count, 1, 50);
            return list.Where(t => t.TypeId == int.Parse(type.Value)).Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Num.ToString()
            }).ToList();
        }

        public static IList<SelectListItem> GetSelectListFromController<T>(string method, params object[] param)
            where T : Controller
        {
            var service = DependencyResolver.Current.GetService<T>();
            var res = service.GetType().GetMethod(method).Invoke(service, param);
            return res as IList<SelectListItem>;
        }

        #region 获取当前用户的权限(数据权限/业务权限)
        /// <summary>
        /// 获取当前用的的数据权限
        /// </summary>
        /// <returns></returns>
        public static List<FunctionalAuthority> GetFunctionalAuthoirty()
        {

            List<FunctionalAuthority> list = new List<FunctionalAuthority>();
            try
            {
                List<int> dic_roleId = CurrentHelper.CurrentUser.Roles.Select(p => p.Rid).ToList();
                var q = Query.And(Query<Model.sys_role_right>.In(t => t.rf_Role_Id, dic_roleId),
                    Query<Model.sys_role_right>.EQ(t => t.rf_Type, "数据管理"));
                List<int> rigthCodeList =
                    BusinessContext.sys_role_right.GetList(q).Select(p => p.rf_Right_Code).ToList();
                if (rigthCodeList != null && rigthCodeList.Count > 0)
                {
                    List<int> dic_funId = BusinessContext.tblGroupButton.GetList(Query<Model.tblGroupButton>.In(t => t.Rid, rigthCodeList)).Select(p => p.Group_NameId).ToList();
                    list = BusinessContext.FunctionalAuthority.GetList(Query<FunctionalAuthority>.In(t=>t.Rid,dic_funId));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("GetFunctionalAuthoirty", ex);
            }
            return list;
        }


        /// <summary>
        /// 获取当前用户页面按钮权限
        /// </summary>
        /// <param name="Func">页面功能名</param>
        /// <returns></returns>
        public static List<string> GetBtnAuthorityForPage(string Func)
        {
            List<string> list = new List<string>();
            try
            {
                List<int> list_roleId = CurrentHelper.CurrentUser.Roles.Select(p => p.Rid).ToList();
                // 获取业务权限对象
                List<int> Ridlist = BusinessContext.FunctionalAuthority.GetList(Query<FunctionalAuthority>.EQ(t => t.Right_Name, Func)).Select(p => p.Rid).ToList();

                // 获取用户所有的数据权限编码列表
                List<int> rigthCodeList = BusinessContext.sys_role_right.GetList(Query.And(Query<Model.sys_role_right>.In(t => t.rf_Role_Id, list_roleId),
                    Query<Model.sys_role_right>.EQ(t => t.rf_Type, "数据管理"))).Select(p => p.rf_Right_Code).Distinct().ToList();

                if (Ridlist.Count > 0 && rigthCodeList.Count > 0)
                {
                    //list = BusinessContext.View_GroupButtonInfo.GetModelList("Group_NameId in " + DBContext.AssemblyInCondition(Ridlist) + " and Rid in " + DBContext.AssemblyInCondition(rigthCodeList)).Select(p => p.ButtonName).ToList();
                    list = (from gb in BusinessContext.tblGroupButton.GetList()
                        join bn in BusinessContext.tblButtonName.GetList() on gb.ButtonNameId equals bn.Rid into g
                        from a in g.DefaultIfEmpty()
                        join f in BusinessContext.FunctionalAuthority.GetList() on gb.Group_NameId equals f.Rid into gg
                        from aa in gg.DefaultIfEmpty()
                        where Ridlist.Contains(gb.Group_NameId)&&rigthCodeList.Contains(gb.Rid)
                        select a.ButtonName).ToList();


                }
            }
            catch (Exception ex)
            {
                LogManager.Error("GetBtnAuthorityForPage", ex);
            }
            return list ?? new List<string>();
        }

        /// <summary>
        /// 获取当前用户页面按钮权限
        /// </summary>
        /// <param name="Module_Name">模块名称</param>
        /// <param name="Func">页面功能名</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetBtnAuthorityForPageList(string Module_Name, string Func)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            List<string> list = new List<string>();
            try
            {
                List<int> list_roleId = CurrentHelper.CurrentUser.Roles.Select(p => p.Rid).ToList();
                List<int> rigthCodeList = BusinessContext.sys_role_Detail.GetModelList("rf_Type='数据管理' and rf_Role_Id in " + DBContext.AssemblyInCondition(list_roleId)).Select(p => p.rf_Right_Code.Value).Distinct().ToList();
                //db.sys_role_Detail.Where(p => list_roleId.Contains(p.rf_Role_Id.Value) && p.rf_Type == "数据管理").Select(p => p.rf_Right_Code.Value).Distinct().ToList();
                List<string> funObj = BusinessContext.tblPageDetail.GetModelList("PageName='" + Func + "' and ModelName='" + Module_Name + "'").Select(p => p.DetailName).ToList();
                //db.tblPageDetail.Where(p => p.PageName == Func && p.ModelName == Module_Name).Select(p => p.DetailName).ToList();
                for (int i = 0; i < funObj.Count; i++)
                {
                    string Name = Convert.ToString(funObj[i]);
                    list = BusinessContext.View_DetailButtonNew.GetModelList("DetailName='" + Name + "' and ModelName='" + Module_Name + "' and PageName='" + Func + "' and Rid in " + DBContext.AssemblyInCondition(rigthCodeList)).Select(p => p.ButtonName).ToList();
                    //db.View_DetailButtonNew.Where(p => p.DetailName == Name && p.ModelName == Module_Name && p.PageName == Func && rigthCodeList.Contains(p.Rid)).Select(p => p.ButtonName).ToList();
                    dic.Add(Name, list);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("GetBtnAuthorityForPageList", ex);
            }
            return dic;
        }
        #endregion
       
        /// <summary>
        /// 从数据表获取绑定下拉列表
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <param name="field">下拉字段</param>
        /// <param name="where">查询条件</param>
        /// <param name="selectValue">选中项</param>
        /// <param name="orderBy">查询排序条件</param>
        /// <returns></returns>
        public static List<SelectListItem> GetSelectList(string tableName, string field, string where = null, string selectValue = null, string orderBy = null)
        {
            List<string> list = DBContext.DataDecision.QueryOrderBy(tableName, field, where, orderBy);
            List<SelectListItem> items = list.Distinct().Select(p => new SelectListItem { Text = p, Value = p, Selected = (p == selectValue) }).ToList();
            items.Insert(0, new SelectListItem { Text = "-请选择-", Value = "" });
            return items;
        }
        /// <summary>
        /// 从数据表获取绑定下拉列表
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <param name="field">下拉字段</param>
        /// <param name="where">查询条件</param>
        /// <param name="selectValue">选中项</param>
        /// <param name="orderBy">查询排序条件</param>
        /// <returns></returns>
        public static List<SelectListItem> GetSelectList2(string tableName, string field, string where = null, string selectValue = null, string orderBy = null)
        {
            List<string> list = DBContext.DataDecision.QueryOrderBy(tableName, field, where, orderBy);
            List<SelectListItem> items = list.Distinct().Select(p => new SelectListItem { Text = p, Value = p, Selected = (p == selectValue) }).ToList();
            return items;
        }
        /// <summary>
        /// 从数据表获取绑定下拉列表
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <param name="field">下拉字段</param>
        /// <param name="where">查询条件</param>
        /// <param name="selectValue">选中项</param>
        /// <param name="orderBy">查询排序条件</param>
        /// <returns></returns>
        public static List<SelectListItem> GetSelectListOnlyData(string tableName, string field, string where = null, string selectValue = null, string orderBy = null)
        {
            List<string> list = DBContext.DataDecision.QueryOrderBy(tableName, field, where, orderBy);
            List<SelectListItem> items = list.Distinct().Select(p => new SelectListItem { Text = p, Value = p, Selected = (p == selectValue) }).ToList();
            return items;
        }
        
        /// <summary>
        /// 单表或试图,获取分页数据并返回JQ [add by th]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <returns></returns>
        public static jqGridData GetJqGridDataList<T>(string where = "", string orderBy = "", int page = 1, int rows = 50, string sidx = "", string sord = "asc") where T : new()
        {
            jqGridData RM = new jqGridData();

            if (!string.IsNullOrWhiteSpace(sidx))
            {
                orderBy = sidx + " " + sord;
            }
            int totalCount = 0;
            string queryFields = "*";
            int CurrentPageIndex = (page != 0 ? (int)page : 1);
            DataTable ds = DBContext.DataDecision.QueryPageByProc(typeof(T).Name, orderBy, out totalCount, queryFields, where, CurrentPageIndex, rows);
            List<T> result = new List<T>();
            result = ds.ToList<T>();
            RM.page = CurrentPageIndex;
            RM.rows = result;
            RM.total = (totalCount % rows == 0 ? totalCount / rows : totalCount / rows + 1);
            RM.records = totalCount;
            return RM;
        }

        /// <summary>
        /// 获取调整线路数据对比,获取分页数据并返回JQ [add by th]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <returns></returns>
        public static jqGridData GetJqComparisonList<T>(int id, int page = 1, int rows = 50, string sidx = "", string sord = "asc") where T : new()
        {
            jqGridData RM = new jqGridData();

            int totalCount = 0;

            int CurrentPageIndex = (page != 0 ? (int)page : 1);

            StringBuilder strSql = new StringBuilder("select   StopId,  Area, District, RoadName, StationName, levelName, PathDirection, StationAddress,LineList,SETime, Remark='撤销'  From tblRoadLineAdjustList_Pre");
            strSql.Append(" where StopId not in (select StopId From tblRoadLineAdjustList) and  MainKey=" + id);
            strSql.Append(" union all ");
            strSql.Append("select   StopId,  Area, District, RoadName, StationName, levelName, PathDirection, StationAddress,LineList,SETime, Remark='添加'  From tblRoadLineAdjustList");
            strSql.Append(" where StopId not in (select StopId From tblRoadLineAdjustList_Pre) and MainKey=" + id);
            DataTable Comparisonds = DBContext.DataDecision.GetDataTable(strSql.ToString());

            // DataTable ds = DBContext.PTSM(typeof(T).Name, orderBy, out totalCount, queryFields, where, CurrentPageIndex, rows);
            List<T> result = new List<T>();
            totalCount = Comparisonds.Rows.Count;
            result = Comparisonds.ToList<T>();
            RM.page = CurrentPageIndex;
            RM.rows = result;
            RM.total = (totalCount % rows == 0 ? totalCount / rows : totalCount / rows + 1);
            RM.records = totalCount;
            return RM;
        }
        /// <summary>
        /// 得到当前用户的业务权限
        /// </summary>
        /// <param name="Func">业务名称</param>
        /// <returns></returns>
        public static string GetBusinessAuthority(string Func)
        {
            string Authority = null;
            try
            {
                //角色集合
                List<string> RolesNamelist = CurrentHelper.CurrentUser.Roles.Select(p => p.role_name).Distinct().ToList();

                List<int> RolesIlist = BusinessContext.sys_role.GetList(null).Where(p => RolesNamelist.Contains(p.role_name)).Select(p => p.Rid).Distinct().ToList();
                // 获取业务权限对象
                var bsObj = BusinessContext.OperationalAuthority.GetModelList(" 1=1 ").FirstOrDefault(p => p.OperRational_Name == Func);
                if (bsObj != null && RolesIlist.Count > 0)
                {
                    // 获取所有角色的业务权限
                    var q = Query.And(Query<Model.sys_role_right>.EQ(t => t.rf_Type, "业务权限"),
                        Query<Model.sys_role_right>.EQ(t => t.rf_Right_Code, bsObj.Rid));
                    List<string> Authoritys = BusinessContext.sys_role_right.
                        GetList(q).Where(p => RolesIlist.Contains(p.rf_Role_Id) && p.rf_Type == "业务权限" && p.rf_Right_Code == bsObj.Rid).Select(p => p.rf_Right_Authority).Distinct().ToList();

                    if (Authoritys.Count > 1)
                    {
                        //有多个权限要判断后才能返回
                        if (Func == "客户查询")
                        {
                            if (Authoritys.Contains("查看所有"))
                            {
                                Authority = "查看所有";
                            }
                            else
                            {
                                if (Authoritys.Contains("查看下级"))
                                {
                                    Authority = "查看下级";
                                }
                                else if (Authoritys.Contains("查看本部门"))
                                {
                                    Authority = "查看本部门";
                                }
                                else
                                {
                                    Authority = "查看本人";
                                }
                            }
                        }
                        else if (Func == "订单查询")
                        {
                            if (Authoritys.Contains("查看所有"))
                            {
                                Authority = "查看所有";
                            }
                            else
                            {
                                if (Authoritys.Contains("查看下级"))
                                {
                                    Authority = "查看下级";
                                }
                                else if (Authoritys.Contains("查看本部门"))
                                {
                                    Authority = "查看本部门";
                                }
                                else
                                {
                                    Authority = "查看本人";
                                }
                            }
                        }
                        else
                        {
                            if (Authoritys.Contains("有权"))
                            {
                                Authority = "有权";
                            }
                        }
                    }
                    else if (Authoritys.Count == 1)
                    {
                        //只有一种权限就直接返回
                        Authority = Authoritys[0];
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Error("GetBusinessAuthority", ex);
                Authority = null;
            }
            return Authority;
        }
        /// <summary>
        /// 根据用户名获取所有下级
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static List<string> GetUnderling(string userName)
        {
            List<string> list = new List<string>();
            try
            {
                SqlParameter[] paras = new SqlParameter[]{
                    new SqlParameter("@leader",userName)
                };
                DataTable dt = DBContext.DataDecision.GetTableByExecProc("pd_GetUnderling", paras);
                list = dt.AsEnumerable().Select(p => p.Field<string>("sell")).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Error("GetUnderling", ex);
            }
            return list;
        }
     
        public static List<int> ToIntArray(this string[] strs)
        {
            if (strs == null || strs.Count() == 0)
            {
                return new List<int>(0);
            }
            //由于集合中的元素是确定的，所以可以指定元素的个数，系统就不会分配多余的空间，效率会高点
            List<int> list = new List<int>(strs.Count());
            foreach (string item in strs)
            {
                if (!string.IsNullOrWhiteSpace(item))
                    list.Add(Convert.ToInt32(item));
            }

            return list;
        }
        public static List<string> ListUnderling(string loginName)
        {
            List<string> list = new List<string>();
            try
            {

                tblUser_Sys user = CurrentHelper.GetUserByLoginName(loginName);
                if (user != null && !String.IsNullOrWhiteSpace(user.UserName))
                {
                    SqlParameter[] paras = new SqlParameter[]{
                         new SqlParameter("@leader",user.UserName)
                     };
                    DataTable dt = DBContext.DataDecision.GetTableByExecProc("pd_GetUnderling", paras);
                    list = dt.AsEnumerable().Select(p => p.Field<string>("sell")).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("GetUnderling", ex);
            }
            return list;
        }
        /// <summary>
        /// 根据登录名与查看权限获取筛选
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="authority">查看权限</param>
        /// <returns></returns>
        public static string AuthWhere(string loginName, string authority)
        {
            StringBuilder where = new StringBuilder();
            tblUser_Sys user = CurrentHelper.GetUserByLoginName(loginName);
            if (authority == "查看本人")
            {
                where.Append(" and (Seller= '" + user.UserName + "')");
            }
            else if (authority == "查看下级")
            {
                List<string> SellerXia = ListUnderling(user.UserName);
                string CdtSeller = " (";
                CdtSeller += String.Join("", SellerXia.Select(p => "'" + p + "',").ToList());
                CdtSeller += "'" + user.UserName + "')";
                where.Append(" and (Seller in " + CdtSeller + ")");
            }
            else if (authority == "查看本部门")
            {
                string Sql = " select UserName from tblUser_Sys where dept_New in (select dept_New from tblUser_Sys where loginName='" + loginName + "')";
                DataTable DT = DBContext.DataDecision.GetDataTable(Sql);
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    string UserName = Convert.ToString(DT.Rows[i]["UserName"]);
                    where.Append(" and (Seller= '" + UserName + "')");
                }
            }
            return where.ToString();
        }
        /// <summary>
        /// 获取有效日期（存在疑问）
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="validDays">有效天数</param>
        /// <returns></returns>
        public static DateTime GetValidDate(DateTime startDate, int validDays)
        {
            DateTime endDate = startDate.AddDays(validDays);
            return endDate;
        }
    }
}
