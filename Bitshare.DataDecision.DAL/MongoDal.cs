using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bitshare.DataDecision.DAL
{
    public class MongoDal<T>:IMongoDal<T> where T : class
    {
        private readonly string _mongoServerAddress = ConfigurationManager.AppSettings["MongoServer"];
        private readonly MongoCollection _mongoCollection;
        private readonly string _dataBase = ConfigurationManager.AppSettings["MongoDataBase"];
        private readonly string _collection = typeof(T).Name;

        public MongoDal()
        {
            var mongoServer = new MongoClient(_mongoServerAddress).GetServer();
            var mongoDatabase = mongoServer.GetDatabase(_dataBase);
            _mongoCollection = mongoDatabase.GetCollection(_collection);
        }
        public T Get(IMongoQuery query)
        {
            var cursor = _mongoCollection.FindAs<T>(query);
            return cursor.FirstOrDefault();
        }

        public List<T> GetList(out long count, int page = 1, int rows = 20, IMongoQuery where = null, string sidx = null,
            string sord = "asc")
        {
            count = _mongoCollection.Count();
            var cursor = _mongoCollection.FindAs<T>(where);
            if (!string.IsNullOrEmpty(sidx))
                cursor.SetSortOrder(sord == "asc" ? SortBy.Ascending(sidx) : SortBy.Descending(sidx));
            cursor.SetSkip(rows * (page - 1));
            cursor.SetLimit(rows);
            return cursor.ToList();
        }

        public bool Add(T model)
        {
            if (typeof(T).GetProperty("Rid") != null)
            {
                var list = _mongoCollection.FindAs<T>(null)
                    .SetSortOrder(SortBy.Descending("Rid"))
                    .SetLimit(1).ToList();
                typeof(T).GetProperty("Rid").SetValue(model, 1, null);
                if (list.Count > 0)
                {
                    var rId = (int) list.First().GetType().GetProperty("Rid").GetValue(list.First(), null);
                    typeof(T).GetProperty("Rid").SetValue(model, rId + 1, null);
                }
                else
                {
                    typeof(T).GetProperty("Rid").SetValue(model, 1, null);
                }
            }
            var res = _mongoCollection.Insert(model);
            return res.Ok;
        }

        public bool Update(T model)
        {
            IMongoQuery query = Query.EQ("Rid", (int)typeof(T).GetProperty("Rid").GetValue(model,null));
            var modelDb = Get(query);
            var id = (ObjectId)modelDb.GetType().GetProperty("_id").GetValue(modelDb, null);
            typeof(T).GetProperty("_id").SetValue(model, id, null);
            //model._id = list.First()._id;
            BsonDocument bd = model.ToBsonDocument();
            var res = _mongoCollection.Update(query, new UpdateDocument(bd));
            return res.Ok;
        }

        public bool Delete(IList<int> rId)
        {
            return rId.Select(id => Query.EQ("Rid", id))
                .Select(query => _mongoCollection.Remove(query))
                .Aggregate(true, (current, res) => current && res.Ok);
        }

        public List<T> GetList(IMongoQuery where)
        {
            return _mongoCollection.FindAs<T>(where).ToList();
        }

        public int GetMaxId()
        {
            var cursor = _mongoCollection.FindAs<T>(null);
            cursor.SetSortOrder(SortBy.Descending("Rid"));
            var first = cursor.FirstOrDefault();
            return first != null ? (int) cursor.First().GetType().GetProperty("Rid").GetValue(cursor.First(), null) : 0;
        }

        public bool Add(IList<T> list)
        {
            return list.Aggregate(true, (current, model) => current && Add(model));
        }
    }
}
