using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostcoSU.Models
{
    public class StatusUpdate
    {
        private string projectID;

        public string ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }

        private int phaseID;

        public int PhaseID
        {
            get { return phaseID; }
            set { phaseID = value; }
        }

        private int verticalID;

        public int VerticalID
        {
            get { return verticalID; }
            set { verticalID = value; }
        }


        private string information;

        public string Information
        {
            get { return information; }
            set { information = value; }
        }

        private Dictionary<string, string> parsedInformation;

        public Dictionary<string, string> ParsedInformation
        {
            get { return parsedInformation; }
            set { parsedInformation = value; }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
    }
}
