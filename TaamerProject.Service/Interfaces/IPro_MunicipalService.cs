using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_MunicipalService  
    {
        Task<IEnumerable<Pro_MunicipalVM>> GetAllMunicipals(string SearchText);
        GeneralMessage SaveMunicipal(Pro_Municipal Municipal, int UserId, int BranchId);
        GeneralMessage DeleteMunicipal(int MunicipalId, int UserId, int BranchId);
    }
}
