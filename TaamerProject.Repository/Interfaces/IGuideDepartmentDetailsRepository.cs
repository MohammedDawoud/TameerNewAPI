using TaamerProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace Bayanatech.TameerPro.Repository
{
    public interface IGuideDepartmentDetailsRepository
    {
        Task<IEnumerable<GuideDepartmentDetailsVM>> GetAllDepDetails(int DepId, string searchStr, int? DepDetailId = null);
        Task<IEnumerable<GuideDepartmentDetailsVM>> GetAllDepDetailsSearch(string searchStr);
    }
}
