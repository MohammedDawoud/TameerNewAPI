using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class NotificationPrivilegesController : ControllerBase
    {
        private readonly IUserNotificationPrivilegesService _NotifprivilegesService;
        private readonly IUsersService _usersservice;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;

        public NotificationPrivilegesController(IUserNotificationPrivilegesService NotifprivilegesService, IUsersService usersservice
           , IConfiguration _configuration)
        {
            _usersservice = usersservice;
            _NotifprivilegesService = NotifprivilegesService;
            Configuration = _configuration;Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }

        [HttpGet("GetNotifPrivilegesTree")]
        public IActionResult GetNotifPrivilegesTree()
        {
            var priv = NotificationPrivilages.NotificationPrivilage;
            if (priv != null && priv.Count() > 0)
            {
                List<AccountTreeVM> treeItems = new List<AccountTreeVM>();
                foreach (var item in priv)
                {
                    treeItems.Add(new AccountTreeVM(item.Id.ToString(), ((item.ParentId == 0 || item.ParentId == null) ? "#" : item.ParentId.ToString()), item.Name = item.Name));
                }
                return Ok(treeItems );
            }
            else
            {
                return Ok(new List<AccountTreeVM>());
            }
        }
        [HttpGet("GetUsersByNotifPrivilegesIds")]
        public IActionResult GetUsersByNotifPrivilegesIds(UserPrivileges Priv)
        {
            var priviledgeIds = _NotifprivilegesService.GetUsersByPrivilegesIds(Priv.PrivilegeId);
            return Ok(priviledgeIds);
        }

        [HttpPost("SaveUserNotifPriv")]
        public ActionResult SaveUserNotifPriv(PrivList privList)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _NotifprivilegesService.SaveUserPrivilegesUsers(privList.UserId??0, privList.PrivIds, _globalshared.UserId_G, _globalshared.BranchId_G, Con);
            return Ok(result );
        }
        [HttpPost("SaveGroupUsersNotifPriv")]
        public ActionResult SaveGroupUsersNotifPriv(PrivList privList)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _NotifprivilegesService.SaveGroupPrivilegesUsers(privList.GroupId??0, privList.PrivIds, _globalshared.UserId_G, _globalshared.BranchId_G, Con);
            return Ok(result);
        }
        [HttpGet("GetNotifPrivilegesIdsByUserId")]
        public IActionResult GetNotifPrivilegesIdsByUserId(int UserId)
        {
            return Ok(_NotifprivilegesService.GetPrivilegesIdsByUserId(UserId));
        }
    }
}
