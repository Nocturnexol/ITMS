using System;
namespace Bitshare.DataDecision.Model
{
    /// <summary>
    /// view_PolesDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class View_PolesDetail
    {
        public View_PolesDetail()
        { }
        #region Model
        private string _polesnumber;
        private string _capitalnumber;
        private string _polestype;
        private string _area;
        private string _district;
        private string _roadname;
        private string _stationname;
        private string _pathdirection;
        private string _stationaddress;
        private string _linelist;
        private DateTime? _settingdate;
        private int _tblstopid;
        private string _stopid;
        private int rid;
        private string _quYu;

        public string QuYu
        {
            get { return _quYu; }
            set { _quYu = value; }
        }


        /// <summary>
        /// 人行道宽度
        /// </summary>
        public decimal SidewalkWidth { set; get; }
        /// <summary>
        /// 沿街情况
        /// </summary>
        public string AfterCondition { set; get; }
        /// <summary>
        /// 路面材质
        /// </summary>
        public string SurfaceProperty { set; get; }
        /// <summary>
        /// 主干道（是否）
        /// </summary>
        public bool IsMainRoad { set; get; }
        /// <summary>
        /// 变更日期
        /// </summary>
        public DateTime? BackupDate { set; get; }
        /// <summary>
        /// 变更情况
        /// </summary>
        public string ChangeCondition { set; get; }
        /// <summary>
        /// 附近标志性建筑
        /// </summary>
        public string Landmark{set;get;}

        /// <summary>
        /// 可换乘地铁（300米）
        /// </summary>
        public string RideMetroRoadName { set; get; }
        /// <summary>
        /// 主要交通枢纽
        /// </summary>
        public string Transporthub { set; get; }
        /// <summary>
        /// 管理厂家
        /// </summary>
        public string Facturer { set; get; }
        

        /// <summary>
        /// 
        /// </summary>
        public string PolesNumber
        {
            set { _polesnumber = value; }
            get { return _polesnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CapitalNumber
        {
            set { _capitalnumber = value; }
            get { return _capitalnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PolesType
        {
            set { _polestype = value; }
            get { return _polestype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Area
        {
            set { _area = value; }
            get { return _area; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string District
        {
            set { _district = value; }
            get { return _district; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RoadName
        {
            set { _roadname = value; }
            get { return _roadname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StationName
        {
            set { _stationname = value; }
            get { return _stationname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PathDirection
        {
            set { _pathdirection = value; }
            get { return _pathdirection; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StationAddress
        {
            set { _stationaddress = value; }
            get { return _stationaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LineList
        {
            set { _linelist = value; }
            get { return _linelist; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SettingDate
        {
            set { _settingdate = value; }
            get { return _settingdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int tblStopId
        {
            set { _tblstopid = value; }
            get { return _tblstopid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StopId
        {
            set { _stopid = value; }
            get { return _stopid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Rid
        {
            set { rid = value; }
            get { return rid; }
        }
        #endregion Model

    }
}

