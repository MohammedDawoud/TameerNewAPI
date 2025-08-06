using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAcc_ClausesRepository
    {
        Task<IEnumerable<Acc_ClausesVM>> GetAllClauses(string SearchText);

    }
}
