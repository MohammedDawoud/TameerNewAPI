using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.Service.Interfaces;
using TaamerProject.API.Helper;
//using System.Drawing;
using iTextSharp.text;
using TaamerProject.Models;
using System.Net;
using iTextSharp.text.pdf;
using TaamerProject.Models.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProjectRequirementsController : ControllerBase
    {

            FilesHelper filesHelper;
            private string UrlBase = "/Files/ProjectFiles/";
            String DeleteURL = "/FileUpload/DeleteFile/?file=";
            String DeleteType = "GET";
            String tempPath = "~/ProjectFiles/";
            String serverMapPath = "~/Files/ProjectFiles/";
            private string StorageRoot
            {
                get { return Path.Combine(Path.Combine(serverMapPath)); }
            }
            private IProjectRequirementsService _projectRequirementsservice;
            private IProjectPhasesTasksService _projectPhasesTasksService;
            private IProjectService _projectService;
            private IBranchesService _branchesService;
            private IOrganizationsService _organizationsService;
            private IFileService _fileService;
            private ICustomerService _customerService;
            private IWorkOrdersService _workOrders;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string Con;

        public ProjectRequirementsController(IProjectRequirementsService projectRequirementsService, IProjectPhasesTasksService projectPhasesTasksService, IProjectService projectService,
                IBranchesService branchesService, IOrganizationsService organizationsService, IFileService fileService, ICustomerService customerService, IWorkOrdersService workOrdersService
             ,IConfiguration _configuration, IWebHostEnvironment webHostEnvironment)
            {
                this._projectRequirementsservice = projectRequirementsService;
                this._projectPhasesTasksService = projectPhasesTasksService;
                this._projectService = projectService;
                this._branchesService = branchesService;
                this._organizationsService = organizationsService;
                this._fileService = fileService;
                this._customerService = customerService;
                this._workOrders = workOrdersService;


            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext;

            _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;


            filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot, UrlBase, tempPath, serverMapPath);

            }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("GetAllProjectRequirement")]
        public IActionResult GetAllProjectRequirement()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectRequirementsservice.GetAllProjectRequirement(_globalshared.BranchId_G).Result );
        }
        [HttpGet("GetAllProjectRequirementByTaskId")]
        public IActionResult GetAllProjectRequirementByTaskId(int PhasesTaskID)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectRequirementsservice.GetAllProjectRequirementByTaskId(_globalshared.BranchId_G, PhasesTaskID).Result);
            }
        [HttpGet("GetAllProjectRequirementById")]
        public IActionResult GetAllProjectRequirementById(int RequirementId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectRequirementsservice.GetAllProjectRequirementById(_globalshared.BranchId_G, RequirementId).Result);
        }
        [HttpGet("GetAllProjectRequirementByOrderId")]
        public IActionResult GetAllProjectRequirementByOrderId(int OrderId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectRequirementsservice.GetAllProjectRequirementByOrder(_globalshared.BranchId_G, OrderId).Result);
            }
        [HttpGet("GetProjectRequirementByProjectSubTypeId")]
        public IActionResult GetProjectRequirementByProjectSubTypeId(int ProjectSubTypeId, string? SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectRequirementsservice.GetProjectRequirementByProjectSubTypeId(ProjectSubTypeId, SearchText??"",_globalshared.BranchId_G).Result);
            }
        [HttpGet("GetProjectRequirementByTaskId")]
        public IActionResult GetProjectRequirementByTaskId(int TaskId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectRequirementsservice.GetProjectRequirementByTaskId(TaskId,_globalshared.BranchId_G).Result);
            }

        [HttpGet("GetProjectRequirementByTaskId_Count")]
        public int GetProjectRequirementByTaskId_Count(int TaskId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _projectRequirementsservice.GetProjectRequirementByTaskId(TaskId,_globalshared.BranchId_G).Result.Count();
                return Result;
            }
        [HttpGet("GetProjectRequirementByOrderId_Count")]
        public int GetProjectRequirementByOrderId_Count(int Orderid)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _projectRequirementsservice.GetProjectRequirementOrderId(Orderid,_globalshared.BranchId_G).Result.Count();
                return Result;
            }


        //[HttpPost("UploadFiles")]
        //public IActionResult UploadFiles([FromBody]List<ProjectRequirements> Proreq)
        //    {
        //    HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        //    List<ProjectRequirements> Prorjeq = new List<ProjectRequirements>();
        //        var count = Convert.ToInt32(Request.Form["rowcount"]) - 1;
        //        int counter = 1;

        //        for (int x = 1; x <= count; x++)
        //        {
        //            ProjectRequirements pro = new ProjectRequirements();
        //            pro.RequirementId = Convert.ToInt32(Request.Form["RequirementId" + x + ""]);
        //            pro.ProjectTypeId = Convert.ToInt32(Request.Form["ProjectTypeId" + x + ""]);

        //            pro.ProjectSubTypeId = Convert.ToInt32(Request.Form["ProjectSubTypeId" + x + ""]);
        //            pro.NameAr = Request.Form["NameAr" + x + ""];
        //            pro.NameEn = Request.Form["NameEn" + x + ""];
        //            var cos = Request.Form["Cost" + x + ""];
        //            if (cos != null && cos != "")
        //            {
        //                pro.Cost = Convert.ToDecimal(cos);
        //            }
        //            var fileline = Convert.ToInt32(Request.Form["Fileline" + x + ""]);
        //            if (fileline == x)
        //            {

        //                if (Request.Files.Count > 0)
        //                {
        //                    try
        //                    {
        //                        //List<string> attach = new List<string>();
        //                        ////  Get all files from Request object  
        //                        HttpFileCollectionBase files = Request.Files;
        //                        var model = Request.Form["Proreq"];


        //                        for (int i = 0; i < files.Count; i++)
        //                        {

        //                            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
        //                            //string filename = Path.GetFileName(Request.Files[i].FileName);  
        //                            if (i == (counter - 1))
        //                            {
        //                                HttpPostedFileBase file = files[counter - 1];
        //                                string fname;

        //                                // Checking for Internet Explorer  
        //                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
        //                                {
        //                                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
        //                                    fname = testfiles[testfiles.Length - 1];
        //                                }
        //                                else
        //                                {
        //                                    fname = file.FileName;
        //                                }

        //                                // Get the complete folder path and store the file inside it.  
        //                                fname = Path.Combine(Server.MapPath("~/Uploads/ProjectRequirements/"), fname);
        //                                file.SaveAs(fname);
        //                                string atturl = Path.Combine(Server.MapPath("/Uploads/ProjectRequirements/"), fname);
        //                                //attach.Add(atturl);
        //                                pro.AttachmentUrl = "/Uploads/ProjectRequirements/" + files[counter - 1].FileName;

        //                            }
        //                        }
        //                        counter++;
        //                    }
        //                    catch
        //                    {

        //                    }
        //                }

        //            }

        //            Prorjeq.Add(pro);

        //        }
        //        var result = _projectRequirementsservice.SaveProjectRequirement2(Prorjeq, UserId,_globalshared.BranchId_G);
        //        if (Lang == "ltr" && result.Result == true)
        //        {
        //            result.Message = "Saved Successfully";
        //        }
        //        else if (Lang == "ltr" && result.Result == false)
        //        {
        //            result.Message = "Saved Falied";
        //        }
        //        return Ok(result);
        //    }



            [HttpPost("SaveAllProjectRequirement")]
            public IActionResult SaveAllProjectRequirement([FromBody]List<ProjectRequirements> Proreq)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //HttpPostedFileBase file = Request.Files["UploadedFile"];
                //IList<HttpPostedFileBase> additionalDocs = Request.Files.GetMultiple("UploadedFile");
                //var file = Request.Files;
                //if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                //{
                //    if (Request.Files["UploadedFile"].ContentLength > 0)
                //    {
                //        string fileLocation = Server.MapPath("~/Uploads/ProjectRequirements");
                //        try
                //        {
                //            if (!System.IO.Directory.Exists(fileLocation))
                //            {
                //                System.IO.Directory.CreateDirectory(fileLocation);
                //            }
                //            var filePath = Server.MapPath("~/Uploads/ProjectRequirements/") + Request.Files["UploadedFile"].FileName;
                //            Request.Files["UploadedFile"].SaveAs(filePath);
                //            //projectRequirements.AttachmentUrl = "/Uploads/ProjectRequirements/" + Request.Files["UploadedFile"].FileName;
                //        }
                //        catch
                //        {
                //            var massage = "";
                //            if (Lang == "rtl")
                //            {
                //                massage = "فشل في رفع المرفقات";
                //            }
                //            else
                //            {
                //                massage = "Failed To Upload Files";
                //            }
                //            return Ok(new GeneralMessage { Result = false, Message = massage } );
                //        }
                //    }
                //}


                var result = _projectRequirementsservice.SaveProjectRequirement2(Proreq, _globalshared.UserId_G,_globalshared.BranchId_G);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "Saved Successfully";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
                {
                    result.ReasonPhrase = "Saved Falied";
                }
                return Ok(result);
            }

        [HttpPost("SaveProjectRequirement")]
        public IActionResult SaveProjectRequirement(IFormFile? UploadedFile, [FromForm] string? RequirementId
            , [FromForm] string? ProjectTypeId, [FromForm] string? ProjectSubTypeId, [FromForm] string? NameAr
            , [FromForm] string? NameEn, [FromForm] string? Cost)
            {
        ProjectRequirements projectRequirements = new ProjectRequirements();
        projectRequirements.RequirementId = Convert.ToInt32(RequirementId);
        projectRequirements.ProjectTypeId = Convert.ToInt32(ProjectTypeId); 
        projectRequirements.ProjectSubTypeId = Convert.ToInt32(ProjectSubTypeId);
        projectRequirements.NameAr = NameAr;
        projectRequirements.NameEn = NameEn;
        if(Cost!="null" && Cost != "")
        {
            projectRequirements.Cost = Convert.ToDecimal(Cost);
        }

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        if (UploadedFile != null)
        {
            System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


            string path = System.IO.Path.Combine("Uploads/", "ProjectRequirements/");
            string pathW = System.IO.Path.Combine("/Uploads/", "ProjectRequirements/");

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            string pathes = "";
            //foreach (IFormFile postedFile in postedFiles)
            //{
            string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedFile.FileName);
            //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

            var path2 = Path.Combine(path, fileName);
            if (System.IO.File.Exists(path2))
            {
                System.IO.File.Delete(path2);
            }
            using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
            {


                UploadedFile.CopyTo(stream);
                uploadedFiles.Add(fileName);
                // string returnpath = host + path + fileName;
                //pathes.Add(pathW + fileName);
                pathes = pathW + fileName;
            }


            if (pathes != null)
            {
                projectRequirements.AttachmentUrl = pathes;
            }
        }
        var result = _projectRequirementsservice.SaveProjectRequirement(projectRequirements, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveProjectRequirement2")]
        public IActionResult SaveProjectRequirement2(ProjectRequirements projectRequirements)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //HttpPostedFileBase file = Request.Files["UploadedFile"];
            //if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //{
            //    if (Request.Files["UploadedFile"].ContentLength > 0)
            //    {
            //        string fileLocation = Server.MapPath("~/Uploads/ProjectRequirements/") + Request.Files["UploadedFile"].FileName;
            //        try
            //        {
            //            if (System.IO.File.Exists(fileLocation))
            //            {
            //                System.IO.File.Delete(fileLocation);
            //            }
            //            Request.Files["UploadedFile"].SaveAs(fileLocation);
            //            projectRequirements.AttachmentUrl = "/Uploads/ProjectRequirements/" + Request.Files["UploadedFile"].FileName;
            //        }
            //        catch
            //        {
            //            return Ok(new GeneralMessage { Result = false, Message = "فشل في رفع المرفقات" } );
            //        }
            //    }
            //}
            //var result = _projectRequirementsservice.SaveProjectRequirement(projectRequirements, UserId,_globalshared.BranchId_G);
            //return Ok(new { result.Result, result.Message } );
            var resultList = new List<ViewDataUploadFilesResult>();
                var CurrentContext = HttpContext;
                //filesHelper.UploadAndShowResults(CurrentContext, resultList);
                JsonFiles files = new JsonFiles(resultList);
                bool isEmpty = !resultList.Any();
                if (isEmpty)
                {
                    return Ok("Error");
                }
                else
                {

                    projectRequirements.RequirementId = projectRequirements.RequirementId;
                    projectRequirements.NameAr = resultList[0].name;
                    //projectRequirements. = resultList[0].size;
                    //projectRequirements.AttachmentUrl = resultList[0].url;
                    projectRequirements.AttachmentUrl = "/Files/ProjectFiles/" + resultList[0].name;
                    projectRequirements.UserId = _globalshared.UserId_G;
                    projectRequirements.PhasesTaskID = projectRequirements.PhasesTaskID;
                    projectRequirements.ProjectSubTypeId = projectRequirements.ProjectSubTypeId;
                    projectRequirements.ProjectTypeId = projectRequirements.ProjectTypeId;
                    projectRequirements.NotifactionId = projectRequirements.NotifactionId;
                    //projectRequirements.DeleteType = resultList[0].deleteType;
                    //projectRequirements.ThumbnailUrl = resultList[0].thumbnailUrl;
                    _projectRequirementsservice.SaveProjectRequirement(projectRequirements, _globalshared.UserId_G, _globalshared.BranchId_G);
                    return Ok(files);
                }
            }
        [HttpPost("SaveProjectRequirement3")]
        public IActionResult SaveProjectRequirement3([FromForm] IFormFile? uploadesgiles, [FromForm] int? RequirementId
            , [FromForm] int? ProjectTypeId, [FromForm] int? ProjectSubTypeId, [FromForm] string? NameAr
            , [FromForm] string? NameEn, [FromForm] decimal? Cost, [FromForm] int? OrderId, [FromForm] int? PhasesTaskID)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ProjectRequirements projectRequirements = new ProjectRequirements();
            projectRequirements.RequirementId = RequirementId ?? 0;
            projectRequirements.ProjectTypeId = ProjectTypeId;
            projectRequirements.ProjectSubTypeId = ProjectSubTypeId;
            projectRequirements.NameAr = NameAr;
            projectRequirements.NameEn = NameEn;
            projectRequirements.Cost = Cost;
            projectRequirements.PhasesTaskID = PhasesTaskID;
            projectRequirements.OrderId = OrderId;

            if (uploadesgiles != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "ProjectRequirements/");
                string pathW = System.IO.Path.Combine("/Uploads/", "ProjectRequirements/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                string pathes = "";
                //foreach (IFormFile postedFile in postedFiles)
                //{
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + uploadesgiles.FileName);
                //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {


                    uploadesgiles.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    // string returnpath = host + path + fileName;
                    //pathes.Add(pathW + fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != null)
                {
                    projectRequirements.AttachmentUrl = pathes;
                }
            }
            var result = _projectRequirementsservice.SaveProjectRequirement(projectRequirements, _globalshared.UserId_G,_globalshared.BranchId_G);
                return Ok(result );
            }

        [HttpPost("SaveProjectRequirement4")]

        public IActionResult SaveProjectRequirement4([FromForm] List<IFormFile>? uploadesgiles,[FromForm] int? RequirementId
            , [FromForm] int? ProjectTypeId, [FromForm] int? ProjectSubTypeId, [FromForm] string? NameAr
            , [FromForm] string? NameEn, [FromForm] decimal? Cost, [FromForm] int? OrderId, [FromForm] int? PhasesTaskID, [FromForm] string? PageInsert, [FromForm] string? FileName=null, [FromForm] string? TypeId=null, [FromForm] bool? IsCertified=null)
        {
            ProjectRequirements projectRequirements = new ProjectRequirements();
            projectRequirements.RequirementId = RequirementId??0;
            projectRequirements.ProjectTypeId = ProjectTypeId;
            projectRequirements.ProjectSubTypeId = ProjectSubTypeId;
            projectRequirements.NameAr = NameAr;
            projectRequirements.NameEn = NameEn;
            projectRequirements.Cost = Cost;
            projectRequirements.PhasesTaskID = PhasesTaskID;
            projectRequirements.OrderId = OrderId;
            if(PageInsert=="" || PageInsert==null)
            {
                projectRequirements.PageInsert = 1;
            }
            else
            {
                projectRequirements.PageInsert =Convert.ToInt32(PageInsert);
            }


            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var FileResult = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يوجد ملفات", ReturnedStr = "" };
            var FileResult2 = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "تأكد من الرقم الضريبي للمنشأة وذلك لاستخدامة في عملية الباركود", ReturnedStr = "" };
            var FileResult3 = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في رفع ملف", ReturnedStr = "" };
            try
            {
                int? ProID = 0;
                if (projectRequirements.PhasesTaskID != null && projectRequirements.PhasesTaskID != 0)
                {

                    ProID = _projectPhasesTasksService.GetProjectNoById(projectRequirements.PhasesTaskID, _globalshared.Lang_G);
                }
                else
                {
                    ProID = _workOrders.GetWorkOrderById(projectRequirements.OrderId??0, _globalshared.Lang_G).Result.ProjectId;
                }
                //else
                //{
                //    return Ok(new { Result=false, Message = "لا يمكن اضافة ملف لعدم الربط بمشروع" } );
                //}

                if (ProID == null || ProID == 0)
                {
                    return Ok(new { Result = false, Message = "لا يمكن اضافة ملف لعدم الربط بمشروع" });
                }
                int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
                string TaxCode = _organizationsService.GetBranchOrganizationData(orgId).Result.TaxCode;
                string OrgaEnglishName = _organizationsService.GetBranchOrganizationData(orgId).Result.NameAr ?? "";

                if (TaxCode == "")
                {
                    //return Ok(false, "تأكد من الرقم الضريبي للمنشأة وذلك لاستخدامة في عملية الباركود" );
                    return Ok(new { FileResult2.StatusCode, FileResult2.ReasonPhrase });


                }
                var project = _projectService.GetProjectById(_globalshared.Lang_G, ProID ?? 0).Result;
                var CustomerName = _projectService.GetProjectById(_globalshared.Lang_G, ProID ?? 0).Result.CustomerName;


                string pathF = System.IO.Path.Combine("Files/", "ProjectFiles/");
                string serverMapPath2 = "";
                if (project != null)
                {
                    serverMapPath2 = Path.Combine("Files/ProjectFiles/" + project.ProjectNo);
                    pathF = System.IO.Path.Combine("Files/", "ProjectFiles/" + project.ProjectNo);
                    if (!Directory.Exists(pathF))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(pathF);
                    }
                }


                UrlBase = UrlBase + project.ProjectNo; /// added recently
                FilesHelper filesHelpers = new FilesHelper(DeleteURL, DeleteType, serverMapPath2, UrlBase, tempPath, serverMapPath2);

                for (int i = 0; i < uploadesgiles.Count; i++)
                {
                    string valrand = RandomNumber(1, 10000).ToString();
                    ProjectRequirements projectRequirementsN = new ProjectRequirements();


                    string fileName = valrand + uploadesgiles[i].FileName;
                    //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                    var path2 = Path.Combine(pathF, fileName);
                    if (System.IO.File.Exists(path2))
                    {
                        System.IO.File.Delete(path2);
                    }
                    using (System.IO.FileStream stream = new System.IO.FileStream(path2, System.IO.FileMode.Create))
                    {
                        uploadesgiles[i].CopyTo(stream);

                    }


                    projectRequirementsN.RequirementId = 0;

                    projectRequirementsN.NameAr = uploadesgiles[i].FileName;

                    var Extension = System.IO.Path.GetExtension(uploadesgiles[i].FileName);
                    if (Extension == "application/pdf" || Extension == ".pdf")
                    {
                        projectRequirementsN.AttachmentUrl = UrlBase + "/" + "_B_" + valrand + uploadesgiles[i].FileName;
                    }
                    else
                    {
                        projectRequirementsN.AttachmentUrl = UrlBase + "/"+ valrand + uploadesgiles[i].FileName;
                    }

                    projectRequirementsN.UserId = _globalshared.UserId_G;
                    projectRequirementsN.PhasesTaskID = projectRequirements.PhasesTaskID;
                    projectRequirementsN.ProjectSubTypeId = projectRequirements.ProjectSubTypeId;
                    projectRequirementsN.ProjectTypeId = projectRequirements.ProjectTypeId;
                    projectRequirementsN.NotifactionId = projectRequirements.NotifactionId;
                    projectRequirementsN.OrderId = projectRequirements.OrderId;
                    projectRequirementsN.PageInsert = projectRequirements.PageInsert;

                    //projectRequirements.DeleteType = resultList[0].deleteType;
                    //projectRequirements.ThumbnailUrl = resultList[0].thumbnailUrl;
                    _projectRequirementsservice.SaveProjectRequirement(projectRequirementsN, _globalshared.UserId_G, project.BranchId ?? _globalshared.BranchId_G);



                    ProjectFiles file = new ProjectFiles();
                    //string BarcodeHash = EncryptValue(TaxCode);
                    string BarcodeHash = "200010001000";


                    string filename = uploadesgiles[i].FileName;
                    file.ProjectId = ProID;
                    file.FileName = FileName ?? uploadesgiles[i].FileName;
                    file.IsCertified = IsCertified ?? file.IsCertified;
                    file.Notes = file.Notes;
                    file.FileSize = uploadesgiles[i].Length;
                    file.TypeId = TypeId ==null || TypeId=="" ? 1 : Convert.ToInt32(TypeId);


                    if (Extension == "application/pdf" || Extension == ".pdf")
                    {
                        file.FileUrl = UrlBase + "/" + "_B_" + valrand + uploadesgiles[i].FileName;
                        file.FileUrlW = UrlBase+ "/" + valrand + uploadesgiles[i].FileName;
                    }
                    else
                    {
                        file.FileUrl = UrlBase + "/" + valrand + uploadesgiles[i].FileName;
                        file.FileUrlW = UrlBase + "/" + valrand + uploadesgiles[i].FileName;
                    }

                    file.BarcodeFileNum = BarcodeHash;

                    file.Extension = Extension;
                    file.DeleteUrl = null;
                    file.DeleteType = null;
                    file.ThumbnailUrl = null;
                    file.Type = uploadesgiles[i].ContentType;
                    file.TaskId = file.TaskId;
                    file.CompanyTaxNo = TaxCode;
                    file.PageInsert = projectRequirements.PageInsert;

                    file.NotificationId = file.NotificationId;

                    FileResult = _fileService.SaveFile_Bar(file, _globalshared.UserId_G, project.BranchId ?? _globalshared.BranchId_G);
                    if (Extension == "application/pdf" || Extension == ".pdf")
                    {
                        BarcodeFun_A3_N(serverMapPath2, fileName, FileResult.ReturnedStr, valrand, project.ProjectNo, OrgaEnglishName, CustomerName);

                    }
                }
                return Ok(new { FileResult.StatusCode, FileResult.ReasonPhrase });
            }
            catch (Exception ex)
            {

                //return Ok(false, "فشل في الحفظ تأكد من اختيار ملف" );
                return Ok(new { FileResult3.StatusCode, FileResult3.ReasonPhrase });

            }

        }

        private readonly Random _random = new Random();

        // Generates a random number within a range.
       [HttpPost("RandomNumber")]
        public int RandomNumber(int min, int max)
            {
                return _random.Next(min, max);
            }
        [HttpPost("BarcodeFun")]
        public IActionResult BarcodeFun(string URL, string Filename, string NumberCode, string Rand, string ProjectNo, string OrgEngName, string CustomerName)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectService.BarcodePDF(1, _globalshared.UserId_G);
                string File = Path.Combine(URL, Filename);
                string newFile = Path.Combine(URL, "_B_" + Rand + Filename);


                // open the reader
                PdfReader reader = new PdfReader(File);
                iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
                Document document = new Document(size);

                // open the writer
                FileStream fs = new FileStream(newFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();


                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    document.NewPage();
                    // the pdf content
                    PdfContentByte cb = writer.DirectContent;

                    var bc = new Barcode128
                    {
                        Code = NumberCode,
                        TextAlignment = Element.ALIGN_CENTER,
                        StartStopText = true,
                        CodeType = Barcode.CODE128,
                        ChecksumText = true,
                        GenerateChecksum = true,
                        Extended = false
                    };



                    var xx = 0;
                    string fontpath = Environment.GetEnvironmentVariable("SystemRoot") +
                     "\\fonts\\tahoma.ttf";
                    BaseFont bf = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font tahomaFont = new Font(bf, 8, Font.NORMAL, BaseColor.DARK_GRAY);

                    cb.SetColorFill(BaseColor.DARK_GRAY);
                    cb.SetFontAndSize(bf, 8);

                    // write the text in the pdf content

                    //cb.BeginText();
                    //string text = " ProjectNo : " + ProjectNo;
                    //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text, 30, 110, 0);
                    //cb.EndText();



                    //cb.BeginText();
                    //text = " ProjectNo : " + ProjectNo;
                    //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text, 30, 90, 0);
                    //cb.EndText();

                    ColumnText ct = new ColumnText(writer.DirectContent);
                    ct.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    ct.SetSimpleColumn(0, 0, 170, 120, 8, Element.ALIGN_LEFT);

                    var chunk = new Chunk(OrgEngName, tahomaFont);
                    var chunk2 = new Chunk(ProjectNo + " - " + CustomerName, tahomaFont);


                    ct.AddElement(chunk2);

                    ct.AddElement(chunk);

                    ct.Go();





                    iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                    var barCodeRect = new iTextSharp.text.Rectangle(bc.BarcodeSize);
                    iTextSharp.text.Rectangle tempRect;
                    tempRect = new iTextSharp.text.Rectangle(0, 0, 140, 40);//(,,3rd,toool)

                    img.ScaleAbsolute(tempRect);
                    img.SetAbsolutePosition(30, 30);
                    cb.AddImage(img);

                    var bc2 = new BarcodeQRCode(NumberCode, 50, 50, null);
                    iTextSharp.text.Image img1 = bc2.GetImage();
                    iTextSharp.text.Rectangle tempRect2;
                    tempRect2 = new iTextSharp.text.Rectangle(0, 0, 120, 120);//(,,3rd,toool)

                    img1.ScaleAbsolute(tempRect2);
                    img1.SetAbsolutePosition((size.Width - 150), 0);
                    cb.AddImage(img1);


                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    cb.AddTemplate(page, 0, 0);
                }


                // close the streams and voilá the file should be changed :)
                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();

                return Ok(result);

            }
        [HttpPost("BarcodeFun_A3")]
        public IActionResult BarcodeFun_A3(string URL, string Filename, string NumberCode, string Rand, string ProjectNo, string OrgEngName, string CustomerName)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectService.BarcodePDF(1, _globalshared.UserId_G);
                string File = Path.Combine(URL, Filename);
                string newFile = Path.Combine(URL, "_B_" + Rand + Filename);
                float LAST_CELL_HEIGHT = 50f;


                // open the reader
                PdfReader reader = new PdfReader(File);
                iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
                var _pdfNewSizeW = size.Width;
                var _pdfNewSizeH = size.Height + 100;
                iTextSharp.text.Rectangle newRect = new iTextSharp.text.Rectangle(0, 0, Convert.ToSingle(_pdfNewSizeW), Convert.ToSingle(_pdfNewSizeH));
                //Document document = new Document(size);
                Document document = new Document(newRect);
                Document.Compress = true;
                document.SetMargins(0, 0, 0, 0);

                // open the writer
                FileStream fs = new FileStream(newFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();


                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    document.NewPage();
                    // the pdf content

                    PdfContentByte cb = writer.DirectContent;

                    var bc = new Barcode128
                    {
                        Code = NumberCode,
                        TextAlignment = Element.ALIGN_CENTER,
                        StartStopText = true,
                        CodeType = Barcode.CODE128,
                        ChecksumText = true,
                        GenerateChecksum = true,
                        Extended = false
                    };

                    var xx = 0;
                    string fontpath = Environment.GetEnvironmentVariable("SystemRoot") +
                     "\\fonts\\tahoma.ttf";
                    BaseFont bf = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font tahomaFont = new Font(bf, 8, Font.NORMAL, BaseColor.DARK_GRAY);

                    cb.SetColorFill(BaseColor.DARK_GRAY);
                    cb.SetFontAndSize(bf, 8);


                    ColumnText ct = new ColumnText(writer.DirectContent);
                    ct.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                    ct.SetSimpleColumn(0, (_pdfNewSizeH - 20), 170, 120, 8, Element.ALIGN_LEFT);

                    var chunk = new Chunk(OrgEngName, tahomaFont);
                    var chunk2 = new Chunk(ProjectNo + " - " + CustomerName, tahomaFont);


                    ct.AddElement(chunk2);

                    ct.AddElement(chunk);

                    ct.Go();

                    iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                    var barCodeRect = new iTextSharp.text.Rectangle(bc.BarcodeSize);
                    iTextSharp.text.Rectangle tempRect;
                    tempRect = new iTextSharp.text.Rectangle(0, 0, 140, 40);//(,,3rd,toool)

                    img.ScaleAbsolute(tempRect);
                    img.SetAbsolutePosition(30, (_pdfNewSizeH - 90));
                    cb.AddImage(img);

                    var bc2 = new BarcodeQRCode(NumberCode, 50, 50, null);
                    iTextSharp.text.Image img1 = bc2.GetImage();
                    iTextSharp.text.Rectangle tempRect2;
                    tempRect2 = new iTextSharp.text.Rectangle(0, 0, 120, 120);//(,,3rd,toool)

                    img1.ScaleAbsolute(tempRect2);
                    img1.SetAbsolutePosition((size.Width - 150), (_pdfNewSizeH - 120));

                    cb.AddImage(img1);


                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    cb.AddTemplate(page, 0, 0);

                }


                // close the streams and voilá the file should be changed :)
                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();

                return Ok(result);

            }

        [HttpPost("BarcodeFun_A3_N")]
        public IActionResult BarcodeFun_A3_N(string URL, string Filename, string NumberCode, string Rand, string ProjectNo, string OrgEngName, string CustomerName)
        {
            var result = _projectService.BarcodePDF(1, _globalshared.UserId_G);
            string File = Path.Combine(URL, Filename);
            string newFile = Path.Combine(URL, "_B_"  + Filename);
            float LAST_CELL_HEIGHT = 50f;


            // open the reader
            PdfReader reader = new PdfReader(File);
            iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
            var _pdfNewSizeW = size.Width;
            var _pdfNewSizeH = size.Height + 100;
            Rectangle newRect = new Rectangle(0, 0, Convert.ToSingle(_pdfNewSizeW), Convert.ToSingle(_pdfNewSizeH));
            //Document document = new Document(size);
            Document document = new Document(newRect);
            Document.Compress = true;
            document.SetMargins(0, 0, 0, 0);

            // open the writer
            FileStream fs = new FileStream(newFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();


            for (var i = 1; i <= reader.NumberOfPages; i++)
            {
                document.NewPage();
                // the pdf content

                PdfContentByte cb = writer.DirectContent;

                var bc = new Barcode128
                {
                    Code = NumberCode,
                    TextAlignment = Element.ALIGN_CENTER,
                    StartStopText = true,
                    CodeType = Barcode.CODE128,
                    ChecksumText = true,
                    GenerateChecksum = true,
                    Extended = false
                };

                var xx = 0;
                string fontpath = Environment.GetEnvironmentVariable("SystemRoot") +
                 "\\fonts\\tahoma.ttf";
                BaseFont bf = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font tahomaFont = new Font(bf, 8, Font.NORMAL, BaseColor.DARK_GRAY);

                cb.SetColorFill(BaseColor.DARK_GRAY);
                cb.SetFontAndSize(bf, 8);


                ColumnText ct = new ColumnText(writer.DirectContent);
                ct.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                ct.SetSimpleColumn(0, (_pdfNewSizeH - 20), 170, 120, 8, Element.ALIGN_LEFT);

                var chunk = new Chunk(OrgEngName, tahomaFont);
                var chunk2 = new Chunk(ProjectNo + " - " + CustomerName, tahomaFont);


                ct.AddElement(chunk2);

                ct.AddElement(chunk);

                ct.Go();

                iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                var barCodeRect = new iTextSharp.text.Rectangle(bc.BarcodeSize);
                iTextSharp.text.Rectangle tempRect;
                tempRect = new iTextSharp.text.Rectangle(0, 0, 140, 40);//(,,3rd,toool)

                img.ScaleAbsolute(tempRect);
                img.SetAbsolutePosition(30, (_pdfNewSizeH - 90));
                cb.AddImage(img);

                var bc2 = new BarcodeQRCode(NumberCode, 50, 50, null);
                iTextSharp.text.Image img1 = bc2.GetImage();
                iTextSharp.text.Rectangle tempRect2;
                tempRect2 = new iTextSharp.text.Rectangle(0, 0, 120, 120);//(,,3rd,toool)

                img1.ScaleAbsolute(tempRect2);
                img1.SetAbsolutePosition((size.Width - 150), (_pdfNewSizeH - 120));

                cb.AddImage(img1);


                PdfImportedPage page = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page, 0, 0);

            }


            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();

            return Ok(result);

        }

        [HttpPost("DeleteProjectRequirements")]
        public IActionResult DeleteProjectRequirements(int RequirementId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectRequirementsservice.DeleteProjectRequirements(RequirementId, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        }
    
}
