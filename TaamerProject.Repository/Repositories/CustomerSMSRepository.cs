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
    public class CustomerSMSRepository :  ICustomerSMSRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CustomerSMSRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<CustomerSMSVM>> GetAllCustomerSMS(int BranchId)
        {
            var CustomerSMS = _TaamerProContext.CustomerSMS.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new CustomerSMSVM
            {
                SMSId = x.SMSId,
                CustomerId = x.CustomerId,
                SenderUser = x.SenderUser,
                SMSText = x.SMSText,
                SMSSubject = x.SMSSubject,
                Date = x.Date,
                HijriDate = x.HijriDate,
                AllCustomers = x.AllCustomers,
          
                BranchId = x.BranchId,
                SenderUserName = x.Users.FullName,

            }).ToList();
            return CustomerSMS;
        }
        public async Task<IEnumerable<CustomerSMSVM>> GetSMSByCustomerId(int? CustomerId)
        {
            var projects = _TaamerProContext.CustomerSMS.Where(s => s.IsDeleted == false && (CustomerId == null|| s.CustomerId == CustomerId)).Select(x => new CustomerSMSVM
            {
                SMSId = x.SMSId,
                CustomerId = x.CustomerId,
                SenderUser = x.SenderUser,
                SMSText = x.SMSText,
                SMSSubject = x.SMSSubject,
                Date = x.Date,
                HijriDate = x.HijriDate,
                AllCustomers = x.AllCustomers,
                BranchId = x.BranchId,
                SenderUserName = x.Users.FullName,
            });
            return projects;
        }
    }
}
