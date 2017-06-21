using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.DataDecision.DAL;
using Bitshare.DataDecision.Model;
using MongoDB.Driver;

namespace Bitshare.DataDecision.BLL
{
    public class PhysicalDeviceBll
    {
        private readonly PhysicalDeviceDal _dal;

        public PhysicalDeviceBll()
        {
            _dal=new PhysicalDeviceDal();
        }

        public List<PhysicalDevice> GetPhysicalDevice(IMongoQuery query)
        {
            return _dal.GetPhysicalDevice(query);
        }
        public List<PhysicalDevice> GetPhysicalDeviceList(int page = 1, int rows = 20, string sidx = null,
            string sord = "asc")
        {
            return _dal.GetPhysicalDeviceList(page, rows, sidx, sord);
        }
        public bool Add(PhysicalDevice model)
        {
            return _dal.Add(model);
        }

        public bool Update(PhysicalDevice model)
        {
            return _dal.Update(model);
        }

        public bool Delete(IList<int> rId)
        {
            return _dal.Delete(rId);
        }
    }
}
