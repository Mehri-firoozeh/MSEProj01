using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService;
using Newtonsoft.Json;

namespace CostcoProjectStatus.Controllers
{
    public class EnvironmentController : Controller
    {

        //public string GetAllEnviromentalDomain()
        //{
        //}
        
        
        // GET: Environment
        public ActionResult Index()
        {
            return View();
        }

        // GET: Environment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Environment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Environment/Create
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

        // GET: Environment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Environment/Edit/5
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

        // GET: Environment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Environment/Delete/5
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
