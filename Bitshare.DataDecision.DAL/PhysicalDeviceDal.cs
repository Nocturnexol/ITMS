using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using Bitshare.DataDecision.Model;
using BS.DB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bitshare.DataDecision.DAL
{
    public class PhysicalDeviceDal
    {
        private readonly string _mongoServerAddress = ConfigurationManager.AppSettings["MongoServer"];
        private readonly MongoServer _mongoServer;
        private readonly MongoDatabase _mongoDatabase;
        private readonly MongoCollection _mongoCollection;
        private readonly string _dataBase = "ITMS";
        private readonly string _collection = typeof(PhysicalDeviceDal).Name;
        //private readonly MongoDBVisitor _dbVisitor;
        public PhysicalDeviceDal()
        {
            //_dbVisitor = DBFactory.CreateMongoDBAccess(_mongoServerAddress);
            _mongoServer=new MongoClient(_mongoServerAddress).GetServer();
            _mongoDatabase = _mongoServer.GetDatabase(_dataBase);
            _mongoCollection = _mongoDatabase.GetCollection(_collection);
        }

        public List<PhysicalDevice> GetPhysicalDevice(IMongoQuery query)
        {
            var cursor = _mongoCollection.FindAs<PhysicalDevice>(query);
            return cursor.ToList();
        }
        public List<PhysicalDevice> GetPhysicalDeviceList(int page = 1, int rows = 20, string sidx = null, string sord = "asc")
        {
            //var list =
            //    _dbVisitor.FindAll<PhysicalDevice>(_dataBase, _collection).Skip((page - 1) * rows).Take(rows).ToList();
            var cursor=_mongoCollection.FindAs<PhysicalDevice>(null);
            if(!string.IsNullOrEmpty(sidx))
                cursor.SetSortOrder(sord == "asc" ? SortBy.Ascending(sidx) : SortBy.Descending(sidx));
            cursor.SetSkip(rows * (page - 1));
            cursor.SetLimit(rows);
            return cursor.ToList();
        }

        public bool Add(PhysicalDevice model)
        {
            var list = _mongoCollection.FindAs<PhysicalDevice>(null)
                .SetSortOrder(SortBy.Descending("Rid"))
                .SetLimit(1).ToList();
            if (list.Count > 0)
            {
                model.Rid = list.First().Rid + 1;
            }
            else
            {
                model.Rid = 1;
            }

            var res = _mongoCollection.Insert(model);
            return res.Ok;
        }

        public bool Update(PhysicalDevice model)
        {
            //var doc=new UpdateDocument();
            //doc.Set("Date", model.Date)
            //    .Set("DeviceType", model.DeviceType)
            //    .Set("IntranetIP", model.IntranetIP)
            //    .Set("ModelNum", model.ModelNum)
            //    .Set("PublicIP", model.PublicIP)
            //    .Set("Remark", model.Remark);
            //var res = _mongoCollection.Update(null, doc);

            IMongoQuery query = Query.EQ("Rid", model.Rid);
            var list = GetPhysicalDevice(query);
            model._id = list.First()._id;
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

    }
}
