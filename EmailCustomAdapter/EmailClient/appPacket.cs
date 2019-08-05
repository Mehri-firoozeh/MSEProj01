using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class appPacket
    {
       
         public string AppId { get; set; }
         public List<appObject> StatusUpdateList = new List<appObject>();
        
    }
}
