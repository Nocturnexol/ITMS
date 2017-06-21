using System.Collections.Generic;
using MongoDB.Driver;

namespace Bitshare.DataDecision.DAL
{
    public interface IMongoDal<T> where T : class 
    {
        T Get(IMongoQuery query);
        int GetMaxId();
        List<T> GetList(IMongoQuery where);
        List<T> GetList(out long count, int page = 1, int rows = 20, IMongoQuery where = null, string sidx = null,
            string sord = "asc");
        bool Add(T model);
        bool Add(IList<T> list);
        bool Update(T model);
        bool Delete(IList<int> rId);
    }
}
