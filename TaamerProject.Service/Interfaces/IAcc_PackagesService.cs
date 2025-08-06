using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IAcc_PackagesService
    {
       
        Task<IEnumerable<Acc_PackagesVM>> GetAllPackages(string SearchText);
        GeneralMessage SavePackage(Acc_Packages Package, int UserId, int BranchId);
        GeneralMessage DeletePackage(int PackageId, int UserId, int BranchId);
    }
}
