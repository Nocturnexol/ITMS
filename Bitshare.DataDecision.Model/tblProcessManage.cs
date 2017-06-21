namespace Bitshare.DataDecision.Model
{
    //tblProcessManage
    public class tblProcessManage:BaseModel
    {

        
        /// <summary>
        /// ProcessNum
        /// </summary>		
        private int _processnum;
        public int ProcessNum
        {
            get { return _processnum; }
            set { _processnum = value; }
        }
        /// <summary>
        /// ProcessName
        /// </summary>		
        private string _processname;
        public string ProcessName
        {
            get { return _processname; }
            set { _processname = value; }
        }
        /// <summary>
        /// NodeNum
        /// </summary>		
        private int _nodenum;
        public int NodeNum
        {
            get { return _nodenum; }
            set { _nodenum = value; }
        }
        /// <summary>
        /// NodeName
        /// </summary>		
        private string _nodename;
        public string NodeName
        {
            get { return _nodename; }
            set { _nodename = value; }
        }
        /// <summary>
        /// NodeNameNext
        /// </summary>		
        private string _nodenamenext;
        public string NodeNameNext
        {
            get { return _nodenamenext; }
            set { _nodenamenext = value; }
        }
        /// <summary>
        /// NodeType
        /// </summary>		
        private string _nodetype;
        public string NodeType
        {
            get { return _nodetype; }
            set { _nodetype = value; }
        }
        /// <summary>
        /// ExecutorRole
        /// </summary>		
        private string _executorrole;
        public string ExecutorRole
        {
            get { return _executorrole; }
            set { _executorrole = value; }
        }
        /// <summary>
        /// ReMark
        /// </summary>		
        private string _remark;
        public string ReMark
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}

