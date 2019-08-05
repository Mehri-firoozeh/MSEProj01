using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CostcoProjectStatus.App_Start
{
    public class RiderectToExternalLogingCallBackController : Controller
    {
        // GET: RiderectToExternalLogingCallBack
        public ActionResult Index()
        {
            return View();
        }

        // GET: RiderectToExternalLogingCallBack/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RiderectToExternalLogingCallBack/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RiderectToExternalLogingCallBack/Create
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

        // GET: RiderectToExternalLogingCallBack/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RiderectToExternalLogingCallBack/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RiderectToExternalLogingCallBack/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RiderectToExternalLogingCallBack/Delete/5
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
      //  [HttpPost]
        //public ActionResult ToExternalLoginCallBack()
        //{
        //    return RedirectToAction("ExternalLoginCallback", "CostcoProjectStatus.Controllers.AccountController");
        //}
    }
}
