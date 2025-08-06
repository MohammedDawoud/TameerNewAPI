using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.Common;
using static TaamerProject.Models.ReportGridVM;
using TaamerProject.Models.Enums;
using Microsoft.Identity.Client;

namespace TaamerProject.Repository.Repositories
{
    public class InvoicesRepository :  IInvoicesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

         public InvoicesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

       public async Task<IEnumerable<InvoicesVM>> GetAllVoucherstoback()
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type==2).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,

                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",

                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr  : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",


                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullName,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue ?? 0,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,

                InvUUID=x.InvUUID??"",

                CreditNotiId = x.CreditNotiId ?? 0,
                DepitNotiId = x.DepitNotiId ?? 0,

                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts!.NameAr,
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts!.NameAr,
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate,
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    ServicesPriceId = z.ServicesPriceId,
                    ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                    ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                    IsRetrieve = z.IsRetrieve??0,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,

                }).ToList()
            }).OrderByDescending(s => s.InvoiceNumber).ToList();

            return details;
        }
       public async Task<IEnumerable<InvoicesVM>> GetAllVouchers(VoucherFilterVM voucherFilterVM, int YearId, int BranchId) 
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad==true ?"ملغي":x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad=x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes??"",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                 InvoiceValue = x.InvoiceValue??0,
                
                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage??0,
                DiscountValue = x.DiscountValue?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue??0,
                ToInvoiceId=x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId=x.InvoiceRetId ?? "000000",
                DunCalc =x.DunCalc??false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : (x.PayType == 15) ? "عهد موظفين" : (x.PayType == 16) ? "جاري المالك" : "",
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                VoucherAlarmDate=x.VoucherAlarmDate??"",
                VoucherAlarmCheck=x.VoucherAlarmCheck??false,
                IsSendAlarm=x.IsSendAlarm??0,
                InvUUID = x.InvUUID ?? "",

                CreditNotiId = x.CreditNotiId ?? 0,
                DepitNotiId = x.DepitNotiId ?? 0,
                AddInvoiceImg=x.AddUsers!.ImgUrl?? "/distnew/images/userprofile.png",
                DelegateId=x.DelegateId,
                InvoicesRequests = x.InvoicesRequests!.Where(s => s.InvoiceId == x.InvoiceId).Select(w => new Acc_InvoicesRequestsVM
                {
                    InvoiceReqId = w.InvoiceReqId,
                    InvoiceId = w.InvoiceId,
                    InvoiceNoRequest = w.InvoiceNoRequest,
                    IsSent = w.IsSent,
                    StatusCode = w.StatusCode,
                    SendingStatus = w.SendingStatus,
                    warningmessage = w.warningmessage,
                    ClearedInvoice = w.ClearedInvoice,
                    errormessage = w.errormessage,
                    PIH = w.PIH,
                    BranchId = w.BranchId,
                }).ToList(),
                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts!=null? z.Accounts!.NameAr:"",
                    Amount = z.Amount,
                    Qty = z.Qty??1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName =z.ToAccounts!=null? z.ToAccounts!.NameAr:"",
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate??"",
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    ServicesPriceId = z.ServicesPriceId,
                    ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                    ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                    IsRetrieve = z.IsRetrieve ?? 0,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,

                }).ToList()
            }).OrderByDescending(s => s.InvoiceNumber).ToList() ;

            return details;
        }
       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersLastMonth(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,

                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue ?? 0,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع"
                : (x.PayType == 15) ? "عهد موظفين" : (x.PayType == 16) ? "جاري المالك" : "",
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,
                InvUUID = x.InvUUID ?? "",

                CreditNotiId = x.CreditNotiId ?? 0,
                DepitNotiId = x.DepitNotiId ?? 0,
                AddInvoiceImg=x.AddUsers!.ImgUrl?? "/distnew/images/userprofile.png",

                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts != null ? z.Accounts!.NameAr : "",
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts != null ? z.ToAccounts!.NameAr : "",
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate ?? "",
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    ServicesPriceId = z.ServicesPriceId,
                    ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                    ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                    IsRetrieve = z.IsRetrieve ?? 0,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,

                }).ToList()
            }).OrderByDescending(s => s.InvoiceNumber).ToList().Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));

            return details;
        }
        
       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersSearch(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId && s.Rad != true && s.PayType == 8 && s.StoreId != 1).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,

                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue ?? 0,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,
                InvUUID = x.InvUUID ?? "",

                CreditNotiId = x.CreditNotiId ?? 0,
                DepitNotiId = x.DepitNotiId ?? 0,
                AddInvoiceImg=x.AddUsers!.ImgUrl?? "/distnew/images/userprofile.png",
                DelegateId=x.DelegateId,
                //InvoicesRequestsCount = x.InvoicesRequests!.Where(s => s.InvoiceId == x.InvoiceId).ToList().Count(),
                InvoicesRequests = x.InvoicesRequests!.Where(s => s.InvoiceId == x.InvoiceId).Select(w => new Acc_InvoicesRequestsVM
                {
                    InvoiceReqId = w.InvoiceReqId,
                    InvoiceId = w.InvoiceId,
                    InvoiceNoRequest = w.InvoiceNoRequest,
                    IsSent = w.IsSent,
                    StatusCode = w.StatusCode,
                    SendingStatus = w.SendingStatus,
                    warningmessage = w.warningmessage,
                    ClearedInvoice = w.ClearedInvoice,
                    errormessage = w.errormessage,
                    PIH = w.PIH,
                    BranchId = w.BranchId,
                }).ToList(),
                
            //CreditDepitNotiTotal=x.InvoicesNoti.Where(s => s.CreditDepitNotiId != null && s.InvoicesNoti.Any(a => a.IsDeleted == false)).Sum(q => q.TotalValue),
            VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts != null ? z.Accounts!.NameAr : "",
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts != null ? z.ToAccounts!.NameAr : "",
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate ?? "",
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    ServicesPriceId = z.ServicesPriceId,
                    ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                    ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة":"خدمة",

                    IsRetrieve = z.IsRetrieve ?? 0,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,

                }).ToList()
            }).OrderByDescending(s => s.InvoiceNumber).ToList();

            return details;
        }
        //dawoudc
       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersSearchCustomer(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost==true && s.Type == voucherFilterVM!.Type
            && s.YearId == YearId && s.BranchId == BranchId &&
            (voucherFilterVM!.CustomerId == null || voucherFilterVM!.CustomerId == 0 || s.CustomerId == voucherFilterVM!.CustomerId ) 
            && s.Rad != true && s.PayType == 8).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,

                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue ?? 0,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,
                InvUUID = x.InvUUID ?? "",
                //ProjectDescription = x.Project != null ? x.Project.ProjectDescription : null,
                //SketchNo = x.Project != null ? x.Project.SketchNo : null,
                //DistrictName = x.Project != null ? x.Project.DistrictName : null,
                //pieceNo = x.Project != null ? _TaamerProContext.ProjectPieces.Where(s => s.PieceId == x.Project.PieceNo)!.FirstOrDefault()!.PieceNo : "" ,
                CreditNotiId = x.CreditNotiId ?? 0,
                DepitNotiId = x.DepitNotiId ?? 0,
                AddInvoiceImg = x.AddUsers!.ImgUrl ?? "/distnew/images/userprofile.png",
                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts != null ? z.Accounts!.NameAr : "",
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts != null ? z.ToAccounts!.NameAr : "",
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate ?? "",
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    ServicesPriceId = z.ServicesPriceId,
                    ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                    ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                    IsRetrieve = z.IsRetrieve ?? 0,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,

                }).ToList()
            }).OrderByDescending(s => s.InvoiceNumber).ToList();


            if (!(voucherFilterVM!.dateFrom == null || voucherFilterVM!.dateFrom == "" || voucherFilterVM!.dateTo == null || voucherFilterVM!.dateTo == ""))
            {
                details=details.ToList().Where(s => (DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                                                      && (DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateTo == null)).ToList();
            }

            return details;
        }

         public async Task<InvoicesVM> GetVouchersSearchInvoiceByID(int InvoiceId, int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId== InvoiceId).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,

                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue ?? 0,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,
                InvUUID = x.InvUUID ?? "",

                CreditNotiId = x.CreditNotiId ?? 0,
                DepitNotiId = x.DepitNotiId ?? 0,
                AddInvoiceImg=x.AddUsers!.ImgUrl?? "/distnew/images/userprofile.png",
                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts != null ? z.Accounts!.NameAr : "",
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts != null ? z.ToAccounts!.NameAr : "",
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate ?? "",
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    ServicesPriceId = z.ServicesPriceId,
                    ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                    ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                    IsRetrieve = z.IsRetrieve ?? 0,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,

                }).ToList()
            }).FirstOrDefault();

            return details!;
        }
         public async Task<InvoicesVM> GetVouchersSearchInvoicePurchaseByID(int InvoiceId, int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == InvoiceId).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,

                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue ?? 0,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,
                InvUUID = x.InvUUID ?? "",

                AddInvoiceImg = x.AddUsers!.ImgUrl ?? "/distnew/images/userprofile.png",
                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts != null ? z.Accounts!.NameAr : "",
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts != null ? z.ToAccounts!.NameAr : "",
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate ?? "",
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    CategoryId = z.CategoryId,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,
                    CategoryName = z.Categories != null ? z.Categories.NAmeAr : "",
                    CategoryTypeName = z.Categories != null ? z.Categories.CategorType != null ? z.Categories.CategorType.NAmeAr ?? "" : "" : "",

                }).ToList()
            }).FirstOrDefault();

            return details!;
        }

       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersPurchase(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,

                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue ?? 0,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId??"000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,
                InvUUID = x.InvUUID ?? "",
                AddInvoiceImg = x.AddUsers!.ImgUrl ?? "/distnew/images/userprofile.png",
                DepitNotiId = x.DepitNotiId ?? 0,
                PurchaseOrderNo = x.PurchaseOrderNo ?? "",
                PurchaseOrderStatus = x.PurchaseOrderNo ==null || x.PurchaseOrderNo =="" ? "تحت المراجعه" : "تم التحويل الي فاتورة",

                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts != null ? z.Accounts!.NameAr : "",
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts != null ? z.ToAccounts!.NameAr : "",
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate ?? "",
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    CategoryId = z.CategoryId,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,
                    CategoryName = z.Categories != null ? z.Categories.NAmeAr : "",
                    CategoryTypeName=z.Categories!=null?z.Categories.CategorType!=null?z.Categories.CategorType.NAmeAr??"":"":"",

                }).ToList()
            }).OrderByDescending(s => s.InvoiceNumber).ToList();

            return details;
        }


       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersProject( int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 2 && s.YearId == YearId && s.BranchId == BranchId && s.IsPost == false && s.PageInsert == 3).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,
                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullName,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,
                InvUUID = x.InvUUID ?? "",

                CreditNotiId = x.CreditNotiId ?? 0,
                DepitNotiId = x.DepitNotiId ?? 0,

                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts!.NameAr,
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts!.NameAr,
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate,
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    ServicesPriceId = z.ServicesPriceId,
                    ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                    ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                    IsRetrieve = z.IsRetrieve ?? 0,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,

                }).ToList()
            }).OrderByDescending(s => s.InvoiceNumber).ToList();

            return details;
        }




       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersRet(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone:"",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    CreditNotiId = x.CreditNotiId ?? 0,
                    DepitNotiId = x.DepitNotiId ?? 0,
                    CustomerAccountId = x.Customer != null ? x.Customer.AccountId : 0,
                    Rad = x.Rad,
                    RadName = x.Rad == false ? "ساري" : "ملغي",
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    //DetailsDesc = x.VoucherDetails[0].Description,


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();


                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.AccountId)) && voucherFilterVM!.AccountId > 0)
                {
                    details = details.Where(s =>  s.CustomerAccountId == voucherFilterVM!.AccountId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile=x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        //DetailsDesc = x.VoucherDetails[0].Description,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName=z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }


                if (voucherFilterVM!.Status == 1 || voucherFilterVM!.Status == 2)
                {
                    var Va = true;
                    if (voucherFilterVM!.Status == 1)
                    {
                        Va = true;
                    }
                    else
                    {
                        Va = false;
                    }
                    details = details.Where(s => s.Rad == Va).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone ,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        //DetailsDesc = x.VoucherDetails[0].Description,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName=z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }


                if (voucherFilterVM!.IsSearch == true && voucherFilterVM!.dateFrom!=null && voucherFilterVM!.dateTo != null)
                {
                    details = details.Where(s => (DateTime.ParseExact(s.Date ??"2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                        && DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        ).Select(x => new InvoicesVM
                        {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",

                            Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",

                            VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                                TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName=z.ServiceTypeName,
                                IsRetrieve = z.IsRetrieve ?? 0,
                                DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                                DiscountValue_Det = z.DiscountValue_Det ?? 0,

                            }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }


                return details;

            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    Rad = x.Rad,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    //DetailsDesc = x.VoucherDetails[0].Description,


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;

            }

        }

       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersRetPurchase(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    CustomerAccountId = x.Customer != null ? x.Customer.AccountId : 0,
                    Rad = x.Rad,
                    RadName = x.Rad == false ? "ساري" : "ملغي",
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    //DetailsDesc = x.VoucherDetails[0].Description,


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        CategoryId = z.CategoryId,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,
                        CategoryName = z.Categories != null ? z.Categories.NAmeAr : "",
                        CategoryTypeName = z.Categories != null ? z.Categories.CategorType != null ? z.Categories.CategorType.NAmeAr ?? "" : "" : "",

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();


                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.AccountId)) && voucherFilterVM!.AccountId > 0)
                {
                    details = details.Where(s => s.CustomerAccountId == voucherFilterVM!.AccountId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        //DetailsDesc = x.VoucherDetails[0].Description,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            CategoryId = z.CategoryId,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,
                            CategoryName = z.CategoryName,
                            CategoryTypeName=z.CategoryTypeName,
                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }


                if (voucherFilterVM!.Status == 1 || voucherFilterVM!.Status == 2)
                {
                    var Va = true;
                    if (voucherFilterVM!.Status == 1)
                    {
                        Va = true;
                    }
                    else
                    {
                        Va = false;
                    }
                    details = details.Where(s => s.Rad == Va).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        //DetailsDesc = x.VoucherDetails[0].Description,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            CategoryId = z.CategoryId,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,
                            CategoryName = z.CategoryName,
                            CategoryTypeName=z.CategoryTypeName,
                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }


                if (voucherFilterVM!.IsSearch == true && voucherFilterVM!.dateFrom != null && voucherFilterVM!.dateTo != null)
                {
                    details = details.Where(s => (DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                        && DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        ).Select(x => new InvoicesVM
                        {
                            InvoiceNumber = x.InvoiceNumber,
                            InvoiceId = x.InvoiceId,
                            Type = x.Type,
                            IsPost = x.IsPost,
                            PostDate = x.PostDate,
                            PostHijriDate = x.PostHijriDate,
                            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            Date = x.Date,
                            HijriDate = x.HijriDate,
                            InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                            InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                            TaxAmount = x.TaxAmount,
                            PayType = x.PayType,
                            ProjectId = x.ProjectId,
                            ProjectNo = x.ProjectNo,
                            ToAccountId = x.ToAccountId,
                            InvoiceValueText = x.InvoiceValueText,
                            BranchId = x.BranchId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            CustomerName_W=x.CustomerName_W,
                            CustomerPhone = x.CustomerPhone,
                            CustomerMobile = x.CustomerMobile,
                            JournalNumber = x.JournalNumber,
                            AddDate = x.AddDate,
                            printBankAccount = x.printBankAccount,
                            DiscountPercentage = x.DiscountPercentage ?? 0,
                            DiscountValue = x.DiscountValue ?? 0,
                            StoreId = x.StoreId,
                            PaidValue = x.PaidValue,
                            ToInvoiceId = x.ToInvoiceId,
                            SupplierInvoiceNo = x.SupplierInvoiceNo,
                            RecevierTxt = x.RecevierTxt,
                            ClauseId = x.ClauseId,
                            SupplierId = x.SupplierId,
                            CostCenterId = x.CostCenterId,
                            PageInsert = x.PageInsert,
                            InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",

                            Rad = x.Rad,
                            RadName = x.Rad == false ? "ساري" : "ملغي",

                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",

                            VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                            {
                                VoucherDetailsId = z.VoucherDetailsId,
                                InvoiceId = z.InvoiceId,
                                LineNumber = z.LineNumber,
                                AccountId = z.AccountId,
                                PayType = z.PayType,
                                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                                TaxType = z.TaxType,
                                TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                                AccountName = z.AccountName,
                                Amount = z.Amount,
                                Qty = z.Qty ?? 1,
                                CostCenterId = z.CostCenterId,
                                TaxAmount = z.TaxAmount ?? 0,
                                CostCenterName = z.CostCenterName ?? "",
                                Description = z.Description,
                                ReferenceNumber = z.ReferenceNumber ?? "",
                                ToAccountId = z.ToAccountId,
                                ToAccountName = z.ToAccountName,
                                TotalAmount = z.TotalAmount ?? 0,

                                CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                                CheckDate = z.CheckDate,
                                BankName = z.BankName,
                                BankId = z.BankId,
                                MoneyOrderDate = z.MoneyOrderDate,
                                MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                                CategoryId = z.CategoryId,
                                DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                                DiscountValue_Det = z.DiscountValue_Det ?? 0,
                                CategoryName = z.CategoryName,
                                CategoryTypeName=z.CategoryTypeName,
                            }).ToList()
                        }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }


                return details;

            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    Rad = x.Rad,
                    InvoiceRetId = x.InvoiceRetId ??"000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    //DetailsDesc = x.VoucherDetails[0].Description,


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        CategoryId = z.CategoryId,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,
                        CategoryName = z.Categories != null ? z.Categories.NAmeAr : "",
                        CategoryTypeName = z.Categories != null ? z.Categories.CategorType != null ? z.Categories.CategorType.NAmeAr ?? "" : "" : "",

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;

            }

        }
       public async Task<IEnumerable<InvoicesVM>> GetAllCreditDepitNotiReport(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true  && (s.Type == 29 || s.Type == 30) && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,
                    
                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    CreditNotiId = x.CreditNotiId ?? 0,
                    DepitNotiId = x.DepitNotiId ?? 0,
                    CustomerAccountId = x.Customer != null ? x.Customer.AccountId : 0,
                    Rad = x.Rad,
                    RadName = x.Rad == false ? "ساري" : "ملغي",
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TransactionTypeName = x.AccTransactionTypes!.NameAr ?? "",

                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();


                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.CustomerId)) && voucherFilterVM!.CustomerId > 0)
                {
                    details = details.Where(s => s.CustomerId == voucherFilterVM!.CustomerId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W = x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TransactionTypeName = x.TransactionTypeName,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName = z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.ProjectId)) && voucherFilterVM!.ProjectId > 0)
                {
                    details = details.Where(s => s.ProjectId == voucherFilterVM!.ProjectId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W = x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TransactionTypeName = x.TransactionTypeName,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName = z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }
                if (voucherFilterVM!.IsSearch == true && voucherFilterVM!.dateFrom != null && voucherFilterVM!.dateTo != null)
                {
                    details = details.Where(s => (DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                        && DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        ).Select(x => new InvoicesVM
                        {
                            InvoiceNumber = x.InvoiceNumber,
                            InvoiceId = x.InvoiceId,
                            Type = x.Type,
                            IsPost = x.IsPost,
                            PostDate = x.PostDate,
                            PostHijriDate = x.PostHijriDate,
                            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            Date = x.Date,
                            HijriDate = x.HijriDate,
                            InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                            InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                            TaxAmount = x.TaxAmount,
                            PayType = x.PayType,
                            ProjectId = x.ProjectId,
                            ProjectNo = x.ProjectNo,
                            ToAccountId = x.ToAccountId,
                            InvoiceValueText = x.InvoiceValueText,
                            BranchId = x.BranchId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            CustomerName_W = x.CustomerName_W,
                            CustomerPhone = x.CustomerPhone,
                            CustomerMobile = x.CustomerMobile,
                            JournalNumber = x.JournalNumber,
                            AddDate = x.AddDate,
                            printBankAccount = x.printBankAccount,
                            DiscountPercentage = x.DiscountPercentage ?? 0,
                            DiscountValue = x.DiscountValue ?? 0,
                            StoreId = x.StoreId,
                            PaidValue = x.PaidValue,
                            ToInvoiceId = x.ToInvoiceId,
                            SupplierInvoiceNo = x.SupplierInvoiceNo,
                            RecevierTxt = x.RecevierTxt,
                            ClauseId = x.ClauseId,
                            SupplierId = x.SupplierId,
                            CostCenterId = x.CostCenterId,
                            PageInsert = x.PageInsert,
                            InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",

                            Rad = x.Rad,
                            RadName = x.Rad == false ? "ساري" : "ملغي",

                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TransactionTypeName = x.TransactionTypeName,

                            VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                            {
                                VoucherDetailsId = z.VoucherDetailsId,
                                InvoiceId = z.InvoiceId,
                                LineNumber = z.LineNumber,
                                AccountId = z.AccountId,
                                PayType = z.PayType,
                                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                                TaxType = z.TaxType,
                                TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                                AccountName = z.AccountName,
                                Amount = z.Amount,
                                Qty = z.Qty ?? 1,
                                CostCenterId = z.CostCenterId,
                                TaxAmount = z.TaxAmount ?? 0,
                                CostCenterName = z.CostCenterName ?? "",
                                Description = z.Description,
                                ReferenceNumber = z.ReferenceNumber ?? "",
                                ToAccountId = z.ToAccountId,
                                ToAccountName = z.ToAccountName,
                                TotalAmount = z.TotalAmount ?? 0,

                                CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                                CheckDate = z.CheckDate,
                                BankName = z.BankName,
                                BankId = z.BankId,
                                MoneyOrderDate = z.MoneyOrderDate,
                                MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                                ServicesPriceId = z.ServicesPriceId,
                                ServicesPriceName = z.ServicesPriceName,
                                ServiceTypeName = z.ServiceTypeName,
                                IsRetrieve = z.IsRetrieve ?? 0,
                                DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                                DiscountValue_Det = z.DiscountValue_Det ?? 0,

                            }).ToList()
                        }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }


                return details;

            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Rad == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    Rad = x.Rad,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TransactionTypeName = x.AccTransactionTypes!.NameAr ?? "",


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;

            }

        }
       public async Task<IEnumerable<InvoicesVM>> GetAllCreditDepitNotiReport_Pur(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && (s.Type == 32 || s.Type == 33) && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    //CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Suppliers != null ? x.Suppliers.NameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    CreditNotiId = x.CreditNotiId ?? 0,
                    DepitNotiId = x.DepitNotiId ?? 0,
                    CustomerAccountId = x.Customer != null ? x.Customer.AccountId : 0,
                    Rad = x.Rad,
                    RadName = x.Rad == false ? "ساري" : "ملغي",
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TransactionTypeName = x.AccTransactionTypes!.NameAr ?? "",

                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();


                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.CustomerId)) && voucherFilterVM!.CustomerId > 0)
                {
                    details = details.Where(s => s.CustomerId == voucherFilterVM!.CustomerId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W = x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TransactionTypeName = x.TransactionTypeName,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName = z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.ProjectId)) && voucherFilterVM!.ProjectId > 0)
                {
                    details = details.Where(s => s.ProjectId == voucherFilterVM!.ProjectId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W = x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TransactionTypeName = x.TransactionTypeName,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName = z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.SupplierId)) && voucherFilterVM!.SupplierId > 0)
                {
                    details = details.Where(s => s.SupplierId == voucherFilterVM!.SupplierId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W = x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TransactionTypeName = x.TransactionTypeName,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName = z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }
                if (voucherFilterVM!.IsSearch == true && voucherFilterVM!.dateFrom != null && voucherFilterVM!.dateTo != null)
                {
                    details = details.Where(s => (DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                        && DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        ).Select(x => new InvoicesVM
                        {
                            InvoiceNumber = x.InvoiceNumber,
                            InvoiceId = x.InvoiceId,
                            Type = x.Type,
                            IsPost = x.IsPost,
                            PostDate = x.PostDate,
                            PostHijriDate = x.PostHijriDate,
                            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            Date = x.Date,
                            HijriDate = x.HijriDate,
                            InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                            InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                            TaxAmount = x.TaxAmount,
                            PayType = x.PayType,
                            ProjectId = x.ProjectId,
                            ProjectNo = x.ProjectNo,
                            ToAccountId = x.ToAccountId,
                            InvoiceValueText = x.InvoiceValueText,
                            BranchId = x.BranchId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            CustomerName_W = x.CustomerName_W,
                            CustomerPhone = x.CustomerPhone,
                            CustomerMobile = x.CustomerMobile,
                            JournalNumber = x.JournalNumber,
                            AddDate = x.AddDate,
                            printBankAccount = x.printBankAccount,
                            DiscountPercentage = x.DiscountPercentage ?? 0,
                            DiscountValue = x.DiscountValue ?? 0,
                            StoreId = x.StoreId,
                            PaidValue = x.PaidValue,
                            ToInvoiceId = x.ToInvoiceId,
                            SupplierInvoiceNo = x.SupplierInvoiceNo,
                            RecevierTxt = x.RecevierTxt,
                            ClauseId = x.ClauseId,
                            SupplierId = x.SupplierId,
                            CostCenterId = x.CostCenterId,
                            PageInsert = x.PageInsert,
                            InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",

                            Rad = x.Rad,
                            RadName = x.Rad == false ? "ساري" : "ملغي",

                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TransactionTypeName = x.TransactionTypeName,

                            VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                            {
                                VoucherDetailsId = z.VoucherDetailsId,
                                InvoiceId = z.InvoiceId,
                                LineNumber = z.LineNumber,
                                AccountId = z.AccountId,
                                PayType = z.PayType,
                                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                                TaxType = z.TaxType,
                                TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                                AccountName = z.AccountName,
                                Amount = z.Amount,
                                Qty = z.Qty ?? 1,
                                CostCenterId = z.CostCenterId,
                                TaxAmount = z.TaxAmount ?? 0,
                                CostCenterName = z.CostCenterName ?? "",
                                Description = z.Description,
                                ReferenceNumber = z.ReferenceNumber ?? "",
                                ToAccountId = z.ToAccountId,
                                ToAccountName = z.ToAccountName,
                                TotalAmount = z.TotalAmount ?? 0,

                                CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                                CheckDate = z.CheckDate,
                                BankName = z.BankName,
                                BankId = z.BankId,
                                MoneyOrderDate = z.MoneyOrderDate,
                                MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                                ServicesPriceId = z.ServicesPriceId,
                                ServicesPriceName = z.ServicesPriceName,
                                ServiceTypeName = z.ServiceTypeName,
                                IsRetrieve = z.IsRetrieve ?? 0,
                                DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                                DiscountValue_Det = z.DiscountValue_Det ?? 0,

                            }).ToList()
                        }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }


                return details;

            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Rad == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    Rad = x.Rad,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TransactionTypeName = x.AccTransactionTypes!.NameAr ?? "",


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;

            }

        }

       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersRetReport(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Rad==true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    CreditNotiId = x.CreditNotiId ?? 0,
                    DepitNotiId = x.DepitNotiId ?? 0,
                    CustomerAccountId = x.Customer != null ? x.Customer.AccountId : 0,
                    Rad = x.Rad,
                    RadName = x.Rad == false ? "ساري" : "ملغي",
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    //DetailsDesc = x.VoucherDetails[0].Description,


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();


                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.CustomerId)) && voucherFilterVM!.CustomerId > 0)
                {
                    details = details.Where(s => s.CustomerId == voucherFilterVM!.CustomerId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        //DetailsDesc = x.VoucherDetails[0].Description,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName=z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.ProjectId)) && voucherFilterVM!.ProjectId > 0)
                {
                    details = details.Where(s => s.ProjectId == voucherFilterVM!.ProjectId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        //DetailsDesc = x.VoucherDetails[0].Description,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName=z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }
                if (voucherFilterVM!.IsSearch == true && voucherFilterVM!.dateFrom != null && voucherFilterVM!.dateTo != null)
                {
                    details = details.Where(s => (DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                        && DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        ).Select(x => new InvoicesVM
                        {
                            InvoiceNumber = x.InvoiceNumber,
                            InvoiceId = x.InvoiceId,
                            Type = x.Type,
                            IsPost = x.IsPost,
                            PostDate = x.PostDate,
                            PostHijriDate = x.PostHijriDate,
                            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            Date = x.Date,
                            HijriDate = x.HijriDate,
                            InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                            InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                            TaxAmount = x.TaxAmount,
                            PayType = x.PayType,
                            ProjectId = x.ProjectId,
                            ProjectNo = x.ProjectNo,
                            ToAccountId = x.ToAccountId,
                            InvoiceValueText = x.InvoiceValueText,
                            BranchId = x.BranchId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            CustomerName_W=x.CustomerName_W,
                            CustomerPhone = x.CustomerPhone,
                            CustomerMobile = x.CustomerMobile,
                            JournalNumber = x.JournalNumber,
                            AddDate = x.AddDate,
                            printBankAccount = x.printBankAccount,
                            DiscountPercentage = x.DiscountPercentage ?? 0,
                            DiscountValue = x.DiscountValue ?? 0,
                            StoreId = x.StoreId,
                            PaidValue = x.PaidValue,
                            ToInvoiceId = x.ToInvoiceId,
                            SupplierInvoiceNo = x.SupplierInvoiceNo,
                            RecevierTxt = x.RecevierTxt,
                            ClauseId = x.ClauseId,
                            SupplierId = x.SupplierId,
                            CostCenterId = x.CostCenterId,
                            PageInsert = x.PageInsert,
                            InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",

                            Rad = x.Rad,
                            RadName = x.Rad == false ? "ساري" : "ملغي",

                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",

                            VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                            {
                                VoucherDetailsId = z.VoucherDetailsId,
                                InvoiceId = z.InvoiceId,
                                LineNumber = z.LineNumber,
                                AccountId = z.AccountId,
                                PayType = z.PayType,
                                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                                TaxType = z.TaxType,
                                TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                                AccountName = z.AccountName,
                                Amount = z.Amount,
                                Qty = z.Qty ?? 1,
                                CostCenterId = z.CostCenterId,
                                TaxAmount = z.TaxAmount ?? 0,
                                CostCenterName = z.CostCenterName ?? "",
                                Description = z.Description,
                                ReferenceNumber = z.ReferenceNumber ?? "",
                                ToAccountId = z.ToAccountId,
                                ToAccountName = z.ToAccountName,
                                TotalAmount = z.TotalAmount ?? 0,

                                CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                                CheckDate = z.CheckDate,
                                BankName = z.BankName,
                                BankId = z.BankId,
                                MoneyOrderDate = z.MoneyOrderDate,
                                MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                                ServicesPriceId = z.ServicesPriceId,
                                ServicesPriceName = z.ServicesPriceName,
                                ServiceTypeName=z.ServiceTypeName,
                                IsRetrieve = z.IsRetrieve ?? 0,
                                DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                                DiscountValue_Det = z.DiscountValue_Det ?? 0,

                            }).ToList()
                        }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }


                return details;

            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Rad == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    Rad = x.Rad,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    //DetailsDesc = x.VoucherDetails[0].Description,


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;

            }

        }

       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersRetReport_Pur(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Rad == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    CustomerAccountId = x.Customer != null ? x.Customer.AccountId : 0,
                    Rad = x.Rad,
                    RadName = x.Rad == false ? "ساري" : "ملغي",
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    //DetailsDesc = x.VoucherDetails[0].Description,


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();


                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.SupplierId)) && voucherFilterVM!.SupplierId > 0)
                {

                    details = details.Where(s => s.SupplierId == voucherFilterVM!.SupplierId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Rad = x.Rad,
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.InvoiceValue ?? 0,

                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue ?? 0,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        PayTypeName = x.PayTypeName,
                        FileName = x.FileName,
                        FileUrl = x.FileUrl,
                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        VoucherDetails = x.VoucherDetails,
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }
                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.ProjectId)) && voucherFilterVM!.ProjectId > 0)
                {
                    details = details.Where(s => s.ProjectId == voucherFilterVM!.ProjectId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        //DetailsDesc = x.VoucherDetails[0].Description,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName=z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }
                if (voucherFilterVM!.IsSearch == true && voucherFilterVM!.dateFrom != null && voucherFilterVM!.dateTo != null)
                {
                    details = details.Where(s => (DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                        && DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        ).Select(x => new InvoicesVM
                        {
                            InvoiceNumber = x.InvoiceNumber,
                            InvoiceId = x.InvoiceId,
                            Type = x.Type,
                            IsPost = x.IsPost,
                            PostDate = x.PostDate,
                            PostHijriDate = x.PostHijriDate,
                            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            Date = x.Date,
                            HijriDate = x.HijriDate,
                            InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                            InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                            TaxAmount = x.TaxAmount,
                            PayType = x.PayType,
                            ProjectId = x.ProjectId,
                            ProjectNo = x.ProjectNo,
                            ToAccountId = x.ToAccountId,
                            InvoiceValueText = x.InvoiceValueText,
                            BranchId = x.BranchId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            CustomerName_W=x.CustomerName_W,
                            CustomerPhone = x.CustomerPhone,
                            CustomerMobile = x.CustomerMobile,
                            JournalNumber = x.JournalNumber,
                            AddDate = x.AddDate,
                            printBankAccount = x.printBankAccount,
                            DiscountPercentage = x.DiscountPercentage ?? 0,
                            DiscountValue = x.DiscountValue ?? 0,
                            StoreId = x.StoreId,
                            PaidValue = x.PaidValue,
                            ToInvoiceId = x.ToInvoiceId,
                            SupplierInvoiceNo = x.SupplierInvoiceNo,
                            RecevierTxt = x.RecevierTxt,
                            ClauseId = x.ClauseId,
                            SupplierId = x.SupplierId,
                            CostCenterId = x.CostCenterId,
                            PageInsert = x.PageInsert,
                            InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",

                            Rad = x.Rad,
                            RadName = x.Rad == false ? "ساري" : "ملغي",

                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",

                            VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                            {
                                VoucherDetailsId = z.VoucherDetailsId,
                                InvoiceId = z.InvoiceId,
                                LineNumber = z.LineNumber,
                                AccountId = z.AccountId,
                                PayType = z.PayType,
                                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                                TaxType = z.TaxType,
                                TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                                AccountName = z.AccountName,
                                Amount = z.Amount,
                                Qty = z.Qty ?? 1,
                                CostCenterId = z.CostCenterId,
                                TaxAmount = z.TaxAmount ?? 0,
                                CostCenterName = z.CostCenterName ?? "",
                                Description = z.Description,
                                ReferenceNumber = z.ReferenceNumber ?? "",
                                ToAccountId = z.ToAccountId,
                                ToAccountName = z.ToAccountName,
                                TotalAmount = z.TotalAmount ?? 0,

                                CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                                CheckDate = z.CheckDate,
                                BankName = z.BankName,
                                BankId = z.BankId,
                                MoneyOrderDate = z.MoneyOrderDate,
                                MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                                ServicesPriceId = z.ServicesPriceId,
                                ServicesPriceName = z.ServicesPriceName,
                                ServiceTypeName=z.ServiceTypeName,
                                IsRetrieve = z.IsRetrieve ?? 0,
                                DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                                DiscountValue_Det = z.DiscountValue_Det ?? 0,

                            }).ToList()
                        }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }


                return details;

            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Rad == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",

                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",
                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    Rad = x.Rad,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    //DetailsDesc = x.VoucherDetails[0].Description,


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;

            }

        }

       public async Task<IEnumerable<InvoicesVM>> GetAllPayVouchersRet(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? ((x.TotalValue??0) - (x.TaxAmount??0)) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount??0,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    CustomerAccountId = x.Customer != null ? x.Customer.AccountId : 0,
                    Rad = x.Rad,
                    RadName = x.Rad == false ? "ساري" : "ملغي",
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    //DetailsDesc = x.VoucherDetails[0].Description,
                    AccountNameRet = x.Accounts==null? "":x.Accounts.NameAr,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    CreditNotiId = x.CreditNotiId ?? 0,
                    DepitNotiId = x.DepitNotiId ?? 0,
                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();


                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.AccountId)) && voucherFilterVM!.AccountId > 0)
                {
                    details = details.Where(s => s.ToAccountId == voucherFilterVM!.AccountId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,
                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        //DetailsDesc = x.VoucherDetails[0].Description,
                        AccountNameRet = x.AccountNameRet,


                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName=z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }


                if (voucherFilterVM!.Status == 1 || voucherFilterVM!.Status == 2)
                {
                    var Va = true;
                    if (voucherFilterVM!.Status == 1)
                    {
                        Va = true;
                    }
                    else
                    {
                        Va = false;
                    }
                    details = details.Where(s => s.Rad == Va).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",

                        Rad = x.Rad,
                        RadName = x.Rad == false ? "ساري" : "ملغي",

                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        //DetailsDesc = x.VoucherDetails[0].Description,
                        AccountNameRet = x.AccountNameRet,

                        VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                        {
                            VoucherDetailsId = z.VoucherDetailsId,
                            InvoiceId = z.InvoiceId,
                            LineNumber = z.LineNumber,
                            AccountId = z.AccountId,
                            PayType = z.PayType,
                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            TaxType = z.TaxType,
                            TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                            AccountName = z.AccountName,
                            Amount = z.Amount,
                            Qty = z.Qty ?? 1,
                            CostCenterId = z.CostCenterId,
                            TaxAmount = z.TaxAmount ?? 0,
                            CostCenterName = z.CostCenterName ?? "",
                            Description = z.Description,
                            ReferenceNumber = z.ReferenceNumber ?? "",
                            ToAccountId = z.ToAccountId,
                            ToAccountName = z.ToAccountName,
                            TotalAmount = z.TotalAmount ?? 0,

                            CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                            CheckDate = z.CheckDate,
                            BankName = z.BankName,
                            BankId = z.BankId,
                            MoneyOrderDate = z.MoneyOrderDate,
                            MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                            ServicesPriceId = z.ServicesPriceId,
                            ServicesPriceName = z.ServicesPriceName,
                            ServiceTypeName=z.ServiceTypeName,
                            IsRetrieve = z.IsRetrieve ?? 0,
                            DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                            DiscountValue_Det = z.DiscountValue_Det ?? 0,

                        }).ToList()
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }


                if (voucherFilterVM!.IsSearch == true && voucherFilterVM!.dateFrom != null && voucherFilterVM!.dateTo != null)
                {
                    details = details.Where(s => (DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                        && DateTime.ParseExact(s.Date ?? "2040-10-10", "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        ).Select(x => new InvoicesVM
                        {
                            InvoiceNumber = x.InvoiceNumber,
                            InvoiceId = x.InvoiceId,
                            Type = x.Type,
                            IsPost = x.IsPost,
                            PostDate = x.PostDate,
                            PostHijriDate = x.PostHijriDate,
                            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            Date = x.Date,
                            HijriDate = x.HijriDate,
                            InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                            InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                            TaxAmount = x.TaxAmount,
                            PayType = x.PayType,
                            ProjectId = x.ProjectId,
                            ProjectNo = x.ProjectNo,
                            ToAccountId = x.ToAccountId,
                            InvoiceValueText = x.InvoiceValueText,
                            BranchId = x.BranchId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            CustomerName_W=x.CustomerName_W,
                            CustomerPhone = x.CustomerPhone,
                            CustomerMobile = x.CustomerMobile,
                            JournalNumber = x.JournalNumber,
                            AddDate = x.AddDate,
                            printBankAccount = x.printBankAccount,
                            DiscountPercentage = x.DiscountPercentage ?? 0,
                            DiscountValue = x.DiscountValue ?? 0,
                            StoreId = x.StoreId,
                            PaidValue = x.PaidValue,
                            ToInvoiceId = x.ToInvoiceId,
                            SupplierInvoiceNo = x.SupplierInvoiceNo,
                            RecevierTxt = x.RecevierTxt,
                            ClauseId = x.ClauseId,
                            SupplierId = x.SupplierId,
                            CostCenterId = x.CostCenterId,
                            PageInsert = x.PageInsert,
                            Rad = x.Rad,
                            RadName = x.Rad == false ? "ساري" : "ملغي",
                            InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",

                            PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                            AccountNameRet = x.AccountNameRet,

                            VoucherDetails = x.VoucherDetails/*.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false)*/.Select(z => new VoucherDetailsVM
                            {
                                VoucherDetailsId = z.VoucherDetailsId,
                                InvoiceId = z.InvoiceId,
                                LineNumber = z.LineNumber,
                                AccountId = z.AccountId,
                                PayType = z.PayType,
                                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                                TaxType = z.TaxType,
                                TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                                AccountName = z.AccountName,
                                Amount = z.Amount,
                                Qty = z.Qty ?? 1,
                                CostCenterId = z.CostCenterId,
                                TaxAmount = z.TaxAmount ?? 0,
                                CostCenterName = z.CostCenterName ?? "",
                                Description = z.Description,
                                ReferenceNumber = z.ReferenceNumber ?? "",
                                ToAccountId = z.ToAccountId,
                                ToAccountName = z.ToAccountName,
                                TotalAmount = z.TotalAmount ?? 0,

                                CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                                CheckDate = z.CheckDate,
                                BankName = z.BankName,
                                BankId = z.BankId,
                                MoneyOrderDate = z.MoneyOrderDate,
                                MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                                ServicesPriceId = z.ServicesPriceId,
                                ServicesPriceName = z.ServicesPriceName,
                                ServiceTypeName=z.ServiceTypeName,
                                IsRetrieve = z.IsRetrieve ?? 0,
                                DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                                DiscountValue_Det = z.DiscountValue_Det ?? 0,

                            }).ToList()
                        }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }


                return details;

            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.IsTax == false ? (x.TotalValue - x.TaxAmount) : x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    Rad = x.Rad,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    //DetailsDesc = x.VoucherDetails[0].Description,
                    AccountNameRet = x.Accounts == null ? "" : x.Accounts.NameAr,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;

            }

        }

       public async Task<IEnumerable<InvoicesVM>> GetVoucherRpt(int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.YearId == YearId && s.BranchId == BranchId && (s.Rad != true) && s.Type !=35).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate??"",
                PostHijriDate = x.PostHijriDate??"",
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Date = x.Date??"",
                HijriDate = x.HijriDate??"",
                InvoiceReference = x.InvoiceReference??"",
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue,
                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ToInvoiceId = x.ToInvoiceId,

                TransactionTypeName = x.AccTransactionTypes!.NameAr??"",
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                TotalExpenses = 0,
                TotalRevenue = 0,
            }).ToList().OrderBy(s => s.InvoiceNumber);

            return details;
        }
       public async Task<IEnumerable<InvoicesVM>> GetCustRevenueExpensesDetails(string FromDate, string ToDate, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.YearId == YearId && s.BranchId == BranchId && s.CustomerId != null).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    TransactionTypeName = x.AccTransactionTypes!.NameAr,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    ProjectName = x.Project!.ProjectDescription,
                    projecttypeName = x.Project!.projecttype!.NameAr,
                    TotalExpenses = 0,
                    TotalRevenue = x.TransactionDetails!.Where(I => I.InvoiceId == x.InvoiceId).Sum(t => t.Depit) ?? 0,
                }).ToList().Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();

                return details;
            }
            catch (Exception)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.YearId == YearId && s.BranchId == BranchId && s.CustomerId != null).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    TransactionTypeName = x.AccTransactionTypes!.NameAr,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    ProjectName = x.Project!.ProjectDescription,
                    projecttypeName = x.Project!.projecttype!.NameAr,
                    TotalExpenses = 0,
                    TotalRevenue = x.TransactionDetails!.Where(I => I.InvoiceId == x.InvoiceId).Sum(t => t.Depit) ?? 0,
                }).ToList();
                return details;
            }
        }
         public async Task<Invoices> MaxVoucherP(int BranchId, int? YearId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.OrderByDescending(i => i.InvoiceId).Where(s => s.IsDeleted == false && s.YearId == YearId && s.BranchId == BranchId).FirstOrDefault();

                return details!;
            }
            catch (Exception ex)
            {
                Invoices Inv = new Invoices();
                return Inv;
            }
        }

       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersBySearch(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Rad = x.Rad,
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue ?? 0,

                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue ?? 0,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : (x.PayType == 15) ? "عهد موظفين" : (x.PayType == 16) ? "جاري المالك" : "",
                    FileName = x.FileName,
                    FileUrl = x.FileUrl,
                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    CreditNotiId = x.CreditNotiId ?? 0,
                    DepitNotiId = x.DepitNotiId ?? 0,
                    AddInvoiceImg=x.AddUsers!.ImgUrl?? "/distnew/images/userprofile.png",
                    DelegateId=x.DelegateId,
                    InvoicesRequests = x.InvoicesRequests!.Where(s => s.InvoiceId == x.InvoiceId).Select(w => new Acc_InvoicesRequestsVM
                    {
                        InvoiceReqId = w.InvoiceReqId,
                        InvoiceId = w.InvoiceId,
                        InvoiceNoRequest = w.InvoiceNoRequest,
                        IsSent = w.IsSent,
                        StatusCode = w.StatusCode,
                        SendingStatus = w.SendingStatus,
                        warningmessage = w.warningmessage,
                        ClearedInvoice = w.ClearedInvoice,
                        errormessage = w.errormessage,
                        PIH = w.PIH,
                        BranchId = w.BranchId,
                    }).ToList(),
                    //CreditDepitNotiTotal = x.InvoicesNoti.Where(s => s.CreditDepitNotiId != null && s.InvoicesNoti.Any(a => a.IsDeleted == false)).Sum(q => q.TotalValue),
                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,



                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();



                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.VoucherNo)) && voucherFilterVM!.VoucherNo > 0)
                {

                    details = details.Where(s => s.InvoiceNumber.Contains(voucherFilterVM!.VoucherNo.ToString())).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Rad = x.Rad,
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.InvoiceValue ?? 0,

                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue ?? 0,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        PayTypeName = x.PayTypeName,
                        FileName = x.FileName,
                        FileUrl = x.FileUrl,
                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",
                        AddInvoiceImg = x.AddInvoiceImg,
                        InvoicesRequests=x.InvoicesRequests,
                        VoucherDetails = x.VoucherDetails,
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }


                ///////////////
                if (voucherFilterVM!.IsChecked == true)
                {


                    details = details.Where(s => (DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                    && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Rad = x.Rad,
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.InvoiceValue ?? 0,

                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue ?? 0,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        PayTypeName = x.PayTypeName,
                        FileName = x.FileName,
                        FileUrl = x.FileUrl,
                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",
                        AddInvoiceImg = x.AddInvoiceImg,
                        InvoicesRequests=x.InvoicesRequests,
                        VoucherDetails = x.VoucherDetails,
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }

                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.IsPost)) && voucherFilterVM!.IsPost > 0)
                {
                    if (voucherFilterVM!.IsPost == 3)
                    {
                        details = details.Where(s => s.Rad == true).Select(x => new InvoicesVM
                        {
                            InvoiceNumber = x.InvoiceNumber,
                            InvoiceId = x.InvoiceId,
                            Type = x.Type,
                            IsPost = x.IsPost,
                            PostDate = x.PostDate,
                            PostHijriDate = x.PostHijriDate,
                            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            Rad = x.Rad,
                            Date = x.Date,
                            HijriDate = x.HijriDate,
                            InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                            InvoiceValue = x.InvoiceValue ?? 0,

                            TaxAmount = x.TaxAmount,
                            PayType = x.PayType,
                            ProjectId = x.ProjectId,
                            ProjectNo = x.ProjectNo,
                            ToAccountId = x.ToAccountId,
                            InvoiceValueText = x.InvoiceValueText,
                            BranchId = x.BranchId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            CustomerName_W=x.CustomerName_W,
                            CustomerPhone = x.CustomerPhone,
                            CustomerMobile = x.CustomerMobile,

                            JournalNumber = x.JournalNumber,
                            AddUser = x.AddUser,
                            AddDate = x.AddDate,
                            printBankAccount = x.printBankAccount,
                            DiscountPercentage = x.DiscountPercentage ?? 0,
                            DiscountValue = x.DiscountValue ?? 0,
                            StoreId = x.StoreId,
                            PaidValue = x.PaidValue ?? 0,
                            ToInvoiceId = x.ToInvoiceId,
                            SupplierInvoiceNo = x.SupplierInvoiceNo,
                            RecevierTxt = x.RecevierTxt,
                            ClauseId = x.ClauseId,
                            SupplierId = x.SupplierId,
                            CostCenterId = x.CostCenterId,
                            PageInsert = x.PageInsert,
                            InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            PayTypeName = x.PayTypeName,
                            FileName = x.FileName,
                            FileUrl = x.FileUrl,
                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",
                            InvoicesRequests=x.InvoicesRequests,
                            VoucherDetails = x.VoucherDetails,
                        }).OrderByDescending(s => s.InvoiceNumber).ToList();

                    }
                    else
                    {
                        bool Ispost = (voucherFilterVM!.IsPost == 1) ? true : false;


                        details = details.Where(s => s.IsPost == Ispost).Select(x => new InvoicesVM
                        {
                            InvoiceNumber = x.InvoiceNumber,
                            InvoiceId = x.InvoiceId,
                            Type = x.Type,
                            IsPost = x.IsPost,
                            PostDate = x.PostDate,
                            PostHijriDate = x.PostHijriDate,
                            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            Rad = x.Rad,
                            Date = x.Date,
                            HijriDate = x.HijriDate,
                            InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                            InvoiceValue = x.InvoiceValue ?? 0,

                            TaxAmount = x.TaxAmount,
                            PayType = x.PayType,
                            ProjectId = x.ProjectId,
                            ProjectNo = x.ProjectNo,
                            ToAccountId = x.ToAccountId,
                            InvoiceValueText = x.InvoiceValueText,
                            BranchId = x.BranchId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            CustomerName_W=x.CustomerName_W,
                            CustomerPhone = x.CustomerPhone,
                            CustomerMobile = x.CustomerMobile,

                            JournalNumber = x.JournalNumber,
                            AddUser = x.AddUser,
                            AddDate = x.AddDate,
                            printBankAccount = x.printBankAccount,
                            DiscountPercentage = x.DiscountPercentage ?? 0,
                            DiscountValue = x.DiscountValue ?? 0,
                            StoreId = x.StoreId,
                            PaidValue = x.PaidValue ?? 0,
                            ToInvoiceId = x.ToInvoiceId,
                            SupplierInvoiceNo = x.SupplierInvoiceNo,
                            RecevierTxt = x.RecevierTxt,
                            ClauseId = x.ClauseId,
                            SupplierId = x.SupplierId,
                            CostCenterId = x.CostCenterId,
                            PageInsert = x.PageInsert,
                            InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            PayTypeName = x.PayTypeName,
                            FileName = x.FileName,
                            FileUrl = x.FileUrl,
                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",
                            AddInvoiceImg = x.AddInvoiceImg,
                            VoucherDetails = x.VoucherDetails,
                            InvoicesRequests=x.InvoicesRequests,
                        }).OrderByDescending(s => s.InvoiceNumber).ToList();
                    }

                }
                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.ProjectId)) && voucherFilterVM!.ProjectId > 0)
                {

                    details = details.Where(s => s.ProjectId == voucherFilterVM!.ProjectId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Rad = x.Rad,
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.InvoiceValue ?? 0,

                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue ?? 0,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        PayTypeName = x.PayTypeName,
                        FileName = x.FileName,
                        FileUrl = x.FileUrl,
                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",
                        AddInvoiceImg = x.AddInvoiceImg,
                        VoucherDetails = x.VoucherDetails,
                        InvoicesRequests=x.InvoicesRequests,
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.CustomerId)) && voucherFilterVM!.CustomerId > 0)
                {

                    details = details.Where(s => s.CustomerId == voucherFilterVM!.CustomerId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Rad = x.Rad,
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.InvoiceValue ?? 0,

                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue ?? 0,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        PayTypeName = x.PayTypeName,
                        FileName = x.FileName,
                        FileUrl = x.FileUrl,
                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",
                        AddInvoiceImg = x.AddInvoiceImg,
                        VoucherDetails = x.VoucherDetails,
                        InvoicesRequests=x.InvoicesRequests,
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                 
                }

                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.InvoiceNote)) && voucherFilterVM!.InvoiceNote !="")
                {

                    details = details.Where(s => s.InvoiceNotes!.Contains(voucherFilterVM!.InvoiceNote)).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Rad = x.Rad,
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.InvoiceValue ?? 0,

                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W = x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue ?? 0,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        PayTypeName = x.PayTypeName,
                        FileName = x.FileName,
                        FileUrl = x.FileUrl,
                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",
                        AddInvoiceImg = x.AddInvoiceImg,
                        VoucherDetails = x.VoucherDetails,
                        InvoicesRequests=x.InvoicesRequests,
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }

                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Rad = x.Rad,
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue ?? 0,

                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue ?? 0,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    FileName = x.FileName,
                    FileUrl = x.FileUrl,
                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",
                    AddInvoiceImg = x.AddUsers!.ImgUrl ?? "/distnew/images/userprofile.png",
                    InvoicesRequests = x.InvoicesRequests!.Where(s => s.InvoiceId == x.InvoiceId).Select(w => new Acc_InvoicesRequestsVM
                    {
                        InvoiceReqId = w.InvoiceReqId,
                        InvoiceId = w.InvoiceId,
                        InvoiceNoRequest = w.InvoiceNoRequest,
                        IsSent = w.IsSent,
                        StatusCode = w.StatusCode,
                        SendingStatus = w.SendingStatus,
                        warningmessage = w.warningmessage,
                        ClearedInvoice = w.ClearedInvoice,
                        errormessage = w.errormessage,
                        PIH = w.PIH,
                        BranchId = w.BranchId,
                    }).ToList(),
                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        ServicesPriceId = z.ServicesPriceId,
                        ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                        ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = z.IsRetrieve ?? 0,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();

                return details;
            }

        }
       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersBySearchPurchase(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Rad = x.Rad,
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue ?? 0,

                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue ?? 0,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    FileName = x.FileName,
                    FileUrl = x.FileUrl,
                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",
                    AddInvoiceImg = x.AddUsers!.ImgUrl ?? "/distnew/images/userprofile.png",
                    DepitNotiId = x.DepitNotiId ?? 0,
                    PurchaseOrderNo=x.PurchaseOrderNo??"",
                    PurchaseOrderStatus = x.PurchaseOrderNo == null || x.PurchaseOrderNo == "" ? "تحت المراجعه" : "تم التحويل الي فاتورة",


                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        CategoryId = z.CategoryId,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,
                        CategoryName = z.Categories != null ? z.Categories.NAmeAr : "",
                        CategoryTypeName = z.Categories != null ? z.Categories.CategorType != null ? z.Categories.CategorType.NAmeAr ?? "" : "" : "",


                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();



                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.VoucherNo)) && voucherFilterVM!.VoucherNo > 0)
                {

                    details = details.Where(s => s.InvoiceNumber == voucherFilterVM!.VoucherNo.ToString()).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Rad = x.Rad,
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.InvoiceValue ?? 0,

                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue ?? 0,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        PayTypeName = x.PayTypeName,
                        FileName = x.FileName,
                        FileUrl = x.FileUrl,
                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",
                        AddInvoiceImg = x.AddInvoiceImg ?? "/distnew/images/userprofile.png",
                        VoucherDetails = x.VoucherDetails,
                        PurchaseOrderNo = x.PurchaseOrderNo ?? "",
                        PurchaseOrderStatus = x.PurchaseOrderNo == null || x.PurchaseOrderNo == "" ? "تحت المراجعه": "تم التحويل الي فاتورة"  ,
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }


                ///////////////
                if (voucherFilterVM!.IsChecked == true)
                {


                    details = details.Where(s => (DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                    && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Rad = x.Rad,
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.InvoiceValue ?? 0,

                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerName_W=x.CustomerName_W,
                        CustomerPhone = x.CustomerPhone,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue ?? 0,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        PayTypeName = x.PayTypeName,
                        FileName = x.FileName,
                        FileUrl = x.FileUrl,
                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",
                        AddInvoiceImg = x.AddInvoiceImg ?? "/distnew/images/userprofile.png",
                        VoucherDetails = x.VoucherDetails,
                        PurchaseOrderNo = x.PurchaseOrderNo ?? "",
                        PurchaseOrderStatus = x.PurchaseOrderNo == null || x.PurchaseOrderNo == "" ? "تحت المراجعه": "تم التحويل الي فاتورة"  ,
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();
                }

                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.IsPost)) && voucherFilterVM!.IsPost > 0)
                {
                    if (voucherFilterVM!.IsPost == 3)
                    {
                        details = details.Where(s => s.Rad == true).Select(x => new InvoicesVM
                        {
                            InvoiceNumber = x.InvoiceNumber,
                            InvoiceId = x.InvoiceId,
                            Type = x.Type,
                            IsPost = x.IsPost,
                            PostDate = x.PostDate,
                            PostHijriDate = x.PostHijriDate,
                            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            Rad = x.Rad,
                            Date = x.Date,
                            HijriDate = x.HijriDate,
                            InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                            InvoiceValue = x.InvoiceValue ?? 0,

                            TaxAmount = x.TaxAmount,
                            PayType = x.PayType,
                            ProjectId = x.ProjectId,
                            ProjectNo = x.ProjectNo,
                            ToAccountId = x.ToAccountId,
                            InvoiceValueText = x.InvoiceValueText,
                            BranchId = x.BranchId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            CustomerName_W=x.CustomerName_W,
                            CustomerPhone = x.CustomerPhone,
                            CustomerMobile = x.CustomerMobile,

                            JournalNumber = x.JournalNumber,
                            AddUser = x.AddUser,
                            AddDate = x.AddDate,
                            printBankAccount = x.printBankAccount,
                            DiscountPercentage = x.DiscountPercentage ?? 0,
                            DiscountValue = x.DiscountValue ?? 0,
                            StoreId = x.StoreId,
                            PaidValue = x.PaidValue ?? 0,
                            ToInvoiceId = x.ToInvoiceId,
                            SupplierInvoiceNo = x.SupplierInvoiceNo,
                            RecevierTxt = x.RecevierTxt,
                            ClauseId = x.ClauseId,
                            SupplierId = x.SupplierId,
                            CostCenterId = x.CostCenterId,
                            PageInsert = x.PageInsert,
                            InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            PayTypeName = x.PayTypeName,
                            FileName = x.FileName,
                            FileUrl = x.FileUrl,
                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",
                            AddInvoiceImg = x.AddInvoiceImg ?? "/distnew/images/userprofile.png",

                            VoucherDetails = x.VoucherDetails,
                            PurchaseOrderNo = x.PurchaseOrderNo ?? "",
                            PurchaseOrderStatus = x.PurchaseOrderNo == null || x.PurchaseOrderNo == "" ? "تحت المراجعه": "تم التحويل الي فاتورة"  ,
                        }).OrderByDescending(s => s.InvoiceNumber).ToList();

                    }
                    else
                    {
                        bool Ispost = (voucherFilterVM!.IsPost == 1) ? true : false;


                        details = details.Where(s => s.IsPost == Ispost).Select(x => new InvoicesVM
                        {
                            InvoiceNumber = x.InvoiceNumber,
                            InvoiceId = x.InvoiceId,
                            Type = x.Type,
                            IsPost = x.IsPost,
                            PostDate = x.PostDate,
                            PostHijriDate = x.PostHijriDate,
                            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                            Rad = x.Rad,
                            Date = x.Date,
                            HijriDate = x.HijriDate,
                            InvoiceReference = x.InvoiceReference,
                            Notes = x.Notes ?? "",
                            InvoiceNotes = x.InvoiceNotes ?? "",
                            TotalValue = x.TotalValue,
                            InvoiceValue = x.InvoiceValue ?? 0,

                            TaxAmount = x.TaxAmount,
                            PayType = x.PayType,
                            ProjectId = x.ProjectId,
                            ProjectNo = x.ProjectNo,
                            ToAccountId = x.ToAccountId,
                            InvoiceValueText = x.InvoiceValueText,
                            BranchId = x.BranchId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            CustomerName_W=x.CustomerName_W,
                            CustomerPhone = x.CustomerPhone,
                            CustomerMobile = x.CustomerMobile,

                            JournalNumber = x.JournalNumber,
                            AddUser = x.AddUser,
                            AddDate = x.AddDate,
                            printBankAccount = x.printBankAccount,
                            DiscountPercentage = x.DiscountPercentage ?? 0,
                            DiscountValue = x.DiscountValue ?? 0,
                            StoreId = x.StoreId,
                            PaidValue = x.PaidValue ?? 0,
                            ToInvoiceId = x.ToInvoiceId,
                            SupplierInvoiceNo = x.SupplierInvoiceNo,
                            RecevierTxt = x.RecevierTxt,
                            ClauseId = x.ClauseId,
                            SupplierId = x.SupplierId,
                            CostCenterId = x.CostCenterId,
                            PageInsert = x.PageInsert,
                            InvoiceRetId = x.InvoiceRetId ?? "000000",
                            DunCalc = x.DunCalc ?? false,
                            VoucherAdjustment = x.VoucherAdjustment ?? false,

                            PayTypeName = x.PayTypeName,
                            FileName = x.FileName,
                            FileUrl = x.FileUrl,
                            VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                            VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                            IsSendAlarm = x.IsSendAlarm ?? 0,
                            InvUUID = x.InvUUID ?? "",
                            AddInvoiceImg = x.AddInvoiceImg ?? "/distnew/images/userprofile.png",
                            VoucherDetails = x.VoucherDetails,
                            PurchaseOrderNo = x.PurchaseOrderNo ?? "",
                            PurchaseOrderStatus = x.PurchaseOrderNo == null || x.PurchaseOrderNo == "" ? "تحت المراجعه": "تم التحويل الي فاتورة"  ,
                        }).OrderByDescending(s => s.InvoiceNumber).ToList();
                    }

                }
                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.SupplierId)) && voucherFilterVM!.SupplierId > 0)
                {

                    details = details.Where(s => s.SupplierId == voucherFilterVM!.SupplierId).Select(x => new InvoicesVM
                    {
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceId = x.InvoiceId,
                        Type = x.Type,
                        IsPost = x.IsPost,
                        PostDate = x.PostDate,
                        PostHijriDate = x.PostHijriDate,
                        StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Rad = x.Rad,
                        Date = x.Date,
                        HijriDate = x.HijriDate,
                        InvoiceReference = x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        InvoiceNotes = x.InvoiceNotes ?? "",
                        TotalValue = x.TotalValue,
                        InvoiceValue = x.InvoiceValue ?? 0,

                        TaxAmount = x.TaxAmount,
                        PayType = x.PayType,
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                        ToAccountId = x.ToAccountId,
                        InvoiceValueText = x.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerPhone = x.CustomerPhone,
                        CustomerName_W=x.CustomerName_W,
                        CustomerMobile = x.CustomerMobile,

                        JournalNumber = x.JournalNumber,
                        AddUser = x.AddUser,
                        AddDate = x.AddDate,
                        printBankAccount = x.printBankAccount,
                        DiscountPercentage = x.DiscountPercentage ?? 0,
                        DiscountValue = x.DiscountValue ?? 0,
                        StoreId = x.StoreId,
                        PaidValue = x.PaidValue ?? 0,
                        ToInvoiceId = x.ToInvoiceId,
                        SupplierInvoiceNo = x.SupplierInvoiceNo,
                        RecevierTxt = x.RecevierTxt,
                        ClauseId = x.ClauseId,
                        SupplierId = x.SupplierId,
                        CostCenterId = x.CostCenterId,
                        PageInsert = x.PageInsert,
                        InvoiceRetId = x.InvoiceRetId ?? "000000",
                        DunCalc = x.DunCalc ?? false,
                        VoucherAdjustment = x.VoucherAdjustment ?? false,

                        PayTypeName = x.PayTypeName,
                        FileName = x.FileName,
                        FileUrl = x.FileUrl,
                        VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                        VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                        IsSendAlarm = x.IsSendAlarm ?? 0,
                        InvUUID = x.InvUUID ?? "",
                        AddInvoiceImg = x.AddInvoiceImg ?? "/distnew/images/userprofile.png",
                        VoucherDetails = x.VoucherDetails,
                        PurchaseOrderNo = x.PurchaseOrderNo ?? "",
                        PurchaseOrderStatus = x.PurchaseOrderNo == null || x.PurchaseOrderNo == "" ? "تحت المراجعه": "تم التحويل الي فاتورة"  ,
                    }).OrderByDescending(s => s.InvoiceNumber).ToList();

                }

                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Rad = x.Rad,
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue ?? 0,

                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue ?? 0,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    FileName = x.FileName,
                    FileUrl = x.FileUrl,
                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",
                    AddInvoiceImg = x.AddUsers!.ImgUrl ?? "/distnew/images/userprofile.png",
                    PurchaseOrderNo = x.PurchaseOrderNo ?? "",
                    PurchaseOrderStatus = x.PurchaseOrderNo !=null || x.PurchaseOrderNo !=""?"تم التحويل الي فاتورة":"تحت المراجعه",
                    VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                    {
                        VoucherDetailsId = z.VoucherDetailsId,
                        InvoiceId = z.InvoiceId,
                        LineNumber = z.LineNumber,
                        AccountId = z.AccountId,
                        PayType = z.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = z.TaxType,
                        TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = z.Accounts!.NameAr,
                        Amount = z.Amount,
                        Qty = z.Qty ?? 1,
                        CostCenterId = z.CostCenterId,
                        TaxAmount = z.TaxAmount ?? 0,
                        CostCenterName = z.CostCenters!.NameAr ?? "",
                        Description = z.Description,
                        ReferenceNumber = z.ReferenceNumber ?? "",
                        ToAccountId = z.ToAccountId,
                        ToAccountName = z.ToAccounts!.NameAr,
                        TotalAmount = z.TotalAmount ?? 0,

                        CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                        CheckDate = z.CheckDate,
                        BankName = z.Banks == null ? "" : z.Banks.NameAr,
                        BankId = z.BankId,
                        MoneyOrderDate = z.MoneyOrderDate,
                        MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                        CategoryId = z.CategoryId,
                        DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = z.DiscountValue_Det ?? 0,
                        CategoryName = z.Categories != null ? z.Categories.NAmeAr : "",
                        CategoryTypeName = z.Categories != null ? z.Categories.CategorType != null ? z.Categories.CategorType.NAmeAr ?? "" : "" : "",

                    }).ToList()
                }).OrderByDescending(s => s.InvoiceNumber).ToList();

                return details;
            }

        }

       public async Task<IEnumerable<InvoicesVM>> GetAllAlarmVoucher(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",
                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",
                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;
            }

        }
       public async Task<IEnumerable<InvoicesVM>> GetAllNotioucher(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId && s.IsPost==true).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",
                    SupplierName = x.Suppliers != null ? x.Suppliers.NameAr : "بدون",

                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId && s.IsPost == true).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",
                    SupplierName = x.Suppliers != null ? x.Suppliers.NameAr : "بدون",

                }).OrderByDescending(s => s.InvoiceNumber).ToList();
                return details;
            }

        }

       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersRetSearch(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Rad==true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,
                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullName,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,
                InvUUID = x.InvUUID ?? "",

                CreditNotiId = x.CreditNotiId ?? 0,
                DepitNotiId = x.DepitNotiId ?? 0,
                //CreditDepitNotiTotal = x.InvoicesNoti.Where(s => s.CreditDepitNotiId != null && s.InvoicesNoti.Any(a => a.IsDeleted == false)).Sum(q => q.TotalValue),
                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                DelegateId=x.DelegateId,
                InvoicesRequests = x.InvoicesRequests!.Where(s => s.InvoiceId == x.InvoiceId).Select(w => new Acc_InvoicesRequestsVM
                {
                    InvoiceReqId = w.InvoiceReqId,
                    InvoiceId = w.InvoiceId,
                    InvoiceNoRequest = w.InvoiceNoRequest,
                    IsSent = w.IsSent,
                    StatusCode = w.StatusCode,
                    SendingStatus = w.SendingStatus,
                    warningmessage = w.warningmessage,
                    ClearedInvoice = w.ClearedInvoice,
                    errormessage = w.errormessage,
                    PIH = w.PIH,
                    BranchId = w.BranchId,
                }).ToList(),
                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts!.NameAr,
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts!.NameAr,
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate,
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    ServicesPriceId = z.ServicesPriceId,
                    ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                    ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                    IsRetrieve = z.IsRetrieve ?? 0,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,

                }).ToList()
            }).OrderByDescending(s => s.InvoiceNumber).ToList();

            return details;
        }
        public async Task<IEnumerable<InvoicesVM>> GetAllVouchersDelegate(int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 2 && s.YearId == YearId && s.BranchId == BranchId &&  s.DelegateId != null  ).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,
                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                InvoiceValueText = x.InvoiceValueText,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",
                JournalNumber = x.JournalNumber,
                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                AddUser = x.AddUsers!.FullName,
                AddDate = x.AddDate,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue,
                RecevierTxt = x.RecevierTxt,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DelegateId=x.DelegateId,
                DelegateName = x.Delegate!=null?x.Delegate.EmployeeNameAr??"":"",
            }).OrderByDescending(s => s.InvoiceNumber).ToList();

            return details;
        }


        public async Task<IEnumerable<InvoicesVM>> GetAllVouchersfromcontractSearch(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.PageInsert == 2 && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,
                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullName,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,
                InvUUID = x.InvUUID ?? "",

                CreditNotiId = x.CreditNotiId ?? 0,
                DepitNotiId = x.DepitNotiId ?? 0,
                AddInvoiceImg = x.AddUsers!.ImgUrl ?? "/distnew/images/userprofile.png",
                //CreditDepitNotiTotal = x.InvoicesNoti.Where(s => s.CreditDepitNotiId != null && s.InvoicesNoti.Any(a => a.IsDeleted == false)).Sum(q => q.TotalValue),
                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                DelegateId=x.DelegateId,
                InvoicesRequests = x.InvoicesRequests!.Where(s => s.InvoiceId == x.InvoiceId).Select(w => new Acc_InvoicesRequestsVM
                {
                    InvoiceReqId = w.InvoiceReqId,
                    InvoiceId = w.InvoiceId,
                    InvoiceNoRequest = w.InvoiceNoRequest,
                    IsSent = w.IsSent,
                    StatusCode = w.StatusCode,
                    SendingStatus = w.SendingStatus,
                    warningmessage = w.warningmessage,
                    ClearedInvoice = w.ClearedInvoice,
                    errormessage = w.errormessage,
                    PIH = w.PIH,
                    BranchId = w.BranchId,
                }).ToList(),
                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts!.NameAr,
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts!.NameAr,
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate,
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    ServicesPriceId = z.ServicesPriceId,
                    ServicesPriceName = z.ServicesPrice != null ? z.ServicesPrice.ServicesName : "",
                    ServiceTypeName = z.ServicesPrice != null ? z.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                    IsRetrieve = z.IsRetrieve ?? 0,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,

                }).ToList()
            }).OrderByDescending(s => s.InvoiceNumber).ToList();

            return details;
        }
       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersRetSearchPurchase(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Rad == true && s.Type == voucherFilterVM!.Type && s.YearId == YearId && s.BranchId == BranchId).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type = x.Type,
                IsPost = x.IsPost,
                PostDate = x.PostDate,
                PostHijriDate = x.PostHijriDate,
                StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                Rad = x.Rad,
                Date = x.Date,
                HijriDate = x.HijriDate,
                InvoiceReference = x.InvoiceReference,
                Notes = x.Notes ?? "",
                InvoiceNotes = x.InvoiceNotes ?? "",
                TotalValue = x.TotalValue,
                InvoiceValue = x.InvoiceValue ?? 0,
                TaxAmount = x.TaxAmount,
                PayType = x.PayType,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                ToAccountId = x.ToAccountId,
                InvoiceValueText = x.InvoiceValueText,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                JournalNumber = x.JournalNumber,
                AddUser = x.AddUsers!.FullName,
                AddDate = x.AddDate,
                printBankAccount = x.printBankAccount,
                DiscountPercentage = x.DiscountPercentage ?? 0,
                DiscountValue = x.DiscountValue ?? 0,
                StoreId = x.StoreId,
                PaidValue = x.PaidValue,
                ToInvoiceId = x.ToInvoiceId,
                SupplierInvoiceNo = x.SupplierInvoiceNo,
                RecevierTxt = x.RecevierTxt,
                ClauseId = x.ClauseId,
                SupplierId = x.SupplierId,
                CostCenterId = x.CostCenterId,
                PageInsert = x.PageInsert,
                InvoiceRetId = x.InvoiceRetId ?? "000000",
                DunCalc = x.DunCalc ?? false,
                VoucherAdjustment = x.VoucherAdjustment ?? false,

                VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                IsSendAlarm = x.IsSendAlarm ?? 0,
                InvUUID = x.InvUUID ?? "",
                AddInvoiceImg = x.AddUsers!.ImgUrl ?? "/distnew/images/userprofile.png",

                PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",

                VoucherDetails = x.VoucherDetails!.Where(s => s.InvoiceId == x.InvoiceId && s.IsDeleted == false).Select(z => new VoucherDetailsVM
                {
                    VoucherDetailsId = z.VoucherDetailsId,
                    InvoiceId = z.InvoiceId,
                    LineNumber = z.LineNumber,
                    AccountId = z.AccountId,
                    PayType = z.PayType,
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    TaxType = z.TaxType,
                    TaxTypeName = z.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                    AccountName = z.Accounts!.NameAr,
                    Amount = z.Amount,
                    Qty = z.Qty ?? 1,
                    CostCenterId = z.CostCenterId,
                    TaxAmount = z.TaxAmount ?? 0,
                    CostCenterName = z.CostCenters!.NameAr ?? "",
                    Description = z.Description,
                    ReferenceNumber = z.ReferenceNumber ?? "",
                    ToAccountId = z.ToAccountId,
                    ToAccountName = z.ToAccounts!.NameAr,
                    TotalAmount = z.TotalAmount ?? 0,

                    CheckNo = (z.CheckNo == "null") ? "" : (z.CheckNo == null) ? "" : z.CheckNo,
                    CheckDate = z.CheckDate,
                    BankName = z.Banks == null ? "" : z.Banks.NameAr,
                    BankId = z.BankId,
                    MoneyOrderDate = z.MoneyOrderDate,
                    MoneyOrderNo = z.MoneyOrderNo == null ? 0 : z.MoneyOrderNo,
                    CategoryId = z.CategoryId,
                    DiscountPercentage_Det = z.DiscountPercentage_Det ?? 0,
                    DiscountValue_Det = z.DiscountValue_Det ?? 0,
                    CategoryName = z.Categories != null ? z.Categories.NAmeAr : "",
                    CategoryTypeName = z.Categories != null ? z.Categories.CategorType != null ? z.Categories.CategorType.NAmeAr ?? "" : "" : "",

                }).ToList()
            }).OrderByDescending(s => s.InvoiceNumber).ToList();

            return details;
        }


       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersBySearchQR(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {

                var details = _TaamerProContext.Invoices.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.YearId == YearId && s.Type == voucherFilterVM!.Type && voucherFilterVM!.QRCode==s.QRCodeNum
                                                                           ).Select(x => new InvoicesVM
                                                                           {
                                                                               InvoiceNumber = x.InvoiceNumber,
                                                                               InvoiceId = x.InvoiceId,
                                                                               Type = x.Type,
                                                                               IsPost = x.IsPost,
                                                                               PostDate = x.PostDate,
                                                                               PostHijriDate = x.PostHijriDate,
                                                                               StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                                                                               Date = x.Date,
                                                                               HijriDate = x.HijriDate,
                                                                               InvoiceReference = x.InvoiceReference,
                                                                               Notes = x.Notes ?? "",
                                                                               InvoiceNotes = x.InvoiceNotes ?? "",
                                                                               TotalValue = x.TotalValue,
                                                                               InvoiceValue = x.InvoiceValue ?? 0,
                                                                               TaxAmount = x.TaxAmount,
                                                                               PayType = x.PayType,
                                                                               ProjectId = x.ProjectId,
                                                                               ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                                                                               ToAccountId = x.ToAccountId,
                                                                               InvoiceValueText = x.InvoiceValueText,
                                                                               BranchId = x.BranchId,
                                                                               CustomerId = x.CustomerId,
                                                                               CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                                                                               CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                                                                               CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                                                                               CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                                                                               JournalNumber = x.JournalNumber,
                                                                               AddUser = x.AddUsers!.FullName,
                                                                               AddDate = x.AddDate,
                                                                               printBankAccount = x.printBankAccount,
                                                                               DiscountPercentage = x.DiscountPercentage ?? 0,
                                                                               DiscountValue = x.DiscountValue ?? 0,
                                                                               StoreId = x.StoreId,
                                                                               PaidValue = x.PaidValue,
                                                                               ToInvoiceId = x.ToInvoiceId,
                                                                               SupplierInvoiceNo = x.SupplierInvoiceNo,
                                                                               RecevierTxt = x.RecevierTxt,
                                                                               ClauseId = x.ClauseId,
                                                                               SupplierId = x.SupplierId,
                                                                               CostCenterId = x.CostCenterId,
                                                                               PageInsert = x.PageInsert,
                                                                               InvoiceRetId = x.InvoiceRetId ?? "000000",
                                                                               DunCalc = x.DunCalc ?? false,
                                                                               VoucherAdjustment = x.VoucherAdjustment ?? false,

                                                                               VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                                                                               VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                                                                               IsSendAlarm = x.IsSendAlarm ?? 0,
                                                                               InvUUID = x.InvUUID ?? "",

                                                                               CreditNotiId = x.CreditNotiId ?? 0,
                                                                               DepitNotiId = x.DepitNotiId ?? 0,
                                                                               //CreditDepitNotiTotal = x.InvoicesNoti.Where(s => s.CreditDepitNotiId != null && s.InvoicesNoti.Any(a => a.IsDeleted == false)).Sum(q => q.TotalValue),
                                                                               PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",


                                                                           }).ToList().OrderBy(s => s.InvoiceNumber);

                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.YearId == YearId && s.Type == voucherFilterVM!.Type && voucherFilterVM!.QRCode == s.QRCodeNum
                                                                           ).Select(x => new InvoicesVM
                                                                           {
                                                                               InvoiceNumber = x.InvoiceNumber,
                                                                               InvoiceId = x.InvoiceId,
                                                                               Type = x.Type,
                                                                               IsPost = x.IsPost,
                                                                               PostDate = x.PostDate,
                                                                               PostHijriDate = x.PostHijriDate,
                                                                               StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                                                                               Date = x.Date,
                                                                               HijriDate = x.HijriDate,
                                                                               InvoiceReference = x.InvoiceReference,
                                                                               Notes = x.Notes ?? "",
                                                                               InvoiceNotes = x.InvoiceNotes ?? "",
                                                                               TotalValue = x.TotalValue,
                                                                               InvoiceValue = x.InvoiceValue ?? 0,
                                                                               TaxAmount = x.TaxAmount,
                                                                               PayType = x.PayType,
                                                                               ProjectId = x.ProjectId,
                                                                               ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                                                                               ToAccountId = x.ToAccountId,
                                                                               InvoiceValueText = x.InvoiceValueText,
                                                                               BranchId = x.BranchId,
                                                                               CustomerId = x.CustomerId,
                                                                               CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                                                                               CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                                                                               CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                                                                               CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                                                                               JournalNumber = x.JournalNumber,
                                                                               AddUser = x.AddUsers!.FullName,
                                                                               AddDate = x.AddDate,
                                                                               printBankAccount = x.printBankAccount,
                                                                               DiscountPercentage = x.DiscountPercentage ?? 0,
                                                                               DiscountValue = x.DiscountValue ?? 0,
                                                                               StoreId = x.StoreId,
                                                                               PaidValue = x.PaidValue,
                                                                               ToInvoiceId = x.ToInvoiceId,
                                                                               SupplierInvoiceNo = x.SupplierInvoiceNo,
                                                                               RecevierTxt = x.RecevierTxt,
                                                                               ClauseId = x.ClauseId,
                                                                               SupplierId = x.SupplierId,
                                                                               CostCenterId = x.CostCenterId,
                                                                               PageInsert = x.PageInsert,
                                                                               InvoiceRetId = x.InvoiceRetId ?? "000000",
                                                                               DunCalc = x.DunCalc ?? false,
                                                                               VoucherAdjustment = x.VoucherAdjustment ?? false,

                                                                               VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                                                                               VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                                                                               IsSendAlarm = x.IsSendAlarm ?? 0,
                                                                               InvUUID = x.InvUUID ?? "",

                                                                               CreditNotiId = x.CreditNotiId ?? 0,
                                                                               DepitNotiId = x.DepitNotiId ?? 0,
                                                                               //CreditDepitNotiTotal = x.InvoicesNoti.Where(s => s.CreditDepitNotiId != null && s.InvoicesNoti.Any(a => a.IsDeleted == false)).Sum(q => q.TotalValue),
                                                                               PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",


                                                                           }).ToList().OrderBy(s => s.InvoiceNumber);

                return details;
            }

        }
       public async Task<IEnumerable<InvoicesVM>> GetAllVouchersBySearchRet(VoucherFilterVM voucherFilterVM, int YearId, int BranchId)
        {
            try
            {

                var details = _TaamerProContext.Invoices.ToList().Where(s => s.IsDeleted == false && s.IsPost==true && s.BranchId == BranchId && s.YearId == YearId && s.Type == voucherFilterVM!.Type
                                                                           ).Select(x => new InvoicesVM
                                                                           {
                                                                               InvoiceNumber = x.InvoiceNumber,
                                                                               InvoiceId = x.InvoiceId,
                                                                               Type = x.Type,
                                                                               IsPost = x.IsPost,
                                                                               PostDate = x.PostDate,
                                                                               PostHijriDate = x.PostHijriDate,
                                                                               StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                                                                               Date = x.Date,
                                                                               HijriDate = x.HijriDate,
                                                                               InvoiceReference = x.InvoiceReference,
                                                                               Notes = x.Notes ?? "",
                                                                               InvoiceNotes = x.InvoiceNotes ?? "",
                                                                               TotalValue = x.TotalValue,
                                                                               InvoiceValue = x.InvoiceValue ?? 0,
                                                                               TaxAmount = x.TaxAmount,
                                                                               PayType = x.PayType,
                                                                               ProjectId = x.ProjectId,
                                                                               ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                                                                               ToAccountId = x.ToAccountId,
                                                                               InvoiceValueText = x.InvoiceValueText,
                                                                               BranchId = x.BranchId,
                                                                               CustomerId = x.CustomerId,
                                                                               CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                                                                               CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                                                                               CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                                                                               CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                                                                               JournalNumber = x.JournalNumber,
                                                                               AddUser = x.AddUsers!.FullName,
                                                                               AddDate = x.AddDate,
                                                                               printBankAccount = x.printBankAccount,
                                                                               DiscountPercentage = x.DiscountPercentage ?? 0,
                                                                               DiscountValue = x.DiscountValue ?? 0,
                                                                               StoreId = x.StoreId,
                                                                               PaidValue = x.PaidValue,
                                                                               ToInvoiceId = x.ToInvoiceId,
                                                                               SupplierInvoiceNo = x.SupplierInvoiceNo,
                                                                               RecevierTxt = x.RecevierTxt,
                                                                               ClauseId = x.ClauseId,
                                                                               SupplierId = x.SupplierId,
                                                                               CostCenterId = x.CostCenterId,
                                                                               PageInsert = x.PageInsert,
                                                                               InvoiceRetId = x.InvoiceRetId ?? "000000",
                                                                               DunCalc = x.DunCalc ?? false,
                                                                               VoucherAdjustment = x.VoucherAdjustment ?? false,

                                                                               VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                                                                               VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                                                                               IsSendAlarm = x.IsSendAlarm ?? 0,
                                                                               InvUUID = x.InvUUID ?? "",

                                                                               CreditNotiId = x.CreditNotiId ?? 0,
                                                                               DepitNotiId = x.DepitNotiId ?? 0,
                                                                               //CreditDepitNotiTotal = x.InvoicesNoti.Where(s => s.CreditDepitNotiId != null && s.InvoicesNoti.Any(a => a.IsDeleted == false)).Sum(q => q.TotalValue),
                                                                               PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",


                                                                           }).ToList().OrderBy(s => s.InvoiceNumber);



                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.VoucherNo)) && voucherFilterVM!.VoucherNo > 0)
                {

                    details = details.Where(s => s.InvoiceNumber == voucherFilterVM!.VoucherNo.ToString()
                                                                              ).Select(x => new InvoicesVM
                                                                              {
                                                                                  InvoiceNumber = x.InvoiceNumber,
                                                                                  InvoiceId = x.InvoiceId,
                                                                                  Type = x.Type,
                                                                                  IsPost = x.IsPost,
                                                                                  PostDate = x.PostDate,
                                                                                  PostHijriDate = x.PostHijriDate,
                                                                                  StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                                                                                  Date = x.Date,
                                                                                  HijriDate = x.HijriDate,
                                                                                  InvoiceReference = x.InvoiceReference,
                                                                                  Notes = x.Notes ?? "",
                                                                                  InvoiceNotes = x.InvoiceNotes ?? "",
                                                                                  TotalValue = x.TotalValue,
                                                                                  InvoiceValue = x.InvoiceValue ?? 0,
                                                                                  TaxAmount = x.TaxAmount,
                                                                                  PayType = x.PayType,
                                                                                  ProjectId = x.ProjectId,
                                                                                  ProjectNo = x.ProjectNo,
                                                                                  ToAccountId = x.ToAccountId,
                                                                                  InvoiceValueText = x.InvoiceValueText,
                                                                                  BranchId = x.BranchId,
                                                                                  CustomerId = x.CustomerId,
                                                                                  CustomerName = x.CustomerName,
                                                                                  CustomerName_W=x.CustomerName_W,
                                                                                  CustomerPhone = x.CustomerPhone,
                                                                                  CustomerMobile = x.CustomerMobile,

                                                                                  JournalNumber = x.JournalNumber,
                                                                                  AddUser = x.AddUser,
                                                                                  AddDate = x.AddDate,
                                                                                  printBankAccount = x.printBankAccount,
                                                                                  DiscountPercentage = x.DiscountPercentage ?? 0,
                                                                                  DiscountValue = x.DiscountValue ?? 0,
                                                                                  StoreId = x.StoreId,
                                                                                  PaidValue = x.PaidValue,
                                                                                  ToInvoiceId = x.ToInvoiceId,
                                                                                  SupplierInvoiceNo = x.SupplierInvoiceNo,
                                                                                  RecevierTxt = x.RecevierTxt,
                                                                                  ClauseId = x.ClauseId,
                                                                                  SupplierId = x.SupplierId,
                                                                                  CostCenterId = x.CostCenterId,
                                                                                  PageInsert = x.PageInsert,
                                                                                  InvoiceRetId = x.InvoiceRetId ?? "000000",
                                                                                  DunCalc = x.DunCalc ?? false,
                                                                                  VoucherAdjustment = x.VoucherAdjustment ?? false,

                                                                                  VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                                                                                  VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                                                                                  IsSendAlarm = x.IsSendAlarm ?? 0,
                                                                                  InvUUID = x.InvUUID ?? "",

                                                                                  CreditNotiId = x.CreditNotiId ?? 0,
                                                                                  DepitNotiId = x.DepitNotiId ?? 0,
                                                                                  //CreditDepitNotiTotal = x.//CreditDepitNotiTotal??0,
                                                                                  PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",



                                                                              }).ToList().OrderBy(s => s.InvoiceNumber);


                }


                ///////////////
                if (voucherFilterVM!.IsChecked == true)
                {
                    //if (voucherFilterVM!.IsPost == 1)
                    //{
                    details = details.Where(s => (DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(voucherFilterVM!.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || voucherFilterVM!.dateFrom == null)
                                                                              && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(voucherFilterVM!.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)

                                                                              ).Select(x => new InvoicesVM
                                                                              {
                                                                                  InvoiceNumber = x.InvoiceNumber,
                                                                                  InvoiceId = x.InvoiceId,
                                                                                  Type = x.Type,
                                                                                  IsPost = x.IsPost,
                                                                                  PostDate = x.PostDate,
                                                                                  PostHijriDate = x.PostHijriDate,
                                                                                  StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                                                                                  Date = x.Date,
                                                                                  HijriDate = x.HijriDate,
                                                                                  InvoiceReference = x.InvoiceReference,
                                                                                  Notes = x.Notes ?? "",
                                                                                  InvoiceNotes = x.InvoiceNotes ?? "",
                                                                                  TotalValue = x.TotalValue,
                                                                                  InvoiceValue = x.InvoiceValue ?? 0,
                                                                                  TaxAmount = x.TaxAmount,
                                                                                  PayType = x.PayType,
                                                                                  ProjectId = x.ProjectId,
                                                                                  ProjectNo = x.ProjectNo,
                                                                                  ToAccountId = x.ToAccountId,
                                                                                  InvoiceValueText = x.InvoiceValueText,
                                                                                  BranchId = x.BranchId,
                                                                                  CustomerId = x.CustomerId,
                                                                                  CustomerName = x.CustomerName,
                                                                                  CustomerName_W=x.CustomerName_W,
                                                                                  CustomerPhone = x.CustomerPhone,
                                                                                  CustomerMobile = x.CustomerMobile,

                                                                                  JournalNumber = x.JournalNumber,
                                                                                  AddUser = x.AddUser,
                                                                                  AddDate = x.AddDate,
                                                                                  printBankAccount = x.printBankAccount,
                                                                                  DiscountPercentage = x.DiscountPercentage ?? 0,
                                                                                  DiscountValue = x.DiscountValue ?? 0,
                                                                                  StoreId = x.StoreId,
                                                                                  PaidValue = x.PaidValue,
                                                                                  ToInvoiceId = x.ToInvoiceId,
                                                                                  SupplierInvoiceNo = x.SupplierInvoiceNo,
                                                                                  RecevierTxt = x.RecevierTxt,
                                                                                  ClauseId = x.ClauseId,
                                                                                  SupplierId = x.SupplierId,
                                                                                  CostCenterId = x.CostCenterId,
                                                                                  PageInsert = x.PageInsert,
                                                                                  InvoiceRetId = x.InvoiceRetId ?? "000000",
                                                                                  DunCalc = x.DunCalc ?? false,
                                                                                  VoucherAdjustment = x.VoucherAdjustment ?? false,

                                                                                  VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                                                                                  VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                                                                                  IsSendAlarm = x.IsSendAlarm ?? 0,
                                                                                  InvUUID = x.InvUUID ?? "",

                                                                                  CreditNotiId = x.CreditNotiId ?? 0,
                                                                                  DepitNotiId = x.DepitNotiId ?? 0,
                                                                                  //CreditDepitNotiTotal = x.//CreditDepitNotiTotal ?? 0,
                                                                                  PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",



                                                                              }).ToList().OrderBy(s => s.InvoiceNumber);

                }

                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.IsPost)) && voucherFilterVM!.IsPost > 0)
                {
                    bool Ispost = (voucherFilterVM!.IsPost == 1) ? true : false;
                    details = details.Where(s => s.IsPost == Ispost
                                                                              ).Select(x => new InvoicesVM
                                                                              {
                                                                                  InvoiceNumber = x.InvoiceNumber,
                                                                                  InvoiceId = x.InvoiceId,
                                                                                  Type = x.Type,
                                                                                  IsPost = x.IsPost,
                                                                                  PostDate = x.PostDate,
                                                                                  PostHijriDate = x.PostHijriDate,
                                                                                  StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                                                                                  Date = x.Date,
                                                                                  HijriDate = x.HijriDate,
                                                                                  InvoiceReference = x.InvoiceReference,
                                                                                  Notes = x.Notes ?? "",
                                                                                  InvoiceNotes = x.InvoiceNotes ?? "",
                                                                                  TotalValue = x.TotalValue,
                                                                                  InvoiceValue = x.InvoiceValue ?? 0,
                                                                                  TaxAmount = x.TaxAmount,
                                                                                  PayType = x.PayType,
                                                                                  ProjectId = x.ProjectId,
                                                                                  ProjectNo = x.ProjectNo,
                                                                                  ToAccountId = x.ToAccountId,
                                                                                  InvoiceValueText = x.InvoiceValueText,
                                                                                  BranchId = x.BranchId,
                                                                                  CustomerId = x.CustomerId,
                                                                                  CustomerName = x.CustomerName,
                                                                                  CustomerName_W=x.CustomerName_W,
                                                                                  CustomerPhone = x.CustomerPhone,
                                                                                  CustomerMobile = x.CustomerMobile,

                                                                                  JournalNumber = x.JournalNumber,
                                                                                  AddUser = x.AddUser,
                                                                                  AddDate = x.AddDate,
                                                                                  printBankAccount = x.printBankAccount,
                                                                                  DiscountPercentage = x.DiscountPercentage ?? 0,
                                                                                  DiscountValue = x.DiscountValue ?? 0,
                                                                                  StoreId = x.StoreId,
                                                                                  PaidValue = x.PaidValue,
                                                                                  ToInvoiceId = x.ToInvoiceId,
                                                                                  SupplierInvoiceNo = x.SupplierInvoiceNo,
                                                                                  RecevierTxt = x.RecevierTxt,
                                                                                  ClauseId = x.ClauseId,
                                                                                  SupplierId = x.SupplierId,
                                                                                  CostCenterId = x.CostCenterId,
                                                                                  PageInsert = x.PageInsert,
                                                                                  InvoiceRetId = x.InvoiceRetId ?? "000000",
                                                                                  DunCalc = x.DunCalc ?? false,
                                                                                  VoucherAdjustment = x.VoucherAdjustment ?? false,

                                                                                  VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                                                                                  VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                                                                                  IsSendAlarm = x.IsSendAlarm ?? 0,
                                                                                  InvUUID = x.InvUUID ?? "",

                                                                                  CreditNotiId = x.CreditNotiId ?? 0,
                                                                                  DepitNotiId = x.DepitNotiId ?? 0,
                                                                                  //CreditDepitNotiTotal = x.//CreditDepitNotiTotal ?? 0,
                                                                                  PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",



                                                                              }).ToList().OrderBy(s => s.InvoiceNumber);

                }
                if (!String.IsNullOrEmpty(Convert.ToString(voucherFilterVM!.ProjectId)) && voucherFilterVM!.ProjectId > 0)
                {

                    details = details.Where(s => s.ProjectId == voucherFilterVM!.ProjectId
                                                                              ).Select(x => new InvoicesVM
                                                                              {
                                                                                  InvoiceNumber = x.InvoiceNumber,
                                                                                  InvoiceId = x.InvoiceId,
                                                                                  Type = x.Type,
                                                                                  IsPost = x.IsPost,
                                                                                  PostDate = x.PostDate,
                                                                                  PostHijriDate = x.PostHijriDate,
                                                                                  StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                                                                                  Date = x.Date,
                                                                                  HijriDate = x.HijriDate,
                                                                                  InvoiceReference = x.InvoiceReference,
                                                                                  Notes = x.Notes ?? "",
                                                                                  InvoiceNotes = x.InvoiceNotes ?? "",
                                                                                  TotalValue = x.TotalValue,
                                                                                  InvoiceValue = x.InvoiceValue ?? 0,
                                                                                  TaxAmount = x.TaxAmount,
                                                                                  PayType = x.PayType,
                                                                                  ProjectId = x.ProjectId,
                                                                                  ProjectNo = x.ProjectNo,
                                                                                  ToAccountId = x.ToAccountId,
                                                                                  InvoiceValueText = x.InvoiceValueText,
                                                                                  BranchId = x.BranchId,
                                                                                  CustomerId = x.CustomerId,
                                                                                  CustomerName = x.CustomerName,
                                                                                  CustomerName_W=x.CustomerName_W,
                                                                                  CustomerPhone = x.CustomerPhone,
                                                                                  CustomerMobile = x.CustomerMobile,

                                                                                  JournalNumber = x.JournalNumber,
                                                                                  AddUser = x.AddUser,
                                                                                  AddDate = x.AddDate,
                                                                                  printBankAccount = x.printBankAccount,
                                                                                  DiscountPercentage = x.DiscountPercentage ?? 0,
                                                                                  DiscountValue = x.DiscountValue ?? 0,
                                                                                  StoreId = x.StoreId,
                                                                                  PaidValue = x.PaidValue,
                                                                                  ToInvoiceId = x.ToInvoiceId,
                                                                                  SupplierInvoiceNo = x.SupplierInvoiceNo,
                                                                                  RecevierTxt = x.RecevierTxt,
                                                                                  ClauseId = x.ClauseId,
                                                                                  SupplierId = x.SupplierId,
                                                                                  CostCenterId = x.CostCenterId,
                                                                                  PageInsert = x.PageInsert,
                                                                                  InvoiceRetId = x.InvoiceRetId ?? "000000",
                                                                                  DunCalc = x.DunCalc ?? false,
                                                                                  VoucherAdjustment = x.VoucherAdjustment ?? false,

                                                                                  VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                                                                                  VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                                                                                  IsSendAlarm = x.IsSendAlarm ?? 0,
                                                                                  InvUUID = x.InvUUID ?? "",

                                                                                  CreditNotiId = x.CreditNotiId ?? 0,
                                                                                  DepitNotiId = x.DepitNotiId ?? 0,
                                                                                  //CreditDepitNotiTotal = x.//CreditDepitNotiTotal ?? 0,
                                                                                  PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",



                                                                              }).ToList().OrderBy(s => s.InvoiceNumber);

                }

                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Invoices.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.YearId == YearId && s.Type == voucherFilterVM!.Type && (s.InvoiceNumber == voucherFilterVM!.VoucherNo.ToString() || voucherFilterVM!.VoucherNo == 0)).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    CreditNotiId = x.CreditNotiId ?? 0,
                    DepitNotiId = x.DepitNotiId ?? 0,
                    //CreditDepitNotiTotal = x.InvoicesNoti.Where(s => s.CreditDepitNotiId != null && s.InvoicesNoti.Any(a => a.IsDeleted == false)).Sum(q => q.TotalValue),
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",


                }).ToList().OrderBy(s => s.InvoiceNumber);

                return details;
            }

        }
         public async Task<DataTable> ReceiptCashingPaying(int InvoiceId, string Con)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Receipt_Cashing_Paying";
                        command.Connection = con;



                        command.Parameters.Add(new SqlParameter("@InvoiceId", InvoiceId));


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);

                        dt = ds.Tables[0];

                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                DataTable dt = new DataTable();
                return dt;
            }

        }
         public async Task<DataTable> ReceiptCashingPayingNoti(int InvoiceId, string Con)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Receipt_Cashing_PayingNoti";
                        command.Connection = con;



                        command.Parameters.Add(new SqlParameter("@InvoiceId", InvoiceId));


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);

                        dt = ds.Tables[0];

                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                DataTable dt = new DataTable();
                return dt;
            }

        }
         public async Task<DataTable> ReceiptCashingPayingNotiDepit(int InvoiceId, string Con)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Receipt_Cashing_PayingNotiDepit";
                        command.Connection = con;



                        command.Parameters.Add(new SqlParameter("@InvoiceId", InvoiceId));


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);

                        dt = ds.Tables[0];

                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                DataTable dt = new DataTable();
                return dt;
            }

        }
         public async Task<DataTable> ReceiptCashingPayingNotiDepitPurchase(int InvoiceId, string Con)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Receipt_Cashing_PayingNotiDepitPurchase";
                        command.Connection = con;



                        command.Parameters.Add(new SqlParameter("@InvoiceId", InvoiceId));


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);

                        dt = ds.Tables[0];

                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                DataTable dt = new DataTable();
                return dt;
            }

        }

         public async Task<InvoicesVM> GetVoucherById(int VoucherId)
        {
            try
            {
                var emp = _TaamerProContext.Invoices.Where(s => s.IsDeleted==false && s.InvoiceId == VoucherId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Rad = x.Rad,
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullNameAr ==null? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",
                    ContractNo=x.Project!=null?x.Project!.Contracts!=null?x.Project!.Contracts.ContractNo:"":"",
                    CreditNotiId = x.CreditNotiId ?? 0,
                    DepitNotiId = x.DepitNotiId ?? 0,
                    AppearUser= x.AddUsers!.AppearInInvoicePrint??0,
                    //CreditDepitNotiTotal = x.InvoicesNoti.Where(s => s.CreditDepitNotiId != null && s.InvoicesNoti.Any(a => a.IsDeleted == false)).Sum(q => q.TotalValue),
                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                    DelegateId=x.DelegateId,
                    VoucherDetails = x.VoucherDetails!.Select(s => new VoucherDetailsVM
                    {
                        VoucherDetailsId = s.VoucherDetailsId,
                        InvoiceId = s.InvoiceId,
                        LineNumber = s.LineNumber,
                        AccountId = s.AccountId,
                        PayType = s.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = s.TaxType,
                        TaxTypeName = s.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = s.Accounts!.NameAr,
                        Amount = s.Amount,
                        Qty = s.Qty ?? 1,
                        CostCenterId = s.CostCenterId,
                        TaxAmount = s.TaxAmount ?? 0,
                        CostCenterName = s.CostCenters!.NameAr ?? "",
                        Description = s.Description,
                        ReferenceNumber = s.ReferenceNumber ?? "",
                        ToAccountId = s.ToAccountId,
                        ToAccountName = s.ToAccounts!.NameAr,
                        TotalAmount = s.TotalAmount ?? 0,
                        CheckNo = (s.CheckNo == "null") ? "" : (s.CheckNo == null) ? "" : s.CheckNo,
                        CheckDate = s.CheckDate,
                        BankName = s.Banks == null ? "" : s.Banks.NameAr,
                        BankId = s.BankId,
                        MoneyOrderDate = s.MoneyOrderDate,
                        MoneyOrderNo = s.MoneyOrderNo == null ? 0 : s.MoneyOrderNo,
                        ServicesPriceId = s.ServicesPriceId,
                        ServicesPriceName = s.ServicesPrice != null ? s.ServicesPrice.ServicesName : "",
                        ServiceTypeName = s.ServicesPrice != null ? s.ServicesPrice.ServiceType == 2 ? "تقرير" : "خدمة" : "خدمة",

                        IsRetrieve = s.IsRetrieve ?? 0,
                        DiscountPercentage_Det = s.DiscountPercentage_Det ?? 0,
                        DiscountValue_Det = s.DiscountValue_Det ?? 0,

                    }).ToList(),
                    Transactions = x.TransactionDetails!.Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = tr.JournalNo,
                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit,
                        Credit = tr.Credit,
                        CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                        TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Invoices != null ? tr.Invoices.InvoiceNotes == null ? tr.Notes : tr.Invoices.InvoiceNotes ?? "" : "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                        AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",

                        AccountId = tr.AccountId,
                        Amount = (tr.Depit > tr.Credit) ? tr.Depit : tr.Credit,
                        DepitOrCreditName = tr.Depit > tr.Credit ? "مدين" : "دائن",
                        CostCenterId = tr.CostCenterId,
                        AccCalcExpen = tr.AccCalcExpen ?? false,

                    }).ToList(),
                }).First();
                return emp;
            }
            catch (Exception ex)
            {
                InvoicesVM ina = new InvoicesVM(); 
                return ina;
            }

        }
         public async Task<InvoicesVM> GetInvoiceDateById(int VoucherId)
        {
            try
            {
                var inv = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == VoucherId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    Date = x.Date,
                }).First();
                return inv;
            }
            catch (Exception ex)
            {
                InvoicesVM ina = new InvoicesVM();
                return ina;
            }

        }

       public async Task<IEnumerable<InvoicesVM>> GetVoucherByIdNoti(int VoucherId)
        {
            var details = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && (s.CreditNotiId == VoucherId || s.DepitNotiId== VoucherId)).Select(x => new InvoicesVM
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceId = x.InvoiceId,
                Type=x.Type,
                TotalValue = x.TotalValue,
                CreditNotiId = x.CreditNotiId ?? 0,
                DepitNotiId = x.DepitNotiId ?? 0,
                CreditNotiTotal = x.VoucherDetails!.Where(I => I.IsDeleted == false && I.InvoiceId == x.InvoiceId).Sum(t => t.TotalAmount) ?? 0,
                DepitNotiTotal = x.VoucherDetails!.Where(I => I.IsDeleted == false && I.InvoiceId == x.InvoiceId).Sum(t => t.TotalAmount) ?? 0,


            }).OrderByDescending(s => s.InvoiceNumber).ToList();

            return details;
        }

         public async Task<InvoicesVM> GetVoucherByIdPurchase(int VoucherId)
        {
            try
            {
                var emp = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == VoucherId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    StatusNameNew = x.Rad == true ? "ملغي" : x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    Rad = x.Rad,
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    CustomerName_W = x.Customer != null ? x.Customer.CustomerNameAr : "بدون",
                    CustomerName = x.Customer != null ? x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "بدون",

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",

                    JournalNumber = x.JournalNumber,
                    AddUser = x.AddUsers!.FullName,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    VoucherAlarmDate = x.VoucherAlarmDate ?? "",
                    VoucherAlarmCheck = x.VoucherAlarmCheck ?? false,
                    IsSendAlarm = x.IsSendAlarm ?? 0,
                    InvUUID = x.InvUUID ?? "",

                    PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",

                    VoucherDetails = x.VoucherDetails!.Select(s => new VoucherDetailsVM
                    {
                        VoucherDetailsId = s.VoucherDetailsId,
                        InvoiceId = s.InvoiceId,
                        LineNumber = s.LineNumber,
                        AccountId = s.AccountId,
                        PayType = s.PayType,
                        PayTypeName = (x.PayType == 1) ? "نقدي" : (x.PayType == 2) ? "شيك" : (x.PayType == 6) ? "حوالة" : (x.PayType == 3) ? "عهده" : (x.PayType == 4) ? "خصم مكتسب" : (x.PayType == 5) ? "خصم مسموح به" : (x.PayType == 8) ? "أجل" : (x.PayType == 9) ? "شبكة" : (x.PayType == 17) ? "نقدا - نقاط البيع" : "",
                        TaxType = s.TaxType,
                        TaxTypeName = s.TaxType == 1 ? "غير شامل الضريبة" : "شامل الضريبة",
                        AccountName = s.Accounts!.NameAr,
                        Amount = s.Amount,
                        Qty = s.Qty ?? 1,
                        CostCenterId = s.CostCenterId,
                        TaxAmount = s.TaxAmount ?? 0,
                        CostCenterName = s.CostCenters!.NameAr ?? "",
                        Description = s.Description,
                        ReferenceNumber = s.ReferenceNumber ?? "",
                        ToAccountId = s.ToAccountId,
                        ToAccountName = s.ToAccounts!.NameAr,
                        TotalAmount = s.TotalAmount ?? 0,
                        CheckNo = (s.CheckNo == "null") ? "" : (s.CheckNo == null) ? "" : s.CheckNo,
                        CheckDate = s.CheckDate,
                        BankName = s.Banks == null ? "" : s.Banks.NameAr,
                        BankId = s.BankId,
                        MoneyOrderDate = s.MoneyOrderDate,
                        MoneyOrderNo = s.MoneyOrderNo == null ? 0 : s.MoneyOrderNo,
                        CategoryId = s.CategoryId,
                        CategoryName = s.Categories != null ? s.Categories.NAmeAr : "",
                        CategoryTypeName = s.Categories != null ? s.Categories.CategorType != null ? s.Categories.CategorType.NAmeAr ?? "" : "" : "",

                    }).ToList(),
                    Transactions = x.TransactionDetails!.Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = tr.JournalNo,
                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit,
                        Credit = tr.Credit,
                        CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                        TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Invoices != null ? tr.Invoices.InvoiceNotes == null ? tr.Notes : tr.Invoices.InvoiceNotes ?? "" : "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                        AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    }).ToList(),
                }).First();
                return emp;
            }
            catch (Exception ex)
            {
                InvoicesVM ina = new InvoicesVM();
                return ina;
            }

        }

         public async Task<Invoices> GetInvoicesById(int VoucherId)
        {
            try
            {
                var emp = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == VoucherId).Select(x => new Invoices
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    Rad = x.Rad,
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    JournalNumber = x.JournalNumber,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    CreditNotiId = x.CreditNotiId ?? 0,
                    DepitNotiId = x.DepitNotiId ?? 0,
                    ////CreditDepitNotiTotal = x.InvoicesNoti != null ? x.InvoicesNoti.TotalValue : 0,
                }).FirstOrDefault();
                return emp;
            }
            catch (Exception ex)
            {
                Invoices ina = new Invoices();
                return ina;
            }

        }
        public async Task<InvoicesVM> GetInvById(int VoucherId)
        {
            try
            {
                var emp = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == VoucherId).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    Rad = x.Rad,
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue ?? 0,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectId = x.ProjectId,
                    ToAccountId = x.ToAccountId,
                    InvoiceValueText = x.InvoiceValueText,
                    BranchId = x.BranchId,
                    CustomerId = x.CustomerId,
                    JournalNumber = x.JournalNumber,
                    AddDate = x.AddDate,
                    printBankAccount = x.printBankAccount,
                    DiscountPercentage = x.DiscountPercentage ?? 0,
                    DiscountValue = x.DiscountValue ?? 0,
                    StoreId = x.StoreId,
                    PaidValue = x.PaidValue,
                    ToInvoiceId = x.ToInvoiceId,
                    SupplierInvoiceNo = x.SupplierInvoiceNo,
                    RecevierTxt = x.RecevierTxt,
                    ClauseId = x.ClauseId,
                    SupplierId = x.SupplierId,
                    CostCenterId = x.CostCenterId,
                    PageInsert = x.PageInsert,
                    InvoiceRetId = x.InvoiceRetId ?? "000000",
                    DunCalc = x.DunCalc ?? false,
                    VoucherAdjustment = x.VoucherAdjustment ?? false,

                    CreditNotiId = x.CreditNotiId ?? 0,
                    DepitNotiId = x.DepitNotiId ?? 0,

                    CustomerPhone = x.Customer != null ? x.Customer.CustomerPhone : "",
                    CustomerMobile = x.Customer != null ? x.Customer.CustomerMobile : "",


                }).FirstOrDefault();
                return emp;
            }
            catch (Exception ex)
            {
                InvoicesVM ina = new InvoicesVM();
                return ina;
            }

        }


        public async Task<DataTable> DailyVoucherReport(int VoucherId, string Con)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Rpt_DailyVoucherReport";
                        command.Connection = con;



                        command.Parameters.Add(new SqlParameter("@Id", VoucherId));


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);

                        dt = ds.Tables[0];

                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                DataTable dt = new DataTable();
                return dt;
            }
        }
         public async Task<DataTable> OpeningVoucherReport(int VoucherId, string Con)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Rpt_DailyVoucherReport";
                        command.Connection = con;

                        command.Parameters.Add(new SqlParameter("@Id", VoucherId));


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);

                        dt = ds.Tables[0];

                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                DataTable dt = new DataTable();
                return dt;
            }
        }
         public async Task<int?> GenerateNextInvoiceNumber(int Type, int? YearId, int BranchId)
         {
            var invoices = _TaamerProContext.Invoices.Where(s => s.Type == Type && s.YearId == YearId  && s.IsDeleted == false);
            if (invoices != null)
            {
                var lastRow = invoices.OrderByDescending(u => u.InvoiceNumber).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    var last = Convert.ToInt32(lastRow.InvoiceNumber);
                    return last + 1;
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


        public async Task<List<GenerateNextVoucherNumberVM>> GenerateVoucherNumberNewPro(int Type, int? YearId, int BranchId, string codePrefix, bool InvoiceBranchSeparated,int Status, string Con)
        {
            try
            {
                List<GenerateNextVoucherNumberVM> lmd = new List<GenerateNextVoucherNumberVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GenerateNextVoucherNumberNew";
                        command.Connection = con;
                        command.Parameters.Add(new SqlParameter("@Type", Type));
                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        if(BranchId == 0)
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }
                        if (Status == 0)
                        {
                            command.Parameters.Add(new SqlParameter("@Status", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@Status", Status));
                        }
                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);

                        lmd.Add(new GenerateNextVoucherNumberVM
                        {
                            InvoiceNumber = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceNumber"]),
                            BranchId = Convert.ToInt32(ds.Tables[0].Rows[0]["BranchId"]),
                            Type = Convert.ToInt32(ds.Tables[0].Rows[0]["Type"]),
                            YearId = Convert.ToInt32(ds.Tables[0].Rows[0]["YearId"]),
                            NameAr = Convert.ToString(ds.Tables[0].Rows[0]["NameAr"]),
                            InvoiceStartCode = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceStartCode"]),
                            InvoiceBranchSeparated = Convert.ToBoolean(ds.Tables[0].Rows[0]["InvoiceBranchSeparated"]),
                            Newinvoicenumber = Convert.ToInt32(ds.Tables[0].Rows[0]["Newinvoicenumber"]),
                        });
                    }
                }

                return lmd;
            }
            catch (Exception ex)
            {
                List<GenerateNextVoucherNumberVM> lmd = new List<GenerateNextVoucherNumberVM>();
                return lmd;
            }
        }

        public async Task<int?> GenerateVoucherZatcaNumber( int? YearId, int BranchId)
        {
            var invoices = _TaamerProContext.Acc_InvoicesRequests.Where(s=>s.BranchId==BranchId);
            if (invoices != null)
            {
                var lastRow = invoices.OrderByDescending(u => u.InvoiceNoRequest).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    var last = Convert.ToInt32(lastRow.InvoiceNoRequest);
                    return last + 1;
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

        public async Task<int?> GenerateNextInvoiceNumberNotiCredit(int Type, int? YearId, int BranchId)
        {
            var invoices = _TaamerProContext.Invoices.Where(s => (s.Type == 29) && s.YearId == YearId && s.IsDeleted == false);
            if (invoices != null)
            {
                var lastRow = invoices.OrderByDescending(u => u.InvoiceNumber).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    var last = Convert.ToInt32(lastRow.InvoiceNumber);
                    return last + 1;
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
         public async Task<int?> GenerateNextInvoiceNumberNotiDepit(int Type, int? YearId, int BranchId)
        {
            var invoices = _TaamerProContext.Invoices.Where(s => (s.Type == 30) && s.YearId == YearId && s.IsDeleted == false);
            if (invoices != null)
            {
                var lastRow = invoices.OrderByDescending(u => u.InvoiceNumber).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    var last = Convert.ToInt32(lastRow.InvoiceNumber);
                    return last + 1;
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
         public async Task<int?> GenerateNextInvoiceNumberPurchaseNotiDepit(int Type, int? YearId, int BranchId)
        {
            var invoices = _TaamerProContext.Invoices.Where(s => (s.Type == 33) && s.YearId == YearId && s.IsDeleted == false);
            if (invoices != null)
            {
                var lastRow = invoices.OrderByDescending(u => u.InvoiceNumber).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    var last = Convert.ToInt32(lastRow.InvoiceNumber);
                    return last + 1;
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

         public async Task<int?> GenerateNextInvoiceNumberRet(int Type, int? YearId, int BranchId)
        {
            var invoices = _TaamerProContext.Invoices.Where(s => s.Type == Type && s.YearId == YearId && s.IsDeleted == false);
            if (invoices != null)
            {
                var lastRow = invoices.OrderByDescending(u => u.InvoiceRetId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    var last = Convert.ToInt32(lastRow.InvoiceRetId);
                    return last + 1;
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

         public async Task<int?> GenerateVoucherNumberOpening(int Type, int? YearId, int BranchId)
        {
            var invoices = _TaamerProContext.Invoices.Where(s => s.Type == Type && s.IsDeleted == false && s.BranchId== BranchId);
            if (invoices != null)
            {
                var lastRow = invoices.OrderByDescending(u => u.InvoiceNumber).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    var last = lastRow.InvoiceNumber;
                    return Convert.ToInt32(lastRow.InvoiceNumber) + 1;

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

         public async Task<int?> GenerateVoucherNumberClosing(int Type, int? YearId, int BranchId)
        {
            var invoices = _TaamerProContext.Invoices.Where(s => s.Type == Type && s.YearId == YearId && s.IsDeleted == false && s.IsPost==true);
            if (invoices != null)
            {
                var lastRow = invoices.OrderByDescending(u => u.InvoiceNumber).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    var last = lastRow.InvoiceNumber;
                    return Convert.ToInt32(lastRow.InvoiceNumber) + 1;
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

         public async Task<int> GetMaxId()
        {
            return _TaamerProContext.Invoices.Count() > 0 ? _TaamerProContext.Invoices.Max(s => s.InvoiceId) : 0;
        }
         public async Task<int> GetReceiptVoucherCount(int YearId, int BranchId)
        {
            return _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 6 && s.YearId == YearId && s.BranchId == BranchId).Count();
        }
         public async Task<int> GetExchangeVoucherCount(int YearId, int BranchId)
        {
            return _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 5 && s.YearId == YearId && s.BranchId == BranchId).Count();
        }
         public async Task<int> GetDailyVoucherCount(int YearId, int BranchId)
        {
            return _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 8 && s.YearId == YearId && s.BranchId == BranchId).Count();
        }
         public async Task<int> GetOpeningVoucherCount(int YearId, int BranchId)
        {
            return _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 10 && s.YearId == YearId && s.BranchId == BranchId).Count();
        }
         public async Task<int> GetOpeningBalanceCount(int YearId, int BranchId)
        {
            return _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 19 && s.YearId == YearId && s.BranchId == BranchId).Count();
        }
         public async Task<int> GetAllVouchersCount(int YearId, int BranchId)
        {
            return _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.YearId == YearId && s.BranchId == BranchId).Count();
        }

       public async Task<IEnumerable<InvoicesVM>> GetProjectManagerRevene(int? ManagerId, string dateFrom, string dateTo, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Invoices.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.ProjectId != null && s.YearId == YearId && s.Type == 6 && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                                               && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture) && (s.Project.MangerId == ManagerId || ManagerId == null)
                                                                               ).Select(x => new InvoicesVM
                                                                               {
                                                                                   InvoiceNumber = x.InvoiceNumber,
                                                                                   Type = x.Type,
                                                                                   IsPost = x.IsPost,
                                                                                   PostDate = x.PostDate,
                                                                                   PostHijriDate = x.PostHijriDate,
                                                                                   StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                                                                                   HijriDate = x.HijriDate,
                                                                                   Date = x.Date,
                                                                                   InvoiceReference = x.InvoiceReference,
                                                                                   Notes = x.Notes ?? "",
                                                                                   InvoiceNotes = x.InvoiceNotes ?? "",
                                                                                   TotalValue = x.TotalValue,
                                                                                   InvoiceValue = x.InvoiceValue,
                                                                                   TaxAmount = x.TaxAmount,
                                                                                   PayType = x.PayType,
                                                                                   ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                                                                               }).ToList().OrderBy(s => s.InvoiceNumber);

                return details;
            }
            catch (Exception)
            {
                var details = _TaamerProContext.Invoices.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.ProjectId != null && s.YearId == YearId && s.Type == 6 && (s.Project.MangerId == ManagerId || ManagerId == null)).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    Type = x.Type,
                    IsPost = x.IsPost,
                    PostDate = x.PostDate,
                    PostHijriDate = x.PostHijriDate,
                    StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
                    HijriDate = x.HijriDate,
                    Date = x.Date,
                    InvoiceReference = x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    InvoiceNotes = x.InvoiceNotes ?? "",
                    TotalValue = x.TotalValue,
                    InvoiceValue = x.InvoiceValue,
                    TaxAmount = x.TaxAmount,
                    PayType = x.PayType,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "بدون",
                }).ToList().OrderBy(s => s.InvoiceNumber);
                return details;
            }

        }
        
        public async Task<IEnumerable<ReturnInvoiceVM>> GetInvoiceReturnData_func(int InvoiceId, int YearId, int BranchId, string lang, string Con)
        {
            try
            {
                List<ReturnInvoiceVM> lmd = new List<ReturnInvoiceVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "InvoiceReturn";
                        command.Connection = con;
                        command.Parameters.Add(new SqlParameter("@InvoiceId", InvoiceId));
                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            int AccountId_V;
                            decimal Total_V, Credit_V, Depit_V;
                            try
                            {AccountId_V = int.Parse(dr[0].ToString());}
                            catch (Exception)
                            {AccountId_V = 0;}
                            try
                            {Total_V = decimal.Parse(dr[1].ToString());}
                            catch (Exception)
                            {Total_V = 0;}

                            if(Total_V>0)
                            {
                                Credit_V = Total_V; Depit_V = 0;
                            }
                            else
                            {
                                Credit_V = 0; Depit_V = Total_V * -1;
                            }

                            lmd.Add(new ReturnInvoiceVM
                            {
                                AccountId = AccountId_V,
                                Total = Total_V,
                                Credit= Credit_V,
                                Depit= Depit_V
                            });
                        }
                    }
                }
                return lmd;
            }
            catch
            {
                List<ReturnInvoiceVM> lmd = new List<ReturnInvoiceVM>();
                return lmd;
            }

        }

        public async Task<List<InvoicesVM>> GetFinancialfollowup(string Con, FinancialfollowupVM _financialfollowupVM)
        {
            try
            {
                List<InvoicesVM> lmd = new List<InvoicesVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Financialfollowup";
                        command.CommandTimeout = 0;
                        command.Connection = con;

                        //---------------------------------------------------------------------
                        if (_financialfollowupVM.UserId == 0 || _financialfollowupVM.UserId == null)
                            command.Parameters.Add(new SqlParameter("@UserId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@UserId", _financialfollowupVM.UserId));
                        //---------------------------------------------------------------------
                        //---------------------------------------------------------------------
                        if (_financialfollowupVM.BranchId == 0 || _financialfollowupVM.BranchId == null)
                            command.Parameters.Add(new SqlParameter("@BranchId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@BranchId", _financialfollowupVM.BranchId));
                        //---------------------------------------------------------------------
                        if (_financialfollowupVM.CustomerId == 0 || _financialfollowupVM.CustomerId == null)
                            command.Parameters.Add(new SqlParameter("@CustomerId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@CustomerId", _financialfollowupVM.CustomerId));
                        //---------------------------------------------------------------------
                        if (_financialfollowupVM.SupplierId == 0 || _financialfollowupVM.SupplierId == null)
                            command.Parameters.Add(new SqlParameter("@SupplierId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@SupplierId", _financialfollowupVM.SupplierId));
                        //---------------------------------------------------------------------
                        if (_financialfollowupVM.PayType == 0 || _financialfollowupVM.PayType == null)
                            command.Parameters.Add(new SqlParameter("@PayType", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@PayType", _financialfollowupVM.PayType));
                        //---------------------------------------------------------------------
                        if (_financialfollowupVM.YearId == 0 || _financialfollowupVM.YearId == null)
                            command.Parameters.Add(new SqlParameter("@YearId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@YearId", _financialfollowupVM.YearId));
                        //---------------------------------------------------------------------
                        if (_financialfollowupVM.startdate == "" || _financialfollowupVM.startdate == null)
                            command.Parameters.Add(new SqlParameter("@startdate", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@startdate", _financialfollowupVM.startdate));
                        //---------------------------------------------------------------------
                        if (_financialfollowupVM.enddate == "" || _financialfollowupVM.enddate == null)
                            command.Parameters.Add(new SqlParameter("@enddate", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@enddate", _financialfollowupVM.enddate));
                        //---------------------------------------------------------------------
                        if (_financialfollowupVM.TabType == 0 || _financialfollowupVM.TabType == null)
                            command.Parameters.Add(new SqlParameter("@TabType", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@TabType", _financialfollowupVM.TabType));
                       

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lmd.Add(new InvoicesVM
                            {
                                InvoiceId = Convert.ToInt32((dr["InvoiceId"]).ToString()),
                                InvoiceNumber = (dr["InvoiceNumber"]).ToString(),
                                Date = (dr["Date"]).ToString(),
                                InvoiceValue = Convert.ToDecimal((dr["InvoiceValue"]).ToString()),
                                TaxAmount = Convert.ToDecimal((dr["TaxAmount"]).ToString()),
                                TotalValue = Convert.ToDecimal((dr["TotalValue"]).ToString()),
                                PayTypeName = (dr["PayTypeName"]).ToString(),
                                StatusNameNew = (dr["StatusNameNew"]).ToString(),
                                PostDate = (dr["PostDate"]).ToString(),
                                CustomerName = (dr["Customer_SupName"]).ToString(),
                                ProjectNo = (dr["ProjectNo"]).ToString(),
                                ContractNo = (dr["ContractNo"]).ToString(),
                                AddUser = (dr["addusername"]).ToString(),
                                SupplierInvoiceNo = Convert.ToString((dr["SupplierInvoiceNo"]).ToString()),
                                ClauseName = (dr["ClauseName"]).ToString(),
                                RecevierTxt = (dr["RecevierTxt"]).ToString(),
                                IsPost = Convert.ToBoolean((dr["IsPost"]).ToString()),
                                FileUrl = (dr["FileUrl"]).ToString(),
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<InvoicesVM> lmd = new List<InvoicesVM>();
                return lmd;
            }

        }

        public async Task<List<InvoicesVM>> GetInvoiceByCustomer(int CustomerId,int YearId)
        {
            try
            {
                var inv = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.CustomerId == CustomerId && s.PayType == 8 && s.StoreId != 1 & s.YearId == YearId && s.Rad == false).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                });
                return inv.ToList().DistinctBy(z => z.InvoiceId).ToList(); 
            }
            catch (Exception ex)
            {
                List<InvoicesVM> lmd = new List<InvoicesVM>();
                return lmd;
            }

        }


        public async Task<InvoicesVM> GetInvoiceByNo(string voucherNo, int YearId)
        {
            try
            {
                var inv = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 2 && s.InvoiceNumber == voucherNo && s.YearId == YearId ).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    TotalValue=x.TotalValue,
                    PaidValue=x.PaidValue,
                    InvoiceValue=x.InvoiceValue,
                }).FirstOrDefault();
                return inv;
            }
            catch (Exception ex)
            {
                InvoicesVM lmd = new InvoicesVM();
                return lmd;
            }

        }



        public async Task<InvoicesVM> GetInvoiceByNo_purches(string voucherNo, int YearId)
        {
            try
            {
                var inv = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceNumber == voucherNo && s.YearId == YearId
                && s.Type== (int)VoucherType.Purches).Select(x => new InvoicesVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceId = x.InvoiceId,
                    TotalValue = x.TotalValue,
                    PaidValue = x.PaidValue,
                    InvoiceValue = x.InvoiceValue,
                }).FirstOrDefault();
                return inv;
            }
            catch (Exception ex)
            {
                InvoicesVM lmd = new InvoicesVM();
                return lmd;
            }

        }

    }
}
