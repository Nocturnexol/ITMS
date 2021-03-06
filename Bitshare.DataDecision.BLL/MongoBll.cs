﻿using System.Collections.Generic;
using Bitshare.DataDecision.DAL;
using MongoDB.Driver;

namespace Bitshare.DataDecision.BLL
{
    public class MongoBll<T> where T : class
    {
        private readonly IMongoDal<T> _dal;

        public MongoBll()
        {
            _dal = new MongoDal<T>();
        }

        public int GetMaxId()
        {
            return _dal.GetMaxId();
        }
        public T Get(IMongoQuery query)
        {
            return _dal.Get(query);
        }

        public List<T> GetList(IMongoQuery where=null)
        {
            return _dal.GetList(where);
        }
        public List<T> GetList(out long count, int page = 1, int rows = 20, IMongoQuery where = null, string sidx = null,
            string sord = "asc")
        {
            return _dal.GetList(out count, page, rows, where, sidx, sord);
        }

        public bool Add(T model)
        {
            return _dal.Add(model);
        }

        public bool Add(IList<T> list)
        {
            return _dal.Add(list);
        }
        public bool Update(T model)
        {
            return _dal.Update(model);
        }

        public bool Delete(IList<int> rId)
        {
            return _dal.Delete(rId);
        }
    }
}
