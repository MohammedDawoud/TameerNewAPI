using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IModelTypeService
    {
        Task<IEnumerable<ModelTypeVM>> GetAllModelTypes(int BranchId);
        GeneralMessage SaveModelType(ModelType modelType, int UserId, int BranchId);
        GeneralMessage DeleteModelType(int ModelTypeId, int UserId, int BranchId);
        IEnumerable<object> FillModelTypeSelect(int BranchId);
    }
}
