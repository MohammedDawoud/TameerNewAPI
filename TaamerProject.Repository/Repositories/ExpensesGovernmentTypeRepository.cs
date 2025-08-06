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

    public class ExpensesGovernmentTypeRepository :IExpensesGovernmentTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ExpensesGovernmentTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<ExpensesGovernmentTypeVM>> GetAllExpensesGovernmentTypes(string SearchText,int BranchId)
        {
            if (SearchText == "")
            {
                var ExpensesGovernmentTypes = _TaamerProContext.ExpensesGovernmentType.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new ExpensesGovernmentTypeVM
                {
                    ExpensesGovernmentTypeId = x.ExpensesGovernmentTypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                    UserId = x.UserId
                }).ToList();
                return ExpensesGovernmentTypes;
            }
            else
            {
                var ExpensesGovernmentTypes = _TaamerProContext.ExpensesGovernmentType.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new ExpensesGovernmentTypeVM
                {
                    ExpensesGovernmentTypeId = x.ExpensesGovernmentTypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                    UserId = x.UserId
                }).ToList();
                return ExpensesGovernmentTypes;
            }
        }
    }
}


