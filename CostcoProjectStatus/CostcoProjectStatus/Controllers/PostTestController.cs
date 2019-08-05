//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using DataService;
//using Newtonsoft.Json;

//namespace CostcoProjectStatus.Controllers
//{
//    public class PostTestController : Controller
//    {

//        private AccessService DataAccsess = new AccessService();

//        // GET: PostTest
//        public ActionResult Index()
//        {
//            return View();
//        }
//        [HttpPost]
//        public string GetStatusUpdates(String id)
//        { 
      
//            var ProjectUpdates = DataAccsess.GetAllUpdatesForProject(id);
//            string result = JsonConvert.SerializeObject(ProjectUpdates);
//            return result;
//        }
//    }
//}