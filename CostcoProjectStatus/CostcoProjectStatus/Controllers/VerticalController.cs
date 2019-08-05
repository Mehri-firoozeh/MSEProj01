using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService;
using Newtonsoft.Json;

namespace CostcoProjectStatus.Controllers
{
    public class VerticalController : Controller
    {/// <summary>
    ///  Getting all the vertical's name from Data Access layer . 
    /// No parameter need to get pass. This method is used in app.js to get the all verticals and
    /// feed the dropdown in nav bar.
    /// </summary>
    /// <returns>verticals in format of json</returns>
        
        public string GetAllVertical()
        {
            AccessService DataAccess = new AccessService();
            var verticals = DataAccess.GetAllVerticals();
            string result = JsonConvert.SerializeObject(verticals);
            return result;            
        }
        /// <summary>
        ///  Get function ,to get all the projects for specific vertical from data access layer.
        ///  It been used in project-list.js to show the project names on a ProjectList.html.
        /// </summary>
        /// <param name="VerticalId">VerticalId (int)index defined in the Verticals enum</param>
        /// <returns>List of Project objects in format of json</returns>
        public string GetVerticalProjects(int VerticalId)
        {
            AccessService DataAccess = new AccessService();
            var passProjectList = new List<StatusUpdatesModel.Project>();
            try {
                if (this.Session["username"].ToString() != null && DataAccess.IsUserAuthorized(this.Session["username"].ToString()))
                {
                   // AccessService DataAccess = new AccessService();
                    var VerticalProjects = DataAccess.GetAllProjectsForVertical(VerticalId);

                    foreach (StatusUpdatesModel.Project project in VerticalProjects)
                    {
                        StatusUpdatesModel.Project tempProject = new StatusUpdatesModel.Project();
                        tempProject.LatestUpdate = project.LatestUpdate;
                        tempProject.ProjectID = project.ProjectID;
                        tempProject.ProjectName = project.ProjectName;
                        passProjectList.Add(tempProject);

                    }
                }
            }
            catch (Exception)
            {
                // Probably not the best way to handle this
                string empty = JsonConvert.SerializeObject(passProjectList);
                return empty;
            }
            string result = JsonConvert.SerializeObject(passProjectList);
            return result;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: Vertical/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Vertical/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Vertical/Create
        //[HttpPost]
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

        //// GET: Vertical/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Vertical/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Vertical/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Vertical/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

    }
}
