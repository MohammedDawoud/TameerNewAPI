using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using System.Net;
using System.Net.Mail;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class BranchesController : ControllerBase
    {
        private readonly IBranchesService _branchesservice;
        public GlobalShared _globalshared;

        public BranchesController(IBranchesService branchesservice)
        {
            _branchesservice = branchesservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllBranches")]

        public IActionResult GetAllBranches()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetAllBranches(_globalshared.Lang_G));
        }
        [HttpGet("GetFirstBranch")]

        public IActionResult GetFirstBranch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetAllBranches(_globalshared.Lang_G).Result.FirstOrDefault());
        }
        [HttpGet("GetAllBranchescount")]

        public IActionResult GetAllBranchescount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int count = _branchesservice.GetAllBranches(_globalshared.Lang_G).Result.Count();
            return Ok(count);
        }
        [HttpGet("GetBranchById")]

        public IActionResult GetBranchById(int branchid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetBranchByBranchId(_globalshared.Lang_G, branchid));
        }
        [HttpGet("GetBranchByBranchId")]

        public IActionResult GetBranchByBranchId()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetBranchByBranchIdCheck")]

        public IActionResult GetBranchByBranchIdCheck()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetBranchByBranchIdCheck(_globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllBranchesByUserId")]

        public IActionResult GetAllBranchesByUserId(int UserId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetAllBranchesByUserId(_globalshared.Lang_G, UserId));
        }
        [HttpGet("GetAllBranchesByUserIdDrop")]
        public IActionResult GetAllBranchesByUserIdDrop()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared. UserId_G));
        }
        [HttpPost("SaveBranches")]

        public IActionResult SaveBranches([FromForm] Branch branches, List<IFormFile>? postedFiles, List<IFormFile>? postedFilesheader, List<IFormFile>? postedFilesfooter)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string path = Path.Combine("Uploads/Organizations/pictures/");
            string fileName = ""; string fname = ""; string fnamepath = "";
            if(postedFiles!=null)
            {
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
                            branches.BranchLogoUrl = "/Uploads/Organizations/pictures/" + fileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        var massege = "فشل في رفع الملفات";
                        return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                    }
                }
            }

            fileName = ""; fname = ""; fnamepath = "";
            if(postedFilesheader!=null)
            {
                foreach (IFormFile postedFile in postedFilesheader)
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
                            branches.HeaderLogoUrl = "/Uploads/Organizations/pictures/" + fileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        var massege = "فشل في رفع الملفات";
                        return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                    }
                }

            }
            fileName = ""; fname = ""; fnamepath = "";
            if(postedFilesfooter!=null)
            {
                foreach (IFormFile postedFile in postedFilesfooter)
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
                            branches.FooterLogoUrl = "/Uploads/Organizations/pictures/" + fileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        var massege = "فشل في رفع الملفات";
                        return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                    }
                }

            }

            var result = _branchesservice.SaveBranches(branches, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveBranchesInvoiceCode")]

        public IActionResult SaveBranchesInvoiceCode(Branch branches)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _branchesservice.SaveBranchesInvoiceCode(branches, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveBranchesAccs")]

        public IActionResult SaveBranchesAccs(Branch branches)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _branchesservice.SaveBrancheAccs(branches, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveBranchesAccsBS")]

        public IActionResult SaveBranchesAccsBS(Branch branches)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _branchesservice.SaveBranchesAccsBS(branches, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveBranchesAccsKD")]

        public IActionResult SaveBranchesAccsKD(Branch branches)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _branchesservice.SaveBranchesAccsKD(branches, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteBranches")]

        public IActionResult DeleteBranches(int BranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _branchesservice.DeleteBranches(BranchId, _globalshared.UserId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpPost("ActivateBranches")]

        public IActionResult ActivateBranches(int BranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _branchesservice.ActivateBranches(BranchId, _globalshared.UserId_G);
            return Ok(result);
        }
        [HttpGet("GetActiveBranch")]

        public IActionResult GetActiveBranch()
        {
            return Ok(_branchesservice.GetActiveBranch());
        }
        [HttpGet("FillBranchSelect")]

        public IActionResult FillBranchSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetAllBranches(_globalshared.Lang_G).Result.Select(s => new {
                Id = s.BranchId,
                Name = s.BranchName
            }));
        }
        [HttpGet("FillBranchSelectNew")]

        public IActionResult FillBranchSelectNew()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.FillBranchSelectNew(_globalshared.Lang_G).Result.Select(s => new {
                Id = s.BranchId,
                Name = s.BranchName
            }));
        }
        [HttpGet("FillBranchNotCurrentSelect")]

        public IActionResult FillBranchNotCurrentSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetAllBranches(_globalshared.Lang_G).Result.Where(s => s.BranchId != _globalshared.BranchId_G).Select(s => new {
                Id = s.BranchId,
                Name = s.BranchName
            }));
        }
        [HttpGet("GetBranchCashBoxAccount")]

        public IActionResult GetBranchCashBoxAccount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetAllBranches(_globalshared.Lang_G).Result.Where(s => s.BranchId == _globalshared.BranchId_G).Select(x => new {
                AccId = x.BoxAccId
                //Name = s.NameAr
            }));
        }
        [HttpGet("GenerateNextBranchNumber")]

        public IActionResult GenerateNextBranchNumber()
        {
            var Number = _branchesservice.GenerateNextBranchNumber().Result;
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Number.ToString() };
            return Ok(generatevalue);
        }
        [HttpGet("FillBranchByUserIdSelect")]

        public IActionResult FillBranchByUserIdSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result.Select(x => new
            {
                Id = x.BranchId,
                Name = x.BranchName
            }));
        }
        [HttpGet("FillBranchByUserIdSelectCount")]

        public int FillBranchByUserIdSelectCount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var count = _branchesservice.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result.Count();
            return count;
        }
        [HttpGet("GetAllBranchesAndMainByUserId")]

        public IActionResult GetAllBranchesAndMainByUserId()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesservice.GetAllBranchesAndMainByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result.Select(x => new
            {
                Id = x.BranchId,
                Name = x.BranchName
            }));
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
