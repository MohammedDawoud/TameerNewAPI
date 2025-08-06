using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class Acc_TotalSpacesRangeRepository : IAcc_TotalSpacesRangeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Acc_TotalSpacesRangeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<Acc_TotalSpacesRangeVM>> GetAllTotalSpacesRange(string SearchText)
        {
            if (SearchText == "")
            {
                var TotalSpacesRange = _TaamerProContext.Acc_TotalSpacesRange.Where(s => s.IsDeleted == false).Select(x => new Acc_TotalSpacesRangeVM
                {
                    TotalSpacesRangeId = x.TotalSpacesRangeId,
                    TotalSpacesRengeName = x.TotalSpacesRengeName ?? "",
                    RangeValue = x.RangeValue,
                }).ToList();
                return TotalSpacesRange;
            }
            else

            {
                var TotalSpacesRange = _TaamerProContext.Acc_TotalSpacesRange.Where(s => s.IsDeleted == false && (s.TotalSpacesRengeName.Contains(SearchText))).Select(x => new Acc_TotalSpacesRangeVM
                {
                    TotalSpacesRangeId = x.TotalSpacesRangeId,
                    TotalSpacesRengeName = x.TotalSpacesRengeName ?? "",
                    RangeValue = x.RangeValue,
                }).ToList();
                return TotalSpacesRange;
            }
        }
    }
}
