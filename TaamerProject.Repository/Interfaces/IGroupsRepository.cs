using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IGroupsRepository
    {
        Task<IEnumerable<GroupsVM>> GetAllGroups();
        Task<IEnumerable<GroupsVM>> GetAllGroups_S(string SearchText);
    }
}
