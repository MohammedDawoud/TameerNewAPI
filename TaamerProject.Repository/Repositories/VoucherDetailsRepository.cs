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
    public class VoucherDetailsRepository : IVoucherDetailsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public VoucherDetailsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByVoucherId(int? voucherId)
        {
            var details = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.InvoiceId == voucherId).Select(x => new VoucherDetailsVM
            {
                VoucherDetailsId = x.VoucherDetailsId,
                InvoiceId = x.InvoiceId,
                LineNumber = x.LineNumber,
                AccountId = x.AccountId,
                PayType = x.PayType,
                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.TaxType == 6) ? "حوالة" : (x.TaxType == 3) ? "عهده" : (x.TaxType == 4) ? "خصم مكتسب" : (x.TaxType == 5) ? "خصم مسموح به" : "",
                TaxType = x.TaxType,
                TaxTypeName = x.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                //AccountName = x.Accounts == null ? "" : x.AccountId == null ? "" : x.Accounts.NameAr,
                AccountName = "",
                Amount = x.Amount,
                Qty = x.Qty??1,
                TFK=x.TFK,
                CheckDate = x.CheckDate,
                CostCenterId = x.CostCenterId,
                TaxAmount = x.TaxAmount ?? 0,
                CostCenterName = x.CostCenters==null? "" : x.CostCenters.NameAr ?? "",
                Description = x.Description,
                ReferenceNumber = x.ReferenceNumber ?? "",
                ToAccountId = x.ToAccountId,
                ToAccountName = x.ToAccounts == null ? "" : x.ToAccounts == null ? "" : x.ToAccounts.NameAr,
                TotalAmount = x.TotalAmount,
                
                CheckNo = (x.CheckNo == "null") ? "" : (x.CheckNo == null) ? "" : x.CheckNo,
                BankName = x.Banks == null ? "" : x.Banks.NameAr??"",
                BankId = x.BankId,


                //BankName =(x.BankName == "1") ? "بنك الراجحي" : (x.BankName == "2") ? "البنك الأهلي" : (x.BankName == "3") ? "بنك البلاد" : (x.BankName == "4") ? "بنك الرياض" : (x.BankName == "5") ? "البنك العربي" :   "",

                MoneyOrderDate = x.MoneyOrderDate,
                MoneyOrderNo = x.MoneyOrderNo == null ? 0 : x.MoneyOrderNo,
                QRCode = x.Invoices.QRCodeNum ?? "",
                IsRetrieve=x.IsRetrieve??0,
                DiscountPercentage_Det = x.DiscountPercentage_Det ?? 0,
                DiscountValue_Det = x.DiscountValue_Det ?? 0,

            }).ToList();

            return details;
        }
        public async Task<VoucherDetailsVM> GetInvoiceIDByProjectID(int? ProjectId)
        {
            var details = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.Invoices.Rad==false && s.Invoices.ProjectId == ProjectId && s.Invoices.Type==2).Select(x => new VoucherDetailsVM
            {
                InvoiceId = x.InvoiceId,
                InvoiceNumber = x.Invoices.InvoiceNumber.ToString(),
                Amount = x.Amount,
                TotalAmount = x.TotalAmount,


            }).ToList().LastOrDefault();

            return details;
        }





        public async Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByInvoiceId(int? voucherId)
        {

            var details = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.InvoiceId == voucherId).Select(x => new VoucherDetailsVM
            {
                VoucherDetailsId = x.VoucherDetailsId,
                InvoiceId = x.InvoiceId,
                LineNumber = x.LineNumber,
                AccountId = x.AccountId,
                PayType = x.PayType,
                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.TaxType == 6) ? "حوالة" : (x.TaxType == 3) ? "عهده" : (x.TaxType == 4) ? "خصم مكتسب" : (x.TaxType == 5) ? "خصم مسموح به" : "",
                TaxType = x.TaxType,
                TaxTypeName = x.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                //AccountName = x.Accounts == null ? "" : x.AccountId == null ? "" : x.Accounts.NameAr,
                AccountName = "",
                Amount = x.Amount,
                Qty = x.Qty ?? 1,
                CostCenterId = x.CostCenterId,
                TaxAmount = x.TaxAmount ?? 0,
                CostCenterName = x.CostCenters == null ? "" : x.CostCenters.NameAr ?? "",
                Description = x.Description,
                ReferenceNumber = x.ReferenceNumber ?? "",
                ToAccountId = x.ToAccountId,
                ToAccountName = x.ToAccounts == null ? "": x.ToAccountId == null ? "" : x.ToAccounts.NameAr,
                //TotalAmount = (x.Amount * x.Qty) + x.TaxAmount ?? 0,
                TotalAmount = x.TotalAmount ?? 0,

                CheckNo = (x.CheckNo == "null") ? "" : (x.CheckNo == null) ? "" : x.CheckNo,
                CheckDate = x.CheckDate,
                BankName = x.Banks == null ? "" : x.Banks.NameAr,
                BankId = x.BankId,
                MoneyOrderDate = x.MoneyOrderDate,
                MoneyOrderNo = x.MoneyOrderNo == null ? 0 : x.MoneyOrderNo,
                ServicesPriceId = x.ServicesPriceId,
                ServicesPriceName = x.ServicesPrice == null ? "" : x.ServicesPrice.ServicesName ?? "",
                ServiceTypeName = x.ServicesPrice != null ? x.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                QRCode = x.Invoices.QRCodeNum ?? "",
                IsRetrieve = x.IsRetrieve ?? 0,
                DiscountPercentage_Det = x.DiscountPercentage_Det ?? 0,
                DiscountValue_Det = x.DiscountValue_Det ?? 0,

                //Reference=x.Invoices.Reference,
                //PaidValue = x.Invoices.PaidValue
            }).ToList();

            return details;
        }
        public async Task<VoucherDetailsVM> GetAllDetailsByVoucherDetailsId(int? VoucherDetailsId)
        {

            var details = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.VoucherDetailsId == VoucherDetailsId).Select(x => new VoucherDetailsVM
            {
                VoucherDetailsId = x.VoucherDetailsId,
                InvoiceId = x.InvoiceId,
                LineNumber = x.LineNumber,
                AccountId = x.AccountId,
                PayType = x.PayType,
                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.TaxType == 6) ? "حوالة" : (x.TaxType == 3) ? "عهده" : (x.TaxType == 4) ? "خصم مكتسب" : (x.TaxType == 5) ? "خصم مسموح به" : "",
                TaxType = x.TaxType,
                TaxTypeName = x.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                //AccountName = x.Accounts == null ? "" : x.AccountId == null ? "" : x.Accounts.NameAr,
                AccountName = "",
                Amount = x.Amount,
                Qty = x.Qty ?? 1,
                CostCenterId = x.CostCenterId,
                TaxAmount = x.TaxAmount ?? 0,
                CostCenterName = x.CostCenters == null ? "" : x.CostCenters.NameAr ?? "",
                Description = x.Description,
                ReferenceNumber = x.ReferenceNumber ?? "",
                ToAccountId = x.ToAccountId,
                ToAccountName = x.ToAccounts == null ? "" : x.ToAccountId == null ? "" : x.ToAccounts.NameAr,
                //TotalAmount = (x.Amount * x.Qty) + x.TaxAmount ?? 0,
                TotalAmount = x.TotalAmount ?? 0,

                CheckNo = (x.CheckNo == "null") ? "" : (x.CheckNo == null) ? "" : x.CheckNo,
                CheckDate = x.CheckDate,
                BankName = x.Banks == null ? "" : x.Banks.NameAr,
                BankId = x.BankId,
                MoneyOrderDate = x.MoneyOrderDate,
                MoneyOrderNo = x.MoneyOrderNo == null ? 0 : x.MoneyOrderNo,
                ServicesPriceId = x.ServicesPriceId,
                ServicesPriceName = x.ServicesPrice == null ? "" : x.ServicesPrice.ServicesName ?? "",
                ServiceTypeName = x.ServicesPrice != null ? x.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                QRCode = x.Invoices.QRCodeNum ?? "",
                IsRetrieve = x.IsRetrieve ?? 0,
                DiscountPercentage_Det = x.DiscountPercentage_Det ?? 0,
                DiscountValue_Det = x.DiscountValue_Det ?? 0,

                //Reference=x.Invoices.Reference,
                //PaidValue = x.Invoices.PaidValue
            }).FirstOrDefault();

            return details;
        }

        public async Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByInvoiceIdFirstOrDef(int? voucherId)
        {

            var details = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.InvoiceId == voucherId && s.LineNumber==1).Select(x => new VoucherDetailsVM
            {
                VoucherDetailsId = x.VoucherDetailsId,
                InvoiceId = x.InvoiceId,
                LineNumber = x.LineNumber,
                AccountId = x.AccountId,
                PayType = x.PayType,
                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.TaxType == 6) ? "حوالة" : (x.TaxType == 3) ? "عهده" : (x.TaxType == 4) ? "خصم مكتسب" : (x.TaxType == 5) ? "خصم مسموح به" : "",
                TaxType = x.TaxType,
                TaxTypeName = x.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                //AccountName = x.Accounts == null ? "" : x.AccountId == null ? "" : x.Accounts.NameAr,
                AccountName = "",
                Amount = x.Amount,
                Qty = x.Qty ?? 1,
                CostCenterId = x.CostCenterId,
                TaxAmount = x.TaxAmount ?? 0,
                CostCenterName = x.CostCenters == null ? "" : x.CostCenters.NameAr ?? "",
                Description = x.Description,
                ReferenceNumber = x.ReferenceNumber ?? "",
                ToAccountId = x.ToAccountId,
                ToAccountName = x.ToAccounts == null ? "" : x.ToAccountId == null ? "" : x.ToAccounts.NameAr,
                //TotalAmount = (x.Amount * x.Qty) + x.TaxAmount ?? 0,
                TotalAmount = x.TotalAmount ?? 0,

                CheckNo = (x.CheckNo == "null") ? "" : (x.CheckNo == null) ? "" : x.CheckNo,
                CheckDate = x.CheckDate,
                BankName = x.Banks == null ? "" : x.Banks.NameAr,
                BankId = x.BankId,
                MoneyOrderDate = x.MoneyOrderDate,
                MoneyOrderNo = x.MoneyOrderNo == null ? 0 : x.MoneyOrderNo,
                ServicesPriceId = x.ServicesPriceId,
                ServicesPriceName = x.ServicesPrice == null ? "" : x.ServicesPrice.ServicesName ?? "",
                ServiceTypeName = x.ServicesPrice != null ? x.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",
                QRCode = x.Invoices.QRCodeNum ?? "",
                IsRetrieve = x.IsRetrieve ?? 0,
                DiscountPercentage_Det = x.DiscountPercentage_Det ?? 0,
                DiscountValue_Det = x.DiscountValue_Det ?? 0,

                //Reference=x.Invoices.Reference,
                //PaidValue = x.Invoices.PaidValue
            }).ToList();

            return details;
        }

        public async Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByInvoiceIdPurchase(int? voucherId)
        {

            var details = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.InvoiceId == voucherId).Select(x => new VoucherDetailsVM
            {
                VoucherDetailsId = x.VoucherDetailsId,
                InvoiceId = x.InvoiceId,
                LineNumber = x.LineNumber,
                AccountId = x.AccountId,
                PayType = x.PayType,
                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.TaxType == 6) ? "حوالة" : (x.TaxType == 3) ? "عهده" : (x.TaxType == 4) ? "خصم مكتسب" : (x.TaxType == 5) ? "خصم مسموح به" : "",
                TaxType = x.TaxType,
                TaxTypeName = x.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                //AccountName = x.Accounts == null ? "" : x.AccountId == null ? "" : x.Accounts.NameAr,
                AccountName = "",
                Amount = x.Amount,
                Qty = x.Qty ?? 1,
                CostCenterId = x.CostCenterId,
                TaxAmount = x.TaxAmount ?? 0,
                CostCenterName = x.CostCenters == null ? "" : x.CostCenters.NameAr ?? "",
                Description = x.Description,
                ReferenceNumber = x.ReferenceNumber ?? "",
                ToAccountId = x.ToAccountId,
                ToAccountName = x.ToAccounts == null ? "" : x.ToAccountId == null ? "" : x.ToAccounts.NameAr,
                //TotalAmount = (x.Amount * x.Qty) + x.TaxAmount ?? 0,
                TotalAmount = x.TotalAmount ?? 0,

                CheckNo = (x.CheckNo == "null") ? "" : (x.CheckNo == null) ? "" : x.CheckNo,
                CheckDate = x.CheckDate,
                BankName = x.Banks == null ? "" : x.Banks.NameAr,
                BankId = x.BankId,
                MoneyOrderDate = x.MoneyOrderDate,
                MoneyOrderNo = x.MoneyOrderNo == null ? 0 : x.MoneyOrderNo,
                CategoryId = x.CategoryId,
                CategoryName = x.Categories == null ? "" : x.Categories.NAmeAr ?? "",
                CategoryTypeName = x.Categories != null ? x.Categories.CategorType != null ? x.Categories.CategorType.NAmeAr ?? "" : "" : "",
                QRCode = x.Invoices.QRCodeNum ?? "",
                IsRetrieve = x.IsRetrieve ?? 0,
                DiscountPercentage_Det = x.DiscountPercentage_Det ?? 0,
                DiscountValue_Det = x.DiscountValue_Det ?? 0,

            }).ToList();

            return details;
        }

        public async Task<IEnumerable<VoucherDetailsVM>> GetAllTransByLineNo(int LineNo)
        {
            var details = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.LineNumber == LineNo).Select(x => new VoucherDetailsVM
            {
                VoucherDetailsId = x.VoucherDetailsId,
                InvoiceId = x.InvoiceId,
                LineNumber = x.LineNumber,
                AccountId = x.AccountId,
                PayType = x.PayType,
                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.TaxType == 6) ? "حوالة" : (x.TaxType == 3) ? "عهده" : (x.TaxType == 4) ? "خصم مكتسب" : (x.TaxType == 5) ? "خصم مسموح به" : "",
                TaxType = x.TaxType,
                TaxTypeName = x.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                //AccountName = x.Accounts == null ? "" : x.AccountId == null ? "" : x.Accounts.NameAr,
                AccountName = "",
                Amount = x.Amount,
                Qty = x.Qty ?? 1,
                CostCenterId = x.CostCenterId,
                TaxAmount = x.TaxAmount ?? 0,
                CostCenterName = x.CostCenters==null?"": x.CostCenters.NameAr ?? "",
                Description = x.Description,
                ReferenceNumber = x.ReferenceNumber ?? "",
                ToAccountId = x.ToAccountId,
                ToAccountName = x.ToAccounts == null ? "" : x.ToAccounts == null ? "" : x.ToAccounts.NameAr,
                TotalAmount = x.TotalAmount,
                CheckNo = (x.CheckNo == "null") ? "" : (x.CheckNo == null) ? "" : x.CheckNo,
                CheckDate = x.CheckDate,
                BankName = x.Banks == null ? "" : x.Banks.NameAr??"",
                BankId = x.BankId,
                MoneyOrderDate = x.MoneyOrderDate,
                MoneyOrderNo = x.MoneyOrderNo == null ? 0 : x.MoneyOrderNo,
                QRCode = x.Invoices.QRCodeNum ?? "",
                IsRetrieve = x.IsRetrieve ?? 0,
                DiscountPercentage_Det = x.DiscountPercentage_Det ?? 0,
                DiscountValue_Det = x.DiscountValue_Det ?? 0,


            }).ToList();

            return details;
        }

        public async Task<IEnumerable<VoucherDetailsVM>> GetAllTrans(int VouDetailsID)
        {
            var details = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false).Select(x => new VoucherDetailsVM
            {
                VoucherDetailsId = x.VoucherDetailsId,
                InvoiceId = x.InvoiceId,
                LineNumber = x.LineNumber,
                AccountId = x.AccountId,
                PayType = x.PayType,
                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.TaxType == 6) ? "حوالة" : (x.TaxType == 3) ? "عهده" : (x.TaxType == 4) ? "خصم مكتسب" : (x.TaxType == 5) ? "خصم مسموح به" : "",
                TaxType = x.TaxType,
                TaxTypeName = x.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                //AccountName = x.Accounts == null ? "" : x.AccountId == null ? "" : x.Accounts.NameAr,
                AccountName = "",
                Amount = x.Amount,
                Qty = x.Qty ?? 1,
                CostCenterId = x.CostCenterId,
                TaxAmount = x.TaxAmount ?? 0,
                CostCenterName = x.CostCenters==null?"": x.CostCenters.NameAr ?? "",
                Description = x.Description,
                ReferenceNumber = x.ReferenceNumber ?? "",
                ToAccountId = x.ToAccountId,
                ToAccountName = x.ToAccounts == null ? "" : x.ToAccounts == null ? "" : x.ToAccounts.NameAr,
                TotalAmount = x.TotalAmount,
                CheckNo = (x.CheckNo == "null") ? "" : (x.CheckNo == null) ? "" : x.CheckNo,
                CheckDate = x.CheckDate,
                BankName = x.Banks == null ? "" : x.Banks.NameAr,
                BankId = x.BankId,
                MoneyOrderDate = x.MoneyOrderDate,
                MoneyOrderNo = x.MoneyOrderNo == null ? 0 : x.MoneyOrderNo,
                QRCode = x.Invoices.QRCodeNum ?? "",
                IsRetrieve = x.IsRetrieve ?? 0,
                DiscountPercentage_Det = x.DiscountPercentage_Det ?? 0,
                DiscountValue_Det = x.DiscountValue_Det ?? 0,


            }).ToList();

            return details;
        }
    }
}


