using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

using StatusUpdatesModel;
using Newtonsoft.Json;


namespace DataService
{
    public class AccessService
    {
        private CostcoDevStatusEntities context;
        private const string ConnectionString = "Server=tcp:costcosu.database.windows.net,1433;Database=CostcoDevStatus;User ID=SUAdmin@costcosu;Password=39ffbJeo;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public AccessService()
        {
            context = CostcoDevStatusEntities.Create(ConnectionString);

        }




        #region Authentication Methods
        public bool AddUser(string email, int userRole)
        {

            if (context.AllowedUsers.Any(a => a.Email == email)) return false;
            try
            {

                AllowedUser newUser = new AllowedUser()
                {
                    Email = email,
                    UserID = Guid.NewGuid(),
                    RoleID = userRole
                };
                context.AllowedUsers.Add(newUser);
                context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool IsUserAuthorized(string email)
        {
            return context.AllowedUsers.Any(u => u.Email == email);
        }
        public bool IsAppAuthorized(string app)
        {
            return context.AllowedApps.Any(u => u.Name == app);
        }

        /// <summary>
        /// Checks for the user in the database. If found RoleID (0,1,2) is returned
        /// If user is not found, -1 is returned
        /// </summary>
        /// <param name="email"></param>
        /// <returns>int indicating user role, or -1</returns>
        public int GetUserRole(string email)
        {
            int result = -1;
            AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                result = user.RoleID;
            }
            return result;
        }


        public bool UpdateUserRole(string email, int newUserRole)
        {
            if (!context.AllowedUsers.Any(a => a.Email == email)) return false;
            try
            {

                AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.Email == email);
                user.RoleID = newUserRole;
                context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool DeleteUser(string email)
        {
            try
            {
                AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    context.AllowedUsers.Remove(user);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateUserEmail(string oldEmail, string newEmail)
        {
            AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.Email == oldEmail);
            if (user == null) return false;
            user.Email = newEmail;
            context.SaveChanges();
            return true;
        }

        public bool UpdateUserEmail(Guid userID, string newEmail)
        {
            AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.UserID == userID);
            if (user == null) return false;
            user.Email = newEmail;
            context.SaveChanges();
            return true;
        }

        public Guid GetUserID(string email)
        {
            AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.Email == email);
            if (user == null) return Guid.Empty;
            return user.UserID;
        }
        #endregion


        #region StatusUpdate and Project Methods
        /// <summary>
        /// This takes an UpdatePackage, which represents an email, and records it to the database
        /// </summary>
        /// <param name="package">An UpdatePackage is a normalized email</param>
        /// <returns>Guid of updated project as a string. 
        /// Useful if this project is new or caller wants to retreive other data on that project</returns>
        public string RecordUpdatePackage(UpdatePackage package)
        {
            //__get the information from input
            string projectName = package.ProjectName;
            string subject = package.Subject;
            string body = package.Body;
            if (string.IsNullOrEmpty(projectName)) return null;

            Dictionary<string, string> updatePairs = package.Updates;

            Project project = context.Projects.FirstOrDefault(p => p.ProjectName == projectName);

            //__if no existing project, create new one
            bool madeNewProject = false;
            if (project == null)
            {
                project = new Project();
                project.ProjectID = Guid.NewGuid();
                madeNewProject = true;
            }

            //__get the Project ID to use locally
            Guid projectID = project.ProjectID;

            //__Look for VerticalID
            int verticalID = -1;//__default is not assigned.
            try
            {
                KeyValuePair<string, string> verticalPair = updatePairs.FirstOrDefault(u => u.Key.ToLower() == "verticalid");
                if (verticalPair.Value != null) verticalID = Convert.ToInt16(verticalPair.Value);
                if (verticalID < -1 || verticalID > 8) verticalID = -1;
            }
            catch (Exception)
            {
                //__just use default value 
                verticalID = -1;
            }

            //__these might be new or changed
            project.VerticalID = verticalID;
            project.ProjectName = projectName;

            //__Look for a PhaseID
            int phaseID = -1;
            try
            {
                KeyValuePair<string, string> phasePair = updatePairs.FirstOrDefault(u => u.Key.ToLower() == "phaseid");
                if (phasePair.Value != null) phaseID = Convert.ToInt16(phasePair.Value);

                if (phaseID < -1 || phaseID > 7) phaseID = -1;

            }
            catch (Exception)
            {
                //_simply use default
                phaseID = -1;
            }

            //__do fuzzy word matches if no phase found yet
            if (phaseID < 0)
            {
                string searchString = subject + body;
                phaseID = Convert.ToInt16(PhaseKeywords.GuessPhase(searchString));
            }

            //__if this is new Project write it to DB
            if (madeNewProject)
            {
                context.Projects.Add(project);
                context.SaveChanges();
            }

            //__create and record new ProjectUpdate
            ProjectUpdate projectUpdate = new ProjectUpdate();
            projectUpdate.ProjectUpdateID = Guid.NewGuid();
            projectUpdate.ProjectID = projectID;
            projectUpdate.Subject = subject;
            projectUpdate.Body = body;
            context.ProjectUpdates.Add(projectUpdate);
            context.SaveChanges();

            //__build and record StatusUpdates from list of Key:Value pairs
            //__also make sure to update ProjectPhase table for efficient queries later
            StatusUpdate statusUpdateTemplate = new StatusUpdate();
            statusUpdateTemplate.ProjectID = projectID;
            statusUpdateTemplate.ProjectUpdateID = projectUpdate.ProjectUpdateID;
            statusUpdateTemplate.PhaseID = phaseID;
            statusUpdateTemplate.VerticalID = verticalID;

            //__safety, incase of duplicate keys, combine the values so there is only one entry
            //___trying to record duplicate keys in the same PackageUpdate will cause primary key error in DB
            Dictionary<string, string> cleanedPairs = combineEqualKeys(updatePairs);

            foreach (var pair in cleanedPairs)
            {
                string key = pair.Key;
                string value = pair.Value;

                StatusUpdate statusUpdate = statusUpdateTemplate.Clone();
                statusUpdate.UpdateKey = key;
                statusUpdate.UpdateValue = value;
                statusUpdate.RecordDate = DateTime.Now;
                context.StatusUpdates.Add(statusUpdate);
                context.SaveChanges();

                updateProjectPhase(projectID, phaseID, key);
            }
            return projectID.ToString();
        }

        /// <summary>
        /// This internal method is used to prevent duplicate keys from causing issues in the database
        /// The value from duplicate keys is concatenated
        /// </summary>
        /// <param name="updatePairs">the list of key-value pairs to remove duplicates from</param>
        /// <returns>A Dictionary with no duplicate keys and no lost information</returns>
        private Dictionary<string, string> combineEqualKeys(Dictionary<string, string> updatePairs)
        {
            Dictionary<string, string> combinedKeys = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in updatePairs)
            {
                string key = pair.Key;
                string value = pair.Value;

                if (combinedKeys.ContainsKey(key))
                {
                    combinedKeys[key] = combinedKeys[key] + "|" + value;
                }
                else
                {
                    combinedKeys.Add(key, value);
                }
            }
            return combinedKeys;
        }

