using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class GuranteesRepository : IGuranteesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public GuranteesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<GuaranteesVM>> GetAllGurantees(int BranchId)
        {
            var Gurantees = _TaamerProContext.Guarantees.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new GuaranteesVM
            {
                GuaranteeId = x.GuaranteeId,
                BankName = x.BankName,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                InvoiceId = x.InvoiceId,
                GuarantorAccId = x.GuarantorAccId,
                CustomerName = x.CustomerName,
                IsReturned = x.IsReturned,
                Number = x.Number,
                Percentage = x.Percentage,
                ProjectId = x.ProjectId,
                ReturnReason = x.ReturnReason,
                Type = x.Type,
                Value = x.Value,
                ProjectName = x.ProjectName,
                Period = x.Period,
                CustomerId = x.CustomerId,
                TypeName = x.Type == 1 ? "ابتدائي" : "نهائي",
                StatusName = x.IsReturned? "تم الاسترجاع" : "لم يتم الاسترجاع"
            }).ToList();
            return Gurantees;
        }
    }
}


