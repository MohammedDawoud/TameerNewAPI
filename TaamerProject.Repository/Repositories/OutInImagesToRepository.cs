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
    public class OutInImagesToRepository : IOutInImagesToRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public OutInImagesToRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        
        public async Task< IEnumerable<OutInImagesToVM>> GetAllOutInImagesTo()
        {
            var outInImagesTo = _TaamerProContext.OutInImagesTo.Where(s => s.IsDeleted == false).Select(x => new OutInImagesToVM
            {
                ImageToId = x.ImageToId,
                OutInboxId = x.OutInboxId,
                DepartmentId = x.DepartmentId,
                BranchId = x.BranchId,
              
            }).ToList();
            return outInImagesTo;
        }
    }
}
