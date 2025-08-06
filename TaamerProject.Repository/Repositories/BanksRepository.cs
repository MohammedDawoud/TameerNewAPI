using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class BanksRepository : IBanksRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public BanksRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<BanksVM>> GetAllBanks(string SearchText)
        {
            var bank = _TaamerProContext.Banks.Where(s => s.IsDeleted == false).Select(x => new BanksVM
            {
                BankId = x.BankId,
                Code = x.Code,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Notes = x.Notes,
                BanckLogo=x.BanckLogo,
            });
            if (SearchText != "")
            {
                bank = bank.Where(s => s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim()));
            }
            return bank;
        }

        public async Task<BanksVM> GetBankByID(int BankId)
        {
            var bank = _TaamerProContext.Banks.Where(s => s.IsDeleted == false && s.BankId== BankId).Select(x => new BanksVM
            {
                BankId = x.BankId,
                Code = x.Code,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Notes = x.Notes,
                BanckLogo = x.BanckLogo,
            }).FirstOrDefault();
         
            return bank;
        }
    }
}
