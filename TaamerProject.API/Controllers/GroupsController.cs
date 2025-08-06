using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class GroupsController : ControllerBase
    {
        private IGroupsService _groupsservice;
        private IGroupPrivilegeService _GroupPrivilegeService;
        private string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public GroupsController(IGroupsService groupsservice, IGroupPrivilegeService groupPrivilegeService, IConfiguration configuration)
        {
            _groupsservice = groupsservice;
            _GroupPrivilegeService = groupPrivilegeService;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = configuration;
            Con = this.Configuration.GetConnectionString("DBConnection");
        }
        [HttpGet("GetAllGroups")]
        public IActionResult GetAllGroups()
        {
            return Ok(_groupsservice.GetAllGroups());
        }
        [HttpGet("GetAllGroups_S")]
        public IActionResult GetAllGroups_S(string? SearchText)
        {
            return Ok(_groupsservice.GetAllGroups_S(SearchText??""));
        }
        [HttpGet("FillGroupsSelect")]
        public IActionResult FillGroupsSelect()
        {
            return Ok(_groupsservice.GetAllGroups().Result.Select(s => new {
                Id = s.GroupId,
                Name = s.NameAr
            }));
        }
        [HttpPost("SaveGroups")]
        public IActionResult SaveGroups(Groups groups)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _groupsservice.SaveGroups(groups, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteGroups")]
        public IActionResult DeleteGroups(int GroupId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _groupsservice.DeleteGroups(GroupId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetGroupById")]
        public IActionResult GetGroupById(int GroupId)
        {
            return Ok(_groupsservice.GetGroupById(GroupId));
        }
        [HttpPost("SaveGroupPriv")]
        public IActionResult SaveGroupPriv(PrivList privList)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _GroupPrivilegeService.SaveUserPrivilegesGroups(privList.GroupId ?? 0, privList.PrivIds, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveGroupPriv2")]
        public IActionResult SaveGroupPriv2(PrivList privList)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _GroupPrivilegeService.SaveUserPrivilegesGroups2(privList.GroupId??0, privList.PrivIds, _globalshared.UserId_G, _globalshared.BranchId_G, Con);
            return Ok(result);
        }
        [HttpGet("GetPrivilegesIdsByGroupId")]
        public IActionResult GetPrivilegesIdsByGroupId(int GroupId)
        {
            return Ok(_GroupPrivilegeService.GetPrivilegesIdsByGroupId(GroupId));
        }
    }
}
