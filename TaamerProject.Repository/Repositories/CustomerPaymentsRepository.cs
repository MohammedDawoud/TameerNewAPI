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
    public class CustomerPaymentsRepository :  ICustomerPaymentsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CustomerPaymentsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPayments(int ContractId)
        {
            var customerPayments = _TaamerProContext.CustomerPayments.Where(s => s.IsDeleted == false && s.ContractId == ContractId).Select(x => new CustomerPaymentsVM
            {
                PaymentId = x.PaymentId,
                PaymentNo = x.PaymentNo,
                ContractId = x.ContractId,
                PaymentDate = x.PaymentDate,
                PaymentDateHijri = x.PaymentDateHijri,
                Amount = x.Amount,
                TaxAmount = x.TaxAmount,
                TotalAmount = x.TotalAmount,
                TransactionId = x.TransactionId,
                InvoiceId = x.InvoiceId,
                ToAccountId = x.ToAccountId,
                IsPaid = x.IsPaid,
                BranchId = x.BranchId,
                AmountValueText = x.AmountValueText,
                ServiceId=x.ServiceId??0,
                AmountValueText_EN=x.AmountValueText_EN==null?x.AmountValueText :x.AmountValueText_EN??"",
                AmountPercentage=x.AmountPercentage??0,   
                IsCanceled=x.IsCanceled??false,
                InvoiceNumber=x.Invoices!=null?x.Invoices.InvoiceNumber:"",

            }).ToList();
        
            return customerPayments;
        }
        public async Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsIsNotCanceled(int ContractId)
        {
            var customerPayments = _TaamerProContext.CustomerPayments.Where(s => s.IsDeleted == false && s.ContractId == ContractId && s.IsCanceled!=true).Select(x => new CustomerPaymentsVM
            {
                PaymentId = x.PaymentId,
                PaymentNo = x.PaymentNo,
                ContractId = x.ContractId,
                PaymentDate = x.PaymentDate,
                PaymentDateHijri = x.PaymentDateHijri,
                Amount = x.Amount,
                TaxAmount = x.TaxAmount,
                TotalAmount = x.TotalAmount,
                TransactionId = x.TransactionId,
                InvoiceId = x.InvoiceId,
                ToAccountId = x.ToAccountId,
                IsPaid = x.IsPaid,
                BranchId = x.BranchId,
                AmountValueText = x.AmountValueText,
                ServiceId = x.ServiceId ?? 0,
                AmountValueText_EN = x.AmountValueText_EN == null ? x.AmountValueText : x.AmountValueText_EN ?? "",
                AmountPercentage = x.AmountPercentage ?? 0,
                IsCanceled = x.IsCanceled ?? false,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber : "",


            }).ToList();

            return customerPayments;
        }

        public async Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsbyofferis(int offerid)
        {
            var customerPayments = _TaamerProContext.CustomerPayments.Where(s => s.IsDeleted == false && s.OfferId == offerid && s.IsCanceled != true).Select(x => new CustomerPaymentsVM
            {
                PaymentId = x.PaymentId,
                PaymentNo = x.PaymentNo,
                ContractId = x.ContractId,
                PaymentDate = x.PaymentDate,
                PaymentDateHijri = x.PaymentDateHijri,
                Amount = x.Amount,
                TaxAmount = x.TaxAmount,
                TotalAmount = x.TotalAmount,
                TransactionId = x.TransactionId,
                InvoiceId = x.InvoiceId,
                ToAccountId = x.ToAccountId,
                IsPaid = x.IsPaid,
                BranchId = x.BranchId,
                AmountValueText = x.AmountValueText,
                Isconst=x.Isconst,
                OfferId=x.OfferId,
                ServiceId = x.ServiceId ?? 0,
                AmountValueText_EN = x.AmountValueText_EN == null ? x.AmountValueText : x.AmountValueText_EN ?? "",
                AmountPercentage = x.AmountPercentage ?? 0,
                IsCanceled = x.IsCanceled ?? false,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber : "",

            }).ToList();

            return customerPayments;
        }

        public async Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsbyamounttxt(decimal amount,string amounttxt)
        {
            var customerPayments = _TaamerProContext.CustomerPayments.Where(s => s.IsDeleted == false && s.Amount== amount &&s.AmountValueText== amounttxt && s.IsCanceled != true).Select(x => new CustomerPaymentsVM
            {
                PaymentId = x.PaymentId,
                PaymentNo = x.PaymentNo,
                ContractId = x.ContractId,
                PaymentDate = x.PaymentDate,
                PaymentDateHijri = x.PaymentDateHijri,
                Amount = x.Amount,
                TaxAmount = x.TaxAmount,
                TotalAmount = x.TotalAmount,
                TransactionId = x.TransactionId,
                InvoiceId = x.InvoiceId,
                ToAccountId = x.ToAccountId,
                IsPaid = x.IsPaid,
                BranchId = x.BranchId,
                AmountValueText = x.AmountValueText,
                Isconst = x.Isconst,
                OfferId = x.OfferId,
                ServiceId = x.ServiceId ?? 0,
                AmountValueText_EN = x.AmountValueText_EN == null ? x.AmountValueText : x.AmountValueText_EN ?? "",
                AmountPercentage = x.AmountPercentage ?? 0,
                IsCanceled = x.IsCanceled ?? false,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber : "",


            }).ToList();

            return customerPayments;
        }


        public async Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsconst(int BranchId)
        {
            var customerPayments = _TaamerProContext.CustomerPayments.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Isconst == 1 && s.IsCanceled != true).Select(x => new CustomerPaymentsVM
            {
                PaymentId = x.PaymentId,
                PaymentNo = x.PaymentNo,
                ContractId = x.ContractId,
                PaymentDate = x.PaymentDate,
                PaymentDateHijri = x.PaymentDateHijri,
                Amount = x.Amount,
                TaxAmount = x.TaxAmount,
                TotalAmount = x.TotalAmount,
                TransactionId = x.TransactionId,
                InvoiceId = x.InvoiceId,
                ToAccountId = x.ToAccountId,
                IsPaid = x.IsPaid,
                BranchId = x.BranchId,
                AmountValueText = x.AmountValueText,
                Isconst = x.Isconst,
                OfferId = x.OfferId,
                ServiceId = x.ServiceId ?? 0,
                AmountValueText_EN = x.AmountValueText_EN == null ? x.AmountValueText : x.AmountValueText_EN ?? "",
                AmountPercentage = x.AmountPercentage ?? 0,
                IsCanceled = x.IsCanceled ?? false,

            }).ToList().GroupBy(s=>s.AmountValueText).Select(x=>x.First());

            return customerPayments;
        }

        public IEnumerable<CustomerPayments> GetAll()
        {
            throw new NotImplementedException();
        }

        public CustomerPayments GetById(int Id)
        {
          return  _TaamerProContext.CustomerPayments.Where(x => x.PaymentId == Id).FirstOrDefault();
        }

        public IEnumerable<CustomerPayments> GetMatching(Func<CustomerPayments, bool> where)
        {
            return _TaamerProContext.CustomerPayments.Where(where).ToList<CustomerPayments>();
        }
    }
}
