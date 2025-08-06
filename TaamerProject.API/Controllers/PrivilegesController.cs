using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class PrivilegesController : ControllerBase
    {
        private readonly IPrivilegesService _privilegesService;
        private readonly IUsersService _usersservice;
        public GlobalShared _globalshared;
        public PrivilegesController(IPrivilegesService privilegesService, IUsersService usersservice)
        {
            _privilegesService = privilegesService;
            _usersservice = usersservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetPrivilegesTree")]
        public IActionResult GetPrivilegesTree()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //var lang = "ar";
            //if (lang != null)
            //{
            //    var cultureInfo = new CultureInfo(lang);
            //    //Thread.CurrentThread.CurrentUICulture = cultureInfo;
            //    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureInfo.Name);

            //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
            //    //var providerResultCulture = new ProviderCultureResult(cultureInfo.Name);


            //}



            var priv = Privileges.PrivilegesList;
            if (priv != null && priv.Count() > 0)
            {
                List<AccountTreeVM> treeItems = new List<AccountTreeVM>();
                foreach (var item in priv)
                {
                    treeItems.Add(new AccountTreeVM(item.Id.ToString(), ((item.ParentId == 0 || item.ParentId == null) ? "#" : item.ParentId.ToString()), item.Name = item.Name));
                }
                return Ok(treeItems); ;
            }
            else
            {
                return Ok(new List<AccountTreeVM>());
            }
        }
        [HttpPost("GetUsersByPrivilegesIds")]
        public IActionResult GetUsersByPrivilegesIds(UserPrivileges Priv)
        {
            var priviledgeIds = _privilegesService.GetUsersByPrivilegesIds(Priv.PrivilegeId).Result.ToList();
            return Ok(priviledgeIds);
        }
    }
}
