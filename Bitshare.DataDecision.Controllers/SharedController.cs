using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Bitshare.DataDecision.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SharedController : Controller
    {
        //
        // GET: /Shared/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Shared/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Shared/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Shared/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Shared/Edit/5

        public ActionResult Edit(string LoginName)
        {

            return View();
        }

        //
        // POST: /Shared/Edit/5

        [HttpPost]
        public ActionResult Edit(UserPassWord upw)
        {
            ReturnMessage RM = new ReturnMessage(false);
            try
            {


                tblUser_Sys tbluse = BusinessContext.tblUser_Sys.GetModelList("Loginname='" + upw.LoginName + "'").FirstOrDefault();

                if (upw.OldPassWord != tbluse.UserPwd)
                {
                    RM.IsSuccess = false;
                    RM.Message = "旧密码输入错误，请重新输入！";
                    return Json(RM);
                }
                if (upw.NewPassWord != upw.SureNewPassWord)
                {
                    RM.IsSuccess = false;
                    RM.Message = "新密码不一致，请重新输入！";
                    return Json(RM);
                }

                List<string> lsSql = new List<string>();

                StringBuilder strSql = new StringBuilder("update tblUser_Sys set UserPwd='" + upw.NewPassWord + "' where loginname='" + upw.LoginName + "'");
                lsSql.Add(strSql.ToString());

                if (DBContext.DataDecision.ExecTrans(lsSql.ToArray()))
                {
                    RM.IsSuccess = true;
                    RM.Message = "密码设置成功！";
                }
                else
                {
                    RM.IsSuccess = true;
                    RM.Message = "密码设置失败！";
                }

                return Json(RM);
            }
            catch
            {
                return Json(RM);
            }
        }

        //
        // GET: /Shared/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Shared/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
