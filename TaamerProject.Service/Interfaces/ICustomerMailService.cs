using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICustomerMailService
    {
        Task<IEnumerable<CustomerMailVM>> GetAllCustomerMails(int BranchId);
        Task<IEnumerable<CustomerMailVM>> GetMailsByCustomerId(int? CustomerId);
        GeneralMessage SaveCustomerMail(CustomerMail CustomerMail, int UserId, int BranchId, string AttachmentFile, bool? IsOrgEmail = null);
        GeneralMessage SaveCustomerMailOfferPrice(CustomerMailVM CustomerMail, int UserId, int BranchId, string AttachmentFile,string body, bool? IsOrgEmail = null);

        GeneralMessage DeleteCustomerMail(int MailId, int UserId, int BranchId);
        bool SendMail_SysNotification(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, bool IsBodyHtml = false, string empmail = null);
    }
}
