using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CustomerPaymentsController : ControllerBase
    {

        private ICustomerPaymentsService _customerPaymentsservice;
        private readonly IFiscalyearsService _FiscalyearsService;
        public GlobalShared _globalshared;
        public CustomerPaymentsController(ICustomerPaymentsService customerPaymentsservice, IFiscalyearsService fiscalyearsService)
        {
            _customerPaymentsservice = customerPaymentsservice;
            _FiscalyearsService = fiscalyearsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("PaymentReceiptVoucher")]
        public IActionResult PaymentReceiptVoucher(int PaymentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var PaymentReceiptVoucherVM = _customerPaymentsservice.GetPaymentReceipVoucher(PaymentId, _globalshared.BranchId_G);
            return Ok(PaymentReceiptVoucherVM);
        }
        [HttpGet("GetAllCustomerPayments")]
        public IActionResult GetAllCustomerPayments(int ContractId)
        {
            return Ok(_customerPaymentsservice.GetAllCustomerPayments(ContractId));
        }
        [HttpGet("GetAllCustomerPaymentsboffer")]
        public IActionResult GetAllCustomerPaymentsboffer(int OfferId)
        {
            return Ok(_customerPaymentsservice.GetAllCustomerPaymentsboffer(OfferId));
        }
        [HttpPost("SaveCustomerPayment")]
        public IActionResult SaveCustomerPayment(CustomerPayments customerPayments)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _customerPaymentsservice.SaveCustomerPayment(customerPayments, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("PayPayment")]
        public IActionResult PayPayment(CustomerPayments customerPayments)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _customerPaymentsservice.PayPayment(customerPayments, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("DeleteCustomerPayment")]
        public IActionResult DeleteCustomerPayment(int PaymentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _customerPaymentsservice.DeleteCustomerPayment(PaymentId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GenerateCustPaymentNumber")]
        public IActionResult GenerateCustPaymentNumber(int ContractId)
        {
            var NewValue=_customerPaymentsservice.GenerateCustPaymentNumber(ContractId);
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = NewValue.ToString() };
            return Ok(generatevalue);
        }
        [HttpPost("CancelPayment")]
        public IActionResult CancelPayment(int PaymentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _customerPaymentsservice.CancelPayment(PaymentId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}
