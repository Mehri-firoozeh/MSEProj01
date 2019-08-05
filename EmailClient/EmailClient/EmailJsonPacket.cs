using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class EmailJsonPacket
    {
        public string AppId;
        public List<EmailJsonObject> StatusUpdateList;
    }
}
