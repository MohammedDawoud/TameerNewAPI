using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAcc_SuppliersService
    {

        Task<IEnumerable<Acc_SuppliersVM>> GetAllSuppliers(string SearchText, int BranchId, int? yearid);
        IEnumerable<Acc_SuppliersVM> GetAllSuppliersAllNoti(string SearchText, string lang, int BranchId, int? yearid);

        Task<Acc_SuppliersVM> GetSupplierByID(int SupplierId);

        GeneralMessage SaveSupplier(Acc_Suppliers Supplier, int UserId, int BranchId);
        GeneralMessage DeleteSupplier(int SupplierId, int UserId, int BranchId);
        string GetTaxNoBySuppId(int SupplierId, string lang, int BranchId);
        int GetAccIdBySuppId(int SupplierId, string lang, int BranchId);
        int GetSuppIdByAccId(int AccountId, string lang, int BranchId);

        string GetSuppNameBySuppId(int SupplierId, string lang, int BranchId);
    }
}
