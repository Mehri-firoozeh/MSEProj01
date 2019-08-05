using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataService;

namespace CostcoProjectStatus.Models
{
    [Serializable]
    public class UpdatePackageModel
    {

        public UpdatePackageModel()
        {
            Updates = new Dictionary<string, string>();
        }

        public string ProjectName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public Dictionary<string, string> Updates { get; set; }

        public UpdatePackage GetUpdatePackage()
        {
            UpdatePackage updatePackage = new UpdatePackage();
            updatePackage.ProjectName = ProjectName;
            updatePackage.Subject = Subject;
            updatePackage.Body = Body;
            updatePackage.Updates = Updates;
            return updatePackage;
        }


    }

}