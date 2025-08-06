using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IFilesAuthService
    {
        Task<IEnumerable<FilesAuthVM>> GetAllFilesAuth();
        Task<FilesAuthVM> GetFilesAuthByTypeId(int TypeId);
        GeneralMessage SaveFileAuth(FilesAuth FileAuth, int UserId, int BranchId);
        GeneralMessage UpdateTokenData(FilesAuth FileAuth, int TypeId);

        GeneralMessage DeleteFileAuth(int FilesAuthId, int UserId, int BranchId);
    }
}
