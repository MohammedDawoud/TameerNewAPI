using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using System.Net.Mail;
using System.Net;
using TaamerProject.Service.Services;
using Google.Apis.Drive.v3.Data;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ItemController : ControllerBase
    {
        private IItemService _itemservice;
        public GlobalShared _globalshared;
        string? Con;
        private IConfiguration Configuration;
        public ItemController(IItemService itemservice, IConfiguration _configuration)
        {
            _itemservice = itemservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");

        }
        [HttpGet("GetAllItems")]
        public IActionResult GetAllItems(int? typeId = null)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_itemservice.GetAllItems(_globalshared.Lang_G, typeId));
        }
        [HttpPost("SaveItem")]
        public IActionResult SaveItem(Item item, List<IFormFile> postedFiles_LiscenceImage, List<IFormFile> postedFiles_InsuranceImage)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string path = Path.Combine("/Uploads/Items/");
            string fileName = ""; string fname = ""; string fnamepath = "";

            foreach (IFormFile postedFile in postedFiles_LiscenceImage)
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
                        item.LiscenceFileUrl = "/Uploads/Items/" + fileName;
                    }
                }
                catch (Exception)
                {
                    var massege = "فشل في رفع الملفات";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }
            }

            fileName = "";  fname = "";  fnamepath = "";

            foreach (IFormFile postedFile in postedFiles_InsuranceImage)
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
                        item.InsuranceFileUrl = "/Uploads/Items/" + fileName;
                    }
                }
                catch (Exception)
                {
                    var massege = "فشل في رفع الملفات";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }
            }
            var result = _itemservice.SaveItem(item, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }




        [HttpPost("SaveItem2")]
        public IActionResult SaveItem2(Item item)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //string path = Path.Combine("/Uploads/Items/");
            //string fileName = ""; string fname = ""; string fnamepath = "";

            //foreach (IFormFile postedFile in postedFiles_LiscenceImage)
            //{
            //    fname = postedFile.FileName;
            //    fileName = System.IO.Path.GetFileName(GenerateRandomNo() + fname);
            //    fnamepath = Path.Combine(path, fileName);

            //    try
            //    {
            //        using (System.IO.FileStream stream = new System.IO.FileStream(fnamepath, System.IO.FileMode.Create))
            //        {
            //            postedFile.CopyTo(stream);
            //            string atturl = Path.Combine(path, fname);
            //            item.LiscenceFileUrl = "/Uploads/Items/" + fileName;
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        var massege = "فشل في رفع الملفات";
            //        return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
            //    }
            //}

            //fileName = ""; fname = ""; fnamepath = "";

            //foreach (IFormFile postedFile in postedFiles_InsuranceImage)
            //{
            //    fname = postedFile.FileName;
            //    fileName = System.IO.Path.GetFileName(GenerateRandomNo() + fname);
            //    fnamepath = Path.Combine(path, fileName);

            //    try
            //    {
            //        using (System.IO.FileStream stream = new System.IO.FileStream(fnamepath, System.IO.FileMode.Create))
            //        {
            //            postedFile.CopyTo(stream);
            //            string atturl = Path.Combine(path, fname);
            //            item.InsuranceFileUrl = "/Uploads/Items/" + fileName;
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        var massege = "فشل في رفع الملفات";
            //        return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
            //    }
            //}
            var result = _itemservice.SaveItem(item, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }


        [HttpPost("DeleteItem")]
        public IActionResult DeleteItem(int ItemId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _itemservice.DeleteItem(ItemId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillItemSelect")]
        public IActionResult FillItemSelect(bool param = true)
        {
            if (param) // IsNew
                return Ok(_itemservice.FillItemSelectSQL(Con??""));
            else
                return Ok(_itemservice.FillItemSelect());
        }
        [HttpGet("FillItemCarSelect")]
        public IActionResult FillItemCarSelect()
        {
            return Ok(_itemservice.FillItemCarSelect());
        }
        [HttpGet("SearchItems")]
        public IActionResult SearchItems(ItemVM ItemsSearch, string lang)
        {
            return Ok(_itemservice.SearchItems(ItemsSearch, lang));
        }
        [HttpGet("IsCar")]
        public IActionResult IsCar(int ItemId)
        {
            return Ok(_itemservice.IsCar(ItemId));
        }
        [HttpGet("GenerateRandomNo")]

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}
