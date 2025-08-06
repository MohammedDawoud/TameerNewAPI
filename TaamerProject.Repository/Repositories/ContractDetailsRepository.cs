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
    public class ContractDetailsRepository : IContractDetailsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ContractDetailsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<ContractDetailsVM>> GetAllConDetailsByContractId(int? ContractId)
        {
            try
            {
                return _TaamerProContext.ContractDetails.Where(s => s.IsDeleted == false && s.ContractId == ContractId).Select(x => new
                {
                    x.ContractDetailId,
                    x.ContractId,
                    x.SerialId,
                    x.Clause,

                }).Select(s => new ContractDetailsVM
                {
                    ContractDetailId = s.ContractDetailId,
                    ContractId = s.ContractId,
                    SerialId = s.SerialId,
                    Clause = s.Clause,

                }).ToList();
            }
            catch (Exception)
            {
                return _TaamerProContext.ContractDetails.Where(s => s.IsDeleted == false && s.ContractId == ContractId).Select(x => new ContractDetailsVM
                {
                    ContractDetailId = x.ContractDetailId,
                    ContractId = x.ContractId,
                    SerialId = x.SerialId,
                    Clause = x.Clause,
                });
            }
        }

        public async Task<IEnumerable<ContractDetailsVM>> GetAllDetailsByContractId(int? ContractId)
        {
            var details = _TaamerProContext.ContractDetails.Where(s => s.IsDeleted == false && s.ContractId == ContractId).Select(x => new ContractDetailsVM
            {
                ContractDetailId = x.ContractDetailId,
                ContractId = x.ContractId,
                SerialId = x.SerialId,
                Clause = x.Clause,
            }).ToList();
            return details;
        }


    }
}

