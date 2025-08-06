using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
  public  interface IContractStageRepository 
    {

        Task<IEnumerable<ContractStageVM>> GetAllphasesByContractId(int? ContractId);
    }
}
