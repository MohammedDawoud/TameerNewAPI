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
    public class VersionRepository :  IVersionRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public VersionRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<VersionVM> GetVersion()
        {
            var Version = _TaamerProContext.Versions.Where(s => s.IsDeleted == false).Select(x => new VersionVM
            {
                VersionId = x.VersionId,
                VersionCode = x.VersionCode,
                Date = x.Date,
                HijriDate = x.HijriDate,
            }).FirstOrDefault();
            return Version;
        }
    }
}


