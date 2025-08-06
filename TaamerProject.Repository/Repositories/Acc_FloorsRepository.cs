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
    public class Acc_FloorsRepository : IAcc_FloorsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Acc_FloorsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Acc_FloorsVM>> GetAllFloors(string SearchText)
        {
            if (SearchText == "")
            {
                var Floors = _TaamerProContext.Acc_Floors.Where(s => s.IsDeleted == false).Select(x => new Acc_FloorsVM
                {
                    FloorId = x.FloorId,
                    FloorName = x.FloorName??"",
                    FloorRatio = x.FloorRatio??0,
                }).ToList();
                return Floors;
            }
            else

            {
                var Floors = _TaamerProContext.Acc_Floors.Where(s => s.IsDeleted == false && (s.FloorName.Contains(SearchText))).Select(x => new Acc_FloorsVM
                {
                    FloorId = x.FloorId,
                    FloorName = x.FloorName??"",
                    FloorRatio = x.FloorRatio??0,
                }).ToList();
                return Floors;
            }
        }
    }
}
