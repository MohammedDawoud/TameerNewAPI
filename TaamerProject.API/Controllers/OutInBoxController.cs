using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class OutInBoxController : ControllerBase
    {
        private IOutInBoxService _OutInBoxservice;
        private IOrganizationsService _organizationsservice;
        public GlobalShared _globalshared;
        public OutInBoxController(IOutInBoxService OutInBoxservice, IOrganizationsService organizationsservice)
        {
             
             _organizationsservice = organizationsservice;
             _OutInBoxservice =  OutInBoxservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

        }

        [HttpGet("GetAllOutInbox")]
        public IActionResult GetAllOutInbox(int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return  Ok(_OutInBoxservice.GetAllOutInbox(Type, _globalshared.BranchId_G) );
        }
        [HttpGet("GetAllOutInboxSearch")]
        public IActionResult GetAllOutInboxSearch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return  Ok(_OutInBoxservice.GetAllOutInboxSearch(_globalshared.BranchId_G) );
        }
        [HttpGet("GetOutInboxById")]
        public IActionResult GetOutInboxById(int OutInBoxId)
        {
            return  Ok(_OutInBoxservice.GetOutInboxById(OutInBoxId) );
        }
        [HttpGet("GetOutInboxFiles")]
        public IActionResult GetOutInboxFiles(int? Type, int? OutInType, int? ArchiveFileId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return  Ok(_OutInBoxservice.GetOutInboxFiles(Type, OutInType, ArchiveFileId, _globalshared.BranchId_G) );
        }
        [HttpGet("GetOutInboxProjectFiles")]
        public IActionResult GetOutInboxProjectFiles(int Type, int? ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return  Ok(_OutInBoxservice.GetOutInboxProjectFiles(Type, ProjectId, _globalshared.BranchId_G) );
        }
        [HttpPost("SaveOutInbox")]
        public IActionResult SaveOutInbox([FromBody]OutInBox Inbox)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _OutInBoxservice.SaveOutInbox(Inbox, _globalshared.UserId_G, _globalshared.BranchId_G);
         
            return  Ok(result);
        }
        [HttpPost("DeleteOutInBox")]
        public IActionResult DeleteOutInBox(int OutInBoxId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _OutInBoxservice.DeleteOutInBox(OutInBoxId, _globalshared.UserId_G, _globalshared.BranchId_G);  //"لا يمكن حذف لأنه مربوط بعمليه أخري"
             
            return  Ok(result);
        }
        [HttpGet("FillOutboxTypeSelect")]
        public IActionResult FillOutboxTypeSelect(int? param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return  Ok(_OutInBoxservice.FillOutboxTypeSelect(param, _globalshared.BranchId_G) );
        }
        [HttpGet("FillInboxTypeSelect")]
        public IActionResult FillInboxTypeSelect(int? param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return  Ok(_OutInBoxservice.FillInboxTypeSelect(param, _globalshared.BranchId_G) );
        }
        [HttpGet("SearchOutbox")]
        public IActionResult SearchOutbox(OutInBoxVM OutInBoxesSearch, string DateFrom, string DateTo)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return  Ok(_OutInBoxservice.SearchOutbox(OutInBoxesSearch, DateFrom, DateTo, _globalshared.BranchId_G) );
        }

        [HttpPost("SearchOutboxNew")]
        public async Task<IActionResult> SearchOutboxNew([FromBody]OutInBoxVM OutInBoxesSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var data =await _OutInBoxservice.SearchOutbox(OutInBoxesSearch, OutInBoxesSearch.DateFrom, OutInBoxesSearch.DateTo, _globalshared.BranchId_G);
            return Ok(data);
        }

        [HttpPost("SearchOutInbox")]
        public IActionResult SearchOutInbox([FromBody]OutInBoxVM OutInBoxesSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return  Ok(_OutInBoxservice.SearchOutInbox(OutInBoxesSearch, OutInBoxesSearch.DateFrom, OutInBoxesSearch.DateTo, _globalshared.BranchId_G) );
        }
        [HttpGet("GetAllOutboxByDateSearch")]
        public IActionResult GetAllOutboxByDateSearch(string DateFrom, string DateTo, int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return  Ok(_OutInBoxservice.GetAllOutboxByDateSearch(DateFrom, DateTo, _globalshared.BranchId_G, Type) );
        }
        [HttpGet("GetAllOutInboxByDateSearch")]
        public IActionResult GetAllOutInboxByDateSearch(string DateFrom, string DateTo)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return  Ok(_OutInBoxservice.GetAllOutInboxByDateSearch(DateFrom, DateTo, _globalshared.BranchId_G) );
        }
        [HttpGet("GetAllDeptsByOutInBoxId")]
        public IActionResult GetAllDeptsByOutInBoxId(int OutInboxId, int Type)
        {
            return  Ok(_OutInBoxservice.GetAllDeptsByOutInBoxId(OutInboxId, Type) );
        }





    }
}
