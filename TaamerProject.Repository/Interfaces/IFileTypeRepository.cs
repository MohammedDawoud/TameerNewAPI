using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IFileTypeRepository : IRepository<FileType>
    {
        Task<IEnumerable<FileTypeVM>> GetAllFileTypes(string SearchText, int BranchId);
    }
}
