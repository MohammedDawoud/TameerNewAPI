using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IOfferpriceconditionService
    {
        Task<IEnumerable<OffersConditionsVM>> GetOfferconditionByid(int offerid);

        Task<IEnumerable<OffersConditionsVM>> GetOfferconditionByid(string offercontxt);
        Task<IEnumerable<OffersConditionsVM>> GetOfferconditionconst(int BranchId);
    }
}
