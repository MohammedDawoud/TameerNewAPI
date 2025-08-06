using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface INationalityService
    {
        Task<IEnumerable<NationalityVM>> GetAllNationalities(string SearchText);
        GeneralMessage SaveNationality(Nationality nationality, int UserId, int BranchId);
        GeneralMessage DeleteNationality(int NationalityId, int UserId, int BranchId);
        IEnumerable<object> FillNationalitySelect(string SearchText = "");
    }

}
