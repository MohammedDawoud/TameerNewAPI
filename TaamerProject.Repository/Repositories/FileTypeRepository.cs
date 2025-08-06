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
    public class FileTypeRepository :  IFileTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public FileTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public IEnumerable<FileType> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FileTypeVM>> GetAllFileTypes(string? SearchText, int BranchId)
        {
            var FileTypes = _TaamerProContext.FileType.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new FileTypeVM
            {
                FileTypeId = x.FileTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            }).ToList();
            if (SearchText != "")
            {
                FileTypes = FileTypes.Where(s => s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim())).ToList();
            }
            return FileTypes;
        }

        public FileType GetById(int Id)
        {
            return _TaamerProContext.FileType.Where(x => x.FileTypeId == Id).FirstOrDefault();
        }

        public IEnumerable<FileType> GetMatching(Func<FileType, bool> where)
        {
            return _TaamerProContext.FileType.Where(where).ToList<FileType>();
        }
    }
}


