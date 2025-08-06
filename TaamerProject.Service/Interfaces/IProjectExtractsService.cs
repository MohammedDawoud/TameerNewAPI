using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectExtractsService  
    {
        Task<IEnumerable<ProjectExtractsVM>> GetAllProjectExtracts(int? ProjectId);
        GeneralMessage SaveProjectExtracts(ProjectExtracts projectExtracts, int UserId, int BranchId);
        GeneralMessage DeleteProjectExtracts(int ExtractId, int UserId, int BranchId);
        GeneralMessage UpdateExtractAttachment(ProjectExtracts projectExtracts, int UserId, int BranchId);
        GeneralMessage UpdateExtractSignature(ProjectExtracts projectExtracts,int UserId, int BranchId);
    }
}
