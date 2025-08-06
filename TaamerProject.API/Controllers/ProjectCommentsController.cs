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

    public class ProjectCommentsController : ControllerBase
    {
        private IProjectCommentsService _projCommentservice;
        public GlobalShared _globalshared;
        public ProjectCommentsController(IProjectCommentsService projCommentservice)
        {
            _projCommentservice = projCommentservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllProjectComments")]
        public IActionResult GetAllProjectComments(int ProjectId)
        {
            return Ok(_projCommentservice.GetAllProjectComments(ProjectId));
        }
        [HttpPost("SaveComment")]
        public IActionResult SaveComment(ProjectComments commentobj)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projCommentservice.SaveComment(commentobj, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteComment")]
        public IActionResult DeleteComment(int CommentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projCommentservice.DeleteComment(CommentId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}
