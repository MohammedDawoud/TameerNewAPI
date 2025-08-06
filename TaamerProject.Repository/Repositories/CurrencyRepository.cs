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
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CurrencyRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<CurrencyVM>> GetAllCurrency(int BranchId)
        {
            var currencies = _TaamerProContext.Currency.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new CurrencyVM
            {
                CurrencyId = x.CurrencyId,
                CurrencyCode=x.CurrencyCode,
                CurrencyNameAr=x.CurrencyNameAr,
                CurrencyNameEn=x.CurrencyNameEn,
                PartCount = x.PartCount,
                PartNameAr = x.PartNameAr,
                PartNameEn = x.PartNameEn,
                ExchangeRate = x.ExchangeRate,
            });
            return currencies;
        }

        public async Task<IEnumerable<CurrencyVM>> GetAllCurrency2(string SearchText)
        {
            if (SearchText == "")
            {
                var Currency = _TaamerProContext.Currency.Where(s => s.IsDeleted == false).Select(x => new CurrencyVM
                {
                    CurrencyId = x.CurrencyId,
                    CurrencyCode = x.CurrencyCode,
                    CurrencyNameAr = x.CurrencyNameAr,
                    CurrencyNameEn = x.CurrencyNameEn,
                    PartCount = x.PartCount,
                    PartNameAr = x.PartNameAr,
                    PartNameEn = x.PartNameEn,
                    ExchangeRate = x.ExchangeRate,
                }).ToList();
                return Currency;
            }
            else
            {
                var Currency = _TaamerProContext.Currency.Where(s => s.IsDeleted == false && (s.CurrencyNameAr.Contains(SearchText) || s.CurrencyNameEn.Contains(SearchText) || s.CurrencyCode.Contains(SearchText))).Select(x => new CurrencyVM
                {
                    CurrencyId = x.CurrencyId,
                    CurrencyCode = x.CurrencyCode,
                    CurrencyNameAr = x.CurrencyNameAr,
                    CurrencyNameEn = x.CurrencyNameEn,
                    PartCount = x.PartCount,
                    PartNameAr = x.PartNameAr,
                    PartNameEn = x.PartNameEn,
                    ExchangeRate = x.ExchangeRate,
                }).ToList();
                return Currency;

            }
        }


        public async Task<int> GenerateCurrencyNumber(int BranchId)
        {
            if (_TaamerProContext.Currency != null)
            {
                var lastRow = _TaamerProContext.Currency.Where(s => s.IsDeleted == false && s.BranchId==BranchId).OrderByDescending(u => u.CurrencyId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    return int.Parse(lastRow.CurrencyCode) + 1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
    }
}
