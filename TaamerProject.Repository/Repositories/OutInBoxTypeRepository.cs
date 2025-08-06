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
    public class OutInBoxTypeRepository : IOutInBoxTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public OutInBoxTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task< IEnumerable<OutInBoxTypeVM>> GetAllOutInBoxTypes(string SearchText, int BranchId)
        {
            var OutInBoxTypes = _TaamerProContext.OutInBoxType.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new OutInBoxTypeVM
            {
                TypeId = x.TypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            }).ToList();
            if (SearchText != "")
            {
                OutInBoxTypes = OutInBoxTypes.Where(s => s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim())).ToList();
            }
            return OutInBoxTypes;
        }
    }
}
