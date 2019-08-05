using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Office = Microsoft.Office.Core;
using ExcelTools = Microsoft.Office.Tools.Excel;
using ExcelIO = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace ExcelDemo
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            ExcelIO.Application excelApp;
            ExcelIO.Worksheet activeSheet;

            try
            {
                excelApp = Globals.ThisAddIn.Application;
                activeSheet = excelApp.ActiveSheet;
            }
            catch (Exception)
            {
                MessageBox.Show("Problem Getting Workbooks & Sheets");
                return;
            }
            ExcelIO.Range usedRange;
            try
            {

                usedRange = activeSheet.UsedRange;

            }
            catch (Exception)
            {

                MessageBox.Show("Nothing Selected to Publish");
                return;
            }

            int iNumColumns = usedRange.Columns.Count;
            int iNumRows = usedRange.Rows.Count;
            //__validate IDs
            //__validate PhaseID
            //__validate VerticalID

            //__get keys from header row. first column is Project Name
            Dictionary<int, string> keys = new Dictionary<int, string>();
            List<UpdatePackage> updates = new List<UpdatePackage>();
            for (int i = 2; i <= iNumColumns; i++)
            {
                ExcelIO.Range cell = usedRange.Cells[1, i];
                keys.Add(i, cell.Value.ToString());
            }

            //__skip header row, get values
            for (int i = 2; i <= iNumRows; i++)
            {
                UpdatePackage package = new UpdatePackage();
                ExcelIO.Range cell = usedRange.Cells[i, 1];
                package.ProjectName = cell.Value.ToString();
                package.Subject = "Excel Update";

                //__remainder of columns should be key/value pairs
                for (int k = 2; k <= iNumColumns; k++)
                {

                    cell = usedRange.Cells[i, k];
                    package.Updates.Add(keys[k], cell.Value.ToString());
                    package.Body += keys[k] + ":" + cell.Value.ToString();
                    if (k != iNumColumns) package.Body += "\n";
                }
                updates.Add(package);
            }

            int updateCount = updates.Count;
            string result = "";
            bool hadError = false;
            for (int i = 0; i < updateCount; i++)
            {

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(updates[i]);
                try
                {

                    using (var client = new WebClient())
                    {
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        string url = "http://costcodevops.azurewebsites.net/ProjectUpdate/Update";
//#if DEBUG
//                        url = "https://localhost:44300/ProjectUpdate/Update";
//#endif 
                        
                        result = client.UploadString(url,  json);

                        Console.WriteLine(result);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message+ "\n" + result);
                    hadError = true;
                    break;
                }
            }

            if (! hadError) MessageBox.Show("Successfully posted " + updateCount + " updates to CostcoDevOps Azure");
        }
    }

    //public partial class StatusUpdate
    //{
    //    public System.Guid ProjectID { get; set; }
    //    public string ProjectName { get; set; }
    //    public int PhaseID { get; set; }
    //    public int StatusSequence { get; set; }
    //    public Nullable<int> VerticalID { get; set; }
    //    public Nullable<System.DateTime> RecordDate { get; set; }
    //    public string UpdateKey { get; set; }
    //    public string UpdateValue { get; set; }
    //}
    public class StatusUpdatePacket
    {
        public string AppId { get; set; }
        public List<StatusUpdate> StatusUpdateList { get; set; }
    }
}
