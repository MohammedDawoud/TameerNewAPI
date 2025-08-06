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
    public class OutInBoxSerialRepository : IOutInBoxSerialRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public OutInBoxSerialRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<OutInBoxSerialVM>> GetAllOutInBoxSerial(int Type,int BranchId)
        {
            var OutInBoxSerial = _TaamerProContext.OutInBoxSerial.Where(s => s.IsDeleted == false && s.Type == Type && s.BranchId == BranchId).Select(x => new OutInBoxSerialVM
            {
                OutInSerialId = x.OutInSerialId,
                Name = x.Name,
                Code = x.Code,
                LastNumber = x.LastNumber,
                Type = x.Type,
            }).ToList();
            return OutInBoxSerial;
        }
    }
}
