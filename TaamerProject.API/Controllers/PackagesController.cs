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

    public class PackagesController : ControllerBase
    {
        private IAcc_PackagesService _Packagesservice;
        public GlobalShared _globalshared;
        public PackagesController(IAcc_PackagesService packagesservice)
        {
            _Packagesservice = packagesservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllPackages")]
        public IActionResult GetAllPackages(string? SearchText)
        {
            return Ok(_Packagesservice.GetAllPackages(SearchText??""));
        }
        [HttpPost("SavePackage")]

        public IActionResult SavePackage(Acc_Packages Packages)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Packagesservice.SavePackage(Packages, _globalshared.UserId_G, _globalshared.BranchId_G);           
            return Ok(result);
        }
        [HttpPost("DeletePackage")]

        public IActionResult DeletePackage(int PackagesId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Packagesservice.DeletePackage(PackagesId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillPackagesSelect")]

        public IActionResult FillPackagesSelect(string SearchText = "")
        {
            return Ok(_Packagesservice.GetAllPackages(SearchText).Result.Select(s => new {
                Id = s.PackageId,
                Name = s.PackageName

            }));
        }
    }
}
