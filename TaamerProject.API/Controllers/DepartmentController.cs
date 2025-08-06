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

    public class DepartmentController : ControllerBase
    {
        private IDepartmentService _departmentservice;
        public GlobalShared _globalshared;
        public DepartmentController(IDepartmentService departmentservice)
        {
            _departmentservice = departmentservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllDepartments")]
        public IActionResult GetAllDepartments(string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_departmentservice.GetAllDepartment(SearchText ?? "", _globalshared.BranchId_G));
        }
        [HttpGet("GetAllDepartmentbyType")]
        public IActionResult GetAllDepartmentbyType(int? Type, string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_departmentservice.GetAllDepartmentbyType(Type, _globalshared.BranchId_G, SearchText ?? ""));
        }

        [HttpGet("GetAllDepartmentbyType2")]
        public IActionResult GetAllDepartmentbyType2(int? Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_departmentservice.GetAllDepartmentbyType(Type, _globalshared.BranchId_G,""));
        }
        [HttpPost("SaveDepartment")]
        public IActionResult SaveDepartment([FromBody]Department department)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _departmentservice.SaveDepartment(department, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteDepartment")]
        public IActionResult DeleteDepartment(int DepartmentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _departmentservice.DeleteDepartment(DepartmentId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillDepartmentSelect")]
        public IActionResult FillDepartmentSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_departmentservice.GetAllDepartment("", _globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.DepartmentId,
                Name = s.DepartmentNameAr
            }));
        }
        [HttpGet("FillExternalDepartmentSelect")]
        public IActionResult FillExternalDepartmentSelect()
        {
            return Ok(_departmentservice.GetExternalDepartment().Result.Select(s => new
            {
                Id = s.DepartmentId,
                Name = s.DepartmentNameAr
            }));
        }
        [HttpGet("FillDepartmentSelectByTypeUser")]
        public IActionResult FillDepartmentSelectByTypeUser(int? param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_departmentservice.GetAllDepartmentbyTypeUser(param, _globalshared.BranchId_G, "").Result.Select(s => new
            {
                Id = s.DepartmentId,
                Name = s.DepartmentNameAr
            }));
        }
        [HttpGet("FillDepartmentSelectByType")]
        public IActionResult FillDepartmentSelectByType(int? param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_departmentservice.GetAllDepartmentbyType(param, _globalshared.BranchId_G, "").Result.Select(s => new
            {
                Id = s.DepartmentId,
                Name = s.DepartmentNameAr
            }));
        }
        [HttpGet("FillDepartmentMultiSelect")]
        public IActionResult FillDepartmentMultiSelect(int? param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_departmentservice.GetAllDepartmentbyType(param, _globalshared.BranchId_G, "").Result.Where(t => t.Type == param).Select(s => new
            {
                Id = s.DepartmentId,
                Name = s.DepartmentNameAr
            }));
        }
        [HttpGet("GetDepartmentbyid")]
        public IActionResult GetDepartmentbyid(int DeptId)
        {
            return Ok(_departmentservice.GetDepartmentbyid(DeptId));
        }
    }
}
