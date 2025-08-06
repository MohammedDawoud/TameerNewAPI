using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IOfferpriceconditionRepository
    {

        Task<IEnumerable<OffersConditionsVM>> GetOfferconditionByid(int offerid);
        Task<IEnumerable<OffersConditionsVM>> GetOfferconditionBytxt(string OfferConditiontxt);
        Task<IEnumerable<OffersConditionsVM>> GetOfferconditionconst(int BranchId);
    }
}
