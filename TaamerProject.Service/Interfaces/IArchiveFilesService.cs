using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IArchiveFilesService
    {
        Task<IEnumerable<ArchiveFilesVM>> GetAllArchiveFiles(string SearchText, int BranchId);
        GeneralMessage SaveArchiveFiles(ArchiveFiles ArchiveFiles, int UserId, int BranchId);
        GeneralMessage DeleteArchiveFiles(int ArchiveFileId, int UserId, int BranchId);
        IEnumerable<object> FillArchiveFilesSelect(int BranchId);
    }
}
