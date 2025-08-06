using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using System.Net;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ModelController : ControllerBase
    {
        private IModelService _modelservice;
        public GlobalShared _globalshared;
        public ModelController(IModelService modelservice)
        {
            _modelservice = modelservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllModels")]
        public IActionResult GetAllModels()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_modelservice.GetAllModels(_globalshared.BranchId_G));
        }
        [HttpPost("SaveModel")]
        public IActionResult SaveModel(Model model, List<IFormFile> postedFiles)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string path = Path.Combine("/Uploads/Projects/Documents/");
            string fileName = ""; string fname = ""; string fnamepath = "";

            foreach (IFormFile postedFile in postedFiles)
            {
                fname = postedFile.FileName;
                fileName = System.IO.Path.GetFileName(GenerateRandomNo() + fname);
                fnamepath = Path.Combine(path, fileName);

                try
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fnamepath, System.IO.FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        string atturl = Path.Combine(path, fname);
                        model.FileUrl = "/Uploads/Projects/Documents/" + fileName;
                    }
                }
                catch (Exception)
                {
                    var massege = "فشل في رفع الملفات";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }
            }
            var result = _modelservice.SaveModel(model, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteModel")]
        public IActionResult DeleteModel(int ModelId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _modelservice.DeleteModel(ModelId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("GenerateRandomNo")]
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}
