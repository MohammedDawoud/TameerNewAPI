using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class ContractServicesRepository : IContractServicesRepository
    {

        private readonly TaamerProjectContext _TaamerProContext;

        public ContractServicesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<ContractServicesVM>> GetContractservicenByid(int contractid)
        {

            var contracts = _TaamerProContext.ContractServices.Where(s => s.IsDeleted == false && s.ContractId == contractid).Select(x => new ContractServicesVM
            {
                ContractServicesId = x.ContractServicesId,
                ContractId = x.ContractId,
                ServiceId = x.ServiceId,
                ServiceQty = x.ServiceQty,
                BranchId = x.BranchId,
                serviceoffertxt = x.serviceoffertxt,
                TaxType = x.TaxType,
                servicename=x.serviceprice.ServicesName,
                Serviceamountval=x.Serviceamountval,
      

            }).ToList();
            return contracts;

        }

    }
}
