using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net;
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

    public class SupervisionsController : ControllerBase
    {
        private ISupervisionsService _supervisionsservice;
        private IPro_Super_PhasesService _Pro_Super_PhasesService;
        private IPro_SupervisionDetailsService _Pro_SupervisionDetailsService;
        private IBranchesService _BranchesService;
        private IOrganizationsService _organizationsservice;
        private IUsersService _usersService;
        private IContractService _contractService;
        string? Con; private IConfiguration Configuration;
        public GlobalShared _globalshared;
        String tempPath = "~/SuperDetailesFile/";
        String serverMapPath = "~/Files/SuperDetailesFile/";
        private string UrlBase = "Files/SuperDetailesFile/";
        String DeleteURL = "";
        String DeleteType = "GET";


        public SupervisionsController(ISupervisionsService supervisionsService, IPro_Super_PhasesService pro_Super_PhasesService
            , IPro_SupervisionDetailsService pro_SupervisionDetailsService, IBranchesService branchesService
            , IOrganizationsService organizationsService, IUsersService usersService, IContractService contractService
            , IConfiguration _configuration)
        {
            _supervisionsservice = supervisionsService;
            _Pro_Super_PhasesService = pro_Super_PhasesService;
            _Pro_SupervisionDetailsService = pro_SupervisionDetailsService;
            _BranchesService = branchesService;
            _organizationsservice = organizationsService;
            _usersService = usersService;
            _contractService = contractService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
        }
        [HttpGet("GetAllSupervisions")]

        public IActionResult GetAllSupervisions(int? ProjectId)
        {
            return Ok(_supervisionsservice.GetAllSupervisions(ProjectId).Result);
        }
        [HttpGet("GetAllBySupervisionId")]

        public IActionResult GetAllBySupervisionId(int SupervisionId)
        {
            return Ok(_supervisionsservice.GetAllBySupervisionId(SupervisionId).Result);
        }
        [HttpGet("GetAllSupervisionDetailsBySuperId")]

        public IActionResult GetAllSupervisionDetailsBySuperId(int SupervisionId)
        {
            return Ok(_Pro_SupervisionDetailsService.GetAllSupervisionDetailsBySuperId(SupervisionId).Result);
        }
        [HttpGet("GenerateNextSupNumber")]

        public IActionResult GenerateNextSupNumber()
        {
            return Ok(_supervisionsservice.GenerateNextSupNumber().Result);
        }
        [HttpGet("GetAllBySupervisionSearch")]
        public IActionResult GetAllBySupervisionSearch(int? ProjectId, int? UserId, int? EmpId, int? PhaseId, string? DateFrom, string? Dateto)
        {
            return Ok(_supervisionsservice.GetAllSupervision_Search(ProjectId, UserId, EmpId,PhaseId, DateFrom??"", Dateto??"").Result);
        }


        [HttpGet("GetAllBySupervisionSearch_paging")]
        public IActionResult GetAllBySupervisionSearch_paging(int? ProjectId, int? UserId, int? EmpId, int? PhaseId, string? DateFrom, string? Dateto, string? Searchtext, int page = 1, int pageSize = 10)
        {
            var super=_supervisionsservice.GetAllSupervision_Search(ProjectId, UserId, EmpId, PhaseId, DateFrom ?? "", Dateto ?? "", Searchtext).Result;
            var data = GeneratePagination<SupervisionsVM>.ToPagedList(super.ToList(), page, pageSize);
            var result = new PagedLists<SupervisionsVM>(data.MetaData, data);
            return Ok(result);
        }


        [HttpGet("GetAllBySupervisionSearch_Export")]
        public IActionResult GetAllBySupervisionSearch_Export(int? ProjectId, int? UserId, int? EmpId, int? PhaseId, string? DateFrom, string? Dateto, string? Searchtext, int page = 1, int pageSize = 10)
        {
            var super = _supervisionsservice.GetAllSupervision_Search(ProjectId, UserId, EmpId, PhaseId, DateFrom ?? "", Dateto ?? "", Searchtext).Result;
          
            return Ok(super);
        }


        [HttpPost("SupervisionAvailability")]

        public IActionResult SupervisionAvailability(Supervisions supervisions)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
            var file = Path.Combine("distnew/images/logo.png");
            var result = _supervisionsservice.SupervisionAvailability(supervisions, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);
            return Ok(result);
        }
        [HttpPost("DeleteSupervision")]

        public IActionResult DeleteSupervision(int SupervisionId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _supervisionsservice.DeleteSupervision(SupervisionId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("PrintSupervisionMail")]

        public IActionResult PrintSupervisionMail(int? SupervisionId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            SupervisionReportVM _supervisionReportVM = new SupervisionReportVM();

            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            List<Pro_SupervisionDetailsVM> SuperDetailsVM = _Pro_SupervisionDetailsService.GetAllSupervisionDetailsBySuperId(SupervisionId).Result.ToList();
            // ReportPDF = ProjectsReports.SupervisionRep(SuperDetailsVM, "", "", infoDoneTasksReport);
            _supervisionReportVM.Org_VD = objOrganization;

            string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            _supervisionReportVM.DateTimeNow = Date;
            _supervisionReportVM.Date = Date;
            _supervisionReportVM.Supervision = SuperDetailsVM;
            return Ok(_supervisionReportVM);
        }


        public class supmail
        {
            public int SupervisionId { get; set; }
            public int? EmailStatusCustomer { get; set; }
            public int? EmailStatusContractor { get; set; }
            public int? EmailStatusOffice { get; set; }
            public string? AttachmentFile { get; set; }
            public string? environmentURL { get; set; }

            
        }
        [HttpPost("SendMSupervision")]

        public IActionResult SendMSupervision(supmail supermail)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var filePath = Path.Combine(supermail.AttachmentFile??"");
            var replacementPath = filePath.Replace('/', '\\');

            var result = _supervisionsservice.SendMSupervision(supermail.SupervisionId, supermail.EmailStatusCustomer??0, supermail.EmailStatusContractor??0, supermail.EmailStatusOffice??0, _globalshared.UserId_G, _globalshared.BranchId_G, replacementPath);
            return Ok(result);
        }
        [HttpPost("SendWSupervision")]

        public IActionResult SendWSupervision(IFormFile? UploadedFile, [FromForm] int SupervisionId, [FromForm] string? EmailStatusCustomer, [FromForm] string? EmailStatusContractor, [FromForm] string? EmailStatusOffice, [FromForm] string? AttachmentFile, [FromForm] string? environmentURL)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //var filePath = Path.Combine(supermail.AttachmentFile ?? "");
            //var replacementPath = filePath.Replace('/', '\\');
            //var link = replacementPath;


            var FileUrl = "";

            if (UploadedFile != null)
            {
                string path = System.IO.Path.Combine("TempFiles/");
                string pathW = System.IO.Path.Combine("/TempFiles/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                string pathes = "";
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedFile.FileName);
                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {
                    UploadedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != "")
                {
                    FileUrl = "/TempFiles/" + fileName;
                }
            }
            var FileAtt = FileUrl;


            var result = _supervisionsservice.SendWSupervision(SupervisionId, Convert.ToInt32(EmailStatusCustomer??"0"), Convert.ToInt32(EmailStatusContractor ?? "0"), Convert.ToInt32(EmailStatusOffice ?? "0"), _globalshared.UserId_G, _globalshared.BranchId_G, FileAtt, environmentURL ?? "");
            return Ok(result);
        }
        [HttpPost("ReadSupervision")]

        public IActionResult ReadSupervision(int SupervisionId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _supervisionsservice.ReadSupervision(SupervisionId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("ReciveSuper")]
        public IActionResult ReciveSuper(int SupervisionId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
            var file = Path.Combine("distnew/images/logo.png");
            var result = _supervisionsservice.ReciveSuper(SupervisionId, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);
            return Ok(result);
        }
        [HttpPost("OutlineChangeSave")]

        public IActionResult OutlineChangeSave(int SupervisionId, string? OutlineChangetxt1, string? OutlineChangetxt2, string? OutlineChangetxt3)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _supervisionsservice.OutlineChangeSave(SupervisionId, OutlineChangetxt1??"", OutlineChangetxt2??"", OutlineChangetxt3 ?? "", _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("PointsNotWrittenSave")]
        public IActionResult PointsNotWrittenSave(int SupervisionId, string? PointsNotWrittentxt1, string? PointsNotWrittentxt2, string? PointsNotWrittentxt3)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _supervisionsservice.PointsNotWrittenSave(SupervisionId, PointsNotWrittentxt1??"", PointsNotWrittentxt2??"", PointsNotWrittentxt3??"", _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("NotReciveSuper")]

        public IActionResult NotReciveSuper(int SupervisionId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _supervisionsservice.NotReciveSuper(SupervisionId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("ReciveSuperDet")]
        public IActionResult ReciveSuperDet(int SuperDetId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_SupervisionDetailsService.ReciveDetails(SuperDetId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("NotReciveSuperDet")]

        public IActionResult NotReciveSuperDet(int SuperDetId, string? Note)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _Pro_SupervisionDetailsService.NotReciveDetails(SuperDetId, Note??"", _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("TheNumberSuperDet")]

        public IActionResult TheNumberSuperDet(int SuperDetId, string Note)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_SupervisionDetailsService.TheNumberSuperDet(SuperDetId, Note, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("TheLocationSuperDet")]
        public IActionResult TheLocationSuperDet(int SuperDetId, string? Note)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_SupervisionDetailsService.TheLocationSuperDet(SuperDetId, Note??"", _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("NotFoundSuperDet")]
        public IActionResult NotFoundSuperDet(int SuperDetId, string? Note)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_SupervisionDetailsService.NotFoundDetails(SuperDetId, Note ?? "", _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("AddNumberSuperDet")]
        public IActionResult AddNumberSuperDet(int SuperDetId, string? Note, int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_SupervisionDetailsService.AddNumberSuperDet(SuperDetId, Note ?? "", Type, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("UploadImageSuperDet")]

        public IActionResult UploadImageSuperDet([FromForm] List<IFormFile>? uploadesgiles,[FromForm]int? SuperDetId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Pro_SupervisionDetails SupDetails =new Pro_SupervisionDetails();
            SupDetails.SuperDetId = SuperDetId??0;
            var FileResult2 = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ تأكد من اختيار ملف", ReturnedStr = "" };

            if (SupDetails.SuperDetId == 0)
            {
                return Ok(FileResult2);
            }
            try
            {
                var FileResult = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يوجد ملفات", ReturnedStr = "" };
               
                string pathF = System.IO.Path.Combine("Files/", "SuperDetailesFile/");
                string serverMapPath2 = "";
                if (SupDetails != null)
                {
                   // serverMapPath2 = Server.MapPath("~/Files/SuperDetailesFile/" + SupDetails.SuperDetId);
                    pathF = System.IO.Path.Combine("Files/", "SuperDetailesFile/" + SupDetails.SuperDetId);

                    if (!Directory.Exists(pathF))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(pathF);
                    }
                }
                UrlBase =  System.IO.Path.Combine(UrlBase, SupDetails.SuperDetId.ToString());
               // UrlBase + SupDetails.SuperDetId; /// added recently
                FilesHelper filesHelpers = new FilesHelper(DeleteURL, DeleteType, serverMapPath2, UrlBase, tempPath, serverMapPath2);
                for (int i = 0; i < uploadesgiles.Count; i++)
                {
                    Pro_SupervisionDetails SupDetailsN = new Pro_SupervisionDetails();
                    //string filename = uploadesgiles[i].FileName;
                    string filename = System.IO.Path.GetFileName(Guid.NewGuid() + uploadesgiles[i].FileName);

                    SupDetailsN.SuperDetId = SupDetails.SuperDetId;
                    SupDetailsN.ImageUrl = "/" + UrlBase + "/" + filename;
                    using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(UrlBase, filename), System.IO.FileMode.Create))
                    {


                        uploadesgiles[i].CopyTo(stream);
                      
                    }

                    FileResult = _Pro_SupervisionDetailsService.UploadImageSuperDet(SupDetailsN.SuperDetId, SupDetailsN.ImageUrl, _globalshared.UserId_G, _globalshared.BranchId_G);
                }
                return Ok(FileResult);
            }
            catch (Exception ex)
            {
                return Ok(FileResult2);
            }

        }
        [HttpPost("UploadHeadImageFir")]

        public IActionResult UploadHeadImageFir( List<IFormFile>? uploadesgiles, [FromForm] int SupervisionId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var FileResult2 = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ تأكد من اختيار ملف", ReturnedStr = "" };

            if (SupervisionId == 0)
            {
                return Ok(FileResult2);
            }
            try
            {
                var FileResult = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يوجد ملفات", ReturnedStr = "" };

                string pathF = System.IO.Path.Combine("Files/", "SuperDetailesFile/");
                string serverMapPath2 = "";
                pathF = System.IO.Path.Combine("Files/", "SuperDetailesFile/" + SupervisionId);
                if (!Directory.Exists(pathF))
                {
                    DirectoryInfo di = Directory.CreateDirectory(pathF);
                }
                UrlBase = UrlBase + SupervisionId; /// added recently
                FilesHelper filesHelpers = new FilesHelper(DeleteURL, DeleteType, serverMapPath2, UrlBase, tempPath, serverMapPath2);
                for (int i = 0; i < uploadesgiles.Count; i++)
                {
                    Supervisions supervisionsN = new Supervisions();
                    string filename = uploadesgiles[i].FileName;
                    supervisionsN.SupervisionId = SupervisionId;
                    supervisionsN.ImageUrl = "/" + UrlBase + "/" + uploadesgiles[i].FileName;
                    using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(UrlBase, filename), System.IO.FileMode.Create))
                    {
                        uploadesgiles[i].CopyTo(stream);
                    }
                    FileResult = _supervisionsservice.UploadHeadImageFir(supervisionsN.SupervisionId, supervisionsN.ImageUrl, _globalshared.UserId_G, _globalshared.BranchId_G);
                }
                return Ok(FileResult);
            }
            catch (Exception ex)
            {
                return Ok(FileResult2);
            }

        }

        [HttpPost("UploadHeadImageSec")]

        public IActionResult UploadHeadImageSec( List<IFormFile>? uploadesgiles, [FromForm] int SupervisionId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var FileResult2 = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ تأكد من اختيار ملف", ReturnedStr = "" };

            if (SupervisionId == 0)
            {
                return Ok(FileResult2);
            }
            try
            {
                var FileResult = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يوجد ملفات", ReturnedStr = "" };

                string serverMapPath2 = "";
                string pathF = System.IO.Path.Combine("Files/", "SuperDetailesFile/");

                pathF = System.IO.Path.Combine("Files/", "SuperDetailesFile/" + SupervisionId);

                if (!Directory.Exists(pathF))
                {
                    DirectoryInfo di = Directory.CreateDirectory(pathF);
                }

                UrlBase = UrlBase + SupervisionId; /// added recently
                FilesHelper filesHelpers = new FilesHelper(DeleteURL, DeleteType, serverMapPath2, UrlBase, tempPath, serverMapPath2);
                for (int i = 0; i < uploadesgiles.Count; i++)
                {
                    Supervisions supervisionsN = new Supervisions();
                    string filename = uploadesgiles[i].FileName;
                    supervisionsN.SupervisionId = SupervisionId;
                    supervisionsN.ImageUrl2 = "/" + UrlBase + "/" + uploadesgiles[i].FileName;
                    using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(UrlBase, filename), System.IO.FileMode.Create))
                    {
                        uploadesgiles[i].CopyTo(stream);
                    }
                    FileResult = _supervisionsservice.UploadHeadImageSec(supervisionsN.SupervisionId, supervisionsN.ImageUrl2, _globalshared.UserId_G, _globalshared.BranchId_G);
                }
                return Ok(FileResult);
            }
            catch (Exception ex)
            {
                return Ok(FileResult2);
            }
        }

        [HttpPost("ConfirmSupervision")]

        public IActionResult ConfirmSupervision(int SupervisionId, int TypeId, int TypeIdAdmin)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _supervisionsservice.ConfirmSupervision(SupervisionId, _globalshared.UserId_G, _globalshared.BranchId_G, TypeId, TypeIdAdmin);
            return Ok(result);
        }
        [HttpGet("GetAllSupervisionsByUserId")]

        public IActionResult GetAllSupervisionsByUserId()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            
            return Ok(_supervisionsservice.GetAllSupervisionsByUserId(_globalshared.UserId_G));
        }
        [HttpGet("GetAllSupervisionsByUserIdHome")]

        public IActionResult GetAllSupervisionsByUserIdHome()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_supervisionsservice.GetAllSupervisionsByUserIdHome(_globalshared.UserId_G).Result);
        }
        [HttpGet("GetAllSuperPhases")]

        public IActionResult GetAllSuperPhases(string? SearchText)
        {
            return Ok(_Pro_Super_PhasesService.GetAllSuper_Phases(SearchText??"").Result);
        }
        [HttpGet("GetAllSuperPhaseDet")]

        public IActionResult GetAllSuperPhaseDet(int? PhaseId)
        {
            return Ok(_Pro_Super_PhasesService.GetAllSuper_PhaseDet(PhaseId).Result);
        }
        [HttpPost("SaveSuperPhases")]
        public IActionResult SaveSuperPhases(Pro_Super_Phases Super_Phases)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _Pro_Super_PhasesService.SaveSuper_Phases(Super_Phases, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteSuperPhases")]
        public IActionResult DeleteSuperPhases(int PhaseId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _Pro_Super_PhasesService.DeleteSuper_Phases(PhaseId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillSuperPhasesSelect")]
        public IActionResult FillSuperPhasesSelect(string SearchText = "")
        {
            return Ok(_Pro_Super_PhasesService.GetAllSuper_Phases(SearchText).Result.Select(s => new
            {
                Id = s.PhaseId,
                Name = s.NameAr,
                NameEn=s.NameEn,
                SuperCode=s.SuperCode,
            }));
        }
        [HttpPost("SaveSuperPhaseDet")]
        public IActionResult SaveSuperPhaseDet(List<Pro_Super_PhaseDet> PhaseDet)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _Pro_Super_PhasesService.SaveSuperPhaseDet(PhaseDet, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveSuperDet")]

        public IActionResult SaveSuperDet(List<Pro_SupervisionDetails> Det)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _Pro_SupervisionDetailsService.SaveSuperDet(Det, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("UpdateStatus_SuperPhaseDet")]

        public IActionResult UpdateStatus_SuperPhaseDet(int PhaseDetailesId, bool? ISRead)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _Pro_Super_PhasesService.UpdateStatus_SuperPhaseDet(PhaseDetailesId, ISRead, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetSuperCodeFromPhaseId")]
        public IActionResult GetSuperCodeFromPhaseId(int? SuperId)
        {
            var PhaseSup = _Pro_Super_PhasesService.GetSuper_PhasesById(SuperId ?? 0).Result;
            return Ok(PhaseSup);
        }
        //public IActionResult PrintPhasesReports(int PhaseId, int SupervisionId, int ProjectId)
        //{
        //    SupervisionReports supervisionReports = new SupervisionReports();
        //    //var Supervision = _supervisionsservice.GetAllSupervisions(ProjectId).Where(x => x.Number == SupervisionNo && x.PhaseId == PhaseId).FirstOrDefault();
        //    //var Supervision = _supervisionsservice.GetAllBySupervisionId(SupervisionId).FirstOrDefault();
        //    var Supervision2 = _Pro_SupervisionDetailsService.GetAllSupervisionDetailsBySuperId(SupervisionId).ToList();
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);
        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);

        //    var Phase = _Pro_Super_PhasesService.GetAllSuper_Phases("").Where(x => x.PhaseId == PhaseId).FirstOrDefault();

        //    //List<Pro_Super_PhaseDetVM> phaseDetails = _Pro_Super_PhasesService.GetAllSuper_PhaseDet(PhaseId).ToList();

        //    string path = Server.MapPath("~/Reports/Supervision/");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    string fileName = "template.docx";

        //    string fileLocation;
        //    string fileDir;
        //    string fileNamed;
        //    string fileNameOnly = "تقرير " + Phase.NameAr;
        //    try
        //    {

        //        fileLocation = Server.MapPath("~/Uploads/Drafts/") + fileNameOnly + ".docx";
        //        fileDir = Server.MapPath("~/Uploads/Drafts/");

        //        if (System.IO.Directory.Exists(fileDir))
        //        {
        //            if (System.IO.File.Exists(fileLocation))
        //            {
        //                System.IO.File.Delete(fileLocation);
        //            }
        //        }
        //        else
        //        {
        //            System.IO.Directory.CreateDirectory(fileDir);
        //            if (System.IO.File.Exists(fileLocation))
        //            {
        //                System.IO.File.Delete(fileLocation);
        //            }
        //        }


        //        fileNamed = fileNameOnly + ".docx";
        //        string sourcePath = path;
        //        string targetPath = fileDir;

        //        // Use Path class to manipulate file and directory paths.
        //        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
        //        string destFile = System.IO.Path.Combine(targetPath, fileNamed);


        //        // To copy a file to another location and
        //        // overwrite the destination file if it already exists.
        //        if (System.IO.File.Exists(destFile))
        //        {
        //            System.IO.File.Delete(destFile);
        //        }
        //        System.IO.File.Copy(sourceFile, destFile, true);
        //        string Image1 = "";
        //        string Image2 = "";
        //        string StampUrl = "";
        //        if (Supervision2.FirstOrDefault().HeadImageUrl != "")
        //        {

        //            Image1 = Server.MapPath("~" + Supervision2.FirstOrDefault().HeadImageUrl);
        //            if (!System.IO.File.Exists(Image1))
        //                Image1 = "";
        //        }
        //        if (Supervision2.FirstOrDefault().HeadImageUrl2 != "")
        //        {
        //            Image2 = Server.MapPath("~" + Supervision2.FirstOrDefault().HeadImageUrl2);
        //            if (!System.IO.File.Exists(Image2))
        //                Image2 = "";
        //        }
        //        if (Supervision2.FirstOrDefault().ReceviedUserStampUrl != "")
        //        {
        //            StampUrl = _usersService.DecryptValue1(Supervision2.FirstOrDefault().ReceviedUserStampUrl);
        //            string ImageIn = Server.MapPath("~" + StampUrl);
        //            string ImageOut = ImageIn.Replace("\\Encrypted", "");
        //            bool flag = RijndaelHelper.DecryptFile(ImageIn, ImageOut);
        //            if (flag)
        //            {
        //                //Not encrypted
        //                StampUrl = ImageOut;
        //            }
        //            if (!System.IO.File.Exists(StampUrl))
        //                StampUrl = "";
        //        }
        //        //string Image1 = Server.MapPath(@"\Uploads\Organizations\pictures\logo.png");
        //        //string Image2 = Server.MapPath(@"\Uploads\Organizations\pictures\logo.png");
        //        //Load Document
        //        string FileName = "تقرير " + Phase.NameAr + ".pdf";
        //        string FilePath = HttpContext.Server.MapPath(@"~\Uploads\Drafts\") + FileName;

        //        //Make a Report  
        //        //var result = supervisionReports.PrintReport(sourceFile, destFile, FilePath, Phase, Supervision, phaseDetails, Image1,Image2, objOrganization);
        //        var result = supervisionReports.PrintReport(sourceFile, destFile, FilePath, Phase, Supervision2, Image1, Image2, StampUrl, objOrganization);

        //        if (result.Result)
        //        {
        //            //return file 
        //            string FilePathReturn = @"/Uploads/Drafts/" + FileName;
        //            if (System.IO.File.Exists(StampUrl))
        //            {
        //                GC.Collect();
        //                GC.WaitForPendingFinalizers();
        //                System.IO.File.Delete(StampUrl);
        //            }
        //            return Content(FilePathReturn);
        //        }
        //        else
        //            //return Content("");
        //            return Content(result.Message);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Content(ex.Message);
        //    }
        //}

        //public IActionResult PrintPhasesReportsNew(int PhaseId, int SupervisionId, int ProjectId)
        //{
        //    SupervisionReports supervisionReports = new SupervisionReports();
        //    //var Supervision = _supervisionsservice.GetAllSupervisions(ProjectId).Where(x => x.Number == SupervisionNo && x.PhaseId == PhaseId).FirstOrDefault();
        //    //var Supervision = _supervisionsservice.GetAllBySupervisionId(SupervisionId).FirstOrDefault();
        //    var Supervision2 = _Pro_SupervisionDetailsService.GetAllSupervisionDetailsBySuperId(SupervisionId).ToList();
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);
        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);

        //    var Phase = _Pro_Super_PhasesService.GetAllSuper_Phases("").Where(x => x.PhaseId == PhaseId).FirstOrDefault();

        //    //List<Pro_Super_PhaseDetVM> phaseDetails = _Pro_Super_PhasesService.GetAllSuper_PhaseDet(PhaseId).ToList();

        //    string path = Server.MapPath("~/Reports/Supervision/");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    string fileName = "template.docx";

        //    string fileLocation;
        //    string fileDir;
        //    string fileNamed;
        //    string fileNameOnly = "تقرير " + Phase.NameAr;
        //    try
        //    {

        //        fileLocation = Server.MapPath("~/Uploads/Drafts/") + fileNameOnly + ".docx";
        //        fileDir = Server.MapPath("~/Uploads/Drafts/");

        //        if (System.IO.Directory.Exists(fileDir))
        //        {
        //            if (System.IO.File.Exists(fileLocation))
        //            {
        //                System.IO.File.Delete(fileLocation);
        //            }
        //        }
        //        else
        //        {
        //            System.IO.Directory.CreateDirectory(fileDir);
        //            if (System.IO.File.Exists(fileLocation))
        //            {
        //                System.IO.File.Delete(fileLocation);
        //            }
        //        }


        //        fileNamed = fileNameOnly + ".docx";
        //        string sourcePath = path;
        //        string targetPath = fileDir;

        //        // Use Path class to manipulate file and directory paths.
        //        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
        //        string destFile = System.IO.Path.Combine(targetPath, fileNamed);


        //        // To copy a file to another location and
        //        // overwrite the destination file if it already exists.
        //        if (System.IO.File.Exists(destFile))
        //        {
        //            System.IO.File.Delete(destFile);
        //        }
        //        System.IO.File.Copy(sourceFile, destFile, true);
        //        string Image1 = "";
        //        string Image2 = "";
        //        string StampUrl = "";
        //        if (Supervision2.FirstOrDefault().HeadImageUrl != "")
        //        {

        //            Image1 = Server.MapPath("~" + Supervision2.FirstOrDefault().HeadImageUrl);
        //            if (!System.IO.File.Exists(Image1))
        //                Image1 = "";
        //        }
        //        if (Supervision2.FirstOrDefault().HeadImageUrl2 != "")
        //        {
        //            Image2 = Server.MapPath("~" + Supervision2.FirstOrDefault().HeadImageUrl2);
        //            if (!System.IO.File.Exists(Image2))
        //                Image2 = "";
        //        }
        //        if (Supervision2.FirstOrDefault().ReceviedUserStampUrl != "")
        //        {
        //            StampUrl = _usersService.DecryptValue1(Supervision2.FirstOrDefault().ReceviedUserStampUrl);
        //            string ImageIn = Server.MapPath("~" + StampUrl);
        //            string ImageOut = ImageIn.Replace("\\Encrypted", "");
        //            bool flag = RijndaelHelper.DecryptFile(ImageIn, ImageOut);
        //            if (flag)
        //            {
        //                //Not encrypted
        //                StampUrl = ImageOut;
        //            }
        //            if (!System.IO.File.Exists(StampUrl))
        //                StampUrl = "";
        //        }
        //        //string Image1 = Server.MapPath(@"\Uploads\Organizations\pictures\logo.png");
        //        //string Image2 = Server.MapPath(@"\Uploads\Organizations\pictures\logo.png");
        //        //Load Document
        //        string FileName = "تقرير " + Phase.NameAr + ".pdf";
        //        string FilePath = HttpContext.Server.MapPath(@"~\Uploads\Drafts\") + FileName;

        //        //Make a Report  
        //        //var result = supervisionReports.PrintReport(sourceFile, destFile, FilePath, Phase, Supervision, phaseDetails, Image1,Image2, objOrganization);
        //        var result = supervisionReports.PrintReport(sourceFile, destFile, FilePath, Phase, Supervision2, Image1, Image2, StampUrl, objOrganization);

        //        if (result.Result)
        //        {
        //            //return file 
        //            string FilePathReturn = @"/Uploads/Drafts/" + FileName;
        //            if (System.IO.File.Exists(StampUrl))
        //            {
        //                GC.Collect();
        //                GC.WaitForPendingFinalizers();
        //                System.IO.File.Delete(StampUrl);
        //            }
        //            return Content(FilePathReturn);
        //        }
        //        else
        //            //return Content("");
        //            return Content(result.Message);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Content(ex.Message);
        //    }
        //}
        [HttpGet("ChangeSupervision")]

        public IActionResult ChangeSupervision(int? SuperId, string SuperCode)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            SupervisionReportVM _supervisionReportVM = new SupervisionReportVM();
            if (SuperId.HasValue)
            {
                int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
                var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
                _supervisionReportVM.Org_VD = objOrganization;

                if (SuperCode == "14")
                {
                    var Sup = _supervisionsservice.GetAllBySupervisionId(SuperId ?? 0).Result.FirstOrDefault();
                    if (Sup != null)
                    {
                        var DateCon = Convert.ToDateTime(Sup.Date, System.Globalization.CultureInfo.CreateSpecificCulture("en"));

                        var day = DateCon.ToString("dddd", new System.Globalization.CultureInfo("ar-AE"));
                        _supervisionReportVM.Date= Sup.Date;
                        var Hijrivalue = _contractService.ConvertDateCalendar(DateCon, "Hijri", "en-US");
                        _supervisionReportVM.HijriDate = Hijrivalue;
                        _supervisionReportVM.day = day;
                        _supervisionReportVM.Sup = Sup;

                        var TextStringTemp = "انه في يوم" + " " + day + " " + "الموافق" + " " + Sup.Date + " " + "الموافق" + " "
                            + Hijrivalue + " هـ " + "قمنا نحن" + " " + objOrganization.NameAr + " "
                            + "بعمل الزيارة الميدانية رقم" + " " + "( " + Sup.Number + " )" + " " + "مشروع" + " "
                            + Sup.ProjectNo + " " + "والذي تعود ملكيته الي" + " " + Sup.CustomerName_W + " "
                            + "الواقع في مدينة" + " " + "( " + Sup.CityName + " )" + " " + "قطعة رقم" + " " + "( " + Sup.ProPieceNumber + " )" + " "
                            + "مخطط تنظيمي رقم" + " " + "( " + Sup.Catego + " )" + " " + "المكون من" + " " + "( " + Sup.Cons_components + " )";

                        var TextStringTempEnd = "أتعهد أنا المالك/ممثل المالك" + " " + Sup.CustomerName_W + " "
                            + "بتنفيذ الملاحظات الواردة بالتقرير خلال مدة" + " " + Sup.TimeStr + " " + "واخطار" + " "
                            + objOrganization.NameAr + " " + "حال اكتمال التنفيذ";

                        _supervisionReportVM.TextStringTemp = TextStringTemp;
                        _supervisionReportVM.TextStringTempEnd = TextStringTempEnd;

                    }
                    else
                    {
                        _supervisionReportVM.Date = null;
                        _supervisionReportVM.HijriDate = null;
                        _supervisionReportVM.day = null;
                        _supervisionReportVM.Sup = null;
                        _supervisionReportVM.TextStringTempEnd = null;
                    }
                }



                string StampUrl = "";
                var Supervision2 = _Pro_SupervisionDetailsService.GetAllSupervisionDetailsBySuperId(SuperId).Result.ToList();
                if (Supervision2.Count() > 0)
                {
                    var Phase = _Pro_Super_PhasesService.GetAllSuper_Phases("").Result.Where(x => x.PhaseId == Supervision2[0].PhaseId).FirstOrDefault();
                    _supervisionReportVM.Phase = Phase;

                    try
                    {
                        if (Supervision2.FirstOrDefault().ReceviedUserStampUrl != "")
                        {
                            StampUrl = _usersService.DecryptValue1(Supervision2.FirstOrDefault().ReceviedUserStampUrl);
                            if (StampUrl != null) StampUrl = StampUrl.Remove(0, 1);
                            string ImageIn = Path.Combine(StampUrl);
                            string ImageOut = ImageIn.Replace("\\Encrypted", "");
                            bool flag = RijndaelHelper.DecryptFile(ImageIn, ImageOut);
                            if (flag)
                            {
                                //Not encrypted
                                string[] splitStr = ImageIn.Split('\\');
                                //string Serverurl = @"http://" + Request.Url.Authority;
                                //StampUrl = Serverurl + @"/Uploads/Users/" + splitStr[splitStr.Length - 1];
                                StampUrl = Path.Combine("Uploads/Users/" + splitStr[splitStr.Length - 1]);
                            }
                            if (!System.IO.File.Exists(ImageOut))
                                StampUrl = "";
                        }
                        _supervisionReportVM.StampUrl = StampUrl;


                    }
                    catch (Exception ex)
                    {
                        _supervisionReportVM.StampUrl = null;
                    }
                }
                else
                {
                    _supervisionReportVM.Phase = null;
                }
                _supervisionReportVM.Supervision = Supervision2;

            }
            else
            {
                _supervisionReportVM.Phase = null;
                _supervisionReportVM.Org_VD = null;
                _supervisionReportVM.Supervision = null;
            }
            return Ok(_supervisionReportVM);
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public void Export2(string GridHtml, string Header, string Footer)
        //{
        //    try
        //    {
        //        string middleFile = Server.MapPath(@"~\Uploads\Drafts\Grid.pdf");
        //        string finalFile = Server.MapPath(@"~\Uploads\Drafts\final.pdf");
        //        string htmlFile = Server.MapPath(@"~\Test\Supervision.cshtml");

        //        PdfWriter writer = new PdfWriter(middleFile);
        //        PdfDocument pdfDocument = new PdfDocument(writer);

        //        pdfDocument.SetDefaultPageSize(new PageSize(PageSize.A4));

        //        //string header = "pdfHtml Header and footer example using page-events";
        //        //Header headerHandler = new Header(header);
        //        Footer footerHandler = new Footer();
        //        FontProvider fontProvider = new DefaultFontProvider(false, false, false);
        //        FontProgram fontProgram = FontProgramFactory.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIAL.TTF");
        //        fontProvider.AddFont(fontProgram);
        //        //pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
        //        pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, footerHandler);

        //        // Base URI is required to resolve the path to source files
        //        ConverterProperties converterProperties = new ConverterProperties();
        //        converterProperties.SetFontProvider(fontProvider);
        //        HtmlConverter.ConvertToDocument(GridHtml, pdfDocument, converterProperties);

        //        // Write the total number of pages to the placeholder
        //        footerHandler.WriteTotal(pdfDocument);
        //        pdfDocument.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        var me = ex.Message;
        //    }

        //}
        //public class TextFooterEventHandler : IEventHandler
        //{
        //    protected Document doc;

        //    public TextFooterEventHandler(Document doc)
        //    {
        //        this.doc = doc;
        //    }


        //    public void HandleEvent(Event currentEvent)
        //    {
        //        PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
        //        Rectangle pageSize = docEvent.GetPage().GetPageSize();

        //        float coordX = ((pageSize.GetLeft() + doc.GetLeftMargin())
        //                         + (pageSize.GetRight() - doc.GetRightMargin())) / 2;
        //        float headerY = pageSize.GetTop() - doc.GetTopMargin() + 10;
        //        float footerY = doc.GetBottomMargin();

        //        //PdfCanvas pdfCanvas = new PdfCanvas(docEvent.GetPage());
        //        //pdfCanvas.AddXObject(placeholder, x + space, y - descent);
        //        //pdfCanvas.Release();

        //        Canvas canvas = new Canvas(docEvent.GetPage(), pageSize);
        //        canvas

        //            // If the exception has been thrown, the font variable is not initialized.
        //            // Therefore null will be set and iText will use the default font - Helvetica

        //            .ShowTextAligned("this is a footer", coordX, footerY, TextAlignment.CENTER)
        //            .Close();
        //    }
        //}

        //// Header event handler
        //public class Header : IEventHandler
        //{
        //    private string header;

        //    public Header(string header)
        //    {
        //        this.header = header;
        //    }

        //    public virtual void HandleEvent(Event @event)
        //    {
        //        PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
        //        PdfDocument pdf = docEvent.GetDocument();

        //        PdfPage page = docEvent.GetPage();
        //        Rectangle pageSize = page.GetPageSize();
        //        string headerStr = @"<header id='header'>
        //    <table style='width:100%'>
        //        <tr>
        //            <td style='width:30%'>
        //                <div class='col-md-4'>
        //                    <div class='img-logo'>
        //                        <img src='https://tameercloud.com/Uploads/Users/logo.png' alt='logo' style='height:50px;width:120px;'>
        //                    </div>
        //                </div>
        //            </td>
        //            <td style='width:40%'>
        //                <div class='col-md-4'></div>
        //            </td>
        //            <td style='width:30%'>
        //                <div class='col-md-4'></div>
        //            </td>
        //        </tr>
        //    </table>
        //    <table style='width:100%'>
        //        <tr>
        //            <td style='width:30%'>
        //                <div class='col-md-4 '>
        //                    <div class='head-green'></div>
        //                </div>
        //            </td>
        //            <td style='width:40%'>
        //                <div class='col-md-4 up '>
        //                    <div class='title '>
        //                        @if (phase != null)
        //                        {
        //                            <p class='text-center largeFont'>@phase.Note </p>
        //                            <p dir='ltr' class='text-center largeFont'>@phase.NameAr </p>
        //                        }
        //                    </div>
        //                </div>
        //            </td>
        //            <td style='width:30%'>
        //                <div class='col-md-4 head-green '></div>
        //            </td>
        //        </tr>
        //    </table>
        //</header>";

        //        Canvas canvas = new Canvas(new PdfCanvas(page), pageSize);
        //        IList<IElement> headerElements = HtmlConverter.ConvertToElements(headerStr);
        //        foreach (IElement headerElement in headerElements)
        //        {
        //            // Making sure we are adding blocks to canvas
        //            if ((IBlockElement)headerElement != null)
        //            {
        //                canvas.Add((IBlockElement)headerElement);
        //            }

        //            canvas.SetFontSize(8);

        //            // Write text at position
        //            //canvas.ShowTextAligned(header,
        //            //    pageSize.GetWidth() / 2,
        //            //    pageSize.GetTop() - 30, TextAlignment.CENTER);
        //            canvas.Close();
        //        }
        //    }
        //}

        //// Footer event handler
        //public class Footer : IEventHandler
        //{
        //    protected PdfFormXObject placeholder;
        //    protected float side = 20;
        //    protected float x = 300;
        //    protected float y = 25;
        //    protected float space = 5.5f;
        //    protected float descent = 3;

        //    public Footer()
        //    {
        //        placeholder = new PdfFormXObject(new Rectangle(0, 0, side, side));
        //    }

        //    public virtual void HandleEvent(Event @event)
        //    {
        //        PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
        //        PdfDocument pdf = docEvent.GetDocument();
        //        PdfPage page = docEvent.GetPage();
        //        int pageNumber = pdf.GetPageNumber(page);
        //        Rectangle pageSize = page.GetPageSize();
        //        x = pageSize.GetLeft() + 1f;
        //        FontProvider fontProvider = new DefaultFontProvider(false, false, false);
        //        FontProgram fontProgram = FontProgramFactory.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIAL.TTF");
        //        fontProvider.AddFont(fontProgram);


        //        // Creates drawing canvas
        //        PdfCanvas pdfCanvas = new PdfCanvas(page);
        //        Canvas canvas = new Canvas(pdfCanvas, pageSize);
        //        canvas.SetFontProvider(fontProvider);

        //        Paragraph p = new Paragraph()
        //            .Add(" Page ")
        //            .Add(pageNumber.ToString())
        //            .Add(" of ");
        //        //p.SetFontSize(8);
        //        canvas.ShowTextAligned(p, x + 5f, y, TextAlignment.LEFT);
        //        canvas.Close();

        //        // Create placeholder object to write number of pages
        //        pdfCanvas.AddXObjectAt(placeholder, x + 60f, y - descent);
        //        pdfCanvas.Release();
        //    }

        //    public void WriteTotal(PdfDocument pdf)
        //    {
        //        Canvas canvas = new Canvas(placeholder, pdf);
        //        canvas.ShowTextAligned(pdf.GetNumberOfPages().ToString(),
        //            0, descent, TextAlignment.LEFT);
        //        canvas.Close();
        //    }
        //}
    }
}
