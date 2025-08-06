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
    public class ServicesRepository : IServicesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ServicesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<ServicesVM>> GetAllServices(int BranchId)
        {
            var Services = _TaamerProContext.Service.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new ServicesVM
            {
                ServiceId = x.ServiceId,
                Number = x.Number,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ExpireDate = x.ExpireDate,
                ExpireHijriDate = x.ExpireHijriDate,
                UserId = x.UserId,
                Notes = x.Notes,
                DepartmentId = x.DepartmentId,
                NotifyCount = x.NotifyCount,
                AccountId = x.AccountId,
                BankId = x.BankId,                
                RepeatAlarm = x.RepeatAlarm,
                RecurrenceRateId = x.RecurrenceRateId,
                AttachmentUrl = x.AttachmentUrl,


                DepartmentName = x.Department.DepartmentNameAr,
                BanksName = x.Banks.NameAr,
                AccountName = x.Account.NameAr,
            }).ToList();
            return Services;
        }
        public async Task<int> GetInvoicesAndServicesCount(int BranchId)
            {
                return _TaamerProContext.Service.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Count();
           
        }

        public async Task<IEnumerable<ServicesVM>> GetServicesSearch(ServicesVM ProjectsSearch, int BranchId)
        {
            var services = _TaamerProContext.Service.Where(s => s.IsDeleted == false && s.BranchId == BranchId
                                                                 && (s.DepartmentId == ProjectsSearch.DepartmentId || ProjectsSearch.DepartmentId == null)
                                                                 && (s.AccountId == ProjectsSearch.AccountId || ProjectsSearch.AccountId == null)
                                                                 && (s.Date == ProjectsSearch.Date || ProjectsSearch.Date == null)
                                                                 && (s.ExpireDate == ProjectsSearch.ExpireDate || ProjectsSearch.ExpireDate == null))
                                                .Select(x => new ServicesVM
                                                {
                                                    ServiceId = x.ServiceId,
                                                    Number = x.Number,
                                                    Date = x.Date,
                                                    HijriDate = x.HijriDate,
                                                    ExpireDate = x.ExpireDate,
                                                    ExpireHijriDate = x.ExpireHijriDate,
                                                    UserId = x.UserId,
                                                    Notes = x.Notes,
                                                    DepartmentId = x.DepartmentId,
                                                    NotifyCount = x.NotifyCount,
                                                    AccountId = x.AccountId,
                                                    BankId = x.BankId,                                                    
                                                    RepeatAlarm = x.RepeatAlarm,
                                                    RecurrenceRateId = x.RecurrenceRateId,
                                                    DepartmentName = x.Department.DepartmentNameAr,
                                                    AttachmentUrl = x.AttachmentUrl,

                                                    BanksName = x.Banks.NameAr,
                                                    AccountName = x.Account.NameAr,
                                                });
            return services;
        }
        public async Task<int?> GenerateNextServicesNumber(int BranchId)
        {
            var services = _TaamerProContext.Service.Where(s => s.BranchId == BranchId);
            if (services != null)
            {
                var lastRow = services.OrderByDescending(u => u.AddDate).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    return int.Parse(lastRow.Number) + 1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
    }
}
