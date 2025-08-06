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
    public class GroupsRepository : IGroupsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public GroupsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        
        public async Task<IEnumerable<GroupsVM>> GetAllGroups()
        {
            var Groups = _TaamerProContext.Groups.Where(s => s.IsDeleted == false).Select(x => new GroupsVM
            {
                GroupId = x.GroupId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Notes = x.Notes,
                BranchId = x.BranchId,
                
            }).ToList();
            return Groups;
        }
        public async Task<IEnumerable<GroupsVM>> GetAllGroups_S(string SearchText)
        {
            var Groups = _TaamerProContext.Groups.Where(s => s.IsDeleted == false).Select(x => new GroupsVM
            {
                GroupId = x.GroupId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Notes = x.Notes,
                BranchId = x.BranchId,

            }).Select(z=>new GroupsVM()
            {
                GroupId = z.GroupId,
                NameAr = z.NameAr,
                NameEn = z.NameEn,
                Notes = z.Notes,
                BranchId = z.BranchId,
            }              
                );
            if (SearchText != "")
            {
                Groups = Groups.Where(s => s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim()));
            }
            return Groups;
        }
    }
}
