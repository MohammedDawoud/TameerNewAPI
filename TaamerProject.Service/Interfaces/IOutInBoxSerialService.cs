using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IOutInBoxSerialService
    {
        Task<IEnumerable<OutInBoxSerialVM>> GetAllOutInBoxSerial(int Type, int BranchId);
        GeneralMessage SaveOutInBoxSerial(OutInBoxSerial OutInBoxSerial, int UserId, int BranchId);
        GeneralMessage DeleteOutInBoxSerial(int OutInSerialId, int UserId, int BranchId);
        int? GenerateOutInBoxNumber(int outInSerialId);
    }
}
