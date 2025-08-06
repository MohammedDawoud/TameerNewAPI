using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectTrailingService  
    {
        Task<IEnumerable<ProjectTrailingVM>> GetProjectTrailingInOfficeArea(int BranchId);
        Task<IEnumerable<ProjectTrailingVM>> GetProjectTrailingInExternalSide(int BranchId);
        GeneralMessage SaveProjectTrailing(ProjectTrailing ProjectTrailing, int UserId, int BranchId, string Lang);
        GeneralMessage DeleteProjectTrailing(int TrailingId, int UserId, int BranchId);
        Task<IEnumerable<TrailingFilesVM>> GetTrailingFilesByTrailingId(int? TrailingId, string SearchText);
        GeneralMessage ReceiveProjectTrailing(ProjectTrailing ProjectTrailing, int UserId, int BranchId);
        GeneralMessage RejectProjectTrailing(ProjectTrailing ProjectTrailing, int UserId, int BranchId);
        GeneralMessage AcceptProjectTrailing(int? TrailingId, int UserId, int BranchId);
    }
}
