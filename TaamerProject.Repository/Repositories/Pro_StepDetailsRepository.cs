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
    public class Pro_StepDetailsRepository: IPro_StepDetailsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Pro_StepDetailsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<Pro_StepDetailsVM>> GetAllStepDetails()
        {
            try
            {
                var RetList = _TaamerProContext.Pro_StepDetails.Where(s => s.IsDeleted == false).Select(x => new Pro_StepDetailsVM
                {
                    StepDetailId = x.StepDetailId,
                    NameAr = x.NameAr??"",
                    NameEn = x.NameEn??"",
                    StepId = x.StepId,
                    StepName = x.StepName ?? "",
                }).ToList();
                return RetList;
            }
            catch (Exception ex)
            {
                IEnumerable<Pro_StepDetailsVM> ListR = new List<Pro_StepDetailsVM>();
                return ListR;
            }
        }
    }
}
