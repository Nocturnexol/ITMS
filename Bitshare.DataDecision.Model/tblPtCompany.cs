using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
    /// <summary>
    /// tblPtCompany:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class tblPtCompany
    {
        public ObjectId _id { get; set; }
        public tblPtCompany()
        { }
        #region Model
        private int rid;
        private string _ptcompany;
        private string _ptcompany_ab;
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        public int Rid
        {
            set { rid = value; }
            get { return rid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PtCompany
        {
            set { _ptcompany = value; }
            get { return _ptcompany; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PtCompany_Ab
        {
            set { _ptcompany_ab = value; }
            get { return _ptcompany_ab; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }




        private string _changecount;
        public string ChangeCount
        {
            set { _changecount = value; }
            get { return _changecount; }
        }
        #endregion Model

    }
}

