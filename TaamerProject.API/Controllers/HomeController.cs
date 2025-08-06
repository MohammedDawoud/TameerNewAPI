using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Reflection;
//using System.Web.Helpers;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class HomeController : ControllerBase
    {
        private readonly IServiceService _Servs;
        private readonly IEmployeesService _Serv;
        private readonly IEmployeesService _EmpsService;
        private IBranchesService _BranchesService;
        private IOrganizationsService _organizationsservice;
        private IProjectPhasesTasksService _ProjectPhasesTasksService;
        private IWorkOrdersService _workOrdersService;
        private IProjectService _ProjectService;
        private IUsersService _UsersService;
        private IHomeService _homerservice;
        private IBranchesService _branchesService;
        private IFiscalyearsService _fiscalyearsService;
        private IAccountsService _accountsService;
        private readonly IEmployeesService _employeeService;
        private readonly IUserMailsService _userMailsservice;
        private ISystemSettingsService _systemSettingsservice;
        private IProjectService _projectservice;
        private IUsersService _usersService;
        private IProjectPhasesTasksService _projectPhasesTasksservice;
        private IFileTypeService _fileTypeservice;
        private IInstrumentSourcesService _InstrumentSourcesservice;
        private IProjectPiecesService _ProjectPiecesservice;
        private IRegionTypesService _RegionTypesservice;
        private ITransactionTypesService _TransactionTypesservice;
        private IBuildTypesService _buildTypeservice;
        private ICityService _cityservice;
        private IPro_MunicipalService _Pro_MunicipalService;
        private IPro_SubMunicipalityService _Pro_SubMunicipalityService;
        private IPro_SuperContractorService _Pro_SuperContractorService;
        private IProUserPrivilegesService _proUserPrivilegesService;
        private ITaskTypeService _taskTypeservice;
        private readonly IFiscalyearsService _FiscalyearsService;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;


        // private TameerProDBEntities db = new TameerProDBEntities();

        private INotificationService _notificationService;
        private ICustomerService _customerService;
        private IExpRevenuExpensesService _expRevenuExpensesService;
        private IServiceService _serviceService;
        private ISupervisionsService _supervisionsService;
        private IOfficialDocumentsService _officialDocumentsService;
        private readonly ILicencesService _licences;
        public HomeController(IProjectService projectService, IProjectPhasesTasksService projectPhasesTasksService, IWorkOrdersService workOrdersService, IUsersService usersService,
             IHomeService homeService, IBranchesService branchesService, IAccountsService accountsService, INotificationService notificationService, ICustomerService customerService, IExpRevenuExpensesService expRevenuExpensesService, IServiceService serviceService, ISupervisionsService supervisionsService, IOfficialDocumentsService officialDocumentsService, IFiscalyearsService fiscalyearsService,
             IBranchesService branchesService1, IOrganizationsService organizationsService, IEmployeesService employeesService, IEmployeesService employeesService1,
             IServiceService serviceService1, IEmployeesService employeesService2, IUserMailsService userMailsService, ISystemSettingsService systemSettingsService,
             IProjectService projectService1, IProjectPhasesTasksService projectPhasesTasksService1, IFileTypeService fileTypeService, IUsersService usersService1,
             IInstrumentSourcesService instrumentSourcesService, IProjectPiecesService projectPiecesService, IRegionTypesService regionTypesService,
             ITransactionTypesService transactionTypesService, IBuildTypesService buildTypesService, ICityService cityService, IPro_MunicipalService pro_MunicipalService,
             IPro_SubMunicipalityService pro_SubMunicipalityService, IPro_SuperContractorService pro_SuperContractorService, IProUserPrivilegesService proUserPrivilegesService,
             ITaskTypeService taskTypeService, IFiscalyearsService fiscalyearsService1, IConfiguration _configuration
            , ILicencesService licences)
        {
            this._BranchesService = branchesService1;
            this._ProjectService = projectService;
            this._ProjectPhasesTasksService = projectPhasesTasksService;
            this._workOrdersService = workOrdersService;
            this._UsersService = usersService;
            this._homerservice = homeService;
            this._branchesService = branchesService;
            this._accountsService = accountsService;
            _notificationService = notificationService;
            _customerService = customerService;
            _expRevenuExpensesService = expRevenuExpensesService;
            _serviceService = serviceService;
            _supervisionsService = supervisionsService;
            _officialDocumentsService = officialDocumentsService;
            _fiscalyearsService = fiscalyearsService;
            this._organizationsservice = organizationsService;
            _EmpsService = employeesService;
            _Serv = employeesService1;
            _Servs = serviceService1;
            _employeeService = employeesService2;
            this._userMailsservice = userMailsService;
            this._systemSettingsservice = systemSettingsService;
            this._projectservice = projectService1;
            this._projectPhasesTasksservice = projectPhasesTasksService1;
            this._fileTypeservice = fileTypeService;
            this._usersService = usersService1;
            this._InstrumentSourcesservice = instrumentSourcesService;
            this._ProjectPiecesservice = projectPiecesService;
            this._RegionTypesservice = regionTypesService;
            this._TransactionTypesservice = transactionTypesService;
            this._buildTypeservice = buildTypesService;
            this._cityservice = cityService;
            this._Pro_MunicipalService = pro_MunicipalService;
            this._Pro_SubMunicipalityService = pro_SubMunicipalityService;
            this._Pro_SuperContractorService = pro_SuperContractorService;
            this._proUserPrivilegesService = proUserPrivilegesService;
            this._taskTypeservice = taskTypeService;
            this._FiscalyearsService = fiscalyearsService1;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            _licences = licences;
        }



        //public PartialViewResult Home()
        //{
        //    return PartialView("_dashboard");
        //}

        [HttpGet("PDFDownload")]
        public FileResult PDFDownload(int type = 1, string branch = "")
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            byte[] pdfByte = { 0 };

            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            switch (type)
            {
                case 1:

                    string[] infoAbouutToExpireReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "إقامات قاربت علي الانتهاء", objOrganization.LogoUrl, branch };
                    string[] columnAbouutToExpireReportHeader = { "رقم الهوية", "اسم الموظف", "الجنسية", "تاريخ الانتهاء", "تاريخ التنبيه", "القسم", "الفرع" };
                    List<rptGetResdencesAboutToExpireVM> DataResDencesAbouutToExpire = _EmpsService.GetResDencesAbouutToExpire(Con).Result.ToList();

                    DataTable listDataAbouutToExpire = ToDataTable(DataResDencesAbouutToExpire);

                    pdfByte = pdfClass.DencesAbouutToExpireReport(infoAbouutToExpireReport, columnAbouutToExpireReportHeader, listDataAbouutToExpire);
                    return File(pdfByte, "application/pdf");

                    break;

                case 2:

                    string[] infoDencesExpiredReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  إقامات منتهيه   ", objOrganization.LogoUrl, branch };
                    string[] columnDencesExpiredHeader = { "رقم الهوية", "اسم الموظف", "الجنسية", "تاريخ الانتهاء", "القسم", "الفرع" };
                    List<rptGetResdencesExpiredVM> DataDencesExpired = _EmpsService.GetResDencesExpired(Con).Result.ToList();

                    DataTable listDataDencesExpired = ToDataTable(DataDencesExpired);

                    pdfByte = pdfClass.ResDencesExpiredReport(infoDencesExpiredReport, columnDencesExpiredHeader, listDataDencesExpired);
                    return File(pdfByte, "application/pdf");

                    break;


                case 3:

                    string[] infoOfficialDocsAboutToExpireReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  وثائق قاربت علي الانتهاء   ", objOrganization.LogoUrl, branch };
                    string[] columnOfficialDocsAboutToExpireHeader = { "إسم الوثيقة", "رقم الوثيقة", "جهة الاصدار", "تاريخ الانتهاء", "تاريخ التنبيه", "الفرع" };
                    List<rptGetOfficialDocsAboutToExpire> DataOfficialDocsAboutToExpire = _EmpsService.GetOfficialDocsAboutToExpire(Con).Result.ToList();

                    DataTable listDataOfficialDocsAboutToExpire = ToDataTable(DataOfficialDocsAboutToExpire);

                    pdfByte = pdfClass.OfficialDocsAboutToExpireReport(infoOfficialDocsAboutToExpireReport, columnOfficialDocsAboutToExpireHeader, listDataOfficialDocsAboutToExpire);
                    return File(pdfByte, "application/pdf");

                    break;
                case 4:

                    string[] infoOfficialDocsExpiredReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "   وثائق منتهية   ", objOrganization.LogoUrl, branch };
                    string[] columnOfficialDocsExpiredHeader = { "إسم الوثيقة", "رقم الوثيقة", "جهة الاصدار", "تاريخ الانتهاء", "الفرع" };
                    List<rptGetOfficialDocsExpiredVM> DataOfficialDocsExpired = _EmpsService.GetOfficialDocsExpired(Con).Result.ToList();

                    DataTable listDataOfficialDocsExpired = ToDataTable(DataOfficialDocsExpired);

                    pdfByte = pdfClass.DataOfficialDocsExpiredReport(infoOfficialDocsExpiredReport, columnOfficialDocsExpiredHeader, listDataOfficialDocsExpired);
                    return File(pdfByte, "application/pdf");

                    break;
                case 5:

                    string[] infoEmpContractsAboutToExpireReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  عقود الموظفين   ", objOrganization.LogoUrl, branch };
                    string[] columnEmpContractsAboutToExpireHeader = { "رقم العقد", "إسم الموظف", "الجنسية", "القسم", "الفرع", "تاريخ الانتهاء" };
                    List<rptGetEmpContractsAboutToExpireVM> DataEmpContractsAboutToExpire = _Serv.GetEmpContractsAboutToExpire(Con).ToList();

                    DataTable listDataEmpContractsAboutToExpire = ToDataTable(DataEmpContractsAboutToExpire);

                    pdfByte = pdfClass.DataEmpContractsAboutToExpire(infoEmpContractsAboutToExpireReport, columnEmpContractsAboutToExpireHeader, listDataEmpContractsAboutToExpire);
                    return File(pdfByte, "application/pdf");

                    break;
                case 8:

                    string[] infoDeservedServicesReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  الفواتير و الخدمات   ", objOrganization.LogoUrl, branch };
                    string[] columnDeservedServicesHeader = { "رقم الخدمة", "رقم الحساب", "جهة الاصدار", "تاريخ الاستحقاق", "الفرع" };
                    List<rptGetDeservedServicesVM> DataDeservedServices = _Servs.GetDeservedServices(Con).ToList();

                    DataTable listDeservedServices = ToDataTable(DataDeservedServices);

                    pdfByte = pdfClass.DataDeservedServices(infoDeservedServicesReport, columnDeservedServicesHeader, listDeservedServices);
                    return File(pdfByte, "application/pdf");

                    break;
                default:
                    break;
            }
            return File(pdfByte, "application/pdf");

        }

        [HttpGet("PDFDownloadIndexUser")]
        public FileResult PDFDownloadIndexUser(int type = 1, string branch = "", string FromDate = "", string ToDate = "")
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            byte[] pdfByte = { 0 };

            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            switch (type)
            {
                case 1:

                    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "المهام المنجزة", objOrganization.LogoUrl, branch };
                    string[] columnDoneTasksReportHeader = { "المهمة", "المدة", "رقم المشروع", "العميل", "إلى تاريخ", "التكلفة" };
                    List<rptGetEmpDoneTasksVM> DataDoneTasks = _ProjectPhasesTasksService.GetDoneTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listDoneTasks = ToDataTable(DataDoneTasks);

                    pdfByte = pdfClass.DataDoneTask(infoDoneTasksReport, columnDoneTasksReportHeader, listDoneTasks);
                    return File(pdfByte, "application/pdf");
                    break;

                case 2:

                    string[] infoDoneWorkOrderReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, " أوامر العمل المنجزة   ", objOrganization.LogoUrl, branch };
                    string[] columnDoneWorkOrderReportHeader = { "رقم الطلب", "المطلوب", "الأمر بواسطة", "العميل", "المدة", "تاريخ الانجاز" };
                    List<rptGetDoneWorkOrdersByExecEmp> DataDoneWorkOrder = _ProjectPhasesTasksService.GetEmpDoneWOsDGV( _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listDoneWorkOrder = ToDataTable(DataDoneWorkOrder);

                    pdfByte = pdfClass.DataDoneWorkOrder(infoDoneWorkOrderReport, columnDoneWorkOrderReportHeader, listDoneWorkOrder);
                    return File(pdfByte, "application/pdf");
                    break;

                case 3:

                    string[] infoRunningTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  المهام قيد الإنجاز", objOrganization.LogoUrl, branch };
                    string[] columnRunningTasksReportHeader = { "المهمة", "المدة", "رقم المشروع", "العميل", "من تاريخ", "الأهمية" };
                    List<rptGetEmpUndergoingTasksVM> DataRunningTasks = _ProjectPhasesTasksService.GetUndergoingTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listRunningTasks = ToDataTable(DataRunningTasks);

                    pdfByte = pdfClass.DataRunningTasks(infoRunningTasksReport, columnRunningTasksReportHeader, listRunningTasks);
                    return File(pdfByte, "application/pdf");
                    break;

                case 4:

                    string[] inforunningWorkOrderReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "   أوامر عمل قيد الإنجاز ", objOrganization.LogoUrl, branch };
                    string[] columnrunningWorkOrderReportHeader = { "رقم الطلب", "المطلوب", "الأمر بواسطة", "العميل", "المدة", "المدة المتبقية" };
                    List<rptGetOnGoingWorkOrdersByExecEmp> DatarunningWorkOrder = _ProjectPhasesTasksService.GetEmpUnderGoingWOsDGV( _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listrunningWorkOrder = ToDataTable(DatarunningWorkOrder);

                    pdfByte = pdfClass.DatarunningWorkOrder(inforunningWorkOrderReport, columnrunningWorkOrderReportHeader, listrunningWorkOrder);
                    return File(pdfByte, "application/pdf");
                    break;

                case 5:

                    string[] infoLateTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  المهام المتاخرة", objOrganization.LogoUrl, branch };
                    string[] columnLateTasksReportHeader = { "المهمة", "المدة", "رقم المشروع", "العميل", "من تاريخ", "مدة التأخير" };
                    List<rptGetEmpDelayedTasksVM> DataLateTasks = _ProjectPhasesTasksService.GetEmpDelayedTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listLateTasks = ToDataTable(DataLateTasks);

                    pdfByte = pdfClass.DataLateTasks(infoLateTasksReport, columnLateTasksReportHeader, listLateTasks);
                    return File(pdfByte, "application/pdf");
                    break;
                case 6:

                    string[] infoEmpDelayedWOReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "    اوامر عمل متأخرة", objOrganization.LogoUrl, branch };
                    string[] columnEmpDelayedWOReportHeader = { "رقم الطلب", "المطلوب", "الامر بواسطة", "العميل", "المدة", "مدة التأخير" };
                    List<rptGetDelayedWorkOrdersByExecEmpVM> DataEmpDelayedWO = _ProjectPhasesTasksService.GetEmpDelayedWOsDGV( _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listEmpDelayedWO = ToDataTable(DataEmpDelayedWO);

                    pdfByte = pdfClass.DataLateTasks(infoEmpDelayedWOReport, columnEmpDelayedWOReportHeader, listEmpDelayedWO);
                    return File(pdfByte, "application/pdf");
                    break;
                default:
                    break;
            }
            return File(pdfByte, "application/pdf");
        }
        [HttpGet("printPDFDirect")]
        public ActionResult printPDFDirect(int type = 1, string branch = "")
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            byte[] pdfByte = { 0 };

            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;


            switch (type)
            {
                case 1:

                    string[] infoAbouutToExpireReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "إقامات قاربت علي الانتهاء", objOrganization.LogoUrl, branch };
                    string[] columnAbouutToExpireReportHeader = { "رقم الهوية", "اسم الموظف", "الجنسية", "تاريخ الانتهاء", "تاريخ التنبيه", "القسم", "الفرع" };
                    List<rptGetResdencesAboutToExpireVM> DataResDencesAbouutToExpire = _EmpsService.GetResDencesAbouutToExpire(Con).Result.ToList();

                    DataTable listDataAbouutToExpire = ToDataTable(DataResDencesAbouutToExpire);

                    pdfByte = pdfClass.DencesAbouutToExpireReport(infoAbouutToExpireReport, columnAbouutToExpireReportHeader, listDataAbouutToExpire);

                    break;

                case 2:

                    string[] infoDencesExpiredReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  إقامات منتهيه   ", objOrganization.LogoUrl, branch };
                    string[] columnDencesExpiredHeader = { "رقم الهوية", "اسم الموظف", "الجنسية", "تاريخ الانتهاء", "القسم", "الفرع" };
                    List<rptGetResdencesExpiredVM> DataDencesExpired = _EmpsService.GetResDencesExpired(Con).Result.ToList();

                    DataTable listDataDencesExpired = ToDataTable(DataDencesExpired);

                    pdfByte = pdfClass.ResDencesExpiredReport(infoDencesExpiredReport, columnDencesExpiredHeader, listDataDencesExpired);

                    break;

                case 3:

                    string[] infoOfficialDocsAboutToExpireReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  وثائق قاربت علي الانتهاء   ", objOrganization.LogoUrl, branch };
                    string[] columnOfficialDocsAboutToExpireHeader = { "إسم الوثيقة", "رقم الوثيقة", "جهة الاصدار", "تاريخ الانتهاء", "تاريخ التنبيه", "الفرع" };
                    List<rptGetOfficialDocsAboutToExpire> DataOfficialDocsAboutToExpire = _EmpsService.GetOfficialDocsAboutToExpire(Con).Result.ToList();

                    DataTable listDataOfficialDocsAboutToExpire = ToDataTable(DataOfficialDocsAboutToExpire);

                    pdfByte = pdfClass.OfficialDocsAboutToExpireReport(infoOfficialDocsAboutToExpireReport, columnOfficialDocsAboutToExpireHeader, listDataOfficialDocsAboutToExpire);

                    break;

                case 4:

                    string[] infoOfficialDocsExpiredReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "   وثائق منتهية   ", objOrganization.LogoUrl, branch };
                    string[] columnOfficialDocsExpiredHeader = { "إسم الوثيقة", "رقم الوثيقة", "جهة الاصدار", "تاريخ الانتهاء", "الفرع" };
                    List<rptGetOfficialDocsExpiredVM> DataOfficialDocsExpired = _EmpsService.GetOfficialDocsExpired(Con).Result.ToList();

                    DataTable listDataOfficialDocsExpired = ToDataTable(DataOfficialDocsExpired);

                    pdfByte = pdfClass.DataOfficialDocsExpiredReport(infoOfficialDocsExpiredReport, columnOfficialDocsExpiredHeader, listDataOfficialDocsExpired);

                    break;
                case 5:

                    string[] infoEmpContractsAboutToExpireReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  عقود الموظفين   ", objOrganization.LogoUrl, branch };
                    string[] columnEmpContractsAboutToExpireHeader = { "رقم العقد", "إسم الموظف", "الجنسية", "القسم", "الفرع", "تاريخ الانتهاء" };
                    List<rptGetEmpContractsAboutToExpireVM> DataEmpContractsAboutToExpire = _Serv.GetEmpContractsAboutToExpire(Con).ToList();

                    DataTable listDataEmpContractsAboutToExpire = ToDataTable(DataEmpContractsAboutToExpire);

                    pdfByte = pdfClass.DataEmpContractsAboutToExpire(infoEmpContractsAboutToExpireReport, columnEmpContractsAboutToExpireHeader, listDataEmpContractsAboutToExpire);

                    break;

                case 8:

                    string[] infoDeservedServicesReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  الفواتير و الخدمات   ", objOrganization.LogoUrl, branch };
                    string[] columnDeservedServicesHeader = { "رقم الخدمة", "رقم الحساب", "جهة الاصدار", "تاريخ الاستحقاق", "الفرع" };
                    List<rptGetDeservedServicesVM> DataDeservedServices = _Servs.GetDeservedServices(Con).ToList();

                    DataTable listDeservedServices = ToDataTable(DataDeservedServices);

                    pdfByte = pdfClass.DataDeservedServices(infoDeservedServicesReport, columnDeservedServicesHeader, listDeservedServices);

                    break;

                default:
                    break;
            }
            //DB DATA 


            string existTemp =Path.Combine(@"~\TempFiles\");

            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            //File  
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = Path.Combine(@"~\TempFiles\") + FileName;

            //create and set PdfReader  
            System.IO.File.WriteAllBytes(FilePath, pdfByte);
            //return file 
            string FilePathReturn = @"TempFiles/" + FileName;
            return Content(FilePathReturn);
        }
        [HttpGet("printPDFDirectIndexUser")]
        public ActionResult printPDFDirectIndexUser(int type = 1, string branch = "", string FromDate = "", string ToDate = "")
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            byte[] pdfByte = { 0 };

            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;


            switch (type)
            {
                case 1:

                    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "المهام المنجزة", objOrganization.LogoUrl, branch };
                    string[] columnDoneTasksReportHeader = { "المهمة", "المدة", "رقم المشروع", "العميل", "إلى تاريخ", "التكلفة" };
                    List<rptGetEmpDoneTasksVM> DataDoneTasks = _ProjectPhasesTasksService.GetDoneTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listDoneTasks = ToDataTable(DataDoneTasks);

                    pdfByte = pdfClass.DataDoneTask(infoDoneTasksReport, columnDoneTasksReportHeader, listDoneTasks);

                    break;

                case 2:

                    string[] infoDoneWorkOrderReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, " أوامر العمل المنجزة   ", objOrganization.LogoUrl, branch };
                    string[] columnDoneWorkOrderReportHeader = { "رقم الطلب", "المطلوب", "الأمر بواسطة", "العميل", "المدة", "تاريخ الانجاز" };
                    List<rptGetDoneWorkOrdersByExecEmp> DataDoneWorkOrder = _ProjectPhasesTasksService.GetEmpDoneWOsDGV( _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listDoneWorkOrder = ToDataTable(DataDoneWorkOrder);

                    pdfByte = pdfClass.DataDoneWorkOrder(infoDoneWorkOrderReport, columnDoneWorkOrderReportHeader, listDoneWorkOrder);

                    break;

                case 3:

                    string[] infoRunningTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  المهام قيد الإنجاز", objOrganization.LogoUrl, branch };
                    string[] columnRunningTasksReportHeader = { "المهمة", "المدة", "رقم المشروع", "العميل", "من تاريخ", "الأهمية" };
                    List<rptGetEmpUndergoingTasksVM> DataRunningTasks = _ProjectPhasesTasksService.GetUndergoingTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listRunningTasks = ToDataTable(DataRunningTasks);

                    pdfByte = pdfClass.DataRunningTasks(infoRunningTasksReport, columnRunningTasksReportHeader, listRunningTasks);

                    break;

                case 4:

                    string[] inforunningWorkOrderReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "   أوامر عمل قيد الإنجاز ", objOrganization.LogoUrl, branch };
                    string[] columnrunningWorkOrderReportHeader = { "رقم الطلب", "المطلوب", "الأمر بواسطة", "العميل", "المدة", "المدة المتبقية" };
                    List<rptGetOnGoingWorkOrdersByExecEmp> DatarunningWorkOrder = _ProjectPhasesTasksService.GetEmpUnderGoingWOsDGV( _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listrunningWorkOrder = ToDataTable(DatarunningWorkOrder);

                    pdfByte = pdfClass.DatarunningWorkOrder(inforunningWorkOrderReport, columnrunningWorkOrderReportHeader, listrunningWorkOrder);

                    break;

                case 5:

                    string[] infoLateTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  المهام المتاخرة", objOrganization.LogoUrl, branch };
                    string[] columnLateTasksReportHeader = { "المهمة", "المدة", "رقم المشروع", "العميل", "من تاريخ", "مدة التأخير" };
                    List<rptGetEmpDelayedTasksVM> DataLateTasks = _ProjectPhasesTasksService.GetEmpDelayedTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listLateTasks = ToDataTable(DataLateTasks);

                    pdfByte = pdfClass.DataLateTasks(infoLateTasksReport, columnLateTasksReportHeader, listLateTasks);

                    break;
                case 6:

                    string[] infoEmpDelayedWOReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "    اوامر عمل متأخرة", objOrganization.LogoUrl, branch };
                    string[] columnEmpDelayedWOReportHeader = { "رقم الطلب", "المطلوب", "الامر بواسطة", "العميل", "المدة", "مدة التأخير" };
                    List<rptGetDelayedWorkOrdersByExecEmpVM> DataEmpDelayedWO = _ProjectPhasesTasksService.GetEmpDelayedWOsDGV( _globalshared.UserId_G, Con).Result.ToList();

                    DataTable listEmpDelayedWO = ToDataTable(DataEmpDelayedWO);

                    pdfByte = pdfClass.DataLateTasks(infoEmpDelayedWOReport, columnEmpDelayedWOReportHeader, listEmpDelayedWO);

                    break;

                default:
                    break;
            }
            //DB DATA 


            string existTemp = Path.Combine(@"~\TempFiles\");

            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            //File  
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = Path.Combine(@"~\TempFiles\") + FileName;

            //create and set PdfReader  
            System.IO.File.WriteAllBytes(FilePath, pdfByte);
            //return file 
            string FilePathReturn = @"TempFiles/" + FileName;
            return Content(FilePathReturn);
        }
        [HttpGet("PDFDownloadArchive")]
        public IActionResult PDFDownloadArchive(string branch, List<ProjectVM> listedData)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            byte[] pdfByte = { 0 };

            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, " بحث في الارشيف", objOrganization.LogoUrl, branch };
            string[] columnDoneTasksReportHeader = { "تاريخ الارشفه", "الحاله", "رقم المشروع", "العميل", "نوع المشروع", "رقم العقد", "المنطقه", "مدير المشروع", "مده المشروع", "المستخدم" };
            string[] columnHeader = { "FinishDate", "ReasonText", "ProjectNo", "CustomerName", "ProjectTypesName", "ContractNo", "CityName", "ProjectMangerName", "TimeStr", "UpdateUser" };
            //List<rptGetEmpDoneTasksVM> DataDoneTasks = _ProjectPhasesTasksService.GetDoneTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con).ToList();
            // var myList = JsonConvert.DeserializeObject<List<ProjectVM>>(listedData);
            DataTable listArchive = ToDataTable(listedData);

            pdfByte = pdfClass.DataArchive(infoDoneTasksReport, columnDoneTasksReportHeader, listArchive, columnHeader);

            string existTemp =Path.Combine(@"~\TempFiles\");

            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            //File  
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath =Path.Combine(@"~\TempFiles\") + FileName;

            //create and set PdfReader  
            System.IO.File.WriteAllBytes(FilePath, pdfByte);
            //return file 
            string FilePathReturn = FileName;

            return Ok(FilePathReturn); 

        }

        [HttpGet("DownloadPDF")]
        public FileResult DownloadPDF(string fileName)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string fullPath = Path.Combine(Path.Combine("~/TempFiles"), fileName);
            byte[] FileBytes = System.IO.File.ReadAllBytes(fullPath);
            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            return File(FileBytes, "application/pdf");

        }

        [HttpPost("PDFDownloadCustomer")]
        public IActionResult PDFDownloadCustomer(string branch, List<CustomerVM> listDataed)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            byte[] pdfByte = { 0 };

            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, " بحث في العملاء", objOrganization.LogoUrl, branch };
            string[] columnDoneTasksReportHeader = { "أسم العميل", "النوع", "رقم الهوية", "البريد الالكتروني", "رقم الهاتف", "رقم الجوال", "رقم الحساب" };
            string[] columnHeader = { "CustomerName", "CustomerTypeName", "CustomerNationalId", "CustomerEmail", "CustomerPhone", "CustomerMobile", "AccountId" };
            //List<rptGetEmpDoneTasksVM> DataDoneTasks = _ProjectPhasesTasksService.GetDoneTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con).ToList();
            // var myList = JsonConvert.DeserializeObject<List<ProjectVM>>(listedData);
            DataTable listArchive = ToDataTable(listDataed);

            pdfByte = pdfClass.DataCustomer(infoDoneTasksReport, columnDoneTasksReportHeader, listArchive, columnHeader);

            string existTemp =Path.Combine(@"~\TempFiles\");

            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            //File  
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath =Path.Combine(@"~\TempFiles\") + FileName;

            //create and set PdfReader  
            System.IO.File.WriteAllBytes(FilePath, pdfByte);
            //return file 
            string FilePathReturn = FileName;

            return Ok(FilePathReturn); ;

        }

        [HttpGet("DownloadPDFCustomer")]
        public FileResult DownloadPDFCustomer(string fileName)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string fullPath = Path.Combine(Path.Combine("~/TempFiles"), fileName);
            byte[] FileBytes = System.IO.File.ReadAllBytes(fullPath);
            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            return File(FileBytes, "application/pdf");

        }

        [HttpGet("NewChart")]
        public IActionResult NewChart()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            List<object> iData = new List<object>();
            //Creating sample data  
            DataTable dt = new DataTable();
            dt.Columns.Add("Employee", System.Type.GetType("System.String"));
            dt.Columns.Add("Credit", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Employee"] = "Sam";
            dr["Credit"] = 123;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Employee"] = "Alex";
            dr["Credit"] = 456;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Employee"] = "Michael";
            dr["Credit"] = 587;
            dt.Rows.Add(dr);
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Ok(iData);
        }

        //public ActionResult IndexUser()
        //{
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    var checkadmin = Request.Cookies["HomeIndex"].Value;
        //    if (checkadmin == "1")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    TameerProContext tameerProContext = new TameerProContext(Con);
        //    int? Raseed = 0;
        //    int? Consumed = 0;
        //    var year = _fiscalyearsService.GetActiveYear();
        //    //if (year != null)
        //    //{
        //    //    var yearslist = tameerProContext.FiscalYears.Where(w => w.YearId <= year.YearId).Select(s => s.YearId).ToList();
        //    //    try
        //    //    {
        //    //        Raseed = tameerProContext.Emp_VacationsStat.Where(w => w.UserId == _globalshared.UserId_G && yearslist.Contains(w.Year)).Sum(s => s.Balance);
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        Raseed = -1000000;
        //    //    }

        //    //    try
        //    //    {
        //    //        Consumed = tameerProContext.Emp_VacationsStat.Where(w => w.UserId == _globalshared.UserId_G && yearslist.Contains(w.Year)).Sum(s => s.Consumed);
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        Consumed = -1000000;
        //    //    }

        //    //}
        //    //ViewBag.Raseed = Raseed;
        //    //ViewBag.Consumed = Consumed;


        //    ViewBag.Users = _UsersService.GetAllUsers();
        //    ViewBag.AllUserTasks = _ProjectPhasesTasksService.GetTasksByUserId( _globalshared.UserId_G, 0, BranchId);
        //    ViewBag.Statistics = _homerservice.GetAllStatistics(BranchId, YearNEW);
        //    ViewBag.UserStatistics = _homerservice.GetAllUserStatistics( _globalshared.UserId_G, BranchId, _globalshared.Lang_G);
        //    string NDay = DateTime.Now.Day.ToString();
        //    if (Convert.ToInt32(NDay) < 10)
        //    {
        //        NDay = "0" + NDay;
        //    }
        //    string NMonth = DateTime.Now.Month.ToString();
        //    if (Convert.ToInt32(NMonth) < 10)
        //    {
        //        NMonth = "0" + NMonth;
        //    }
        //    ViewBag.AllUserNewTasks = _ProjectPhasesTasksService.GetNewTasksByUserId(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, _globalshared.UserId_G, BranchId, _globalshared.Lang_G);
        //    ViewBag.AllUserLateTasks = _ProjectPhasesTasksService.GetLateTasksByUserIdHome(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, _globalshared.UserId_G, BranchId, _globalshared.Lang_G);
        //    ViewBag.AllUserLateWorkOrders = _workOrdersService.GetLateWorkOrdersByUserId(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, _globalshared.UserId_G, BranchId);

        //    return View();
        //}



        //public ActionResult ChangeLanguage(string Lang)
        //{
        //    try
        //    {
        //        var cultureInfo = new CultureInfo(Lang);
        //        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        //        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
        //        HttpCookie _globalshared.Lang_GCookie = new HttpCookie("culture", _globalshared.Lang_G);
        //        _globalshared.Lang_GCookie.Expires = DateTime.Now.AddYears(1);
        //        HttpContext.Response.Cookies.Add(_globalshared.Lang_GCookie);
        //    }
        //    catch (Exception) { }
        //    return Redirect(Request.UrlReferrer.ToString());
        //}
        [HttpGet("GetUserStatistics")]
        public ActionResult GetUserStatistics()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _homerservice.GetAllUserStatistics(_globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpGet("GetUserStatisticsPercentData")]
        public ActionResult GetUserStatisticsPercentData()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _homerservice.GetUserStatisticsPercentData(_globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpGet("GetAllUserCustodiesStatistics")]
        public ActionResult GetAllUserCustodiesStatistics()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _homerservice.GetAllUserCustodiesStatistics(_globalshared.UserId_G);
            return Ok(result);
        }

        [HttpGet("GetProjectExpRev")]
        public ActionResult GetProjectExpRev(string CostCenter)
        {
            var result = _accountsService.GetProjectExpRev(CostCenter, Con);
            return Ok(result);
        }

        //public ActionResult Popup()
        //{
        //    return View();
        //}
        [HttpGet("GetOnlineUsers")]
        public ActionResult GetOnlineUsers()
        {
          //  string Con = ConfigurationManager.ConnectionStrings["TameerProConn"].ConnectionString;
            //var result = _UsersService.GetOnlineUsers();
            var result = _UsersService.GetAllUsersOnline2().Result.Count();
            return Ok(result);
        }
        [HttpGet("GetOnlineUsers2")]
        public ActionResult GetOnlineUsers2()
        {
            var result = _UsersService.GetAllUsersOnline2().Result.Count();
            return Ok(result);
        }
        [HttpGet("GetLayoutVm")]
        public ActionResult GetLayoutVm()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _homerservice.GetLayoutVm( _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpGet("GetHomeVm")]
        public ActionResult GetHomeVm()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = new
            {
                GetTasksByUserIdHome = _ProjectPhasesTasksService.GetTasksByUserIdHome( _globalshared.UserId_G, 1, _globalshared.BranchId_G),
                GetWorkOrdersByUserId = _workOrdersService.GetWorkOrdersByUserId( _globalshared.UserId_G, _globalshared.BranchId_G),
                GetOfficialPapersStatitecs = _notificationService.GetOfficialDocsStatsecs(Con),
                //GetprojectCount = _ProjectService.GetAllProject(_globalshared.Lang_G, BranchId).Count(),
                //GetCustomerCount = _customerService.GetAllCustomersCount(BranchId).Count(),
                // GetTasksByUserId = _ProjectPhasesTasksService.GetTasksByUserId( _globalshared.UserId_G, 0, BranchId),
                //GetTotalExpRevByCC = _expRevenuExpensesService.GetTotalExpRevByCC(Con),
                GetServicesToNotified = _serviceService.GetServicesToNotified(_globalshared.BranchId_G, _globalshared.Lang_G),
                GetAllSupervisionsByUserIdHome = _supervisionsService.GetAllSupervisionsByUserIdHome( _globalshared.UserId_G),
                GetNotifiedDocuments = _officialDocumentsService.GetDocumentToNotified(_globalshared.BranchId_G, _globalshared.Lang_G),
            };
            return Ok(result);
        }
        [HttpGet("GetProjectVM")]
        public ActionResult GetProjectVM()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = new
            {
                GetProjectsStoppedVMCount = _ProjectService.GetProjectsStoppedVM().Result.Count(),
                GetProjectsWithoutContractVMVMCount = _ProjectService.GetProjectsWithoutContractVM().Result.Count(),
                GetProjectsLateVMCount = _ProjectService.GetProjectsLateVM().Result.Count(),
                GetProjectsWithProSettingVMCount = _ProjectPhasesTasksService.GetProjectsWithProSettingVM().Result.Count(),
                GetProjectsWithoutProSettingVMCount = _ProjectPhasesTasksService.GetProjectsWithoutProSettingVM().Result.Count(),
                GetProjectsSupervisionVMVMCount = _supervisionsService.GetAllSupervisionsProject(_globalshared.BranchId_G).Count(),
                GetdestinationsUploadVMCount = _ProjectService.GetdestinationsUploadVM().Result.Count(),
            };
            return Ok(result);
        }

        [HttpGet("GetProjectVMNew")]
        public IActionResult GetProjectVMNew()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var FullReport = _ProjectService.GetProjectVMNew(_globalshared.Lang_G, Con??"", _globalshared.BranchId_G).Result.FirstOrDefault();
            return Ok(FullReport);
        }
        [HttpGet("GetProjectVMStatNew")]
        public IActionResult GetProjectVMStatNew(int ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var FullReport = _ProjectService.GetProjectVMStatNew(ProjectId, _globalshared.Lang_G, Con ?? "", _globalshared.BranchId_G).Result.FirstOrDefault();
            return Ok(FullReport);
        }

        [HttpGet("ToDataTable")]
        public DataTable ToDataTable<T>(List<T> items)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        [HttpGet("ActiveYear")]
        public ActionResult ActiveYear()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_globalshared.YearId_G);
        }

        [HttpGet("GetHostingExpireAlert")]
        public ActionResult GetHostingExpireAlert()
        {
            var items = _licences.GetAllLicences("").Result.ToList();
            if (items != null) {
               var meassege= items.Select(item => new
                 {

                     ExpireDate = item.Support_Expiry_Date,
                     Message = (DateTime.ParseExact(item.Support_Expiry_Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) - DateTime.UtcNow).TotalDays switch
                     {
                         > 0 and < 30 => $"الاشتراك سينتهي خلال ( {(int)((DateTime.ParseExact(item.Support_Expiry_Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)) - DateTime.UtcNow).TotalDays}   يوم)  فيٍ {item.Support_Expiry_Date}",
                         _ => ""
                     }
                 }).FirstOrDefault();
                if (meassege.Message == "")
                {
                    return Ok(null);
                }
                else
                {
                    return Ok(meassege);
                }
            }
            else
            {
                return Ok(null);
            }
        }


        [HttpGet("GetSystemSettingsByUserId")]
        public ActionResult GetSystemSettingsByUserId()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var SystemSettingsByBranchId_M = _systemSettingsservice.GetSystemSettingsByUserId(orgId, _globalshared.UserId_G, Con);

            return Ok(SystemSettingsByBranchId_M);
        }

        [HttpGet("GetLayoutReadyVm")]
        public ActionResult GetLayoutReadyVm(int FiscalId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            var ProjectCount = 0;
            ProjectCount = _ProjectService.GetUserProjects2( _globalshared.UserId_G, _globalshared.BranchId_G, "").Result.Count();
            var emp = _employeeService.GetEmployeeByUserid( _globalshared.UserId_G).Result.FirstOrDefault();
            int VactionBalance = 0;
            var VactionBalancestr = "";
            if (emp != null)
            {
                VactionBalance = emp.VacationEndCount ?? 0;
            }
            VactionBalancestr = (VactionBalance < 30) ? VactionBalance + " يوم " : (VactionBalance == 30) ? VactionBalance / 30 + " شهر " : (VactionBalance > 30) ? ((VactionBalance / 30) + " شهر " + (VactionBalance % 30) + " يوم  ") : "";


            var ActiveYearID_M = _fiscalyearsService.GetActiveYearID(FiscalId);
            var BackupAlertLoad_M = _notificationService.GetUserBackupNotesAlert( _globalshared.UserId_G);
            var ItemsAccountload_M = _accountsService.GetAccountsByType("ايرادات", _globalshared.Lang_G);
            var ValueAdded_Global_M = _organizationsservice.GetBranchOrganizationData(orgId);
            var UpdateActiveTime_M = _UsersService.UpdateActiveTime( _globalshared.UserId_G, _globalshared.BranchId_G);
            var SystemSettingsByBranchId_M = _systemSettingsservice.GetSystemSettingsByUserId(orgId, _globalshared.UserId_G, Con);
            var NotificationsCount_M = _notificationService.GetNotificationReceived( _globalshared.UserId_G).Result.Where(m => m.IsRead != true).Count();
            var AllertCount_M = _notificationService.GetUserAlert( _globalshared.UserId_G).Result.Count();
            var TasksByUserCount_M = _ProjectPhasesTasksService.GetTasksByUserId( _globalshared.UserId_G, 0, _globalshared.BranchId_G).Result.Count();
            var MyInboxCount_M = _userMailsservice.GetAllUserMails( _globalshared.UserId_G, _globalshared.BranchId_G).Result.Count();


            var userBranchs2 = _branchesService.GetAllBranches(_globalshared.Lang_G).Result;

            var someProject = _projectservice.GetAllProjects3(Con, _globalshared.Lang_G, 0, _globalshared.UserId_G).Result;
            foreach (var userBranch in userBranchs2)
            {

                var AllPojects = _projectservice.GetAllProjects3(Con, _globalshared.Lang_G, userBranch.BranchId, _globalshared.UserId_G).Result.ToList();

                var Projects = someProject.Union(AllPojects);
                someProject = Projects.ToList();
            }
            MaxProjevtAndTasksVM mxpro = new MaxProjevtAndTasksVM();
            if (someProject.Count() > 0)
            {


                var max = someProject.GroupBy(n => n.MangerId).OrderByDescending(g => g.Count()).First();
                mxpro.Count = max.Count();
                var maxpro = max.ToList().FirstOrDefault();
                mxpro.ManagerId = maxpro.MangerId;

                mxpro.ManagerName = maxpro.ProjectMangerName;
                mxpro.ImgUrl = _usersService.GetUserById((int)maxpro.MangerId, _globalshared.Lang_G).Result.ImgUrl;
                //  mxpro.ImgUrl = _usersService.GetUserById(1, _globalshared.Lang_G).ImgUrl;
            }
            else
            {
                mxpro.Count = 0;
                mxpro.ImgUrl = "/distnew/images/userprofile.png";

            }

            var someTask = _projectPhasesTasksservice.GetAllProjectPhasesTasks("", 0, _globalshared.Lang_G).Result;
            MaxProjevtAndTasksVM mxprotask = new MaxProjevtAndTasksVM();
            var alltasks = someTask.Where(x => x.UserId != 0 & x.Status != 4);
            if (alltasks.Count() > 0)
            {


                var max = alltasks.GroupBy(n => n.UserId).OrderByDescending(g => g.Count()).First();
                mxprotask.Count = max.Count();
                var maxpro = max.ToList().FirstOrDefault();
                mxprotask.ManagerId = maxpro.UserId;

                mxprotask.ManagerName = maxpro.ProjectMangerName;
                mxprotask.ImgUrl = _usersService.GetUserById((int)maxpro.UserId, _globalshared.Lang_G).Result.ImgUrl;
                //mxpro.ImgUrl = _usersservice.GetUserById(1, _globalshared.Lang_G).ImgUrl;
            }
            else
            {
                mxprotask.Count = 0;
                mxprotask.ImgUrl = "/distnew/images/userprofile.png";

            }

            string SelectStetment = "";

            if ( _globalshared.UserId_G == 1)
            {
                SelectStetment = "select distinct FiscalId as Id, CAST(fis.YearId AS nvarchar) + '-01' + '-01' + ' / ' + CAST(fis.YearId AS nvarchar) + '-12' + '-31' as Name from Acc_FiscalYears fis where fis.IsDeleted=0";
            }
            else
            {
                SelectStetment = "select distinct FiscalId as Id, CAST(fis.YearId AS nvarchar) + '-01' + '-01' + ' / ' + CAST(fis.YearId AS nvarchar) + '-12' + '-31' as Name from Acc_FiscalYears fis left join Acc_EmpFinYears empfin on fis.FiscalId = empfin.YearID where (IsNULL(fis.IsDeleted,1)=0) and ((fis.IsActive=1) or (fis.IsActive=0 and (IsNULL(empfin.IsDeleted,1)=0))) and ((empfin.EmpID)=" + _globalshared.UserId_G + " or (fis.IsActive=1))and ((empfin.BranchID)= " + _globalshared.BranchId_G + " or (fis.IsActive=1))";
            }


            var FiscalyearsPriv_M = _accountsService.FillAccountSelect(Con, SelectStetment);

            //string ActiveYearID_MJson = new JavaScriptSerializer().Serialize(ActiveYearID_M);
            //var ActiveYearID_Mcookie = new HttpCookie("ActiveYearID_MKey", ActiveYearID_MJson)
            //{ Expires = DateTime.Now.AddYears(1)};
            //HttpContext.Response.Cookies.Add(ActiveYearID_Mcookie);

            //string BackupAlertLoad_MJson = new JavaScriptSerializer().Serialize(BackupAlertLoad_M);
            //var BackupAlertLoad_Mcookie = new HttpCookie("BackupAlertLoad_MKey", BackupAlertLoad_MJson)
            //{ Expires = DateTime.Now.AddYears(1) };
            //HttpContext.Response.Cookies.Add(BackupAlertLoad_Mcookie);

            //string ItemsAccountload_MJson = new JavaScriptSerializer().Serialize(ItemsAccountload_M);
            //var ItemsAccountload_Mcookie = new HttpCookie("ItemsAccountload_MKey", ItemsAccountload_MJson)
            //{ Expires = DateTime.Now.AddYears(1) };
            //HttpContext.Response.Cookies.Add(ItemsAccountload_Mcookie);

            //string ValueAdded_Global_MJson = new JavaScriptSerializer().Serialize(ValueAdded_Global_M);
            //var ValueAdded_Global_Mcookie = new HttpCookie("ValueAdded_Global_MKey", ValueAdded_Global_MJson)
            //{ Expires = DateTime.Now.AddYears(1) };
            //HttpContext.Response.Cookies.Add(ValueAdded_Global_Mcookie);

            //string SystemSettingsByBranchId_MJson = new JavaScriptSerializer().Serialize(SystemSettingsByBranchId_M);
            //var SystemSettingsByBranchId_Mcookie = new HttpCookie("SystemSettingsByBranchId_MKey", SystemSettingsByBranchId_MJson)
            //{ Expires = DateTime.Now.AddYears(1) };
            //HttpContext.Response.Cookies.Add(SystemSettingsByBranchId_Mcookie);

            //string NotificationsCount_MJson = new JavaScriptSerializer().Serialize(NotificationsCount_M);
            //var NotificationsCount_Mcookie = new HttpCookie("NotificationsCount_MKey", NotificationsCount_MJson)
            //{ Expires = DateTime.Now.AddYears(1) };
            //HttpContext.Response.Cookies.Add(NotificationsCount_Mcookie);

            //string AllertCount_MJson = new JavaScriptSerializer().Serialize(AllertCount_M);
            //var AllertCount_Mcookie = new HttpCookie("AllertCount_MKey", AllertCount_MJson)
            //{ Expires = DateTime.Now.AddYears(1) };
            //HttpContext.Response.Cookies.Add(AllertCount_Mcookie);

            //string TasksByUserCount_MJson = new JavaScriptSerializer().Serialize(TasksByUserCount_M);
            //var TasksByUserCount_Mcookie = new HttpCookie("TasksByUserCount_MKey", TasksByUserCount_MJson)
            //{ Expires = DateTime.Now.AddYears(1) };
            //HttpContext.Response.Cookies.Add(TasksByUserCount_Mcookie);

            //string MyInboxCount_MJson = new JavaScriptSerializer().Serialize(MyInboxCount_M);
            //var MyInboxCount_Mcookie = new HttpCookie("MyInboxCount_MKey", MyInboxCount_MJson)
            //{ Expires = DateTime.Now.AddYears(1) };
            //HttpContext.Response.Cookies.Add(MyInboxCount_Mcookie);


            var result = new
            {
                Branch_DropLoad = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result.Select(x => new
                {
                    Id = x.BranchId,
                    Name = x.BranchName
                }),
                //YearFormat_DropLoad = _fiscalyearsService.GetAllFiscalyears().Select(s => new {
                //    Id = s.FiscalId,
                //    Name = s.YearId.ToString() + "-01" + "-01" + " / " + s.YearId.ToString() + "-12" + "-31",
                //}),
                YearFormat_DropLoad = FiscalyearsPriv_M,
                ActiveYearID = ActiveYearID_M,
                BackupAlertLoad = BackupAlertLoad_M,
                ItemsAccountload = ItemsAccountload_M,
                ValueAdded_Global = ValueAdded_Global_M,
                UpdateActiveTime = UpdateActiveTime_M,
                SystemSettingsByBranchId = SystemSettingsByBranchId_M,
                NotificationsCount = NotificationsCount_M,
                AllertCount = AllertCount_M,
                TasksByUserCount = TasksByUserCount_M,
                MyInboxCount = MyInboxCount_M,
                GetUserProjects = ProjectCount,
                VacationBalance = VactionBalancestr,
                MaxProject = mxpro,
                MaxTask = mxprotask

            };
            return Ok(result);
        }
        [HttpGet("GetProDetailsReadyVm")]
        public ActionResult GetProDetailsReadyVm(int ProjectId, int ProMangerID)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var NotificationFileType_M = _fileTypeservice.FillFileTypeSelect(_globalshared.BranchId_G);
            var ProManger_M = _UsersService.FillAllUsersSelectAll( _globalshared.UserId_G);
            var workOrder_M = _workOrdersService.GetAllWorkOrdersyProjectId(ProjectId);
            var users_M = _usersService.GetUserById(ProMangerID, _globalshared.Lang_G);
            var Privs_M = _proUserPrivilegesService.GetAllPrivUser( _globalshared.UserId_G, ProjectId);
            var result = new
            {
                ProdetailsInstrumentSources_DropLoad = _InstrumentSourcesservice.GetAllInstrumentSources("").Result.Select(x => new
                {
                    Id = x.SourceId,
                    Name = x.NameAr,
                    NameEn = x.NameEn,
                }),
                Contractor_DropLoad = _Pro_SuperContractorService.GetAllSuperContractor("").Result.Select(x => new
                {
                    Id = x.ContractorId,
                    Name = x.NameAr,
                    NameEn = x.NameEn,
                    Email = x.Email,
                    CommercialRegister = x.CommercialRegister,
                    PhoneNo = x.PhoneNo,
                }),
                WorkOrderProDetPicNo_DropLoad = _ProjectPiecesservice.GetAllProjectPieces(ProjectId, "").Result.Select(x => new
                {
                    Id = x.PieceId,
                    Name = x.PieceNo,
                    Notes = x.Notes,
                    ProjectId = x.ProjectId,
                }),
                ProjectDetailsRegion_DropLoad = _RegionTypesservice.GetAllRegionTypes("").Result.Select(x => new
                {
                    Id = x.RegionTypeId,
                    Name = x.NameAr,
                    NameEn = x.NameEn,
                }),
                ProjectDetailsTransaction_DropLoad = _TransactionTypesservice.GetAllTransactionTypes("").Result.Select(x => new
                {
                    Id = x.TransactionTypeId,
                    Name = x.NameAr,
                    NameEn = x.NameEn,
                }),
                ProBuilding_DropLoad = _buildTypeservice.GetAllBuildTypes("").Result.Select(x => new
                {
                    Id = x.BuildTypeId,
                    Name = x.NameAr,
                    NameEn = x.NameEn,
                    Description = x.Description,
                }),
                RegionName_DropLoad = _cityservice.GetAllCities("").Result.Select(x => new
                {
                    Id = x.CityId,
                    Name = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                }),
                Municipal_DropLoad = _Pro_MunicipalService.GetAllMunicipals("").Result.Select(x => new
                {
                    Id = x.MunicipalId,
                    Name = x.NameAr,
                    NameEn = x.NameEn,
                }),
                SubMunicipality_DropLoad = _Pro_SubMunicipalityService.GetAllSubMunicipalitys("").Result.Select(x => new
                {
                    Id = x.SubMunicipalityId,
                    Name = x.NameAr,
                    NameEn = x.NameEn,
                }),

                NotificationFileType_DropLoad = NotificationFileType_M,
                ProManger_DropLoad = ProManger_M,
                workOrder = workOrder_M,
                users = users_M,
                Privs = Privs_M,
            };
            return Ok(result);
        }
        [HttpGet("GetAddTaskReadyVm")]
        public ActionResult GetAddTaskReadyVm(int CustomerId, int ProjectId, int ActiveMainPhaseId, int? Flag, bool? FlagUserAll)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var CurrentUser_M = _usersService.GetUserById( _globalshared.UserId_G, _globalshared.Lang_G);

            var TaskType_DropLoad_M = _taskTypeservice.FillTaskTypeSelectAE(_globalshared.BranchId_G);

            var UsersAll_M = _UsersService.FillAllUsersSelectAll( _globalshared.UserId_G);
            var Users_M = _UsersService.FillAllUserSelect( _globalshared.UserId_G);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            var someProject = _projectservice.GetAllProject(_globalshared.Lang_G, 0).Result.Where(t => t.CustomerId == CustomerId).Select(s => new
            {
                Id = s.ProjectId,
                Name = s.ProjectNo + " - " + s.CustomerName,
            });
            foreach (var userBranch in userBranchs)
            {
                var AllPojects = _projectservice.GetAllProject(_globalshared.Lang_G, userBranch.BranchId).Result.Where(t => t.CustomerId == CustomerId).Select(s => new
                {
                    Id = s.ProjectId,
                    Name = s.ProjectNo + " - " + s.CustomerName,
                }).ToList();
                var Projects = someProject.Union(AllPojects);
                someProject = Projects.ToList();
            }

            var ContractProj_DropLoad_M = someProject;

            var MainPhase_DropLoad_M = _projectPhasesTasksservice.FillProjectMainPhases(ProjectId);
            var SubPhase_DropLoad_M = _projectPhasesTasksservice.FillProjectSubPhases(ActiveMainPhaseId);


            var someCustomer = _customerService.GetCustomersOwnProjects(_globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
            {
                Id = s.CustomerId,
                Name = s.CustomerName
            });
            foreach (var userBranch in userBranchs)
            {

                var AllCustomers = _customerService.GetCustomersOwnProjects(_globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName
                });
                var Customers = someCustomer.Union(AllCustomers);
                someCustomer = Customers;
            }
            var CustomersOwnProjects_M = someCustomer;

            var result = new
            {
                CurrentUser = CurrentUser_M,
                TaskType_DropLoad = TaskType_DropLoad_M,
                UsersAll = UsersAll_M,
                Users = Users_M,
                ContractProj_DropLoad = ContractProj_DropLoad_M,
                MainPhase_DropLoad = MainPhase_DropLoad_M,
                SubPhase_DropLoad = SubPhase_DropLoad_M,
                CustomersOwnProjects = CustomersOwnProjects_M,

            };
            return Ok(result);
        }


        [HttpGet("printPDFDirectNew")]
        public ActionResult printPDFDirectNew(int type = 1,int DepartmentId=0)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            byte[] pdfByte = { 0 };

            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result.NameAr;


            switch (type)
            {
                case 1:

                    string[] infoAbouutToExpireReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "إقامات قاربت علي الانتهاء", objOrganization.LogoUrl, branch };
                    string[] columnAbouutToExpireReportHeader = { "رقم الهوية", "اسم الموظف", "الجنسية", "تاريخ الانتهاء", "تاريخ التنبيه", "القسم", "الفرع" };
                    List<rptGetResdencesAboutToExpireVM> DataResDencesAbouutToExpire = _EmpsService.GetResDencesAbouutToExpire(Con, DepartmentId == 0 ? null : DepartmentId).Result.ToList();

                    DataTable listDataAbouutToExpire = ToDataTable(DataResDencesAbouutToExpire);

                    pdfByte = pdfClass.DencesAbouutToExpireReport(infoAbouutToExpireReport, columnAbouutToExpireReportHeader, listDataAbouutToExpire);

                    break;

                case 2:

                    string[] infoDencesExpiredReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  إقامات منتهيه   ", objOrganization.LogoUrl, branch };
                    string[] columnDencesExpiredHeader = { "رقم الهوية", "اسم الموظف", "الجنسية", "تاريخ الانتهاء", "القسم", "الفرع" };
                    List<rptGetResdencesExpiredVM> DataDencesExpired = _EmpsService.GetResDencesExpired(Con, DepartmentId == 0 ? null : DepartmentId).Result.ToList();

                    DataTable listDataDencesExpired = ToDataTable(DataDencesExpired);

                    pdfByte = pdfClass.ResDencesExpiredReport(infoDencesExpiredReport, columnDencesExpiredHeader, listDataDencesExpired);

                    break;

                case 3:

                    string[] infoOfficialDocsAboutToExpireReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  وثائق قاربت علي الانتهاء   ", objOrganization.LogoUrl, branch };
                    string[] columnOfficialDocsAboutToExpireHeader = { "إسم الوثيقة", "رقم الوثيقة", "جهة الاصدار", "تاريخ الانتهاء", "تاريخ التنبيه", "الفرع" };
                    List<rptGetOfficialDocsAboutToExpire> DataOfficialDocsAboutToExpire = _EmpsService.GetOfficialDocsAboutToExpire(Con, DepartmentId == 0 ? null : DepartmentId).Result.ToList();

                    DataTable listDataOfficialDocsAboutToExpire = ToDataTable(DataOfficialDocsAboutToExpire);

                    pdfByte = pdfClass.OfficialDocsAboutToExpireReport(infoOfficialDocsAboutToExpireReport, columnOfficialDocsAboutToExpireHeader, listDataOfficialDocsAboutToExpire);

                    break;

                case 4:

                    string[] infoOfficialDocsExpiredReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "   وثائق منتهية   ", objOrganization.LogoUrl, branch };
                    string[] columnOfficialDocsExpiredHeader = { "إسم الوثيقة", "رقم الوثيقة", "جهة الاصدار", "تاريخ الانتهاء", "الفرع" };
                    List<rptGetOfficialDocsExpiredVM> DataOfficialDocsExpired = _EmpsService.GetOfficialDocsExpired(Con, DepartmentId == 0 ? null : DepartmentId).Result.ToList();

                    DataTable listDataOfficialDocsExpired = ToDataTable(DataOfficialDocsExpired);

                    pdfByte = pdfClass.DataOfficialDocsExpiredReport(infoOfficialDocsExpiredReport, columnOfficialDocsExpiredHeader, listDataOfficialDocsExpired);

                    break;
                case 5:

                    string[] infoEmpContractsAboutToExpireReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  عقود الموظفين   ", objOrganization.LogoUrl, branch };
                    string[] columnEmpContractsAboutToExpireHeader = { "رقم العقد", "إسم الموظف", "الجنسية", "القسم", "الفرع", "تاريخ الانتهاء" };
                    List<rptGetEmpContractsAboutToExpireVM> DataEmpContractsAboutToExpire = _Serv.GetEmpContractsAboutToExpire(Con, DepartmentId == 0 ? null : DepartmentId).ToList();

                    DataTable listDataEmpContractsAboutToExpire = ToDataTable(DataEmpContractsAboutToExpire);

                    pdfByte = pdfClass.DataEmpContractsAboutToExpire(infoEmpContractsAboutToExpireReport, columnEmpContractsAboutToExpireHeader, listDataEmpContractsAboutToExpire);

                    break;

                case 8:

                    string[] infoDeservedServicesReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "  الفواتير و الخدمات   ", objOrganization.LogoUrl, branch };
                    string[] columnDeservedServicesHeader = { "رقم الخدمة", "رقم الحساب", "جهة الاصدار", "تاريخ الاستحقاق", "الفرع" };
                    List<rptGetDeservedServicesVM> DataDeservedServices = _Servs.GetDeservedServices(Con).ToList();

                    DataTable listDeservedServices = ToDataTable(DataDeservedServices);

                    pdfByte = pdfClass.DataDeservedServices(infoDeservedServicesReport, columnDeservedServicesHeader, listDeservedServices);

                    break;
                case 9:

                    string[] infoEmpContractsExpireReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "    عقود الموظفين المنتهية   ", objOrganization.LogoUrl, branch };
                    string[] columnEmpContractsExpireHeader = { "رقم العقد", "إسم الموظف", "الجنسية", "القسم", "الفرع", "تاريخ الانتهاء" };
                    List<rptGetEmpContractsAboutToExpireVM> DataEmpContractsExpire = _Serv.GetEmpContractsExpired(Con ?? "", DepartmentId == 0 ? null : DepartmentId).ToList();

                    DataTable listDataEmpContractsExpire = ToDataTable(DataEmpContractsExpire);

                    pdfByte = pdfClass.DataEmpContractsAboutToExpire(infoEmpContractsExpireReport, columnEmpContractsExpireHeader, listDataEmpContractsExpire);

                    break;
                case 10:

                    string[] infoEmpWithoutContractsReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, "   الموظفين الذي ليس لديهم عقود   ", objOrganization.LogoUrl, branch };
                    string[] columnEmpWithoutContractsHeader = {"إسم الموظف", "الجنسية","المسمي الوظيفي", "القسم", "الفرع", "رقم الجوال" ,"البريد الالكتروني","رقم الهوية"};
                    List<EmployeesVM> DataEmpWithoutContracts =  _EmpsService.GetEmployeeWithoutContract(DepartmentId == 0 ? null : DepartmentId, _globalshared.Lang_G).Result.ToList();

                    DataTable listDataEmpWithoutContract = ToDataTable(DataEmpWithoutContracts);

                    pdfByte = pdfClass.DataEmpWithoutContracts(infoEmpWithoutContractsReport, columnEmpWithoutContractsHeader, listDataEmpWithoutContract);

                    break;
                default:
                    break;
            }
            //DB DATA 


            string existTemp = Path.Combine(@"~\TempFiles\");

            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            //File  
        //File  
            string FileName = "PDF_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, pdfByte);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });

        }
        public class YearFisClass
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }


    }

}
