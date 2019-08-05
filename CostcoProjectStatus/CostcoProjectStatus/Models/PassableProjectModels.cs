using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CostcoProjectStatus.Models
{
    public class PassableProjectModel
    {
         public string ProjectID { get; set; }
        public string ProjectName { get; set; }
         public DateTime LatestUpdate { get; set; }
         public int? VerticalID { get; set; } 
    }
}