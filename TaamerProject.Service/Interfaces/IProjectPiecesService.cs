using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectPiecesService  
    {
        Task<IEnumerable<ProjectPiecesVM>> GetAllProjectPieces(int ProjectId,string SearchText);
        GeneralMessage SaveProjectPieces(ProjectPieces projectPieces, int UserId, int BranchId);
        GeneralMessage DeleteProjectPieces(int PieceId, int UserId, int BranchId);
    }
}
