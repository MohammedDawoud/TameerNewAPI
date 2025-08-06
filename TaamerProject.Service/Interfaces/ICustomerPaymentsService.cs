using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICustomerPaymentsService
    {
        Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPayments(int ContractId);
        Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsIsNotCanceled(int ContractId);

        IEnumerable<CustomerPaymentsVM> GetAllCustomerPaymentsPaid(int ContractId);

        GeneralMessage SaveCustomerPayment(CustomerPayments customerPayments, int UserId, int BranchId);
        GeneralMessage PayPayment(CustomerPayments customerPayments, int UserId, int BranchId, int? yearid);
        GeneralMessage DeleteCustomerPayment(int PaymentId, int UserId, int BranchId);
        int? GenerateCustPaymentNumber(int ContractId);
        GeneralMessage CancelPayment(int PaymentId, int UserId, int BranchId);
        PaymentReceiptVoucherVM GetPaymentReceipVoucher(int PaymentId, int BranchId);
        Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsboffer(int offerid);

        Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsbyamonttxt(decimal amount, string txt);
        Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsconst(int BranchId);
    }
}
