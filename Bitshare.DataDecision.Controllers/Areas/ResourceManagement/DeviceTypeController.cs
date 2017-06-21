using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Bitshare.DataDecision.BLL;
using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using Bitshare.DataDecision.Service.DTO;
using Bitshare.DataDecision.Service.Enum;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bitshare.DataDecision.Controllers.Areas.ResourceManagement
{
    public class DeviceTypeController:Controller
    {
        private readonly MongoBll<BasicType> _bll = new MongoBll<BasicType>();
        public ActionResult Index()
        {
            List<string> btnList = FlowHelper.GetBtnAuthorityForPage("基础数据");
            ViewBag.BtnList = btnList;
            ViewBag.BasicTypeList = FlowHelper.BasicTypeList;
            return View();
        }
        public ActionResult GetDeviceTypeList(int page = 1, int rows = 20, BasicType search = null, string sidx = null, string sord = "asc")
        {
            if (string.IsNullOrEmpty(sidx))
                sidx = "TypeId";
            var pager = new PageInfo
            {
                PageSize = rows,
                CurrentPageIndex = page > 0 ? page : 1
            };

            List<IMongoQuery> queryList = null;
            if (search != null)
            {
                queryList = new List<IMongoQuery>();
                if (search.TypeId.HasValue)
                {
                    queryList.Add(Query<BasicType>.EQ(t => t.TypeId, search.TypeId));
                }
                if (search.Num.HasValue)
                {
                    queryList.Add(Query<BasicType>.EQ(t => t.Num, search.Num));
                }
                if (!string.IsNullOrWhiteSpace(search.Name))
                {
                    queryList.Add(Query<BasicType>.EQ(t => t.Name, search.Name));
                }
            }
            var where = queryList != null && queryList.Any() ? Query.And(queryList) : null;

            long totalCount;
            var rm = new jqGridData
            {
                page = pager.CurrentPageIndex,
                rows = _bll.GetList(out totalCount,page, rows, where,sidx, sord),
                total = (int) (totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1),
                records = (int) totalCount
            };
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        
        //public IList<SelectListItem> GetSelectList(string typeName)
        //{
        //    if (!FlowHelper.BasicTypeList.Select(t => t.Text).Contains(typeName))
        //    {
        //        throw new ArgumentException("无效的类型名称");
        //    }
        //    var type = FlowHelper.BasicTypeList.First(t => t.Text == typeName);
        //    long count;
        //    var list = _bll.GetList(out count,1, 50);
        //    return list.Where(t=>t.TypeId==int.Parse(type.Value)).Select(t => new SelectListItem
        //    {
        //        Text = t.Name,
        //        Value = t.Num.ToString()
        //    }).ToList();
        //}
        public ActionResult Create()
        {
            ViewBag.BasicTypeList = FlowHelper.BasicTypeList;
            return View();
        }

        [HttpPost]
        public ActionResult Create(BasicType collection, string IsContinue = "0")
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(collection.Name))
                    {
                        if (!CheckRepeat(collection))
                        {
                            RM.IsSuccess = false;
                            RM.Message = "已有相同记录存在！";
                        }
                        else
                        {
                            bool res = _bll.Add(collection);
                            RM.IsSuccess = res;
                            if (RM.IsSuccess)
                            {
                                OperateLogHelper.Create(collection);
                                RM.IsContinue = IsContinue == "1";
                            }
                        }
                    }
                    else
                    {
                        RM.IsSuccess = false;
                        RM.Message = "名称不能为空！";
                    }

                }
                catch (Exception ex)
                {
                    RM.Message = ex.Message;
                }
            }
            return Json(RM);
        }
        public ActionResult Delete()
        {
            ReturnMessage RM = new ReturnMessage();
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
                        RM.IsSuccess = true;
                        RM.Message = "删除成功！";
                    }
                    else
                    {
                        RM.IsSuccess = false;
                        RM.Message = "删除失败！";
                    }

                }
            }
            catch (Exception ex)
            {
                RM.IsSuccess = false;
                RM.Message = ex.Message;
            }
            return Json(RM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            var model = _bll.Get(Query.EQ("Rid", id));
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.BasicTypeList = FlowHelper.BasicTypeList;
            return View(model);

        }
        [HttpPost]
        public ActionResult Edit(BasicType collection)
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    if (!CheckRepeat(collection))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "已有相同记录存在！";
                    }
                    else
                    {
                        RM.IsSuccess = _bll.Update(collection);
                    }
                }
                catch (Exception ex)
                {
                    RM.Message = ex.Message;
                }
            }
            return Json(RM);
        }

        private bool CheckRepeat(BasicType model)
        {
            var query = Query.And(Query.EQ("Num", model.Num), Query.EQ("TypeId", model.TypeId));
            var modelDb = _bll.Get(query);
            if (modelDb==null) return true;
            return model.Rid == modelDb.Rid;
        }

        //private IList<SelectListItem> GetBasicTypeList()
        //{
        //    //var enumerable = from a in new List<BasicType>() group a by a.Rid<40;
        //    //return (from object t in Enum.GetValues(typeof(BasicTypeEnum))
        //    //        select new SelectListItem
        //    //        {
        //    //            Text = t.ToString(),
        //    //            Value = ((int)t).ToString()
        //    //        }).ToList();
        //    var typeArr = ConfigurationManager.AppSettings["BasicTypes"].Split(new[] {','},
        //        StringSplitOptions.RemoveEmptyEntries);
        //    return typeArr.Select((t, i) => new SelectListItem
        //    {
        //        Text = t,
        //        Value = (i + 1).ToString()
        //    }).ToList();
        //}
    }
}
