using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService;
using Newtonsoft.Json;
using CostcoProjectStatus.CustomAttributes;

namespace CostcoProjectStatus.Controllers
{
    
    public class ProjectListController : Controller
    {
/// <summary>
/// This controller read from the data base and pass the needed information to Angular therefore all the methods
/// are GET. All routes definded in RouteConfig file and can be searched by the name of the methods.
/// In general each method get the infromation from Data Access layer which is a wrapper around SQL database and 
/// pass a json object to the UI. 
/// 
///
/// </summary>
        private AccessService DataAccsess = new AccessService();
            // GET: ProjectList
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProjectList/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: ProjectList/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ProjectList/Create
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

        //// GET: ProjectList/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ProjectList/Edit/5
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

        //// GET: ProjectList/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ProjectList/Delete/5
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
        /// <summary>
        /// Not getting use currently.
        /// </summary>
        /// <returns></returns>
        public string Display()
        {
           // var ProjectNames = DataAccsess.GetAllProjectNames();
        string result = "<script>window.location.replace(\"/dashboard/index.html\");</script>";
            return result;
        }

        //[AuthAttribute]
        //    [BasicAuthentication]
        //public string GetStatusUpdates(String id)
        //{

        //    try
        //    {

        //        if (this.Session["username"].ToString() != null && DataAccsess.IsUserAuthorized(this.Session["username"].ToString()))
        //        {
        //            var ProjectUpdates = DataAccsess.GetAllUpdatesForProject(id);
        //            //var passedStatusUpdateList = new List<StatusUpdatesModel.StatusUpdate>();
        //            //foreach (StatusUpdatesModel.StatusUpdate passedStatusUpdate in ProjectUpdates)
        //            //{
        //            //    StatusUpdatesModel.StatusUpdate tempStatusUpdate = new StatusUpdatesModel.StatusUpdate();
        //            //    //tempStatusUpdate.Phase = passedStatusUpdate.Phase;
        //            //    tempStatusUpdate.PhaseID = passedStatusUpdate.PhaseID;
        //            //    //tempStatusUpdate.Project = passedStatusUpdate.Project;
        //            //    tempStatusUpdate.ProjectID = passedStatusUpdate.ProjectID;
        //            //    tempStatusUpdate.ProjectName = passedStatusUpdate.ProjectName;
        //            //    tempStatusUpdate.RecordDate = passedStatusUpdate.RecordDate;
        //            //    tempStatusUpdate.ProjectUpdateID = passedStatusUpdate.ProjectUpdateID;
        //            //    tempStatusUpdate.UpdateKey = passedStatusUpdate.UpdateKey;
        //            //    tempStatusUpdate.UpdateValue = passedStatusUpdate.UpdateValue;
        //            //    //tempStatusUpdate.Vertical = passedStatusUpdate.Vertical;
        //            //    tempStatusUpdate.VerticalID = passedStatusUpdate.VerticalID;
        //            //    passedStatusUpdateList.Add(tempStatusUpdate);

        //            //}
        //            //string result = JsonConvert.SerializeObject(passedStatusUpdateList);
        //            string result = JsonConvert.SerializeObject(ProjectUpdates);
        //            return result;
        //        }
        //    } catch (Exception)
        //    {
        //        string emptyException = JsonConvert.SerializeObject("");
        //        return emptyException;
        //    }
        //    string empty = JsonConvert.SerializeObject("");
        //    return empty;

