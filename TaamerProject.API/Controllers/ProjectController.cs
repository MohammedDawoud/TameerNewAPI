using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System;
using System.Data.SqlClient;
using System.Data;
using System.IO.Compression;
using Newtonsoft.Json;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using System.Web.Script.Serialization;
using System.Globalization;
using TaamerProject.Service.Interfaces;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaamerProject.Models.Common;
using System.Net.Mail;
using ZXing;
using TaamerProject.Service.Services;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProjectController : ControllerBase
    {
            private IProjectService _projectservice;
            private IBranchesService _branchesService;
            private IAccountsService _AccountsService;
            private ICustomerService _customerService;
            private ITasksDependencyService _tasksDependencyService;
            private IProUserPrivilegesService _proUserPrivilegesService;
            private IProSettingDetailsService _ProSettingDetailsService;
            private IProjectPhasesTasksService _projectPhasesTasksservice;
            private IOrganizationsService _organizationsservice;
            private IBranchesService _BranchesService;
            private byte[] ReportPDF;
            private IProjectPiecesService _ProjectPiecesservice;
            private IProjectArchivesReService _ProjectArchivesReService;
            private IProjectArchivesSeeService _ProjectArchivesSeeService;
            private IPro_SuperContractorService _Pro_SuperContractorService;
            private IFollowProjService _followProjService;
            private IImportantProjectService _importantProjectService;
            private IUsersService _usersService;
            private readonly IFiscalyearsService _FiscalyearsService;

            private IWorkOrdersService _workOrdersService;
            private IConfiguration Configuration;
            public GlobalShared _globalshared;
            private readonly IWebHostEnvironment _hostingEnvironment;

        string Con;
            public ProjectController(IProjectService projectService, IBranchesService branchesService, IAccountsService accountsService, ICustomerService customerService,
                ITasksDependencyService tasksDependencyService, IProUserPrivilegesService proUserPrivilegesService, IProSettingDetailsService proSettingDetailsService,
                IProjectPhasesTasksService projectPhasesTasks, IOrganizationsService organizations, IBranchesService branchesService1, IProjectPiecesService projectPieces,
                IProjectArchivesReService projectArchivesRe, IProjectArchivesSeeService projectArchivesSee, IPro_SuperContractorService pro_SuperContractor,
                IFollowProjService followProjService, IImportantProjectService importantProjectService, IUsersService usersService, IFiscalyearsService fiscalyearsService,
                IWorkOrdersService workOrdersService, IConfiguration _configuration, IWebHostEnvironment webHostEnvironment)
            {
                this._BranchesService = branchesService1;
                this._projectservice = projectService;
                this._branchesService = branchesService;
                this._AccountsService = accountsService;
                this._customerService =  customerService;
                this._projectPhasesTasksservice = projectPhasesTasks;
                this._organizationsservice = organizations;

                this._proUserPrivilegesService = proUserPrivilegesService;
                this._tasksDependencyService = tasksDependencyService;

                this._ProSettingDetailsService = proSettingDetailsService;
                this._ProjectPiecesservice = projectPieces;
                this._ProjectArchivesReService = projectArchivesRe;
                this._ProjectArchivesSeeService = projectArchivesSee;
                this._Pro_SuperContractorService = pro_SuperContractor;
                this._followProjService = followProjService;
                this._importantProjectService = importantProjectService;

                this._usersService = usersService;
                this._workOrdersService = workOrdersService;
                this._FiscalyearsService = fiscalyearsService;

            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext;

            _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        //public IActionResult DetailedRevenue()
        //{
        //    return View();
        //}
        //public IActionResult DetailedRevenueExtra()
        //{
        //    return View();
        //}
        //public IActionResult DetailedExpenses()
        //{
        //    return View();
        //}
        ////public IActionResult GetUserProjectsCount()
        ////{
        ////    var Counts = new
        ////    {
        ////        GetUserProjects = _projectservice.GetUserProjects(UserId,  _globalshared.BranchId_G).Count(),
        ////    };
        ////    return Ok(Counts );
        ////}
        //public IActionResult ArchiveProjects()
        //{
        //    //return PartialView("_archiveProjects");
        //    return View();
        //}
        //public IActionResult CostProjects()
        //{
        //    return View();
        //}
        //public IActionResult followProject()
        //{
        //    return View();
        //}
        //public IActionResult ProjectUsers()
        //{
        //    return View();
        //}
        //public IActionResult ProjectsTasksReport()
        //{
        //    return View();
        //}
        //public IActionResult FileSearch()
        //{
        //    return View();
        //}
        //public IActionResult ProjectManagerRevenue()
        //{
        //    return View();
        //}W
        //public IActionResult PrintProjectManagerRevenue()
        //{
        //    return View();
        //}

        //public IActionResult ProjectReport()
        //{
        //    return View();
        //}

        //public IActionResult ProjectStatus()
        //{
        //    return View();
        //}

        [HttpGet("ProjectDetails")]
        public IActionResult ProjectDetails(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //var AllProject = _projectservice.GetProjectById(_globalshared.Lang_G, ProjectId).Result;
            var obj = new ProjectVM();
            obj.BranchId = _globalshared.BranchId_G;
            obj.Status = 0;
            obj.ProjectId = ProjectId;

            var AllProject = _projectservice.GetAllProjectsNew(Con ?? "", obj, _globalshared.UserId_G, 0,0, _globalshared.BranchId_G).Result.FirstOrDefault();

            var ProS = _tasksDependencyService.GetTasksDependency(ProjectId);
                if (ProS != null)
                {


                    var result = _ProSettingDetailsService.CheckProSettingData2(ProS.ProjSubTypeId, _globalshared.BranchId_G).Result;
                    if (result != null)
                    {
                        AllProject!.SettingNoP = result.ProSettingNo;
                        AllProject!.SettingNoteP = result.ProSettingNote;
                    }
                    else
                    {
                        AllProject!.SettingNoP = "لا يوجد";
                        AllProject!.SettingNoteP = "لا يوجد";
                    }

                }
                else
                {
                    AllProject.SettingNoP = "لا يوجد";
                    AllProject.SettingNoteP = "لا يوجد";
                }


                return Ok(AllProject);
            }
        [HttpGet("GetProSettingDetails")]
        public IActionResult GetProSettingDetails(int ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllProject = new ProjectVM();

            var ProS = _tasksDependencyService.GetTasksDependency(ProjectId);
            if (ProS != null)
            {
                var result = _ProSettingDetailsService.CheckProSettingData2(ProS.ProjSubTypeId, _globalshared.BranchId_G).Result;
                if (result != null)
                {
                    AllProject.SettingNoP = result.ProSettingNo;
                    AllProject.SettingNoteP = result.ProSettingNote;
                }
                else
                {
                    AllProject.SettingNoP = "لا يوجد";
                    AllProject.SettingNoteP = "لا يوجد";
                }

            }
            else
            {
                AllProject.SettingNoP = "لا يوجد";
                AllProject.SettingNoteP = "لا يوجد";
            }
            return Ok(AllProject);
        }

        [HttpGet("ProjectDetails2")]
        public IActionResult ProjectDetails2(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllProject = _projectservice.GetProjectById(_globalshared.Lang_G, ProjectId).Result;

                var ProS = _tasksDependencyService.GetTasksDependency(ProjectId);
                if (ProS != null)
                {


                    var result = _ProSettingDetailsService.CheckProSettingData2(ProS.ProjSubTypeId, _globalshared.BranchId_G).Result;
                    if (result != null)
                    {
                        AllProject.SettingNoP = result.ProSettingNo;
                        AllProject.SettingNoteP = result.ProSettingNote;
                    }
                    else
                    {
                        AllProject.SettingNoP = "لا يوجد";
                        AllProject.SettingNoteP = "لا يوجد";
                    }

                }
                else
                {
                    AllProject.SettingNoP = "لا يوجد";
                    AllProject.SettingNoteP = "لا يوجد";
                }

                return Ok(AllProject);

            }
        [HttpPost("SaveProjectDetails")]
        public IActionResult SaveProjectDetails(ProjectVM project)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.SaveProjectDetails(project, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }
        [HttpGet("GetProjectById")]
        public IActionResult GetProjectById(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectservice.GetProjectById(_globalshared.Lang_G, ProjectId) );
            }

        [HttpGet("GetProjectByIdSomeData")]
        public IActionResult GetProjectByIdSomeData(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectservice.GetProjectByIdSome(_globalshared.Lang_G, ProjectId) );
            }
        [HttpGet("GetCostCenterByProId")]
        public IActionResult GetCostCenterByProId(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectservice.GetCostCenterByProId(_globalshared.Lang_G, ProjectId) );
            }

        [HttpGet("GetSomeDataByProjId")]
        public IActionResult GetSomeDataByProjId(int Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var OfferPrice = _projectservice.GetSomeDataByProjId(Param).Result.Select(s => new
                {
                    Id = s.OffersPricesId,
                    Name = s.OfferPriceNoName
                });
                return Ok(OfferPrice );

            }

        [HttpGet("GetProjectDataOffice")]
        public IActionResult GetProjectDataOffice(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectservice.GetProjectDataOffice(_globalshared.Lang_G, ProjectId).Result );
            }

        [HttpGet("GetProjectByIdStopType")]
        public IActionResult GetProjectByIdStopType(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectservice.GetProjectByIdStopType(_globalshared.Lang_G, ProjectId).Result.StopProjectType );
            }

        [HttpGet("GetProjectAddUser")]
        public IActionResult GetProjectAddUser(int ProjectId)
        {
        return Ok(_projectservice.GetProjectAddUser(ProjectId) );
        }

        [HttpGet("GetLastProject")]
        public IActionResult GetLastProject()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someProject = _projectservice.GetAllProject(_globalshared.Lang_G, 0).Result;
                foreach (var userBranch in userBranchs)
                {

                    var AllPojects = _projectservice.GetAllProject(_globalshared.Lang_G, userBranch.BranchId).Result.ToList();

                    var Projects = someProject.Union(AllPojects);
                    someProject = Projects.ToList();
                }

                var lastpro = someProject.Max(p => p.ProjectId);
                var proj = _projectservice.GetProjectById(_globalshared.Lang_G, lastpro);
                return Ok(proj );

            }
        //public IActionResult GetAllProjects()
        //{
        //    var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G);
        //    var someProject = _projectservice.GetAllProject(_globalshared.Lang_G, 0);
        //    foreach (var userBranch in userBranchs)
        //    {

        //        var AllPojects = _projectservice.GetAllProject(_globalshared.Lang_G, userBranch. _globalshared.BranchId_G).ToList();

        //        var Projects = someProject.Union(AllPojects);
        //        someProject = Projects.ToList();
        //    }
        //    List<ProjectVM> allproject = new List<ProjectVM>();
        //    foreach (var item in someProject)
        //    {
        //        var imp = _importantProjectService.GetImportantProjects(item.ProjectId,_globalshared.UserId_G).FirstOrDefault();
        //        if (imp != null)
        //        {
        //            item.Isimportant = imp.IsImportant ?? 0;
        //            item.importantid = imp.ImportantProId;
        //            item.flag = imp.Flag ?? 0;
        //        }
        //        else
        //        {
        //            item.Isimportant = 0;
        //            item.importantid = 0;
        //            item.flag = 0;
        //        }
        //        allproject.Add(item);
        //    }

        //    var serializer = new JavaScriptSerializer();
        //    serializer.MaxJsonLength = Int32.MaxValue;
        //    var result = new ContentResult
        //    {
        //        Content = serializer.Serialize(allproject),
        //        ContentType = "application/json"
        //    };
        //    //return Ok(someProject );
        //    return result;

        //}


        //Get Projects With Procedure
        [HttpGet("GetAllProjects")]
        public IActionResult GetAllProjects()
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var obj = new ProjectVM();
            obj.BranchId= _globalshared.BranchId_G;
            obj.Status = 0;
            var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", obj, _globalshared.UserId_G, 0,0, _globalshared.BranchId_G).Result.ToList();
            return Ok(AllPojects);
        }

        [HttpGet("GetAllProjectsmartfollow")]
        public IActionResult GetAllProjectsmartfollow()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var SearchObj = new ProjectVM();
            SearchObj.Status = 0;
            SearchObj.MangerId = _globalshared.UserId_G;
            SearchObj.BranchId = _globalshared.BranchId_G;

            var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", SearchObj, _globalshared.UserId_G, 0,0, _globalshared.BranchId_G).Result.ToList();
            return Ok(AllPojects);
        }
        [HttpGet("GetAllProjectsmartfollowforadmin")]
        public IActionResult GetAllProjectsmartfollowforadmin()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var SearchObj = new ProjectVM();
            SearchObj.Status = 0;
            SearchObj.BranchId = _globalshared.BranchId_G;
            var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", SearchObj, _globalshared.UserId_G, 0,0, _globalshared.BranchId_G).Result.ToList();
            return Ok(AllPojects);

        }
        [HttpGet("GetMaxUserProjects")]
        public IActionResult GetMaxUserProjects()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranches(_globalshared.Lang_G).Result;
                var someProject = _projectservice.GetAllProjects3(Con, _globalshared.Lang_G, 0,_globalshared.UserId_G).Result;
                foreach (var userBranch in userBranchs)
                {

                    var AllPojects = _projectservice.GetAllProjects3(Con, _globalshared.Lang_G, userBranch.BranchId,_globalshared.UserId_G).Result.ToList();

                    var Projects = someProject.Union(AllPojects);
                    someProject = Projects.ToList();
                }
                MaxProjevtAndTasksVM mxpro = new MaxProjevtAndTasksVM();
                if (someProject.Count() > 0)
                {


                    var max = someProject.GroupBy(n => n.MangerId).OrderByDescending(g => g.Count()).First();
                    mxpro.Count = max.Count();
                    var maxpro = max.ToList().FirstOrDefault();
                    mxpro.ManagerId = maxpro!.MangerId;

                    mxpro.ManagerName = maxpro.ProjectMangerName;
                    mxpro.ImgUrl = _usersService.GetUserById(maxpro.MangerId??0, _globalshared.Lang_G).Result.ImgUrl;
                    //  mxpro.ImgUrl = _usersService.GetUserById(1, _globalshared.Lang_G).ImgUrl;
                }
                else
                {
                    mxpro.Count = 0;
                    mxpro.ImgUrl = "/distnew/images/userprofile.png";

                }
                return Ok(mxpro );


            }

        [HttpGet("GetAllProjectsWithCostE_CostS")]
        public IActionResult GetAllProjectsWithCostE_CostS()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var SearchObj = new ProjectVM();
            SearchObj.Status = 0;
            SearchObj.BranchId = _globalshared.BranchId_G;
            var AllPojects = _projectservice.GetAllProjectsNew(Con??"", SearchObj, _globalshared.UserId_G,0,0,_globalshared.BranchId_G).Result.Where(s => s.CostE > 0 || s.CostS > 0).ToList();
            return Ok(AllPojects);
        }
        [HttpGet("GetAllProjectsWithoutCostE_CostS")]
        public IActionResult GetAllProjectsWithoutCostE_CostS()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var SearchObj = new ProjectVM();
            SearchObj.Status = 0;
            SearchObj.BranchId = _globalshared.BranchId_G;
            var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", SearchObj, _globalshared.UserId_G, 0,0, _globalshared.BranchId_G).Result.ToList();
            return Ok(AllPojects);
        }
        [HttpGet("GetAllProjectsStatusTasks")]
        public IActionResult GetAllProjectsStatusTasks()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someProject = _projectservice.GetAllProjectsStatusTasks(_globalshared.Lang_G, 0).Result;
                foreach (var userBranch in userBranchs)
                {

                    var AllPojects = _projectservice.GetAllProjectsStatusTasks(_globalshared.Lang_G, userBranch.BranchId).Result.ToList();
                    var Projects = someProject.Union(AllPojects);
                    someProject = Projects.ToList();
                }
            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;
            //var result = new ContentResult
            //{
            //    Content = serializer.Serialize(someProject),
            //    ContentType = "application/json"
            //};
            ////return Ok(someProject );
            //return result;
            return Ok(someProject);

        }

        [HttpGet("GetAllProjectsCount")]
        public int GetAllProjectsCount()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var ProjectCount = 0;
                foreach (var userBranch in userBranchs)
                {
                    var AllPojects = _projectservice.GetAllProject(_globalshared.Lang_G, userBranch.BranchId).Result.Count();
                    var Projects = ProjectCount + AllPojects;
                    ProjectCount = Projects;
                }
                return ProjectCount;
            }
        [HttpGet("GetArchiveProjects")]
        public int GetArchiveProjects()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var ProjectCount = 0;
                foreach (var userBranch in userBranchs)
                {
                    var AllPojects = _projectservice.GetAllArchiveProject(userBranch.BranchId).Result.Count();
                    var Projects = ProjectCount + AllPojects;
                    ProjectCount = Projects;
                }
                return ProjectCount;
            }
        [HttpGet("GetAllProjectNumbers")]
        public IActionResult GetAllProjectNumbers(string SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someProject = _projectservice.GetAllProjectNumber(SearchText, 0).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllPojects = _projectservice.GetAllProjectNumber(SearchText, userBranch.BranchId).Result.ToList();
                    var Projects = someProject.Union(AllPojects);
                    someProject = Projects.ToList();
                }
                return Ok(someProject );
            }
        [HttpGet("BarcodePDF")]
        public IActionResult BarcodePDF(int FileID)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.BarcodePDF(FileID,_globalshared.UserId_G);


                //string oldFile = Path.Combine(Server.MapPath("~/TempFiles"), fileName);
                string oldFile = Path.Combine(Path.Combine("TempFiles"), "PDFFile_637493197851726875.pdf");

                string newFile = Path.Combine(Path.Combine("TempFiles"), "PDFFile_637493197981665437.pdf");

                // open the reader
                PdfReader reader = new PdfReader(oldFile);
                iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
                Document document = new Document(size);

                // open the writer
                FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // the pdf content
                PdfContentByte cb = writer.DirectContent;

                var bc = new Barcode128
                {
                    Code = "123456789000",
                    TextAlignment = Element.ALIGN_CENTER,
                    StartStopText = true,
                    CodeType = Barcode.CODE128,
                    Extended = false
                };

                iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                var barCodeRect = new iTextSharp.text.Rectangle(bc.BarcodeSize);
                //var widthScale = size.Width / barCodeRect.Width;
                //var heightScale = size.Height / barCodeRect.Height;
                //var hMargin = 1;
                //var vMargin = 1;
                iTextSharp.text.Rectangle tempRect;
                tempRect = new iTextSharp.text.Rectangle(0, 0, 180, 50);//(,,3rd,toool)

                img.ScaleAbsolute(tempRect);
                img.SetAbsolutePosition(30, 30);

                //img.SetAbsolutePosition((size.Width - tempRect.Width) / 2, (size.Height - tempRect.Height) / 2);
                cb.AddImage(img);


                PdfImportedPage page = writer.GetImportedPage(reader, 1);
                cb.AddTemplate(page, 0, 0);

                // close the streams and voilá the file should be changed :)
                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();

            return Ok(result);

        }


        [HttpGet("GetAllArchiveProjects")]
        public IActionResult GetAllArchiveProjects()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var obj = new ProjectVM();
            obj.BranchId = _globalshared.BranchId_G;
            obj.Status = 1;
            var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", obj, _globalshared.UserId_G, 0,0, _globalshared.BranchId_G).Result.ToList();
            return Ok(AllPojects);
        }

        [HttpGet("GetProjectsByCustomerId")]
        public IActionResult GetProjectsByCustomerId(int? CustomerId, int? Status)
        {HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectservice.GetProjectsByCustomerId(CustomerId, Status) );
        }
        [HttpGet("GetUserProjects")]
        public IActionResult GetUserProjects(int UserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someProject = _projectservice.GetUserProjects(UserId, 0, "").Result;
                foreach (var userBranch in userBranchs)
                {

                    var AllPojects = _projectservice.GetUserProjects(UserId, userBranch.BranchId, "").Result.ToList();
                    var Projects = someProject.Union(AllPojects);
                    someProject = Projects.ToList();
                }
                return Ok(someProject );
            }
        [HttpPost("SaveProject")]
        public IActionResult SaveProject(Project project)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            try
            {
                var url = Path.Combine("Email/MailStamp.html");
                //var file = Server.MapPath("~/dist/assets/images/logo.png");
                string resultLogoUrl = org.LogoUrl.Remove(0, 1);
                var file = Path.Combine(resultLogoUrl);
                var result = _projectservice.SaveProject(project, _globalshared.UserId_G, project.BranchId, url, file);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var projectid = Convert.ToInt32(result.ReturnedStr);

                    //ProjectPhasesTasks _ProjectPhasesTasks = new ProjectPhasesTasks();
                    //ProjectPhasesTasks _ProjectPhasesTasks2 = new ProjectPhasesTasks();
                    //_ProjectPhasesTasks.ProjectId = Convert.ToInt32(result.ReturnedStr);
                    //_ProjectPhasesTasks2.ProjectId = Convert.ToInt32(result.ReturnedStr);
                    var resultMain = _projectPhasesTasksservice.SavefourMainPhases_P(projectid, _globalshared.UserId_G, _globalshared.BranchId_G);

                    //var resultMain = _projectPhasesTasksservice.SaveMainPhases_P(_ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G);
                    //var resultSub = _projectPhasesTasksservice.SaveSubPhases_P(_ProjectPhasesTasks2, _globalshared.UserId_G, _globalshared.BranchId_G, Convert.ToInt32(resultMain.ReturnedParm));
                    List<ProUserPrivileges> NewPrivs = new List<ProUserPrivileges>();
                    foreach (var pri in project.ProUserPrivileges)
                    {
                        pri.ProjectID = Convert.ToInt32(result.ReturnedStr);
                        NewPrivs.Add(pri);
                    }

                    var result2 = _proUserPrivilegesService.SavePrivProList(NewPrivs, _globalshared.UserId_G, project.BranchId);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                SendMail_ProjectSavedWrong("Part (0)" + " " + ex.Message + ">>>>" + ex.InnerException, false, org);
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "فشل في الحفظ", ReturnedStr = "Part (0)" + " " + ex.Message + ">>>>" + ex.InnerException });
            }

        }
        [HttpPost("SendMail_ProjectSavedWrong")]
        public bool SendMail_ProjectSavedWrong(string textBody, bool IsBodyHtml,OrganizationsVM Org)
        {
            try
            {

                var mail = new MailMessage();
                var email = "noreply-tameer@bayanatech.com.sa";
                var password = "eA4LQkrbQdCm5jqt";
                var loginInfo = new NetworkCredential(email, password);
                mail.From = new MailAddress(email, "TAMEER-CLOUD-SYSTEM");

                mail.To.Add(new MailAddress("mohammeddawoud66@gmail.com"));
                mail.Subject = "فشل حفظ مشروع " + " " + Org.NameAr;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var smtpClient = new SmtpClient("mail.bayanatech.com.sa");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost("ChangeProjectManagerCheckBox")]
        public IActionResult ChangeProjectManagerCheckBox([FromForm] List<string> projectIds,[FromForm] string mangerId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.PostProjectsCheckBox(projectIds, mangerId,  _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("DeleteProject")]
        public IActionResult DeleteProject(int ProjectId)
            {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.DeleteProject(ProjectId,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SendCustomerEmail_SMS")]
        public IActionResult SendCustomerEmail_SMS(int CustomerId, int ProjectId, int TypeId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.SendCustomerEmail_SMS(CustomerId, ProjectId, TypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("Updateskiptime")]
        public IActionResult Updateskiptime(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.Updateskiptime(ProjectId,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("UpdateProjectnoSpaces")]
        public IActionResult UpdateProjectnoSpaces()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.UpdateProjectnoSpaces(_globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteProject_NEW")]
        public IActionResult DeleteProject_NEW(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.DeleteProjectNEW(ProjectId,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteAllProject_NEW")]
        public IActionResult DeleteAllProject_NEW(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.DeleteAllProject_NEW(ProjectId,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteAllProject_NEWWithVouchers")]
        public IActionResult DeleteAllProject_NEWWithVouchers(int ProjectId,string password)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.DeleteAllProject_NEWWithVouchers(ProjectId, password, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("PrintProjectsPhasesTasks")]

        public IActionResult PrintProjectsPhasesTasks(int? ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            PhasesReportVM _phasesReportVM = new PhasesReportVM();
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            List<ProjectPhasesTasksVM> phasesVM = _projectPhasesTasksservice.GetAllProjectPhasesAndTasks(ProjectId ?? 0, _globalshared.Lang_G).Result.ToList();
            // ReportPDF = ProjectsReports.SupervisionRep(SuperDetailsVM, "", "", infoDoneTasksReport);
            _phasesReportVM.Org_VD = objOrganization;
            string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            _phasesReportVM.DateTimeNow = Date;
            _phasesReportVM.Date = Date;
            _phasesReportVM.phases = phasesVM;
            return Ok(_phasesReportVM);
        }

        [HttpPost("DestinationsUploadProject")]
        public IActionResult DestinationsUploadProject(int ProjectId, int? status)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.DestinationsUploadProject(ProjectId, status??0, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("StopProject")]
        public IActionResult StopProject(int ProjectId,int TypeId,int whichClickDesc)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectservice.StopProject(ProjectId,_globalshared.UserId_G,  _globalshared.BranchId_G, url, file, TypeId, whichClickDesc);
            return Ok(result);
        }
        [HttpPost("PlayProject")]
        public IActionResult PlayProject(int ProjectId, int TypeId, int whichClickDesc)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectservice.PlayProject(ProjectId,_globalshared.UserId_G,  _globalshared.BranchId_G, url, file, TypeId, whichClickDesc);
            return Ok(result);
        }

        [HttpGet("FillProjectSelect")]
        public IActionResult FillProjectSelect()         
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con,_globalshared.UserId_G,1,null,0, _globalshared.BranchId_G).Result;
            return Ok(result);
        }
        [HttpGet("FillProjectSelectGantt")]
        public IActionResult FillProjectSelectGantt()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, 1, 0, _globalshared.BranchId_G).Result;
            return Ok(result);
        }
        [HttpGet("FillProjectSelectByCustomerId")]
        public IActionResult FillProjectSelectByCustomerId(int Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result.Where(t => t.CustomerId == Param).Select(s => new {
                Id = s.Id,
                Name = s.ProjectNo + " - " + s.CustomerName,
            }); ;
            return Ok(result);
        }
        [HttpGet("FillProjectSelectByCustomerIdWiBranch")]
        public IActionResult FillProjectSelectByCustomerIdWiBranch(int Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 2, null, 0, null).Result.Where(t => t.CustomerId == Param).Select(s => new {
                Id = s.Id,
                Name = s.ProjectNo + " - " + s.CustomerName,
            }); ;
            return Ok(result);
        }
        [HttpGet("FillProjectSelectByCustomerId2")]
        public IActionResult FillProjectSelectByCustomerId2(int Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result.Where(t => t.CustomerId == Param && (t.ContractId == 0)).Select(s => new {
                Id = s.Id,
                Name = s.ProjectNo + " - " + s.CustomerName,
            }); 
            return Ok(result);
        }
        [HttpGet("FillProjectSelectByCustomerId_W")]
        public IActionResult FillProjectSelectByCustomerId_W(int Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result.Where(t => t.CustomerId == Param).Select(s => new {
                Id = s.Id,
                Name = s.ProjectNo,
            }); ;
            return Ok(result);
        }
        [HttpGet("FillCustomerSelectWProC")]
        public IActionResult FillCustomerSelectWProC()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result.Where(t => t.ContractId == 0).Select(s => new {
                Id = s.CustomerId ?? 0,
                Name = s.CustomerName,
            }).Distinct(); 
            return Ok(result);
        }

        [HttpGet("FillCustomerSelectWProC2")]
        public IActionResult FillCustomerSelectWProC2()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result.Where(t => t.ContractId == 0 && t.ProjectTypeId == 1).Select(s => new {
                Id = s.CustomerId ?? 0,
                Name = s.CustomerName,
            }).Distinct(); 
            return Ok(result);
        }


        [HttpGet("FillCustomerSelectWProC3")]
        public IActionResult FillCustomerSelectWProC3()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result.Where(t => t.ContractId == 0 && (t.TypeCode == 3 || t.TypeCode == 6 || t.TypeCode == 7)).Select(s => new {
                Id = s.CustomerId ?? 0,
                Name = s.CustomerName,
            }).Distinct(); 
            return Ok(result);
        }
        [HttpGet("FillProjectSelectByCustomerId2_Supervision")]
        public IActionResult FillProjectSelectByCustomerId2_Supervision(int Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result.Where(t => t.CustomerId == Param && (t.TypeCode == 3 || t.TypeCode == 6 || t.TypeCode == 7)).Select(s => new {
                Id = s.Id,
                Name = s.ProjectNo + " - " + s.CustomerName,
            }); 
            return Ok(result);
        }

        [HttpGet("FillCustomerSelectWProC_Supervision")]
        public IActionResult FillCustomerSelectWProC_Supervision()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result.Where(t => t.TypeCode == 3 || t.TypeCode == 6 || t.TypeCode == 7).Select(s => new {
                Id = s.CustomerId ?? 0,
                Name = s.CustomerName,
            });
            return Ok(result);
        }



        [HttpGet("FillCustomerSelectWProOnly")]
        public IActionResult FillCustomerSelectWProOnly()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result.Select(s => new {
                Id = s.CustomerId ?? 0,
                Name = s.CustomerName,
            });
            return Ok(result);
        }
        [HttpGet("FillCustomerSelectWProOnlyWithBranch")]
        public IActionResult FillCustomerSelectWProOnlyWithBranch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 3, null, 0, _globalshared.BranchId_G).Result.Select(s => new {
                Id = s.CustomerId ?? 0,
                Name = s.CustomerName,
            });
            return Ok(result);
        }
        [HttpGet("FillCustomerSelectWProOnlyWithout")]
        public IActionResult FillCustomerSelectWProOnlyWithout()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, null, _globalshared.BranchId_G).Result.Select(s => new {
                Id = s.CustomerId ?? 0,
                Name = s.CustomerName,
            });
            return Ok(result);
        }

        [HttpGet("FillProjectSelectByCustomerIdNotifaction")]
        public IActionResult FillProjectSelectByCustomerIdNotifaction()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result.Select(s => new {
                Id = s.Id,
                Name = s.ProjectNo + " - " + s.CustomerName,
            });
            return Ok(result);
        }
        [HttpPost("GenerateProjectFiles")]
        public IActionResult GenerateProjectFiles(List<Project> projects)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            List<string> fileUrls = new List<string>();
                foreach (var item in projects)
                {
                    var project = _projectservice.GetProjectById(_globalshared.Lang_G, item.ProjectId);
                    try
                    {
                        string dir = Path.Combine("Files/ProjectFiles/" + item.ProjectNo.ToString());
                        var fileName = Path.Combine("Files/ProjectFiles/" + item.ProjectNo.ToString() + ".zip");
                        if (System.IO.File.Exists(fileName))
                        {
                            System.IO.File.Delete(fileName);
                        }
                        fileUrls.Add("/Files/ProjectFiles/" + item.ProjectNo.ToString() + ".zip");
                        ZipFile.CreateFromDirectory(dir, fileName, CompressionLevel.Fastest, true);
                    }
                    catch (Exception e)
                    {
                        var x = e.Message;
                        return Ok(0 );
                    }
                    finally { }
                }
                return Ok(fileUrls );
            }


        //public IActionResult AddTask(int ProjectNo, string ProjectMainPhases, string ProjectSubPhases, string CustomerName, int ProjectId)
        //{

        //    TempData["ProjectNo"] = ProjectNo;
        //    TempData["ProjectMainPhases"] = ProjectMainPhases;
        //    TempData["ProjectSubPhases"] = ProjectSubPhases;
        //    TempData["CustomerName"] = CustomerName;
        //    TempData["ProjectId"] = ProjectId;

        //    //return View("~/Views/ProjectPhasesTasks/Tasks.cshtml");
        //    return View("../ProjectPhasesTasks/Tasks", 2);
        //}
        [HttpPost("GetProjectsSearch")]
        public IActionResult GetProjectsSearch(ProjectVM ProjectsSearch)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllUserBranch = 0;
            //if(ProjectsSearch.BranchId==-1)
            //{
            //    AllUserBranch = 1;
            //}
            AllUserBranch = 1;
            var AccBranchesList = ProjectsSearch.BranchesList ?? new List<int>();
            if (AccBranchesList.Count() == 0)
            {
                AccBranchesList.Add(_globalshared.BranchId_G);
            }

            ProjectsSearch.BranchId = _globalshared.BranchId_G;
            var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", ProjectsSearch, _globalshared.UserId_G, AllUserBranch, ProjectsSearch.FilterType??0, _globalshared.BranchId_G).Result.ToList();
            AllPojects = AllPojects.Where(s => AccBranchesList.Contains(s.BranchId ?? 0)).ToList();
            return Ok(AllPojects);
        }

        [HttpPost("GetProjectsSearch_paging")]
        public IActionResult GetProjectsSearch(ProjectVM ProjectsSearch, string? SearchText, int page = 1, int pageSize = 10)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ProjectsSearch.BranchId = _globalshared.BranchId_G;
            //var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0, ProjectsSearch.FilterType ?? 0, _globalshared.BranchId_G).Result.ToList();
            //return Ok(AllPojects);

            List<ProjectVM> AllPojects = new List<ProjectVM>();
            if (SearchText != null && SearchText != "")
            {
                AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0, ProjectsSearch.FilterType ?? 0, _globalshared.BranchId_G).Result.ToList().Where(x => x.CustomerName.Contains(SearchText) || x.ProjectNo.Contains(SearchText) || x.ProjectDescription.Contains(SearchText) || x.ContractValue.Contains(SearchText) || SearchText == null || SearchText == "").ToList();

            }
            else
            {
                AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0, ProjectsSearch.FilterType ?? 0, _globalshared.BranchId_G).Result.ToList();

            }
            var data = GeneratePagination<ProjectVM>.ToPagedList(AllPojects.ToList(), page, pageSize);
            var result = new PagedLists<ProjectVM>(data.MetaData, data);
            return Ok(result);
        }


        [HttpPost("GetProjectsSearch_Export")]
        public IActionResult GetProjectsSearch_Export(ProjectVM ProjectsSearch, string? SearchText, int page = 1, int pageSize = 10)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ProjectsSearch.BranchId = _globalshared.BranchId_G;
            //var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0, ProjectsSearch.FilterType ?? 0, _globalshared.BranchId_G).Result.ToList();
            //return Ok(AllPojects);

            List<ProjectVM> AllPojects = new List<ProjectVM>();
            if (SearchText != null && SearchText != "")
            {
                AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0, ProjectsSearch.FilterType ?? 0, _globalshared.BranchId_G).Result.ToList().Where(x => x.CustomerName.Contains(SearchText) || x.ProjectNo.Contains(SearchText) || x.ProjectDescription.Contains(SearchText) || x.ContractValue.Contains(SearchText) || SearchText == null || SearchText == "").ToList();

            }
            else
            {
                AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0, ProjectsSearch.FilterType ?? 0, _globalshared.BranchId_G).Result.ToList();

            }
            return Ok(AllPojects);
        }


        [HttpPost("GetAllProjectsNew_DashBoard")]
        public IActionResult GetAllProjectsNew_DashBoard(ProjectVM ProjectsSearch)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ProjectsSearch.BranchId = _globalshared.BranchId_G;
            var AllPojects = _projectservice.GetAllProjectsNew_DashBoard(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0, _globalshared.BranchId_G).Result.ToList();
            return Ok(AllPojects);
        }


        [HttpPost("GetAllProjectsNew_DashBoard_Paging")]
        public IActionResult GetAllProjectsNew_DashBoard_Paging(ProjectVM ProjectsSearch,string? SearchText, int page = 1, int pageSize = 10)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ProjectsSearch.BranchId = _globalshared.BranchId_G;
            List<ProjectVM> AllPojects = new List<ProjectVM>();
            if (SearchText != null && SearchText != "")
            {
                 AllPojects = _projectservice.GetAllProjectsNew_DashBoard(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0, _globalshared.BranchId_G).Result.ToList().Where(x => x.CustomerName.Contains(SearchText) || x.ProjectNo.Contains(SearchText) || x.ProjectDescription.Contains(SearchText) || x.ContractValue.Contains(SearchText) || SearchText == null || SearchText == "").ToList();

            }
            else
            {
                 AllPojects = _projectservice.GetAllProjectsNew_DashBoard(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0, _globalshared.BranchId_G).Result.ToList();

            }
            var data = GeneratePagination<ProjectVM>.ToPagedList(AllPojects.ToList(), page, pageSize);
            var result = new PagedLists<ProjectVM>(data.MetaData, data);
            return Ok(result);
        }

        [HttpPost("GetProjectsStatusTasksSearch")]
        public IActionResult GetProjectsStatusTasksSearch(ProjectVM ProjectsSearch)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someProject = _projectservice.GetProjectsStatusTasksSearch(ProjectsSearch, 0, Con, _globalshared.Lang_G).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllPojects = _projectservice.GetProjectsStatusTasksSearch(ProjectsSearch, userBranch.BranchId, Con, _globalshared.Lang_G).Result.ToList();
                    var Projects = someProject.Union(AllPojects);
                    someProject = Projects.ToList();
                }
                return Ok(someProject );
            }
        [HttpGet("FillAllUsersByProject")]
        public IActionResult FillAllUsersByProject()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectservice.FillAllUsersByProject( _globalshared.BranchId_G, _globalshared.Lang_G).Result);
            }
        [HttpGet("GetUserProjectsReport")]
        public IActionResult GetUserProjectsReport(int UserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllPojects = _projectservice.GetUserProjectsReport(UserId,  _globalshared.BranchId_G, "").Result.ToList();

                return Ok(AllPojects );
            }
        [HttpGet("GetUserProjectsReportW")]
        public IActionResult GetUserProjectsReportW()
            {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someTask = _projectservice.GetUserProjectsReportW( _globalshared.BranchId_G, "").Result.ToList();

                //var serializer = new JavaScriptSerializer();
                //serializer.MaxJsonLength = Int32.MaxValue;
                //var result = new ContentResult
                //{
                //    Content = serializer.Serialize(someTask),
                //    ContentType = "application/json"
                //};
                //return result;
            return Ok(someTask);
            }
        [HttpGet("CheckPercentage")]
        public IActionResult CheckPercentage(int phasetaskid)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.checkprojectpercentage( _globalshared.BranchId_G, "", phasetaskid);
                return Ok(result );


            }
        [HttpGet("GetUserProjectsReportS")]
        public IActionResult GetUserProjectsReportS(int? UserId, int? CustomerId, string? DateFrom, string? DateTo,int? BranchId, [FromQuery] List<int> BranchesList)
        {
        if (UserId == 0) UserId = null;
        HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllUserBranch = 0;
            if (BranchId == -1) AllUserBranch = 0;
            else AllUserBranch = _globalshared.BranchId_G;
            var AccBranchesList = BranchesList ?? new List<int>();
            if (AccBranchesList.Count() == 0)
            {
                AccBranchesList.Add(_globalshared.BranchId_G);
            }
            var AllPojects = _projectservice.GetUserProjectsReport(UserId, CustomerId, AllUserBranch, DateFrom??"", DateTo??"", AccBranchesList).Result.ToList();

            return Ok(AllPojects );
        }



        [HttpGet("GetUserProjectsReportS_paging")]
        public IActionResult GetUserProjectsReportS_paging(int? UserId, int? CustomerId, string? DateFrom, string? DateTo,string? SearchText, int page = 1, int pageSize = 10)
        {
            if (UserId == 0) UserId = null;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllPojects = _projectservice.GetUserProjectsReport(UserId, CustomerId, _globalshared.BranchId_G, DateFrom ?? "", DateTo ?? "", SearchText).Result.ToList();
            var data = GeneratePagination<ProjectVM>.ToPagedList(AllPojects.ToList(), page, pageSize);
            var result =new PagedLists<ProjectVM>(data.MetaData, data);


            return Ok(result);
        }

        [HttpGet("GetUserProjectsReportS_Export")]
        public IActionResult GetUserProjectsReportS_Export(int? UserId, int? CustomerId, string? DateFrom, string? DateTo, string? SearchText, int page = 1, int pageSize = 10)
        {
            if (UserId == 0) UserId = null;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllPojects = _projectservice.GetUserProjectsReport(UserId, CustomerId, _globalshared.BranchId_G, DateFrom ?? "", DateTo ?? "", SearchText).Result.ToList();
           


            return Ok(AllPojects);
        }
        //public IActionResult GetAllProjectUsersData(int_globalshared.UserId_G)
        //{
        //     ouddddd
        //    try
        //    {
        //        List<ProjectVM> Proj=new List<ProjectVM>();
        //        using (SqlConnection con = new SqlConnection(Con))
        //        {
        //            using (SqlCommand command = new SqlCommand())
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.CommandText = "Rpt_ProjectsUsers";
        //                command.Connection = con;
        //                //if(us)
        //                command.Parameters.Add(new SqlParameter("@userid",_globalshared.UserId_G));
        //                con.Open();
        //                SqlDataAdapter a = new SqlDataAdapter(command);
        //                DataSet ds = new DataSet();
        //                a.Fill(ds);
        //                DataTable dt = new DataTable();
        //                dt = ds.Tables[0];
        //                var x = dt.Rows.ToString();
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    Proj.Add(new ProjectVM
        //                    {
        //                        ProjectId =Convert.ToInt32((dr[0]).ToString()),
        //                        ProjectNo = dr[1].ToString(),
        //                        ProjectTypesName = dr[2].ToString(),
        //                        ProjectDescription = dr[3].ToString(),
        //                        CustomerName = dr[4].ToString(),
        //                        ProjectSubTypeName = dr[5].ToString(),
        //                        ProjectMangerName = dr[6].ToString(),
        //                        NoOfDays =Convert.ToInt32(dr[7].ToString()),
        //                        FirstProjectDate = dr[8].ToString(),
        //                        FirstProjectExpireDate = dr[9].ToString(),

        //                    });
        //                }
        //            }
        //        }
        //        return Ok(Proj );
        //    }
        //    catch (Exception)
        //    {
        //        return Ok(null );
        //    }

        //}

        //public IActionResult GetAllProjectUsersDataw()
        //{
        //    try
        //    {
        //        List<ProjectVM> Proj = new List<ProjectVM>();
        //        using (SqlConnection con = new SqlConnection(Con))
        //        {
        //            using (SqlCommand command = new SqlCommand())
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.CommandText = "Rpt_ProjectsUsers";
        //                command.Connection = con;
        //                //if(us)
        //                command.Parameters.Add(new SqlParameter("@userid", DBNull.Value));
        //                con.Open();
        //                SqlDataAdapter a = new SqlDataAdapter(command);
        //                DataSet ds = new DataSet();
        //                a.Fill(ds);
        //                DataTable dt = new DataTable();
        //                dt = ds.Tables[0];
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    Proj.Add(new ProjectVM
        //                    {
        //                        ProjectId = Convert.ToInt32((dr[0]).ToString()),
        //                        ProjectNo = dr[1].ToString(),
        //                        ProjectTypesName = dr[2].ToString(),
        //                        ProjectDescription = dr[3].ToString(),
        //                        CustomerName = dr[4].ToString(),
        //                        ProjectSubTypeName = dr[5].ToString(),
        //                        ProjectMangerName = dr[6].ToString(),
        //                        NoOfDays = Convert.ToInt32(dr[7].ToString()),
        //                        FirstProjectDate = dr[8].ToString(),
        //                        FirstProjectExpireDate = dr[9].ToString(),

        //                    });
        //                }
        //            }
        //        }
        //        return Ok(Proj );
        //    }
        //    catch (Exception)
        //    {
        //        return Ok(null );
        //    }

        //}
        [HttpGet("GetAllProjectsByDateSearch")]
        public IActionResult GetAllProjectsByDateSearch(string DateFrom, string DateTo)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someProject = _projectservice.GetAllProjectsByDateSearch(DateFrom, DateTo, 0).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllPojects = _projectservice.GetAllProjectsByDateSearch(DateFrom, DateTo, userBranch.BranchId).Result.ToList();
                    var Projects = someProject.Union(AllPojects);
                    someProject = Projects.ToList();
                }

                return Ok(someProject );
            }
        [HttpPost("GetArchiveProjectsSearch")]
        public IActionResult GetArchiveProjectsSearch(ProjectVM ProjectsSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ProjectsSearch.BranchId = _globalshared.BranchId_G;
            ProjectsSearch.Status = 1;
            var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0,0, _globalshared.BranchId_G).Result.ToList();
            return Ok(AllPojects);
        }

        [HttpGet("GetAllArchiveProjectsByDateSearch")]
        public IActionResult GetAllArchiveProjectsByDateSearch(string DateFrom, string DateTo)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var obj = new ProjectVM();
            obj.BranchId = _globalshared.BranchId_G;
            obj.Status = 1;
            obj.ProjectDate = DateFrom;
            obj.ProjectExpireDate = DateTo;
            var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", obj, _globalshared.UserId_G, 0,0, _globalshared.BranchId_G).Result.ToList();
            return Ok(AllPojects);
        }
        [HttpPost("FinishProject")]
        public IActionResult FinishProject(int ProjectId, int ReasonsId, string Reason, int Reasontype, string Reasontext, string Date, int TypeId, int whichClickDesc)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
                //var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            //string Con = System.Configuration.ConfigurationManager.ConnectionStrings["TameerProConn"].ConnectionString;
            var result = _projectservice.FinishProject(ProjectId, ReasonsId, Reason, Reasontype, Reasontext, Date,_globalshared.UserId_G, Con,  _globalshared.BranchId_G, url, file, TypeId, whichClickDesc);

            return Ok(result);
        }

        [HttpGet("GetPhasesDetails")]
        public IActionResult GetPhasesDetails(int ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var PhasesDetail = _projectservice.GetPhasesDetails(_globalshared.Lang_G, Con ?? "", ProjectId);
            return Ok(PhasesDetail);
        }

        [HttpGet("GenerateNextProjectNumber")]
        public IActionResult GenerateNextProjectNumber()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectservice.GenerateNextProjectNumber( _globalshared.BranchId_G) );
            }
        [HttpGet("GetProjectCode_S")]
        public IActionResult GetProjectCode_S()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectservice.GetProjectCode_S( _globalshared.BranchId_G) );
            }

        [HttpGet("GetProjectArchReC")]
        public int GetProjectArchReC()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Count = 0;
                var resultRe = _ProjectArchivesReService.GetProjectArchRe().Result;
                var resultSee = _ProjectArchivesSeeService.GetProjectArchSeeParm(0, 0).Result;

                if (resultRe.Count() > 0)
                {
                    foreach (var item in resultRe)
                    {
                        resultSee = _ProjectArchivesSeeService.GetProjectArchSeeParm(item.ProArchReID,_globalshared.UserId_G).Result;
                        if (resultSee.Count() > 0)
                        {
                            Count = 0;

                        }
                        else
                        {
                            Count = 1;

                        }
                    }

                }
                else
                {
                    Count = 0;

                }
                return Count;
            }
        [HttpGet("GetProjectArchReC_Phases")]
        public int GetProjectArchReC_Phases()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Count = 0;
                var resultRe = _ProjectArchivesReService.GetProjectArchRe_Phases().Result;
                var resultSee = _ProjectArchivesSeeService.GetProjectArchSeeParm_Phases(0, 0).Result;

                if (resultRe.Count() > 0)
                {
                    foreach (var item in resultRe)
                    {
                        resultSee = _ProjectArchivesSeeService.GetProjectArchSeeParm_Phases(item.ProArchReID,_globalshared.UserId_G).Result;
                        if (resultSee.Count() > 0)
                        {
                            Count = 0;

                        }
                        else
                        {
                            Count = 1;

                        }
                    }

                }
                else
                {
                    Count = 0;

                }
                return Count;
            }

        [HttpGet("GetProjectArchRe")]
        public IActionResult GetProjectArchRe()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var resultRe = _ProjectArchivesReService.GetProjectArchRe().Result;
                var resultReTemp = _ProjectArchivesReService.GetProjectArchRe().Result;
                var resultReTemp2 = _ProjectArchivesReService.GetProjectArchReParm(0).Result;

                var resultSee = _ProjectArchivesSeeService.GetProjectArchSeeParm(0, 0).Result;

                if (resultRe.Count() > 0)
                {
                    foreach (var item in resultRe)
                    {
                        resultSee = _ProjectArchivesSeeService.GetProjectArchSeeParm(item.ProArchReID,_globalshared.UserId_G).Result;
                        if (resultSee.Count() > 0)
                        {

                        }
                        else
                        {
                            resultReTemp = _ProjectArchivesReService.GetProjectArchReParm(item.ProArchReID).Result;
                            resultReTemp2 = resultReTemp2.Union(resultReTemp);
                        }
                    }


                }
                return Ok(resultReTemp2 );


            }
        [HttpGet("GetProjectArchRe_Phases")]
        public IActionResult GetProjectArchRe_Phases()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var resultRe = _ProjectArchivesReService.GetProjectArchRe_Phases().Result;
                var resultReTemp = _ProjectArchivesReService.GetProjectArchRe_Phases().Result;
                var resultReTemp2 = _ProjectArchivesReService.GetProjectArchReParm_Phases(0).Result;

                var resultSee = _ProjectArchivesSeeService.GetProjectArchSeeParm_Phases(0, 0).Result;

                if (resultRe.Count() > 0)
                {
                    foreach (var item in resultRe)
                    {
                        resultSee = _ProjectArchivesSeeService.GetProjectArchSeeParm_Phases(item.ProArchReID,_globalshared.UserId_G).Result;
                        if (resultSee.Count() > 0)
                        {

                        }
                        else
                        {
                            resultReTemp = _ProjectArchivesReService.GetProjectArchReParm_Phases(item.ProArchReID).Result;
                            resultReTemp2 = resultReTemp2.Union(resultReTemp);
                        }
                    }


                }
                return Ok(resultReTemp2 );


            }

        [HttpGet("InsertProjectArchSee")]
        public IActionResult InsertProjectArchSee(int ProArchReID)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectArchivesSeeService.InsertProjectArchSee(ProArchReID,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("InsertProjectArchSee_Phases")]
        public IActionResult InsertProjectArchSee_Phases(int ProArchReID)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectArchivesSeeService.InsertProjectArchSee_Phases(ProArchReID,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("UpdateStatusProject")]
        public IActionResult UpdateStatusProject(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
                //var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectservice.UpdateStatusProject(ProjectId,_globalshared.UserId_G,  _globalshared.BranchId_G, url, file);
            return Ok(result);
        }
        [HttpPost("ConvertManagerProjectsSome")]
        public IActionResult ConvertManagerProjectsSome(int ProjectId, int FromUserId, int ToUserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
                //var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectservice.ConvertManagerProjectsSome(ProjectId, FromUserId, ToUserId,_globalshared.UserId_G,  _globalshared.BranchId_G, url, file);
            return Ok(result);
        }
        [HttpPost("ConvertMoreManagerProjects")]
        public IActionResult ConvertMoreManagerProjects(List<int> ProjectId, int FromUserId, int ToUserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
                //var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectservice.ConvertMoreManagerProjects(ProjectId, FromUserId, ToUserId,_globalshared.UserId_G,  _globalshared.BranchId_G, url, file);
            return Ok(result);
        }
        [HttpGet("FillAllProjectSelectByNAccId")]
        public IActionResult FillAllProjectSelectByNAccId(int Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 2, null, 0, _globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.Id,
                Name = s.ProjectNo
            }); ;
            return Ok(result);
            }

        [HttpGet("FillAllProjectSelectByNAccIdWithout")]
        public IActionResult FillAllProjectSelectByNAccIdWithout(int Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 2, null, null, _globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.Id,
                Name = s.ProjectNo
            });
            return Ok(result);

        }
        [HttpGet("FillAllProjectSelectByAccId")]
        public IActionResult FillAllProjectSelectByAccId(int Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var CusId = _customerService.GetAllCustomers(_globalshared.Lang_G,  _globalshared.BranchId_G).Result
                    .Where(w => w.AccountId == Param)
                    .Select(s => s.CustomerId)
                    .FirstOrDefault();
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 2, CusId, 0, _globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.Id,
                Name = s.ProjectNo
            });
            return Ok(result);
        }

        [HttpGet("FillAllProjectSelectByAccIdWithout")]
        public IActionResult FillAllProjectSelectByAccIdWithout(int Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var CusId = _customerService.GetAllCustomers(_globalshared.Lang_G,  _globalshared.BranchId_G).Result
                   .Where(w => w.AccountId == Param)
                   .Select(s => s.CustomerId)
                   .FirstOrDefault();
            var result = _organizationsservice.FillSelect_Proc(Con, _globalshared.UserId_G, 2, CusId, null, _globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.Id,
                Name = s.ProjectNo
            });
            return Ok(result);

        }


        [HttpGet("FillAllProjectSelectByAllFawater")]
        public IActionResult FillAllProjectSelectByAllFawater(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Projects = _projectservice.GetAllProjByFawater(_globalshared.Lang_G,  _globalshared.BranchId_G).Result.Select(s => new
                {
                    Id = s.ProjectId,
                    Name = s.ProjectNo
                });
                return Ok(Projects );

            }
        [HttpGet("FillAllProjectSelectByAllMrdod")]
        public IActionResult FillAllProjectSelectByAllMrdod(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Projects = _projectservice.GetAllProjByMrdod(_globalshared.Lang_G,  _globalshared.BranchId_G).Result.Select(s => new
                {
                    Id = s.ProjectId,
                    Name = s.ProjectNo
                });
                return Ok(Projects );

            }
        [HttpGet("FillAllProjectSelectByAllNoti")]
        public IActionResult FillAllProjectSelectByAllNoti(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            if (Param == 0)
                {
                    ProjectVM Proj = new ProjectVM();
                    return Ok(Proj );
                }
                var Projects = _projectservice.GetAllProjByNoti(_globalshared.Lang_G,  _globalshared.BranchId_G, _globalshared.YearId_G).Result.Select(s => new
                {
                    Id = s.ProjectId,
                    Name = s.ProjectNo
                });
                return Ok(Projects );

            }
        [HttpGet("FillAllProjectSelectByAllMrdod_Pur")]
        public IActionResult FillAllProjectSelectByAllMrdod_Pur(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Projects = _projectservice.GetAllProjByMrdod_Pur(_globalshared.Lang_G,  _globalshared.BranchId_G).Result.Select(s => new
                {
                    Id = s.ProjectId,
                    Name = s.ProjectNo
                });
                return Ok(Projects );

            }
        [HttpGet("FillAllProjectSelectByAllMrdod_C")]
        public IActionResult FillAllProjectSelectByAllMrdod_C(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Projects = _projectservice.GetAllProjByMrdod_C(_globalshared.Lang_G,  _globalshared.BranchId_G, Param).Result.Select(s => new
                {
                    Id = s.ProjectId,
                    Name = s.ProjectNo
                });
                return Ok(Projects );

            }
        [HttpGet("FillAllProjectSelectByAllNoti_C")]
        public IActionResult FillAllProjectSelectByAllNoti_C(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

                var Projects = _projectservice.GetAllProjByNoti_C(_globalshared.Lang_G,  _globalshared.BranchId_G, Param, _globalshared.YearId_G).Result.Select(s => new
                {
                    Id = s.ProjectId,
                    Name = s.ProjectNo
                });
                return Ok(Projects );

            }
        [HttpGet("FillAllProjectSelectByAllMrdod_C_Pur")]
        public IActionResult FillAllProjectSelectByAllMrdod_C_Pur(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Projects = _projectservice.GetAllProjByMrdod_C_Pur(_globalshared.Lang_G,  _globalshared.BranchId_G, Param).Result.Select(s => new
                {
                    Id = s.ProjectId,
                    Name = s.ProjectNo
                });
                return Ok(Projects );

            }
        [HttpGet("FillAllCustomerSelectByAllFawater")]
        public IActionResult FillAllCustomerSelectByAllFawater(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Customers = _projectservice.GetAllCustByFawater(_globalshared.Lang_G,  _globalshared.BranchId_G).Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName
                });
                return Ok(Customers );

            }
        [HttpGet("FillAllCustomerSelectByAllMrdod")]
        public IActionResult FillAllCustomerSelectByAllMrdod(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Customers = _projectservice.GetAllCustByMrdod(_globalshared.Lang_G,  _globalshared.BranchId_G).Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName
                });
                return Ok(Customers );

            }
        [HttpGet("FillAllCustomerSelectByAllNoti")]
        public IActionResult FillAllCustomerSelectByAllNoti(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                var Customers = _projectservice.GetAllCustByNoti(_globalshared.Lang_G,  _globalshared.BranchId_G, _globalshared.YearId_G).Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName
                });
                return Ok(Customers );

            }
        [HttpGet("FillAllCustomerSelectByAllReVoucher")]
        public IActionResult FillAllCustomerSelectByAllReVoucher(int? Param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Customers = _projectservice.GetAllCustByReVoucher(_globalshared.Lang_G,  _globalshared.BranchId_G).Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName
                });
                return Ok(Customers );

            }
        [HttpGet("SavePrivProList")]
        public IActionResult SavePrivProList([FromBody]List<ProUserPrivileges> Privs)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _proUserPrivilegesService.SavePrivProList(Privs,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SavePriv2")]
        public IActionResult SavePriv2(ProUserPrivileges Privs)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
                //var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _proUserPrivilegesService.SavePriv2(Privs,_globalshared.UserId_G,  _globalshared.BranchId_G, url, file);
            return Ok(result);

        }
        [HttpPost("UpdateProject")]
        public IActionResult UpdateProject(Project project)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.UpdateProject(project,_globalshared.UserId_G,  _globalshared.BranchId_G);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var projectid = Convert.ToInt32(result.ReturnedStr);
                var resultMain = _projectPhasesTasksservice.SavefourMainPhases_P(projectid, _globalshared.UserId_G, _globalshared.BranchId_G);
            }

            //if (result.ReturnedStr != "-1")
            //    {
            //        ProjectPhasesTasks _ProjectPhasesTasks = new ProjectPhasesTasks();
            //        ProjectPhasesTasks _ProjectPhasesTasks2 = new ProjectPhasesTasks();
            //        _ProjectPhasesTasks.ProjectId = Convert.ToInt32(result.ReturnedStr);
            //        _ProjectPhasesTasks2.ProjectId = Convert.ToInt32(result.ReturnedStr);
            //        var resultMain = _projectPhasesTasksservice.SaveMainPhases_P(_ProjectPhasesTasks,_globalshared.UserId_G,  _globalshared.BranchId_G);
            //        var resultSub = _projectPhasesTasksservice.SaveSubPhases_P(_ProjectPhasesTasks2,_globalshared.UserId_G,  _globalshared.BranchId_G, Convert.ToInt32(resultMain.ReturnedParm));
            //    }
            return Ok(result);
        }

        [HttpPost("UpdateProjectEnddate")]
        public IActionResult UpdateProjectEnddate(int projectid, string ProjectEnddate)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.UpdateProjectEnddate(projectid, ProjectEnddate,_globalshared.UserId_G,  _globalshared.BranchId_G);

                //if (result.ReturnedStr != "-1")
                //{
                //    ProjectPhasesTasks _ProjectPhasesTasks = new ProjectPhasesTasks();
                //    ProjectPhasesTasks _ProjectPhasesTasks2 = new ProjectPhasesTasks();
                //    _ProjectPhasesTasks.ProjectId = Convert.ToInt32(result.ReturnedStr);
                //    _ProjectPhasesTasks2.ProjectId = Convert.ToInt32(result.ReturnedStr);
                //    var resultMain = _projectPhasesTasksservice.SaveMainPhases_P(_ProjectPhasesTasks,_globalshared.UserId_G,  _globalshared.BranchId_G);
                //    var resultSub = _projectPhasesTasksservice.SaveSubPhases_P(_ProjectPhasesTasks2,_globalshared.UserId_G,  _globalshared.BranchId_G, Convert.ToInt32(resultMain.ReturnedParm));
                //}

            return Ok(result);
        }





        //public IActionResult UpdateImportant(int ProjectId,int Important)
        //{
        //    var result = _projectservice.UpdateImportant(ProjectId, Important,_globalshared.UserId_G,  _globalshared.BranchId_G);
        //    return Ok(new { result.Result, result.ReasonPhrase });
        //}

        [HttpPost("ChangeImportant")]
        public IActionResult ChangeImportant(ImportantProject impproject)       
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _importantProjectService.ChangeImportant(impproject,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("ChangeFlag")]
        public IActionResult ChangeFlag(ImportantProject impproject)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _importantProjectService.ChangeFlag(impproject,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetAllPriv")]
        public IActionResult GetAllPriv(string? SearchText, string Projectno)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Privs = _proUserPrivilegesService.GetAllPriv(SearchText??"", Projectno, 0).Result;

                return Ok(Privs );
            }
        [HttpGet("GetAllProjByCustomerId")]
        public IActionResult GetAllProjByCustomerId(int customerId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var listProjectId = _projectservice.GetAllProjByCustomerId(customerId).Result;

                return Ok(listProjectId );
            }
        [HttpGet("GetAllProjByCustomerIdWithout")]
        public IActionResult GetAllProjByCustomerIdWithout(int customerId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var listProjectId = _projectservice.GetAllProjByCustomerIdWithout(customerId).Result;

                return Ok(listProjectId );
            }
        [HttpGet("GetAllProjByCustomerIdHaveTasks")]
        public IActionResult GetAllProjByCustomerIdHaveTasks(int customerId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var listProjectId = _projectservice.GetAllProjByCustomerIdHaveTasks(customerId).Result;

                return Ok(listProjectId );
            }

        [HttpGet("GetAllProjByCustomerIdandbranchHaveTasks")]
        public IActionResult GetAllProjByCustomerIdandbranchHaveTasks(int customerId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var listProjectId = _projectservice.GetAllProjByCustomerIdandbranchHaveTasks(customerId,_globalshared.BranchId_G).Result;

            return Ok(listProjectId);
        }

        [HttpPost("DeletePriv")]
        public IActionResult DeletePriv(int PrivID)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _proUserPrivilegesService.DeletePriv(PrivID,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetAllPrivUser")]
        public IActionResult GetAllPrivUser(int Projectid)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Privs = _proUserPrivilegesService.GetAllPrivUser(_globalshared.UserId_G, Projectid).Result;

                return Ok(Privs );
            }

        //[HttpPost]
        //public JsonResult PDFDownloadProjectCustomer(string branch, List<ProjectVM> listDataed)
        //{
        //HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        //byte[] pdfByte = { 0 };

        //    int orgId = _BranchesService.GetOrganizationId( _globalshared.BranchId_G);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);

        //    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, " مشاريع المستخدم", objOrganization.LogoUrl, branch };
        //    string[] columnDoneTasksReportHeader = { "رقم المشروع", "نوع المشروع", "نوع المشروع الفرعى", "نسبة الانجاز", "مدة المشروع بالأيام", "وصف المشروع" };
        //    string[] columnHeader = { "ProjectNo", "ProjectTypesName", "ProjectSubTypeName", "BuildingPercent", "TimeStr", "ProjectDescription" };
        //    //List<rptGetEmpDoneTasksVM> DataDoneTasks = _ProjectPhasesTasksService.GetDoneTasksDGV(FromDate, ToDate,_globalshared.UserId_G, Con).ToList();
        //    // var myList = JsonConvert.DeserializeObject<List<ProjectVM>>(listedData);
        //    DataTable listArchive = ToDataTable(listDataed);

        //    pdfByte = pdfClass.DataProjectCustomer(infoDoneTasksReport, columnDoneTasksReportHeader, listArchive, columnHeader);

        //    string existTemp = HttpContext.Server.MapPath(@"~\TempFiles\");

        //    if (!Directory.Exists(existTemp))
        //    {
        //        Directory.CreateDirectory(existTemp);
        //    }
        //    //File  
        //    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
        //    string FilePath = HttpContext.Server.MapPath(@"~\TempFiles\") + FileName;

        //    //create and set PdfReader  
        //    System.IO.File.WriteAllBytes(FilePath, pdfByte);
        //    //return file 
        //    string FilePathReturn = FileName;

        //    return Ok(new { fileName = FilePathReturn, errorMessage = "" }); ;

        //}

        //[HttpGet]
        //public FileResult DownloadPDFProjectCustomer(string fileName)
        //{
        //    string fullPath = Path.Combine(Server.MapPath("~/TempFiles"), fileName);
        //    byte[] FileBytes = System.IO.File.ReadAllBytes(fullPath);
        //    //return the file for download, this is an Excel 
        //    //so I set the file content type to "application/vnd.ms-excel"
        //    return File(FileBytes, "application/pdf");

        //}
        [HttpGet("ToDataTable")]
        public DataTable ToDataTable<T>(List<T> items)
            {
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
        ////مشاريع المستخدمين
        //public IActionResult PrintUserProjects(int_globalshared.UserId_G, string UserName, string DateFrom, string DateTo)
        //{
        //    int orgId = _branchesService.GetOrganizationId( _globalshared.BranchId_G);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    List<ProjectVM> AllPojects =_globalshared.UserId_G == 0 ? _projectservice.GetUserProjectsReportW2( _globalshared.BranchId_G, DateFrom, DateTo).ToList() : _projectservice.GetUserProjectsReport(UserId,  _globalshared.BranchId_G, DateFrom, DateTo).ToList();

        //    ReportPDF = ProjectsReports.PrintUserProjects(AllPojects, UserName, infoDoneTasksReport, DateFrom, DateTo);
        //    string existTemp = HttpContext.Server.MapPath(@"~\TempFiles\");

        //    if (!Directory.Exists(existTemp))
        //    {
        //        Directory.CreateDirectory(existTemp);
        //    }
        //    //File  
        //    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
        //    string FilePath = HttpContext.Server.MapPath(@"~\TempFiles\") + FileName;

        //    //create and set PdfReader  
        //    System.IO.File.WriteAllBytes(FilePath, ReportPDF);
        //    //return file 
        //    string FilePathReturn = @"TempFiles/" + FileName;
        //    return Content(FilePathReturn);
        //}


        ////جميع المهام
        //public IActionResult PrintAllEmpTasksRpt(int_globalshared.UserId_G, int? status, string UserName, string DateFrom, string DateTo)
        //{
        //    int orgId = _branchesService.GetOrganizationId( _globalshared.BranchId_G);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
        //    List<ProjectPhasesTasksVM> AllPojects = new List<ProjectPhasesTasksVM>();
        //    List<ProjectPhasesTasksVM> AllPojects2 = new List<ProjectPhasesTasksVM>();
        //    if (status == null)
        //    {
        //        AllPojects =_globalshared.UserId_G == 0 ? _projectPhasesTasksservice.GetAllTasksByProjectIdW2(DateFrom, DateTo,  _globalshared.BranchId_G).ToList() : _projectPhasesTasksservice.GetAllProjectPhasesTasksU2(UserId,  _globalshared.BranchId_G, _globalshared.Lang_G, DateFrom, DateTo).ToList();
        //    }

        //    else if (status == 8)
        //    {
        //        if (DateFrom == "" && DateTo == "")
        //        {
        //            AllPojects = _projectPhasesTasksservice.GetAllLateProjectPhasesTasksbyUserId2(DateTo,  _globalshared.BranchId_G,_globalshared.UserId_G, _globalshared.Lang_G).ToList();


        //        }
        //        else
        //        {
        //            AllPojects = _projectPhasesTasksservice.GetLateTasksByUserIdrptsearch(UserId, status, _globalshared.Lang_G, DateFrom, DateTo,  _globalshared.BranchId_G).ToList();


        //        }

        //    }
        //    else
        //    {
        //        AllPojects = _projectPhasesTasksservice.GetAllProjectPhasesTasksbystatus(UserId,  _globalshared.BranchId_G, status, _globalshared.Lang_G, DateFrom, DateTo).ToList();
        //    }
        //    var wo2 = _workOrdersService.GetWorkOrderReport_print(UserId, status ?? 0,  _globalshared.BranchId_G, _globalshared.Lang_G, DateFrom, DateTo);
        //    AllPojects2 = AllPojects.Union(wo2).ToList();
        //    ReportPDF = ProjectsReports.PrintAllEmpTasksRpt(AllPojects2, UserName, status, infoDoneTasksReport, DateFrom, DateTo);
        //    string existTemp = HttpContext.Server.MapPath(@"~\TempFiles\");

        //    if (!Directory.Exists(existTemp))
        //    {
        //        Directory.CreateDirectory(existTemp);
        //    }
        //    //File  
        //    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
        //    string FilePath = HttpContext.Server.MapPath(@"~\TempFiles\") + FileName;

        //    //create and set PdfReader  
        //    System.IO.File.WriteAllBytes(FilePath, ReportPDF);
        //    //return file 
        //    string FilePathReturn = @"TempFiles/" + FileName;
        //    return Content(FilePathReturn);
        //}
        ////مهام المشروع
        //public IActionResult PrintProjectsTasksReport(int ProjectId, string ProjectName, string DateFrom, string DateTo)
        //{
        //    int orgId = _branchesService.GetOrganizationId( _globalshared.BranchId_G);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
        //    //List<ProjectPhasesTasksVM> AllPojects = ProjectId == 0 ? _projectPhasesTasksservice.GetAllProjectPhasesTasks( "", _globalshared.BranchId_G, _globalshared.Lang_G).ToList(): _projectPhasesTasksservice.GetAllTasksPhasesByProjectId(ProjectId,  _globalshared.BranchId_G).ToList();
        //    List<ProjectPhasesTasksVM> AllPojects = ProjectId == 0 ? _projectPhasesTasksservice.GetAllProjectPhasesTasks_WithB("",  _globalshared.BranchId_G, _globalshared.Lang_G).ToList() : _projectPhasesTasksservice.GetAllTasksPhasesByProjectId(ProjectId,  _globalshared.BranchId_G).ToList();
        //    if (!(DateFrom == "" || DateTo == ""))
        //    {
        //        AllPojects = AllPojects.ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
        //    }
        //    var wo = _workOrdersService.GetWorkOrderReport_ptoject(ProjectId, _globalshared.Lang_G, "", "",  _globalshared.BranchId_G).ToList();
        //    AllPojects.Union(wo);
        //    //ReportPDF = ProjectsReports.PrintProjectsTasksReport(AllPojects, ProjectName, infoDoneTasksReport);
        //    ReportPDF = ProjectsReports.PrintProjectsTasksReport2(AllPojects, ProjectName, infoDoneTasksReport, "", "");

        //    string existTemp = HttpContext.Server.MapPath(@"~\TempFiles\");

        //    if (!Directory.Exists(existTemp))
        //    {
        //        Directory.CreateDirectory(existTemp);
        //    }
        //    //File  
        //    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
        //    string FilePath = HttpContext.Server.MapPath(@"~\TempFiles\") + FileName;

        //    //create and set PdfReader  
        //    System.IO.File.WriteAllBytes(FilePath, ReportPDF);
        //    //return file 
        //    string FilePathReturn = @"TempFiles/" + FileName;
        //    return Content(FilePathReturn);
        //}
        ////تكلفة المشروع
        //public IActionResult PrintCostProjects(int? ProjectTypeId, int? SubProjectTypeId, int? CustomerId, string ProjectNo, string DateFrom, string DateTo)
        //{
        //    int orgId = _branchesService.GetOrganizationId( _globalshared.BranchId_G);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
        //    //var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G);
        //    //var someProject = _projectservice.GetProjectsSearch(ProjectsSearch, 0, Con, _globalshared.Lang_G);
        //    //foreach (var userBranch in userBranchs)
        //    //{
        //    //    var AllPojects = _projectservice.GetProjectsSearch(ProjectsSearch, userBranch. _globalshared.BranchId_G, Con, _globalshared.Lang_G).ToList();
        //    //    var Projects = someProject.Union(AllPojects);
        //    //    someProject = Projects.ToList();
        //    //}
        //    ProjectVM ProjectsSearch = new ProjectVM();
        //    ProjectsSearch.ProjectTypeId = ProjectTypeId;
        //    ProjectsSearch.SubProjectTypeId = SubProjectTypeId;
        //    ProjectsSearch.CustomerId = CustomerId;
        //    if (ProjectNo == "")
        //    {
        //        ProjectsSearch.ProjectNo = null;
        //    }
        //    else
        //    {
        //        ProjectsSearch.ProjectNo = ProjectNo;

        //    }
        //    ProjectsSearch.DateFrom_Search = DateFrom;
        //    ProjectsSearch.DateTo_Search = DateTo;

        //    var someProject = _projectservice.GetProjectsSearch(ProjectsSearch,  _globalshared.BranchId_G, Con, _globalshared.Lang_G).ToList();

        //    if (!(ProjectsSearch.DateFrom_Search == "" || ProjectsSearch.DateTo_Search == "" || ProjectsSearch.DateFrom_Search == null || ProjectsSearch.DateTo_Search == null))
        //    {
        //        someProject = someProject.ToList().Where(s => DateTime.ParseExact(s.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(ProjectsSearch.DateFrom_Search, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ProjectsSearch.DateTo_Search, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
        //    }


        //    List<ProjectVM> res = someProject.ToList();

        //    ReportPDF = ProjectsReports.PrintCostProjects(res, (ProjectsSearch.ProjectNo == null ? "" : res[0].CustomerName), infoDoneTasksReport);
        //    string existTemp = HttpContext.Server.MapPath(@"~\TempFiles\");

        //    if (!Directory.Exists(existTemp))
        //    {
        //        Directory.CreateDirectory(existTemp);
        //    }
        //    //File  
        //    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
        //    string FilePath = HttpContext.Server.MapPath(@"~\TempFiles\") + FileName;

        //    //create and set PdfReader  
        //    System.IO.File.WriteAllBytes(FilePath, ReportPDF);
        //    //return file 
        //    string FilePathReturn = @"TempFiles/" + FileName;
        //    return Content(FilePathReturn);
        //}
        [HttpGet("GetAllSuperContractor")]
        public IActionResult GetAllSuperContractor(string? SearchText)
            {
                return Ok(_Pro_SuperContractorService.GetAllSuperContractor(SearchText ?? "") );
            }
        [HttpGet("GetContractorData")]
        public IActionResult GetContractorData(int? ContractorId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Pro_SuperContractorService.GetContractorData(ContractorId,_globalshared.UserId_G,  _globalshared.BranchId_G) );
            }
        [HttpPost("SaveSuperContractor")]
        public IActionResult SaveSuperContractor(Pro_SuperContractor SuperContractor)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_SuperContractorService.SaveSuperContractor(SuperContractor,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteSuperContractor")]
        public IActionResult DeleteSuperContractor(int ContractorId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_SuperContractorService.DeleteSuperContractor(ContractorId,_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillContractorsSelect")]
        public IActionResult FillContractorsSelect(string? SearchText)
        {
        return Ok(_Pro_SuperContractorService.GetAllSuperContractor(SearchText ?? "").Result.Select(s => new {
            Id = s.ContractorId,
            Name = s.NameAr,
            NameEn = s.NameEn,
            Email = s.Email,
            CommercialRegister = s.CommercialRegister,
            PhoneNo = s.PhoneNo,
        }) );
        }
        [HttpGet("GetTypeOfProjct")]
        public IActionResult GetTypeOfProjct(int projectId)
            {
                return Ok(_projectservice.GetTypeOfProjct(projectId) );
            }
        [HttpPost("SaveFollowProj")]
        public IActionResult SaveFollowProj(ListOfFollowProj _followProj)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _followProjService.SaveFollowProj(_followProj.FollowProj ?? new List<FollowProj>(),_globalshared.UserId_G,  _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetAllFollowProj")]
        public IActionResult GetAllFollowProj(int ProjectId)
            {
                var AllPojects = _followProjService.GetAllFollowProjByProId(ProjectId).Result.ToList();
                return Ok(AllPojects );
            }
        [HttpGet("GetMaxCosEManagerNameTOP1")]
        public IActionResult GetMaxCosEManagerNameTOP1()
            {

                var SelectStetment = "select TOP(1) u.FullName,u.ImgUrl, sum(acc.InvoiceValue) as CostE  from Acc_Invoices acc join Pro_Projects pro on acc.ProjectId=pro.ProjectId join Sys_Users u on u.UserId=pro.MangerId where acc.IsDeleted = 0 and pro.IsDeleted=0 and u.IsDeleted=0 and acc.ProjectId = Pro.ProjectId and Type = 2 and Rad != 1 group by u.FullName,u.ImgUrl order by CostE desc";
                var MaxCostEManagerName = _projectservice.GetMaxCosEManagerName_StatmentTOP1(Con, SelectStetment);
                return Ok(MaxCostEManagerName.FirstOrDefault() );

            }

        [HttpGet("GetMaxCosECustomerNameTOP1")]
        public IActionResult GetMaxCosECustomerNameTOP1()
            {

                var SelectStetment = "select TOP(1) p.NameAr,p.LogoUrl, sum(acc.InvoiceValue) as CostE  from Acc_Invoices acc join Pro_Projects pro on acc.ProjectId=pro.ProjectId join Pro_Customers p on p.CustomerId=pro.CustomerId where acc.IsDeleted = 0 and pro.IsDeleted=0 and p.IsDeleted=0 and acc.ProjectId = Pro.ProjectId and Type = 2 and Rad != 1 group by p.NameAr,p.LogoUrl order by CostE desc";
                var MaxCostECustomerName = _projectservice.GetMaxCosECustomerName_StatmentTOP1(Con, SelectStetment);
                return Ok(MaxCostECustomerName.FirstOrDefault() );

            }
        [HttpGet("GetMaxCosEManagerTOP10")]
        public IActionResult GetMaxCosEManagerTOP10()
            {

                var SelectStetment = "select TOP(5) Pro.ProjectNo,u.FullName,ISNULL((select Sum(InvoiceValue) from Acc_Invoices acc where IsDeleted = 0 and acc.ProjectId = Pro.ProjectId and Type = 2 and Rad != 1),0) as CostES from Pro_Projects Pro join Sys_Users u on Pro.MangerId = u.UserId where Pro.IsDeleted= 0 order by CostES desc";
                var SelectStetment2 = "select TOP(5) Pro.ProjectNo,u.FullName,ISNULL((select Sum(InvoiceValue) from Acc_Invoices acc where IsDeleted = 0 and acc.ProjectId = Pro.ProjectId and Type = 5),0) as CostES from Pro_Projects Pro join Sys_Users u on Pro.MangerId = u.UserId where Pro.IsDeleted= 0 order by CostES desc";

                var MaxCostE = _projectservice.GetMaxCosEManagerName_Statment(Con, SelectStetment);
                var MaxCostS = _projectservice.GetMaxCosEManagerName_Statment(Con, SelectStetment2);

                var MaxCostES = MaxCostE.Union(MaxCostS);

                return Ok(MaxCostES );

            }


        [HttpGet("FillProjectSelectNewTask")]
        public IActionResult FillProjectSelectNewTask()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.GetprojectNewTasks(_globalshared.UserId_G, _globalshared.BranchId_G,_globalshared.Lang_G).Result.Select(x=> new {
                Id=x.ProjectId,
                Name=x.ProjectNo +"-" + x.CustomerName,
            });
            return Ok(result);
        }

        [HttpGet("FillCustomerSelectNewTask")]
        public IActionResult FillCustomerSelectNewTask()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.GetprojectNewTasks(_globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G).Result.Select(x => new {
                Id = x.CustomerId,
                Name =  x.CustomerName,
            }).DistinctBy(x=>x.Id);
            return Ok(result);
        }



        [HttpGet("FillProjectSelectLateTask")]
        public IActionResult FillProjectSelectLateTask()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.GetprojectLateTasks(_globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G).Result.Select(x => new {
                Id = x.ProjectId,
                Name = x.ProjectNo + "-" + x.CustomerName,
            });
            return Ok(result);
        }

        [HttpGet("FillCustomerSelectLateTask")]
        public IActionResult FillCustomerSelectLateTask()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.GetprojectLateTasks(_globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G).Result.Select(x => new {
                Id = x.CustomerId,
                Name = x.CustomerName,
            }).DistinctBy(x => x.Id);
            return Ok(result);
        }
        //Work Order

        [HttpGet("FillProjectSelectWorkOrder")]
        public IActionResult FillProjectSelectWorkOrder()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.GetprojectNewWorkOrder(_globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G).Result.Select(x => new {
                Id = x.ProjectId,
                Name = x.ProjectNo + "-" + x.CustomerName,
            });
            return Ok(result);
        }

        [HttpGet("FillCustomerSelectWorkOrder")]
        public IActionResult FillCustomerSelectWorkOrder()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.GetprojectNewWorkOrder(_globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G).Result.Select(x => new {
                Id = x.CustomerId,
                Name = x.CustomerName,
            }).DistinctBy(x => x.Id);
            return Ok(result);
        }

        [HttpGet("FillProjectSelecLatetWorkOrder")]
        public IActionResult FillProjectSelecLatetWorkOrder()
        {
            var enddt = DateTime.Now.ToString("yyyy-MM-dd");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.GetprojectNewWorkOrder(_globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, enddt).Result;
            if (result != null && result.Count() > 0)
            {
              var  res = result.Select(x => new
                {
                    Id = x.ProjectId,
                    Name = x.ProjectNo + "-" + x.CustomerName,
                });
                return Ok(res);
            }
            else
            {
                return Ok(result); 
            }
        }

        [HttpGet("FillCustomerSelectLateWorkOrder")]
        public IActionResult FillCustomerSelectLateWorkOrder()
        {
            var enddt = DateTime.Now.ToString("yyyy-MM-dd");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectservice.GetprojectNewWorkOrder(_globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, enddt).Result;
                if (result != null && result.Count() > 0)
                {
                var res = result.Select(x => new
                {
                    Id = x.CustomerId,
                    Name =  x.CustomerName,
                }).DistinctBy(x=>x.Id);
                return Ok(res);
                }
                else
                {
                    return Ok(result);
                }
        }

        [HttpGet("GetTaskData")]
        public IActionResult GetTaskData(int ProjectId)
        {
            var result = _projectservice.GetTaskData(ProjectId,Con);
            return Ok(result);
        }

        [HttpGet("GetTaskData_phases")]
        public IActionResult GetTaskData_phases(int ProjectId)
        {
            var result = _projectservice.GetTaskData_phases(ProjectId, Con);
            return Ok(result);
        }

        [HttpPost("SaveProjectLocation")]
        public IActionResult SaveProjectLocation([FromBody]ProjectLocationVM projectLocation)
        {
            var result = _projectservice.SaveProjectLocation(projectLocation);
            return Ok(result);
        }

        [HttpPost("DeleteProjectLocation")]
        public IActionResult DeleteProjectLocation(int ProjectId)
        {
            var result = _projectservice.deleteProjectLocation(ProjectId);
            return Ok(result);
        }


        [HttpGet("GetProjectLocation")]
        public IActionResult GetProjectLocation(int ProjectId)
        {
            var result = _projectservice.GetProjectLocation(ProjectId);
            return Ok(result);
        }


        //public IActionResult PrintProjDist(int ProjectId, decimal ContractValue, decimal CostE, decimal CostS, string ProjectNo, string TimeStr)
        //{HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        //    int orgId = _branchesService.GetOrganizationId( _globalshared.BranchId_G);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    List<FollowProjVM> followproj = _followProjService.GetAllFollowProjByProId(ProjectId).ToList();

        //    ReportPDF = ProjectsReports.PrintProjDist(followproj, ContractValue, CostE, CostS, ProjectNo, TimeStr, infoDoneTasksReport);

        //    //List<ProjectVM> AllPojects =_globalshared.UserId_G == 0 ? _projectservice.GetUserProjectsReportW( _globalshared.BranchId_G, "").ToList() : _projectservice.GetUserProjectsReport(UserId,  _globalshared.BranchId_G, "").ToList();

        //    //ReportPDF = ProjectsReports.PrintUserProjects(AllPojects, "", infoDoneTasksReport);
        //    string existTemp = HttpContext.Server.MapPath(@"~\TempFiles\");

        //    if (!Directory.Exists(existTemp))
        //    {
        //        Directory.CreateDirectory(existTemp);
        //    }
        //    //File  
        //    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
        //    string FilePath = HttpContext.Server.MapPath(@"~\TempFiles\") + FileName;

        //    //create and set PdfReader  
        //    System.IO.File.WriteAllBytes(FilePath, ReportPDF);
        //    //return file 
        //    string FilePathReturn = @"TempFiles/" + FileName;
        //    return Content(FilePathReturn);
        //}

        public class ListOfFollowProj
        {
            public List<FollowProj>? FollowProj { get; set; }
        }


    }
}
