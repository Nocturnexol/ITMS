using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Bitshare.DataDecision.BLL;
using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using Bitshare.DataDecision.Service.DTO;
using Bitshare.DataDecision.Service.Enum;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bitshare.DataDecision.Controllers.Areas.ResourceManagement
{
    public class ServiceController:Controller
    {
        private readonly MongoBll<Model.Service> _bll = new MongoBll<Model.Service>();
        public ActionResult Index(bool? isDependency)
        {
            List<string> btnList = FlowHelper.GetBtnAuthorityForPage("服务管理");
            ViewBag.ServiceList = FlowHelper.GetTypeSelectList("服务类型");
            if (isDependency != null)
                ViewBag.isDependency = isDependency;
            else
                ViewBag.BtnList = btnList;
            return View();
        }
        public ActionResult GetServiceList(int page = 1, int rows = 20, Model.Service search = null, string sidx = null, string sord = "asc")
        {
            if (string.IsNullOrEmpty(sidx))
                sidx = "Type";
            var pager = new PageInfo
            {
                PageSize = rows,
                CurrentPageIndex = page > 0 ? page : 1
            };

            List<IMongoQuery> queryList = null;
            if (search != null)
            {
                queryList = new List<IMongoQuery>();
                if (search.Type.HasValue)
                {
                    queryList.Add(Query<Model.Service>.EQ(t => t.Type, search.Type));
                }
                if (!string.IsNullOrWhiteSpace(search.Address))
                {
                    queryList.Add(Query<Model.Service>.EQ(t => t.Address,
                        new BsonRegularExpression(new Regex(search.Address, RegexOptions.IgnoreCase))));
                }
                if (!string.IsNullOrWhiteSpace(search.Description))
                {
                    queryList.Add(Query<Model.Service>.EQ(t => t.Description, search.Description));
                }
                if (!string.IsNullOrWhiteSpace(search.Version))
                {
                    queryList.Add(Query<Model.Service>.EQ(t => t.Version, new BsonRegularExpression(new Regex(search.Version, RegexOptions.IgnoreCase))));
                }
                if (search.Dependency.HasValue)
                {
                    queryList.Add(Query<Model.Service>.EQ(t => t.Dependency, search.Dependency.Value));
                }
            }
            var where = queryList != null && queryList.Any() ? Query.And(queryList) : null;
            long totalCount;
            var rm = new jqGridData
            {
                page = pager.CurrentPageIndex,
                rows = _bll.GetList(out totalCount, page, rows, where,sidx, sord),
                total =
                    (int)
                    (totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1),
                records = (int) totalCount
            };
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.ServiceList = FlowHelper.GetTypeSelectList("服务类型");
            ViewBag.DeployTypeList = GetDeployTypeList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Model.Service collection, string IsContinue = "0")
        {
            ReturnMessage rm = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                        if (!CheckRepeat(collection))
                        {
                            rm.IsSuccess = false;
                            rm.Message = "已有相同记录存在！";
                        }
                        else
                        {
                            bool res = _bll.Add(collection);
                            rm.IsSuccess = res;
                            if (rm.IsSuccess)
                            {
                                OperateLogHelper.Create(collection);
                                rm.IsContinue = IsContinue == "1";
                            }
                        }
                    

                }
                catch (Exception ex)
                {
                    rm.Message = ex.Message;
                }
            }
            return Json(rm);
        }
        public ActionResult Delete()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                if (!string.IsNullOrWhiteSpace(paramData))
                {
                    string[] ids = paramData.Split('*');
                    var rIds = ids.Select(int.Parse).ToList();
                    if (_bll.Delete(rIds))
                    {
                        //OperateLogHelper.Delete(modelList);
                        rm.IsSuccess = true;
                        rm.Message = "删除成功！";
                    }
                    else
                    {
                        rm.IsSuccess = false;
                        rm.Message = "删除失败！";
                    }

                }
            }
            catch (Exception ex)
            {
                rm.IsSuccess = false;
                rm.Message = ex.Message;
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            var model = _bll.Get(Query.EQ("Rid", id));
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceList = FlowHelper.GetTypeSelectList("服务类型");
            ViewBag.DeployTypeList = GetDeployTypeList();
            if (model.DeployLocale.HasValue)
            {
                if (model.DeployDeviceType == (int) DeployDeviceTypeEnum.Physical)
                {
                    ViewBag.DeployLocaleName =
                        DependencyResolver.Current.GetService<PhysicalDeviceController>()
                            .GetHostDeviceName(model.DeployLocale.Value);
                }
                else
                {
                    ViewBag.DeployLocaleName =
                        DependencyResolver.Current.GetService<VirtualDeviceController>()
                            .GetVirtualDeviceName(model.DeployLocale.Value);
                }
            }
            return View(model);

        }
        [HttpPost]
        public ActionResult Edit(Model.Service collection)
        {
            ReturnMessage rm = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    if (!CheckRepeat(collection))
                    {
                        rm.IsSuccess = false;
                        rm.Message = "已有相同记录存在！";
                    }
                    else
                    {
                        rm.IsSuccess = _bll.Update(collection);
                    }
                }
                catch (Exception ex)
                {
                    rm.Message = ex.Message;
                }
            }
            return Json(rm);
        }

        private bool CheckRepeat(Model.Service model)
        {
            var query = Query.And(Query.EQ("Type", model.Type), Query.EQ("Address", model.Address));
            var modelDb = _bll.Get(query);
            if (modelDb==null) return true;
            return model.Rid == modelDb.Rid;
        }
        //private IList<SelectListItem> GetSelectList(string typeName)
        //{
        //    //return DependencyResolver.Current.GetService<DeviceTypeController>().GetSelectList(typeName);
        //    return FlowHelper.GetSelectListFromController<DeviceTypeController>("GetSelectList", typeName);
        //}
        private IList<SelectListItem> GetDeployTypeList()
        {
            return (from object t in Enum.GetValues(typeof(DeployDeviceTypeEnum))
                    select new SelectListItem
                    {
                        Text = t.ToString(),
                        Value = ((int)t).ToString()
                    }).ToList();
        }
    }
}
