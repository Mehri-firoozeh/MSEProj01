using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // This code has to run in an endless loop as per Microsoft Webjob's website:
            // "Code for a continuous job needs to be written to run in an endless loop."
            // https://azure.microsoft.com/en-us/documentation/articles/web-sites-create-web-jobs/
            int temp = 0;
            while (temp < 1)
            {
                var emailObject = new ImapService().ParseNewEmail();
                Console.ReadLine();
            }
        }
    }
}
