using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class appObject
    {
        public Guid ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string PhaseID { get; set; }
        public string VerticalID { get; set; }
        public string UpdateKey { get; set; }
        public string UpdateValue { get; set; }
        public DateTime RecordedDate { get; set; }
    }
}
