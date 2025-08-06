using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICustomerSMSService
    {
        Task<IEnumerable<CustomerSMSVM>> GetAllCustomerSMS(int BranchId);
        Task<IEnumerable<CustomerSMSVM>> GetSMSByCustomerId(int? CustomerId);
        GeneralMessage SaveCustomerSMS(CustomerSMS CustomerSMS, int UserId, int BranchId);
        GeneralMessage SaveCustomerSMS_Notification(string ReceiveNumber, string Message, int UserId, int BranchId);
        GeneralMessage  SendWhatsApp_Notification(string ReceiveNumber, string Message, int UserId, int BranchId, string environmentURL, string PDFURL);
        Task<int> SendWhatsApp_Notification_Task(string ReceiveNumber, string Message, int UserId, int BranchId, string environmentURL, string PDFURL);

        GeneralMessage DeleteCustomerSMS(int SMSId, int UserId, int BranchId);
        GeneralMessage SaveCustomerSMS2(CustomerSMS customerSMS, int UserId, int BranchId);
    }
}
