using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IContractStageService
    {
        Task<IEnumerable<ContractStageVM>> GetAllphasesByContractId(int? ContractId);

    }
}
