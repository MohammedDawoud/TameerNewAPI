
using TaamerProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace Bayanatech.TameerPro.Repository
{
    public interface IGuideDepartmentsRepository
    {
        Task<IEnumerable<GuideDepartmentsVM>> GetAllDeps(string lang, int? DepId = null);
    }
}
