using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class AppraisalRepository : IAppraisalRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AppraisalRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<AppraisalVM>> GetAllAppraisal()
        {
            var appraisal = _TaamerProContext.Appraisal.Where(s => s.IsDeleted == false).Select(x => new AppraisalVM
            {
                AppraisalId = x.AppraisalId,
                EmpId = x.EmpId,
                Degree = x.Degree,
                ManagerId = x.ManagerId,
                MonthDate = x.MonthDate,
                Month = x.Month,
                Year = x.Year,
                EmployeeName = x.Employees.EmployeeNameAr,

            });
            return appraisal;
        }


        public async Task<IEnumerable<AppraisalVM>> SearchAppraisal(AppraisalVM AppraisalySearch, string lang, int BranchId)
        {

            var appraisal = _TaamerProContext.Appraisal.Where(s => s.IsDeleted == false).Select(x => new AppraisalVM
            {
                AppraisalId = x.AppraisalId,
                EmpId = x.EmpId,
                Degree = x.Degree,
                ManagerId = x.ManagerId,
                MonthDate = x.MonthDate,
                Month = x.Month,
                Year = x.Year,
                EmployeeName = x.Employees.EmployeeNameAr,

            }).ToList();


         


            if (!String.IsNullOrWhiteSpace(Convert.ToString(AppraisalySearch.EmpId)))
            {
                appraisal = appraisal.Where(w => w.EmpId == AppraisalySearch.EmpId).ToList();
            }
           

            return appraisal;
        }
    }
}
