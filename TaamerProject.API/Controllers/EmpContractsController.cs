using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaamerProject.API.Helper;
using TaamerProject.API.pdfHandler;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.ViewModels;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class EmpContractsController : ControllerBase
    {
        private readonly IEmployeesService _Serv;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        private readonly IEmpContractService _EmpContractService;
        private readonly IBranchesService _branchesService;

        private readonly IReasonLeaveService _ReasonLeave;
        private IOrganizationsService _organizationsservice;
        private byte[] ReportPDF;
        private readonly IAttachmentService _attachmentService;
        private readonly IAllowanceService _allowanceService;
        private readonly IAllowanceTypeService _allowanceType;

        public EmpContractsController(IEmployeesService serv, IConfiguration _configuration, IEmpContractService empContractService,
            IBranchesService branchesService, IReasonLeaveService reasonLeave, IOrganizationsService organizationsService, IAttachmentService attachmentService,
            IAllowanceService allowanceService, IAllowanceTypeService allowanceType)
        {
            _Serv = serv;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            _EmpContractService = empContractService;
            _branchesService = branchesService;
            _ReasonLeave = reasonLeave;
            this._organizationsservice = organizationsService;
            _attachmentService = attachmentService;
            _allowanceService = allowanceService;
            _allowanceType = allowanceType;
        }
        [HttpGet("GetEmpContractsAboutToExpire")]
        public IActionResult GetEmpContractsAboutToExpire(int? DepartmentId, bool? Issort = false)
        {
            if (Issort == true)
            {


                var result = _Serv.GetEmpContractsAboutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId);
                return Ok(result.OrderBy(x=>x.ContractEndDate));
            }
            else
            {
                var result = _Serv.GetEmpContractsAboutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId);
                return Ok(result);
            }
        }

        [HttpGet("GetEmpContractsAboutToExpire_paging")]
        public IActionResult GetEmpContractsAboutToExpire_paging(int? DepartmentId, string? SearchText, int page = 1, int pageSize = 10, bool? Issort = false)
        {
            if (Issort == true)
            {

                if (SearchText == null || SearchText == "")
                {
                    var contr = _Serv.GetEmpContractsAboutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId);
                    var data = GeneratePagination<rptGetEmpContractsAboutToExpireVM>.ToPagedList(contr.ToList(), page, pageSize);
                    var result = new PagedLists<rptGetEmpContractsAboutToExpireVM>(data.MetaData, data);
                    return Ok(result);
                }
                else
                {
                    var contr = _Serv.GetEmpContractsAboutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Where(x => x.ContractNo.Contains(SearchText) || x.NameAr.Contains(SearchText) || x.JobName.Contains(SearchText) || x.Nationality.Contains(SearchText) || x.Department.Contains(SearchText) || x.Branch.Contains(SearchText) || x.Salary.Contains(SearchText)).OrderBy(x => x.ContractEndDate);
                    var data = GeneratePagination<rptGetEmpContractsAboutToExpireVM>.ToPagedList(contr.ToList(), page, pageSize);
                    var result = new PagedLists<rptGetEmpContractsAboutToExpireVM>(data.MetaData, data);
                    return Ok(result);
                }
            }
            else
            {
                if (SearchText == null || SearchText == "")
                {
                    var contr = _Serv.GetEmpContractsAboutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId);
                    var data = GeneratePagination<rptGetEmpContractsAboutToExpireVM>.ToPagedList(contr.ToList(), page, pageSize);
                    var result = new PagedLists<rptGetEmpContractsAboutToExpireVM>(data.MetaData, data);
                    return Ok(result);
                }
                else
                {
                    var contr = _Serv.GetEmpContractsAboutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Where(x => x.ContractNo.Contains(SearchText) || x.NameAr.Contains(SearchText) || x.JobName.Contains(SearchText) || x.Nationality.Contains(SearchText) || x.Department.Contains(SearchText) || x.Branch.Contains(SearchText) || x.Salary.Contains(SearchText));
                    var data = GeneratePagination<rptGetEmpContractsAboutToExpireVM>.ToPagedList(contr.ToList(), page, pageSize);
                    var result = new PagedLists<rptGetEmpContractsAboutToExpireVM>(data.MetaData, data);
                    return Ok(result);

                }
            }
        }
        [HttpGet("GetEmpContractsExpired")]
        public IActionResult GetEmpContractsExpired(int? DepartmentId)
        {

            var result = _Serv.GetEmpContractsExpired(Con ?? "", DepartmentId == 0 ? null : DepartmentId);
            return Ok(result);
        }


        [HttpGet("GetEmpContractsExpired_paging")]
        public IActionResult GetEmpContractsExpired_paging(int? DepartmentId, string? SearchText, int page = 1, int pageSize = 10)
        {
            if (SearchText == null || SearchText == "")
            {
                var contr = _Serv.GetEmpContractsExpired(Con ?? "", DepartmentId == 0 ? null : DepartmentId);
                var data = GeneratePagination<rptGetEmpContractsAboutToExpireVM>.ToPagedList(contr.ToList(), page, pageSize);
                var result = new PagedLists<rptGetEmpContractsAboutToExpireVM>(data.MetaData, data);
                return Ok(result);
            }
            else
            {
                var contr = _Serv.GetEmpContractsExpired(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Where(x => x.ContractNo.Contains(SearchText) || x.NameAr.Contains(SearchText) || x.JobName.Contains(SearchText) || x.Nationality.Contains(SearchText) || x.Department.Contains(SearchText) || x.Branch.Contains(SearchText) || x.Salary.Contains(SearchText));
                var data = GeneratePagination<rptGetEmpContractsAboutToExpireVM>.ToPagedList(contr.ToList(), page, pageSize);
                var result = new PagedLists<rptGetEmpContractsAboutToExpireVM>(data.MetaData, data);
                return Ok(result);

            }
        }

        [HttpGet("GetAllEmpContract")]
        public ActionResult GetAllEmpContract()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_EmpContractService.GetAllEmpContract(_globalshared.Lang_G, _globalshared.BranchId_G));
        }



        [HttpGet("GetLastEmpContractSearch")]
        public ActionResult GetLastEmpContractSearch(EmpContractVM Search)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            IEnumerable<EmpContractVM> someContracts = _EmpContractService.GetAllEmpContractSearch(Search, _globalshared.Lang_G, 0).Result;
            if ((bool)!Search.IsSearch)
            {
                foreach (var userBranch in userBranchs)
                {
                    var empContract = _EmpContractService.GetAllEmpContractSearch(Search, _globalshared.Lang_G, userBranch.BranchId).Result;
                    var contract = someContracts.Union(empContract);
                    someContracts = contract;
                }
            }
            var lastinvoice = someContracts.Max(p => p.ContractId);
            var lastone = _EmpContractService.GetLastEmpContractSearch(lastinvoice, _globalshared.Lang_G);

            return Ok(lastone);
        }
        [HttpGet("GetEmployeeContractByID")]
        public ActionResult GetEmployeeContractByID(int contractid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var contrat = _EmpContractService.GetLastEmpContractSearch(contractid, _globalshared.Lang_G);
            return Ok(contrat);
        }
        [HttpPost("GetAllEmpContractSearch")]
        public ActionResult GetAllEmpContractSearch(EmpContractVM Search)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            IEnumerable<EmpContractVM> someContracts = _EmpContractService.GetAllEmpContractSearch(Search, _globalshared.Lang_G, 0).Result;
            if ((bool)!Search.IsSearch)
            {
                foreach (var userBranch in userBranchs)
                {
                    var empContract = _EmpContractService.GetAllEmpContractSearch(Search, _globalshared.Lang_G, userBranch.BranchId).Result;
                    var contract = someContracts.Union(empContract);
                    someContracts = contract;
                }
            }
            return Ok(someContracts);
        }

        [HttpPost("SaveEmpContract")]
        public ActionResult SaveEmpContract([FromBody]EmpContract data)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _EmpContractService.SaveEmpContract(data, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpPost("BeginNewEmployeeWork")]
        public ActionResult BeginNewEmployeeWork(EmpContract data)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _EmpContractService.BeginNewEmployeeWork(data, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("EndWorkforAnEmployee")]
        public ActionResult EndWorkforAnEmployee([FromBody]EmpContract data, string? Reason, string? Duration)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _EmpContractService.EndWorkforAnEmployee(data, Reason, Duration, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("EndWorkforAnEmployeeQuaContract")]
        public ActionResult EndWorkforAnEmployeeQuaContract(int EmpId, string? Reason, string? Duration)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _EmpContractService.EndWorkforAnEmployeeQuaContract(EmpId, Reason, Duration, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteEmployee")]
        public ActionResult DeleteEmployee(int ContractId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _EmpContractService.DeleteEmpContract(ContractId, _globalshared.UserId_G, _globalshared.BranchId_G);
      
            return Ok(result);
        }
        [HttpGet("GetAllDetailsByContractId")]
        public ActionResult GetAllDetailsByContractId(int? ContractId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_EmpContractService.GetAllDetailsByContractId(ContractId));
        }
        [HttpGet("GenerateEmpContractNumber")]
        public ActionResult GenerateEmpContractNumber()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_EmpContractService.GenerateNextEmpContractNumber(_globalshared.BranchId_G));
        }

        [HttpGet("GetAllreasons")]
        public ActionResult GetAllreasons(string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_ReasonLeave.GetAllreasons(SearchText??""));
        }

        [HttpGet("GetAllreasons2")]
        public ActionResult GetAllreasons2()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_ReasonLeave.GetAllreasons(""));
        }

        [HttpGet("Getreasonbyid")]

        public ActionResult Getreasonbyid(int ReasonId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_ReasonLeave.Getreasonbyid(ReasonId));
        }
        [HttpGet("FillReasonSelect")]

        public ActionResult FillReasonSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_ReasonLeave.FillReasonSelect());
        }

        [HttpPost("SaveReason")]
        public ActionResult SaveReason(ReasonLeave reason)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _ReasonLeave.SaveReason(reason, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("DeleteReason")]
        public ActionResult DeleteReason(int ReasonId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _ReasonLeave.DeleteReason(ReasonId, _globalshared.UserId_G, _globalshared.BranchId_G);
          
            return Ok(result);
        }

        //الموظفين عقود
        [HttpPost("PrintEmpContractReport")]
        public ActionResult PrintEmpContractReport(int? ContractId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            System.Data.DataTable EmpContractDetail = _EmpContractService.GetAllContractDetailsByContractId(ContractId, Con).Result;
            ReportPDF = humanResourcesReports.PrintEmpContractReport(EmpContractDetail, infoDoneTasksReport, _allowanceService,_allowanceType);
            string existTemp = System.IO.Path.Combine("Uploads/Attachment/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            //File  
            string FileName = "EmpRpt_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("Uploads/Attachment/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/Uploads/Attachment/" + FileName;



           
            //return file 

            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }
    

        //public ActionResult PrintEmpContractReport(int? ContractId)
        //{
        //    int orgId = _branchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    System.Data.DataTable EmpContractDetail = _EmpContractService.GetAllContractDetailsByContractId(ContractId, Con);
        //    //ReportPDF = humanResourcesReports.PrintEmpContractReport(EmpContractDetail, infoDoneTasksReport);

        //    ViewData["EmpContractDetail"] = EmpContractDetail;


        //    var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId);
        //    if (objOrganization2 != null)
        //        ViewData["Org_VD"] = objOrganization2;
        //    else
        //        ViewData["Org_VD"] = null;

        //    ViewData["Date"] = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

        //    return PartialView("_StaffContract");
        //}


        [HttpPost("PrintEmpEndWork")]
        public ActionResult PrintEmpEndWork([FromBody]EndWorkPrintVM endWork)
        {
            EmpContract data = new EmpContract();
            data.EmpId = endWork.EmpId.Value;
            data.ContractId = endWork.ContractId.Value;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            System.Data.DataTable EmpContractDetail = _EmpContractService.GetAllContractDetailsByContractId(data.ContractId, Con).Result;
            var empdatrpt = _EmpContractService.GetEmpdatatoendwork(data, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G);
            if(endWork.reson !=null && endWork.reson != "")
            {
                empdatrpt.reson = _ReasonLeave.Getreasonbyid(Convert.ToInt32(endWork.reson)).Result.ReasonTxt;
            }
            //empdatrpt.reson = endWork.reson;
            empdatrpt.reasontxt = endWork.resontxt;
            empdatrpt.Enddate = endWork.date;
            ReportPDF = humanResourcesReports.PrintEmpEndworkrpt(EmpContractDetail, empdatrpt, infoDoneTasksReport);
         

            string existTemp = System.IO.Path.Combine("Uploads/Attachment/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            //File  
            string FileName = "EmpRpt_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("Uploads/Attachment/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/Uploads/Attachment/" + FileName;

         

            //create and set PdfReader  
            Attachment attachment = new Attachment();
            attachment.AttachmentUrl = "/Uploads/Attachment/" + FileName;
            attachment.EmployeeId = data.EmpId;
            attachment.AttachmentName = FileName;

            _attachmentService.SaveAttachment(attachment, _globalshared.UserId_G, _globalshared.BranchId_G);
            //return file 


            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }

        [HttpPost("GetEmpdatatoendwork")]
        public ActionResult GetEmpdatatoendwork(EmpContract data)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _EmpContractService.GetEmpdatatoendwork(data, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("DeleteEmployeeContractQua")]
        public ActionResult DeleteEmployeeContractQua(int EmpId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var emp = _EmpContractService.GetEMployeeContractByEmp(EmpId).Result;
            if (emp != null) { 
            var result = _EmpContractService.DeleteEmpContract(emp.ContractId, _globalshared.UserId_G, _globalshared.BranchId_G);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                result = _Serv.DeleteQuacontractDetails(EmpId);
            }
            return Ok(result);
        }
            return BadRequest();

        }




        [HttpGet("GetContractByEmpId")]
        public ActionResult GetContractByEmpId(int EmpId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var emp = _EmpContractService.GetEMployeeContractByEmp(EmpId).Result;
            return Ok(emp);

        }


    }
   
}
