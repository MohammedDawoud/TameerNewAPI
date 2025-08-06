using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
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

    public class WorkOrdersController : ControllerBase
    {
        private IWorkOrdersService _workOrdersservice;
        public GlobalShared _globalshared;
        private IOrganizationsService _organizationsservice;

        public WorkOrdersController(IWorkOrdersService workOrdersservice, IOrganizationsService organizationsService)
        {
            _workOrdersservice = workOrdersservice;
            this._organizationsservice = organizationsService;
            HttpContext httpContext = HttpContext;

        _globalshared = new GlobalShared(httpContext);
        }
        [HttpPost("SaveWorkOrder")]
        public IActionResult SaveWorkOrder([FromForm]string? WorkOrderId, [FromForm] string? OrderNo, [FromForm] string? OrderNoType, [FromForm] string? OrderDate
            , [FromForm] string? OrderHijriDate, [FromForm] string? ExecutiveEng, [FromForm] string? ResponsibleEng
            , [FromForm] string? CustomerId, [FromForm] string? Required, [FromForm] string? Note
            ,[FromForm] string? EndDate, [FromForm] string? WOStatus, [FromForm] string? AgentId, [FromForm] string? PhasePriority, [FromForm] string? ProjectId,
            [FromForm] List<IFormFile>? postedFiles)
        {

            WorkOrders workOrders = new WorkOrders();
            workOrders.WorkOrderId = Convert.ToInt32(WorkOrderId);
            if(workOrders.WorkOrderId==0)
            {
                if (OrderNo == null || OrderNo == "null" || OrderNo == "[object Object]")
                {
                    workOrders.OrderNo = _workOrdersservice.GenerateNextOrderNumber(_globalshared.BranchId_G,0).Result.ToString();
                }
                else
                {
                    workOrders.OrderNo = OrderNo;
                }
            }
            else
            {
                workOrders.OrderNo = OrderNo;
            }
            workOrders.OrderNoType = Convert.ToInt32(OrderNoType);
            workOrders.OrderDate = OrderDate;
            workOrders.OrderHijriDate = OrderHijriDate;
            workOrders.ExecutiveEng = Convert.ToInt32(ExecutiveEng);
            workOrders.ResponsibleEng = Convert.ToInt32(ResponsibleEng);
            workOrders.CustomerId = Convert.ToInt32(CustomerId);
            workOrders.Required = Required;
            workOrders.Note = Note;
            workOrders.EndDate = EndDate;
            workOrders.WOStatus = Convert.ToInt32(WOStatus);
            workOrders.AgentId = AgentId==null?"": AgentId == "null" ? "": AgentId;
            workOrders.PhasePriority = PhasePriority == null ? 0 : PhasePriority == "null" ? 0 : Convert.ToInt32(PhasePriority ?? "0"); ;
            if (ProjectId!=null)
                workOrders.ProjectId = Convert.ToInt32(ProjectId);


            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string path = Path.Combine("Uploads/ProjectRequirements/");
            string pathW = Path.Combine("/Uploads/ProjectRequirements/");

            string fileName = "";string fname = "";string fnamepath = "";

            if(postedFiles!=null)
            {
                foreach (IFormFile postedFile in postedFiles)
                {
                    fname = postedFile.FileName;
                    fileName = System.IO.Path.GetFileName(GenerateRandomNo() + fname);
                    fnamepath = Path.Combine(path, fileName);

                    using (System.IO.FileStream stream = new System.IO.FileStream(fnamepath, System.IO.FileMode.Create))
                    {

                        postedFile.CopyTo(stream);
                        string atturl = Path.Combine(path, fname);
                        workOrders.AttatchmentUrl = "/Uploads/ProjectRequirements/" + fileName;
                    }
                }
            }
            var result = _workOrdersservice.SaveWorkOrders(workOrders, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveWorkOrderFile")]
        public IActionResult SaveWorkOrderFile([FromForm] string? WorkOrderId,[FromForm] List<IFormFile>? postedFiles)
        {

            WorkOrders workOrders = new WorkOrders();
            workOrders.WorkOrderId = Convert.ToInt32(WorkOrderId);

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string path = Path.Combine("Uploads/ProjectRequirements/");
            string pathW = Path.Combine("/Uploads/ProjectRequirements/");

            string fileName = ""; string fname = ""; string fnamepath = "";

            if (postedFiles != null)
            {
                foreach (IFormFile postedFile in postedFiles)
                {
                    fname = postedFile.FileName;
                    fileName = System.IO.Path.GetFileName(GenerateRandomNo() + fname);
                    fnamepath = Path.Combine(path, fileName);

                    using (System.IO.FileStream stream = new System.IO.FileStream(fnamepath, System.IO.FileMode.Create))
                    {

                        postedFile.CopyTo(stream);
                        string atturl = Path.Combine(path, fname);
                        workOrders.AttatchmentUrl = "/Uploads/ProjectRequirements/" + fileName;
                    }
                }
            }
            var result = _workOrdersservice.EditWorkOrdersFile(workOrders, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteWorkOrder")]
        public IActionResult DeleteWorkOrder(int WorkOrderId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _workOrdersservice.DeleteWorkOrders(WorkOrderId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SearchWorkOrders")]
        public IActionResult SearchWorkOrders(WorkOrdersVM WorkOrdersSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.SearchWorkOrders(WorkOrdersSearch, _globalshared.Lang_G, _globalshared.BranchId_G).Result);
        }
        [HttpGet("GetAllWorkOrders")]
        public IActionResult GetAllWorkOrders()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetAllWorkOrders(_globalshared.BranchId_G, _globalshared.UserId_G).Result);
        }

        [HttpGet("GetAllWorkOrdersFilterd")]
        public IActionResult GetAllWorkOrdersFilterd(int? CustomerId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetAllWorkOrdersFilterd(_globalshared.BranchId_G, _globalshared.UserId_G, CustomerId).Result);
        }

        [HttpGet("GetAllWorkOrdersFilterd_Paging")]
        public IActionResult GetAllWorkOrdersFilterd_Paging(int? CustomerId,string? Searchtext, int page = 1, int pageSize = 10)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Wo=_workOrdersservice.GetAllWorkOrdersFilterd(_globalshared.BranchId_G, _globalshared.UserId_G, CustomerId, Searchtext).Result;
            var data = GeneratePagination<WorkOrdersVM>.ToPagedList(Wo.ToList(), page, pageSize);
            var result = new PagedLists<WorkOrdersVM>(data.MetaData, data);
            return Ok(result);
        }
        [HttpGet("GetAllWorkOrdersyProjectId")]

        public IActionResult GetAllWorkOrdersyProjectId(int ProjectId)
        {
            return Ok(_workOrdersservice.GetAllWorkOrdersyProjectId(ProjectId));
        }
        [HttpGet("GetMaxOrderNumber")]
        public IActionResult GetMaxOrderNumber()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Value = _workOrdersservice.GetMaxOrderNumber(_globalshared.BranchId_G).Result;
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Value.ToString() };
            return Ok(generatevalue);
        }
        [HttpGet("GetAllWorkOrdersByDateSearch")]
        public IActionResult GetAllWorkOrdersByDateSearch(string DateFrom, string DateTo)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetAllWorkOrdersByDateSearch(DateFrom, DateTo, _globalshared.BranchId_G, _globalshared.UserId_G));
        }
        [HttpGet("GetWorkOrderById")]
        public IActionResult GetWorkOrderById(int WorkOrderId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetWorkOrderById(WorkOrderId, _globalshared.Lang_G));
        }
        [HttpGet("GetLateWorkOrdersByUserId")]
        public IActionResult GetLateWorkOrdersByUserId(string EndDateP)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetLateWorkOrdersByUserId(EndDateP, _globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetLateWorkOrdersByUserIdFilterd")]
        public IActionResult GetLateWorkOrdersByUserIdFilterd(string EndDateP,int? CustomerId,int? ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetLateWorkOrdersByUserIdFilterd(EndDateP, _globalshared.UserId_G, _globalshared.BranchId_G, CustomerId, ProjectId));
        }



        [HttpGet("GetLateWorkOrdersByUserIdFilterd_paging")]
        public IActionResult GetLateWorkOrdersByUserIdFilterd_paging(string? EndDateP, int? CustomerId, int? ProjectId, string? Searchtext, int page = 1, int pageSize = 10)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var WO=_workOrdersservice.GetLateWorkOrdersByUserIdFilterd(EndDateP??"", _globalshared.UserId_G, _globalshared.BranchId_G, CustomerId, ProjectId, Searchtext).Result;
            var data = GeneratePagination<WorkOrdersVM>.ToPagedList(WO.ToList(), page, pageSize);
            var result = new PagedLists<WorkOrdersVM>(data.MetaData, data);
            return Ok(result);
        }


        //---------------------------------------------------------------------------------
        [HttpPost("ConvertUserTasksSome")]
        public IActionResult ConvertUserTasksSome(int WorkOrderId, int FromUserId, int ToUserId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _workOrdersservice.ConvertUserTasksSome(WorkOrderId, FromUserId, ToUserId, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);
            return Ok(result);
        }

        [HttpPost("RequestConvertTask")]
        public IActionResult RequestConvertTask(WorkOrders WorkOrders)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _workOrdersservice.RequestConvertTask(WorkOrders, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);
            return Ok(result);
        }

        [HttpPost("PlayPauseTask")]
        public IActionResult PlayPauseTask(WorkOrders WorkOrders)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _workOrdersservice.PlayPauseTask(WorkOrders, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpPost("PlustimeTask")]
        public IActionResult PlustimeTask(WorkOrders WorkOrders)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _workOrdersservice.PlustimeTask(WorkOrders, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);

            return Ok(result);
        }
        [HttpPost("RefusePlustimeTask")]
        public IActionResult RefusePlustimeTask(WorkOrders WorkOrders)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _workOrdersservice.RefusePlustimeTask(WorkOrders, _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpPost("ChangeTaskTime")]
        public IActionResult ChangeTaskTime(WorkOrders WorkOrders)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
            //var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _workOrdersservice.ChangeTaskTime(WorkOrders, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);

            return Ok(result);
        }
        [HttpPost("ConvertTask")]
        public IActionResult ConvertTask(WorkOrders WorkOrders)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/End Task.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _workOrdersservice.ConvertTask(WorkOrders, _globalshared.UserId_G, _globalshared.BranchId_G, url, file);
            return Ok(result);
        }
        [HttpPost("SaveTaskLongDesc")]
        public IActionResult SaveTaskLongDesc(int WorkOrderId, string taskLongDesc)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _workOrdersservice.SaveTaskLongDesc(WorkOrderId, taskLongDesc, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        //---------------------------------------------------------------------------------
        [HttpGet("GetNewWorkOrdersByUserId")]
        public IActionResult GetNewWorkOrdersByUserId(string EndDateP)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetNewWorkOrdersByUserId(EndDateP, _globalshared.UserId_G, _globalshared.BranchId_G));
        }

        [HttpGet("GetNewWorkOrdersByUserIdFilterd")]
        public IActionResult GetNewWorkOrdersByUserIdFilterd(string EndDateP, int? CustomerId, int? ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetNewWorkOrdersByUserIdFilterd(EndDateP, _globalshared.UserId_G, _globalshared.BranchId_G, CustomerId, ProjectId));
        }

        [HttpGet("GetNewWorkOrdersByUserIdFilterd_paging")]
        public IActionResult GetNewWorkOrdersByUserIdFilterd_paging(string? EndDateP, int? CustomerId, int? ProjectId, string? Searchtext, int page = 1, int pageSize = 10)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var WO=_workOrdersservice.GetNewWorkOrdersByUserIdFilterd(EndDateP??"", _globalshared.UserId_G, _globalshared.BranchId_G, CustomerId, ProjectId,Searchtext).Result;
            var data = GeneratePagination<WorkOrdersVM>.ToPagedList(WO.ToList(), page, pageSize);
            var result = new PagedLists<WorkOrdersVM>(data.MetaData, data);
            return Ok(result);
        }

        [HttpGet("GetWorkOrdersByUserId")]
        public IActionResult GetWorkOrdersByUserId()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetWorkOrdersByUserId(_globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetWorkOrdersByUserIdCount")]
        public int GetWorkOrdersByUserIdCount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _workOrdersservice.GetWorkOrdersByUserId(_globalshared.UserId_G, _globalshared.BranchId_G).Result.Count();
            return result;
        }
        [HttpGet("GetWorkOrdersFilterationByUserId")]
        public IActionResult GetWorkOrdersFilterationByUserId(int flag, string? startdate, string? enddate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetDayWeekMonth_Orders(_globalshared.UserId_G, 0, _globalshared.BranchId_G, flag, startdate, enddate));

        }
        [HttpPost("FinishOrder")]

        public IActionResult FinishOrder(WorkOrders workOrders)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _workOrdersservice.FinishOrder(workOrders, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetWorkOrdersByUserIdandtask")]
        public IActionResult GetWorkOrdersByUserIdandtask(string task)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetWorkOrdersByUserIdandtask(task, _globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetWorkOrdersByUserIdandtask2")]
        public IActionResult GetWorkOrdersByUserIdandtask2(string task)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetWorkOrdersByUserIdandtask2(task, _globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetWorkOrdersBytask")]
        public IActionResult GetWorkOrdersBytask(string task)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetWorkOrdersBytask(task, _globalshared.BranchId_G,_globalshared.UserId_G));
        }
        [HttpGet("GetDayWeekMonth_Orders")]
        public int GetDayWeekMonth_Orders(int flag, string? startdate, string? enddate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _workOrdersservice.GetDayWeekMonth_Orders(_globalshared.UserId_G, 0, _globalshared.BranchId_G, flag, startdate, enddate).Result.Count();
            return result;
        }
        [HttpGet("GetTaskOperationsByTaskId")]
        public IActionResult GetTaskOperationsByTaskId(int WorkOrderId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllTasks = _workOrdersservice.GetTaskOperationsByTaskId(WorkOrderId);
            return Ok(AllTasks);
        }
        [HttpGet("GetnextOrderNo")]
        public ActionResult GetnextTaskNo()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _workOrdersservice.GenerateNextOrderNumber(_globalshared.BranchId_G, 0);
            return Ok(result);
        }
        [HttpGet("GetOrderCode_S")]
        public IActionResult GetTaskCode_S()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_workOrdersservice.GetOrderCode_S(_globalshared.BranchId_G, 0));
        }

        [HttpPost("GenerateRandomNo")]
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}
