using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProjectPiecesController : ControllerBase
    {
        private readonly IProjectPiecesService _ProjectPiecesservice;
        public GlobalShared _globalshared;

        public ProjectPiecesController(IProjectPiecesService projectPiecesservice)
        {
            _ProjectPiecesservice = projectPiecesservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllProjectPieces")]

        public IActionResult GetAllProjectPieces(int ProjectId, string? SearchText)
        {
            return Ok(_ProjectPiecesservice.GetAllProjectPieces(ProjectId, SearchText??""));
        }
        [HttpGet("FillProjectPieces")]

        public IActionResult FillProjectPieces(int param)
        {
            return Ok(_ProjectPiecesservice.GetAllProjectPieces(param, "").Result.Select(s => new {
                Id = s.PieceId,
                Name = s.PieceNo,
                Notes = s.Notes,
                ProjectId = s.ProjectId
            }));
        }
        [HttpPost("SaveProjectPieces")]

        public IActionResult SaveProjectPieces(ProjectPieces projectPieces)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectPiecesservice.SaveProjectPieces(projectPieces, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteProjectPieces")]

        public IActionResult DeleteProjectPieces(int PieceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectPiecesservice.DeleteProjectPieces(PieceId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}
