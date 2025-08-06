using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IFileTypeService
    {
        Task<IEnumerable<FileTypeVM>> GetAllFileTypes(string SearchText, int BranchId);
        GeneralMessage SaveFileType(FileType fileType, int UerId, int BranchId);
        GeneralMessage DeleteFileType(int FileTypeId, int UserId, int BranchId);
        IEnumerable<object> FillFileTypeSelect(int BranchId);
    }
}
