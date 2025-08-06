using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IEmpStructureService  
    {
        GeneralMessage SaveEmpStructure(List<EmpStructure> empLink, List<NodeLocations> NodeLocList, int UserId, int BranchId);
        EmpNodeVM GetAllNodesEmps(string lang, int BranchId);
    }
}
