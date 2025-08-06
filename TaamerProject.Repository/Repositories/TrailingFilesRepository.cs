using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class TrailingFilesRepository : ITrailingFilesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public TrailingFilesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<TrailingFilesVM>> GetTrailingFilesByTrailingId(int? TrailingId, string SearchText)
        {
            var trailingFiles = _TaamerProContext.TrailingFiles.Where(s => s.IsDeleted == false && s.TrailingId == TrailingId).Select(x => new TrailingFilesVM
            {
                FileId = x.FileId,
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                TypeId = x.TypeId,
                ProjectId = x.ProjectId,
                TrailingId = x.TrailingId,
                Notes = x.Notes,
                BranchId = x.BranchId,
            }).ToList();
            if (SearchText != "")
            {
                trailingFiles = trailingFiles.Where(s => s.FileName.Contains(SearchText.Trim())).ToList();
            }
            return trailingFiles;
        }
    }
}
