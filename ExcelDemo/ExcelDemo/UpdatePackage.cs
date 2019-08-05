using System;
using System.Collections.Generic;

namespace ExcelDemo
{
    [Serializable]
    public class UpdatePackage
    {
        public string ProjectName;
        public string Subject;
        public string Body;

        public Dictionary<string, string> Updates = new Dictionary<string, string>();
    }
}
