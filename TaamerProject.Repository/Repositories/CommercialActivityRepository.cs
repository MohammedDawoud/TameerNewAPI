using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class CommercialActivityRepository : ICommercialActivityRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;
        public CommercialActivityRepository(TaamerProjectContext taamerProContext)
        {
            _TaamerProContext = taamerProContext;
        }

        public async Task<IEnumerable<CommercialActivityVM>> GetCommercialActivities(string SearchText, int Type)
        {
            if (SearchText == "")
            {
                var transactionTypes = _TaamerProContext.CommercialActivities.Where(s => s.IsDeleted == false && s.Type==Type).Select(x => new CommercialActivityVM
                {
                    CommercialActivityId = x.CommercialActivityId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return transactionTypes;
            }
            else
            {
                var transactionTypes = _TaamerProContext.CommercialActivities.Where(s => s.IsDeleted == false && s.Type==Type && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new CommercialActivityVM
                {
                    CommercialActivityId = x.CommercialActivityId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return transactionTypes;
            }
        }
    }
    }

