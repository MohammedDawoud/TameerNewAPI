using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IGroupPrivilegeService  
    {
        GeneralMessage SaveUserPrivilegesGroups(int GroupId, List<int> Privs, int UserId, int BranchId);
        GeneralMessage SaveUserPrivilegesGroups2(int GroupId, List<int> Privs, int UserId, int BranchId,string Con);

        List<int> GetPrivilegesIdsByGroupId(int GroupId);
    }
}