        /// <summary>
        /// Currently (5/24/16) not utilized. This method will scrub a Project and all related entries from the DB
        /// </summary>
        /// <param name="projectName">Names in the system are unique identifiers</param>
        public void DeleteProject(string projectName)
        {
            Guid projectID = context.Projects.FirstOrDefault(p => p.ProjectName == projectName).ProjectID;
            DeleteProject(projectID);
        }


        /// <summary>
        /// Internal method to update ProjectPhase entry when a new StatusUpdate changes the update count
        /// ProjectPhase table is no longer utilized by current UI and this should likely be removed during refactoring
        /// </summary>
        /// <param name="projectId">ProjectID is part of the primary key of each ProjectPhase entry</param>
        /// <param name="phaseId">PhaseID is part of the primary key of each ProjectPhase entry</param>
        /// <param name="key">UpdateKey is part of the primary key of each ProjectPhase entry</param>
        private void updateProjectPhase(Guid projectId, int phaseId, string key)
        {
            ProjectPhase projectPhase = context.ProjectPhases.FirstOrDefault(pp =>
                pp.PhaseID == phaseId &&
                pp.ProjectID == projectId &&
                pp.UpdateKey == key);
            //__increment or create entry
            if (projectPhase != null)
            {
                int? newCount = projectPhase.UpdateCount + 1;
                projectPhase.UpdateCount = newCount;
            }
            else
            {
                ProjectPhase newProjectPhase = new ProjectPhase();
                newProjectPhase.PhaseID = phaseId;
                newProjectPhase.ProjectID = projectId;
                newProjectPhase.UpdateKey = key;
                newProjectPhase.LatestUpdate = DateTime.Now;
                newProjectPhase.UpdateCount = 1;
                context.ProjectPhases.Add(newProjectPhase);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Deprecated and not currently referenced
        /// </summary>
        /// <param name="projectUpdate"></param>
        /// <returns></returns>
        public bool RecordProjectUpdate(ProjectUpdate projectUpdate)
        {

            try
            {
                List<StatusUpdate> updates = projectUpdate.StatusUpdates.ToList();

                //__first make sure each StatusUpdate has necessary info'
                Guid projectUpdateId = Guid.NewGuid();
                foreach (StatusUpdate statusUpdate in updates)
                {
                    statusUpdate.ProjectUpdateID = projectUpdateId;
                }

                //__create new entry in ProjectUpdate table
                projectUpdate.ProjectUpdateID = projectUpdateId;
                context.ProjectUpdates.Add(projectUpdate);
                context.SaveChanges();

                //__use existing method to record StatusUpdates
                RecordStatusUpdate(updates);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deprecated, only referenced by sample data generator, deprecated RecordProjectUpdate, and unit tests
        /// </summary>
        /// <param name="updates">Method is deprecated</param>
        /// <returns>Method is deprecated</returns>
        public bool? RecordStatusUpdate(List<StatusUpdate> updates)
        {
            //__safety check, cannot record an empty list
            if (updates.Count == 0) return null;
            StatusUpdate refUpdate = updates[0];

            //__check for existence of this project by ID, Name
            Guid projectID = refUpdate.ProjectID;
            string projectName = refUpdate.ProjectName;
            int? verticalID = refUpdate.VerticalID;
            if (verticalID == null || verticalID > 7) verticalID = -1;
            int? phaseID = refUpdate.PhaseID;
            if (phaseID == null || phaseID > 6) phaseID = -1;
            bool hasID = projectID != Guid.Empty;
            bool hasName = !string.IsNullOrEmpty(projectName);
            if (!hasID && !hasName) return null;//__cannot record anonymous updates

            if (hasID)
            {
                Project recordedProject = context.Projects.FirstOrDefault(p => p.ProjectID == projectID);
                //__if no name is provided, use a placeholder

                if (recordedProject == null) //__must  be a new project
                {
                    if (!hasName) projectName = "**Name Not Set**";
                    context.Projects.Add(new Project()
                    {
                        ProjectID = projectID,
                        ProjectName = projectName,
                        VerticalID = verticalID
                    });
                }
                else //__project already exists, so simply check if we need to update the name
                {
                    if (recordedProject.ProjectName != projectName)
                        recordedProject.ProjectName = projectName;
                }
                context.SaveChanges();
            }
            else
            {
                //__reaching this code means we have a name but no ID
                //__see if any ID Is already recorded
                Project recordedProject = context.Projects.FirstOrDefault(p => p.ProjectName == projectName);
                if (recordedProject != null) projectID = recordedProject.ProjectID;
                else //__this must be new project, generate ID and record
                {
                    projectID = Guid.NewGuid();
                    context.Projects.Add(new Project()
                    {
                        ProjectID = projectID,
                        ProjectName = projectName,
                        VerticalID = verticalID
                    });
                }
                context.SaveChanges();
            }
            try
            {
                //__make sure all updates have proper ProjectID, PhaseID, and VerticalID as these might have 
                //___changed or been generated here
                foreach (StatusUpdate u in updates)
                {
                    u.ProjectID = projectID;
                    u.PhaseID = phaseID;
                    u.VerticalID = verticalID;
                    u.ProjectName = projectName;
                }


                DateTime currentDT = DateTime.Now;


                foreach (StatusUpdate u in updates)
                {


                    // check for existing entries for this Project & Phase & UpdateKey
                    ProjectPhase projectPhaseEntry = context.ProjectPhases.FirstOrDefault(
                        p => p.ProjectID == u.ProjectID &&
                        p.PhaseID == u.PhaseID &&
                        p.UpdateKey == u.UpdateKey);

                    if (projectPhaseEntry != null)//__an entry exists
                    {
                        //__update existing update count and use this for sequence number
                        int iOldCount = Convert.ToInt32(projectPhaseEntry.UpdateCount);
                        int iNewCount = iOldCount + 1;
                        projectPhaseEntry.UpdateCount = iNewCount;
                        projectPhaseEntry.LatestUpdate = currentDT;
                    }
                    else //__since none was found we need a new entry
                    {
                        context.ProjectPhases.Add(new ProjectPhase()
                        {
                            ProjectID = u.ProjectID,
                            PhaseID = Convert.ToInt16(u.PhaseID),
                            UpdateKey = u.UpdateKey,
                            UpdateCount = 0,
                            LatestUpdate = currentDT
                        });
                        context.SaveChanges();
                    }


                    if (u.ProjectID == Guid.Empty) u.ProjectID = projectID;
                    if (string.IsNullOrEmpty(u.ProjectName)) u.ProjectName = projectName;
                    if (u.VerticalID == null || u.VerticalID < 0 || u.VerticalID > 7) u.VerticalID = verticalID;
                    u.RecordDate = DateTime.Now;
                    //u.ProjectUpdateID = projectUpdateID; This needs to be recorded in RecordProjectUpdateMethod
                    Console.WriteLine("\n--Added Update| updateKey=" + u.UpdateKey + ", updateValue=" + u.UpdateValue);
                    context.StatusUpdates.Add(u);

                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        /// <summary>
        /// Deprecatd in favor of RecordUpdatePackage
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        private List<StatusUpdate> GetAllUpdatesForProjectPhase(string projectID, int phaseID)
        {
            Guid projectGuid = new Guid(projectID);
            List<StatusUpdate> updates = new List<StatusUpdate>();
            updates = context.StatusUpdates.Where(p => p.ProjectID == projectGuid && p.PhaseID == phaseID).ToList();

            //__now also write ProjectName to each update
            foreach (StatusUpdate update in updates)
            {
                update.ProjectName = context.Projects.FirstOrDefault(p => p.ProjectID == update.ProjectID).ProjectName;
            }
            return updates;
        }

        /// <summary>
        /// Reads all Projects currently stored in DB
        /// </summary>
        /// <returns>List of currently tracked Projects as Project objects</returns>
        public List<Project> GetAllProjectNames()
        {
            List<Project> projects = context.Projects.AsEnumerable().ToList();
            DateTime now = DateTime.Now;
            foreach (var project in projects)
            {
                List<ProjectUpdate> recordedProjectUpdates = context.ProjectUpdates.Where(pu => pu.ProjectID == project.ProjectID).ToList();
                if (recordedProjectUpdates.Count == 0) continue;

                IEnumerable<DateTime> dates = recordedProjectUpdates.Select(pu => Convert.ToDateTime(pu.Date));
                DateTime lastUpdateDate = dates.Max();
                project.LatestUpdate = lastUpdateDate;
            }
            return projects;
        }

        /// <summary>
        /// Deprecated, referenced only by Unit Tests as of 5/24/16
        /// </summary>
        /// <param name="projectID">ProjectID Guid as string</param>
        /// <returns>List of StatusUpdate Objects</returns>
        public List<StatusUpdate> GetAllUpdatesForProject(string projectID)
        {
            Guid projectGuid = new Guid(projectID);
            string projectName = context.Projects.FirstOrDefault(p => p.ProjectID == projectGuid).ProjectName;
            if (string.IsNullOrEmpty(projectName)) return new List<StatusUpdate>();//__return empty list when project not found
            var updates = context.StatusUpdates.Where(s => s.ProjectID == projectGuid).ToList();
            foreach (var update in updates) update.ProjectName = projectName;

            return updates;
        }

        /// <summary>
        /// Gets all existing ProjectUpdates for the Project associated with this ID
        /// ProjectUpdate is a normalized update message (email)
        /// </summary>
        /// <param name="projectID">Guid ProjectID as string</param>
        /// <returns>Returns a list of ProjectUpdates for the specified Project</returns>
        public List<ProjectUpdate> GetProjectUpdates(string projectID)
        {
            Guid projectGuid = Guid.Parse(projectID);
            List<ProjectUpdate> projectUpdates = context.ProjectUpdates.Where(pu => pu.ProjectID == projectGuid).ToList();

            //_now need to populate required column headings for more efficient client-side processing
            foreach (ProjectUpdate projectUpdate in projectUpdates)
            {
                //__some Properties should be available on all StatusUpdates
                List<StatusUpdate> statusUpdates = projectUpdate.StatusUpdates.ToList();
                StatusUpdate referenceUpdate = statusUpdates.FirstOrDefault();
                if (referenceUpdate == null) continue;//__should not happen

                projectUpdate.Date = referenceUpdate.RecordDate.ToString();
                int? phaseIndex = referenceUpdate.PhaseID;
                  
                //__add some safety checks, default to -1, Not_Assigned
                if (phaseIndex == null) phaseIndex = -1;
                if (phaseIndex < -1 || phaseIndex > 7) phaseIndex = -1;

                projectUpdate.Phase = Enum.GetName(typeof(Phases),phaseIndex);
                projectUpdate.PhaseID = Convert.ToInt16( phaseIndex);

                //__will need to look for Environment and Description
                StatusUpdate environmentUpdate = statusUpdates.FirstOrDefault(su => su.UpdateKey == "Environment");
                projectUpdate.Environment = environmentUpdate == null ? "--" : environmentUpdate.UpdateValue;

                StatusUpdate descriptionUpdate = statusUpdates.FirstOrDefault(su => su.UpdateKey == "Description");
                projectUpdate.Description = descriptionUpdate == null ? "--" : descriptionUpdate.UpdateValue;

            }
            return projectUpdates;
        }

        /// <summary>
        /// Gets all UpdateValues for a particular UpdateKey, with options to filter by Project, Phase, and date
        /// </summary>
        /// <param name="updateKey">The UpdateKey to look up</param>
        /// <param name="projectID">Optional, if set only StatusUpdates associated with this project are reviewed</param>
        /// <param name="phaseID">Optional, if set will only look at StatusUpdates with this Phase</param>
        /// <param name="getOnlyLatest">Optional, if set to True will return only a single StatusUpdate, the most recently recorded</param>
        /// <returns>List of StatusUpdates which have the specified UpdateKey, potentially filtered by Project, Phase, and date</returns>
        public List<StatusUpdate> GetUpdatesForKey(string updateKey, Guid? projectID = null, int phaseID = -2,
            bool getOnlyLatest = false)
        {

            bool getUpdatesForSpecificProject = projectID != null;
            bool getUpdatesForSpecificPhase = phaseID >= -1;

            //__first get just the updates with the key of interest
            List<StatusUpdate> updates = context.StatusUpdates.Where(su => su.UpdateKey == updateKey).ToList();


            if (updates.Count == 0) return updates;//__nothing found, return empty list

            if (getUpdatesForSpecificProject)
                updates = updates.Where(su => su.ProjectID == projectID).ToList();
            if (updates.Count == 0) return updates; //__still nothing found

            if (getUpdatesForSpecificPhase) updates = updates.Where(su => su.PhaseID == phaseID).ToList();
            if (updates.Count == 0) return updates;

            if (getOnlyLatest)
            {
                updates.OrderBy(su => su.RecordDate);
                StatusUpdate lastUpdate = updates.Last();
                updates.Clear();
                updates.Add(lastUpdate);
            }

            //__now also write ProjectName to each update
            foreach (StatusUpdate update in updates)
            {
                update.ProjectName = context.Projects.FirstOrDefault(p => p.ProjectID == update.ProjectID).ProjectName;
            }
            return updates;
        }

        /// <summary>
        /// Used to retreive the names of all currently defined Verticals
        /// </summary>
        /// <returns>List of Key-Value pairs which map directly to the Verticals enum</returns>
        public List<KeyValuePair<int, string>> GetAllVerticals()
        {
            string[] names = Enum.GetNames(typeof(Verticals));
            int[] values = (int[])Enum.GetValues(typeof(Verticals));
            List<KeyValuePair<int, string>> verticals = new List<KeyValuePair<int, string>>();
            for (int i = 0; i < names.Length; i++)
            {
                verticals.Add(new KeyValuePair<int, string>(values[i], names[i]));
            }
            return verticals;
        }

        /// <summary>
        /// Used to query for all currently tracked Projects associated with a particular VerticalID (int)
        /// </summary>
        /// <param name="verticalID">VerticalID is the (int)index defined in the Verticals enum</param>
        /// <returns>List of Project objects associated with the Vertical</returns>
        public List<Project> GetAllProjectsForVertical(int verticalID)
        {
            //return context.Projects.Where(p => p.VerticalID == verticalID).Select(p => p.ProjectID).ToList();
            var projects = context.Projects.Where(p => p.VerticalID == verticalID).ToList();
            foreach (Project project in projects)
            {
                Guid projectID = project.ProjectID;
                var lastUpdateDate = context.ProjectPhases.Where(p => p.ProjectID == projectID && p.LatestUpdate != null).Max(p => p.LatestUpdate);
                if(lastUpdateDate != null) project.LatestUpdate = (DateTime)lastUpdateDate;
            }
            return projects;
        }

        /// <summary>
        /// Gets all StatusUpdate objects which came from a particular ProjectUpdate(email)
        /// </summary>
        /// <param name="projectUpdateID">Guid which is the unique identifer for the ProjectUpdate being queried</param>
        /// <returns>List of StatusUpdate objects</returns>
        public List<StatusUpdate> GetAllUpdatesFromEmail(Guid projectUpdateID)
        {
            var updates = new List<StatusUpdate>();

            updates = context.StatusUpdates.Where(su => su.ProjectUpdateID == projectUpdateID).ToList();

            if (updates.Count > 0)
            {
                Guid projectID = updates.First().ProjectID;
                string projectName = context.Projects.FirstOrDefault(p => p.ProjectID == projectID).ProjectName;
                foreach (StatusUpdate update in updates)
                {
                    update.ProjectName = projectName;//__now also write ProjectName to each update
                }
            }
            return updates;
        }

        /// <summary>
        /// This overload was implimented to match UI input
        /// </summary>
        /// <param name="projectUpdate">The ProjectUpdate to modify, it already has the new PhaseID set
        /// all StatusUpdates which originated with this ProjectUpdate will get the new PhaseID</param>
        /// <returns>true for success, false for failure</returns>
        public bool ChangeProjectUpdatePhase(ProjectUpdate projectUpdate)
        {
            List<StatusUpdate> oldStatusUpdates = GetAllUpdatesFromEmail(projectUpdate.ProjectUpdateID);

            return ChangeProjectUpdatePhase(oldStatusUpdates.First(), projectUpdate.PhaseID);
        }

        /// <summary>
        /// Originally this would update each related StatusUpdate, and the entries in ProjectPhase, but this was commented
        /// out since the ProjectPhase table is not currently in use. Instead, each StatusUpdate which came from the associated 
        /// ProjectUpdate is modified with the new PhaseID
        /// </summary>
        /// <param name="update">An representative StatusUpdate, it is only used to get ProjectUpdateID</param>
        /// <param name="newPhase">An integer representing the new PhaseID</param>
        /// <returns>True or False to indicate success or failure</returns>
        public bool ChangeProjectUpdatePhase(StatusUpdate update, int newPhase)
        {
            if (update == null || newPhase < 0 || newPhase > Enum.GetNames(typeof(Phases)).Length) return false;

            //int oldPhase = update.PhaseID.Value;
            //Guid projectID = update.ProjectID;
            //string updateKey = update.UpdateKey;
            Guid projectUpdateID = update.ProjectUpdateID;
            var updates = context.StatusUpdates.Where(u => u.ProjectUpdateID == projectUpdateID);
            foreach (StatusUpdate statusUpdate in updates)
            {
                statusUpdate.PhaseID = newPhase;

                ////__also update any related entries in ProjectPhase table
                //ProjectPhase pp = context.ProjectPhases.FirstOrDefault(p => p.ProjectID == projectID
                //                                                            && p.UpdateKey == updateKey);
                //if (pp != null) pp.PhaseID = newPhase;
            }
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Used to modify the Vertical a given Project is associated with.
        /// Either projectID or projectName must be provided.
        /// </summary>
        /// <param name="newVerticalID">(int) Index to Verticals enum</param>
        /// <param name="projectID">Optional. Guid which identifies the Project being modified</param>
        /// <param name="projectName">Optional. The name of the Project to be modified</param>
        /// <returns>True upon successful modification, else false</returns>
        public bool UpdateProjectVertical(int newVerticalID, Guid? projectID = null, string projectName = "")
        {
            //__validate arguments
            if (newVerticalID < -1 || newVerticalID > 7) return false;
            bool haveNoId = projectID == null || projectID == Guid.Empty;
            bool haveNoName = string.IsNullOrEmpty(projectName);
            if (haveNoId && haveNoName) return false;

            projectName = projectName.Trim();

            Project recordedProject;
            if (!haveNoId) recordedProject = context.Projects.FirstOrDefault(p => p.ProjectID == projectID);
            else recordedProject = context.Projects.FirstOrDefault(p => p.ProjectName == projectName);

            //__exit if no project found
            if (recordedProject == null) return false;

            recordedProject.VerticalID = newVerticalID;
            return true;
        }

        /// <summary>
        /// Deprecated method to retrieve Projects from a particular vertical
        /// </summary>
        /// <param name="projectName">Deprecated</param>
        /// <param name="verticalID">Deprecated</param>
        /// <returns></returns>
        public List<Project> GetProjectIDs(string projectName = "", int verticalID = -1)
        {
            List<Project> projects = new List<Project>();
            bool bHaveName = !string.IsNullOrEmpty(projectName);
            bool bHaveVertical = verticalID >= 0;
            if (bHaveName && bHaveVertical)
            {
                projects = context.Projects.Where(p =>
                p.ProjectName == projectName &&
                p.VerticalID == verticalID).ToList();
            }
            else if (bHaveName && !bHaveVertical)
            {
                projects = context.Projects.Where(p => p.ProjectName == projectName).ToList();
            }
            else if (!bHaveName && bHaveVertical)
            {
                projects = context.Projects.Where(p => p.VerticalID == verticalID).ToList();
            }
            return projects;
        }

        /// <summary>
        /// Gets the ProjectID for a given ProjectName
        /// </summary>
        /// <param name="projectName">String, name of Project being queried</param>
        /// <returns>Project ID as Guid</returns>
        public Guid GetProjectIDbyName(string projectName)
        {
            Project project = context.Projects.FirstOrDefault(p => p.ProjectName == projectName);
            Guid? projectID = project?.ProjectID;
            if (projectID == null) projectID = Guid.Empty;
            return (Guid)projectID;
        }

        /// <summary>
        /// Gets the Project Name associated with a particular ID
        /// </summary>
        /// <param name="projectID">The Guid ProjectID associated with the Project being queried</param>
        /// <returns>Project Name as string if Project is found, else returns empty string</returns>
        public string GetProjectNameForID(Guid projectID)
        {
            return context.Projects.FirstOrDefault(p => p.ProjectID == projectID).ProjectName;
        }

        /// <summary>
        /// Takes a Proejct ID and removes all records associated with that Project
        /// </summary>
        /// <param name="projectID">Guid which identifies the Project to be deleted</param>
        /// <returns>True if the project was found and deleted, false if no project or error encountered</returns>
        public bool DeleteProject(Guid projectID)//__this must be done in a specific order so as to not violate SQL integrity
        {
            try
            {
                //__first remove all related StatusUpdates
                var statusUpdatesToRemove = context.StatusUpdates.Where(su => su.ProjectID == projectID);
                context.StatusUpdates.RemoveRange(statusUpdatesToRemove);
                context.SaveChanges();

                //__Remove projectupdates
                var projectUpdatesToRemove = context.ProjectUpdates.Where(pu => pu.ProjectID == projectID);
                context.ProjectUpdates.RemoveRange(projectUpdatesToRemove);
                context.SaveChanges();

                //__Remove all projectPhase entries
                var projectPhaseToRemove = context.ProjectPhases.Where(pp => pp.ProjectID == projectID);
                context.ProjectPhases.RemoveRange(projectPhaseToRemove);
                context.SaveChanges();

                //__Finally remove the Project entry itself
                Project projectToRemove = context.Projects.FirstOrDefault(p => p.ProjectID == projectID);
                context.Projects.Remove(projectToRemove);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
