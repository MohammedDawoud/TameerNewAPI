using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ReportGridVM
    {

        public class FinancialfollowupVM
        {
            public int? UserId { get; set; }
            public int? BranchId { get; set; }
            public int? CustomerId { get; set; }
            public int? SupplierId { get; set; }
            public int? PayType { get; set; }
            public int? YearId { get; set; }
            public string? startdate { get; set; }
            public string? enddate { get; set; }
            public int? TabType { get; set; }

            public string? SearchText { get; set; }
            public int? Page { get; set; } = 1;
            public int? PageSize { get; set; } = 10;

        }


        public List<TransactionsVM>? TransactionVM { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? RasedBefore { get; set; }
        public decimal? Result { get; set; }
        public string? AccountName { get; set; }
        public string? AccountCode { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
        public List<InvoicesVM>? InvoicesVM { get; set; }
        public string? Totalres { get; set; }
        public string? ResultCre { get; set; }

        public string? ResultDep { get; set; }
        public string? BranchName { get; set; }
    }

    public class ValueNeededVM
    {
        public List<double>? Result { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? rob3 { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
        public string? BranchName { get; set; }

    }

    public class RevenuReportVM
    {
        public List<DetailedRevenuVM>? Result { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? custName { get; set; }
        public string? ProjectNo { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
        public string? BranchName { get; set; }
    }
    public class ContractReportVM
    {
        public List<ContractsVM>? someContracts { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? ManagerName { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
    }
    public class DetailedExpensesdtVMReportVM
    {
        public List<DetailedExpenseVM>? Result { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? custName { get; set; }
        public string? ProjectNo { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
        public string? BranchName { get; set; }
    }
    public class CostCenterEX_REVMReportVM
    {
        public List<CostCenterEX_REVM>? Result { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? custName { get; set; }
        public string? ProjectNo { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
        public string? BranchName { get; set; }
    }
    public class ManagerRevenueReport
    {
        public List<GeneralmanagerRevVM>? result { get; set; }
        public List<GeneralBudgetVM>? result2 { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? ManagerName { get; set; }
        public string? ProjectNo { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
        public string? BranchName { get; set; }
    }

    public class TrialBalance_PDFVM
    {
        public List<TrainBalanceVM>? TrainBalanceVM_VD { get; set; }
        public BranchesVM? Branch_VD { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? filtertypename { get; set; }
        public string? costCenterNam { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
        public bool? OrgIsRequired_VD { get; set; }
        public string? TempCheck { get; set; }

        public string[]? InfoDoneTasksReport { get; set; }

    }

    public class ContractsPrintVM
    {
        public List<AccountVM>? result { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
    }
    public class CustomerFinancialPrintVM
    {
        public List<TrainBalanceVM>? result { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
        public string? BranchName { get; set; }

    }
    public class CostCentersPrintVM
    {
        public List<CostCentersVM>? result { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
        public string? CostCenterName { get; set; }
        public string? BranchName { get; set; }


    }
    public class ReportIncomeLevelsVM
    {
        public List<IncomeStatmentVMWithLevels>? result { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public OrganizationsVM? Org_VD { get; set; }
        public string? DateTimeNow { get; set; }
        public string? Filtertxt { get; set; }
        public string? BranchName { get; set; }
    }

    public class OfferReportVM
    {

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }

        public string? TaxAmount { get; set; }

        public CustomerVM? Customer { get; set; }

        public List<OfferServiceVM>? Offerservce { get; set; }
        public List<AccServicesPricesOfferVM>? ServicesPricesOffer { get; set; }


        public List<CustomerPaymentsVM>? payment { get; set; }

        public List<OffersConditionsVM>? offercondition { get; set; }

        public OffersPricesVM? offers { get; set; }
        public string? DateTimeNow { get; set; }

        public string? offersvaltxt { get; set; }
        public OrganizationsVM? Org_VD { get; set; }

        public BranchesVM? Branch_VD { get; set; }

        public bool? OrgIsRequired_VD { get; set; }


    }
    public class EntryVoucherReportVM
    {
        public OrganizationsVM? Org_VD { get; set; }
        public BranchesVM? Branch { get; set; }
        public string? DateTimeNow { get; set; }
        public string? NumString { get; set; }
        public List<EntryVoucherVMDatatable>? InvoicesVM { get; set; }

    }
    public class VoucherReportVM
    {
        public OrganizationsVM? Org_VD { get; set; }
        public BranchesVM? Branch { get; set; }
        public string? DateTimeNow { get; set; }
        public string? NumString { get; set; }
        public List<VoucherVMDatatable>? VoucherVM { get; set; }

    }
    public class TreeReportVM
    {
        public OrganizationsVM? Org_VD { get; set; }
        public BranchesVM? Branch { get; set; }
        public string? DateTimeNow { get; set; }
        public List<TreeVMDatatable>? TreeViewVM { get; set; }

    }
    public class VoucherVMDatatable
    {
        public string? InvoiceNumber { get; set; }
        public string? JoNo { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public string? CustomerName { get; set; }
        public string? InvoiceValueText { get; set; }
        public string? TotalValue { get; set; }
        public string? RecevierTxt { get; set; }
        public string? PayType { get; set; }
        public string? MoneyOrderDate { get; set; }
        public string? CheckDate { get; set; }
        public string? BankName { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string? CostCenterName { get; set; }
        public string? FullName { get; set; }
        public string? SupplierName { get; set; }
        public string? ToInvoiceId { get; set; }

    }

    public class TreeVMDatatable
    {
        public string? AccountCode { get; set; }
        public string? AccountLevel { get; set; }
        public string? Level1 { get; set; }
        public string? Level2 { get; set; }
        public string? Level3 { get; set; }
        public string? Level4 { get; set; }
        public string? Level5 { get; set; }
        public string? Level6 { get; set; }
        public string? Level7 { get; set; }
    }

    public class EntryVoucherVMDatatable
    {
        public string? InvoiceNumber { get; set; }
        public string? InvoiceDate { get; set; }
        public string? InvoiceHijriDate { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountNameAr { get; set; }
        public string? Depit { get; set; }
        public string? Credit { get; set; }
        public string? Notes { get; set; }
        public string? CostCenterNameAr { get; set; }

    }
    public class SupervisionReportVM
    {
        public OrganizationsVM? Org_VD { get; set; }
        public BranchesVM? Branch { get; set; }
        public string? DateTimeNow { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public string? day { get; set; }
        public SupervisionsVM? Sup { get; set; }
        public string? TextStringTemp { get; set; }
        public string? TextStringTempEnd { get; set; }
        public Pro_Super_PhasesVM? Phase { get; set; }
        public string? StampUrl { get; set; }

        public List<Pro_SupervisionDetailsVM>? Supervision { get; set; }

    }
    public class PhasesReportVM
    {
        public OrganizationsVM? Org_VD { get; set; }
        public BranchesVM? Branch { get; set; }
        public string? DateTimeNow { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public string? day { get; set; }
        public string? TextStringTemp { get; set; }
        public string? StampUrl { get; set; }
        public List<ProjectPhasesTasksVM>? phases { get; set; }

    }



    public class Services_PriceWithDetails
    {
        public Acc_Services_Price? services_price { get; set; }
        public List<Acc_Services_Price>? details { get; set; }
    }
    public class PrivList
    {
        public int? UserId { get; set; }
        public int? GroupId { get; set; }
        public List<int>? PrivIds { get; set; }

    }

}


