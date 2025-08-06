using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IModelService
    {
       Task<IEnumerable<ModelVM>> GetAllModels(int BranchId);
        GeneralMessage SaveModel(Model model, int UserId, int BranchId);
        GeneralMessage DeleteModel(int ModelId, int UserId, int BranchId);
    }
}
