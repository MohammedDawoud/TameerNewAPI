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
    public class EmpStructureRepository :  IEmpStructureRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public EmpStructureRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<EmpStructureVM>> GetAllEmpStructure(int BranchId)
        {
            var structures = _TaamerProContext.EmpStructure.Where(s => s.IsDeleted == false).Select(x => new EmpStructureVM
            {
                StructureId = x.StructureId,
                EmpId = x.EmpId,
                ManagerId = x.ManagerId,
                BranchId = x.BranchId,
            }).ToList();
            return structures;
        }
    }
}


