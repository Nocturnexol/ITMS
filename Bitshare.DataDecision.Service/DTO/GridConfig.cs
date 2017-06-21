namespace Bitshare.DataDecision.Service.DTO
{
    public class GridConfig
    {
        public bool IsPrimaryKey { set; get; }
        public string Align { get; set; }
        public bool Fixed { get; set; }
        public string Formatter { get; set; }
        public bool Hidden { get; set; }
        public bool Frozen { get; set; }
        public string Index { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int OrderNum { set; get; }
    }

}
