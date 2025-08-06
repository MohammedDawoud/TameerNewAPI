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
    public class Acc_PackagesRepository : IAcc_PackagesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Acc_PackagesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Acc_PackagesVM>> GetAllPackages(string SearchText)
        {
            if (SearchText == "")
            {
                var Packages = _TaamerProContext.Acc_Packages.Where(s => s.IsDeleted == false).Select(x => new Acc_PackagesVM
                {
                    PackageId = x.PackageId,
                    PackageName = x.PackageName??"",
                    MeterPrice1 = x.MeterPrice1,
                    MeterPrice2 = x.MeterPrice2,
                    MeterPrice3 = x.MeterPrice3,
                    PackageRatio1 = x.PackageRatio1,
                    PackageRatio2 = x.PackageRatio2,
                    PackageRatio3 = x.PackageRatio3,
                }).ToList();
                return Packages;
            }
            else

            {
                var Packages = _TaamerProContext.Acc_Packages.Where(s => s.IsDeleted == false && (s.PackageName.Contains(SearchText))).Select(x => new Acc_PackagesVM
                {
                    PackageId = x.PackageId,
                    PackageName = x.PackageName??"",
                    MeterPrice1 = x.MeterPrice1,
                    MeterPrice2 = x.MeterPrice2,
                    MeterPrice3 = x.MeterPrice3,
                    PackageRatio1 = x.PackageRatio1,
                    PackageRatio2 = x.PackageRatio2,
                    PackageRatio3 = x.PackageRatio3,
                }).ToList();
                return Packages;
            }
        }
    }
}
