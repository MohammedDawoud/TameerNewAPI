using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProUserPrivilegesService  
    {
        GeneralMessage SavePrivProList(List<ProUserPrivileges> Privs, int UserId, int BranchId);
        //GeneralMessage SavePriv2(ProUserPrivileges Privs, int UserId, int BranchId);
        GeneralMessage SavePriv2(ProUserPrivileges Privs, int UserId, int BranchId, string Url, string ImgUrl);
        Task<IEnumerable<ProUserPrivilegesVM>> GetAllPriv(string SearchText,string Projectno, int BranchId);
        Task<IEnumerable<ProUserPrivilegesVM>> GetAllPrivUser(int UserId, int projectId);
        GeneralMessage DeletePriv(int PrivID, int UserId, int BranchId);


    }
}
