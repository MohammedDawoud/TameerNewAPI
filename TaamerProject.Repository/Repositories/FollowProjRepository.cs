using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace TaamerProject.Repository.Repositories
{
    public class FollowProjRepository : IFollowProjRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public FollowProjRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public IEnumerable<FollowProj> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FollowProjVM>> GetAllFollowProj()
        {
            var FollowProj = _TaamerProContext.FollowProj.Where(s => s.IsDeleted == false).Select(x => new FollowProjVM
            {
                FollowProjId = x.FollowProjId,
                ProjectId = x.ProjectId,
                EmpId = x.EmpId,
                TimeNo = x.TimeNo,
                TimeType = x.TimeType,
                EmpRate = x.EmpRate,
                Amount = x.Amount,
                ExpectedCost = x.ExpectedCost,
                ConfirmRate = x.ConfirmRate,
                EmpSalary = x.employees != null ? x.employees.Salary : 0,
                EmployeeName = x.employees != null ? x.employees.EmployeeNameAr : "",
                TimeStr = x.TimeType == "1" ? x.TimeNo + " يوم " : x.TimeType == "2" ? x.TimeNo + " أسبوع " : x.TimeType == "3" ? x.TimeNo + " شهر  " : x.TimeType == "4" ? x.TimeNo + " ساعة " : "",
                ProContractAmount = x.project != null ? x.project.Contracts != null ? x.project.Contracts.TotalValue : 0 : 0,
            }).ToList();
            return FollowProj;
        }
        public async Task<IEnumerable<FollowProjVM>> GetAllFollowProjById(int FollowProjId)
        {
            var FollowProj = _TaamerProContext.FollowProj.Where(s => s.IsDeleted == false && s.FollowProjId == FollowProjId).Select(x => new FollowProjVM
            {
                FollowProjId = x.FollowProjId,
                ProjectId = x.ProjectId,
                EmpId = x.EmpId,
                TimeNo = x.TimeNo,
                TimeType = x.TimeType,
                EmpRate = x.EmpRate,
                Amount = x.Amount,
                ExpectedCost = x.ExpectedCost,
                ConfirmRate = x.ConfirmRate,
                EmpSalary = x.employees != null ? x.employees.Salary : 0,
                EmployeeName = x.employees != null ? x.employees.EmployeeNameAr : "",
                TimeStr = x.TimeType == "1" ? x.TimeNo + " يوم " : x.TimeType == "2" ? x.TimeNo + " أسبوع " : x.TimeType == "3" ? x.TimeNo + " شهر  " : x.TimeType == "4" ? x.TimeNo + " ساعة " : "",
                ProContractAmount = x.project != null ? x.project.Contracts != null ? x.project.Contracts.TotalValue : 0 : 0,
            }).ToList();
            return FollowProj;
        }
        public async Task<IEnumerable<FollowProjVM>> GetAllFollowProjByProId(int ProjectId)
        {
            var FollowProj = _TaamerProContext.FollowProj.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new FollowProjVM
            {
                FollowProjId = x.FollowProjId,
                ProjectId = x.ProjectId,
                EmpId = x.EmpId,
                TimeNo = x.TimeNo,
                TimeType = x.TimeType,
                EmpRate = x.EmpRate,
                Amount = x.Amount,
                ExpectedCost = x.ExpectedCost,
                ConfirmRate = x.ConfirmRate,
                EmpSalary = x.employees != null ? x.employees.Salary : 0,
                EmployeeName = x.employees != null ? x.employees.EmployeeNameAr : "",
                TimeStr = x.TimeType == "1" ? x.TimeNo + " يوم " : x.TimeType == "2" ? x.TimeNo + " أسبوع " : x.TimeType == "3" ? x.TimeNo + " شهر  " : x.TimeType == "4" ? x.TimeNo + " ساعة " : "",
                ProContractAmount = x.project != null ? x.project.Contracts != null ? x.project.Contracts.TotalValue : 0 : 0,
            }).ToList();
            return FollowProj;
        }

        public FollowProj GetById(int Id)
        {
            return _TaamerProContext.FollowProj.Where(x => x.FollowProjId == Id).FirstOrDefault();
        }

        public IEnumerable<FollowProj> GetMatching(Func<FollowProj, bool> where)
        {
            return _TaamerProContext.FollowProj.Where(where).ToList<FollowProj>();
        }
    }

}