using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AllowanceController : ControllerBase
    {
        private readonly IAllowanceService _allowanceservice;
        private readonly IAllowanceTypeService _allowanceTypeService;

        public GlobalShared _globalshared;

        public AllowanceController(IAllowanceService allowanceservice, IAllowanceTypeService allowanceTypeService)
        {
            _allowanceservice = allowanceservice;
            _allowanceTypeService = allowanceTypeService;
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllAllowances")]

        public IActionResult GetAllAllowances(int? EmpId, string? SearchText)
        {
            return Ok(_allowanceservice.GetAllAllowances(EmpId, SearchText ?? "", false));
        }
        [HttpGet("GetAllAllowancesSearch")]

        public IActionResult GetAllAllowancesSearch(AllowanceVM AllowanceSearch)
        {
            return Ok(_allowanceservice.GetAllAllowancesSearch(AllowanceSearch));
        }
        [HttpPost("SaveAllowance")]

        public IActionResult SaveAllowance(Allowance allowance)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _allowanceservice.SaveAllowance(allowance, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(Result);
        }
        [HttpGet("GetSalaryParts")]

        public IActionResult GetSalaryParts(int? EmpId)
        {
            EmpSalaryPartsVM salaryParts = GetSalaryPartObject(EmpId);
            return Ok(salaryParts);
        }

        [HttpGet("GetSalaryPartObject")]

        public EmpSalaryPartsVM GetSalaryPartObject(int? EmpId)
        {
            IEnumerable<AllowanceVM> allowances = null;
            if (EmpId.HasValue)
                allowances = _allowanceservice.GetAllAllowances(EmpId.Value, "", true).Result;

            EmpSalaryPartsVM salaryParts = new EmpSalaryPartsVM();
            var allawTypes = _allowanceTypeService.GetAllAllowancesTypes("", true).Result.Where(x => x.NameEn == "Housing allowance").FirstOrDefault();
            if (allawTypes != null)
            {
                salaryParts.HousingAllowance.AllowanceTypeId = allawTypes.AllowanceTypeId;
            }


            if (allowances != null && allowances.Count() > 0 && salaryParts.HousingAllowance.AllowanceTypeId.HasValue)
            {
                salaryParts.HousingAllowance = allowances.Where(x => x.AllowanceTypeId == salaryParts.HousingAllowance.AllowanceTypeId).Select(x => new Allowance
                {
                    AllowanceAmount = x.AllowanceAmount,
                    AllowanceId = x.AllowanceId,
                    AllowanceTypeId = x.AllowanceTypeId
                }).FirstOrDefault();
            }

            return salaryParts;
        }
        [HttpPost("SaveSalaryParts")]


        public IActionResult SaveSalaryParts(EmpSalaryPartsVM salaryParts)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_allowanceservice.SaveSalaryParts(salaryParts, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G));
        }
        [HttpPost("DeleteAllowance")]


        public IActionResult DeleteAllowance(int AllowanceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _allowanceservice.DeleteAllowance(AllowanceId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}
