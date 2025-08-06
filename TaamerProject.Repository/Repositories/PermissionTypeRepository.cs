using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class PermissionTypeRepository : IPermissionTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public PermissionTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<PermissionTypeVM>> GetAllPermissionTypes(string SearchText)
        {
            if (SearchText == "")
            {
                var PermissionTypes = _TaamerProContext.PermissionTypes.Where(s => s.IsDeleted == false).Select(x => new PermissionTypeVM
                {
                    TypeId = x.TypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes
                }).ToList();
                return PermissionTypes;
            }
            else
            {
                var PermissionTypes = _TaamerProContext.PermissionTypes.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new PermissionTypeVM
                {
                    TypeId = x.TypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes
                }).ToList();
                return PermissionTypes;
            }
        }


    }
}
