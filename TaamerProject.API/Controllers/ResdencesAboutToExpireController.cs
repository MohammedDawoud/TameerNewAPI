using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ResdencesAboutToExpireController : ControllerBase
    {
        private readonly IEmployeesService _EmpsService;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public ResdencesAboutToExpireController(IEmployeesService empsService, IConfiguration _configuration)
        {
            _EmpsService = empsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
        }
        [HttpGet("GetResDencesAbouutToExpire")]
        public IActionResult GetResDencesAbouutToExpire(int? DepartmentId,bool? Issort=false)
        {


            if (Issort == true)
            {
                var result = _EmpsService.GetResDencesAbouutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId);
                return Ok(result.Result.OrderBy(x=>x.NationalIdEndDate));
            }
            else
            {
                var result = _EmpsService.GetResDencesAbouutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Result;
                return Ok(result);
            }
            
        }


    

    [HttpGet("GetResDencesAbouutToExpire_paging")]
    public IActionResult GetResDencesAbouutToExpire_paging(int? DepartmentId, string? SearchText, int page = 1, int pageSize = 10, bool? Issort = false)
    {


        if (Issort == true)
        {
                if (SearchText == null || SearchText == "")
                {
                    var resd = _EmpsService.GetResDencesAbouutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Result.OrderBy(x => x.NationalIdEndDate);
                    //return Ok(result.Result.OrderBy(x => x.NationalIdEndDate));
                    var data = GeneratePagination<rptGetResdencesAboutToExpireVM>.ToPagedList(resd.ToList(), page, pageSize);
                    var result2 = new PagedLists<rptGetResdencesAboutToExpireVM>(data.MetaData, data);
                    return Ok(result2);
                }
                else
                {
                    var resd = _EmpsService.GetResDencesAbouutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Result.Where(x => x.NameAr.Contains(SearchText) || x.Nationality.Contains(SearchText) ||
         x.NationalId.Contains(SearchText) || x.Department.Contains(SearchText) || x.Branch.Contains(SearchText) || x.JobName.Contains(SearchText) || SearchText == null || SearchText == "").OrderBy(x => x.NationalIdEndDate);
                    //return Ok(result.Result.OrderBy(x => x.NationalIdEndDate));
                    var data = GeneratePagination<rptGetResdencesAboutToExpireVM>.ToPagedList(resd.ToList(), page, pageSize);
                    var result2 = new PagedLists<rptGetResdencesAboutToExpireVM>(data.MetaData, data);
                    return Ok(result2);
                }
            }
        else
        {
                if (SearchText == null || SearchText == "")
                {
                    var resd = _EmpsService.GetResDencesAbouutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Result;
                    var data = GeneratePagination<rptGetResdencesAboutToExpireVM>.ToPagedList(resd.ToList(), page, pageSize);
                    var result2 = new PagedLists<rptGetResdencesAboutToExpireVM>(data.MetaData, data);
                    return Ok(result2);
                }
                else
                {
                    var resd = _EmpsService.GetResDencesAbouutToExpire(Con ?? "", DepartmentId == 0 ? null : DepartmentId).Result.Where(x => x.NameAr.Contains(SearchText) || x.Nationality.Contains(SearchText) ||
                   x.NationalId.Contains(SearchText) || x.Department.Contains(SearchText) || x.Branch.Contains(SearchText) || x.JobName.Contains(SearchText) || SearchText == null || SearchText == ""); ;
                    var data = GeneratePagination<rptGetResdencesAboutToExpireVM>.ToPagedList(resd.ToList(), page, pageSize);
                    var result2 = new PagedLists<rptGetResdencesAboutToExpireVM>(data.MetaData, data);
                    return Ok(result2);
                }
        }

    }
}
}
