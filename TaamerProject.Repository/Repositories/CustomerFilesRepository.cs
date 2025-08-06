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
    public class CustomerFilesRepository :  ICustomerFilesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CustomerFilesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<CustomerFilesVM>> GetAllCustomerFilesUploaded(int CustomerId ,string SearchText)
        {
            var customerFiles = _TaamerProContext.CustomerFiles.Where(s => s.IsDeleted == true && s.CustomerId == CustomerId).Select(x => new CustomerFilesVM
            {
                FileId = x.FileId,
                FileName = x.FileName,
                Description = x.Description,
                Extenstion = x.Extenstion,
                OriginalFileName = x.OriginalFileName,
                UploadDate = x.UploadDate,
                CustomerId = x.CustomerId,
                UserId = x.UserId,
                FileUrl = x.FileUrl,
                TypeId = x.TypeId,
                FileTypeName = x.FileType.NameAr,
                UserName = x.Users.FullName,
            }).ToList();
            return customerFiles;
         }
        public async Task<IEnumerable<CustomerFilesVM>> GetAllCustomerFiles(int? CustomerId)
        {
            var customerFiles = _TaamerProContext.CustomerFiles.Where(s => s.IsDeleted == false &&( CustomerId == null|| s.CustomerId == CustomerId)).Select(x => new CustomerFilesVM
            {
                FileId = x.FileId,
                FileName = x.FileName,
                Description = x.Description,
                Extenstion = x.Extenstion,
                OriginalFileName = x.OriginalFileName,
                UploadDate = x.UploadDate,
                CustomerId = x.CustomerId,
                UserId = x.UserId,
                FileUrl = x.FileUrl,
                TypeId = x.TypeId,
                FileTypeName = x.FileType.NameAr,
                UserName = x.Users.FullName,
            }).ToList();
            return customerFiles;
        }

        public IEnumerable<CustomerFiles> GetAll()
        {
            throw new NotImplementedException();
        }

        public CustomerFiles GetById(int Id)
        {
            return _TaamerProContext.CustomerFiles.Where(x => x.CustomerId == Id).FirstOrDefault();
        }

        public IEnumerable<CustomerFiles> GetMatching(Func<CustomerFiles, bool> where)
        {
            return _TaamerProContext.CustomerFiles.Where(where).ToList<CustomerFiles>();
        }
    }
}
