using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class NodeLocationsRepository :INodeLocationsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public NodeLocationsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        
        public async Task< IEnumerable<NodeLocationsVM>> GetAllNodeLocations()
        {
            var nodeLocations = _TaamerProContext.NodeLocations.Where(s => s.IsDeleted == false).Select(x => new NodeLocationsVM
            {
               // LocationId = x.LocationId,
                SettingId = x.SettingId,
                TaskId = x.TaskId,
                Location = x.Location,
             
            }).ToList();
            return nodeLocations;
        }
    }
}
