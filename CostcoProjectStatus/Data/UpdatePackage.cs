using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService
{
    [Serializable]
    public class UpdatePackage
    {
        public string ProjectName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        private Dictionary<string, string > updates  = new Dictionary<string, string>();
        public Dictionary<string, string> Updates {
            get { return updates; }
            set { updates = value; } }
    }
}
