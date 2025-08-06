using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IInstrumentsService
    {
        Task<IEnumerable<InstrumentsVM>> GetAllInstruments(int ProjectId);
        GeneralMessage SaveInstruments(List<Instruments> instruments, int UserId, int BranchId);
        GeneralMessage DeleteInstruments(int instrumentId, int UserId, int BranchId);
        GeneralMessage SaveInstrument(Instruments instruments, int UserId, int BranchId);
    }
}
