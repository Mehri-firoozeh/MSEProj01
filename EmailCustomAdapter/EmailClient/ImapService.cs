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
        public string ApplicationName = "";
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
            imapClient.NewMessage += (sender, e) =>
            {
                var msg = imapClient.GetMessage(e.MessageCount - 1);
                UpdatePackage up = new UpdatePackage();
                up.Updates = ParseBody(msg.Body);
                up.Subject = msg.Subject;
                up.Body = msg.Body;
                up.ProjectName = ApplicationName;              
               
              emailJson = JsonConvert.SerializeObject(up);                          
              string result = "";
              using (var client = new WebClient())
              {
                  client.Headers[HttpRequestHeader.ContentType] = "application/json";
                 // result = client.UploadString("https://localhost:44300/ProjectUpdate/Update", "Post", emailJson);
                  result = client.UploadString("https://costcodevops.azurewebsites.net/ProjectUpdate/Update", "Post", emailJson);
                  Console.WriteLine(result);
              }
              
               
            };           

           return emailJson;           
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

        public Dictionary<string,string> ParseBody(string Body)
        {
            int counter = 1;
            string line;
            string summary = "";
            var dict = new Dictionary<string, string>();
            System.IO.File.WriteAllText("Test.txt",Body);
            System.IO.StreamReader file =
            new System.IO.StreamReader("Test.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals("Application:"))
                {
                    ApplicationName = getNextLine(counter);
                }
                
                if (line.Equals("Application:") || line.Equals("Process:") || line.Equals("Environment:") || line.Equals("Requested By:") || line.Equals("Requested On:")||line.Equals("Description:"))
                {
                    // Console.WriteLine(getNextLine(counter)); 
                     dict.Add(line, getNextLine(counter));
                  //  list.Add(new KeyValuePair<string, string>(line, getNextLine(counter)));        
                              
                }
               
                else
                {
                    summary += line;
                }
                counter++;
            }
                   
            file.Close();
            return dict;         
     
        }

        /// <summary>
        /// Function Name : getNextLine
        /// Input Parameters : lineNumber  
        /// Input Parameter Type : Integer
        /// Description : This function is going read the email line by line and return the next line of give line number
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <returns>
        /// Return Parameter: nextLine
        /// Return Parameter Type : String
        /// </returns>
        private string getNextLine(int lineNumber)
        {
            // using will make sure the file is closed
            string nextLine;
            using (System.IO.StreamReader file = new System.IO.StreamReader("Test.txt"))
            {
                // Skip lines
                for (int i = 0; i <= lineNumber; ++i)
                    file.ReadLine();

                // Store your line
               nextLine=file.ReadLine();
               // LastLineNumber++;
            }
            return nextLine;
        }


 





    }
}
