using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class LoanController : ControllerBase
    {
        private ILoanService _loanservice;
        private IOrganizationsService _organizationsservice;
        private readonly IFiscalyearsService _FiscalyearsService;
 
        public GlobalShared _globalshared;
        public LoanController( ILoanService loanservice,IOrganizationsService organizationsservice, IFiscalyearsService FiscalyearsService)
        {

             _loanservice = loanservice ;
             _organizationsservice = organizationsservice;
             _FiscalyearsService = FiscalyearsService;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }

        [HttpGet("GetAllLoansW")]
        public IActionResult GetAllLoansW(string SearchText)
        {
            return Ok(_loanservice.GetAllLoansW(SearchText ?? ""));
        }

        [HttpGet("GetAllLoansWithout")]
        public IActionResult GetAllLoansWithout()
        {
            var loan = _loanservice.GetAllLoansW("");
            return Ok(loan);
        }

        [HttpGet("GetAllLoansW2")]
        public IActionResult GetAllLoansW2(string? SearchText, int? status)
        {
            if (status == null)
            {
                status = 0;
            }
            return Ok(_loanservice.GetAllLoansW2(SearchText ?? "", (int)status) );
        }

        [HttpGet("GetAllLoans")]
        public IActionResult GetAllLoans(int? EmpId, string SearchText)
        {
            return Ok(_loanservice.GetAllLoans(EmpId, SearchText ?? "") );
        }

        [HttpGet("GetAllLoansE")]
        public IActionResult GetAllLoansE(int? EmpId, string? SearchText)
        {
            var loans = _loanservice.GetAllLoansE(EmpId, SearchText ?? "");
            return Ok(loans );
        }

        [HttpGet("GetAllLoansAfterDecision")]
        public IActionResult GetAllLoansAfterDecision(int? EmpId, string SearchText)
        {
            return Ok(_loanservice.GetAllLoansAfterDecision(EmpId, SearchText ?? "") );
        }

        [HttpGet("GetAllLoans2")]
        public IActionResult GetAllLoans2(string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_loanservice.GetAllLoans2( _globalshared.UserId_G, SearchText ?? "") );
        }

        [HttpGet("GetAllLoanDetails")]
        public IActionResult GetAllLoanDetails(int? LoanId)
        {
            var loandet = _loanservice.GetAllLoanDetails(LoanId);
            return Ok(loandet);
        }

        [HttpGet("GetAllLoanDetails2")]
        public IActionResult GetAllLoanDetails2(int LoanId)
        {
            var loandet = _loanservice.GetAllLoanDetails2(LoanId);
            return Ok(loandet);
        }

        [HttpPost("GetAllImprestSearch")]
        public IActionResult GetAllImprestSearch(LoanVM ImprestSearch)
         {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _loanservice.GetAllImprestSearch(ImprestSearch, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpGet("GetAmountPayedAndNotPayed")]
        public IActionResult GetAmountPayedAndNotPayed(int? LoanId)
        {
            return Ok(_loanservice.GetAmountPayedAndNotPayed(LoanId) );
        }

        [HttpPost("SaveLoan")]
        public IActionResult SaveLoan(Loan loan)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.Combine("/Email/MailStamp.html");

            //var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.Combine(org.Result?.LogoUrl??"");

            var result = _loanservice.SaveLoan(loan, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
           
            return Ok(result );
        }

        [HttpPost("SaveLoan_Management")]
        public IActionResult SaveLoan_Management(Loan loan)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.Combine("/Email/MailStamp.html");

            //var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.Combine(org.Result?.LogoUrl ?? "");
            var result = _loanservice.SaveLoan_Management(loan, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
            
            return Ok(result );
        }

        [HttpPost("SaveLoan2")]
        public IActionResult SaveLoan2(Loan loan)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.Combine("/Email/MailStamp.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.Combine(org.Result?.LogoUrl ?? "");

            var result = _loanservice.SaveLoan2(loan, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
            
            return Ok(result );
        }

        [HttpPost("SaveLoanWorkers")]
        public IActionResult SaveLoanWorkers(Loan loan)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.Combine("/Email/MailStamp.html");

            //var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.Combine(org.Result?.LogoUrl ?? "");

            var result = _loanservice.SaveLoanWorkers(loan, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
             
            return Ok(result );
        }
        [HttpGet("PayLoan")]
        public IActionResult PayLoan(int LoanDetailsId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _loanservice.PayLoan(LoanDetailsId, _globalshared.UserId_G, _globalshared.BranchId_G);
          
            return Ok(result );
        }
        [HttpPost("DeleteLoan")]
        public IActionResult DeleteLoan(int LoanId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _loanservice.DeleteLoan(LoanId, _globalshared.UserId_G, _globalshared.BranchId_G);
            
            return Ok(result);
        }
        [HttpPost("UpdateStatus")]
        public IActionResult UpdateStatus(int ImprestId, int Type,string? Reason)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //var url = Server.MapPath("/Email/MailStamp.html");
            var url = Path.GetFullPath("Email/MailStamp.html");

            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.GetFullPath(org.Result.LogoUrl.TrimStart('/'));

            var result = _loanservice.UpdateStatus(ImprestId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, Type, _globalshared.YearId_G, url, file,Reason??"");
            return Ok(result);
        }

        [HttpPost("UpdateDecisionType")]
        public IActionResult UpdateDecisionType(int ImprestId, int DecisionType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _loanservice.UpdateDecisionType(ImprestId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, DecisionType);
            return Ok(result );

        }
        [HttpPost("Updateconverttoaccounts")]
        public IActionResult Updateconverttoaccounts(int ImprestId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _loanservice.Updateconverttoaccounts(ImprestId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);

        }

    }
}
