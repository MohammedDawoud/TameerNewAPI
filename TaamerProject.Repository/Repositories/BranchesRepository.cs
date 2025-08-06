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
    public class BranchesRepository :IBranchesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public BranchesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public Branch GetById(int BranchId)
        {
            return _TaamerProContext.Branch.Where(x => x.BranchId == BranchId).FirstOrDefault();
        }
        public async Task<IEnumerable<BranchesVM>> GetAllBranches( string lang)
        {
            var branches = _TaamerProContext.Branch.Where(s => s.IsDeleted == false).Select(x => new BranchesVM
            {  
                BranchId=x.BranchId,
                Code=x.Code,
                BranchName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr =x.NameAr,
                NameEn=x.NameEn,
                BranchManager = x.BranchManager,
                Phone = x.Phone,
                Mobile = x.Mobile,
                WarehouseId = x.WarehouseId,
                CurrencyId = x.CurrencyId,
                BoxAccId = x.BoxAccId,
                StockAccd = x.StockAccd,
                SaleCostAccId = x.SaleCostAccId,
                SaleCashAccId = x.SaleCashAccId,
                SaleDelayAccId = x.SaleDelayAccId,
                SaleDiscountAccId = x.SaleDiscountAccId,
                SaleReturnCashAccId = x.SaleReturnCashAccId,
                SaleReturnDelayAccId = x.SaleReturnDelayAccId,
                SaleReturnDiscountAccId = x.SaleReturnDiscountAccId,
                PurchaseCashAccId = x.PurchaseCashAccId,
                PurchaseDelayAccId = x.PurchaseDelayAccId,
                PurchaseApprovalAccId = x.PurchaseApprovalAccId,
                PurchaseOutCashAccId = x.PurchaseOutCashAccId,
                PurchaseOutDelayAccId = x.PurchaseOutDelayAccId,
                PurchaseDiscAccId = x.PurchaseDiscAccId,
                PurchaseReturnCashAccId = x.PurchaseReturnCashAccId,
                PurchaseReturnDelayAccId = x.PurchaseReturnDelayAccId,
                PurchaseReturnApprovAccId = x.PurchaseReturnApprovAccId,
                PurchaseReturnDiscAccId = x.PurchaseReturnDiscAccId,
                RevenuesAccountId = x.RevenuesAccountId,
                SuspendedFundAccId = x.SuspendedFundAccId,
                CashInvoicesAccId = x.CashInvoicesAccId,
                DelayInvoicesAccId = x.DelayInvoicesAccId,
                DiscountInvoicesAccId = x.DiscountInvoicesAccId,
                 CashReturnInvoicesAccId = x.CashReturnInvoicesAccId,
                DelayReturnInvoicesAccId = x.DelayReturnInvoicesAccId,
                DiscountReturnInvoiceAccId = x.DiscountReturnInvoiceAccId,
                CheckInvoicesAccId = x.CheckInvoicesAccId,
                VisaInvoicesAccId = x.VisaInvoicesAccId,
                TeleInvoicesAccId = x.TeleInvoicesAccId,
                AmericanAccId = x.AmericanAccId,
                CustomersAccId = x.CustomersAccId,
                SuppliersAccId = x.SuppliersAccId,
                EmployeesAccId = x.EmployeesAccId,
                GuaranteeAccId = x.GuaranteeAccId,
                ContractsAccId = x.ContractsAccId,
                TaxsAccId = x.TaxsAccId,
                EngineeringLicense = x.EngineeringLicense,
                LabLicense = x.LabLicense,
                Mailbox = x.Mailbox,
                CityId = x.CityId,
                LastExport = x.LastExport,
                LastExportInner = x.LastExportInner,
                CityName = x.City.NameAr,
                CurrencyName = x.Currency.CurrencyNameAr,
                IsActive = x.IsActive,
                OrganizationId=x.OrganizationId,
                LoanAccId=x.LoanAccId,
                BoxAccId2 = x.BoxAccId2,
                AccountBank = x.AccountBank ?? "",
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                BuildingNumber = x.BuildingNumber,
                StreetName = x.StreetName,
                Address = x.Address,
                TaxCode = x.TaxCode,
                AccountBank2 = x.AccountBank2 ?? "",
                PostalCode=x.PostalCode,
                ProjectStartCode=x.ProjectStartCode ??"",
                OfferStartCode = x.OfferStartCode ?? "",
                TaskStartCode = x.TaskStartCode ?? "",
                OrderStartCode = x.OrderStartCode ?? "",
                InvoiceStartCode = x.InvoiceStartCode ?? "",
                InvoiceBranchSeparated = x.InvoiceBranchSeparated ?? false,
                Engineering_License = x.Engineering_License??"",
                Engineering_LicenseDate=x.Engineering_LicenseDate??"",
                BankId=x.BankId,
                BankId2=x.BankId2,
                BankIdImgURL = x.Bank != null ? x.Bank.BanckLogo : "",
                BankId2ImgURL = x.Bank2 != null ? x.Bank2.BanckLogo : "",
                IsPrintInvoice=x.IsPrintInvoice??false,
                BranchLogoUrl=x.BranchLogoUrl??"",
                HeaderLogoUrl = x.HeaderLogoUrl ?? "",
                FooterLogoUrl = x.FooterLogoUrl ?? "",
                headerPrintInvoice=x.headerPrintInvoice??false,
                headerPrintpayvoucher=x.headerPrintpayvoucher??false,
                headerprintdarvoucher=x.headerprintdarvoucher??false,
                headerPrintrevoucher=x.headerPrintrevoucher??false,
                headerPrintcontract = x.headerPrintcontract ?? false,
                BublicRevenue = x.BublicRevenue??0,
                OtherRevenue=x.OtherRevenue??0,
                CSR = x.CSR ?? "",
                PrivateKey = x.PrivateKey ?? "",
                PublicKey = x.PublicKey ?? "",
                SecreteKey = x.SecreteKey ?? "",
            });
            return branches;
        }
        public async Task<IEnumerable<BranchesVM>> FillBranchSelectNew(string lang)
        {
            var branches = _TaamerProContext.Branch.Where(s => s.IsDeleted == false).Select(x => new BranchesVM
            {
                BranchId = x.BranchId,
                Code = x.Code,
                BranchName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            });
            return branches;
        }

        public async Task<IEnumerable<BranchesVM>> GetBranchByBranchId(string lang, int BranchId)
        {
            var branches = _TaamerProContext.Branch.Where(s => s.IsDeleted == false && s.BranchId==BranchId).Select(x => new BranchesVM
            {
                BranchId = x.BranchId,
                Code = x.Code,
                BranchName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                BranchManager = x.BranchManager,
                Phone = x.Phone,
                Mobile = x.Mobile,
                WarehouseId = x.WarehouseId,
                CurrencyId = x.CurrencyId,
                BoxAccId = x.BoxAccId,
                StockAccd = x.StockAccd,
                SaleCostAccId = x.SaleCostAccId,
                SaleCashAccId = x.SaleCashAccId,
                SaleDelayAccId = x.SaleDelayAccId,
                SaleDiscountAccId = x.SaleDiscountAccId,
                SaleReturnCashAccId = x.SaleReturnCashAccId,
                SaleReturnDelayAccId = x.SaleReturnDelayAccId,
                SaleReturnDiscountAccId = x.SaleReturnDiscountAccId,
                PurchaseCashAccId = x.PurchaseCashAccId,
                PurchaseDelayAccId = x.PurchaseDelayAccId,
                PurchaseApprovalAccId = x.PurchaseApprovalAccId,
                PurchaseOutCashAccId = x.PurchaseOutCashAccId,
                PurchaseOutDelayAccId = x.PurchaseOutDelayAccId,
                PurchaseDiscAccId = x.PurchaseDiscAccId,
                PurchaseReturnCashAccId = x.PurchaseReturnCashAccId,
                PurchaseReturnDelayAccId = x.PurchaseReturnDelayAccId,
                PurchaseReturnApprovAccId = x.PurchaseReturnApprovAccId,
                PurchaseReturnDiscAccId = x.PurchaseReturnDiscAccId,
                RevenuesAccountId = x.RevenuesAccountId,
                SuspendedFundAccId = x.SuspendedFundAccId,
                CashInvoicesAccId = x.CashInvoicesAccId,
                DelayInvoicesAccId = x.DelayInvoicesAccId,
                DiscountInvoicesAccId = x.DiscountInvoicesAccId,
                CashReturnInvoicesAccId = x.CashReturnInvoicesAccId,
                DelayReturnInvoicesAccId = x.DelayReturnInvoicesAccId,
                DiscountReturnInvoiceAccId = x.DiscountReturnInvoiceAccId,
                CheckInvoicesAccId = x.CheckInvoicesAccId,
                VisaInvoicesAccId = x.VisaInvoicesAccId,
                TeleInvoicesAccId = x.TeleInvoicesAccId,
                AmericanAccId = x.AmericanAccId,
                CustomersAccId = x.CustomersAccId,
                SuppliersAccId = x.SuppliersAccId,
                EmployeesAccId = x.EmployeesAccId,
                GuaranteeAccId = x.GuaranteeAccId,
                ContractsAccId = x.ContractsAccId,
                TaxsAccId = x.TaxsAccId,
                EngineeringLicense = x.EngineeringLicense,
                LabLicense = x.LabLicense,
                Mailbox = x.Mailbox,
                CityId = x.CityId,
                LastExport = x.LastExport,
                LastExportInner = x.LastExportInner,
                CityName = x.City.NameAr,
                CurrencyName = x.Currency.CurrencyNameAr,
                IsActive = x.IsActive,
                OrganizationId = x.OrganizationId,
                LoanAccId = x.LoanAccId,
                BoxAccId2 = x.BoxAccId2,
                AccountBank = x.AccountBank ?? "",
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                BuildingNumber = x.BuildingNumber,
                StreetName = x.StreetName,
                Address = x.Address,
                TaxCode = x.TaxCode,
                AccountBank2 = x.AccountBank2 ?? "",
                PostalCode = x.PostalCode,
                ProjectStartCode = x.ProjectStartCode ?? "",
                OfferStartCode = x.OfferStartCode ?? "",
                TaskStartCode = x.TaskStartCode ?? "",
                OrderStartCode = x.OrderStartCode ?? "",
                InvoiceStartCode = x.InvoiceStartCode ?? "",
                InvoiceBranchSeparated = x.InvoiceBranchSeparated ?? false,
                Engineering_License = x.Engineering_License ?? "",
                Engineering_LicenseDate = x.Engineering_LicenseDate ?? "",
                BankId=x.BankId,
                BankId2=x.BankId2,
                BankIdImgURL = x.Bank != null ? x.Bank.BanckLogo : "",
                BankId2ImgURL = x.Bank2 != null ? x.Bank2.BanckLogo : "",
                IsPrintInvoice = x.IsPrintInvoice ?? false,
                BranchLogoUrl = x.BranchLogoUrl ?? "",
                HeaderLogoUrl=x.HeaderLogoUrl ??"",
                FooterLogoUrl=x.FooterLogoUrl ??"",
                headerPrintInvoice = x.headerPrintInvoice ?? false,
                headerPrintpayvoucher = x.headerPrintpayvoucher ?? false,
                headerprintdarvoucher = x.headerprintdarvoucher ?? false,
                headerPrintrevoucher = x.headerPrintrevoucher ?? false,
                headerPrintcontract = x.headerPrintcontract ?? false,              
                BublicRevenue = x.BublicRevenue ?? 0,
                OtherRevenue = x.OtherRevenue ?? 0,
                CSR = x.CSR ?? "",
                PrivateKey = x.PrivateKey ?? "",
                PublicKey = x.PublicKey ?? "",
                SecreteKey = x.SecreteKey ?? "",
            });
            return branches;
        }
        public async Task<BranchesVM> GetBranchByBranchIdCheck(string lang, int BranchId)
        {
            var branches = _TaamerProContext.Branch.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new BranchesVM
            {
                BranchId = x.BranchId,
                Code = x.Code,
                BranchName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                TaxCode=x.TaxCode,
            }).FirstOrDefault();
            return branches;
        }

        public async Task<int> GenerateNextBranchNumber()
        {
            if (_TaamerProContext.Branch != null)
            {
                //var lastRow = _TaamerProContext.Branch.Where(s => s.IsDeleted == false).OrderByDescending(u => u.Code).Take(1).FirstOrDefault();
                var lastRow = _TaamerProContext.Branch.Where(s => s.IsDeleted == false).OrderByDescending(u => u.BranchId).Take(1).FirstOrDefault();

                if (lastRow != null)
                {
                    return int.Parse(lastRow.Code) + 1;
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
        public async Task<int> GetOrganizationId(int BranchId)
        {
            if (_TaamerProContext.Branch != null)
            {
                int orgId = _TaamerProContext.Branch.Where(s => s.IsDeleted == false && s.BranchId==BranchId).Select(s=>s.OrganizationId).FirstOrDefault();
                if (orgId >0)
                {
                    return orgId;
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


        public async Task<IEnumerable<BranchesVM>> GetActiveBranch()
        {
            var ActivateBranch = _TaamerProContext.Branch.Where(s => s.IsDeleted == false && s.IsActive == true).Select(x => new BranchesVM
            {
                BranchId = x.BranchId,
                BranchName = x.NameAr
            });
            return ActivateBranch;
        }
        public async Task<IEnumerable<BranchesVM>> GetBranchById(int branchId, string Lang)
        {
            var Branch = _TaamerProContext.Branch.Where(s => s.IsDeleted == false && s.BranchId == branchId).Select(x => new BranchesVM
            {
                BranchId = x.BranchId,
                BranchName = Lang == "ltr" ? x.NameEn : x.NameAr
            }).ToList();
            return Branch;
        }
        public async Task<BranchesVM> GetCashBoxViaBranchID(int BranchId)
        {

            var branches = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).Select(x => new BranchesVM
            {
                BoxAccId = x.BoxAccId
            }).FirstOrDefault();

            return branches;
        }

        
    }
}
