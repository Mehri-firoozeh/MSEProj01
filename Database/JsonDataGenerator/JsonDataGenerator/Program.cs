using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using DataService;
using StatusUpdatesModel;

namespace JsonDataGenerator
{
    class Program
    {
        private static int numberOfProjectsToGenerate = 12;
        private static string errorMessage = "";

        private static bool inDebug = true;

        static void Main(string[] args)
        {

            if (inDebug)
            {
                AccessService dbService = new AccessService();
                //
                var projectUpdates = dbService.GetProjectUpdates("0a9514be-0e61-4ffc-9d7b-8e151a126038");
                UpdatePackage package = new UpdatePackage();
                package.ProjectName = "we done testing";
                package.Subject = "Deployment";
                package.Body = "Environment:br549|Jimmy, toloose";

                package.Updates.Add("VerticalID",  "-1");
                package.Updates.Add( "PhaseID", "0");
                package.Updates.Add( "Environment",  "Joe's Computer");
                package.Updates.Add("Some Key","Some Value" );

                dbService.RecordUpdatePackage(package);
                var db = 1;

            }

            //AccessService dataAccess = new AccessService();
            int actionOption = 1;
           

            while (actionOption > 0)
            {
                Console.WriteLine("Select action to take:");
                Console.WriteLine("--> 1) Clear all data");
                Console.WriteLine("--> 2) Create sample data");
                Console.WriteLine("--> 3) Delete Project by ID");
                Console.WriteLine("--> 4) Delete Project by Name");
                Console.WriteLine("--> 5) Exit application");
                Console.WriteLine("??\n");

                try
                {
                    string input = Console.ReadLine();
                    actionOption = Convert.ToInt16(input);
                }
                catch (Exception)
                {

                    Console.WriteLine("Integers only please");
                    Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-\n");
                    continue;
                }
                bool success = false;
                switch (actionOption)
                {
                    case 1:
                        success = deleteAllData();
                        if (!success)
                        {
                            Console.WriteLine("Problem deleting data: " + errorMessage);
                            errorMessage = "";
                        }
                        break;
                    case 2:
                        success = writeSampleData();
                        if (!success)
                        {
                            Console.WriteLine("Problem writing sample data: " + errorMessage);
                            errorMessage = "";
                        }
                        break;
                    case 3:
                        Console.WriteLine("Enter ProjectID Guid");
                        string projectID = Console.ReadLine();
                        try
                        {
                            deleteProject(projectID, null);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error deleting project, " + e.Message);
                            continue;
                        }
                        break;
                    case 4:
                        Console.WriteLine("Enter Project Name");
                        string projectName = Console.ReadLine();
                        try
                        {
                            deleteProject(null, projectName);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error deleting project, " + e.Message);
                            continue;
                        }
                        break;
                    case 5:
                        return;
                    default:
                        return;
                }
                success = false;
            }
        }

        private static void deleteProject(string projectIDstring = null, string projectName = null)
        {
            AccessService dataService = new AccessService();
            Guid projectID = new Guid();
            if (string.IsNullOrEmpty(projectIDstring) )
            {
                projectID = dataService.GetProjectIDbyName(projectName);
            }
            else
            {
                try
                {
                    projectID = new Guid(projectIDstring);
                }
                catch (Exception)
                {
                    Console.WriteLine("Problem creating Guid, delete has failed");
                    return;
                }
            }

            if (projectID == Guid.Empty)
            {
                Console.WriteLine("Problem getting ID for " + projectName);
                return;
            }

            projectName = dataService.GetProjectNameForID(projectID);
            int updatesRemoved = 0;
            var context = createContext();
            List<StatusUpdate> updatesToRemove = context.StatusUpdates.Where(u => u.ProjectID == projectID).ToList();
            updatesRemoved = updatesToRemove.Count;
            context.StatusUpdates.RemoveRange(updatesToRemove);
            context.SaveChanges();
            Console.WriteLine("Removed " + updatesRemoved + " updates from Status Update Table");

            List<ProjectPhase> projectPhases = context.ProjectPhases.Where(p => p.ProjectID == projectID).ToList();

            int phaseEntryCount = projectPhases.Count(p => p.ProjectID == projectID);
            context.ProjectPhases.RemoveRange(projectPhases);
            context.SaveChanges();
            Console.WriteLine("Removed " + phaseEntryCount + " ProjectPhase entries");

            Project projectToDelete = context.Projects.FirstOrDefault(p => p.ProjectID == projectID);
            context.Projects.Remove(projectToDelete);
            context.SaveChanges();
            Console.WriteLine("Project " + projectName + " with ID=" + projectID + " is removed");
        
        }

        private static bool deleteAllData()
        {
            try
            {
                CostcoDevStatusEntities context = createContext();
                IEnumerable<StatusUpdate> updates = context.StatusUpdates;
                foreach (StatusUpdate update in updates)
                {
                    Console.WriteLine("Removing Status Update " + update.ProjectName + "," + update.UpdateKey);
                    context.StatusUpdates.Remove(update);
                }
                context.SaveChanges();

                IEnumerable<ProjectPhase> statusSequences = context.ProjectPhases;
                foreach (ProjectPhase statusSequence in statusSequences)
                {
                    Console.WriteLine("Removing ProjectPhase record " + statusSequence.Project.ProjectName + "_"
                        + statusSequence.UpdateKey);
                    context.ProjectPhases.Remove(statusSequence);
                }
                context.SaveChanges();

                IEnumerable<Project> projects = context.Projects;
                foreach (Project project in projects)
                {
                    Console.WriteLine("Removing Project " + project.ProjectName);
                    context.Projects.Remove(project);
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
            return true;
        }

        private static CostcoDevStatusEntities createContext()
        {
            const string ConnectionString = "Server=tcp:costcosu.database.windows.net,1433;Database=CostcoDevStatus;User ID=SUAdmin@costcosu;Password=39ffbJeo;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            CostcoDevStatusEntities context = CostcoDevStatusEntities.Create(ConnectionString);
            return context;
        }

        private static bool writeSampleData()
        {
            try
            {
                int totalRecords = 0;
                DateTime start = DateTime.Now;
                AccessService dataAccess = new AccessService();
                List<ProjectUpdate> projects = UpdateGenerator.GenerateUpdates(numberOfProjectsToGenerate);
                int numberOfProjects = projects.Count;
                foreach (ProjectUpdate project in projects)
                {
                    List<StatusUpdate> updates = project.StatusUpdates.ToList();
                    totalRecords += updates.Count;
                    dataAccess.RecordStatusUpdate(updates);
                    Console.WriteLine("Recorded " + updates.Count + " updates for Project ");
                }
                int durationInMinutes = (DateTime.Now - start).Minutes;
                Console.WriteLine("Recorded " + totalRecords + " for " + numberOfProjects + " projects in " + durationInMinutes + "m");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Console.Write("*******Error: " + errorMessage + "\n");
                Console.Write("*******InnerException: " + e.InnerException + "\n");
                Console.WriteLine("*********Detail: " + e.InnerException.Message + "\n");
                return false;
            }
            return true;

        }
    }
}
