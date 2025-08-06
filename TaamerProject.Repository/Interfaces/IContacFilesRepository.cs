using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IContacFilesRepository 
    {
        Task<IEnumerable<ContacFilesVM>> GetAllFiles(int? OutInBoxId);
        Task<IEnumerable<ContacFilesVM>> GetAllContacFiles();
        Task<IEnumerable<ContacFilesVM>> GetAllFilesByParams(int? ArchiveFileId, int? Type, int? OutInType, int BranchId);
        Task<IEnumerable<ContacFilesVM>> GetAllFilesByParams(int? ArchiveFileId, int? Type, int? OutInType, int? OutInboxId, int BranchId);
    }
}
