using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System.IO;
//using System.Web.Script.Serialization;
using TaamerProject.API.Helper;
using TaamerProject.Service.Interfaces;
using TaamerProject.Models;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Mail;
using TaamerProject.Models.Common;
using static TaamerProject.API.Controllers.ProjectSettingsController;
using TaamerProject.Service.Services;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using TaamerProject.API.pdfHandler;
using TaamerProject.Models.DBContext;
using Twilio.TwiML.Messaging;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProjectPhasesTasksController : ControllerBase
    {
            private IProjectPhasesTasksService _projectPhasesTasksservice;
            private IProjectService _projectservice;
            private IBranchesService _branchesService;
            private IProUserPrivilegesService _proUserPrivilegesService;
            private string Con;
            private readonly IUsersService _usersservice;
            private byte[] ReportPDF;
            private IOrganizationsService _organizationsservice;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ICustomerSMSService _sMSService;

        private IWorkOrdersService _workOrdersService;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private ISettingsService _settingsservice;
        public ProjectPhasesTasksController(IProjectPhasesTasksService projectPhasesTasksService, IProjectService projectService, IBranchesService branchesService,
               IProUserPrivilegesService proUserPrivilegesService, IUsersService usersService, IOrganizationsService organizationsService, IWorkOrdersService workOrdersService
            , IConfiguration _configuration, ICustomerSMSService sMSService, TaamerProjectContext dataContext, IWebHostEnvironment webHostEnvironment, ISettingsService settingsService)
            {
                this._projectPhasesTasksservice = projectPhasesTasksService;
                this._branchesService = branchesService;
                this._projectservice = projectService;
                this._proUserPrivilegesService = proUserPrivilegesService;
                this._usersservice = usersService;
                this._organizationsservice = organizationsService;
                this._workOrdersService = workOrdersService;
            _sMSService = sMSService;

            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
                HttpContext httpContext = HttpContext;
            _TaamerProContext = dataContext;

            _globalshared = new GlobalShared(httpContext);
                _hostingEnvironment = webHostEnvironment;
            _settingsservice = settingsService;
        }
        //public IActionResult Index()
        //{
        //    return Ok();
        //}
        //public IActionResult TaskDiagram()
        //{
        //    return Ok();
        //}
        [HttpGet("ProjectPhasesTasks")]
        public IActionResult ProjectPhasesTasks(int ProjectId, int PageNo)
            {
           
                //ViewBag.Flag = ProjectId;
                //ViewBag.Flag_PageNo = PageNo;
                //if (ProjectId != -1)
                //{
                //    var AllProject = _projectservice.GetProjectByIdSome(_globalshared.Lang_G, ProjectId);
                //    ViewBag.ProjectId = AllProject.ProjectId;
                //    ViewBag.ProjectNo = AllProject.ProjectNo;
                //    ViewBag.CustomerId = AllProject.CustomerId;
                //    ViewBag.ActiveMainPhaseId = AllProject.ActiveMainPhaseId;
                //    ViewBag.ActiveSubPhaseId = AllProject.ActiveSubPhaseId;
                //    ViewBag.ProjectDate = AllProject.ProjectDate;
                //    ViewBag.TimeStr = AllProject.TimeStr;
                //    ViewBag.ProjectExpireDate = AllProject.ProjectExpireDate;


                //}

                return Ok();
            }

        //public IActionResult AddNewTask()
        //{
        //    return Ok();
        //}

        //public IActionResult ProjectTasks_N()
        //{
        //    return Ok();
        //}
        [HttpGet("EditProjectPhasesTasks")]
        public IActionResult EditProjectPhasesTasks(int ProjectId)
            {

                //ViewBag.Flag = ProjectId;
                //if (ProjectId != -1)
                //{
                //    ViewBag.ProjectId = ProjectId;
                //}
                return Ok();
            }

        //public IActionResult GetTasksByUserCount()
        //{
        //    var Counts = new
        //    {
        //        TasksByUserCount = _projectPhasesTasksservice.GetTasksByUserId(_globalshared.UserId_G, 0, _globalshared.BranchId_G).Count(),
        //    };
        //    return Ok(Counts );
        //}
        [HttpGet("GetProjectPhasesTasksbygoalandproject")]
        public IActionResult GetProjectPhasesTasksbygoalandproject(int ProjectId, int ProjectGoal)
            {
                var AllTasks = _projectPhasesTasksservice.GetProjectPhasesTasksbygoalandproject(ProjectId, ProjectGoal, _globalshared.BranchId_G, _globalshared.Lang_G);

                return Ok(AllTasks );
            }
        [HttpGet("GetAllProjectPhasesTasks")]
        public IActionResult GetAllProjectPhasesTasks(string SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someTask = _projectPhasesTasksservice.GetAllProjectPhasesTasks(SearchText ?? "", 0, _globalshared.Lang_G).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasks(SearchText ?? "", userBranch.BranchId, _globalshared.Lang_G).Result.ToList();
                    var Tasks = someTask.Union(AllTasks);
                    someTask = Tasks.ToList();
                }

                return Ok(someTask );
            }
        [HttpGet("GetMaxUserProjectPhasesTasks")]
        public IActionResult GetMaxUserProjectPhasesTasks()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranches(_globalshared.Lang_G);
                var someTask = _projectPhasesTasksservice.GetAllProjectPhasesTasks("", 0, _globalshared.Lang_G).Result;
                MaxProjevtAndTasksVM mxpro = new MaxProjevtAndTasksVM();
                var alltasks = someTask.Where(x => x.UserId != 0 & x.Status != 4);
                if (alltasks.Count() > 0)
                {


                    var max = alltasks.GroupBy(n => n.UserId).OrderByDescending(g => g.Count()).First();
                    mxpro.Count = max.Count();
                    var maxpro = max.ToList().FirstOrDefault();
                    mxpro.ManagerId = maxpro.UserId;

                    mxpro.ManagerName = maxpro.ProjectMangerName;
                    mxpro.ImgUrl = _usersservice.GetUserById((int)maxpro.UserId, _globalshared.Lang_G).Result.ImgUrl;
                    //mxpro.ImgUrl = _usersservice.GetUserById(1, _globalshared.Lang_G).ImgUrl;
                }
                else
                {
                    mxpro.Count = 0;
                    mxpro.ImgUrl = "/distnew/images/userprofile.png";

                }
                return Ok(mxpro );
            }
        [HttpGet("GetAllProjectPhasesTasksS")]
        public IActionResult GetAllProjectPhasesTasksS(int? UserId, int? status, string? DateFrom, string? DateTo)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //var AccBranchesList = BranchesList ?? new List<int>();
            var AccBranchesList = new List<int>();
            if (AccBranchesList.Count() == 0)
            {
                AccBranchesList.Add(_globalshared.BranchId_G);
            }
            if (status == 8)
                {
                    
                if ((DateFrom == "" && DateTo == "") || (DateFrom == null && DateTo == null))
                {
                    var atasks = _projectPhasesTasksservice.GetAllLateProjectPhasesTasksbyUserId2(DateTo, _globalshared.BranchId_G, _globalshared.UserId_G, _globalshared.Lang_G, AccBranchesList);

                    return Ok(atasks );
                }
                else
                {
                    var AllTasks1 = _projectPhasesTasksservice.GetLateTasksByUserIdrptsearch(_globalshared.UserId_G, status, _globalshared.Lang_G, DateFrom, DateTo, _globalshared.BranchId_G, AccBranchesList).Result.ToList();

                    return Ok(AllTasks1 );
                }
            
            }
            var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasksS(_globalshared.UserId_G, _globalshared.BranchId_G, status, _globalshared.Lang_G, DateFrom, DateTo, AccBranchesList).Result.ToList();

            return Ok(AllTasks );
            }
        [HttpGet("GetAllProjectPhasesTasksS_whithworkorder")]
        public IActionResult GetAllProjectPhasesTasksS_whithworkorder(int? UserId, int? status, string? DateFrom, string? DateTo, int? BranchId, [FromQuery] List<int> BranchesList)       
        {
            if (UserId == 0)UserId = null;
            if (status == 0) status = null;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllUserBranch = 0;
            if (BranchId == -1) AllUserBranch = 0;
            else AllUserBranch = _globalshared.BranchId_G;

            var AccBranchesList = BranchesList ?? new List<int>();
            if (AccBranchesList.Count() == 0)
            {
                AccBranchesList.Add(_globalshared.BranchId_G);
            }

            if (status == 8)
                {
                    if (DateFrom == "" && DateTo == "")
                    {
                        var atasks = _projectPhasesTasksservice.GetAllLateProjectPhasesTasksbyUserId2(DateTo, AllUserBranch, _globalshared.UserId_G, _globalshared.Lang_G, AccBranchesList).Result;
                        var wo2 = _workOrdersService.GetWorkOrderReport(UserId, status ?? 0, AllUserBranch, _globalshared.Lang_G, DateFrom, DateTo, AccBranchesList).Result;


                        return Ok(atasks.Union(wo2) );
                    }
                    else
                    {
                        var AllTasks1 = _projectPhasesTasksservice.GetLateTasksByUserIdrptsearch(UserId, status, _globalshared.Lang_G, DateFrom, DateTo, AllUserBranch, AccBranchesList).Result.ToList();
                        var wo3 = _workOrdersService.GetWorkOrderReport(UserId, status ?? 0, AllUserBranch, _globalshared.Lang_G, DateFrom, DateTo, AccBranchesList).Result;


                        return Ok(AllTasks1.Union(wo3) );
                    }
                }

                var wo = _workOrdersService.GetWorkOrderReport(UserId, status ?? 0, AllUserBranch, _globalshared.Lang_G, DateFrom, DateTo, AccBranchesList).Result;

                var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasksS(UserId, AllUserBranch, status, _globalshared.Lang_G, DateFrom, DateTo, AccBranchesList).Result.ToList();

                return Ok(AllTasks.Union(wo) );
            }





        [HttpGet("GetAllProjectPhasesTasksS_whithworkorder_paging")]
        public IActionResult GetAllProjectPhasesTasksS_whithworkorder_paging(int? UserId, int? status, string? DateFrom, string? DateTo,string? SearchText, int? page, int? pageSize)
        {
            if (UserId == 0) UserId = null;
            if (status == 0) status = null;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            if (status == 8)
            {
                if (DateFrom == "" && DateTo == "")
                {
                    var atasks = _projectPhasesTasksservice.GetAllLateProjectPhasesTasksbyUserId2(DateTo, _globalshared.BranchId_G, _globalshared.UserId_G, _globalshared.Lang_G, SearchText).Result;
                    var wo2 = _workOrdersService.GetWorkOrderReport(UserId, status ?? 0, _globalshared.BranchId_G, _globalshared.Lang_G, DateFrom, DateTo, SearchText).Result;


                    //return Ok(atasks.Union(wo2));

                    var all2 = atasks.Union(wo2);
                    if (page > 0 && pageSize > 0)
                    {
                        var data2 = GeneratePagination<ProjectPhasesTasksVM>.ToPagedList(all2.ToList(), page.Value, pageSize.Value);
                        var result2 = new PagedLists<ProjectPhasesTasksVM>(data2.MetaData, data2);
                        return Ok(result2);
                    }
                    else
                    {
                        return Ok(all2);

                    }
                }
                else
                {
                    var AllTasks1 = _projectPhasesTasksservice.GetLateTasksByUserIdrptsearch(UserId, status, _globalshared.Lang_G, DateFrom, DateTo, _globalshared.BranchId_G,SearchText).Result.ToList();
                    var wo3 = _workOrdersService.GetWorkOrderReport(UserId, status ?? 0, _globalshared.BranchId_G, _globalshared.Lang_G, DateFrom, DateTo,SearchText).Result;


                    var all3=AllTasks1.Union(wo3);
                    if (page > 0 && pageSize > 0)
                    {
                        var data3 = GeneratePagination<ProjectPhasesTasksVM>.ToPagedList(all3.ToList(), page.Value, pageSize.Value);
                    var result3 = new PagedLists<ProjectPhasesTasksVM>(data3.MetaData, data3);
                        return Ok(result3);
                    }
                    else
                    {
                        return Ok(all3);

                    }
                }
            }

            var wo = _workOrdersService.GetWorkOrderReport(UserId, status ?? 0, _globalshared.BranchId_G, _globalshared.Lang_G, DateFrom, DateTo,SearchText).Result;

            var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasksS(UserId, _globalshared.BranchId_G, status, _globalshared.Lang_G, DateFrom, DateTo,SearchText).Result.ToList();

            //return Ok(AllTasks.Union(wo));

            var all = AllTasks.Union(wo);
            if (page > 0 && pageSize > 0)
            {
                var data = GeneratePagination<ProjectPhasesTasksVM>.ToPagedList(all.ToList(), page.Value, pageSize.Value);
                var result = new PagedLists<ProjectPhasesTasksVM>(data.MetaData, data);
                return Ok(result);
            }
            else
            {
                return Ok(all);

            }
        }



        [HttpGet("GetAllLateProjectPhasesByuser_rpt")]
        public IActionResult GetAllLateProjectPhasesByuser_rpt(int? UserId, int? status, string? DateFrom, string? DateTo, int? BranchId, [FromQuery] List<int> BranchesList)          
        {
            if (UserId == 0)UserId = null;
            if (status == 0) status = null;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllUserBranch = 0;
            if (BranchId == -1) AllUserBranch = 0;
            else AllUserBranch = _globalshared.BranchId_G;
            var AccBranchesList = BranchesList ?? new List<int>();
            if (AccBranchesList.Count() == 0)
            {
                AccBranchesList.Add(_globalshared.BranchId_G);
            }
            if ((DateFrom == "" && DateTo == "") || (DateFrom == null && DateTo == null))
                {
                    var atasks = _projectPhasesTasksservice.GetAllLateProjectPhasesTasksbyUserId2(DateTo, AllUserBranch, UserId, _globalshared.Lang_G,AccBranchesList);

                    return Ok(atasks );
                }
                else
                {
                    var AllTasks = _projectPhasesTasksservice.GetLateTasksByUserIdrptsearch(UserId, status, _globalshared.Lang_G, DateFrom, DateTo, AllUserBranch,AccBranchesList).Result.ToList();

                    return Ok(AllTasks );
                }
         }

        [HttpGet("GetAllLateProjectPhasesByuser_rpt_paging")]
        public IActionResult GetAllLateProjectPhasesByuser_rpt_paging(int? UserId, int? status, string? DateFrom, string? DateTo, string? SearchText, int? page, int? pageSize)
        {
            if (UserId == 0) UserId = null;
            if (status == 0) status = null;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            if ((DateFrom == "" && DateTo == "") || (DateFrom == null && DateTo == null))
            {
                var atasks = _projectPhasesTasksservice.GetAllLateProjectPhasesTasksbyUserId2(DateTo, _globalshared.BranchId_G, UserId, _globalshared.Lang_G,SearchText);
                if (page > 0 && pageSize > 0)
                {
                    var data = GeneratePagination<ProjectPhasesTasksVM>.ToPagedList(atasks.Result.ToList(), page.Value, pageSize.Value);
                    var result = new PagedLists<ProjectPhasesTasksVM>(data.MetaData, data);
                    return Ok(result);
                
            }
            else
            {
                return Ok(atasks);

            }
        }
            else
            {
                var AllTasks = _projectPhasesTasksservice.GetLateTasksByUserIdrptsearch(UserId, status, _globalshared.Lang_G, DateFrom, DateTo, _globalshared.BranchId_G,SearchText).Result.ToList();
                if (page > 0 && pageSize > 0)
                {
                    var data = GeneratePagination<ProjectPhasesTasksVM>.ToPagedList(AllTasks.ToList(), page.Value, pageSize.Value);
                    var result = new PagedLists<ProjectPhasesTasksVM>(data.MetaData, data);
                    return Ok(result);
                }
                else
                {
                    return Ok(AllTasks);

                }
            }
        }



        [HttpGet("GetAllProjectPhasesTasksbystatus")]
        public IActionResult GetAllProjectPhasesTasksbystatus(int? UserId, int? status, string? DateFrom, string? DateTo)           
        {
            if (UserId == 0)UserId = null;
            if (status == 0) status = null;
            //var AccBranchesList = BranchesList ?? new List<int>();
            var AccBranchesList = new List<int>();
            if (AccBranchesList.Count() == 0)
            {
                AccBranchesList.Add(_globalshared.BranchId_G);
            }
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasksbystatus(UserId, _globalshared.BranchId_G, status, _globalshared.Lang_G, DateFrom, DateTo, AccBranchesList).Result.ToList();

                return Ok(AllTasks );           
        }
        [HttpGet("GetAllProjectPhasesTasksbystatus_WithworkOrder")]
        public IActionResult GetAllProjectPhasesTasksbystatus_WithworkOrder(int? UserId, int? status, string? DateFrom, string? DateTo, int? BranchId, [FromQuery] List<int> BranchesList)
        {
            if (UserId == 0)UserId = null;
            if (status == 0) status = null;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var AllUserBranch = 0;
            if (BranchId == -1) AllUserBranch = 0;
            else AllUserBranch = _globalshared.BranchId_G;
            var AccBranchesList = BranchesList ?? new List<int>();
            if (AccBranchesList.Count() == 0)
            {
                AccBranchesList.Add(_globalshared.BranchId_G);
            }


            var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasksbystatus(UserId, AllUserBranch, status, _globalshared.Lang_G, DateFrom, DateTo,AccBranchesList).Result.ToList();
                var wo = _workOrdersService.GetWorkOrderReport(UserId, status ?? 0, AllUserBranch, _globalshared.Lang_G, DateFrom, DateTo,AccBranchesList).Result;
                return Ok(AllTasks.Union(wo));
        }


        [HttpGet("GetAllProjectPhasesTasksbystatus_WithworkOrder_paging")]
        public IActionResult GetAllProjectPhasesTasksbystatus_WithworkOrder_paging(int? UserId, int? status, string? DateFrom, string? DateTo, string? SearchText, int? page, int? pageSize)
        {
            if (UserId == 0) UserId = null;
            if (status == 0) status = null;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasksbystatus(UserId, _globalshared.BranchId_G, status, _globalshared.Lang_G, DateFrom, DateTo, SearchText).Result.ToList();
            var wo = _workOrdersService.GetWorkOrderReport(UserId, status ?? 0, _globalshared.BranchId_G, _globalshared.Lang_G, DateFrom, DateTo, SearchText).Result;
            var AllTask=AllTasks.Union(wo);
            if (page > 0 && pageSize > 0)
            {
                var data = GeneratePagination<ProjectPhasesTasksVM>.ToPagedList(AllTask.ToList(), page.Value, pageSize.Value);
                var result = new PagedLists<ProjectPhasesTasksVM>(data.MetaData, data);
                return Ok(result);
            }
            else
            {
                return Ok(AllTask);

            }
        }

        [HttpGet("GetAllTasksByProjectIdS")]
        public IActionResult GetAllTasksByProjectIdS(int? ProjectId, string? DateFrom, string? DateTo)           
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetAllTasksByProjectIdS(ProjectId,0, DateFrom??"", DateTo??"", _globalshared.BranchId_G) );            
        }

        [HttpGet("GetAllTasksByProjectIdS_withworkorder")]
        public IActionResult GetAllTasksByProjectIdS_withworkorder(int? ProjectId, int? DepartmentId, string? DateFrom, string? DateTo)
            {
            if (ProjectId == 0)
                ProjectId = null;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var wo = new List<ProjectPhasesTasksVM>();
            if (DepartmentId==0)
            {
                wo = _workOrdersService.GetWorkOrderReport_ptoject(ProjectId, _globalshared.Lang_G, DateFrom ?? "", DateTo ?? "", _globalshared.BranchId_G).Result.ToList();
            }
            //var wo = _workOrdersService.GetWorkOrderReport_ptoject(ProjectId, _globalshared.Lang_G, DateFrom??"", DateTo??"", _globalshared.BranchId_G).Result;
              return Ok(_projectPhasesTasksservice.GetAllTasksByProjectIdS(ProjectId, DepartmentId, DateFrom??"", DateTo??"", _globalshared.BranchId_G).Result.ToList().Union(wo) );

            }


        [HttpGet("GetAllTasksByProjectIdS_withworkorder_paging")]
        public IActionResult GetAllTasksByProjectIdS_withworkorder_paging(int? ProjectId, int? DepartmentId, string? DateFrom, string? DateTo, string? Searchtext, int? page , int? pageSize)
        {
            if (ProjectId == 0)
                ProjectId = null;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var wo = new List<ProjectPhasesTasksVM>();
            if (DepartmentId == 0)
            {
                wo = _workOrdersService.GetWorkOrderReport_ptoject(ProjectId, _globalshared.Lang_G, DateFrom ?? "", DateTo ?? "", _globalshared.BranchId_G, Searchtext).Result.ToList();
            }
            //var wo = _workOrdersService.GetWorkOrderReport_ptoject(ProjectId, _globalshared.Lang_G, DateFrom??"", DateTo??"", _globalshared.BranchId_G).Result;
            var tasks=_projectPhasesTasksservice.GetAllTasksByProjectIdS(ProjectId, DepartmentId, DateFrom ?? "", DateTo ?? "", _globalshared.BranchId_G,Searchtext).Result.ToList().Union(wo);
            if (page > 0 && pageSize > 0)
            {
                var data = GeneratePagination<ProjectPhasesTasksVM>.ToPagedList(tasks.ToList(), page.Value, pageSize.Value);
                var result = new PagedLists<ProjectPhasesTasksVM>(data.MetaData, data);
                return Ok(result);
            }
            else
            {
                return Ok(tasks);
            }

        }



        [HttpGet("GetAllProjectPhasesTasksU")]
        public IActionResult GetAllProjectPhasesTasksU(int UserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasksU(UserId, _globalshared.BranchId_G, _globalshared.Lang_G).Result.ToList();
                return Ok(AllTasks );
            }
        [HttpGet("GetAllProjectPhasesTasksUPage")]
        public IActionResult GetAllProjectPhasesTasksUPage(int UserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someTask = _projectPhasesTasksservice.GetAllProjectPhasesTasksUPage(0, _globalshared.BranchId_G, _globalshared.Lang_G).Result.ToList();
                foreach (var userBranch in userBranchs)
                {
                    var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasksUPage(UserId, userBranch.BranchId, _globalshared.Lang_G).Result.ToList();
                    var Tasks = someTask.Union(AllTasks);
                    someTask = Tasks.ToList();
                }

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
        [HttpGet("GetUserDataById")]
        public IActionResult GetUserDataById(int UserId)
        {
            ProjectVM ProjectsSearch =new ProjectVM();
            ProjectsSearch.MangerId = UserId;
            ProjectsSearch.Status = 0;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            var someTask = _projectPhasesTasksservice.GetAllProjectPhasesTasksUPage(0, _globalshared.BranchId_G, _globalshared.Lang_G).Result.ToList();
            foreach (var userBranch in userBranchs)
            {
                var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasksUPage(UserId, userBranch.BranchId, _globalshared.Lang_G).Result.ToList();
                var Tasks = someTask.Union(AllTasks);
                someTask = Tasks.ToList();
            }

            ProjectsSearch.BranchId = _globalshared.BranchId_G;
            var AllPojects = _projectservice.GetAllProjectsNew(Con ?? "", ProjectsSearch, _globalshared.UserId_G, 0, ProjectsSearch.FilterType ?? 0, _globalshared.BranchId_G).Result.ToList();


            var SelectStetment = "select se.SettingId,se.DescriptionAr,det.ProSettingNo,ProSettingNote,se.UserId  from Pro_Settings se join Pro_ProSettingDetails det on se.ProjSubTypeId=det.ProjectSubtypeId where se.IsDeleted=0 and det.IsDeleted=0 and se.Type=3 and se.UserId=" + UserId + "";
            var result = _settingsservice.GetSetting_Statment(Con, SelectStetment).ToList();
            var UserData = new 
            {
                ProjectCounts=AllPojects.Count(),
                TaskCount= someTask.Count(),
                Setting= result.Count()

            };
            return Ok(UserData);
        }

        [HttpGet("GetAllProjectPhasesTasksW")]
        public IActionResult GetAllProjectPhasesTasksW()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var someTask = _projectPhasesTasksservice.GetAllProjectPhasesTasksW(_globalshared.BranchId_G, _globalshared.Lang_G).Result.ToList();

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

        [HttpGet("GetAllProjectPhasesTasksW_whithworkorder")]
        public IActionResult GetAllProjectPhasesTasksW_whithworkorder()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someTask = _projectPhasesTasksservice.GetAllProjectPhasesTasksW(_globalshared.BranchId_G, _globalshared.Lang_G).Result.ToList();

                var wo = _workOrdersService.GetALlWorkOrderReport(_globalshared.Lang_G, _globalshared.BranchId_G).Result;

            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;
            //var result = new ContentResult
            //{
            //    Content = serializer.Serialize(someTask.Union(wo)),
            //    ContentType = "application/json"
            //};
            //return result;
            return Ok(someTask.Union(wo));


        }

        //public IActionResult GetAllTasksPhasesByProjectId(int ProjectId )
        //{
        //    var AllTasks = _projectPhasesTasksservice.GetAllTasksPhasesByProjectId(ProjectId, _globalshared.BranchId_G).ToList();

        //    return Ok(AllTasks );
        //}
        [HttpGet("Tasks")]
        public IActionResult Tasks(int ProjectId, int PageNo)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //ViewBag.Flag = ProjectId;
            //    ViewBag.Flag_PageNo = PageNo;
            //    if (ProjectId != -1)
            //    {
            //        var AllProject = _projectservice.GetProjectById(_globalshared.Lang_G, ProjectId);
            //        ViewBag.ProjectId = AllProject.ProjectId;
            //        ViewBag.ProjectNo = AllProject.ProjectNo;
            //        ViewBag.CustomerId = AllProject.CustomerId;
            //        ViewBag.ActiveMainPhaseId = AllProject.ActiveMainPhaseId;
            //        ViewBag.ActiveSubPhaseId = AllProject.ActiveSubPhaseId;

                    //TempData["ProjectId"] = AllProject.ProjectId;
                    //TempData["ProjectNo"] = AllProject.ProjectNo;
                    //TempData["CustomerId"] = AllProject.CustomerId;
                    //TempData["ActiveMainPhaseId"] = AllProject.ActiveMainPhaseId;
                    //TempData["ActiveSubPhaseId"] = AllProject.ActiveSubPhaseId;
                //}

                return Ok();
                //return Ok();
            }
            [HttpGet("GetAllNewProjectPhasesTasks")]
            public IActionResult GetAllNewProjectPhasesTasks(string? EndDateP)
            {

                var AllTasks = _projectPhasesTasksservice.GetAllNewProjectPhasesTasks(EndDateP ?? "", 0).Result.ToList();
                return Ok(AllTasks );


            }

        [HttpGet("GetTasksWithoutUser")]
        public IActionResult GetTasksWithoutUser(int ? DepartmentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllTasks = _projectPhasesTasksservice.GetTasksWithoutUser(DepartmentId,_globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(AllTasks);

        }
        [HttpGet("GetAllNewProjectPhasesTasksTree")]
        public IActionResult GetAllNewProjectPhasesTasksTree(string? EndDateP)
            {

                var AllTasks = _projectPhasesTasksservice.GetAllNewProjectPhasesTasks(EndDateP ?? "", 0).Result.GroupBy(x => x.ProjectTypeId).ToList();
                return Ok(AllTasks );


            }
        [HttpGet("GetAllNewProjectPhasesTasksTreed")]
        public IActionResult GetAllNewProjectPhasesTasksTreed(string? EndDateP)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var AllTasks = _projectPhasesTasksservice.GetAllNewProjectPhasesTasksd(EndDateP ?? "", _globalshared.BranchId_G, _globalshared.Lang_G).ToList();

                return Ok(AllTasks );


            }
        [HttpGet("GetAllNewProjectPhasesTasksByUserId")]
        public IActionResult GetAllNewProjectPhasesTasksByUserId(string? EndDateP, int UserId2)
            {
                var AllTasks = _projectPhasesTasksservice.GetAllNewProjectPhasesTasksByUserId(EndDateP ?? "", 0, UserId2).Result.ToList();
                return Ok(AllTasks );
            }
        [HttpGet("GetAllLateProjectPhasesTasks")]
        public IActionResult GetAllLateProjectPhasesTasks(string? EndDateP)
            {
                var AllTasks = _projectPhasesTasksservice.GetAllLateProjectPhasesTasks(EndDateP ?? "", 0).Result.ToList();
                return Ok(AllTasks );
            }
        [HttpGet("GetAllLateProjectPhasesTasksd")]
        public IActionResult GetAllLateProjectPhasesTasksd(string? EndDateP)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var AllTasks = _projectPhasesTasksservice.GetAllLateProjectPhasesTasksd(EndDateP ?? "", _globalshared.BranchId_G, _globalshared.Lang_G).ToList();
                return Ok(AllTasks );
            }
        [HttpGet("GetAllLateProjectPhasesTasksbyUserId2")]
        public IActionResult GetAllLateProjectPhasesTasksbyUserId2(string? EndDateP, int UserId2)
            {
                var AllTasks = _projectPhasesTasksservice.GetAllLateProjectPhasesTasksbyUserId(EndDateP ?? "", 0, UserId2).Result.ToList();
                return Ok(AllTasks );
            }
        [HttpGet("GetAllProjectPhasesTasks2")]
        public IActionResult GetAllProjectPhasesTasks2(string? SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasks2(SearchText ?? "", 0, _globalshared.Lang_G).Result.ToList();
                return Ok(AllTasks );


            }
        [HttpGet("GetProjectPhasesTasks2")]
        public IActionResult GetProjectPhasesTasks2(int? ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var AllTasks = _projectPhasesTasksservice.GetProjectPhasesTasks2(ProjectId??0, 0, _globalshared.Lang_G).Result.ToList();
            return Ok(AllTasks);

        }
        [HttpPost("GetProjectPhasesTasks2Tree")]
        public IActionResult GetProjectPhasesTasks2Tree([FromForm] int? ProjectId, [FromForm] string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var AllTasks = _projectPhasesTasksservice.GetProjectPhasesTasks2Tree(ProjectId ?? 0, SearchText??"", 0, _globalshared.Lang_G).Result.ToList();
            return Ok(AllTasks);

        }
        //Dashboard Admin
        [HttpGet("GetInProgressProjectPhasesTasks")]
        public IActionResult GetInProgressProjectPhasesTasks(string? SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetInProgressProjectPhasesTasks(SearchText ?? "", _globalshared.Lang_G) );
            }

        [HttpGet("GetInProgressProjectPhasesTasks_Branches")]
        public IActionResult GetInProgressProjectPhasesTasks_Branches(string? SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //5le balk mst5dmnha f home btgeb l tasks l f dashboard
            //-------------------------------------------------------

            //var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G);
            //var someProjectPhasesTasks = _projectPhasesTasksservice.GetInProgressProjectPhasesTasks_Branches(0, _globalshared.Lang_G);
            //foreach (var userBranch in userBranchs)
            //{
            //    var AllTasks = _projectPhasesTasksservice.GetInProgressProjectPhasesTasks_Branches(userBranch._globalshared.BranchId_G, _globalshared.Lang_G).ToList();
            //    var Tasks = someProjectPhasesTasks.Union(AllTasks);
            //    someProjectPhasesTasks = Tasks.ToList();
            //}
            var someProjectPhasesTasks = _projectPhasesTasksservice.GetInProgressProjectPhasesTasks_Branches(_globalshared.BranchId_G, _globalshared.Lang_G).Result.ToList();

            return Ok(someProjectPhasesTasks);

        }

        [HttpGet("GetInProgressProjectPhasesTasks_BranchesFilterd")]
        public IActionResult GetInProgressProjectPhasesTasks_BranchesFilterd(int? CustomerId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        
            var someProjectPhasesTasks = _projectPhasesTasksservice.GetInProgressProjectPhasesTasks_Branches(_globalshared.BranchId_G, _globalshared.Lang_G, CustomerId).Result.ToList();

            return Ok(someProjectPhasesTasks);

        }


        [HttpGet("GetInProgressProjectPhasesTasks_BranchesFilterd_paging")]
        public IActionResult GetInProgressProjectPhasesTasks_BranchesFilterd_paging(int? CustomerId,string? Searchtext, int page = 1, int pageSize = 10)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var someProjectPhasesTasks = _projectPhasesTasksservice.GetInProgressProjectPhasesTasks_Branches(_globalshared.BranchId_G, _globalshared.Lang_G, CustomerId, Searchtext).Result.ToList();
            var data = GeneratePagination<ProjectPhasesTasksVM>.ToPagedList(someProjectPhasesTasks.ToList(), page, pageSize);
            var result = new PagedLists<ProjectPhasesTasksVM>(data.MetaData, data);
            return Ok(result);

        }


        [HttpGet("GetInProgressProjectPhasesTasksHome")]
        public IActionResult GetInProgressProjectPhasesTasksHome(string? SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllTasks = _projectPhasesTasksservice.GetInProgressProjectPhasesTasksHome(SearchText ?? "", 0, _globalshared.Lang_G).Result.ToList();
                return Ok(AllTasks );
            }

        [HttpGet("GetInProgressProjectPhasesTasksHome_Search")]
        public IActionResult GetInProgressProjectPhasesTasksHome_Search(ProjectPhasesTasksVM Search)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Search.UserId = _globalshared.UserId_G;
                if (Search.BranchId == null)
                {
                    Search.BranchId = 0;
                }
                var AllTasks = _projectPhasesTasksservice.GetInProgressProjectPhasesTasksHome_Search(Search, _globalshared.Lang_G).Result;
                int x = AllTasks.Count();
                return Ok(AllTasks );
            }

        [HttpPost("GetInProgressProjectPhasesTasksHome_SearchPost")]
        public IActionResult GetInProgressProjectPhasesTasksHome_SearchPost([FromBody]ProjectPhasesTasksVM Search)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Search.UserId = _globalshared.UserId_G;
            if (Search.BranchId == null)
            {
                Search.BranchId = 0;
            }
            var AllTasks = _projectPhasesTasksservice.GetInProgressProjectPhasesTasksHome_Search(Search, _globalshared.Lang_G).Result;
            int x = AllTasks.Count();
            return Ok(AllTasks);
        }
        [HttpGet("GetTasksByUserIdCustomerIdProjectId")]
        public IActionResult GetTasksByUserIdCustomerIdProjectId(int? UserId, int? CustomerId, int? ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var tasks = _projectPhasesTasksservice.GetTasksByUserIdCustomerIdProjectId(_globalshared.UserId_G, CustomerId, ProjectId);
                return Ok(tasks );
            }

        [HttpPost("GetTasksByUserIdCustomerIdProjectIdTree")]
        public IActionResult GetTasksByUserIdCustomerIdProjectIdTree([FromForm]int? UserId, [FromForm] int? CustomerId, [FromForm] int? ProjectId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var tasks = _projectPhasesTasksservice.GetTasksByUserIdCustomerIdProjectIdTree(UserId, CustomerId, ProjectId,_globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(tasks );
        }
        [HttpGet("GetLateTasksByUserIdCustomerIdProjectId")]
        public IActionResult GetLateTasksByUserIdCustomerIdProjectId(int? UserId, int? CustomerId, int? ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var tasks = _projectPhasesTasksservice.GetLateTasksByUserIdCustomerIdProjectId(_globalshared.UserId_G, CustomerId, ProjectId);
                return Ok(tasks );
            }

        [HttpPost("GetLateTasksByUserIdCustomerIdProjectIdTree")]
        public IActionResult GetLateTasksByUserIdCustomerIdProjectIdTree([FromForm] int? UserId, [FromForm] int? CustomerId, [FromForm] int? ProjectId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var tasks = _projectPhasesTasksservice.GetLateTasksByUserIdCustomerIdProjectIdTree(UserId, CustomerId, ProjectId,_globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(tasks );
        }
        [HttpGet("GetTasksByUserId")]
        public IActionResult GetTasksByUserId(int Status, string? SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetTasksByUserId(_globalshared.UserId_G, Status, _globalshared.BranchId_G) );

            }
        [HttpGet("GetTasksByUserIdHome")]
        public IActionResult GetTasksByUserIdHome(int Status, string? SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetTasksByUserIdHome(_globalshared.UserId_G, Status, _globalshared.BranchId_G) );

            }
        [HttpGet("GetTasksByUserIdUser")]
        public IActionResult GetTasksByUserIdUser(int Status, string? SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetTasksByUserIdUser(_globalshared.UserId_G, _globalshared.Lang_G, Status, _globalshared.BranchId_G) );

            }
        [HttpGet("GetTasksByUserIdUserCount")]
        public int GetTasksByUserIdUserCount(int Status, string? SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.GetTasksByUserIdUser(_globalshared.UserId_G, _globalshared.Lang_G, Status, _globalshared.BranchId_G).Result.Count();
                return result;
            }
        [HttpGet("GetTasksFilterationByUserId")]
        public IActionResult GetTasksFilterationByUserId(int flag, string? startdate, string? enddate)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetDayWeekMonth_Tasks(_globalshared.UserId_G, 0, _globalshared.BranchId_G, flag, startdate, enddate) );

            }
        [HttpGet("GetTasksSearchByUserId")]
        public IActionResult GetTasksSearchByUserId(int UserId, int Status)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetTasksByUserId(_globalshared.UserId_G, Status, _globalshared.BranchId_G) );
            }
        //public IActionResult GetTasksSearchByUserId(int? _globalshared.UserId_G, string SearchText )
        //{
        //    return Ok(_projectPhasesTasksservice.GetTasksByUserId(_globalshared.UserId_G,0 SearchText ?? "", _globalshared.BranchId_G) );
        //}
        [HttpGet("GetTasksByDate")]
        public IActionResult GetTasksByDate(string StartDate, string EndDate)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someTask = _projectPhasesTasksservice.GetTasksByDate(StartDate, EndDate, 0).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllTasks = _projectPhasesTasksservice.GetTasksByDate(StartDate, EndDate, userBranch.BranchId).Result.ToList();
                    var Tasks = someTask.Union(AllTasks);
                    someTask = Tasks.ToList();
                }
                return Ok(someTask );
            }

        //Used for IndexUser
        [HttpGet("GetNewTasksByUserId")]
        public IActionResult GetNewTasksByUserId(string? EndDateP, bool? AllStatusExptEnd = null)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetNewTasksByUserId(EndDateP ?? "", _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, AllStatusExptEnd) );
            }

        [HttpGet("GetNewTasksByUserId2")]
        public IActionResult GetNewTasksByUserId2(int? PorjectId,int? CustomerId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetNewTasksByUserId2(_globalshared.UserId_G, _globalshared.Lang_G, PorjectId, CustomerId));
        }



        [HttpGet("GetNewTasksByUserId2_Paging")]
        public IActionResult GetNewTasksByUserId2_Paging(int? PorjectId, int? CustomerId,string? Searchtext, int page = 1, int pageSize = 10)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var res=_projectPhasesTasksservice.GetNewTasksByUserId2(_globalshared.UserId_G, _globalshared.Lang_G, PorjectId, CustomerId, Searchtext);
            var data = GeneratePagination<ProjectPhasesTasksVM>.ToPagedList(res.Result.ToList(), page, pageSize);
            var result = new PagedLists<ProjectPhasesTasksVM>(data.MetaData, data);
            return Ok(result);
        }


        [HttpGet("FillNewTasksByUserId")]
        public IActionResult FillNewTasksByUserId(string? EndDateP, bool? AllStatusExptEnd = null)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var ProjectsList = _projectPhasesTasksservice.GetNewTasksByUserId(EndDateP ?? "", _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, AllStatusExptEnd).Result.Select(s => new
                {
                    Id = s.ProjectId,
                    Name = s.ProjectNumber
                }).Distinct();

                return Ok(ProjectsList );
            }
        //Used for IndexUser Search ProjectId
        [HttpGet("GetNewTasksByUserIdSearchProj")]
        public IActionResult GetNewTasksByUserIdSearchProj(string? EndDateP, int? ProjectId, bool? AllStatusExptEnd = null)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetNewTasksByUserIdSearchProj(EndDateP ?? "", ProjectId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, AllStatusExptEnd) );
            }
        [HttpGet("GetLateTasksByUserId")]
        public IActionResult GetLateTasksByUserId(string? EndDateP)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetLateTasksByUserId(EndDateP??"", _globalshared.UserId_G, _globalshared.BranchId_G) );
            }
        [HttpGet("GetLateTasksByUserIdHome")]
        public IActionResult GetLateTasksByUserIdHome(string? EndDateP)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetLateTasksByUserIdHome(EndDateP??"", _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G) );
            }

        [HttpGet("GetLateTasksByUserIdHomeFilterd")]
        public IActionResult GetLateTasksByUserIdHomeFilterd(int? PorjectId, int? CustomerId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetLateTasksByUserIdHomefilterd( _globalshared.UserId_G, _globalshared.Lang_G, PorjectId, CustomerId));
        }


        [HttpGet("GetLateTasksByUserIdHomeFilterd_paging")]
        public IActionResult GetLateTasksByUserIdHomeFilterd_paging(int? PorjectId, int? CustomerId, string? Searchtext, int page = 1, int pageSize = 10)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var res=_projectPhasesTasksservice.GetLateTasksByUserIdHomefilterd(_globalshared.UserId_G, _globalshared.Lang_G, PorjectId, CustomerId, Searchtext);
            var data = GeneratePagination<ProjectPhasesTasksVM>.ToPagedList(res.Result.ToList(), page, pageSize);
            var result = new PagedLists<ProjectPhasesTasksVM>(data.MetaData, data);
            return Ok(result);

        }

        [HttpGet("GetLateTasksByUserIdHome_Search")]
        public IActionResult GetLateTasksByUserIdHome_Search(ProjectPhasesTasksVM Search, string Lang)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Search.UserId = _globalshared.UserId_G;
                return Ok(_projectPhasesTasksservice.GetLateTasksByUserIdHome_Search(Search, _globalshared.Lang_G) );
            }
        [HttpGet("GetTasksByStatus")]
        public IActionResult GetTasksByStatus(int? StatusId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someTask = _projectPhasesTasksservice.GetTasksByStatus(StatusId, 0).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllTasks = _projectPhasesTasksservice.GetTasksByStatus(StatusId, userBranch.BranchId).Result.ToList();
                    var Tasks = someTask.Union(AllTasks);
                    someTask = Tasks.ToList();
                }
                return Ok(someTask );
            }
        [HttpGet("GetTasksByProjectNo")]
        public IActionResult GetTasksByProjectNo(string ProjectNo)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someTask = _projectPhasesTasksservice.GetTasksByProjectNo(ProjectNo ?? "", 0).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllTasks = _projectPhasesTasksservice.GetTasksByProjectNo(ProjectNo ?? "", userBranch.BranchId).Result.ToList();
                    var Tasks = someTask.Union(AllTasks);
                    someTask = Tasks.ToList();
                }
                return Ok(someTask );
            }
        [HttpGet("GetAllTasksSearch")]
        public IActionResult GetAllTasksSearch(ProjectPhasesTasksVM TasksSearch)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someTask = _projectPhasesTasksservice.GetAllTasksSearch(TasksSearch, 0).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllTasks = _projectPhasesTasksservice.GetAllTasksSearch(TasksSearch, userBranch.BranchId).Result.ToList();
                    var Tasks = someTask.Union(AllTasks);
                    someTask = Tasks.ToList();
                }
                return Ok(someTask );
            }
        [HttpGet("GetProjectMainPhasesByProjectId")]
        public IActionResult GetProjectMainPhasesByProjectId(int ProjectId)
            {
                return Ok(_projectPhasesTasksservice.GetProjectMainPhasesByProjectId(ProjectId) );
            }
        [HttpGet("GetProjectSubPhasesByProjectId")]
        public IActionResult GetProjectSubPhasesByProjectId(int MainPhaseId)
            {
                return Ok(_projectPhasesTasksservice.GetProjectSubPhasesByProjectId(MainPhaseId) );
            }
        [HttpGet("GetAllSubPhasesTasks")]
        public IActionResult GetAllSubPhasesTasks(int SubPhaseId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.GetAllSubPhasesTasks(SubPhaseId, _globalshared.Lang_G);
                return Ok(result );
            }
        [HttpPost("SaveProjectPhasesTasks")]
        public IActionResult SaveProjectPhasesTasks(Project Project)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;

            try
            {
                var url = Path.Combine("Email/MailStamp.html");
                //var file = Server.MapPath("~/dist/assets/images/logo.png");
                string resultLogoUrl = org.LogoUrl.Remove(0, 1);
                var file = Path.Combine(resultLogoUrl);
                var result = new GeneralMessage();
                if (Project.NewSetting == true)
                {
                    result = _projectPhasesTasksservice.SaveProjectPhasesTasksNew(Project, _globalshared.UserId_G, Project.BranchId, url, file);
                }
                else
                {
                    result = _projectPhasesTasksservice.SaveProjectPhasesTasks(Project, _globalshared.UserId_G, Project.BranchId, url, file);
                }

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    List<ProUserPrivileges> NewPrivs = new List<ProUserPrivileges>();
                    foreach (var pri in Project.ProUserPrivileges)
                    {
                        pri.ProjectID = Convert.ToInt32(result.ReturnedStr);
                        NewPrivs.Add(pri);
                    }

                    var result2 = _proUserPrivilegesService.SavePrivProList(NewPrivs, _globalshared.UserId_G, Project.BranchId);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                SendMail_ProjectSavedWrong("Part phase(0)" + " " + ex.Message + ">>>>" + ex.InnerException, false, org);
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "فشل في الحفظ", ReturnedStr = "Part (0)" + " " + ex.Message + ">>>>" + ex.InnerException });
            }

        }

        [HttpPost("SaveProjectPhasesTasksPart1")]
        public IActionResult SaveProjectPhasesTasksPart1(Project Project)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;

            try
            {
                var url = Path.Combine("Email/MailStamp.html");
                //var file = Server.MapPath("~/dist/assets/images/logo.png");
                string resultLogoUrl = org.LogoUrl.Remove(0, 1);
                var file = Path.Combine(resultLogoUrl);
                var result = new GeneralMessage();
                if (Project.NewSetting == true)
                {
                    result = _projectPhasesTasksservice.SaveProjectPhasesTasksNewPart1(Project, _globalshared.UserId_G, Project.BranchId, url, file);
                }
                else
                {
                    result = _projectPhasesTasksservice.SaveProjectPhasesTasksPart1(Project, _globalshared.UserId_G, Project.BranchId, url, file);
                }

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    List<ProUserPrivileges> NewPrivs = new List<ProUserPrivileges>();
                    foreach (var pri in Project.ProUserPrivileges)
                    {
                        pri.ProjectID = Convert.ToInt32(result.ReturnedStr);
                        NewPrivs.Add(pri);
                    }

                    var result2 = _proUserPrivilegesService.SavePrivProList(NewPrivs, _globalshared.UserId_G, Project.BranchId);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                SendMail_ProjectSavedWrong("Part phase(0)" + " " + ex.Message + ">>>>" + ex.InnerException, false, org);
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "فشل في الحفظ", ReturnedStr = "Part (0)" + " " + ex.Message + ">>>>" + ex.InnerException });
            }

        }
        [HttpPost("SaveProjectPhasesTasksPart2")]
        public IActionResult SaveProjectPhasesTasksPart2(Project Project)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            try
            {
                var result = new GeneralMessage();
                if (Project.NewSetting == true)
                {
                    result = _projectPhasesTasksservice.SaveProjectPhasesTasksNewPart2(Project, _globalshared.UserId_G, Project.BranchId, "", "");
                }
                else
                {
                    result = _projectPhasesTasksservice.SaveProjectPhasesTasksPart2(Project, _globalshared.UserId_G, Project.BranchId, "", "");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "فشل في الحفظ", ReturnedStr = "Part (0)" + " " + ex.Message + ">>>>" + ex.InnerException });
            }

        }
        [HttpPost("SaveProjectPhasesTasksPart3")]
        public IActionResult SaveProjectPhasesTasksPart3(Project Project)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            try
            {
                var result = new GeneralMessage();
                if (Project.NewSetting == true)
                {
                    result = _projectPhasesTasksservice.SaveProjectPhasesTasksNewPart3(Project, _globalshared.UserId_G, Project.BranchId, "", "");
                }
                else
                {
                    result = _projectPhasesTasksservice.SaveProjectPhasesTasksPart3(Project, _globalshared.UserId_G, Project.BranchId, "", "");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "فشل في الحفظ", ReturnedStr = "Part (0)" + " " + ex.Message + ">>>>" + ex.InnerException });
            }

        }

        [HttpPost("SendMail_ProjectSavedWrong")]
        public bool SendMail_ProjectSavedWrong(string textBody, bool IsBodyHtml, OrganizationsVM Org)
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
        [HttpPost("UpdateProjectPhasesTasks")]
        public IActionResult UpdateProjectPhasesTasks(Project Project)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);    
            var result = new GeneralMessage();
            if (Project.NewSetting == true)
            {
                result = _projectPhasesTasksservice.UpdateProjectPhasesTasksNew(Project, _globalshared.UserId_G, Project.BranchId);
            }
            else
            {
                result = _projectPhasesTasksservice.UpdateProjectPhasesTasks(Project, _globalshared.UserId_G, Project.BranchId);
            }
            return Ok(result);
            }
        [HttpPost("SaveNewProjectPhasesTasks")]
        public IActionResult SaveNewProjectPhasesTasks(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.SaveNewProjectPhasesTasks(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpPost("SaveNewProjectPhasesTasks2")]
        public IActionResult SaveNewProjectPhasesTasks2(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.SaveNewProjectPhasesTasks2(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(new { result } );
            }
        [HttpPost("SaveNewProjectPhasesTasks_E")]
        public IActionResult SaveNewProjectPhasesTasks_E(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
                // var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectPhasesTasksservice.SaveNewProjectPhasesTasks_E(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);
                return Ok(result);
            }


        [HttpPost("SendWhatsAppTask")]
        public IActionResult SendWhatsAppTask([FromForm] int? projectid, [FromForm] int? taskId, [FromForm] string? environmentURL)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //var TaskId = 7220;
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            if (projectid!=null)
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == projectid && s.Type == 3 && s.IsMerig == -1).ToList();
                foreach(var item in projectPhasesTasks)
                {
                    var Task = _projectPhasesTasksservice.GetTaskById(item.PhaseTaskId, _globalshared.Lang_G, _globalshared.UserId_G).Result;
                    ReportPDF = ProjectsReports.PrintTaskPDF(Task, "", infoDoneTasksReport);
                    //dawoudprint
                    string existTemp = System.IO.Path.Combine("TempFiles/");
                    if (!Directory.Exists(existTemp))
                    {
                        Directory.CreateDirectory(existTemp);
                    }
                    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
                    string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
                    System.IO.File.WriteAllBytes(FilePath, ReportPDF);
                    string FilePathReturn = "/TempFiles/" + FileName;
                    var Message = "لديك مهمة جديدة";
                    var resultSend = _sMSService.SendWhatsApp_Notification_Task(Task.Mobile ?? "", Message ?? "", _globalshared.UserId_G, _globalshared.BranchId_G, environmentURL ?? "", FilePathReturn);

                }
            }
            if(taskId!=null)
            {
                var Task = _projectPhasesTasksservice.GetTaskById(taskId??0, _globalshared.Lang_G, _globalshared.UserId_G).Result;
                ReportPDF = ProjectsReports.PrintTaskPDF(Task, "", infoDoneTasksReport);
                //dawoudprint
                string existTemp = System.IO.Path.Combine("TempFiles/");
                if (!Directory.Exists(existTemp))
                {
                    Directory.CreateDirectory(existTemp);
                }
                string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
                string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
                System.IO.File.WriteAllBytes(FilePath, ReportPDF);
                string FilePathReturn = "/TempFiles/" + FileName;
                var Message = "لديك مهمة جديدة";
                var resultSend = _sMSService.SendWhatsApp_Notification_Task(Task.Mobile ?? "", Message ?? "", _globalshared.UserId_G, _globalshared.BranchId_G, environmentURL ?? "", FilePathReturn);
            }

            var result = new GeneralMessage();
            result.StatusCode = HttpStatusCode.OK;
            result.ReasonPhrase = "تم الانتهاء من الارسال";
            return Ok(result);
        }
        [HttpPost("SaveTaskSetting")]
        public IActionResult SaveTaskSetting([FromBody]List<ProjectPhasesTasks> ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.SaveTaskSetting(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpPost("ConvertTask")]
        public IActionResult ConvertTask(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
                // var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectPhasesTasksservice.ConvertTask(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);
                return Ok(result);
            }
        [HttpPost("SaveTaskLongDesc")]
        public IActionResult SaveTaskLongDesc(int ProjectPhaseTasksId, string taskLongDesc)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.SaveTaskLongDesc(ProjectPhaseTasksId, taskLongDesc, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpPost("ConvertUserTasks")]
        public IActionResult ConvertUserTasks(int FromUserId, int ToUserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.ConvertUserTasks(FromUserId, ToUserId, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result );
            }
        [HttpPost("ConvertUserTasksSome")]
        public IActionResult ConvertUserTasksSome(int PhasesTaskId, int FromUserId, int ToUserId)
        {
        HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        var url = Path.Combine("Email/End Task.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
        string resultLogoUrl = org.LogoUrl.Remove(0, 1);
        var file = Path.Combine(resultLogoUrl);
        var result = _projectPhasesTasksservice.ConvertUserTasksSome(PhasesTaskId, FromUserId, ToUserId, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);
            return Ok(result );
        }
        [HttpPost("ConvertMoreUserTasks")]
        public IActionResult ConvertMoreUserTasks(List<int> PhasesTaskId, int FromUserId, int ToUserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
                // var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectPhasesTasksservice.ConvertMoreUserTasks(PhasesTaskId, FromUserId, ToUserId, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);
                return Ok(result );
            }
        [HttpPost("RequestConvertTask")]
        public IActionResult RequestConvertTask(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
                // var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectPhasesTasksservice.RequestConvertTask(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);
                return Ok(result);
            }
        [HttpPost("SetUserTask")]
        public IActionResult SetUserTask(ProjectPhasesTasks ProjectPhasesTasks)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.SetUserTask(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("PlayPauseTask")]
        public IActionResult PlayPauseTask(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.PlayPauseTask(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G);

                return Ok(result );
            }
        [HttpPost("PlustimeTask")]
        public IActionResult PlustimeTask(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
                // var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectPhasesTasksservice.PlustimeTask(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);

                return Ok(result);
            }
        [HttpPost("RefusePlustimeTask")]
        public IActionResult RefusePlustimeTask(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl); var result = _projectPhasesTasksservice.RefusePlustimeTask(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);

                return Ok(result);
            }


        [HttpPost("RefuseConvertTask")]
        public IActionResult RefuseConvertTask(ProjectPhasesTasks ProjectPhasesTasks)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl); 
            var result = _projectPhasesTasksservice.RefuseConvertTask(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);

            return Ok(result);
        }

        [HttpPost("AskForMoreDetails")]
        public IActionResult AskForMoreDetails(int projectphaseid, int askdetail)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.AskForMoreDetails(projectphaseid, askdetail, _globalshared.UserId_G, _globalshared.BranchId_G);

                return Ok(result);
            }
        [HttpPost("FinishTask")]
        public IActionResult FinishTask(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
                // var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
           var file = Path.Combine(resultLogoUrl);

            var result = _projectPhasesTasksservice.FinishTask(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);

                return Ok(result);
            }
        [HttpPost("FinishTaskManager")]
        public IActionResult FinishTaskManager(ProjectPhasesTasks ProjectPhasesTasks)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);

            var result = _projectPhasesTasksservice.FinishTaskManager(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);

            return Ok(result);
        }
        [HttpPost("FinishTaskCheck")]
        public IActionResult FinishTaskCheck(ProjectPhasesTasks ProjectPhasesTasks)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);

            var result = _projectPhasesTasksservice.FinishTaskCheck(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);

            return Ok(result);
        }
        [HttpPost("ChangePriorityTask")]
        public IActionResult ChangePriorityTask(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.ChangePriorityTask(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G);

                return Ok(result);
            }
        [HttpPost("DeleteProjectPhasesTasks")]
        public IActionResult DeleteProjectPhasesTasks(int PhaseTaskId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.DeleteProjectPhasesTasks(PhaseTaskId, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpPost("DeleteProjectPhasesTasksNEW")]
        public IActionResult DeleteProjectPhasesTasksNEW(int PhaseTaskId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.DeleteProjectPhasesTasksNEW(PhaseTaskId, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpPost("ShowmanagerapprovalTask")]
        public IActionResult ShowmanagerapprovalTask(int PhaseTaskId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.ShowmanagerapprovalTask(PhaseTaskId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("AcceptmanagerapprovalTask")]
        public IActionResult AcceptmanagerapprovalTask(int PhaseTaskId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.AcceptmanagerapprovalTask(PhaseTaskId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("ConditionTaskStopR")]
        public IActionResult ConditionTaskStopR(int PhaseTaskId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.ConditionTaskStopR(PhaseTaskId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("VoucherTaskStop")]
        public IActionResult VoucherTaskStop(int PhaseTaskId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.VoucherTaskStop(PhaseTaskId, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpPost("VoucherTaskStopR")]
        public IActionResult VoucherTaskStopR(int PhaseTaskId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.VoucherTaskStopR(PhaseTaskId, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpPost("TransferTask")]
        public IActionResult TransferTask(int PhaseTaskId, int MainSelectId, int SubSelectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.TransferTask(PhaseTaskId, MainSelectId, SubSelectId, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpPost("ChangeTaskTime")]
        public IActionResult ChangeTaskTime(ProjectPhasesTasks ProjectPhasesTasks)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
                //var file = Server.MapPath("~/dist/assets/images/logo.png");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _projectPhasesTasksservice.ChangeTaskTime(ProjectPhasesTasks, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);

                return Ok(result );
            }
        [HttpGet("GetTaskById")]
        public IActionResult GetTaskById(int TaskId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.GetTaskById(TaskId, _globalshared.Lang_G, _globalshared.UserId_G);
                return Ok(result );
            }
        [HttpGet("GetTaskByUserId")]
        public IActionResult GetTaskByUserId(int TaskId, int UserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.GetTaskByUserId(TaskId, _globalshared.UserId_G);
                return Ok(result );
            }
        [HttpGet("FillProjectMainPhases")]
        public IActionResult FillProjectMainPhases(int param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.FillProjectMainPhases(param) );
            }
        [HttpGet("FillProjectSubPhases")]
        public IActionResult FillProjectSubPhases(int param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.FillProjectSubPhases(param) );
            }
        [HttpGet("GetAllProjectCurrentTasks")]
        public IActionResult GetAllProjectCurrentTasks(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetAllProjectCurrentTasks(ProjectId) );
            }
        [HttpGet("GetAllTasksByProjectId")]
        public IActionResult GetAllTasksByProjectId(int ProjectId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetAllTasksByProjectId(ProjectId, _globalshared.BranchId_G) );
            }
        [HttpGet("GetAllTasksByProjectIdW")]
        public IActionResult GetAllTasksByProjectIdW()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someTask = _projectPhasesTasksservice.GetAllTasksByProjectIdW(_globalshared.BranchId_G).Result.ToList();

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
        [HttpGet("GetAllTasksByProjectIdW_whithWotkOrder")]
        public IActionResult GetAllTasksByProjectIdW_whithWotkOrder()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                var wo = _workOrdersService.GetALlWorkOrderReport(_globalshared.Lang_G, _globalshared.BranchId_G).Result.Where(s => s.ProjectId != null);
            var someTask = _projectPhasesTasksservice.GetAllTasksByProjectIdW(_globalshared.BranchId_G).Result.Union(wo).ToList();

            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;
            //var result = new ContentResult
            //{
            //    Content = serializer.Serialize(someTask.Union(wo)),
            //    ContentType = "application/json"
            //};
            //return result;
            return Ok(someTask);
            }
        [HttpGet("GetAllTasksBySubPhase")]
        public IActionResult GetAllTasksBySubPhase(int SubPhaseId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.GetAllTasksBySubPhase(SubPhaseId);
                return Ok(result );
            }
        [HttpPost("MerigTasks")]
        public IActionResult MerigTasks(SettingDetailsData _settingDetailsData)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.MerigTasks(_settingDetailsData.TasksIdArray ?? new int[] { }, _settingDetailsData.Description ?? "", _settingDetailsData.Note ?? "", _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result );
            }
        [HttpGet("GetAllProjectTasksByPhaseId")]
        public IActionResult GetAllProjectTasksByPhaseId(int id)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.GetAllProjectTasksByPhaseId(id);
                return Ok(result );
            }

        [HttpGet("FillUsersTasksVacationSelect")]
        public IActionResult FillUsersTasksVacationSelect(int param)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_projectPhasesTasksservice.FillUsersTasksVacationSelect(param) );
            }
        [HttpPost("RestartTask")]
        public IActionResult RestartTask(int param,string RetrievedReason)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.RestartTask(param, RetrievedReason, _globalshared.UserId_G, _globalshared.BranchId_G) );
            }
        [HttpGet("UpdateIsNew")]
        public IActionResult UpdateIsNew(int TaskId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_projectPhasesTasksservice.UpdateIsNew(TaskId, _globalshared.UserId_G, _globalshared.BranchId_G) );
            }
        [HttpGet("UpdateprojectphaseRequirment")]
        public IActionResult UpdateprojectphaseRequirment(int projectphaseid, int projectgaolid)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.UpdateprojectphaseRequirment(projectphaseid, projectgaolid, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result );
            }
        //public IActionResult ExportReport()
        //{
        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine("C:/Users/saber.mahmoud/source/Bayanatech.Business/Bayanateck.TameerPro.DataModel/Reports/TasksReport.rpt"));
        //    var list = _projectPhasesTasksservice.GetTasksByUserId(2);
        //    if (list != null)
        //    {
        //        rd.SetDataSource(list);
        //    }
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return File(stream, "application/pdf", "Tasks_list.pdf");
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        [HttpGet("getreport")]
        public IActionResult getreport(ProjectPhasesTasksVM Search)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Users = _usersservice.GetAllUsers().Result;
                List<RptAllEmpPerformance> allEmpPerformances = new List<RptAllEmpPerformance>();
                if (Search.UserId == null || Search.UserId == 0)
                {
                    foreach (var usr in Users)
                    {
                        if (usr.UserId != 1)
                        {
                            Search.UserId = usr.UserId;
                            RptAllEmpPerformance rptAll = new RptAllEmpPerformance();
                            rptAll = _projectPhasesTasksservice.getempdata(Search, _globalshared.Lang_G, Con);
                            allEmpPerformances.Add(rptAll);
                        }
                    }
                }
                else
                {
                    RptAllEmpPerformance rptAll1 = new RptAllEmpPerformance();
                    rptAll1 = _projectPhasesTasksservice.getempdata(Search, _globalshared.Lang_G, Con);
                    allEmpPerformances.Add(rptAll1);

                }
                return Ok(allEmpPerformances );
            }
        [HttpPost("getreportNew")]
        public IActionResult getreportNew(PerformanceReportVM Search)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var AllUserBranch = 0;
            if (Search.AccBranchId == -1) AllUserBranch = 0;
            else AllUserBranch = _globalshared.BranchId_G;


            if (Search.BranchId == null)
                {
                    Search.BranchId = 0;
                }
                if (Search.UserId == null)
                {
                    Search.UserId = 0;
                }
                var FullReport = _projectPhasesTasksservice.getempdataNew_Proc(Search, _globalshared.Lang_G, Con, AllUserBranch).Result;
                return Ok(FullReport );
            }


        //public IActionResult PrintAllEmpTasksRpt(ProjectPhasesTasksVM Search)
        //{
        //HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        //int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G);
        //    var username = "";
        //    int datetype = 0;
        //    if (Search.UserId != null && Search.UserId != 0)
        //    {
        //        username = _usersservice.GetUserById((int)Search.UserId, _globalshared.Lang_G).FullName;
        //    }
        //    if (Search.ProTypeName != null && Search.ProTypeName != "")
        //    {
        //        datetype = int.Parse(Search.ProTypeName);
        //    }

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
        //    //var Users = _usersservice.GetAllUsers();
        //    //List<RptAllEmpPerformance> allEmpPerformances = new List<RptAllEmpPerformance>();
        //    //if (Search.UserId == null || Search.UserId == 0)
        //    //{
        //    //    foreach (var usr in Users)
        //    //    {
        //    //        Search.UserId = usr.UserId;
        //    //        RptAllEmpPerformance rptAll = new RptAllEmpPerformance();
        //    //        rptAll = _projectPhasesTasksservice.getempdata(Search, _globalshared.Lang_G, Con);
        //    //        allEmpPerformances.Add(rptAll);

        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    RptAllEmpPerformance rptAll1 = new RptAllEmpPerformance();
        //    //    rptAll1 = _projectPhasesTasksservice.getempdata(Search, _globalshared.Lang_G, Con);
        //    //    allEmpPerformances.Add(rptAll1);

        //    //}

        //    if (Search._globalshared.BranchId_G == null)
        //    {
        //        Search._globalshared.BranchId_G = 0;
        //    }
        //    if (Search.UserId == null)
        //    {
        //        Search.UserId = 0;
        //    }
        //    var FullReport = _projectPhasesTasksservice.getempdataNew(Search, _globalshared.Lang_G, Con, _globalshared.BranchId_G);

        //    ReportPDF = ProjectsReports.PrintAllEmpPerformancrRpt(FullReport, infoDoneTasksReport, Search.StartDate, Search.EndDate, username, datetype, (int)Search.TimeType);
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
        [HttpGet("GetAllProjectPhasesTasks_Costs")]
        public IActionResult GetAllProjectPhasesTasks_Costs(int? UserId, string? DateFrom, string? DateTo, int? BranchId, [FromQuery] List<int> BranchesList)
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


            var AllTasks = _projectPhasesTasksservice.GetAllProjectPhasesTasks_Costs(UserId, AllUserBranch, _globalshared.Lang_G, DateFrom, DateTo, AccBranchesList).Result.ToList();

            return Ok(AllTasks);
        }


        [HttpGet("GetTaskOperationsByTaskId")]
        public IActionResult GetTaskOperationsByTaskId(int PhasesTaskId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllTasks = _projectPhasesTasksservice.GetTaskOperationsByTaskId(PhasesTaskId);
            return Ok(AllTasks);
        }

        [HttpGet("GetnextTaskNo")]
        public ActionResult GetnextTaskNo()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectPhasesTasksservice.GenerateNextTaskNumber(_globalshared.BranchId_G,0);
            return Ok(result);
        }
        [HttpGet("GetTaskCode_S")]
        public IActionResult GetTaskCode_S()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectPhasesTasksservice.GetTaskCode_S(_globalshared.BranchId_G,0));
        }


    }
}
