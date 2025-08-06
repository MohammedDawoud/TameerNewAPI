using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using System.Data;
using static TaamerProject.Models.ReportGridVM;

namespace TaamerProject.Service.Interfaces
{
    public interface IVoucherService 
    {
        Task<IEnumerable<InvoicesVM>> GetAllVouchers(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);
        GeneralMessage SendWInvoice(int InvoiceId, int UserId, int BranchId, string AttachmentFile, string environmentURL,string fileTypeUpload);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersLastMonth(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersSearch(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersSearchCustomer(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);

        Task<InvoicesVM> GetVouchersSearchInvoiceByID(int InvoiceId, int BranchId, int? yearid);
        Task<InvoicesVM> GetVouchersSearchInvoicePurchaseByID(int InvoiceId, int BranchId, int? yearid);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersPurchase(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);

        Task<IEnumerable<InvoicesVM>> GetAllAlarmVoucher(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);
        Task<IEnumerable<InvoicesVM>> GetAllNotioucher(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersRetSearch(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersDelegate(int BranchId, int? yearid);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersRetSearchPurchase(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);


        Task<IEnumerable<InvoicesVM>> GetAllVouchersQR(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersProject(int BranchId, int? yearid);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersRet(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersRetPurchase(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);
        Task<IEnumerable<InvoicesVM>> GetAllCreditDepitNotiReport(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);

        Task<IEnumerable<InvoicesVM>> GetAllVouchersRetReport(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersRetReport_Pur(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);


        Task<IEnumerable<InvoicesVM>> GetAllPayVouchersRet(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);


        Task<IEnumerable<InvoicesVM>> GetCustRevenueExpensesDetails(string FromDate, string ToDate, int BranchId, int? yearid);
        Task<IEnumerable<InvoicesVM>> GetVoucherRpt( int BranchId, int? yearid);
        Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByVoucherId(int? voucherId);
        Task<VoucherDetailsVM> GetInvoiceIDByProjectID(int? ProjectId);

        Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByInvoiceId(int? voucherId);
        Task<VoucherDetailsVM> GetAllDetailsByVoucherDetailsId(int? VoucherDetailsId);

        Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByInvoiceIdFirstOrDef(int? voucherId);

        Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByInvoiceIdPurchase(int? voucherId);


        Task<IEnumerable<VoucherDetailsVM>> GetAllTransByLineNo(int LineNo);
        Task<IEnumerable<VoucherDetailsVM>> GetAllTrans(int VouDetailsID);
        GeneralMessage Issuing_invoice(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);

        GeneralMessage SaveVoucher(Invoices voucher,int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SaveandPostVoucher(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);

         GeneralMessage SaveVoucherP(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
         GeneralMessage  SaveVoucherPUpdateImage(int InvoiceId, int UserId, int BranchId, int? yearid,string FileName,string FileUrl); 
         Task<Invoices> MaxVoucherP(int BranchId, int? yearid);
        GeneralMessage SaveandPostVoucherP(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);

        GeneralMessage UpdateVoucher(int InvoiceId, int UserId, int BranchId);
        //GeneralMessage UpdateVoucherZatca(int InvoiceId, string InvoiceHash, string SingedXML, string EncodedInvoice, string ZatcaUUID, string QRCode, string PIH, string SingedXMLFileName,int zatcaVoucherNumber, int UserId, int BranchId);

        GeneralMessage UpdateVoucherDraft(int InvoiceId, int UserId, int BranchId, int yearid, string Con);

        GeneralMessage UpdateVoucher_payed(int InvoiceId, int UserId, int BranchId);
        GeneralMessage UpdateVoucher_payed_by(string SupplierInvoiceNo, int UserId, int BranchId, int YearId);
        decimal? VousherRe_Sum(int InvoiceId);

        GeneralMessage SaveConvertVoucher(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        Task<int?> GenerateVoucherNumber(int Type, int BranchId, int? yearid);
        Task<string?> GenerateVoucherNumberNewPro(int Type, int BranchId, int? yearid,int Status, string Con);

        Task<int?> GenerateVoucherZatcaNumber( int BranchId, int? yearid);

        Task<int?> GenerateVoucherNumberOpening(int Type, int BranchId, int? yearid);

        Task<int?> GenerateVoucherNumberClosing(int Type, int BranchId, int? yearid);
        GeneralMessage SaveDailyVoucher(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SaveandPostDailyVoucher(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);

        GeneralMessage SaveClosingVoucher(Invoices voucher, int UserId, int BranchId, int? maxVoucherNo, int? yearid, string Con);

        GeneralMessage SaveEmpVoucher(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SaveRecycleVoucher(int YearID, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SaveRecycleReturnVoucher(int YearID, int UserId, int BranchId, int? yearid, string Con);


        GeneralMessage SaveDailyVoucher2(List<Transactions> voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SaveOpeningVoucher(Invoices voucher, int UserId, int BranchId,int? maxVoucherNo, int? yearid, string Con);
        GeneralMessage SaveandPostOpeningVoucher(Invoices voucher, int UserId, int BranchId, int? maxVoucherNo, int? yearid, string Con);

        GeneralMessage SaveInvoice(Invoices voucher, int UserId, int BranchId,int? yearid, string Con);
        GeneralMessage SaveInvoiceForServices(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SaveInvoiceForServicesDraft(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);

        GeneralMessage SaveInvoiceForServicesNoti(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SaveInvoiceForServicesNotiDepit(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);


        GeneralMessage SaveandPostInvoiceForServices(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);

        GeneralMessage SaveInvoiceForServicesRet(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SaveInvoiceForServicesRetNEW_func(Invoices voucher, int UserId, int BranchId, int? yearid, string lang, string Con);

        GeneralMessage ReturnNotiCreditBack(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage ReturnNotiDepitBack(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);

        GeneralMessage SaveInvoiceForServicesRet_Back(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);


        GeneralMessage SavePurchaseForServices(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SavePurchaseForServicesNotiDepit(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);

        GeneralMessage SaveandPostPurchaseForServices(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SaveandPostPurchaseOrderForServices(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage ConverOrderToInvoice(int voucherId, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SavePurchaseForServicesRet(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SavePurchaseForServicesRetNEW_func(Invoices voucher, int UserId, int BranchId, int? yearid, string lang, string Con);

        GeneralMessage SavePayVoucherForServicesRet(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);


        GeneralMessage SaveInvoiceForServices2(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);
        GeneralMessage SaveandPostInvoiceForServices2(Invoices voucher, int UserId, int BranchId, int? yearid, string Con);

        Task<IEnumerable<TransactionsVM>> GetAllTransByVoucherId(int? voucherId);
         GeneralMessage PostVouchers(List<Invoices> PostedList, int UserId,int BranchId, int? yearid);
         GeneralMessage PostVouchersCustody(List<Custody> PostedList, int UserId, int BranchId, int? yearid);

         GeneralMessage PostVouchersCheckBox(List<Int32> voucherIds, int UserId, int BranchId, int? yearid);

        GeneralMessage PostBackVouchers(List<Invoices> PostedList, int UserId, int BranchId, int? yearid);
        GeneralMessage PostBackVouchersCustody(List<Custody> PostedList, int UserId, int BranchId, int? yearid);

        GeneralMessage CancelPostVouchers(List<Invoices> PostedList, int UserId, int BranchId, int? yearid);
        GeneralMessage DeleteVoucher(int VoucherId, int UserId, int BranchId);
        GeneralMessage SaveVoucherAlarmDate(int VoucherId,string VoucherAlarmDate, int UserId, int BranchId);
        GeneralMessage SaveVueDate(int VoucherId, string VueDate, int UserId, int BranchId);

        Task<InvoicesVM> GetVoucherById(int VoucherId);
        Task<InvoicesVM> GetInvoiceDateById(int VoucherId);

        Invoices GetVoucherByIdNotiDepit_Purchase(int VoucherIdNotiDepit);

        Task<IEnumerable<InvoicesVM>> GetVoucherByIdNoti(int VoucherId);

        Task<InvoicesVM> GetVoucherByIdPurchase(int VoucherId);

        Task<Invoices> GetInvoicesById(int VoucherId);
        //heba
        Task<DataTable> ReceiptCashingPaying(int VoucherId,string Con);
        Task<DataTable> ReceiptCashingPayingNoti(int VoucherId, string Con);
        Task<DataTable> ReceiptCashingPayingNotiDepit(int VoucherId, string Con);
        Task<DataTable> ReceiptCashingPayingNotiDepitPurchase(int VoucherId, string Con);

        Task<DataTable> DailyVoucherReport(int VoucherId, string Con);

        Task<DataTable> OpeningVoucherReport(int VoucherId, string Con);

        //IEnumerable<TransactionsVM> GetAllJournals(int? FromJournal, int? ToJournal, string FromDate, string ToDate,  int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvID(int? invId, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDPurchase(int? invId, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDPurchaseOrder(int? invId, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByReVoucherID(int? invId, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByPayVoucherID(int? invId, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByDailyID(int? invId, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByDailyID_Custody(int? invId, int BranchId, int? yearid);

        Task<IEnumerable<TransactionsVM>> GetAllJournalsByClosingID(int? invId, int BranchId, int? yearid);

        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDRet(int? invId, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDCreditDepitNoti(int? invId, int BranchId, int? yearid);

        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDRetPurchase(int? invId, int BranchId, int? yearid);

        Task<IEnumerable<TransactionsVM>> GetAllPayJournalsByInvIDRet(int? invId, int BranchId, int? yearid);

        Task<IEnumerable<TransactionsVM>> GetAllJournals(int? FromJournal, int? ToJournal, string FromDate, string ToDate, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllTotalJournals(int? FromJournal, int? ToJournal, string FromDate, string ToDate, int BranchId, int? yearid);
        Task<IEnumerable<InvoicesVM>> GetProjectManagerRevene(int? ManagerId, string dateFrom, string dateTo, int BranchId, int? yearid);
        //GeneralMessage UpdateInvoiceWithZatcaData(Invoices voucher, int UserId, int BranchId);
        Task<IEnumerable<InvoicesVM>> GetAllVouchersback();

        Task<IEnumerable<InvoicesVM>> GetAllVouchersfromcontractSearch(VoucherFilterVM voucherFilterVM, int BranchId, int? yearid);

        Task<List<InvoicesVM>> GetFinancialfollowup(string Con, FinancialfollowupVM _financialfollowupVM);

        GeneralMessage UpdateVoucherRecepient(string InvoiceId, int UserId, int BranchId, int YearId);
        Task<List<InvoicesVM>> GetInvoiceByCustomer(int CustomerId, int YearId);

        Task<InvoicesVM> GetInvoiceByNo(string VocherNo, int YearId);
        Task<InvoicesVM> GetInvoiceByNo_purches(string VocherNo, int YearId);
        decimal? PayVousher_Sum(int InvoiceId);
    }
}
