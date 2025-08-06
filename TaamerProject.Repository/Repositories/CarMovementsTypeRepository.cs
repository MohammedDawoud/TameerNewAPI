using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class CarMovementsTypeRepository : ICarMovementsTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CarMovementsTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<CarMovementsTypeVM>> GetAllCarMovmentsTypes(string SearchText)
        {
            if (SearchText == "" ||SearchText==null)
            {
                var ItemTypes = _TaamerProContext.CarMovementsType.Where(s => s.IsDeleted == false).Select(x => new CarMovementsTypeVM
                {
                    TypeId = x.TypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                });
                return ItemTypes;
            }
            else
            {
                var ItemTypes = _TaamerProContext.CarMovementsType.Where(s => s.IsDeleted == false  && (s.NameAr.Contains(SearchText) || s.NameEn.ToLower().Contains(SearchText.ToLower()))).Select(x => new CarMovementsTypeVM
                {
                    TypeId = x.TypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                });
                return ItemTypes;
            }
        }
    }
}
