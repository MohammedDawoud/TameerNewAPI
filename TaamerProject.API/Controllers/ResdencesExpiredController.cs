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

    public class ResdencesExpiredController : ControllerBase
    {
        private readonly IEmployeesService _EmpsService;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public ResdencesExpiredController(IEmployeesService empsService, IConfiguration _configuration)
        {
            _EmpsService = empsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration;Con = this.Configuration.GetConnectionString("DBConnection");
        }
        [HttpGet("GetResDencesExpired")]
        public IActionResult GetResDencesExpired(int? DepartmentId)
        {
            var result = _EmpsService.GetResDencesExpired(Con??"", DepartmentId == 0 ? null : DepartmentId);
            return Ok(result);
        }

        [HttpGet("GetResDencesExpired_paging")]
        public IActionResult GetResDencesExpired_paging(int? DepartmentId, string? SearchText, int page = 1, int pageSize = 10)
        {
            SearchText = SearchText ?? "";
            var resd = _EmpsService.GetResDencesExpired(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Result.Where(x => x.NameAr.Contains(SearchText) || x.Nationality.Contains(SearchText) ||
            x.NationalId.Contains(SearchText) || x.Department.Contains(SearchText) || x.Branch.Contains(SearchText) || x.JobName.Contains(SearchText)||x.DirectManager.Contains(SearchText) || SearchText == null || SearchText == "").ToList();
            var data = GeneratePagination<rptGetResdencesExpiredVM>.ToPagedList(resd.ToList(), page, pageSize);
            var result = new PagedLists<rptGetResdencesExpiredVM>(data.MetaData, data);
            return Ok(result);
        }
    }
}
