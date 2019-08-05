using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelDemo
{

    public partial class StatusUpdate
    {
        public System.Guid ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int PhaseID { get; set; }
        public int StatusSequence { get; set; }
        public Nullable<int> VerticalID { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public string UpdateKey { get; set; }
        public string UpdateValue { get; set; }
    }
}

