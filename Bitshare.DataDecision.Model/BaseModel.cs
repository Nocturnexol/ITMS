using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
    /// <summary>
    /// 基类
    /// </summary>
    public partial class BaseModel
    {
        public ObjectId _id { get; set; }
        public int Rid { get; set; }

        public bool IsDel { get; set; }
    }
}
