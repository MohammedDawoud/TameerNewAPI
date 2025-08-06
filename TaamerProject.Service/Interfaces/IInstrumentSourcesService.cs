using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IInstrumentSourcesService
    {
       Task<IEnumerable<InstrumentSourcesVM>> GetAllInstrumentSources(string SearchText);
        GeneralMessage SaveInstrumentSources(InstrumentSources instrumentSources, int UserId, int BranchId);
        GeneralMessage DeleteInstrumentSources(int instrumentSource, int UserId, int BranchId);
    }
}
