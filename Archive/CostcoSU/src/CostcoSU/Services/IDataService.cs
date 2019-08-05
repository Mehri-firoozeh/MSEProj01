using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostcoSU.Models;

namespace CostcoSU.Services
{
    interface IDataService
    {
        Task<List<StatusUpdate>> GetAllUpdatesForProjectAsync(string ProjectID);

        Task<List<Project>> GetAllProjectsAsync();

        Task<List<Project>> GetAllProjectsForVerticalAsync(Vertical vertical);

        Task<List<StatusUpdate>> GetAllUpdatesForProjectPhaseAsynch(string ProjectID, Phase phase);

        Task RecordStatusUpdate(StatusUpdate newUpdate);
    }
}
