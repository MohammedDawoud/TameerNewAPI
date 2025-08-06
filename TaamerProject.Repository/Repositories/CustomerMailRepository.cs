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
    public class CustomerMailRepository :ICustomerMailRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CustomerMailRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<CustomerMailVM>> GetAllCustomerMails(int BranchId)
        {
            var CustomerMails = _TaamerProContext.CustomerMail.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new CustomerMailVM
            {
                MailId = x.MailId,
                SenderUser = x.SenderUser,
                CustomerId = x.CustomerId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                MailText = x.MailText,
                MailSubject = x.MailSubject,
                AllCustomers = x.AllCustomers,
                BranchId = x.BranchId,
                SenderUserName = x.Users.FullName,
            });
            return CustomerMails;
        }
        public async Task<IEnumerable<CustomerMailVM>> GetMailsByCustomerId(int? CustomerId)
        {
            var projects = _TaamerProContext.CustomerMail.Where(s => s.IsDeleted == false && (CustomerId == null|| s.CustomerId == CustomerId)).Select(x => new CustomerMailVM
            {
                MailId = x.MailId,
                SenderUser = x.SenderUser,
                CustomerId = x.CustomerId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                MailText = x.MailText,
                MailSubject = x.MailSubject,
                AllCustomers = x.AllCustomers,
                BranchId = x.BranchId,
                SenderUserName = x.Users.FullName,
            });
            return projects;
        }
    }
}


