using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using System.Globalization;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class ContractsRepository : IContractRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ContractsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public Contracts GetById(int contractid)
        {
            return _TaamerProContext.Contracts.Where(x => x.ContractId == contractid).FirstOrDefault();
        }
        public async Task<IEnumerable<ContractsVM>> GetAllContracts()
        {
            var contracts = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false).Select(x => new ContractsVM
            {
                ContractId = x.ContractId,
                ContractNo = x.ContractNo,
                BranchId = x.BranchId,
                Date = x.Date,
                HijriDate=x.HijriDate,
                ValueText=x.ValueText,
                CustomerId = x.CustomerId,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project!.ProjectNo??"",
                ProjectTypeId = x.Project!.ProjectTypeId ?? null,
                Value =x.Value,
                TaxesValue=x.TaxesValue ?? 0,
                AttachmentUrl=x.AttachmentUrl,
                TaxType = x.TaxType,
                CustomerName_W = x.Customer!.CustomerNameAr??"",
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr,
                CustomerMobile = x.Customer.CustomerMobile??"",

                ProjectName = x.Project.ProjectNo,
                TotalValue=x.TotalValue,
                TotalValuetxt=x.TotalValuetxt,
                OrgId = x.OrgId,
                UpdateUser=x.UpdateUser??0,
                OrgEmpId = x.OrgEmpId,
                OrgEmpJobId = x.OrgEmpJobId,
                ServiceId = x.ServiceId,
                AttachmentUrlExtra=x.AttachmentUrlExtra??"",
                TotalRemainingPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == false && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalPaidPayment = x.CustomerPayments.Where(t => t.IsDeleted == false && t.IsPaid == true && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalServiceCount = x.ContractServices!.Where(t => t.IsDeleted == false).Count(),
                Oper_expeValue=x.Oper_expeValue??0,


            }).ToList();
                return contracts;
        }


        public async Task<IEnumerable<ContractsVM>> GetContractById(int contractid)
        {
            var contracts = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.ContractId == contractid).Select(x => new ContractsVM
            {
                ContractId = x.ContractId,
                ContractNo = x.ContractNo,
                BranchId = x.BranchId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ValueText = x.ValueText,
                CustomerId = x.CustomerId,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project!.ProjectNo ?? "",
                ProjectTypeId = x.Project!.ProjectTypeId ?? null,
                Value = x.Value,
                TaxesValue = x.TaxesValue ?? 0,
                AttachmentUrl = x.AttachmentUrl,
                TaxType = x.TaxType,
                CustomerName_W = x.Customer!.CustomerNameAr ?? "",
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr,
                CustomerMobile = x.Customer.CustomerMobile ?? "",

                Type = x.Type,
                ProjectName = x.Project.ProjectNo,
                TotalValue = x.TotalValue,
                TotalValuetxt = x.TotalValuetxt,
                OrgId = x.OrgId,
                UpdateUser = x.UpdateUser ?? 0,
                OrgEmpId = x.OrgEmpId,
                OrgEmpJobId = x.OrgEmpJobId,
                ServiceId = x.ServiceId,
                AttachmentUrlExtra = x.AttachmentUrlExtra ?? "",
                TotalRemainingPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == false && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalPaidPayment = x.CustomerPayments.Where(t => t.IsDeleted == false && t.IsPaid == true && t.IsCanceled != true).Sum(t => t.TotalAmount),
                Appr_LetterDate_Des=x.Appr_LetterDate_Des,
                Cons_TotalFees_Des=x.Cons_TotalFees_Des,
                ContDate_Des=x.ContDate_Des,
                ContPeriod_Des=x.ContPeriod_Des,
                ContractDurCommit_Des=x.ContractDurCommit_Des,
                Engineering_License=x.Engineering_License,
                ContractorName_Des=x.ContractorName_Des,
                Engineering_LicenseDate=x.Engineering_LicenseDate,
                EngServ_OfferDate_Des=x.EngServ_OfferDate_Des,
                TeamWork_Note10_Des=x.TeamWork_Note10_Des,
                TeamWork_Note1_Des=x.TeamWork_Note1_Des,
                TeamWork_Note2_Des=x.TeamWork_Note2_Des,
                TeamWork_Note3_Des=x.TeamWork_Note3_Des,
                TeamWork_Note4_Des=x.TeamWork_Note4_Des,
                TeamWork_Note5_Des=x.TeamWork_Note5_Des,
                TeamWork_Note6_Des=x.TeamWork_Note6_Des,
                TeamWork_Note7_Des=x.TeamWork_Note7_Des,
                 TeamWork_Note8_Des=x.TeamWork_Note8_Des,
                 TeamWork_Note9_Des=x.TeamWork_Note9_Des,
                 TeamWork_Num10_Des=x.TeamWork_Num10_Des,
                  TeamWork_Num1_Des=x.TeamWork_Num1_Des,
                  TeamWork_Num2_Des=x.TeamWork_Num2_Des,
                  TeamWork_Num3_Des=x.TeamWork_Num3_Des,
                  TeamWork_Num4_Des=x.TeamWork_Num4_Des,
                  TeamWork_Num5_Des=x.TeamWork_Num5_Des,
                  TeamWork_Num6_Des=x.TeamWork_Num6_Des,
                TeamWork_Num7_Des = x.TeamWork_Num7_Des,
                TeamWork_Num8_Des = x.TeamWork_Num8_Des,
                TeamWork_Num9_Des = x.TeamWork_Num9_Des,
                MaxPay_Des=x.MaxPay_Des,
               ProjBriefDesc_Des=x.ProjBriefDesc_Des,
                Oper_expeValue = x.Oper_expeValue ?? 0,
                FirstProjectDate = x.Project!=null? x.Project.FirstProjectDate:"",
                FirstProjectExpireDate = x.Project != null ? x.Project.FirstProjectExpireDate : "",
                StopProjectType = x.Project != null ? x.Project.StopProjectType ?? 0:0,


            }).ToList();
            return contracts;
        }

        public async Task<IEnumerable<ContractsVM>> GetAllContracts_B(int BranchId,int yearid)
        {
            var contracts = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.BranchId==BranchId).Select(x => new ContractsVM
            {
                ContractId = x.ContractId,
                ContractNo = x.ContractNo,
                BranchId = x.BranchId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ValueText = x.ValueText,
                CustomerId = x.CustomerId,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project!.ProjectNo ?? "",
                ProjectTypeId = x.Project!.ProjectTypeId ?? null,
                Value = x.Value,
                TaxesValue = x.TaxesValue ?? 0,
                AttachmentUrl = x.AttachmentUrl,
                TaxType = x.TaxType,
                CustomerName_W = x.Customer!.CustomerNameAr ?? "",
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr,
                CustomerMobile = x.Customer.CustomerMobile ?? "",

                Type = x.Type,
                ProjectName = x.Project.ProjectNo,
                TotalValue = x.TotalValue,
                TotalValuetxt = x.TotalValuetxt,
                OrgId = x.OrgId,
                UpdateUser = x.UpdateUser ?? 0,

                OrgEmpId = x.OrgEmpId,
                OrgEmpJobId = x.OrgEmpJobId,
                ServiceId = x.ServiceId,
                AttachmentUrlExtra = x.AttachmentUrlExtra ?? "",
                 ProjectDescription=x.Project.ProjectDescription??"",

                TotalRemainingPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == false && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalPaidPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == true && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalServiceCount = x.ContractServices!.Where(t => t.IsDeleted == false).Count(),
                Oper_expeValue = x.Oper_expeValue ?? 0,
                FirstProjectDate = x.Project != null ? x.Project.FirstProjectDate : "",
                FirstProjectExpireDate = x.Project != null ? x.Project.FirstProjectExpireDate : "",
                StopProjectType = x.Project != null ? x.Project.StopProjectType ?? 0 : 0,

            }).ToList();
            return contracts;
        }


        public async Task<IEnumerable<ContractsVM>> GetAllContracts_BSearch(ContractsVM contractsVM, int BranchId, int yearid)
        {
            var contracts = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.Project.MangerId == contractsVM.ManagerId || contractsVM.ManagerId == null)).Select(x => new ContractsVM
            {
                ContractId = x.ContractId,
                ContractNo = x.ContractNo,
                BranchId = x.BranchId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ValueText = x.ValueText,
                CustomerId = x.CustomerId,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project!.ProjectNo ?? "",
                ProjectTypeId = x.Project!.ProjectTypeId ?? null,
                Value = x.Value,
                TaxesValue = x.TaxesValue ?? 0,
                AttachmentUrl = x.AttachmentUrl,
                TaxType = x.TaxType,
                CustomerName_W = x.Customer!.CustomerNameAr ?? "",
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr,
                CustomerMobile = x.Customer.CustomerMobile ?? "",

                Type = x.Type,
                ProjectName = x.Project.ProjectNo,
                TotalValue = x.TotalValue,
                TotalValuetxt = x.TotalValuetxt,
                OrgId = x.OrgId,
                UpdateUser = x.UpdateUser ?? 0,

                OrgEmpId = x.OrgEmpId,
                OrgEmpJobId = x.OrgEmpJobId,
                ServiceId = x.ServiceId,
                AttachmentUrlExtra = x.AttachmentUrlExtra ?? "",
                ProjectDescription = x.Project.ProjectDescription ?? "",

                TotalRemainingPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == false && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalPaidPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == true && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalServiceCount = x.ContractServices!.Where(t => t.IsDeleted == false).Count(),
                Oper_expeValue = x.Oper_expeValue ?? 0,
                FirstProjectDate = x.Project != null ? x.Project.FirstProjectDate : "",
                FirstProjectExpireDate = x.Project != null ? x.Project.FirstProjectExpireDate : "",
                StopProjectType = x.Project != null ? x.Project.StopProjectType ?? 0 : 0,

            }).ToList();
            return contracts;
        }
        public async Task<IEnumerable<ContractsVM>> GetAllContracts_BSearchCustomer(ContractsVM contractsVM, int BranchId, int yearid)
        {
            var contracts = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.Project.CustomerId == contractsVM.CustomerId || contractsVM.CustomerId == null)).Select(x => new ContractsVM
            {
                ContractId = x.ContractId,
                ContractNo = x.ContractNo,
                BranchId = x.BranchId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ValueText = x.ValueText,
                CustomerId = x.CustomerId,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project!.ProjectNo ?? "",
                ProjectTypeId = x.Project!.ProjectTypeId ?? null,
                Value = x.Value,
                TaxesValue = x.TaxesValue ?? 0,
                AttachmentUrl = x.AttachmentUrl,
                TaxType = x.TaxType,
                CustomerName_W = x.Customer!.CustomerNameAr ?? "",
                CustomerName = x.Customer!.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr,
                CustomerMobile = x.Customer.CustomerMobile ?? "",

                Type = x.Type,
                ProjectName = x.Project.ProjectNo,
                TotalValue = x.TotalValue,
                TotalValuetxt = x.TotalValuetxt,
                OrgId = x.OrgId,
                UpdateUser = x.UpdateUser ?? 0,

                OrgEmpId = x.OrgEmpId,
                OrgEmpJobId = x.OrgEmpJobId,
                ServiceId = x.ServiceId,
                AttachmentUrlExtra = x.AttachmentUrlExtra ?? "",
                ProjectDescription = x.Project.ProjectDescription ?? "",

                TotalRemainingPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == false && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalPaidPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == true && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalServiceCount = x.ContractServices!.Where(t => t.IsDeleted == false).Count(),
                Oper_expeValue = x.Oper_expeValue ?? 0,
                FirstProjectDate = x.Project != null ? x.Project.FirstProjectDate : "",
                FirstProjectExpireDate = x.Project != null ? x.Project.FirstProjectExpireDate : "",
                StopProjectType = x.Project != null ? x.Project.StopProjectType ?? 0 : 0,

            }).ToList();
            return contracts;
        }



        public async Task<IEnumerable<ContractsVM>> GetAllContractsBySearch(ContractsVM contractsVM, int BranchId, int yearid)
        {
            //DateTime dtFrom = Convert.ToDateTime(DateTime.ParseExact(contractsVM.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            //DateTime dtTo = Convert.ToDateTime(DateTime.ParseExact(contractsVM.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            //DateTime dtTo = Convert.ToDateTime(contractsVM.dateTo);
            var contracts = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.BranchId == BranchId&&(s.Project.MangerId== contractsVM.ManagerId || contractsVM.ManagerId==null)).Select(x => new ContractsVM
            {
                ContractId = x.ContractId,
                ContractNo = x.ContractNo,
                BranchId = x.BranchId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ValueText = x.ValueText,
                CustomerId = x.CustomerId,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project!.ProjectNo ?? "",
                ProjectTypeId = x.Project!.ProjectTypeId ?? null,
                Value = x.Value,
                TaxesValue = x.TaxesValue ?? 0,
                AttachmentUrl = x.AttachmentUrl,
                TaxType = x.TaxType,
                CustomerName_W = x.Customer!.CustomerNameAr ?? "",
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr,
                CustomerMobile = x.Customer.CustomerMobile ?? "",

                Type = x.Type,
                ProjectName = x.Project.ProjectNo,
                TotalValue = x.TotalValue,
                TotalValuetxt = x.TotalValuetxt,
                OrgId = x.OrgId,
                UpdateUser = x.UpdateUser ?? 0,

                OrgEmpId = x.OrgEmpId,
                OrgEmpJobId = x.OrgEmpJobId,
                ServiceId = x.ServiceId,
                AttachmentUrlExtra = x.AttachmentUrlExtra ?? "",
                ProjectDescription = x.Project.ProjectDescription ?? "",

                TotalRemainingPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == false && t.IsCanceled!=true).Sum(t => t.TotalAmount),
                TotalPaidPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == true && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalServiceCount = x.ContractServices!.Where(t => t.IsDeleted == false).Count(),
                Oper_expeValue = x.Oper_expeValue ?? 0,
                FirstProjectDate = x.Project != null ? x.Project.FirstProjectDate : "",
                FirstProjectExpireDate = x.Project != null ? x.Project.FirstProjectExpireDate : "",
                StopProjectType = x.Project != null ? x.Project.StopProjectType ?? 0 : 0,

            }).ToList().Where(s => (DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(contractsVM.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || contractsVM.dateFrom == null)
                                                                              && (DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(contractsVM.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture) || contractsVM.dateTo == null));
            return contracts;
        }
        public async Task<IEnumerable<ContractsVM>> GetAllContractsBySearchCustomer(ContractsVM contractsVM, int BranchId, int yearid)
        {
            //DateTime dtFrom = Convert.ToDateTime(DateTime.ParseExact(contractsVM.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            //DateTime dtTo = Convert.ToDateTime(DateTime.ParseExact(contractsVM.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            //DateTime dtTo = Convert.ToDateTime(contractsVM.dateTo);
            var contracts = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.Project.CustomerId == contractsVM.CustomerId || contractsVM.CustomerId == null)).Select(x => new ContractsVM
            {
                ContractId = x.ContractId,
                ContractNo = x.ContractNo,
                BranchId = x.BranchId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ValueText = x.ValueText,
                CustomerId = x.CustomerId,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project!.ProjectNo ?? "",
                ProjectTypeId = x.Project!.ProjectTypeId ?? null,
                Value = x.Value,
                TaxesValue = x.TaxesValue ?? 0,
                AttachmentUrl = x.AttachmentUrl,
                TaxType = x.TaxType,
                CustomerName_W = x.Customer!.CustomerNameAr ?? "",
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr,
                CustomerMobile = x.Customer.CustomerMobile ?? "",

                Type = x.Type,
                ProjectName = x.Project.ProjectNo,
                TotalValue = x.TotalValue,
                TotalValuetxt = x.TotalValuetxt,
                OrgId = x.OrgId,
                UpdateUser = x.UpdateUser ?? 0,

                OrgEmpId = x.OrgEmpId,
                OrgEmpJobId = x.OrgEmpJobId,
                ServiceId = x.ServiceId,
                AttachmentUrlExtra = x.AttachmentUrlExtra ?? "",
                ProjectDescription = x.Project.ProjectDescription ?? "",

                TotalRemainingPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == false && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalPaidPayment = x.CustomerPayments!.Where(t => t.IsDeleted == false && t.IsPaid == true && t.IsCanceled != true).Sum(t => t.TotalAmount),
                TotalServiceCount = x.ContractServices!.Where(t => t.IsDeleted == false).Count(),
                Oper_expeValue = x.Oper_expeValue ?? 0,
                FirstProjectDate = x.Project != null ? x.Project.FirstProjectDate : "",
                FirstProjectExpireDate = x.Project != null ? x.Project.FirstProjectExpireDate : "",
                StopProjectType = x.Project != null ? x.Project.StopProjectType ?? 0 : 0,

            }).ToList().Where(s => (DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(contractsVM.dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) || contractsVM.dateFrom == null)
                                                                              && (DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(contractsVM.dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture) || contractsVM.dateTo == null));
            return contracts;
        }

        public async Task<IEnumerable<ContractsVM>> GetAllCustHaveRemainingMoney(CustomerVM CustomersSearch, string lang, int BranchId)     
        {
            var contracts = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CustomerPayments.Where(t => t.IsDeleted == false && t.IsPaid == false && t.IsCanceled != true).Sum(t => t.Amount) > 0 && s.ProjectId != null && s.CustomerId != null && (s.Customer.CustomerNameAr == CustomersSearch.CustomerNameAr || s.Customer.CustomerNameAr.Contains(CustomersSearch.CustomerNameAr) || CustomersSearch.CustomerNameAr == null) &&
                                                (s.Project.ProjectNo == CustomersSearch.ProjectNo || CustomersSearch.ProjectNo == null) &&
                                                (s.Customer.CustomerMobile == CustomersSearch.CustomerMobile || s.Customer.CustomerMobile.Contains(CustomersSearch.CustomerMobile) || CustomersSearch.CustomerMobile == null)).Select(x => new ContractsVM
            {
                ContractNo = x.ContractNo,
                CustomerMobile = x.Customer.CustomerMobile,
                                                    CustomerName_W = x.Customer.CustomerNameAr,
                                                    CustomerName = x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameAr + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr,

                                                    ProjectNo = x.Project.ProjectNo,
                ProjectTypeName = x.Project.projecttype.NameAr,
                TotalRemainingPayment = x.CustomerPayments.Where(t => t.IsDeleted == false && t.IsPaid == false && t.IsCanceled != true).Sum(t => t.TotalAmount),
            }).ToList();
            return contracts;
        }
        public async Task<int> GetMaxId()
        {
            return (_TaamerProContext.Contracts.Count() > 0) ? _TaamerProContext.Contracts.Max(s => s.ContractId) : 0;
        }
        public async Task<int> GenerateNextContractNumber(int BranchId, string codePrefix)
        {
            if (_TaamerProContext.Contracts.ToList().Count() > 0)
            {
                var lastRow = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.Type==1).OrderByDescending(u => u.ContractId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {
                        var ContractNumber =0;

                        if(codePrefix=="")
                        {
                            ContractNumber = int.Parse(lastRow.ContractNo) + 1;
                        }
                        else
                        {

                            //int Num = int.Parse(lastRow.ContractNo.Replace(codePrefix, "").Trim());
                            ContractNumber = int.Parse(lastRow.ContractNo.Replace(codePrefix, "").Trim()) + 1;
                        }

                        return ContractNumber;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
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
        public async Task<int> GenerateNextContractNumber2(int BranchId, string codePrefix)
        {
            if (_TaamerProContext.Contracts.ToList().Count() > 0)
            {
                var lastRow = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.Type == 2).OrderByDescending(u => u.ContractId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {
                        var ContractNumber = 0;

                        if (codePrefix == "")
                        {
                            ContractNumber = int.Parse(lastRow.ContractNo) + 1;
                        }
                        else
                        {

                            //int Num = int.Parse(lastRow.ContractNo.Replace(codePrefix, "").Trim());
                            ContractNumber = int.Parse(lastRow.ContractNo.Replace(codePrefix, "").Trim()) + 1;
                        }

                        return ContractNumber;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
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



        public async Task<int> GenerateNextContractNumber3(int BranchId, string codePrefix)
        {
            if (_TaamerProContext.Contracts.ToList().Count()>0)
            {
                var lastRow = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.Type == 3).OrderByDescending(u => u.ContractId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {
                        var ContractNumber = 0;

                        if (codePrefix == "")
                        {
                            ContractNumber = int.Parse(lastRow.ContractNo) + 1;
                        }
                        else
                        {

                            //int Num = int.Parse(lastRow.ContractNo.Replace(codePrefix, "").Trim());
                            ContractNumber = int.Parse(lastRow.ContractNo.Replace(codePrefix, "").Trim()) + 1;
                        }

                        return ContractNumber;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
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

        public async Task<int> GenerateNextContractNumber4(int BranchId, string codePrefix)
        {
            if (_TaamerProContext.Contracts.ToList().Count() > 0)
            {
                var lastRow = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.Type == 4).OrderByDescending(u => u.ContractId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {
                        var ContractNumber = 0;

                        if (codePrefix == "")
                        {
                            ContractNumber = int.Parse(lastRow.ContractNo) + 1;
                        }
                        else
                        {

                            //int Num = int.Parse(lastRow.ContractNo.Replace(codePrefix, "").Trim());
                            ContractNumber = int.Parse(lastRow.ContractNo.Replace(codePrefix, "").Trim()) + 1;
                        }

                        return ContractNumber;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
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

        public async Task<int> GenerateNextContractNumber5(int BranchId, string codePrefix)
        {
            if (_TaamerProContext.Contracts.ToList().Count() > 0)
            {
                var lastRow = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.Type == 5).OrderByDescending(u => u.ContractId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {
                        var ContractNumber = 0;

                        if (codePrefix == "")
                        {
                            ContractNumber = int.Parse(lastRow.ContractNo) + 1;
                        }
                        else
                        {

                            //int Num = int.Parse(lastRow.ContractNo.Replace(codePrefix, "").Trim());
                            ContractNumber = int.Parse(lastRow.ContractNo.Replace(codePrefix, "").Trim()) + 1;
                        }

                        return ContractNumber;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
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

    }
}


