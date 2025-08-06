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
    public class FilesAuthRepository: IFilesAuthRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public FilesAuthRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<FilesAuthVM>> GetAllFilesAuth()
        {
            try
            {
                var FileAuthObj = _TaamerProContext.FilesAuth.Where(s => s.IsDeleted == false).Select(x => new FilesAuthVM
                {
                    FilesAuthId = x.FilesAuthId,
                    AppKey = x.AppKey ?? "",
                    AppSecret = x.AppSecret ?? "",
                    RedirectUri = x.RedirectUri ?? "",
                    Code = x.Code ?? "",
                    AccessToken = x.AccessToken ?? "",
                    RefreshToken = x.RefreshToken ?? "",
                    FolderName = x.FolderName ?? "",
                    ExpiresIn = x.ExpiresIn ?? 0,
                    CreationDate = x.CreationDate,
                    TypeId = x.TypeId,
                    BranchId = x.BranchId,
                    Sendactive = x.Sendactive??false,

                }).ToList();
                return FileAuthObj;
            }
            catch (Exception ex)
            {
                IEnumerable<FilesAuthVM> FAuth = new List<FilesAuthVM>();
                return FAuth;

            }
        }
        public async Task<FilesAuthVM> GetFilesAuthByTypeId(int TypeId)
        {
            try
            {
                var FileAuthObj = _TaamerProContext.FilesAuth.Where(s => s.IsDeleted == false && s.TypeId== TypeId).Select(x => new FilesAuthVM
                {
                    FilesAuthId = x.FilesAuthId,
                    AppKey = x.AppKey ?? "",
                    AppSecret = x.AppSecret ?? "",
                    RedirectUri = x.RedirectUri ?? "",
                    Code = x.Code ?? "",
                    AccessToken = x.AccessToken ?? "",
                    RefreshToken = x.RefreshToken ?? "",
                    FolderName = x.FolderName ?? "",
                    ExpiresIn = x.ExpiresIn ?? 0,
                    CreationDate = x.CreationDate,
                    TypeId = x.TypeId,
                    BranchId = x.BranchId,
                    Sendactive = x.Sendactive ?? false,
                }).FirstOrDefault();
                return FileAuthObj;
            }
            catch (Exception ex)
            {
                FilesAuthVM FAuth = new FilesAuthVM();
                return FAuth;

            }
        }

    }
}
