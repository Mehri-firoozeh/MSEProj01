using AE.Net.Mail;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Collections.Specialized;


namespace EmailClient
{
    class ImapService
    {
        private string _imapServer;
        private string _userId;
        private string _password;
        public ImapService()
        {
            _imapServer = ConfigurationManager.AppSettings["ImapServer"];
            _userId = ConfigurationManager.AppSettings["UserId"];
            _password = ConfigurationManager.AppSettings["Password"];
        }

        /// <summary>
        /// Function Name : ParseNewEmail
        /// Input Parameters : none  
        /// Input Parameter Type : none
        /// Description:This method is going to record the event when an email is received to the Test email. It uses ImapClient which is available in AE.Net.Mail Nuget package.
        /// ImapClient has inbuilt properties to extract subject, body, sender information details from a newly received email
        /// </summary>
        /// <returns>
        /// Return Parameter : emailJson 
        /// Return Parameter Type : string
        /// </returns>

        public string ParseNewEmail()
        {
            // Connect to the IMAP server. The 'true' parameter specifies to use SSL, which is important (for Gmail at least)
            ImapClient imapClient = new ImapClient(ConfigurationManager.AppSettings["ImapServer"], ConfigurationManager.AppSettings["UserId"], ConfigurationManager.AppSettings["Password"], AuthMethods.Login, 993, true);
            var userName = ConfigurationManager.AppSettings["UserID"];
          //  ImapClient imapClient = new ImapClient(ConfigurationManager.AppSettings["ImapServer"], "jayasreetestemail@gmail.com", "7Ywy7N[S", AuthMethods.Login, 993, true);
            // Select a mailbox. Case-insensitive
            imapClient.SelectMailbox("INBOX");
            string emailJson="";           
            Console.WriteLine(imapClient.GetMessageCount());

            imapClient.NewMessage += (sender, e) =>
            {
                var msg = imapClient.GetMessage(e.MessageCount - 1);                             
                string result = "";
                UpdatePackage up = new UpdatePackage();               
                List<string> li = new List<string>();
                up.Subject = msg.Subject;
                up.Body = msg.Body;
                up.Updates = ParseBody(msg.Body);
                li = ParseSubject(msg.Subject);
                up.ProjectName = li[li.Count - 1];
                up.Updates.Add("PhaseId", li[1]);
                up.Updates.Add("VerticalId", li[2]);
                emailJson = JsonConvert.SerializeObject(up);
               
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                   // result = client.UploadString("https://localhost:44300/ProjectUpdate/Update", "Post", emailJson);
                    result = client.UploadString("http://costcodevops.azurewebsites.net/ProjectUpdate/Update", "Post", emailJson);
                    Console.WriteLine(result);
                }

            };
           return emailJson;           
        }

        /// <summary>
        /// Function Name : ParseSubject
        /// Input Parameters : Subject
        /// Input Parameter Type : string
        /// Description: This function is going to parse the subject of the email and returns a list of strings.
        /// </summary>
        /// <param name="Body"></param>
        /// <returns>
        /// Returns Parameter : li
        /// Returns Parameter type: List<string>
        /// </returns>

        public List<string> ParseSubject(string Subject)
        {
            List<string> li = new List<string>();
            string[] sub = Subject.Split('|');
            li.Add(sub[0]); // project Id
            li.Add(sub[1]); // Phase Id
            li.Add(sub[2]); // Vertical Id
            li.Add(sub[3]); // Project Name
            return li;

        }

        /// <summary>
        /// Function Name : ParseBody
        /// Input Parameters : Body  
        /// Input Parameter Type : string
        /// Description: This function is going to parse the body of the email and returns key value pairs.
        /// </summary>
        /// <param name="Body"></param>
        /// <returns>
        /// Returns Parameter : dict
        /// Returns Parameter type: Dictionary
        /// </returns>

        public Dictionary<string, string> ParseBody(string Body)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] body = Body.Split('|');
            foreach (string s in body)
            {
                string[] temp = s.Split(':');
                dict.Add(temp[0], temp[1]);
                //Console.WriteLine(temp[0]);
                //Console.WriteLine(temp[1]);
            }

            return dict;
        }


    }
}
