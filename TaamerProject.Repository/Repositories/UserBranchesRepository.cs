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
    public class UserBranchesRepository : IUserBranchesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public UserBranchesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<BranchesVM>> GetAllBranchesByUserId(string Lang, int UserId)
        {
            if(UserId==1)
            {
                var branches = _TaamerProContext.UserBranches.Where(s => s.UserId == 1).Select(s => new BranchesVM
                {
                    BranchId = s.BranchId,
                    BranchName = Lang == "ltr" ? s.Branches.NameEn : s.Branches.NameAr
                }).ToList();
                return branches;
            }
            else
            {
                var branches = _TaamerProContext.UserBranches.Where(s => (s.UserId == UserId && s.IsDeleted == false)).Select(s => new BranchesVM
                {
                    BranchId = s.BranchId,
                    BranchName = Lang == "ltr" ? s.Branches.NameEn : s.Branches.NameAr
                }).ToList();
                return branches;
            }

        }
        public async Task<IEnumerable<BranchesVM>> GetAllBranchesAndMainByUserId(string Lang, int UserId)
        {
            if(UserId==1)
            {
                var branches = _TaamerProContext.UserBranches.Where(s => s.UserId == 1).Select(s => new BranchesVM
                {
                    BranchId = s.BranchId,
                    BranchName = Lang == "ltr" ? s.Branches.NameEn : s.Branches.NameAr
                }).ToList();



                var user = _TaamerProContext.UserBranches.Where(s => s.UserId == 1).Select(x => new UsersVM
                {
                    BranchId = x.BranchId,
                    BranchName = x.Branches.NameAr,
                }).FirstOrDefault();

                var BranchIDMain = user.BranchId;

                var BranchNameMain = user.BranchName;

                var mainbranch = new BranchesVM();

                mainbranch.BranchId = BranchIDMain ?? 0;
                mainbranch.BranchName = BranchNameMain;

                for (int i = 0; i < branches.Count(); i++)
                {
                    if (branches.Contains(mainbranch))
                    {
                        branches.Add(mainbranch);

                    }
                }
                return branches;
            }
            else
            {
                var branches = _TaamerProContext.UserBranches.Where(s => (s.UserId == UserId && s.IsDeleted == false)).Select(s => new BranchesVM
                {
                    BranchId = s.BranchId,
                    BranchName = Lang == "ltr" ? s.Branches.NameEn : s.Branches.NameAr
                }).ToList();



                var user = _TaamerProContext.UserBranches.Where(s => (s.UserId == UserId && s.IsDeleted == false)).Select(x => new UsersVM
                {
                    BranchId = x.BranchId,
                    BranchName = x.Branches.NameAr,
                }).FirstOrDefault();

                var BranchIDMain = user.BranchId;

                var BranchNameMain = user.BranchName;

                var mainbranch = new BranchesVM();

                mainbranch.BranchId = BranchIDMain ?? 0;
                mainbranch.BranchName = BranchNameMain;

                for (int i = 0; i < branches.Count(); i++)
                {
                    if (branches.Contains(mainbranch))
                    {
                        branches.Add(mainbranch);

                    }
                }
                return branches;
            }


        }
        
        public async Task<IEnumerable<BranchesVM>> GetBranchByBranchId(string Lang, int BranchId)
        {
            var branches = _TaamerProContext.UserBranches.Where(s => s.BranchId == BranchId && s.IsDeleted == false).Select(s => new BranchesVM
            {
                BranchId = s.BranchId,
                BranchName = Lang == "ltr" ? s.Branches.NameEn : s.Branches.NameAr
            });
            return branches;
        }
    }
}
