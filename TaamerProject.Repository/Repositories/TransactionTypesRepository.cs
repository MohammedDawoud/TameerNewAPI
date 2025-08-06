using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class TransactionTypesRepository : ITransactionTypesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public TransactionTypesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<TransactionTypesVM>> GetAllTransactionTypes(string SearchText)
        {
            if (SearchText == "")
            {
                var transactionTypes = _TaamerProContext.TransactionTypes.Where(s => s.IsDeleted == false).Select(x => new TransactionTypesVM
                {
                    TransactionTypeId = x.TransactionTypeId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return transactionTypes;
            }
          else 
        { 
          var transactionTypes = _TaamerProContext.TransactionTypes.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText) )).Select(x => new TransactionTypesVM
          {
              TransactionTypeId = x.TransactionTypeId,
              NameAr = x.NameAr,
              NameEn = x.NameEn,
          }).ToList();
                return transactionTypes;
            }
          }
        }
    }
