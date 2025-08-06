using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IGroupsService  
    {
        Task<IEnumerable<GroupsVM>> GetAllGroups();
        Task<IEnumerable<GroupsVM>> GetAllGroups_S(string SearchText);
        GeneralMessage SaveGroups(Groups groups,int UserId, int BranchId);
        GeneralMessage DeleteGroups(int GroupId,int UserId,int BranchId);
        GroupsVM GetGroupById(int GroupId);
    }
}
