using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IContact_BranchesService
    {
        Task<IEnumerable<Contact_BranchesVM>> GetAllContactBranches();
        GeneralMessage SaveContactBranches(Contact_Branches COntactbranch, int UserId, int BranchId);
        GeneralMessage DeleteContactBranches(int ContactId, int UserId, int BranchId);

        GeneralMessage SendMail_SysContact(int branchcontactid, string Name, string textBody, string MobileNumber);
    }
}
