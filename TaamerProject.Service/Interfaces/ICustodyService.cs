using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICustodyService
    {
        Task<IEnumerable<CustodyVM>> GetAllCustody(int BranchId);

        Task<IEnumerable<CustodyVM>> GetSomeCustody(int BranchId, bool Status);
        Task<IEnumerable<CustodyVM>> GetSomeCustodyVoucher(int BranchId, bool Status);

        //  GeneralMessage SaveCustody(Custody custody,int UserId, int BranchId, string Lang);
        GeneralMessage SaveCustody(Custody custody, int UserId, int BranchId, string Lang, string Url, string ImgUrl);
        GeneralMessage SaveCustodyVoucher(Custody custody, int UserId, int BranchId, string Lang);

        GeneralMessage DeleteCustody(int CustodyId, int UserId, int BranchId);
        Task<EmployeesVM> GetEmployeeByItemId(int Item, int BranchId);
        Task<IEnumerable<CustodyVM>> SearchCustody(CustodyVM CustodySearch, string lang, int BranchId);
        Task<IEnumerable<CustodyVM>> SearchCustodyVoucher(CustodyVM CustodySearch, string lang, int BranchId);

        Task<IEnumerable<object>> FillCustodySelect(string lang, int BranchId);

        //GeneralMessage FreeCustody(int CustodyId, int UserId, int BranchId, string Lang);
        GeneralMessage FreeCustody(int CustodyId, int UserId, int BranchId, string Lang, string Url, string ImgUrl);
        GeneralMessage ConvertStatusCustody(int CustodyId, int UserId, int BranchId, string Lang);
        GeneralMessage ReturnConvetCustody(int CustodyId, int UserId, int BranchId, string Lang);
        Task<string> GetEmployeeCustodies(int EmployeeId);
    }
}
