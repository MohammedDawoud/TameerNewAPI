using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IBanksService
    {
       Task< IEnumerable<BanksVM>> GetAllBanks(string SearchText);
        GeneralMessage SaveBanks(Banks banks, int UserId, int BranchId);
        GeneralMessage DeleteBanks(int BankId, int UserId, int BranchId);
        IEnumerable<object> FillBankSelect(string SearchText = "");
        Task<BanksVM> GetBankByID(int BankId);
    }
}
