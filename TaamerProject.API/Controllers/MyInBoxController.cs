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

    public class MyInBoxController : ControllerBase
    {
        private IUserMailsService _userMailsservice;
        public GlobalShared _globalshared;
        public MyInBoxController(IUserMailsService userMailsservice)
        {
            _userMailsservice = userMailsservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllUserMailsCount")]
        public IActionResult GetAllUserMailsCount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_userMailsservice.GetAllUserMailsCount(_globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllUserMailsSentCount")]
        public IActionResult GetAllUserMailsSentCount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_userMailsservice.GetAllUserMailsSentCount(_globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllUserMailsTrashCount")]
        public IActionResult GetAllUserMailsTrashCount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_userMailsservice.GetAllUserMailsTrashCount(_globalshared.UserId_G, _globalshared.BranchId_G));
        }
    }
}
