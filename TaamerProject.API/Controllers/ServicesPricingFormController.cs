using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using static TaamerProject.API.Controllers.VoucherController;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ServicesPricingFormController : ControllerBase
    {
        private IServicesPricingFormService _servicesPricingFormService;
        private IBranchesService _BranchesService;
        private IOrganizationsService _organizationsservice;
        private ISystemSettingsService _systemSettingsService;
        public GlobalShared _globalshared;
        public ServicesPricingFormController(IServicesPricingFormService servicesPricingFormService
            , IBranchesService branchesService, IOrganizationsService organizationsservice
            , ISystemSettingsService systemSettingsService)
        {
            _servicesPricingFormService = servicesPricingFormService;
            _BranchesService = branchesService;
            _organizationsservice = organizationsservice;
            _systemSettingsService = systemSettingsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllPublicanddesignServicesPricingForms")]
        public IActionResult GetAllPublicanddesignServicesPricingForms(int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _servicesPricingFormService.GetAllPublicanddesignServicesPricingForms(Type,_globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetAllPublicanddesignServicesPricingFormstocount")]
        public IActionResult GetAllPublicanddesignServicesPricingFormstocount(int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _servicesPricingFormService.GetAllPublicanddesignServicesPricingFormstocount(Type, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FilterOffer")]
        public IActionResult FilterOffer(int Type, string DateSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _servicesPricingFormService.FilterPublicanddesignServicesPricingForms(Type, _globalshared.BranchId_G, DateSearch);
            return Ok(result);
        }
        [HttpPost("SavePricingForm")]
        public IActionResult SavePricingForm(ServicesPricingForm Form)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _servicesPricingFormService.SaveServicesPricingForm(Form, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeletePricingForm")]
        public IActionResult DeletePricingForm(int FormId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _servicesPricingFormService.DeleteServicesPricingForm(FormId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("UpdateStatusServicesPricingForm")]
        public IActionResult UpdateStatusServicesPricingForm(int FormId, bool status)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _servicesPricingFormService.UpdateStatusServicesPricingForm(FormId, status, _globalshared.UserId_G);
            return Ok(result);
        }
        [HttpGet("ChangeOfferGene_PDF")]
        public ActionResult ChangeOfferGene_PDF(int? FormId, int? TempCheck)
        {
            PriceForm_PDF priceForm_PDF = new PriceForm_PDF();
            if (FormId.HasValue)
            {
                HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                ServicesPricingFormVM FormVM = _servicesPricingFormService.GetServicesPricingFormById(FormId ?? 0, _globalshared.BranchId_G).Result;
                int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
                var objOrganization = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
                priceForm_PDF.Org_VD = objOrganization;


                var objBranch = _BranchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
                var OrgIsRequired = _systemSettingsService.GetSystemSettingsByBranchId(_globalshared.BranchId_G).Result.OrgDataIsRequired;


                priceForm_PDF.Branch_VD = objBranch;
                priceForm_PDF.FormVM_VD = FormVM;
                priceForm_PDF.OrgIsRequired_VD = OrgIsRequired;
                priceForm_PDF.TempCheck = TempCheck;
                priceForm_PDF.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            }
            else
            {
                priceForm_PDF.Branch_VD = null;
                priceForm_PDF.FormVM_VD = null;
                priceForm_PDF.OrgIsRequired_VD = null;
                priceForm_PDF.TempCheck = null;
                priceForm_PDF.DateTimeNow = null;

            }

            return Ok(priceForm_PDF);

            //if (TempCheck == 2)
            //{
            //    return PartialView("_Designquoterequest");
            //}
            //else
            //{
            //    return PartialView("_Generalquoterequest");
            //}
        }
        public class PriceForm_PDF
        {
            public ServicesPricingFormVM? FormVM_VD { get; set; }

            public OrganizationsVM? Org_VD { get; set; }
            public BranchesVM? Branch_VD { get; set; }
            public string? DateTimeNow { get; set; }
            public bool? OrgIsRequired_VD { get; set; }
            public int? TempCheck { get; set; }
        }
    }
}