        //}
        /// <summary>
        /// This method get the project informatiion from GetAllUpdatesFromEmail in Data Access layer.
        /// It is  passing a json object to project-List.js which the format is clear from the foreach loop. 
        /// The route defined in routconfig file looks like :ProjectList/GetStatusData/{projectId}/{projectUpdateId}
        /// The controller for angular can be found in Project-List.js.  
        /// </summary>
        /// <param name="projectId">integer defined for each project</param>
        /// <param name="ProjectUpdateId"> integer defined for each email.</param>
        /// <returns>List of StatusUpdate objects in format of json</returns>
        public string GetStatusData(String projectId, String ProjectUpdateId)
        {
            var passedStatusUpdateList = new List<StatusUpdatesModel.StatusUpdate>();

            try
            {
                if (this.Session["username"].ToString() != null && DataAccsess.IsUserAuthorized(this.Session["username"].ToString()))
                {

                    var statusData = DataAccsess.GetAllUpdatesFromEmail(Guid.Parse(ProjectUpdateId));
                  
                    foreach (StatusUpdatesModel.StatusUpdate passedStatusUpdate in statusData)
                    {
                        StatusUpdatesModel.StatusUpdate tempStatusUpdate = new StatusUpdatesModel.StatusUpdate();
                        //tempStatusUpdate.Phase = passedStatusUpdate.Phase;
                        tempStatusUpdate.PhaseID = passedStatusUpdate.PhaseID;
                        //tempStatusUpdate.Project = passedStatusUpdate.Project;
                        tempStatusUpdate.ProjectID = passedStatusUpdate.ProjectID;
                        tempStatusUpdate.ProjectName = passedStatusUpdate.ProjectName;
                        tempStatusUpdate.RecordDate = passedStatusUpdate.RecordDate;
                        tempStatusUpdate.ProjectUpdateID = passedStatusUpdate.ProjectUpdateID;
                        tempStatusUpdate.UpdateKey = passedStatusUpdate.UpdateKey;
                        tempStatusUpdate.UpdateValue = passedStatusUpdate.UpdateValue;
                        //tempStatusUpdate.Vertical = passedStatusUpdate.Vertical;
                        tempStatusUpdate.VerticalID = passedStatusUpdate.VerticalID;
                        passedStatusUpdateList.Add(tempStatusUpdate);

                    }
                   
                }
            }
            catch (Exception)
            {
                string emptyException = JsonConvert.SerializeObject("");
                return emptyException;
            }
            string result = JsonConvert.SerializeObject(passedStatusUpdateList);
            return result;


        }
        /// <summary>
        /// This method get all information about specific project from GetProjectUpdates on Data Access layer based on the projectID.
        /// The URL defined on route config look like ProjectList/GetprojectUpdates/{projectID}. 
        /// The json format is clear from the foreach loop and the controller for angular can be found on Project-List.js.
        /// </summary>
        /// <param name="projectID">Guid ProjectID as string</param>
        /// <returns>Returns a string includes list of ProjectUpdates for the specified Project</returns>
        public string GetprojectUpdates(string projectID)
        {
            //var ProjectUpdateKeys = DataAccsess.GetUpdatesForKey(projectID);
            //string result = JsonConvert.SerializeObject(ProjectUpdateKeys);
            //return result;
            var passedStatusUpdateList = new List<StatusUpdatesModel.ProjectUpdate>();
            try
            {
                if (this.Session["username"].ToString() != null && DataAccsess.IsUserAuthorized(this.Session["username"].ToString()))
                {
                    var statusData = DataAccsess.GetProjectUpdates(projectID);
                    

                    foreach (StatusUpdatesModel.ProjectUpdate passedStatusUpdate in statusData)
                    {
                        StatusUpdatesModel.ProjectUpdate tempStatusUpdate = new StatusUpdatesModel.ProjectUpdate();
                        tempStatusUpdate.Phase = passedStatusUpdate.Phase;
                        tempStatusUpdate.PhaseID = passedStatusUpdate.PhaseID;
                        tempStatusUpdate.ProjectID = passedStatusUpdate.ProjectID;
                        tempStatusUpdate.Date = passedStatusUpdate.Date;
                        //tempStatusUpdate.Project = passedStatusUpdate.Project;
                        tempStatusUpdate.Subject = passedStatusUpdate.Subject;
                        tempStatusUpdate.Phase = passedStatusUpdate.Phase;
                        tempStatusUpdate.Environment = passedStatusUpdate.Environment;
                        tempStatusUpdate.Description = passedStatusUpdate.Description;
                        tempStatusUpdate.Body = passedStatusUpdate.Body;
                        tempStatusUpdate.Project = passedStatusUpdate.Project;
                        tempStatusUpdate.StatusUpdates = passedStatusUpdate.StatusUpdates;
                        //tempStatusUpdate.Vertical = passedStatusUpdate.Vertical;
                        
                        tempStatusUpdate.ProjectUpdateID = passedStatusUpdate.ProjectUpdateID;
                        passedStatusUpdateList.Add(tempStatusUpdate);

                    }
                }
            }
            catch (Exception e)
            {
                string emptyException = JsonConvert.SerializeObject("");
                return emptyException;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            string result = JsonConvert.SerializeObject(passedStatusUpdateList, settings);
            return result;

        }
        //
        // POST: /Account/ExternalLogin
        //[HttpPost]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    Request a redirect to the external login provider
        //    return null;
        //}


    }
}
