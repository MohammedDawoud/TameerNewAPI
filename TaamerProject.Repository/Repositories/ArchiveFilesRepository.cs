using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class ArchiveFilesRepository :  IArchiveFilesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ArchiveFilesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<ArchiveFilesVM>> GetAllArchiveFiles(string SearchText,int BranchId)
        {
            var ArchiveFiles = _TaamerProContext.ArchiveFiles.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new ArchiveFilesVM
            {
                ArchiveFileId = x.ArchiveFileId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            });
            if (SearchText != "")
            {
                ArchiveFiles = ArchiveFiles.Where(s => s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim()));
            }
            return ArchiveFiles;
        }
    }
}
