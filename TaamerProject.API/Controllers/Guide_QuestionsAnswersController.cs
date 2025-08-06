using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Guide_QuestionsAnswersController : ControllerBase
    {
        private IGuide_QuestionsAnswersService _Guide_Questions;
        private string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public Guide_QuestionsAnswersController(IGuide_QuestionsAnswersService guide_QuestionsAnswers, IConfiguration configuration)
        {
            _Guide_Questions = guide_QuestionsAnswers;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = configuration;
            Con = this.Configuration.GetConnectionString("DBConnection");
        }

        [HttpGet("GetAllQuestionAnswers")]
        public IActionResult GetAllQuestionAnswers()
        {
            return Ok(_Guide_Questions.GetAllquestionAnswers());
        }

        [HttpPost("SaveQuestionAnswers")]
        public IActionResult SaveQuestionAnswers(Guide_QuestionsAnswers questions)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Guide_Questions.SaveQuestionAnswers(questions, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteQuestionAnswers")]
        public IActionResult DeleteQuestionAnswers(int questionId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Guide_Questions.DeleteQuestions(questionId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

    }
}
