using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaamerProject.Models.ReportGridVM;

namespace TaamerProject.Repository.Interfaces
{
    public interface IInvoicesRepository
    {
        Task<IEnumerable<InvoicesVM>> GetAllVouchers(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersLastMonth(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersSearch(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersSearchCustomer(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);

        Task<InvoicesVM> GetVouchersSearchInvoiceByID(int InvoiceId, int YearId, int BranchId);
        Task<InvoicesVM> GetVouchersSearchInvoicePurchaseByID(int InvoiceId, int YearId, int BranchId);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersPurchase(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersProject( int YearId, int BranchId);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersRet(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersRetPurchase(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllCreditDepitNotiReport(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllCreditDepitNotiReport_Pur(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersRetReport(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersRetReport_Pur(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);

        Task<IEnumerable<InvoicesVM>> GetAllPayVouchersRet(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);


        Task<IEnumerable<InvoicesVM>> GetCustRevenueExpensesDetails(string FromDate, string ToDate, int YearId, int BranchId);
        Task<Invoices> MaxVoucherP(int BranchId, int? yearid);
        Task<IEnumerable<InvoicesVM>> GetVoucherRpt(int YearId,int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersBySearch(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersBySearchPurchase(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);

        Task<IEnumerable<InvoicesVM>> GetAllAlarmVoucher(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllNotioucher(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersRetSearch(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersDelegate(int YearId, int BranchId);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersRetSearchPurchase(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);


        Task<IEnumerable<InvoicesVM>> GetAllVouchersBySearchQR(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersBySearchRet(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);

        Task<InvoicesVM> GetVoucherById(int VoucherId);
        Task<InvoicesVM> GetInvoiceDateById(int VoucherId);

        Task<IEnumerable<InvoicesVM>> GetVoucherByIdNoti(int VoucherId);

        Task<InvoicesVM> GetVoucherByIdPurchase(int VoucherId);

        Task<Invoices> GetInvoicesById(int VoucherId);
        Task<InvoicesVM> GetInvById(int VoucherId);

        //heba 
        Task<DataTable> ReceiptCashingPaying(int VoucherId, string Con);
        Task<DataTable> ReceiptCashingPayingNoti(int VoucherId, string Con);
        Task<DataTable> ReceiptCashingPayingNotiDepit(int VoucherId, string Con);
        Task<DataTable> ReceiptCashingPayingNotiDepitPurchase(int VoucherId, string Con);

        Task<DataTable> DailyVoucherReport(int VoucherId, string Con);

        Task<DataTable> OpeningVoucherReport(int VoucherId, string Con);
        
        Task<IEnumerable<InvoicesVM>> GetProjectManagerRevene(int? ManagerId, string dateFrom, string dateTo, int YearId, int BranchId);
        Task<int?> GenerateNextInvoiceNumber(int Type, int? YearId, int BranchId);
        Task<List<GenerateNextVoucherNumberVM>> GenerateVoucherNumberNewPro(int Type, int? YearId, int BranchId, string codePrefix, bool InvoiceBranchSeparated,int Status, string Con);


        Task<int?> GenerateVoucherZatcaNumber(int? YearId, int BranchId);

        Task<int?> GenerateNextInvoiceNumberNotiCredit(int Type, int? YearId, int BranchId);
        Task<int?> GenerateNextInvoiceNumberNotiDepit(int Type, int? YearId, int BranchId);
        Task<int?> GenerateNextInvoiceNumberPurchaseNotiDepit(int Type, int? YearId, int BranchId);

        Task<int?> GenerateNextInvoiceNumberRet(int Type, int? YearId, int BranchId);

        Task<int?> GenerateVoucherNumberOpening(int Type, int? YearId, int BranchId);
        Task<int?> GenerateVoucherNumberClosing(int Type, int? YearId, int BranchId);
        Task<int> GetMaxId();
        Task<int> GetReceiptVoucherCount(int YearId, int BranchId);
        Task<int> GetExchangeVoucherCount(int YearId, int BranchId);
        Task<int> GetDailyVoucherCount(int YearId, int BranchId);
        Task<int> GetOpeningVoucherCount(int YearId, int BranchId);
        Task<int> GetOpeningBalanceCount(int YearId, int BranchId);
        Task<int> GetAllVouchersCount(int YearId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllVoucherstoback();

        Task<IEnumerable<InvoicesVM>> GetAllVouchersfromcontractSearch(VoucherFilterVM voucherFilterVM, int YearId, int BranchId);
        Task<IEnumerable<ReturnInvoiceVM>> GetInvoiceReturnData_func(int InvoiceId,int YearId, int BranchId, string lang, string Con);
        Task<List<InvoicesVM>> GetFinancialfollowup(string Con, FinancialfollowupVM _financialfollowupVM);

        Task<List<InvoicesVM>> GetInvoiceByCustomer(int CustomerId, int YearId);
        Task<InvoicesVM> GetInvoiceByNo(string voucherNo, int YearId);
        Task<InvoicesVM> GetInvoiceByNo_purches(string voucherNo, int YearId);


    }
}
