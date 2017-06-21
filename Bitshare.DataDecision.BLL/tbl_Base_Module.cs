using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Bitshare.DataDecision.BLL
{
    public partial class tbl_Base_Module
    {

        private readonly Bitshare.DataDecision.DAL.tbl_Base_Module dal = new Bitshare.DataDecision.DAL.tbl_Base_Module();
        public tbl_Base_Module()
        { }

        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Rid)
        {
            return dal.Exists(Rid);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Bitshare.DataDecision.Model.tbl_Base_Module model)
        {
            return dal.Add(model);

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Bitshare.DataDecision.Model.tbl_Base_Module model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Rid)
        {

            return dal.Delete(Rid);
        }
        /// <summary>
        /// 批量删除一批数据
        /// </summary>
        public bool DeleteList(string Ridlist)
        {
            return dal.DeleteList(Ridlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.tbl_Base_Module GetModel(int Rid)
        {

            return dal.GetModel(Rid);
        }

       
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Bitshare.DataDecision.Model.tbl_Base_Module> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Bitshare.DataDecision.Model.tbl_Base_Module> DataTableToList(DataTable dt)
        {
            List<Bitshare.DataDecision.Model.tbl_Base_Module> modelList = new List<Bitshare.DataDecision.Model.tbl_Base_Module>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Bitshare.DataDecision.Model.tbl_Base_Module model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Bitshare.DataDecision.Model.tbl_Base_Module();
                    if (dt.Rows[n]["Rid"].ToString() != "")
                    {
                        model.Rid = int.Parse(dt.Rows[n]["Rid"].ToString());
                    }
                    if (dt.Rows[n]["UpdateTime"].ToString() != "")
                    {
                        model.UpdateTime = DateTime.Parse(dt.Rows[n]["UpdateTime"].ToString());
                    }
                    model.UpdateUser = dt.Rows[n]["UpdateUser"].ToString();
                    if (dt.Rows[n]["ModuleId"].ToString() != "")
                    {
                        model.ModuleId = int.Parse(dt.Rows[n]["ModuleId"].ToString());
                    }
                    if (dt.Rows[n]["ParentId"].ToString() != "")
                    {
                        model.ParentId = int.Parse(dt.Rows[n]["ParentId"].ToString());
                    }
                    model.ModuleName = dt.Rows[n]["ModuleName"].ToString();
                    model.Icon = dt.Rows[n]["Icon"].ToString();
                    model.Url = dt.Rows[n]["Url"].ToString();
                    model.Remark = dt.Rows[n]["Remark"].ToString();
                    if (dt.Rows[n]["CreateTime"].ToString() != "")
                    {
                        model.CreateTime = DateTime.Parse(dt.Rows[n]["CreateTime"].ToString());
                    }
                    model.CreateUser = dt.Rows[n]["CreateUser"].ToString();


                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion

    }
}
