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

    public class CustodyController : ControllerBase
    {
        private ICustodyService _custodyservice;
        private IOrganizationsService _organizationsService;
        public GlobalShared _globalshared;
        public CustodyController(ICustodyService custodyservice, IOrganizationsService organizationsService)
        {
            _custodyservice = custodyservice;
            _organizationsService = organizationsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllCustody")]
        public IActionResult GetAllCustody()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_custodyservice.GetAllCustody(_globalshared.BranchId_G));
        }
        [HttpGet("FillCustodySelect")]
        public IActionResult FillCustodySelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_custodyservice.FillCustodySelect(_globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpPost("SearchCustody")]
        public IActionResult SearchCustody(CustodyVM CustodySearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_custodyservice.SearchCustody(CustodySearch, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpPost("SearchCustodyVoucher")]
        public IActionResult SearchCustodyVoucher(CustodyVM CustodySearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_custodyservice.SearchCustodyVoucher(CustodySearch, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetSomeCustody")]
        public IActionResult GetSomeCustody(bool Status)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var cut = _custodyservice.GetSomeCustody(_globalshared.BranchId_G, Status);
            return Ok(cut);
        }
        [HttpGet("GetSomeCustodyVoucher")]
        public IActionResult GetSomeCustodyVoucher(bool Status)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_custodyservice.GetSomeCustodyVoucher(_globalshared.BranchId_G, Status));
        }

        [HttpPost("SaveCustody")]
        public IActionResult SaveCustody(Custody custody)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //var url = Server.MapPath("~/Email/MailStamp.html");
            //var path = Path.GetFullPath("");
            var url = Path.GetFullPath("Email/MailStamp.html");

            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsService.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.GetFullPath(org.Result.LogoUrl.TrimStart('/'));

            var result = _custodyservice.SaveCustody(custody, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
            return Ok(result);
        }
        [HttpPost("SaveCustodyVoucher")]
        public IActionResult SaveCustodyVoucher(Custody custody)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _custodyservice.SaveCustodyVoucher(custody, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpGet("GetEmployeeByItem")]
        public IActionResult GetEmployeeByItem(int ItemId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_custodyservice.GetEmployeeByItemId(ItemId, _globalshared.BranchId_G));
        }
        [HttpPost("DeleteCustody")]
        public IActionResult DeleteCustody(int CustodyId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _custodyservice.DeleteCustody(CustodyId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("FreeCustody")]
        public IActionResult FreeCustody(int CustodyId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.GetFullPath("Email/MailStamp.html");


            //var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsService.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.GetFullPath(org.Result.LogoUrl.TrimStart('/'));

            var result = _custodyservice.FreeCustody(CustodyId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
            return Ok(result);
        }
        [HttpPost("ConvertStatusCustody")]
        public IActionResult ConvertStatusCustody(int CustodyId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _custodyservice.ConvertStatusCustody(CustodyId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpPost("ReturnConvetCustody")]
        public IActionResult ReturnConvetCustody(int CustodyId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _custodyservice.ReturnConvetCustody(CustodyId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
    }
}
