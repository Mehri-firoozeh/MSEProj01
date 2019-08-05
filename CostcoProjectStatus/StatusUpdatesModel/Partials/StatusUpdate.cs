namespace StatusUpdatesModel
{
    public partial class StatusUpdate
    {
        public string ProjectName { get; set; }

        public StatusUpdate Clone()
        {
            StatusUpdate newStatusUpdate = new StatusUpdate();
            newStatusUpdate.ProjectID = ProjectID;
            newStatusUpdate.PhaseID = PhaseID;
            newStatusUpdate.VerticalID = VerticalID;
            newStatusUpdate.ProjectUpdateID = ProjectUpdateID;
            return newStatusUpdate;
        }
    }

}
