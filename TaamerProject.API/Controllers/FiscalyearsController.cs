using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class FiscalyearsController : ControllerBase
    {
        private IFiscalyearsService _fiscalyearsservice;
        private IAcc_EmpFinYearsService _fiscalyearsPrivservice;
        private string? Con;
        public GlobalShared _globalshared;
        private IConfiguration Configuration;

        public FiscalyearsController(IConfiguration _configuration,IFiscalyearsService fiscalyearsservice, IAcc_EmpFinYearsService fiscalyearsPrivservice)
        {
            _fiscalyearsservice = fiscalyearsservice;
            _fiscalyearsPrivservice = fiscalyearsPrivservice;

            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration;

            Con = this.Configuration.GetConnectionString("DBConnection");

        }
        [HttpGet("GetAllFiscalyears")]

        public ActionResult GetAllFiscalyears()
        {
            return Ok(_fiscalyearsservice.GetAllFiscalyears());
        }
        [HttpGet("GetAllFiscalyearsPriv")]

        public ActionResult GetAllFiscalyearsPriv()
        {
            return Ok(_fiscalyearsPrivservice.GetAllFiscalyearsPriv());
        }

        [HttpPost("SaveFiscalyears")]

        public ActionResult SaveFiscalyears([FromBody]FiscalYears fiscalyears)
        {
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
            var result = _fiscalyearsservice.SaveFiscalyears(fiscalyears, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveFiscalyearsPriv")]

        public ActionResult SaveFiscalyearsPriv(Acc_EmpFinYears fiscalyearsPriv)
        {
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
            var result = _fiscalyearsPrivservice.SaveFiscalyearsPriv(fiscalyearsPriv, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(new { result.Result});
        }

        [HttpGet("CheckPriv")]

        public int CheckPriv(int? EmpID_P, int? BranchID_P, int? YearID_P)
        {
            var result = _fiscalyearsPrivservice.CheckPriv(EmpID_P, BranchID_P, YearID_P);
            return result.Result;
        }

        [HttpPost("DeleteFiscalyears")]

        public ActionResult DeleteFiscalyears(int FiscalId)
        {
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            var result = _fiscalyearsservice.DeleteFiscalyears(FiscalId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteFiscalyearsPriv")]

        public ActionResult DeleteFiscalyearsPriv(int ID)
        {
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
            var result = _fiscalyearsPrivservice.DeleteFiscalyearsPriv(ID, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("ActivateFiscalYear")]

        public ActionResult ActivateFiscalYear(int FiscalId, int SystemSettingId)
        {
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            var result = _fiscalyearsservice.ActivateFiscalYear(FiscalId, SystemSettingId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetActiveYear")]

        public ActionResult GetActiveYear()
        {
            return Ok(_fiscalyearsservice.GetActiveYear());
        }
        [HttpGet("GetActiveYearID")]

        public ActionResult GetActiveYearID(int FiscalId)
        {
            return Ok(_fiscalyearsservice.GetActiveYearID(FiscalId));
        }
        [HttpGet("FillYearSelect")]

        public ActionResult FillYearSelect()
        {
            return Ok(_fiscalyearsservice.GetAllFiscalyears().Result.Select(s => new {
                Id = s.FiscalId,
                Name = s.YearId.ToString(),
            }));
        }

        [HttpGet("FillYearSelectFormat")]

        public ActionResult FillYearSelectFormat()
        {
            return Ok(_fiscalyearsservice.GetAllFiscalyears().Result.Select(s => new {
                Id = s.FiscalId,
                Name = s.YearId.ToString() + "-01" + "-01" + " / " + s.YearId.ToString() + "-12" + "-31",
            }));
        }
        [HttpPost("AccountRecycle")]

        public ActionResult AccountRecycle(int YearID)
        {
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            var result = _fiscalyearsPrivservice.AccountRecycle(YearID, Con, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(new { result });
        }
        [HttpPost("AccountRecycleCheckYear")]

        public int AccountRecycleCheckYear(int YearID)
        {
            var result = _fiscalyearsPrivservice.AccountRecycleCheckYear(YearID, Con, _globalshared.UserId_G, _globalshared.BranchId_G);
            return result;
        }
        [HttpPost("AccountRecycleDeleteYear")]

        public ActionResult AccountRecycleDeleteYear(int YearID)
        {
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            var result = _fiscalyearsPrivservice.AccountRecycleDeleteYear(YearID, Con, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillBranchByUserIdSelect")]

        public ActionResult FillBranchByUserIdSelect()
        {
            var result = _fiscalyearsPrivservice.GetAllBranchesByUserId(_globalshared.UserId_G).Result.Select(x => new
            {
                Id = x.BranchID,
                Name = x.Branchname
            });
            return Ok(result);
        }
        [HttpGet("FillYearByUserIdandBranchSelect")]

        public ActionResult FillYearByUserIdandBranchSelect(int? param)
        {
            var result = _fiscalyearsPrivservice.FillYearByUserIdandBranchSelect(_globalshared.UserId_G, param).Result.Select(x => new
            {
                Id = x.YearValue,
                Name = x.YearID
            });
            return Ok(result);
        }
    }
}
