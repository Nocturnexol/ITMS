using System;
using System.Collections.Generic;
using System.Data;
namespace Bitshare.DataDecision.BLL
{
    //OperationalAuthority
    public partial class OperationalAuthority
    {

        private readonly Bitshare.DataDecision.DAL.OperationalAuthority dal = new Bitshare.DataDecision.DAL.OperationalAuthority();
        public OperationalAuthority()
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
        public int Add(Bitshare.DataDecision.Model.OperationalAuthority model)
        {
            return dal.Add(model);

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Bitshare.DataDecision.Model.OperationalAuthority model)
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
        public Bitshare.DataDecision.Model.OperationalAuthority GetModel(int Rid)
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
        public List<Bitshare.DataDecision.Model.OperationalAuthority> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Bitshare.DataDecision.Model.OperationalAuthority> DataTableToList(DataTable dt)
        {
            List<Bitshare.DataDecision.Model.OperationalAuthority> modelList = new List<Bitshare.DataDecision.Model.OperationalAuthority>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Bitshare.DataDecision.Model.OperationalAuthority model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Bitshare.DataDecision.Model.OperationalAuthority();
                    if (dt.Rows[n]["Rid"].ToString() != "")
                    {
                        model.Rid = int.Parse(dt.Rows[n]["Rid"].ToString());
                    }
                    model.UpdateUser = dt.Rows[n]["UpdateUser"].ToString();
                    model.OperRational_Name = dt.Rows[n]["OperRational_Name"].ToString();
                    model.RightsOptions = dt.Rows[n]["RightsOptions"].ToString();
                    if (dt.Rows[n]["Options"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Options"].ToString() == "1") || (dt.Rows[n]["Options"].ToString().ToLower() == "true"))
                        {
                            model.Options = true;
                        }
                        else
                        {
                            model.Options = false;
                        }
                    }
                    if (dt.Rows[n]["CreateDataTime"].ToString() != "")
                    {
                        model.CreateDataTime = DateTime.Parse(dt.Rows[n]["CreateDataTime"].ToString());
                    }
                    model.CreateDataUser = dt.Rows[n]["CreateDataUser"].ToString();
                    model.HandleCompany = dt.Rows[n]["HandleCompany"].ToString();
                    if (dt.Rows[n]["UpdateTime"].ToString() != "")
                    {
                        model.UpdateTime = DateTime.Parse(dt.Rows[n]["UpdateTime"].ToString());
                    }


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