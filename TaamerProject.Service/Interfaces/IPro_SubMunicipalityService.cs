using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_SubMunicipalityService  
    {
        Task<IEnumerable<Pro_SubMunicipalityVM>> GetAllSubMunicipalitys(string SearchText);
        GeneralMessage SaveSubMunicipality(Pro_SubMunicipality SubMunicipality, int UserId, int BranchId);
        GeneralMessage DeleteSubMunicipality(int SubMunicipalityId, int UserId, int BranchId);
    }
}
