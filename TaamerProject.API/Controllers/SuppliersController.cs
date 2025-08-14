using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class SuppliersController : ControllerBase
    {
        private IAcc_SuppliersService _Acc_SuppliersService;
        private readonly IFiscalyearsService _FiscalyearsService;
        public GlobalShared _globalshared;
        private ISystemSettingsService _systemSettingsService;
        private string? Con;
        private IConfiguration Configuration;
        public SuppliersController(IAcc_SuppliersService acc_SuppliersService, IConfiguration _configuration,
            ISystemSettingsService systemSettingsService, IFiscalyearsService fiscalyearsService)
        {
            _Acc_SuppliersService = acc_SuppliersService;
            _FiscalyearsService = fiscalyearsService;
            this._systemSettingsService = systemSettingsService;
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");

            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllSuppliers")]

        public IActionResult GetAllSuppliers(string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Acc_SuppliersService.GetAllSuppliers(SearchText??"", _globalshared.BranchId_G, _globalshared.YearId_G).Result);
        }
        [HttpPost("SaveSupplier")]

        public IActionResult SaveSupplier(Acc_Suppliers Supplier)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Acc_SuppliersService.SaveSupplier(Supplier, _globalshared.UserId_G, _globalshared.BranchId_G);
            var result2 = _systemSettingsService.MaintenanceFunc(Con, _globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.UserId_G, 0);
            return Ok(result);
        }
        [HttpPost("DeleteSupplier")]
        public IActionResult DeleteSupplier(int SupplierId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Acc_SuppliersService.DeleteSupplier(SupplierId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillSuppliersSelect")]

        public IActionResult FillSuppliersSelect(string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Acc_SuppliersService.GetAllSuppliers(SearchText??"", _globalshared.BranchId_G, _globalshared.YearId_G).Result.Select(s => new {
                Id = s.SupplierId,
                Name = s.NameAr
            }));
        }

        [HttpGet("FillSuppliersSelect2")]
        public IActionResult FillSuppliersSelect2()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Acc_SuppliersService.GetAllSuppliers("", _globalshared.BranchId_G, _globalshared.YearId_G).Result.Select(s => new {
                Id = s.SupplierId,
                Name = s.NameAr
            }));
        }
        [HttpGet("FillSuppliersAllNotiSelect")]
        public IActionResult FillSuppliersAllNotiSelect2()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Acc_SuppliersService.GetAllSuppliersAllNoti("", _globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.YearId_G).Select(s => new {
                Id = s.SupplierId,
                Name = s.NameAr
            }));
        }
        [HttpGet("GetTaxNoBySuppId")]

        public IActionResult GetTaxNoBySuppId(int SupplierId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Accounts = _Acc_SuppliersService.GetTaxNoBySuppId(SupplierId, _globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(Accounts);
        }
        [HttpGet("GetAccIdBySuppId")]

        public IActionResult GetAccIdBySuppId(int SupplierId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Accounts = _Acc_SuppliersService.GetAccIdBySuppId(SupplierId, _globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(Accounts);
        }
        [HttpGet("GetSuppIdByAccId")]

        public IActionResult GetSuppIdByAccId(int AccountId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Accounts = _Acc_SuppliersService.GetSuppIdByAccId(AccountId, _globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(Accounts);
        }


    }
}
