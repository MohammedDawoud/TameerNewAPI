using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]
    public class BackupRestoreDbController : ControllerBase
    {

            private readonly IDatabaseBackupService _DatabaseBackupService;
            private IConfiguration Configuration;
            public GlobalShared _globalshared;
            private readonly IWebHostEnvironment _hostingEnvironment;
            string? Con;
            private IProjectService _projectService;
            private IBranchesService _branchesService;
            private readonly IVoucherService _voucherService;
        private readonly IEmpContractService _EmpContractService;
        private readonly ICustomerService _customerservice;
        private readonly IAccountsService _accountsService;
        private readonly IUsersService _usersservice;
        
        

        public BackupRestoreDbController(IDatabaseBackupService databaseBackupService, IConfiguration _configuration, IWebHostEnvironment hostingEnvironment,
            IProjectService projectService, IBranchesService branchesService, IVoucherService voucherService, IEmpContractService empContractService,
            ICustomerService customerService, IAccountsService accountsService, IUsersService usersService)
            {
                _DatabaseBackupService = databaseBackupService;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = hostingEnvironment;
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            _projectService = projectService;
            _branchesService = branchesService;
            _voucherService = voucherService;
            _EmpContractService = empContractService;
            _customerservice = customerService;
            _accountsService = accountsService;
            _usersservice = usersService;


        }

        [HttpGet("GetBackupById")]

        public IActionResult GetBackupById(int Backupid)
            {
                return Ok(_DatabaseBackupService.GetDBDBackupById(Backupid));
            }

        [HttpGet("GetDBackupByIDWithDetails")]

        public IActionResult GetDBackupByIDWithDetails(int Backupid)

        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            return Ok(_DatabaseBackupService.GetDBackupByIDWithDetails(Backupid, _globalshared.Lang_G).Result);
        }


        [HttpGet("GetAllBackupData")]

        public IActionResult GetAllBackupData()
            {
                return Ok(_DatabaseBackupService.GetAllDBackup());
            }

        [HttpGet("GetAllBackupDataSearch")]

        public IActionResult GetAllBackupDataSearch()
            {
                return Ok(_DatabaseBackupService.GetAllDBackup());
            }
        [HttpPost("SendNotification")]

        public IActionResult SendNotification(List<int> Users, string Dayes, string Time)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _DatabaseBackupService.sendmailnotification(Users, _globalshared.UserId_G, _globalshared.BranchId_G, Dayes, Time);

         
                return Ok(result);
            }
        [HttpPost("SaveBackup")]

        public IActionResult SaveBackup(DatabaseBackup info)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string backupRelativePath = "Backup/"; // Your desired relative path
            string backupDIR = System.IO.Path.Combine(backupRelativePath);
            if (!System.IO.Directory.Exists(backupDIR))
            {
                System.IO.Directory.CreateDirectory(backupDIR);
            }


            var remote = Directory.GetCurrentDirectory() + "/";//_hostingEnvironment.WebRootPath;

            var result = _DatabaseBackupService.SaveDBackup(info, _globalshared.UserId_G, backupDIR, _globalshared.BranchId_G, remote,Con);
                
                return Ok(result );
            }

        [HttpPost("SaveDBackup_ActiveYear")]

        public IActionResult SaveDBackup_ActiveYear(DatabaseBackup info)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string backupRelativePath = "Backup/"; // Your desired relative path
            string backupDIR = System.IO.Path.Combine(backupRelativePath);
            if (!System.IO.Directory.Exists(backupDIR))
                {
                    System.IO.Directory.CreateDirectory(backupDIR);
                }


            var remote = Directory.GetCurrentDirectory() +"/";//_hostingEnvironment.WebRootPath;

            var result = _DatabaseBackupService.SaveDBackup_ActiveYear(info, _globalshared.UserId_G, backupDIR, _globalshared.BranchId_G, remote, _globalshared.YearId_G,Con);
              
                return Ok(result);
            }

        [HttpPost("SavespecificBackup")]

        public IActionResult SavespecificBackup()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string backupRelativePath = "Backup/"; // Your desired relative path
            string backupDIR = _hostingEnvironment.WebRootPath + backupRelativePath;
            if (!System.IO.Directory.Exists(backupDIR))
                {
                    System.IO.Directory.CreateDirectory(backupDIR);
                }


                var result = _DatabaseBackupService.SaveDBackup2(backupDIR,Con);
               
                return Ok(result);
            }
        [HttpPost("DeleteBackup")]

        public IActionResult DeleteBackup(int BackupId)
            {
            string backupRelativePath = "Backup/"; // Your desired relative path
            string backupDIR = _hostingEnvironment.WebRootPath + backupRelativePath;
            var result = _DatabaseBackupService.DeleteBackup(BackupId,_globalshared.UserId_G, _globalshared.BranchId_G, backupDIR);
                return Ok(result);
            }

        [HttpPost("DownloadBackupFile")]

        public IActionResult DownloadBackupFile(int BackupId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            string filepath = "";
                string filename = "";
                string URII;
                var BackupFile = _DatabaseBackupService.GetAllDBackup().Result.Where(w => w.BackupId == BackupId).ToList();
                if (BackupFile != null)
                {
                    filepath = Convert.ToString(BackupFile.Select(s => s.LocalSavedPath).FirstOrDefault());
                    filename = Convert.ToString(BackupFile.Select(s => s.SavedName).FirstOrDefault());
                }
                string FullPath = "/Backup/" + filename;
                bool result = true;
                var resultTxt = "";
                try
                {
                    var UrlS = _hostingEnvironment.WebRootPath;
                    StringBuilder sb = new StringBuilder(UrlS);
                    sb.Replace("BackupRestoreDb/DownloadBackupFile", FullPath);
                    URII = sb.ToString();

                }
                catch
                {
                
                    return Ok( result);
                }

                return Ok(new { Filepath = FullPath, File_name = "test" });

            }
        [HttpPost("RestoreBackupFile")]

        public IActionResult RestoreBackupFile(int BackupId)
            {

                SqlConnection con = new SqlConnection(Con);
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Con);

                string filepath = "";
                string filename = "";
                string URII;
                var BackupFile = _DatabaseBackupService.GetAllDBackup().Result.Where(w => w.BackupId == BackupId).ToList();

                if (BackupFile != null)
                {
                    filepath = Convert.ToString(BackupFile.Select(s => s.LocalSavedPath).FirstOrDefault());
                    filename = Convert.ToString(BackupFile.Select(s => s.SavedName).FirstOrDefault());

                }

                string FullPath = "/Uploads/BackupDb/" + filename + ".Bak";
                var UrlS = _hostingEnvironment.WebRootPath;
                StringBuilder sb = new StringBuilder(UrlS);
                sb.Replace("BackupRestoreDb/RestoreBackupFile", FullPath);
                URII = sb.ToString();

                try
                {
                    con.Open();


                    sqlcmd = new SqlCommand("Restore database " + builder.InitialCatalog + " From disk='" + URII + "'", con);
                    sqlcmd.ExecuteNonQuery();

                    con.Close();


                }
                catch (Exception ex)
                {

                    return Ok(new { Result = false, Message = "Resources.MDa_RestoreFailed" });
                }


                return Ok(new { Result = true, Message = "Resources.MDa_RestoreSuccess" });

            }
        [HttpPost("DownloadFile")]

        public IActionResult DownloadFile(int BackupId)
            {
                //  string filepath = "";
                string filename = "";
                //  string URII;
                var BackupFile = _DatabaseBackupService.GetAllDBackup().Result.Where(w => w.BackupId == BackupId).ToList();

                if (BackupFile != null)
                {
                    filename = Convert.ToString(BackupFile.Select(s => s.SavedName).FirstOrDefault());

                }



                filename = filename + ".Bak";
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "Uploads\\BackupDb\\" + filename;
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = "";// MimeMapping.GetMimeMapping(filepath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                return File(filedata, contentType);
            }
        [HttpPost("GetFileToupload")]

        public IActionResult GetFileToupload()
            {
                //  string filepath = "";
                string filename = "";
                //  string URII;

                var BackupFile = _DatabaseBackupService.GetAllDBackup().Result.OrderByDescending(p => p.BackupId).FirstOrDefault();

                if (BackupFile != null)
                {
                    //  filepath = Convert.ToString(BackupFile.Select(s => s.LocalSavedPath).FirstOrDefault());
                    filename = Convert.ToString(BackupFile.SavedName);

                }

                //   string FullPath = "/Uploads/BackupDb/" + filename + ".Bak";



                string filepath = AppDomain.CurrentDomain.BaseDirectory + "Backup\\" + filename;
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = "";// MimeMapping.GetMimeMapping(filepath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };
                string base64 = Convert.ToBase64String(filedata, 0, filedata.Length);

                return Content(base64);
            }

        [HttpGet("GetackupFileName")]

        public IActionResult GetackupFileName()
            {
                //  string filepath = "";
                string filename = "";
                //  string URII;

                var BackupFile = _DatabaseBackupService.GetAllDBackup().Result.OrderByDescending(p => p.BackupId).FirstOrDefault();

                if (BackupFile != null)
                {
                    filename = Convert.ToString(BackupFile.SavedName);

                }



                return Content(filename);

            }

        [HttpPost("UploadfileDriveFile")]

        public IActionResult UploadfileDriveFile(int BackupId)
            {
                bool result = true;
                var resultTxt = "";
                try
                {
                    string filepath = "";
                    string filename = "";
                    string URII;
                    var BackupFile = _DatabaseBackupService.GetAllDBackup().Result.Where(w => w.BackupId == BackupId).ToList();
                    if (BackupFile != null)
                    {
                        filepath = Convert.ToString(BackupFile.Select(s => s.LocalSavedPath).FirstOrDefault());
                        filename = Convert.ToString(BackupFile.Select(s => s.SavedName).FirstOrDefault());
                    }
                    string FullPath = "/Backup/" + filename;

                
                    string backupRelativePath = "GoogleDriveFiles/"; // Your desired relative path
                    string pth = _hostingEnvironment.WebRootPath + backupRelativePath;

                    Path.GetFileName(filename);
                var cspth = _hostingEnvironment.WebRootPath;// System.Web.Hosting.HostingEnvironment.MapPath("~/");
                var fldrpth = _hostingEnvironment.WebRootPath;// System.Web.Hosting.HostingEnvironment.MapPath("~/");
                var mim = "";// MimeMapping.GetMimeMapping(FullPath);
                    var flpth = cspth + "Backup/" + filename; ;
                    var res = _DatabaseBackupService.UplaodFileOnDrive(filename, pth, cspth, fldrpth, mim, flpth);
                    return Ok(res);

                }
                catch (Exception ex)
                {
                    result = false;
                  
                    return Ok(new { result, ex.Message });
                }

            }      

        [HttpGet("GetAllStatistics")]
        public IActionResult GetAllStatistics()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            BackupStatistics statistics = new BackupStatistics();
            statistics = _DatabaseBackupService.GetBackupStatistics(_globalshared.Lang_G);
            //اجمالي المصروفات
            var Revenu = _accountsService.GetDetailedRevenu(null, "", "", _globalshared.BranchId_G, Con, _globalshared.YearId_G);
            statistics.TotalDetailedRevenu = Revenu.Result.Sum(item => Convert.ToDouble(item.TotalValue)).ToString();

            //اجمالي الايرادات
            var Expensesd = _accountsService.GetDetailedExpensesd(null, "", "", "", _globalshared.BranchId_G, Con, _globalshared.YearId_G);
            statistics.TotalDetailedExpensed = Expensesd.Result.Sum(item => Convert.ToDouble(item.Price)).ToString();


            return Ok(statistics);
        }
    }
    
}
