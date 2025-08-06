using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TaamerProject.Models;
using System.Data;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
  public  class ContractStageRepository :  IContractStageRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ContractStageRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<ContractStageVM>> GetAllphasesByContractId(int? ContractId)
        {
            var Stage = _TaamerProContext.ContractStage.Where(s => s.IsDeleted == false && s.ContractId == ContractId).Select(x => new ContractStageVM
            {
                ContractStageId=x.ContractStageId,
                Stage=x.Stage,
                StageDescreption=x.StageDescreption,
                Stageenddate=x.Stageenddate,
                Stagestartdate=x.Stagestartdate,
                ContractId=x.ContractId,
            
            }).ToList();
            return Stage;
        }


    }
}
