using TaamerProject.Models.Common;
using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaamerProject.Service.Interfaces
{
    public interface IOutMovementsService
    {
        Task<IEnumerable<OutMovementsVM>> GetAllOutMovements(int? TrailingId);
        GeneralMessage SaveOutMovements(OutMovements outMovements, int UserId, int BranchId);
        GeneralMessage DeleteOutMovements(int MoveId, int UserId, int BranchId);
        GeneralMessage ConfirmOutMovements(int? TrailingId, int UserId, int BranchId);
    }
}
