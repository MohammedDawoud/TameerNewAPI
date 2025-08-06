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

    public class UserMailsController : ControllerBase
    {
        private IUserMailsService _userMailsservice;
        public GlobalShared _globalshared;
        public UserMailsController(IUserMailsService userMailsservice)
        {
            _userMailsservice = userMailsservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllUserMails")]
        public ActionResult GetAllUserMails()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_userMailsservice.GetAllUserMails(_globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllUnReadUserMails")]
        public IActionResult GetAllUnReadUserMails()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_userMailsservice.GetAllUnReadUserMails(_globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllUserMailsSent")]
        public IActionResult GetAllUserMailsSent()
        {
            return Ok(_userMailsservice.GetAllUserMailsSent(_globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllUserMailsTrash")]
        public IActionResult GetAllUserMailsTrash()
        {
            return Ok(_userMailsservice.GetAllUserMailsTrash(_globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpPost("SaveUserMails")]
        public IActionResult SaveUserMails(UserMails userMails)
        {
            var result = _userMailsservice.SaveUserMails(userMails, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteUserMails")]
        public IActionResult DeleteUserMails(int MailId)
        {
            var result = _userMailsservice.DeleteUserMails(MailId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("ReadUserMails")]
        public IActionResult ReadUserMails()
        {
            return Ok(_userMailsservice.ReadUserMails(_globalshared.UserId_G, _globalshared.BranchId_G));
        }
    }
}
