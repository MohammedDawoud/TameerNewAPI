using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class OfficialDocsAboutToExpireController : ControllerBase
    {
        private readonly IEmployeesService _EmpsService;
        string? Con;
        private IConfiguration Configuration;

        public GlobalShared _globalshared;
        public OfficialDocsAboutToExpireController(IEmployeesService empsService, IConfiguration _configuration)
        {
            _EmpsService = empsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");

        }
        [HttpGet("GetOfficialDocsAboutToExpire")]
        public IActionResult GetOfficialDocsAboutToExpire(int? DepartmentId, bool? Issort = false)
        {


            if (Issort == true)
            {
                var result = _EmpsService.GetOfficialDocsAboutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId);
                return Ok(result.Result.OrderBy(x=>x.ExpiredDate));
            }
            else
            {
                var result = _EmpsService.GetOfficialDocsAboutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId);
                return Ok(result.Result);

            }
        }

        [HttpGet("GetOfficialDocsAboutToExpire_paging")]
        public IActionResult GetOfficialDocsAboutToExpire_paging(int? DepartmentId, string? SearchText, int page = 1, int pageSize = 10, bool? Issort = false)
        {

            SearchText = SearchText ?? "";
            if (Issort == true)
            {
                var offic = _EmpsService.GetOfficialDocsAboutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Result.Where(x=>x.NameAr.Contains(SearchText) || x.Number.Contains(SearchText) || x.DocSource.Contains(SearchText) || x.Branch.Contains(SearchText) ||x.Notes.Contains(SearchText) || SearchText ==null || SearchText=="").OrderBy(x => x.ExpiredDate);
                var data = GeneratePagination<rptGetOfficialDocsAboutToExpire>.ToPagedList(offic.ToList(), page, pageSize);
                var result = new PagedLists<rptGetOfficialDocsAboutToExpire>(data.MetaData, data);
                return Ok(result);
            }
            else
            {
                var offic = _EmpsService.GetOfficialDocsAboutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Result.Where(x => x.NameAr.Contains(SearchText) || x.Number.Contains(SearchText) || x.DocSource.Contains(SearchText) || x.Branch.Contains(SearchText) || x.Notes.Contains(SearchText) || SearchText == null || SearchText == "") ;
                var data = GeneratePagination<rptGetOfficialDocsAboutToExpire>.ToPagedList(offic.ToList(), page, pageSize);
                var result = new PagedLists<rptGetOfficialDocsAboutToExpire>(data.MetaData, data);
                return Ok(result);

            }
        }
    }
}
