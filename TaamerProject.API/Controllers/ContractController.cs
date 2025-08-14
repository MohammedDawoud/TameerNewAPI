using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Paragraph = Spire.Doc.Documents.Paragraph;
using TaamerProject.Models.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using static TaamerProject.API.Controllers.VoucherController;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class ContractController : ControllerBase
    {
            private IContractService _contractservice;
            private ICustomerService _customerservice;
            private IBranchesService _branchservice;
            private IOrganizationsService _organizationservice;
            private ICustomerPaymentsService _customerPaymentservice;
            private IVoucherService _voucherService;
            private IAccountsService _accountsService;
            private ICustomerPaymentsService _customerPaymentsservice;
            private IBranchesService _branchesService;
            private IEmployeesService _employeeService;
            private IServicesPriceService _servicesPriceService;
            private IDraftService _draftService;
            private IProjectService _projectService;
            private IContractDetailsService _contractDetailsService;
            private IDraftDetailsService _draftDetailsService;
            private IContractStageService _ContractStage;
            private IContractServicesService _contractService;
            private IUsersService _usersService;
            private readonly IFiscalyearsService _FiscalyearsService;
            private readonly IDrafts_TemplatesService _drafts_TemplatesService;
            private ITransactionsService _transactionsService;
        private readonly IServicesPriceOfferService _servicesPriceOfferService;


        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string Con;
        private byte[] ReportPDF;
            public ContractController(IContractService contractService, ICustomerService customerService, IBranchesService branchesService, IOrganizationsService organizationsService,
                ICustomerPaymentsService customerPaymentsService, IVoucherService voucherService, IAccountsService accountsService, ICustomerPaymentsService customerPaymentsService1,
                IBranchesService branchesService1, IEmployeesService employeesService, IServicesPriceService servicesPriceService, IDraftService draftService, IProjectService projectService,
               IContractDetailsService contractDetailsService, IDraftDetailsService draftDetailsService, IContractStageService contractStageService, IContractServicesService contractServicesService,
               IUsersService usersService, IFiscalyearsService fiscalyearsService, IDrafts_TemplatesService drafts_TemplatesService, ITransactionsService transactionsService
                , IServicesPriceOfferService servicesPriceOfferService, IConfiguration _configuration, IWebHostEnvironment webHostEnvironment)
            {
                this._accountsService = accountsService;
                this._contractservice = contractService;
                this._organizationservice = organizationsService;
                this._branchservice = branchesService;
                this._customerservice = customerService;
                this._customerPaymentservice = customerPaymentsService;
                this._voucherService = voucherService;
                this._customerPaymentsservice = customerPaymentsService1;
                this._branchesService = branchesService1;
                this._usersService = usersService;
                _employeeService = employeesService;
                _servicesPriceService = servicesPriceService;
                _draftService = draftService;
                _projectService = projectService;
                _contractDetailsService = contractDetailsService;
                _draftDetailsService = draftDetailsService;
                _ContractStage = contractStageService;
                _contractService = contractServicesService;
                this._FiscalyearsService = fiscalyearsService;
                _drafts_TemplatesService = drafts_TemplatesService;
                _transactionsService = transactionsService;
            _servicesPriceOfferService = servicesPriceOfferService;

            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext;

            _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;

        }

        [HttpGet("GetAllContracts")]
        public IActionResult GetAllContracts()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someContracts = _contractservice.GetAllContracts_B(_globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

                return Ok(someContracts);
            }
        [HttpGet("GetAllContractsNotPaid")]
        public IActionResult GetAllContractsNotPaid(bool FirstLoad)
            {
                List<ContractsVM> list = new List<ContractsVM>();
                if (FirstLoad == true)
                {
                    return Ok(list);
                }

                
                HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                var someContracts = _contractservice.GetAllContracts_B(_globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList().Where(s => s.TotalRemainingPayment > 0).ToList();
                return Ok(someContracts);
            }

        [HttpGet("GetAllContractsNotPaidCustomer")]
        public IActionResult GetAllContractsNotPaidCustomer(bool FirstLoad)
            {
                List<ContractsVM> list = new List<ContractsVM>();
                if (FirstLoad == true)
                {
                    return Ok(list);
                }
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                VoucherFilterVM voucherFilterVM = new VoucherFilterVM();
                voucherFilterVM.Type = 2;
                voucherFilterVM.CustomerId = null;

                VoucherFilterVM voucherFilterVM2 = new VoucherFilterVM();
                voucherFilterVM2.Type = 29;
                voucherFilterVM2.CustomerId = null;

            var someVoucher = _voucherService.GetAllVouchersSearchCustomer(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
                var someVoucherCredit = _voucherService.GetAllVouchersSearchCustomer(voucherFilterVM2, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

                var result = someVoucher
                    .GroupBy(s => new { s.CustomerId, s.CustomerMobile, s.CustomerName, s.CustomerName_W })
                    .Select(cl => new ContractsVM
                    {
                        CustomerId = cl.First().CustomerId,
                        CustomerName = cl.First().CustomerName ?? "",
                        CustomerName_W = cl.First().CustomerName_W ?? "",
                        CustomerMobile = cl.First().CustomerMobile ?? "",
                        TotalValue = cl.Sum(c => c.TotalValue),
                    }).ToList();
                foreach (var inv in result)
                {
                    decimal TotalPaidPaymentResult = 0;
                    decimal resultReVoucher = 0;
                    decimal resultPayVoucher = 0;
                    decimal resultInvoicePayVoucherCreditPaid = 0;
                    decimal resultEntryVoucher = 0;
                    decimal resultInvoicePayVoucher = 0;
                    decimal resultInvoicePayVoucherCredit = 0;

                //var resultReVoucher = _voucherService.VousherRe_Sum(inv.InvoiceId);
                var AccountResult = _customerservice.GetCustomersByCustomerId(inv.CustomerId ?? 0, _globalshared.Lang_G).Result;
                    if (AccountResult != null)
                    {
                        var Allresult = _transactionsService.GetAllTransByAccountId(AccountResult.AccountId, "", "", _globalshared.YearId_G).Result;

                        resultReVoucher = Allresult.Where(s => s.Type == 6 && s.IsPost == true).Sum(t => t.Credit) ?? 0;
                        resultPayVoucher = Allresult.Where(s => s.Type == 5 && s.IsPost == true).Sum(t => t.Depit) ?? 0;
                        resultInvoicePayVoucherCreditPaid = someVoucherCredit.Where(s => s.Type == 29 && s.IsPost == true && s.CustomerId == inv.CustomerId).Sum(t => t.PaidValue) ?? 0;

                        resultEntryVoucher = Allresult.Where(s => s.Type == 8 && s.IsPost == true).Sum(t => (t.Credit - t.Depit)) ?? 0;
                        resultInvoicePayVoucher = someVoucher.Where(s => s.Type == 2 && s.IsPost == true && s.CustomerId == inv.CustomerId).Sum(t => t.PaidValue) ?? 0;

                        var PayVoucherCredit = Allresult.Where(s => s.Type == 29 && s.IsPost == true).ToList();
                        foreach (var item in PayVoucherCredit)
                        {
                            var NotiCredit = _voucherService.GetVoucherById(item.InvoiceId ?? 0).Result;
                            if (NotiCredit != null)
                            {
                                var InvoiceNotiCredit = _voucherService.GetVoucherById(NotiCredit.CreditNotiId ?? 0).Result;

                                if (InvoiceNotiCredit != null)
                                {
                                    if (InvoiceNotiCredit.PayType == 8 && InvoiceNotiCredit.Rad != true)
                                    {
                                        resultInvoicePayVoucherCredit += item.Credit ?? 0;
                                }
                            }
                            }
                        }

                    }
                    else
                    {
                        resultReVoucher = 0;
                        resultPayVoucher = 0;
                        resultEntryVoucher = 0;
                        resultInvoicePayVoucher = 0;
                        resultInvoicePayVoucherCredit = 0;
                        resultInvoicePayVoucherCreditPaid = 0;
                    }


                    TotalPaidPaymentResult = (resultReVoucher) + (resultEntryVoucher) + (resultInvoicePayVoucher)-(resultPayVoucher) - (resultInvoicePayVoucherCreditPaid);
                    list.Add(new ContractsVM
                    {
                        //ContractNo = "",
                        //Date = inv.Date,
                        TotalValue = inv.TotalValue - resultInvoicePayVoucherCredit,
                        CustomerName = inv.CustomerName ?? "",
                        CustomerName_W = inv.CustomerName_W ?? "",
                        CustomerMobile = inv.CustomerMobile ?? "",
                        //ProjectDescription="",
                        //ProjectName = inv.ProjectNo,
                        TotalPaidPayment = TotalPaidPaymentResult,
                        TotalRemainingPayment = ((inv.TotalValue - resultInvoicePayVoucherCredit) - TotalPaidPaymentResult),

                    });
                }
                IEnumerable<ContractsVM> InvoicesList = list;
                //var Vouchers = someContracts.Union(InvoicesList);
                var Vouchers = InvoicesList;

                return Ok(Vouchers);
            }

        [HttpPost("GetAllContractsBySearch")]
        public IActionResult GetAllContractsBySearch(ContractsVM contractsVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someContracts = _contractservice.GetAllContractsBySearch(contractsVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            return Ok(someContracts);
        }

    [HttpPost("GetAllContractsBySearchCustomer")]
    public IActionResult GetAllContractsBySearchCustomer(ContractsVM contractsVM)
        {
            List<ContractsVM> list = new List<ContractsVM>();
                
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            VoucherFilterVM voucherFilterVM = new VoucherFilterVM();
            voucherFilterVM.Type = 2;
            voucherFilterVM.CustomerId = contractsVM.CustomerId;
            voucherFilterVM.dateFrom = contractsVM.dateFrom;
            voucherFilterVM.dateTo = contractsVM.dateTo;

            VoucherFilterVM voucherFilterVM2 = new VoucherFilterVM();
            voucherFilterVM2.Type = 29;
            voucherFilterVM2.CustomerId = contractsVM.CustomerId;
            voucherFilterVM2.dateFrom = contractsVM.dateFrom;
            voucherFilterVM2.dateTo = contractsVM.dateTo;

            var someVoucher = _voucherService.GetAllVouchersSearchCustomer(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
        var someVoucherCredit = _voucherService.GetAllVouchersSearchCustomer(voucherFilterVM2, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

        var result = someVoucher
                .GroupBy(s => new { s.CustomerId, s.CustomerMobile, s.CustomerName, s.CustomerName_W })
                .Select(cl => new ContractsVM
                {
                    CustomerId = cl.First().CustomerId,
                    CustomerName = cl.First().CustomerName ?? "",
                    CustomerName_W = cl.First().CustomerName_W ?? "",
                    CustomerMobile = cl.First().CustomerMobile ?? "",
                    TotalValue = cl.Sum(c => c.TotalValue),
                }).ToList();
            foreach (var inv in result)
            {
                decimal TotalPaidPaymentResult = 0;
                decimal resultReVoucher = 0;
                decimal resultEntryVoucher = 0;
                decimal resultInvoicePayVoucher = 0;
                decimal resultInvoicePayVoucherCredit = 0;
                decimal resultPayVoucher = 0;
                decimal resultInvoicePayVoucherCreditPaid = 0;


            //var resultReVoucher = _voucherService.VousherRe_Sum(inv.InvoiceId);
            var AccountResult = _customerservice.GetCustomersByCustomerId(inv.CustomerId ?? 0, _globalshared.Lang_G).Result;
                if (AccountResult != null)
                {

                    var Allresult = _transactionsService.GetAllTransByAccountId(AccountResult.AccountId, "", "", _globalshared.YearId_G).Result;
                    resultReVoucher = Allresult.Where(s => s.Type == 6 && s.IsPost == true).Sum(t => t.Credit) ?? 0;
                    resultEntryVoucher = Allresult.Where(s => s.Type == 8 && s.IsPost == true).Sum(t => (t.Credit - t.Depit)) ?? 0;
                    resultInvoicePayVoucher = someVoucher.Where(s => s.Type == 2 && s.IsPost == true && s.CustomerId == inv.CustomerId).Sum(t => t.PaidValue) ?? 0;
                    resultPayVoucher = Allresult.Where(s => s.Type == 5 && s.IsPost == true).Sum(t => t.Depit) ?? 0;
                    resultInvoicePayVoucherCreditPaid = someVoucherCredit.Where(s => s.Type == 29 && s.IsPost == true && s.CustomerId == inv.CustomerId).Sum(t => t.PaidValue) ?? 0;
                    var PayVoucherCredit = Allresult.Where(s => s.Type == 29 && s.IsPost == true).ToList();
                    foreach (var item in PayVoucherCredit)
                    {
                        var NotiCredit = _voucherService.GetVoucherById(item.InvoiceId ?? 0).Result;
                        if (NotiCredit != null)
                        {
                            var InvoiceNotiCredit = _voucherService.GetVoucherById(NotiCredit.CreditNotiId ?? 0).Result;

                            if (InvoiceNotiCredit != null)
                            {
                                if (InvoiceNotiCredit.PayType == 8 && InvoiceNotiCredit.Rad != true)
                                {
                                    resultInvoicePayVoucherCredit += item.Credit ?? 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    resultReVoucher = 0;
                    resultEntryVoucher = 0;
                    resultInvoicePayVoucher = 0;
                    resultInvoicePayVoucherCredit = 0;
                    resultPayVoucher = 0;
                    resultInvoicePayVoucherCreditPaid = 0;
                }


                TotalPaidPaymentResult = (resultReVoucher) + (resultEntryVoucher) + (resultInvoicePayVoucher) - (resultPayVoucher) - (resultInvoicePayVoucherCreditPaid);
            list.Add(new ContractsVM
                {
                    //ContractNo = "",
                    //Date = inv.Date,
                    TotalValue = inv.TotalValue - resultInvoicePayVoucherCredit,
                    CustomerName = inv.CustomerName ?? "",
                    CustomerName_W = inv.CustomerName_W ?? "",
                    CustomerMobile = inv.CustomerMobile ?? "",
                    //ProjectDescription="",
                    //ProjectName = inv.ProjectNo,
                    TotalPaidPayment = TotalPaidPaymentResult,
                    TotalRemainingPayment = ((inv.TotalValue - resultInvoicePayVoucherCredit) - TotalPaidPaymentResult),

                });
            }
            IEnumerable<ContractsVM> InvoicesList = list;
            //var Vouchers = someContracts.Union(InvoicesList);
            var Vouchers = InvoicesList;
            return Ok(Vouchers);
        }





        [HttpGet("GetContractserviceBycontractid")]
        public IActionResult GetContractserviceBycontractid(int Contractid)
            {

                var someContracts = _contractService.GetContractservicenByid(Contractid).Result.ToList();

                return Ok(someContracts);

            }
        [HttpGet("FillAllContractserviceByConId")]
        public IActionResult FillAllContractserviceByConId(int Param)
            {

                var cons = _contractService.GetContractservicenByid(Param).Result.Select(s => new
                {
                    Id = s.ServiceId,
                    Name = s.servicename
                });
                return Ok(cons);

            }
        [HttpGet("GetAllContractsBySearchlist")]
        public IEnumerable<ContractsVM> GetAllContractsBySearchlist(ContractsVM contractsVM)
            {
                if (contractsVM.dateFrom != null && contractsVM.dateFrom != "" && contractsVM.dateTo != null && contractsVM.dateTo != "")
                {
                    contractsVM.IsSearch = true;
                    contractsVM.IsChecked = true;
                }
                
                HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                var someContracts = _contractservice.GetAllContractsBySearch(contractsVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

                return someContracts;
            }


        [HttpPost("SaveContract")]
        public IActionResult SaveContract(Contracts contract)
            {
                
                HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                var result = _contractservice.SaveContract(contract, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
                if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "الكود موجود من قبل")
                {
                    result.ReasonPhrase = "The code already exists";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ ,تأكد من اختيار السنة الماليه أولا")
                {
                    result.ReasonPhrase = "Failed to save, be sure to choose the fiscal year first";
                }
                else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ ,تأكد من وجود حساب للعميل")
                {
                    result.ReasonPhrase = "Failed to save, make sure there is an account for the customer";
                }
                else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع")
                {
                    result.ReasonPhrase = "Failed to save, be sure to link the contract account to the branch";
                }
                else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ")
                {
                    result.ReasonPhrase = "Saved Falied";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "Saved Successfully";
                }
                if (contract.ContractId != 0)
                {
                    var result2 = UploadFile_Draft(contract.ContractId);
                    if (result2.StatusCode==HttpStatusCode.OK)
                    {
                        result.ReasonPhrase = "تم الحفظ";
                    }
                    else
                    {
                        result.ReasonPhrase = "تم الحفظ ولكن فشل في تحميل العقد";
                    }
                }
                return Ok(result);
            
        }

        [HttpPost("SaveContract_2")]
        public IActionResult SaveContract_2(Contracts contract)
        {
                
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _contractservice.SaveContract(contract, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "الكود موجود من قبل")
            {
                result.ReasonPhrase = "The code already exists";
            }
            else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ ,تأكد من اختيار السنة الماليه أولا")
            {
                result.ReasonPhrase = "Failed to save, be sure to choose the fiscal year first";
            }
            else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ ,تأكد من وجود حساب للعميل")
            {
                result.ReasonPhrase = "Failed to save, make sure there is an account for the customer";
            }
            else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع")
            {
                result.ReasonPhrase = "Failed to save, be sure to link the contract account to the branch";
            }
            else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ")
            {
                result.ReasonPhrase = "Saved Falied";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            if (contract.ContractId != 0)
            {
                var project = _projectService.GetProjectById(_globalshared.Lang_G, contract.ProjectId??0).Result;
                var drft = _drafts_TemplatesService.GetDraft_templateByProjectId(project.ProjectTypeId??0).Result;
                if (drft != null)
                {
                    //  var draft = _draftService.GetDraftById(DraftId);

                    var result2 = Connect_appendFile_Draft(contract.ProjectId??0, drft.DraftUrl);

                    if (result2.StatusCode==HttpStatusCode.OK)
                    {
                        result.ReasonPhrase ="تم الحفظ بنجاح";
                    }
                    else
                    { 
                        result.ReasonPhrase = "تم الحفظ ولكن فشل ف تحميل العقد";
                        result.ReturnedStrNeeded = result2.ReturnedStrNeeded;
                    }
                }
                else
                {

                    var result2 = UploadFile_Draft(contract.ContractId);
                    if (result2.StatusCode==HttpStatusCode.OK)
                    {
                        result.ReasonPhrase = "تم الحفظ";
                    }
                    else
                    {
                        result.ReasonPhrase = "تم الحفظ ولكن فشل في تحميل العقد";
                        result.ReturnedStrNeeded = result2.ReturnedStrNeeded;

                
                    }          
                }           
            } return Ok(result);
        }
        [HttpPost("EditContract")]
        public IActionResult EditContract(Contracts contract)
            {
                
                HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                var result = _contractservice.EditContract(contract, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
                if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "الكود موجود من قبل")
                {
                    result.ReasonPhrase = "The code already exists";
                }
                else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ ,تأكد من اختيار السنة الماليه أولا")
                {
                    result.ReasonPhrase = "Failed to save, be sure to choose the fiscal year first";
                }
                else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ ,تأكد من وجود حساب للعميل")
                {
                    result.ReasonPhrase = "Failed to save, make sure there is an account for the customer";
                }
                else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع")
                {
                    result.ReasonPhrase = "Failed to save, be sure to link the contract account to the branch";
                }
                else if (_globalshared.Lang_G == "ltr" &&result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ")
                {
                    result.ReasonPhrase = "Saved Falied";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "Saved Successfully";
                }
                return Ok(result);
            }
        [HttpPost("PostInvoiceAndPayPayment")]
        public IActionResult PostInvoiceAndPayPayment(List<Invoices> vouchers)
            {
                
                HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                var result = _voucherService.PostVouchers(vouchers, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
                return Ok(result);
            }
        [HttpPost("PostInvoiceAndPayPayment2")]
        public IActionResult PostInvoiceAndPayPayment2(CustomerPayments customerPayments)
            {
                var voucher = _voucherService.GetVoucherById(customerPayments.PaymentId).Result;

                customerPayments.PaymentId = 0;
                customerPayments.ContractId = customerPayments.ContractId;
                customerPayments.PaymentNo = 1;
                customerPayments.PaymentDate = voucher.Date;
                customerPayments.PaymentDateHijri = voucher.HijriDate;
                customerPayments.Amount = voucher.InvoiceValue ?? 0;
                customerPayments.TaxAmount = voucher.TaxAmount ?? 0;
                customerPayments.InvoiceId = voucher.InvoiceId;

                customerPayments.TotalAmount = voucher.TotalValue ?? 0;
                customerPayments.IsPaid = true;

                var result = _customerPaymentsservice.SaveCustomerPayment(customerPayments, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpGet("FillContractSelect")]
        public IActionResult FillContractSelect()
            {
                return Ok(_contractservice.GetAllContracts().Result.Select(s => new
                {
                    Id = s.ContractId,
                    Name = s.ContractNo
                }));
            }
        [HttpPost("GenerateCustomerPayments")]
        public IActionResult GenerateCustomerPayments(ContractsVM contractsVM)
            {
                int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
                return Ok(_contractservice.GenerateCustomerPayments(contractsVM, orgId));
            }

        [HttpPost("GetHijriDate")]
        public IActionResult GetHijriDate([FromBody]DateTime Date)
            {
                var date = _contractservice.ConvertDateCalendar(Date, "Hijri", "en-US");
           
                return Ok(date.ToString());
            }

        [HttpPost("GetHijriDate2")]
        public IActionResult GetHijriDate2([FromBody] DateTime Date)
        {
            var date = _contractservice.ConvertDateCalendar3(Date, "Hijri", "en-US");

            return Ok(date);
        }
        [HttpGet("GetDraft_templateByProjectId")]
        public IActionResult GetDraft_templateByProjectId(int ProjectId)
            {
                var project = _projectService.GetProjectById(_globalshared.Lang_G, ProjectId).Result;
                var drft = _drafts_TemplatesService.GetDraft_templateByProjectId((int)project.ProjectTypeId);
                return Ok(drft);
            }

        [HttpPost("CancelContract")]
        public IActionResult CancelContract(int ContractId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _contractservice.CancelContract(ContractId, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpPost("GenerateRandomNo")]
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        [HttpPost("SaveProjectContract")]
        public IActionResult SaveProjectContract(IFormFile? UploadedFile,[FromForm] ContractsVM contractsVM)
            {
                string fileLocation = "";
                string fileDir = "";
                string filDown = "";
            string fileName1 = "";
            var CustContract = _contractservice.GetAllContracts().Result.Where(w => w.ContractId == contractsVM.ContractId).ToList();
              //  HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((UploadedFile != null) && (UploadedFile.Length > 0) && !string.IsNullOrEmpty(UploadedFile.FileName))
                {
                string BranchId = Convert.ToString(CustContract.Select(s => s.BranchId).FirstOrDefault());
                string CustomerId = Convert.ToString(CustContract.Select(s => s.CustomerId).FirstOrDefault());
                string ProjectId = Convert.ToString(CustContract.Select(s => s.ProjectId).FirstOrDefault());
                if (UploadedFile.Length > 0)
                    {
                        if (CustContract != null)
                        {

                        fileName1 = System.IO.Path.GetFileName(GenerateRandomNo() + UploadedFile.FileName);

                        fileLocation = Path.Combine("Uploads/Contract/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/")
                                + UploadedFile.FileName;
                            fileDir = Path.Combine("Uploads/Contract/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/");
                            filDown = "Uploads/Contract/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/" + UploadedFile.FileName; ;

                            //D:\00000000000\Bayanatech.Business\Bayanatech.TameerPro.UI\Uploads\Contract\1\6\5\Contract_Proj5.docx
                        }
                        else
                        {
                            var massage = "";
                            if (_globalshared.Lang_G == "rtl")
                            {
                                massage = "فشل في رفع العقد";
                            }
                            else
                            {
                                massage = "Failed To Upload Contract Files";
                            }
                            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage });
                        }

                        try
                        {
                            if (System.IO.Directory.Exists(fileDir))
                            {
                                if (System.IO.File.Exists(fileLocation))
                                {
                                    System.IO.File.Delete(fileLocation);
                                }
                            }
                            else
                            {

                                System.IO.Directory.CreateDirectory(fileDir);
                                if (System.IO.File.Exists(fileLocation))
                                {
                                    System.IO.File.Delete(fileLocation);
                                }
                            }

                        if (UploadedFile != null)
                        {
                            System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                            string path = Path.Combine("Uploads/Contract/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/");
                            string pathW = Path.Combine("Uploads/Contract/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/");

                            if (!System.IO.Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }

                            List<string> uploadedFiles = new List<string>();
                            string pathes = "";
                            //foreach (IFormFile postedFile in postedFiles)
                            //{
                            string fileName = Path.Combine("Uploads/Contract/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/")
                                + fileName1;
                            //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                            var path2 = Path.Combine(fileName);
                            if (System.IO.File.Exists(path2))
                            {
                                System.IO.File.Delete(path2);
                            }
                            using (System.IO.FileStream stream = new System.IO.FileStream(path2, System.IO.FileMode.Create))
                            {


                                UploadedFile.CopyTo(stream);
                                uploadedFiles.Add(fileName);
                                // string returnpath = host + path + fileName;
                                //pathes.Add(pathW + fileName);
                                pathes = fileName;
                            }


                            if (pathes != null)
                            {
                                contractsVM.AttachmentUrl = "/"+ path2;
                            }
                        }
                        //Request.Files["UploadedFile"].SaveAs(fileLocation);
                        //contractsVM.AttachmentUrl = "/" + filDown;
                        }
                        catch (Exception ex)
                        {
                            var massage = "";
                            if (_globalshared.Lang_G == "rtl")
                            {
                                massage = "فشل في رفع العقد";
                            }
                            else
                            {
                                massage = "Failed To Upload Contract Files";
                            }
                            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage });
                        }
                    }
                }
                var result = _contractservice.SaveContractFile(contractsVM.ContractId, _globalshared.UserId_G, _globalshared.BranchId_G, contractsVM.AttachmentUrl);
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "Saved Successfully";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
                {
                    result.ReasonPhrase = "Saved Falied";
                }
                return Ok(result);
            }
        [HttpPost("UploadFile_Draft")]
        public GeneralMessage UploadFile_Draft(int ContractId)/*IFormFile file*//*ContractId*/
            {
            var massage = "";

            
                try
                {
                Spire.Doc.Document doc = new Spire.Doc.Document();

                List<string> fileUrls = new List<string>();
                //foreach (var item in contracts)
                //{
                var pathh = "";
                string path = Path.Combine("Reports/Contract/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = "Contract1_Draft.docx";
                pathh = path + fileName;

                // var pathh = "";
                int? custid = 0;
                string fileLocation = "";
                string fileDir = "";
                string fileNamed;
                string filDown;
                String URII;

                var CustContract = _contractservice.GetAllContracts().Result.Where(w => w.ContractId == ContractId).FirstOrDefault();
                    custid = CustContract.CustomerId;
                    var customerData = _customerservice.GetCustomersByCustomerId(custid, _globalshared.Lang_G).Result;
                    var branchObj = _branchservice.GetBranchById(CustContract.BranchId??0).Result;
                    int branchdata = branchObj.OrganizationId;
                    var OrgData = _organizationservice.GetBranchOrganizationData(branchdata).Result;
                    string BranchId = Convert.ToString(branchObj.BranchId);
                    string CustomerId = Convert.ToString(CustContract.CustomerId);
                    string ProjectId = Convert.ToString(CustContract.ProjectId);
                    string contanme = "Contract_" + CustContract.ProjectNo;

                    var Emp = _employeeService.GetEmployeeById(CustContract.OrgEmpId.Value, _globalshared.Lang_G).Result;
                    fileLocation = Path.Combine("Uploads/Drafts/")
                               + contanme + ".docx";

                    fileDir = Path.Combine("Uploads/Drafts/");
                    filDown = "Uploads/Drafts/";

                    if (System.IO.Directory.Exists(fileDir))
                    {
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(fileDir);
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                    }


                    fileNamed = contanme + ".docx";
                    string sourcePath = path;
                    string targetPath = fileDir;

                    // Use Path class to manipulate file and directory paths.
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                    string destFile = System.IO.Path.Combine(targetPath, fileNamed);
                    string IsContains = CustContract.TaxType == 3 ? "شامل" : "غير شامل";
                    var ContractDetails = _contractDetailsService.GetAllEmpConDetailsByContractId(ContractId).Result;
                    string ContractDetailsStr = "";
                    if (ContractDetails.Count() > 0)
                    {
                        string[] strings = { "الأول", "الثاني", "الثالث", "الرابع", "الخامس", "السادس", "السابع", "الثامن", "التاسع", "العاشر" };
                        ContractDetailsStr = "بنود العقد: " + "\r\n";
                        int x = 0;
                        foreach (var item in ContractDetails)
                        {
                            ContractDetailsStr = ContractDetailsStr + string.Format("البند {0}: {1}", strings[x], item.Clause) + "\r\n";
                            x++;
                        }
                    }

                    // To copy a file to another location and
                    // overwrite the destination file if it already exists.
                    if (System.IO.File.Exists(destFile))
                    {
                        System.IO.File.Delete(destFile);
                    }
                    System.IO.File.Copy(sourceFile, destFile, true);

                    Spire.Doc.Document doc2 = new Spire.Doc.Document();
                    WebClient webClient = new WebClient();
                    using (MemoryStream ms = new MemoryStream(webClient.DownloadData(destFile)))
                    {
                        doc.LoadFromStream(ms, FileFormat.Docx);
                        string dayname = GetDayName((DateTime.ParseExact(CustContract.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        doc.Replace("#ContNo", CustContract.ContractNo, false, true);
                        doc.Replace("#Hijri", CustContract.HijriDate??"", false, true);
                        doc.Replace("#Miladi", CustContract.Date, false, true);
                        doc.Replace("#Day", dayname, false, true);
                        doc.Replace("#CustName", customerData.CustomerName, false, true);
                        doc.Replace("#CompName", OrgData.NameAr, false, true);
                        string CustomerNationalData = "";
                        if (customerData.CustomerTypeId == 1)
                        {
                            CustomerNationalData = string.Format("بطاقة الهوية: {0}", (customerData.CustomerNationalId??""));
                        }
                        else if (customerData.CustomerTypeId == 2)
                        {
                            CustomerNationalData = string.Format("سجل تجاري: {0}", (customerData.CommercialRegister??""));
                        }

                        doc.Replace("#CustNationalData", CustomerNationalData, false, true);
                        doc.Replace("#TotalTxt", (CustContract.ValueText??""), false, true);
                        //New
                        // doc.Replace("#TaxValue", CustContract.TaxesValue.HasValue ? CustContract.TaxesValue.Value.ToString() : "", false, true);
                        // doc.Replace("#ServiceName", Service.Name, false, true);
                        // doc.Replace("#ServiceDetails", serviceDetails, false, true);
                        //doc.Replace("#ContractValue", CustContract.TotalValue.ToString(), false, true);
                        doc.Replace("#IsContain", IsContains, false, true);

                        doc.Replace("#OrgEmpName", (Emp.EmployeeNameAr??""), false, true);
                        doc.Replace("#OrgEmpJob", (Emp.JobName??""), false, true);
                        doc.Replace("#EmpNationalId", (Emp.NationalId ?? ""), false, true);

                        doc.Replace("#ContractDetails", (ContractDetailsStr??""), false, true);

                        // doc.Replace("#ProjectNo", CustContract.ProjectNo, false, true);

                        /////////////////////


                        Section section3 = doc.Sections[0];

                        TextSelection selection1 = doc.FindString("#khadmat", true, true);

                        TextRange range1 = selection1.GetAsOneRange();

                        Spire.Doc.Documents.Paragraph paragraph1 = range1.OwnerParagraph;

                        Spire.Doc.Body body1 = paragraph1.OwnerTextBody;

                        int index1 = body1.ChildObjects.IndexOf(paragraph1);

                        var conservices = _contractService.GetContractservicenByid(ContractId).Result;
                        if (conservices.Count() > 0)
                        {

                            Spire.Doc.Table table2 = section3.AddTable(true);
                            var countS = conservices.Count();
                            table2.ResetCells(1, 6);
                            // table.AddRow(false, 2);
                            //table2.Rows[0].Cells[0].Width = table2.Rows[0].Cells[1].Width = 60F;

                            table2.TableFormat.Bidi = true;
                            table2[0, 0].AddParagraph().AppendText("رقم المشروع");
                            string text = "الخدمة";
                            table2[0, 1].AddParagraph().AppendText(text);
                            table2[0, 2].AddParagraph().AppendText("تفاصيل الخدمة");
                            table2[0, 3].AddParagraph().AppendText(" الكمية");
                            table2[0, 4].AddParagraph().AppendText("مبلغ العقد");
                            table2[0, 5].AddParagraph().AppendText("مبلغ ضريبة");
                            int i = 1;
                            foreach (var S in conservices)
                            {

                                string serviceDetails1 = "";
                                //var ServiceDetObjs12 = _servicesPriceService.GetServicesPriceByParentId(S.ServiceId).Result;
                                var ServiceDetObjs1 = _servicesPriceOfferService.GetServicesPriceByParentIdAndContractId(S.ServiceId, ContractId).Result;
                                ServiceDetObjs1 = ServiceDetObjs1.OrderBy(q => q.LineNumber).ToList();

                            var TaxAmount = "";
                            if(CustContract.TaxType==3)
                            {
                                TaxAmount = (Convert.ToDecimal(S.Serviceamountval - (S.Serviceamountval / Convert.ToDecimal(1.15)))).ToString("0.00"); 
                            }
                            else
                            {
                                TaxAmount = (Convert.ToDecimal((S.Serviceamountval * Convert.ToDecimal(0.15)))).ToString("0.00");

                            }
                            foreach (var item in ServiceDetObjs1)
                                {
                                    serviceDetails1 = serviceDetails1 + (item.ServicesName) + "\r\n";
                                }


                                table2.AddRow(true, 6);
                                table2[i, 0].AddParagraph().AppendText((CustContract.ProjectNo??"").ToString());
                                table2[i, 1].AddParagraph().AppendText((S.servicename??"").ToString());
                                table2[i, 2].AddParagraph().AppendText((serviceDetails1??"").ToString());
                                table2[i, 3].AddParagraph().AppendText((S.ServiceQty??1).ToString());
                                table2[i, 4].AddParagraph().AppendText((S.Serviceamountval??0).ToString());
                                table2[i, 5].AddParagraph().AppendText((TaxAmount ?? "").ToString());
                                i++;
                            }

                            body1.ChildObjects.Remove(paragraph1);

                            body1.ChildObjects.Insert(index1, table2);
                        }



                        Section section2 = doc.Sections[0];

                        TextSelection selection = doc.FindString("#Dof3at", true, true);

                        TextRange range = selection.GetAsOneRange();

                        Spire.Doc.Documents.Paragraph paragraph = range.OwnerParagraph;

                        Spire.Doc.Body body = paragraph.OwnerTextBody;

                        int index = body.ChildObjects.IndexOf(paragraph);



                        var payment = _customerPaymentservice.GetAllCustomerPayments(Convert.ToInt32(ContractId)).Result;

                        if (payment.Count() > 0)
                        {
                            Spire.Doc.Table table2 = section2.AddTable(true);
                            var countp = payment.Count();
                            table2.ResetCells(1, 2);
                            // table.AddRow(false, 2);
                            //table2.Rows[0].Cells[0].Width = table2.Rows[0].Cells[1].Width = 60F;

                            table2.TableFormat.Bidi = true;
                            table2[0, 0].AddParagraph().AppendText("المبلغ");
                            string text = "تاريخ الدفعة";
                            table2[0, 1].AddParagraph().AppendText(text);

                            int i = 1;
                            foreach (var p in payment)
                            {

                                table2.AddRow(true, 2);
                                table2[i, 0].AddParagraph().AppendText((p.TotalAmount).ToString());
                                table2[i, 1].AddParagraph().AppendText((p.PaymentDate??"").ToString());
                                i++;
                            }

                            body.ChildObjects.Remove(paragraph);

                            body.ChildObjects.Insert(index, table2);

                        }
                        else
                        {
                            body.ChildObjects.Remove(paragraph);

                            selection = doc.FindString("جدول الدفعات:", true, true);

                            range = selection.GetAsOneRange();

                            paragraph = range.OwnerParagraph;

                            body.ChildObjects.Remove(paragraph);
                        }
                        ///////////////////////

                        Spire.Doc.Table table = section2.AddTable(true);

                        table.ResetCells(3, 2);

                        table[0, 0].AddParagraph().AppendText(string.Format("الطرف الأول"));
                        table[0, 0].Paragraphs[0].Format.IsBidi = true;
                        table.Rows[0].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                        table[0, 1].AddParagraph().AppendText(string.Format("الطرف الثاني"));
                        table[0, 1].Paragraphs[0].Format.IsBidi = true;
                        table.Rows[0].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                        //table.AddRow(true, 2);
                        table[1, 0].AddParagraph().AppendText(string.Format("الاسم: {0}", OrgData.NameAr));
                        table[1, 0].Paragraphs[0].Format.IsBidi = true;
                        table.Rows[1].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                        table[1, 1].AddParagraph().AppendText(string.Format("الاسم: {0}", customerData.CustomerName));
                        table[1, 1].Paragraphs[0].Format.IsBidi = true;
                        table.Rows[1].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                        //table.AddRow(true, 3);
                        table[2, 0].AddParagraph().AppendText(string.Format("التوقيع:.................................."));
                        table[2, 0].Paragraphs[0].Format.IsBidi = true;
                        table.Rows[2].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                        table[2, 1].AddParagraph().AppendText(string.Format("التوقيع:.................................."));
                        table[2, 1].Paragraphs[0].Format.IsBidi = true;
                        table.Rows[2].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                        table.TableFormat.Borders.Color = System.Drawing.Color.Transparent;
                        table.TableFormat.Bidi = true;
                        table.TableFormat.HorizontalAlignment = Spire.Doc.Documents.RowAlignment.Center;

                    }


                    doc.SaveToFile(destFile, FileFormat.Docx);
                    fileUrls.Add(destFile);
                    //TempData["Linkurl"] = destFile;


                    RemoveRedLine(destFile);
                    var UrlS = _hostingEnvironment.WebRootPath;

                    StringBuilder sb = new StringBuilder(UrlS);

                    sb.Replace("Contract", filDown);
                    sb.Replace("UploadFile", fileNamed);
                    URII = sb.ToString();
                    //TempData["Linkurl"] = URII;


                    URII = URII.Replace("//SaveUploads/Drafts", "") + fileNamed;
                var pathfile = "/" + targetPath + fileNamed;
                    //Save as a draft
                    int projectType = _projectService.GetTypeOfProjct(CustContract.ProjectId??0);
                    Draft draft = new Draft() { DraftUrl = pathfile, Name = fileNamed, ProjectTypeId = projectType };
                    _draftService.SaveDraft(draft, _globalshared.UserId_G, _globalshared.BranchId_G);
                    var result = _draftDetailsService.SaveDraftDetails(new DraftDetails()
                    {
                        DraftId = draft.DraftId,
                        ProjectId = CustContract.ProjectId.Value
                    }, _globalshared.UserId_G, _globalshared.BranchId_G);
                    return result;
                }

                catch (Exception ex)
                {

                    if (_globalshared.Lang_G == "rtl")
                    {
                        massage = "فشل في تحميل العقد";
                    }
                    else
                    {
                        massage = "Failed To Download Contract Files";
                    }
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage,ReturnedStrNeeded=ex.Message };
                }
            }

        [HttpPost("Connect_appendFile_Draft")]

        public GeneralMessage Connect_appendFile_Draft(int ProjectId, string uploadedFile)
            {
            var massage = "";

            try
            {
                var project = _projectService.GetProjectById("rtl", ProjectId).Result;
                int ContractId = project.ContractId.Value;


                string[] fileNamedSplit = uploadedFile.Split('/');
                string uploadedFileName = fileNamedSplit[(fileNamedSplit.Count() - 1)];

                if (uploadedFileName.Contains(project.ProjectNo))
                {
                    //return Ok(new GeneralMessage { Result = true, Message = "Resources.General_SavedSuccessfully", ReturnedStr = uploadedFile },
                    //    JsonRequestBehavior.AllowGet);


                }
                uploadedFile = Path.Combine("Uploads/Drafts/") + uploadedFileName;

                Spire.Doc.Document doc = new Spire.Doc.Document();
                Spire.Doc.Document doc_Uploaded = new Spire.Doc.Document();

                List<string> fileUrls = new List<string>();
                //foreach (var item in contracts)
                //{
                var pathh = "";
                string path = Path.Combine("Reports/Contract/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = "DraftTemplate.docx";
                pathh = path + fileName;

                // var pathh = "";
                int? custid = 0;
                string fileLocation = "";
                string fileDir = "";
                string fileNamed;
                string filDown;
                String URII;

                #region Initilizations

                EmployeesVM Emp = null;
                    AccServicesPricesVM Service = null;
                    string serviceDetails = "";

                    var CustContract = _contractservice.GetAllContracts().Result.Where(w => w.ContractId == ContractId).FirstOrDefault();
                    custid = CustContract.CustomerId;
                    var customerData = _customerservice.GetCustomersByCustomerId(custid, _globalshared.Lang_G).Result;
                    var branchObj = _branchservice.GetBranchById(CustContract.BranchId??0).Result;
                    int branchdata = branchObj.OrganizationId;
                    var OrgData = _organizationservice.GetBranchOrganizationData(branchdata).Result;
                    string BranchId = Convert.ToString(branchObj.BranchId);
                    string CustomerId = Convert.ToString(CustContract.CustomerId);
                    //string ProjectId = Convert.ToString(CustContract.ProjectId);
                    string contanme = "Contract_ProjDraft" + CustContract.ProjectNo;
                    string IsContains = CustContract.TaxType == 3 ? "شامل" : "غير شامل ";

                    if (CustContract.OrgEmpId != null)
                    {
                        Emp = _employeeService.GetEmployeeById(CustContract.OrgEmpId.Value, _globalshared.Lang_G).Result;
                    }

                    if (CustContract.ServiceId != null)
                    {
                        //Service = _servicesPriceService.GetAllServicesPrice().Result.Where(x => x.ServicesId == CustContract.ServiceId).FirstOrDefault();
                        var ServiceDetObjs = _servicesPriceOfferService.GetServicesPriceByParentIdAndContractId(CustContract.ServiceId, ContractId).Result;
                    ServiceDetObjs = ServiceDetObjs.OrderBy(q => q.LineNumber).ToList();
                    //var ServiceDetObjs2 = _servicesPriceService.GetServicesPriceByParentId(Service.ServicesId).Result;

                        foreach (var item in ServiceDetObjs)
                        {
                            serviceDetails = serviceDetails + (item.ServicesName) + "\r\n";
                        }
                    }
                    #endregion
                    fileLocation = Path.Combine("Uploads/Drafts/") + contanme + ".docx";

                    fileDir = Path.Combine("Uploads/Drafts/");
                    filDown = "Uploads/Drafts/";

                    #region conditions


                    if (System.IO.Directory.Exists(fileDir))
                    {
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(fileDir);
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                    }
                    #endregion


                    fileNamed = contanme + ".docx";
                    string sourcePath = path;
                    string targetPath = fileDir;

                    // Use Path class to manipulate file and directory paths.
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);

                    var ContractDetails = _contractDetailsService.GetAllEmpConDetailsByContractId(ContractId).Result;
                    string ContractDetailsStr = "";
                    if (ContractDetails.Count() > 0)
                    {
                        string[] strings = { "الأول", "الثاني", "الثالث", "الرابع", "الخامس", "السادس", "السابع", "الثامن", "التاسع", "العاشر" };
                        ContractDetailsStr = "بنود العقد: " + "\r\n";
                        int x = 0;
                        foreach (var item in ContractDetails)
                        {
                            ContractDetailsStr = ContractDetailsStr + string.Format("البند {0}: {1}", strings[x], item.Clause) + "\r\n";
                            x++;
                        }
                    }

                    WebClient webClient = new WebClient();
                    MemoryStream ms_Uploaded = new MemoryStream(webClient.DownloadData(uploadedFile));
                    doc_Uploaded.LoadFromStream(ms_Uploaded, FileFormat.Auto);


                    using (MemoryStream ms = new MemoryStream(webClient.DownloadData(sourceFile)))
                    {

                        doc.LoadFromStream(ms, FileFormat.Docx);

                        string dayname = GetDayName((DateTime.ParseExact(CustContract.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        doc.Replace("#ContNo", CustContract.ContractNo, false, true);
                        doc.Replace("#Hijri", CustContract.HijriDate, false, true);
                        doc.Replace("#Miladi", CustContract.Date, false, true);
                        doc.Replace("#Day", dayname, false, true);
                        doc.Replace("#CustName", customerData.CustomerName, false, true);
                        doc.Replace("#CompName", OrgData.NameAr, false, true);
                        string CustomerNationalData = "";
                        if (customerData.CustomerTypeId == 1)
                        {
                            CustomerNationalData = string.Format("بطاقة الهوية {0}", customerData.CustomerNationalId);
                        }
                        else if (customerData.CustomerTypeId == 2)
                        {
                            CustomerNationalData = string.Format("سجل تجاري: {0}", customerData.CommercialRegister);
                        }

                        doc.Replace("#CustNationalData", CustomerNationalData, false, true);
                        doc.Replace("#TotalTxt", CustContract.ValueText??"", false, true);
                        //New
                        doc.Replace("#TaxValue", CustContract.TaxesValue.HasValue ? CustContract.TaxesValue.Value.ToString() : "", false, true);
                        if (Service != null)
                        {
                            doc.Replace("#ServiceName", Service.Name, false, true);
                            doc.Replace("#ServiceDetails", serviceDetails, false, true);
                        }
                        else
                        {
                            doc.Replace("#ServiceName", "", false, true);
                            doc.Replace("#ServiceDetails", "", false, true);
                        }
                        doc.Replace("#ContractValue", CustContract.TotalValue.ToString(), false, true);
                        doc.Replace("#IsContain", IsContains, false, true);
                        if (Emp != null)
                        {
                            doc.Replace("#OrgEmpName", Emp.EmployeeNameAr, false, true);
                            doc.Replace("#OrgEmpJob", Emp.JobName, false, true);
                            doc.Replace("#EmpNationalId", Emp.NationalId, false, true);
                        }
                        else
                        {
                            doc.Replace("#OrgEmpName", "", false, true);
                            doc.Replace("#OrgEmpJob", "", false, true);
                            doc.Replace("#EmpNationalId", "", false, true);
                        }

                        doc.Replace("#ContractDetails", ContractDetailsStr, false, true);

                        doc.Replace("#ProjectNo", CustContract.ProjectNo, false, true);



                    Section section3 = doc.Sections[0];

                    TextSelection selection1 = doc.FindString("#khadmat", true, true);

                    TextRange range1 = selection1.GetAsOneRange();

                    Spire.Doc.Documents.Paragraph paragraph1 = range1.OwnerParagraph;

                    Spire.Doc.Body body1 = paragraph1.OwnerTextBody;

                    int index1 = body1.ChildObjects.IndexOf(paragraph1);

                    var conservices = _contractService.GetContractservicenByid(ContractId).Result;
                    if (conservices.Count() > 0)
                    {

                        Spire.Doc.Table table2 = section3.AddTable(true);
                        var countS = conservices.Count();
                        table2.ResetCells(1, 6);
                        // table.AddRow(false, 2);
                        //table2.Rows[0].Cells[0].Width = table2.Rows[0].Cells[1].Width = 60F;

                        table2.TableFormat.Bidi = true;
                        table2[0, 0].AddParagraph().AppendText("رقم المشروع");
                        string text = "الخدمة";
                        table2[0, 1].AddParagraph().AppendText(text);
                        table2[0, 2].AddParagraph().AppendText("تفاصيل الخدمة");
                        table2[0, 3].AddParagraph().AppendText(" الكمية");
                        table2[0, 4].AddParagraph().AppendText("مبلغ العقد");
                        table2[0, 5].AddParagraph().AppendText("مبلغ ضريبة");
                        int ii = 1;
                        foreach (var S in conservices)
                        {

                            string serviceDetails1 = "";
                            //var ServiceDetObjs12 = _servicesPriceService.GetServicesPriceByParentId(S.ServiceId).Result;
                            var ServiceDetObjs1 = _servicesPriceOfferService.GetServicesPriceByParentIdAndContractId(S.ServiceId, ContractId).Result;
                            ServiceDetObjs1 = ServiceDetObjs1.OrderBy(q => q.LineNumber).ToList();

                            var TaxAmount = "";
                            if (CustContract.TaxType == 3)
                            {
                                TaxAmount = (Convert.ToDecimal(S.Serviceamountval - (S.Serviceamountval / Convert.ToDecimal(1.15)))).ToString("0.00");
                            }
                            else
                            {
                                TaxAmount = (Convert.ToDecimal((S.Serviceamountval * Convert.ToDecimal(0.15)))).ToString("0.00");

                            }
                            foreach (var item in ServiceDetObjs1)
                            {
                                serviceDetails1 = serviceDetails1 + (item.ServicesName) + "\r\n";
                            }


                            table2.AddRow(true, 6);
                            table2[ii, 0].AddParagraph().AppendText((CustContract.ProjectNo ?? "").ToString());
                            table2[ii, 1].AddParagraph().AppendText((S.servicename ?? "").ToString());
                            table2[ii, 2].AddParagraph().AppendText((serviceDetails1 ?? "").ToString());
                            table2[ii, 3].AddParagraph().AppendText((S.ServiceQty ?? 1).ToString());
                            table2[ii, 4].AddParagraph().AppendText((S.Serviceamountval ?? 0).ToString());
                            table2[ii, 5].AddParagraph().AppendText((TaxAmount ?? "").ToString());
                            ii++;
                        }

                        body1.ChildObjects.Remove(paragraph1);

                        body1.ChildObjects.Insert(index1, table2);
                    }




                    /////////////////////
                    Section section2 = doc.Sections[0];

                        TextSelection selection = doc.FindString("#Dof3at", true, true);

                        TextRange range = selection.GetAsOneRange();

                        Spire.Doc.Documents.Paragraph paragraph = range.OwnerParagraph;

                        Spire.Doc.Body body = paragraph.OwnerTextBody;

                        int index = body.ChildObjects.IndexOf(paragraph);

                        var payment = _customerPaymentservice.GetAllCustomerPayments(Convert.ToInt32(ContractId)).Result;

                        if (payment.Count() > 0)
                        {
                            Spire.Doc.Table table2 = section2.AddTable(true);
                            var countp = payment.Count();
                            table2.ResetCells(1, 2);
                            // table.AddRow(false, 2);
                            //table2.Rows[0].Cells[0].Width = table2.Rows[0].Cells[1].Width = 60F;

                            table2.TableFormat.Bidi = true;
                            table2[0, 0].AddParagraph().AppendText("المبلغ");
                            string text = "تاريخ الدفعة";
                            table2[0, 1].AddParagraph().AppendText(text);

                            int x = 1;
                            foreach (var p in payment)
                            {

                                table2.AddRow(true, 2);
                                table2[x, 0].AddParagraph().AppendText(p.TotalAmount.ToString());
                                table2[x, 1].AddParagraph().AppendText(p.PaymentDate.ToString());
                                x++;
                            }

                            body.ChildObjects.Remove(paragraph);

                            body.ChildObjects.Insert(index, table2);

                        }
                        else
                        {
                            body.ChildObjects.Remove(paragraph);

                            selection = doc.FindString("جدول الدفعات:", true, true);

                            range = selection.GetAsOneRange();

                            paragraph = range.OwnerParagraph;

                            body.ChildObjects.Remove(paragraph);
                        }
                    }
                    int i = 0;
                    foreach (Section sec in doc.Sections)
                    {
                        foreach (DocumentObject obj in sec.Body.ChildObjects)
                        {
                            doc_Uploaded.Sections[0].Body.ChildObjects.Insert(i, obj.Clone());
                            i++;
                        }
                    }

                    //Save Word
                    #region Save tables

                    Section section = doc_Uploaded.AddSection();
                    Spire.Doc.Table table = section.AddTable(true);

                    table.ResetCells(3, 2);

                    table.TableFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[0, 0].AddParagraph().AppendText(string.Format("الطرف الأول"));
                    table[0, 0].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[0].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[0, 1].AddParagraph().AppendText(string.Format("الطرف الثاني"));
                    table[0, 1].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[0].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    //table.AddRow(true, 2);

                    table[1, 0].AddParagraph().AppendText(string.Format("الاسم: {0}", OrgData.NameAr));
                    table[1, 0].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[1].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[1, 1].AddParagraph().AppendText(string.Format("الاسم: {0}", customerData.CustomerName));
                    table[1, 1].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[1].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    //table.AddRow(true, 2);

                    table[2, 0].AddParagraph().AppendText(string.Format("التوقيع: .................................."));
                    table[2, 0].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[2].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[2, 1].AddParagraph().AppendText(string.Format("التوقيع: .................................."));
                    table[2, 1].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[2].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    //table.AddRow(true, 2);

                    table.TableFormat.Bidi = true;

                    #endregion

                    #region Haeader And Footer

                    //Add Header
                    Section Headersection = doc_Uploaded.Sections[0];

                    HeaderFooter header = Headersection.HeadersFooters.Header;
                    Spire.Doc.Table Htable = header.AddTable(true);

                    Htable.ResetCells(1, 3);

                    Htable.TableFormat.Borders.Bottom.Color = System.Drawing.Color.Gray;

                    TextRange FText = Htable[0, 0].AddParagraph().AppendText(string.Format(OrgData!.NameAr ?? ""));

                    FText.CharacterFormat.FontName = "Calibri";
                    FText.CharacterFormat.FontSize = 13;
                    FText.CharacterFormat.TextColor = System.Drawing.Color.Black;

                    Htable[0, 0].Paragraphs[0].Format.IsBidi = true;
                    Htable.Rows[0].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    Htable.Rows[0].Cells[0].CellFormat.Borders.Bottom.Color = System.Drawing.Color.Gray;

                    Htable.Rows[0].Cells[1].AddParagraph();
                    string resultLogoUrl = OrgData.LogoUrl.Remove(0, 1);
                DocPicture picture = Htable.Rows[0].Cells[1].Paragraphs[0].AppendPicture(Image.FromFile(Path.Combine(resultLogoUrl)));
                    picture.Width = 90;
                    picture.Height = 80;

                    Htable[0, 1].Paragraphs[0].Format.HorizontalAlignment = HorizontalAlignment.Center;
                    Htable.Rows[0].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    Htable.Rows[0].Cells[1].CellFormat.Borders.Bottom.Color = System.Drawing.Color.Gray;

                    //Htable.AddRow(true, 2);

                    FText = Htable[0, 2].AddParagraph().AppendText(string.Format("التاريخ {0}:", CustContract.Date));

                    FText.CharacterFormat.FontName = "Calibri";
                    FText.CharacterFormat.FontSize = 13;
                    FText.CharacterFormat.TextColor = System.Drawing.Color.Black;

                    Htable[0, 2].Paragraphs[0].Format.HorizontalAlignment = HorizontalAlignment.Center;
                    Htable[0, 2].Paragraphs[0].Format.IsBidi = true;

                    Htable.Rows[0].Cells[2].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    Htable.Rows[0].Cells[2].CellFormat.Borders.Bottom.Color = System.Drawing.Color.Gray;

                    Htable.TableFormat.Bidi = true;


                    //Add Footer
                    HeaderFooter footer = Headersection.HeadersFooters.Footer;
                    Paragraph FParagraph = footer.AddParagraph();
                    string footerStr = string.Format("{0} - {1} - تليفون ,{2}: فاكس ,{3}: بريد الكترونى {4}: ", OrgData.NameAr, OrgData.Address, OrgData.Mobile, OrgData.Fax, OrgData.Email);
                    FText = FParagraph.AppendText(footerStr);

                    //Set Footer Text Format
                    FText.CharacterFormat.FontName = "Calibri";
                    FText.CharacterFormat.FontSize = 8;
                    FText.CharacterFormat.TextColor = System.Drawing.Color.Black;

                    //Set Footer Paragrah Format
                    FParagraph.Format.HorizontalAlignment = HorizontalAlignment.Left;
                    FParagraph.Format.IsBidi = true;
                    FParagraph.Format.Borders.Top.BorderType = BorderStyle.ThinThinSmallGap;
                    FParagraph.Format.Borders.Top.Space = 0.15f;
                    FParagraph.Format.Borders.Color = System.Drawing.Color.DarkGray;
                    #endregion

                    uploadedFile = fileLocation = Path.Combine("Uploads/Drafts/") + string.Format("{0}{1}", CustContract.ProjectNo, uploadedFileName);
                    doc_Uploaded.SaveToFile(uploadedFile, FileFormat.Docx);
                    fileUrls.Add(uploadedFile);
                    //TempData["Linkurl"] = uploadedFile;

                    ////////////////////////////////////////////
                    ///


                    RemoveRedLine(uploadedFile);
                    ////////////////////////////////////////////

                    var UrlS =_hostingEnvironment.WebRootPath;
                //URII = URII.Replace("//SaveUploads/Drafts", "") + fileNamed;
                var pathfile = "/" + uploadedFile;
                //UrlS = UrlS.Replace("/Contract/SaveContract_2", "") + "/Uploads//Drafts//" + string.Format("{0}{1}", CustContract.ProjectNo, uploadedFileName);


                    // UrlS = UrlS.Replace("/Contract/SaveContract_2", "");

                    //Save as a draft
                    int projectType = _projectService.GetTypeOfProjct(CustContract.ProjectId.Value);
                    Draft draft = new Draft() { DraftUrl = pathfile, Name = string.Format("{0}{1}", CustContract.ProjectNo, uploadedFileName), ProjectTypeId = projectType };
                    _draftService.SaveDraft(draft, _globalshared.UserId_G, _globalshared.BranchId_G);
                    var result = _draftDetailsService.SaveDraftDetails(new DraftDetails()
                    {
                        DraftId = draft.DraftId,
                        ProjectId = CustContract.ProjectId.Value
                    }, _globalshared.UserId_G, _globalshared.BranchId_G);
                    return result;

                    // return Ok(new GeneralMessage { Result = true, Message = "Resources.General_SavedSuccessfully", ReturnedStr = UrlS });
                }

                catch (Exception ex)
                {
                    if (_globalshared.Lang_G == "rtl")
                    {
                        massage = "فشل في تحميل العقد";
                    }
                    else
                    {
                        massage = "Failed To Download Contract Files";
                    }
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage,ReturnedStrNeeded=ex.Message };
                }


                //return new GeneralMessage { Result = true, Message = "" };
            }
   
        [HttpGet("Issuing_invoice")]
        public IActionResult Issuing_invoice(Invoices voucher)
            {
                
                HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                var result = _voucherService.Issuing_invoice(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
                return Ok(result);
            }
        [HttpGet("GetDayName")]
        private string GetDayName(DateTime dt)
            {
                string day = "";
                day = dt.DayOfWeek.ToString();
                StringBuilder sb = new StringBuilder(day);


                string dayAr =
                    sb
                      .Replace("Saturday", "السبت")
                      .Replace("Sunday", "الأحد")
                      .Replace("Monday", "الإثنين")
                       .Replace("Tuesday", "الثلاثاء")
                        .Replace("Wednesday", "الأربعاء")
                         .Replace("Thursday", "الخميس")
                          .Replace("Friday", "الجمعة")
                      .ToString();
                return dayAr;
            }
        [HttpGet("GenerateContractNumber")]
        public IActionResult GenerateContractNumber()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var NewValue=_contractservice.GenerateContractNumber(_globalshared.BranchId_G);
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = NewValue };
            return Ok(generatevalue);
        }
        [HttpGet("GenerateContractNumber2")]
        public IActionResult GenerateContractNumber2()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var NewValue = _contractservice.GenerateContractNumber2(_globalshared.BranchId_G);
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = NewValue };
            return Ok(generatevalue);
        }
        [HttpGet("GenerateContractNumber3")]
        public IActionResult GenerateContractNumber3()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var NewValue = _contractservice.GenerateContractNumber3(_globalshared.BranchId_G);
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = NewValue };
            return Ok(generatevalue);
        }
        [HttpGet("GenerateContractNumber4")]
        public IActionResult GenerateContractNumber4()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var NewValue = _contractservice.GenerateContractNumber4(_globalshared.BranchId_G);
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = NewValue };
            return Ok(generatevalue);
        }
        [HttpGet("GenerateContractNumber5")]
        public IActionResult GenerateContractNumber5()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var NewValue = _contractservice.GenerateContractNumber5(_globalshared.BranchId_G);
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = NewValue };
            return Ok(generatevalue);
        }
       

        [HttpPost("PrintVoucher")]
        public IActionResult PrintVoucher([FromForm]string? CustomerId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] int? Zerocheck)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ContractsPrintVM _contractsPrintVM = new ContractsPrintVM();
            int? CustId = string.IsNullOrEmpty(CustomerId) ? (int?)null : Convert.ToInt32(CustomerId);

            List<AccountVM> ContractsVM = _accountsService.GetCustomerFinancialDetailsByFilter(CustId, FromDate??"", ToDate??"", _globalshared.BranchId_G, _globalshared.Lang_G, _globalshared.YearId_G, Zerocheck??0).Result.ToList();

            int orgId = _branchservice.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };


            _contractsPrintVM.result = ContractsVM;
            _contractsPrintVM.StartDate = FromDate;
            _contractsPrintVM.EndDate = ToDate;
            _contractsPrintVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            var objOrganization2 = _organizationservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _contractsPrintVM.Org_VD = objOrganization2;
            return Ok(_contractsPrintVM);
        }



        [HttpGet("RemoveRedLine")]
        public bool RemoveRedLine(string filePath)
            {
                try
                {
                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
                    {
                        string docText = null;
                        using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                        {
                            docText = sr.ReadToEnd();
                        }

                        Regex regexText = new Regex("Evaluation Warning: The document was created with Spire.Doc for .NET.");
                        docText = regexText.Replace(docText, "");

                        using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                        {
                            sw.Write(docText);
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        [HttpPost("UploadExtraFile")]
        public IActionResult UploadExtraFile(IFormFile? UploadedFile,[FromForm] ContractsVM contractsVM)
            {
                string fileLocation = "";
                string fileDir = "";
                string filDown = "";
                var CustContract = _contractservice.GetAllContracts().Result.Where(w => w.ContractId == contractsVM.ContractId).ToList();
               // HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((UploadedFile != null) && (UploadedFile.Length > 0) && !string.IsNullOrEmpty(UploadedFile.FileName))
                {
                string BranchId = Convert.ToString(CustContract.Select(s => s.BranchId).FirstOrDefault());
                string CustomerId = Convert.ToString(CustContract.Select(s => s.CustomerId).FirstOrDefault());
                string ProjectId = Convert.ToString(CustContract.Select(s => s.ProjectId).FirstOrDefault());
                if (UploadedFile.Length > 0)
                    {
                        if (CustContract != null)
                        {
                  

                            fileLocation = Path.Combine("~/Uploads/ContractFileExtra/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/")
                                + UploadedFile.FileName;
                            fileDir = Path.Combine("~/Uploads/ContractFileExtra/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/");
                            filDown = "Uploads/ContractFileExtra/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/" + UploadedFile.FileName;
                        }
                        else
                        {
                            var massage = "";
                            if (_globalshared.Lang_G == "rtl")
                            {
                                massage = "فشل في رفع ملف";
                            }
                            else
                            {
                                massage = "Failed To Upload File";
                            }
                            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage });
                        }

                        try
                        {
                            if (System.IO.Directory.Exists(fileDir))
                            {
                                if (System.IO.File.Exists(fileLocation))
                                {
                                    System.IO.File.Delete(fileLocation);
                                }
                            }
                            else
                            {

                                System.IO.Directory.CreateDirectory(fileDir);
                                if (System.IO.File.Exists(fileLocation))
                                {
                                    System.IO.File.Delete(fileLocation);
                                }
                            }
                        if (UploadedFile != null)
                        {
                            System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                            string path = Path.Combine("~/Uploads/ContractFileExtra/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/");
                            string pathW = Path.Combine("~/Uploads/ContractFileExtra/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/");

                            if (!System.IO.Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }

                            List<string> uploadedFiles = new List<string>();
                            string pathes = "";
                            //foreach (IFormFile postedFile in postedFiles)
                            //{
                            string fileName = Path.Combine("~/Uploads/ContractFileExtra/" + BranchId + "/" + CustomerId + "/" + ProjectId + "/")
                                + UploadedFile.FileName; ;
                            //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                            var path2 = Path.Combine(path, fileName);
                            if (System.IO.File.Exists(path2))
                            {
                                System.IO.File.Delete(path2);
                            }
                            using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                            {


                                UploadedFile.CopyTo(stream);
                                uploadedFiles.Add(fileName);
                                // string returnpath = host + path + fileName;
                                //pathes.Add(pathW + fileName);
                                pathes = pathW + fileName;
                            }


                            if (pathes != null)
                            {
                                contractsVM.AttachmentUrlExtra = filDown;
                            }
                        }
                        //Request.Files["UploadedFile"].SaveAs(fileLocation);
                        }
                        catch (Exception ex)
                        {
                            var massage = "";
                            if (_globalshared.Lang_G == "rtl")
                            {
                                massage = "فشل في رفع العقد";
                            }
                            else
                            {
                                massage = "Failed To Upload Contract Files";
                            }
                            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage });
                        }
                    }
                }
                var result = _contractservice.SaveContractFileExtra(contractsVM.ContractId, _globalshared.UserId_G, _globalshared.BranchId_G, contractsVM.AttachmentUrlExtra);
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "Saved Successfully";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
                {
                    result.ReasonPhrase = "Saved Falied";
                }
                return Ok(result);
            }

        [HttpPost("GetReportGrid")]
        public IActionResult GetReportGrid([FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] string[] Sortedlist, [FromForm] int Type, [FromForm] int? ManagerId)
        {

            ContractsVM contracts = new ContractsVM();
            contracts.dateFrom = FromDate;
            contracts.dateTo = ToDate;
            contracts.ManagerId = ManagerId;

            if (contracts.dateFrom != null && contracts.dateFrom != "" && contracts.dateTo != null && contracts.dateTo != "")
            {
                if (Type == 0)
                {
                    contracts.IsSearch = false;
                    contracts.IsChecked = false;
                    contracts.dateFrom = "";
                    contracts.dateTo = "";
                }
                else
                {
                    contracts.IsSearch = true;
                    contracts.IsChecked = true;
                }
            }
            else
            {
                contracts.IsSearch = false;
                contracts.IsChecked = false;
                contracts.dateFrom = "";
                contracts.dateTo = "";
            }

            ContractReportVM _contractReportVM = new ContractReportVM();

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someContracts = _contractservice.GetAllContractsBySearch(contracts, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            if (Type == 1 && ManagerId == null && contracts.IsSearch == false)
            {
                someContracts = _contractservice.GetAllContracts_B(_globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList().Where(x => x.TotalRemainingPayment > 0).ToList();
            }
            else
            {

            }
            
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            var ManagerName = "";
            if (ManagerId != null)
            {
                ManagerName = _usersService.GetUserById(ManagerId ?? 0, _globalshared.Lang_G).Result.FullName;

            }

            _contractReportVM.someContracts = someContracts;
            _contractReportVM.FromDate = FromDate;
            _contractReportVM.ToDate = ToDate;
            _contractReportVM.ManagerName = ManagerName;
            _contractReportVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            var objOrganization2 = _organizationservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _contractReportVM.Org_VD = objOrganization2;
            return Ok(_contractReportVM);
        }

        [HttpPost("GetReportGridCustomer")]
        public IActionResult GetReportGridCustomer([FromBody] reportParameterscontract reportParameterscontract)
        {
            List<ContractsVM> list = new List<ContractsVM>();
            reportresult results = new reportresult();
            ContractsVM contracts = new ContractsVM();
            contracts.dateFrom = reportParameterscontract.FromDate;
            contracts.dateTo = reportParameterscontract.ToDate;
            contracts.CustomerId = reportParameterscontract.CustomerId;

            if (contracts.dateFrom != null && contracts.dateFrom != "" && contracts.dateTo != null && contracts.dateTo != "")
            {
                if (reportParameterscontract.Type == 0)
                {
                    contracts.IsSearch = false;
                    contracts.IsChecked = false;
                    contracts.dateFrom = "";
                    contracts.dateTo = "";
                }
                else
                {
                    contracts.IsSearch = true;
                    contracts.IsChecked = true;
                }
            }
            else
            {
                contracts.IsSearch = false;
                contracts.IsChecked = false;
                contracts.dateFrom = "";
                contracts.dateTo = "";
            }


            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            if (reportParameterscontract.Type == 1 && reportParameterscontract.CustomerId == null && contracts.IsSearch == false)
            {
                //someContracts = _contractservice.GetAllContracts_B(BranchId, _globalshared.YearId_G).ToList().Where(x => x.TotalRemainingPayment > 0).ToList();
            }
            else
            {

            }

            VoucherFilterVM voucherFilterVM = new VoucherFilterVM();
            voucherFilterVM.Type = 2;
            voucherFilterVM.CustomerId = contracts.CustomerId;
            voucherFilterVM.dateFrom = contracts.dateFrom;
            voucherFilterVM.dateTo = contracts.dateTo;

            var someVoucher = _voucherService.GetAllVouchersSearchCustomer(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            var result = someVoucher
                .GroupBy(s => new { s.CustomerId, s.CustomerMobile, s.CustomerName, s.CustomerName_W })
                .Select(cl => new ContractsVM
                {
                    CustomerId = cl.First().CustomerId,
                    CustomerName = cl.First().CustomerName ?? "",
                    CustomerName_W = cl.First().CustomerName_W ?? "",
                    CustomerMobile = cl.First().CustomerMobile ?? "",
                    TotalValue = cl.Sum(c => c.TotalValue),
                }).ToList();
            foreach (var inv in result)
            {
                decimal TotalPaidPaymentResult = 0;
                decimal resultReVoucher = 0;
                decimal resultEntryVoucher = 0;
                decimal resultInvoicePayVoucher = 0;
                decimal resultInvoicePayVoucherCredit = 0;

                //var resultReVoucher = _voucherService.VousherRe_Sum(inv.InvoiceId);
                var AccountResult = _customerservice.GetCustomersByCustomerId(inv.CustomerId ?? 0, _globalshared.Lang_G).Result;
                if (AccountResult != null)
                {
                    var Allresult = _transactionsService.GetAllTransByAccountId(AccountResult.AccountId, "", "", _globalshared.YearId_G).Result;
                    resultReVoucher = Allresult.Where(s => s.Type == 6 && s.IsPost == true).Sum(t => t.Credit) ?? 0;
                    resultEntryVoucher = Allresult.Where(s => s.Type == 8 && s.IsPost == true).Sum(t => (t.Credit - t.Depit)) ?? 0;
                    resultInvoicePayVoucher = someVoucher.Where(s => s.Type == 2 && s.IsPost == true && s.CustomerId == inv.CustomerId).Sum(t => t.PaidValue) ?? 0;
                    var PayVoucherCredit = Allresult.Where(s => s.Type == 29 && s.IsPost == true).ToList();
                    foreach (var item in PayVoucherCredit)
                    {
                        var NotiCredit = _voucherService.GetVoucherById(item.InvoiceId ?? 0).Result;
                        if (NotiCredit != null)
                        {
                            var InvoiceNotiCredit = _voucherService.GetVoucherById(NotiCredit.CreditNotiId ?? 0).Result;

                            if (InvoiceNotiCredit != null)
                            {
                                if (InvoiceNotiCredit.PayType == 8 && InvoiceNotiCredit.Rad != true)
                                {
                                    resultInvoicePayVoucherCredit += item.Credit ?? 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    resultReVoucher = 0;
                    resultEntryVoucher = 0;
                    resultInvoicePayVoucher = 0;
                    resultInvoicePayVoucherCredit = 0;
                }


                TotalPaidPaymentResult = (resultReVoucher) + (resultEntryVoucher) + (resultInvoicePayVoucher);
                list.Add(new ContractsVM
                {
                    //ContractNo = "",
                    //Date = inv.Date,
                    TotalValue = inv.TotalValue - resultInvoicePayVoucherCredit,
                    CustomerName = inv.CustomerName ?? "",
                    CustomerName_W = inv.CustomerName_W ?? "",
                    CustomerMobile = inv.CustomerMobile ?? "",
                    //ProjectDescription="",
                    //ProjectName = inv.ProjectNo,
                    TotalPaidPayment = TotalPaidPaymentResult,
                    TotalRemainingPayment = ((inv.TotalValue - resultInvoicePayVoucherCredit) - TotalPaidPaymentResult),

                });
            }
            IEnumerable<ContractsVM> InvoicesList = list;
            //var Vouchers = someContracts.Union(InvoicesList);
            var Vouchers = InvoicesList;

            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            //  ReportPDF = Bayanatech.TameerUI.pdfHandler.ReportsOf7sabat.Contract(someContracts, FromDate, ToDate, infoDoneTasksReport);
            var CustomerName = "";
            if (reportParameterscontract.CustomerId != null && reportParameterscontract.CustomerId != 0)
            {
                CustomerName = _customerservice.GetCustomersByCustomerId((int)reportParameterscontract.CustomerId, _globalshared.Lang_G).Result.CustomerName;

            }
            if (Vouchers != null)
            {
                var AllValue = Vouchers.Sum(x => x.TotalValue);
                var Paidvalue = Vouchers.Sum(x => x.TotalPaidPayment);
                var RemainValue = Vouchers.Sum(x => x.TotalRemainingPayment);
                results.AllValue = AllValue.ToString();
                results.RemainValue = RemainValue.ToString();
                results.Paidvalue = Paidvalue.ToString();
            }
            results.InvoicesList = Vouchers.ToList();
            results.CustomerName = CustomerName;




            return Ok(results);

        }

      
        public class Contract_PDF
        {
            public ContractsVM? contract { get; set; }
            public CustomerVM? customer { get; set; }
            public List<ContractStageVM>? contractphases { get; set; }
            public List<ContractServicesVM>? conservices { get; set; }
            public List<CustomerPaymentsVM>? payment { get; set; }
            public EmployeesVM? Emp { get; set; }
            public ProjectVM? project { get; set; }
            public OrganizationsVM? Org_VD { get; set; }
            public BranchesVM? objBranch { get; set; }
            public string? dayname { get; set; }

        }

        [HttpGet("printnewcontract")]
        public IActionResult printnewcontract(int ContractId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Contract_PDF _contract_PDF = new Contract_PDF();

            var contract = _contractservice.GetContractbyid(ContractId).Result.FirstOrDefault();
            var contractphases = _ContractStage.GetAllphasesByContractId(ContractId).Result.ToList();
            var customer = _customerservice.GetCustomersByCustId(contract.CustomerId).Result.FirstOrDefault();
            var Emp = _employeeService.GetEmployeeById(contract.OrgEmpId ?? 0, _globalshared.Lang_G).Result;
            var project = _projectService.GetProjectById(_globalshared.Lang_G, contract.ProjectId ?? 0).Result;

            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            var objBranch = _branchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();

            var objOrganization2 = _organizationservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _contract_PDF.Org_VD = objOrganization2;

            _contract_PDF.contract = contract;
            _contract_PDF.contractphases = contractphases;
            _contract_PDF.customer = customer;
            _contract_PDF.Emp = Emp;
            _contract_PDF.project = project;
            _contract_PDF.objBranch = objBranch;
            _contract_PDF.Org_VD = objOrganization2;
            _contract_PDF.Org_VD = objOrganization2;

            return Ok(_contract_PDF);
        }




        [HttpGet("printaqsatdofaatContract")]
        public IActionResult printaqsatdofaatContract(int ContractId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Contract_PDF _contract_PDF = new Contract_PDF();

            var contract = _contractservice.GetContractbyid(ContractId).Result.FirstOrDefault();
            var customer = _customerservice.GetCustomersByCustId(contract.CustomerId).Result.FirstOrDefault();
            var Emp = _employeeService.GetEmployeeById(contract.OrgEmpId ?? 0, _globalshared.Lang_G).Result;
            var project = _projectService.GetProjectById(_globalshared.Lang_G, contract.ProjectId ?? 0).Result;

            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            var objBranch = _branchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
            string dayname = GetDayName((DateTime.ParseExact(contract.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
            var conservices = _contractService.GetContractservicenByid(ContractId).Result.ToList();
            var payment = _customerPaymentservice.GetAllCustomerPayments(Convert.ToInt32(ContractId)).Result.ToList();



            var objOrganization2 = _organizationservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _contract_PDF.Org_VD = objOrganization2;

            _contract_PDF.contract = contract;
            _contract_PDF.customer = customer;
            _contract_PDF.Emp = Emp;
            _contract_PDF.project = project;
            _contract_PDF.objBranch = objBranch;
            _contract_PDF.dayname = dayname;
            _contract_PDF.conservices = conservices;

            _contract_PDF.payment = payment;

            //ViewData["datasource"] = this.ActiveDataSource;
            return Ok(_contract_PDF);
        }

        [HttpPost("EditContractService")]

        public IActionResult EditContractService(Contracts contract)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _contractservice.EditContractService(contract, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);

        }



    }

    public class reportParameterscontract
    {
        public int? CustomerId { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public int? Type { get; set; }

    }

    public class reportresult
    {
        public List<ContractsVM>? InvoicesList { get; set; }
        public string? CustomerName { get; set; }
        public string? AllValue { get; set; }
        public string? Paidvalue { get; set; }
        public string? RemainValue { get; set; }
    }
}
