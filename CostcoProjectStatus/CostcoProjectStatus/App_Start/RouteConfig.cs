using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CostcoProjectStatus
{
    public class RouteConfig
    {/// <summary>
     /// All the URLs are intorduced on this file. As it is clear the route can get tracked based on the name of controller
     /// and the name of the action which is the name of the method defined in each controller. The default format of each URL
     /// "{controller}/{action}/{id}" controller: Nmae of the controller in CotcoPrjectStatus>>controllers folder. 
     /// Action: name of the method. id: the parameter passing through the URL.
     /// </summary>
     /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {

            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "StatusUpdateList",
                "ProjectList/GetprojectUpdates/{projectID}",
                new { Controller = "ProjectList", action = "GetprojectUpdates", projectID = "projectID" }
                );
            routes.MapRoute(
                "StatusDataList",
                "ProjectList/GetStatusData/{projectId}/{projectUpdateId}",
                new { Controller = "ProjectList", action = "GetStatusData", projectId = "projectId", ProjectUpdateId = "projectUpdateId" }
                );
            routes.MapRoute(
                "VerticalList",
                "Vertical/GetAllVertical",
                 new { Controller = "Vertical", action = "GetAllVertical" }
                );
            routes.MapRoute(
                "VerticalProjects",
                "Vertical/GetVerticalProjects/{VerticalId}",
                 new { Controller = "Vertical", action = "GetVerticalProjects", VerticalId = "VerticalId" }
                );
            routes.MapRoute(
                "ProjectUpdate",
                "ProjectUpdate/Update",
                 new { Controller="ProjectUpdate",action="Update"}
                );
            routes.MapRoute(
               "ProjectUpdatePhase",
               "ProjectUpdate/UpdatePhase",
                new { Controller = "ProjectUpdate", action = "UpdatePhase" }
               );
            routes.MapRoute(
                "LoginCheck",
                "Account/IsLogged",
                new { Controller = "Account", action = "IsLogin" }
                );
            routes.MapRoute(
                "LogOff",
                "Account/LogOff",
                new { Controller = "Account", action = "LogOff" }
               );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ProjectList", action = "Display", id = "" }
            );


        }
    }
}
