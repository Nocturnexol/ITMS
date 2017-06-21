using System;

namespace Bitshare.DataDecision.Service.DTO
{
    [Serializable]
    public partial class DesignTotal
    {
        public DesignTotal()
        { }
        #region Model
        private string _loginname;
        private string _designer;
        private int _adcount;
        private int _contentcount;
        private int _vehicletypecount;
        private int _designercount;
        private int _younum;
        private int _liangnum;
        private int _zhongnum;
        private int _chanum;
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        public string LoginName
        {
            set { _loginname = value; }
            get { return _loginname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Designer
        {
            set { _designer = value; }
            get { return _designer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AdCount
        {
            set { _adcount = value; }
            get { return _adcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ContentCount
        {
            set { _contentcount = value; }
            get { return _contentcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int VehicleTypeCount
        {
            set { _vehicletypecount = value; }
            get { return _vehicletypecount; }
        }
        public int DesignerCount
        {
            set { _designercount = value; }
            get { return _designercount; }
        }
        public int YouNum
        {
            set { _younum = value; }
            get { return _younum; }
        }
        public int LiangNum
        {
            set { _liangnum = value; }
            get { return _liangnum; }
        }
        public int ZhongNum
        {
            set { _zhongnum = value; }
            get { return _zhongnum; }
        }
        public int ChaNum
        {
            set { _chanum = value; }
            get { return _chanum; }
        }
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model

    }
}
