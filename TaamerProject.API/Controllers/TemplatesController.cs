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

    public class TemplatesController : ControllerBase
    {
        private ITemplatesService _templatesservice;
        private ITemplatesTasksService _templatestasksService;
        public GlobalShared _globalshared;
        public TemplatesController(ITemplatesService templatesservice, ITemplatesTasksService templatestasksService)
        {
            _templatesservice = templatesservice;
            _templatestasksService = templatestasksService;

            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllTemplates")]

        public IActionResult GetAllTemplates()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_templatesservice.GetAllTemplates(_globalshared.BranchId_G));
        }
        [HttpGet("GetAllTemplatesTasks")]

        public IActionResult GetAllTemplatesTasks()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_templatestasksService.GetAllTemplatesTasks(_globalshared.BranchId_G));
        }
        [HttpGet("GetAllTemplatesTasksByTemplateId")]

        public IActionResult GetAllTemplatesTasksByTemplateId(int TemplateId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_templatestasksService.GetAllTemplatesTasksByTemplateId(TemplateId, _globalshared.BranchId_G));
        }
        [HttpPost("SaveTemplates")]

        public IActionResult SaveTemplates(Templates templates)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _templatesservice.SaveTemplates(templates, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveTemplatesTasks")]

        public IActionResult SaveTemplatesTasks(TemplatesTasks templatesTasks)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _templatestasksService.SaveTemplatesTasks(templatesTasks, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteTemplates")]

        public IActionResult DeleteTemplates(int TemplateId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _templatesservice.DeleteTemplates(TemplateId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteTemplatestasks")]

        public IActionResult DeleteTemplatestasks(int TemplateTaskId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _templatestasksService.DeleteTemplatestasks(TemplateTaskId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}
