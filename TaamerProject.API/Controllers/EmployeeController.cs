using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Service.Interfaces;
using TaamerProject.Models;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Globalization;
using System.Text;
using TaamerProject.Models.Common;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class EmployeeController : ControllerBase
    {
            private IEmployeesService _employesService;
            private readonly string _Con;
            private IBranchesService _branchesService;
            private IOrganizationsService _organizationsservice;
            private IPayrollMarchesService _payrollMarchesService;
            private readonly IAllowanceService _allowanceService;
            private readonly ILoanService _loanService;
            private readonly IDiscountRewardService _discountRewardService;
            private readonly IVacationService _vacationService;
            private readonly ICityService _cityService;
            private readonly ICityPassService _cityPassService;
            private readonly IBanksService _banksService;
        private ISystemSettingsService _systemSettingsService;
        private byte[] ReportPDF;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;

        public EmployeeController(IEmployeesService employeesService, IBranchesService branchesService, IOrganizationsService organizationsService, IPayrollMarchesService payrollMarches,
                IAllowanceService allowanceService, ILoanService loanService, IDiscountRewardService discountRewardService, IVacationService vacationService, ICityService cityService,
                ISystemSettingsService systemSettingsService, ICityPassService cityPassService, IBanksService banksService, IConfiguration _configuration)
            {
                this._branchesService = branchesService;
                this._organizationsservice = organizationsService;
                this._employesService = employeesService;
                this._payrollMarchesService = payrollMarches;
            this._systemSettingsService = systemSettingsService;

            _allowanceService = allowanceService;
                _loanService = loanService;
                _discountRewardService = discountRewardService;
                _vacationService = vacationService;
                _cityService = cityService;
                _cityPassService = cityPassService;
                _banksService = banksService;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
        }
        [HttpGet("EmployeeCard")]
        public IActionResult EmployeeCard(int EmpId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var empcard = _employesService.GetEmployeeById(EmpId, _globalshared.Lang_G);
                return Ok(empcard);
            }
        [HttpGet("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Employees = _employesService.GetAllEmployees(_globalshared.Lang_G, _globalshared.BranchId_G);
                return Ok(Employees);
        }
        [HttpGet("GetAllEmployeesByLocationId")]
        public IActionResult GetAllEmployeesByLocationId(int LocationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Employees = _employesService.GetAllEmployeesByLocationId(_globalshared.Lang_G,LocationId);
            return Ok(Employees);
        }
        [HttpGet("GetAllArchivesEmployees")]
        public IActionResult GetAllArchivesEmployees()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

                var Employees = _employesService.GetAllArchivesEmployees(_globalshared.Lang_G, _globalshared.BranchId_G);
                return Ok(Employees);
            }
        [HttpGet("GetEmployeeJobName")]
        public IActionResult GetEmployeeJobName(int EmpId)
        {
            EmployeesVM emp = new EmployeesVM();
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _employesService.GetEmployeeJobName(EmpId, _globalshared.Lang_G, _globalshared.BranchId_G);
            emp.JobName = result;

            return Ok(emp);
            }
        [HttpGet("GetEmployeeJob")]
        public IActionResult GetEmployeeJob(int EmpId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.GetEmployeeJob(EmpId, _globalshared.Lang_G, _globalshared.BranchId_G));
            }
        [HttpPost("CheckifCodeIsExist")]
        public IActionResult CheckifCodeIsExist(string empCode)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _employesService.CheckifCodeIsExist(empCode, _globalshared.UserId_G, _globalshared.BranchId_G);
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "رقم الموظف موجود من قبل")
                {
                    result.ReasonPhrase = "employee number already exit";
                }
                return Ok(new { result.StatusCode, result.ReasonPhrase });
            }
        [HttpPost("SaveEmployee")]
        public IActionResult SaveEmployee([FromForm]Employees emp,IFormFile? UploadedEmployeeImage)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            

            if (UploadedEmployeeImage != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "Employees/");
                string pathW = System.IO.Path.Combine("/Uploads/", "Employees/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                string pathes = "";
                //foreach (IFormFile postedFile in postedFiles)
                //{
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedEmployeeImage.FileName);
                //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {


                    UploadedEmployeeImage.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    // string returnpath = host + path + fileName;
                    //pathes.Add(pathW + fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != null)
                {
                    emp.PhotoUrl = pathes;
                }
            }
            var result = _employesService.SaveEmployee(emp, _globalshared.UserId_G, _globalshared.BranchId_G);
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "رقم الموظف موجود من قبل")
                {
                    result.ReasonPhrase = "employee number already exit";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "قم بتغيير اسم الميل، فهو موجود بالفعل!")
                {
                    result.ReasonPhrase = "Change the name of the slope, it already exists!";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "خطأ في الحفظ, فشل في انشاء حساب للموظف تأكد من انشاء حساب رئيسي للموظفين وربطه بالفرع الحالي ")
                {
                    result.ReasonPhrase = "Error saving, failed to create an account for the client. Make sure to create a master account for clients and link it to the current branch";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ")
                {
                    result.ReasonPhrase = "Saved Falied";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
                {
                    result.ReasonPhrase = "Saved Successfully";
                }
            var result2 = _systemSettingsService.MaintenanceFunc(Con, _globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.UserId_G, 0);

            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm, result.ReturnedStr });
            }
        [HttpPost("SaveOfficialDocuments")]
        public IActionResult SaveOfficialDocuments([FromBody]Employees OffDoc)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _employesService.SaveOfficialDocuments(OffDoc, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "قم بتغيير رقم الاقامه لانه موجود بالفعل")
                {
                    result.ReasonPhrase = "Change the residence number because it already exists";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "قم بتغيير رقم جواز السفر لانه موجود بالفعل")
                {
                    result.ReasonPhrase = "Change the passport number because it already exists";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ")
                {
                    result.ReasonPhrase = "Saved Falied";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "Saved Successfully";
                }
                return Ok(new { result.StatusCode, result.ReasonPhrase });
            }


        [HttpPost("SaveAttachment")]

        public IActionResult SaveAttachment([FromForm] TaamerProject.Models.Attachment attachment, IFormFile postedFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string fileName = "";
            string pathes = "";
            if (postedFiles != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "Attachment/");
                string pathW = System.IO.Path.Combine("/Uploads/", "Attachment/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();

                //foreach (IFormFile postedFile in postedFiles)
                //{
                fileName = System.IO.Path.GetFileName(Guid.NewGuid() + postedFiles.FileName);
                //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {


                    postedFiles.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    // string returnpath = host + path + fileName;
                    //pathes.Add(pathW + fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != null)
                {
                    attachment.AttachmentUrl = pathes;
                }
            }

            var result = _employesService.Savequacontract((int)attachment.EmployeeId,attachment.AttachmentUrl, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, _globalshared.Lang_G);
            return Ok(result);
        }



        [HttpPost("SaveEmpBonus")]
        public IActionResult SaveEmpBonus(int EmpId, int bouns)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.SaveEmpBouns(EmpId, bouns, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpPost("DeleteEmplocation")]
        public IActionResult DeleteEmplocation(int EmpId, int LocationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.DeleteEmplocation(EmpId, LocationId, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpPost("AllowEmployeesites")]
        public IActionResult AllowEmployeesites(int EmpId,bool Check,int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.AllowEmployeesites(EmpId, Check, Type, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpPost("AllowEmployeesitesList")]    
        public IActionResult AllowEmployeesitesList(List<int> EmpIds, bool Check, int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.AllowEmployeesitesList(EmpIds, Check, Type, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpPost("ConvertEmplocation")]
        public IActionResult ConvertEmplocation(int EmpId, int oldLocationId, int newLocationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.ConvertEmplocation(EmpId, oldLocationId, newLocationId, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpPost("SaveEmplocation")]
        public IActionResult SaveEmplocation(int EmpId, int LocationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.SaveEmplocation(EmpId, LocationId, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpPost("SaveEmplocationList")]
        public IActionResult SaveEmplocationList(LocationDataNew locationDataNew)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.SaveEmplocationList(locationDataNew!.EmpList??new List<int>(), locationDataNew.LocationId, _globalshared.UserId_G, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        public class LocationDataNew
        {
            public int LocationId { get; set; }
            public List<int>? EmpList { get; set; }
        }


        [HttpPost("DeleteEmployee")]
        public IActionResult DeleteEmployee(int EmployeeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var result = _employesService.DeleteEmployee(EmployeeId, _globalshared.UserId_G, _globalshared.BranchId_G);
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "Deleted Successfully";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
                {
                    result.ReasonPhrase = "Deleted Falied";
                }
                return Ok(result);
            }


        [HttpPost("RemoveEmployee")]
        public IActionResult RemoveEmployee(int EmployeeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var result = _employesService.RemoveEmployee(EmployeeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Deleted Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Deleted Falied";
            }
            return Ok(result);
        }
        [HttpGet("FillEmployeeSelect")]
        public IActionResult FillEmployeeSelect(int? EmpId, bool IsNewContract = false)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                IEnumerable<Models.NodeVM> someEmps = _employesService.FillEmployeeSelect(_globalshared.Lang_G, 0, IsNewContract, EmpId);
                foreach (var userBranch in userBranchs)
                {
                    var empSelect = _employesService.FillEmployeeSelect(_globalshared.Lang_G, userBranch.BranchId, IsNewContract, EmpId);
                    var Emps = someEmps.Union(empSelect);
                    someEmps = Emps;
                }
                var Org = _organizationsservice.GetBranchOrganization(1).Result;
                List<Models.NodeVM> finalList = someEmps.ToList();
                if (Org != null & Org.RepresentorEmpId.HasValue)
                {
                    //var found = finalList.First(x => x.Id == Org.RepresentorEmpId.Value);

                    if (finalList.Where(x => x.Id == Org.RepresentorEmpId.Value).Count() == 0)
                    {
                        var orgEmp = _employesService.GetEmployeeById_d(Org.RepresentorEmpId.Value, _globalshared.Lang_G).Result;
                        if (orgEmp != null)
                        {
                            NodeVM obj = new NodeVM() { Id = orgEmp.EmployeeId, Name = orgEmp.EmployeeNameAr };
                            finalList.Add(obj);
                        }
                    }
                }
                return Ok(finalList);
            }
        [HttpGet("FillEmpAppraisSelect")]
        public IActionResult FillEmpAppraisSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.FillEmpAppraisSelect(_globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.UserId_G));
            }
        [HttpGet("GetEmployeeById")]
        public IActionResult GetEmployeeById(int EmpId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.GetEmployeeById(EmpId, _globalshared.Lang_G));
            }
        [HttpGet("GetCurrentEmployee")]
        public IActionResult GetCurrentEmployee()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.GetEmployeeByUserid(_globalshared.UserId_G).Result.FirstOrDefault());
            }
        [HttpPost("SearchEmployees")]
        public IActionResult SearchEmployees([FromBody]Models.EmployeesVM EmployeesSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.SearchEmployees(EmployeesSearch, _globalshared.Lang_G, _globalshared.BranchId_G));
            }
        [HttpPost("SearchArchiveEmployees")]
        public IActionResult SearchArchiveEmployees(Models.EmployeesVM EmployeesSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.SearchArchiveEmployees(EmployeesSearch, _globalshared.Lang_G, _globalshared.BranchId_G));
            }
        [HttpGet("GetEmployeeInfo")]
        public IActionResult GetEmployeeInfo(int EmployeeId, string? lang)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.GetEmployeeInfo(EmployeeId, lang??"", _globalshared.BranchId_G).Result);
            }

        [HttpGet("GenerateNextEmpNumber")]
        public IActionResult GenerateNextEmpNumber()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.GenerateNextEmpNumber(_globalshared.BranchId_G));
            }
        [HttpPost("GetAllEmployeeSearch")]
        public IActionResult GetAllEmployeeSearch(Models.EmployeesVM SalarySearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var data = _employesService.GetAllEmployeesSearch(SalarySearch, _globalshared.Lang_G, _globalshared.UserId_G, _globalshared.BranchId_G, Con);
            return Ok(data);
        }

        [HttpPost("GetAllEmployeeSearchNew")]
        public IActionResult GetAllEmployeeSearchNew(Models.EmployeesVM SalarySearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            bool IsAllBranch = false;
            if (SalarySearch.MonthNo == null || SalarySearch.MonthNo == 0)
            {
                SalarySearch.MonthNo = DateTime.Now.Month;
            }
            if (SalarySearch.BranchId == null || SalarySearch.BranchId == 0)
            {
                SalarySearch.BranchId = 0;
                IsAllBranch = true;
            }
            var data = _employesService.GetAllEmployeesSearch(IsAllBranch, _globalshared.Lang_G, _globalshared.UserId_G, SalarySearch.BranchId.Value, SalarySearch.MonthNo.Value,_globalshared.YearId_G, Con);
            return Ok(data);
        }
        [HttpGet("GetEmployeesForPayroll")]
        public IActionResult GetEmployeesForPayroll(bool IsAllBranch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.GetEmployeesForPayroll(IsAllBranch, _globalshared.Lang_G, _globalshared.UserId_G, _globalshared.BranchId_G, _Con));
            }

        [HttpGet("GetEmployeesForPayrollNew")]
        public IActionResult GetEmployeesForPayrollNew(bool IsAllBranch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.GetEmployeesForPayroll(IsAllBranch, _globalshared.Lang_G, _globalshared.UserId_G, _globalshared.BranchId_G,DateTime.Now.Month,_globalshared.YearId_G, _Con).DistinctBy(x=>x.EmployeeId));
        }
        [HttpPost("GetEmpPayrollMarches")]
        public IActionResult GetEmpPayrollMarches(Models.EmployeesVM SalarySearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            bool IsAllBranch = false;
                if (SalarySearch.MonthNo == null || SalarySearch.MonthNo == 0)
                {
                    SalarySearch.MonthNo = DateTime.Now.Month;
                }
                if (SalarySearch.BranchId == null || SalarySearch.BranchId == 0)
                {
                    SalarySearch.BranchId = 0;
                    IsAllBranch = true;
                }


                var emp = _employesService.GetEmployeesForPayroll(IsAllBranch, _globalshared.Lang_G, _globalshared.UserId_G, (int)SalarySearch.BranchId, Con);

                var payrolls =  _payrollMarchesService.GetPayrollMarches((int)SalarySearch.MonthNo, (int)SalarySearch.BranchId, "");
            //payrolls = payrolls.ToList();
            var payroll= payrolls.Result.Where(x => (emp.Select(y => y.EmployeeId).Contains(x.EmpId))).ToList();
                return Ok(payroll);
            }

        [HttpPost("GetEmpPayrollMarchesNew")]
        public IActionResult GetEmpPayrollMarchesNew(Models.EmployeesVM SalarySearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            bool IsAllBranch = false;
            if (SalarySearch.MonthNo == null || SalarySearch.MonthNo == 0)
            {
                SalarySearch.MonthNo = DateTime.Now.Month;
            }
            if (SalarySearch.BranchId == null || SalarySearch.BranchId == 0)
            {
                SalarySearch.BranchId = 0;
                IsAllBranch = true;
            }

            var emp = _employesService.GetEmployeesForPayroll(IsAllBranch, _globalshared.Lang_G, _globalshared.UserId_G, (int)SalarySearch.BranchId, (int)SalarySearch.MonthNo, _globalshared.YearId_G, Con);

            var payrolls = _payrollMarchesService.GetPayrollMarches((int)SalarySearch.MonthNo, (int)SalarySearch.BranchId, "", _globalshared.YearId_G);
            //payrolls = payrolls.ToList();
            var payroll = payrolls.Result.Where(x => (emp.Select(y => y.EmployeeId).Contains(x.EmpId))).ToList().DistinctBy(x => x.EmpId).ToList();

            return Ok(payroll);

          
        }
        [HttpPost("PostEmpPayroll")]
        public IActionResult PostEmpPayroll([FromBody]int payrollId)
            {
                return Ok(_payrollMarchesService.PostPayrollMarches(payrollId, _globalshared.UserId_G, _globalshared.BranchId_G));
            }
        [HttpPost("PostEmpPayroll_Back")]
        public IActionResult PostEmpPayroll_Back([FromBody]int payrollId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_payrollMarchesService.PostEmpPayroll_Back(payrollId, _globalshared.UserId_G, _globalshared.BranchId_G));
            }
        [HttpPost("PostEmpPayrollVoucher")]
        public IActionResult PostEmpPayrollVoucher(int payrollId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_payrollMarchesService.PostEmpPayrollVoucher(payrollId, _globalshared.UserId_G, _globalshared.BranchId_G));
            }
        [HttpPost("PostAllEmpPayrollVoucher")]
        public IActionResult PostAllEmpPayrollVoucher(List<int> payrollId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_payrollMarchesService.PostAllEmpPayrollVoucher(payrollId, _globalshared.UserId_G, _globalshared.BranchId_G));
            }
        [HttpPost("PostEmpPayrollPayVoucher")]
        public IActionResult PostEmpPayrollPayVoucher(int payrollId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_payrollMarchesService.PostEmpPayrollPayVoucher(payrollId, _globalshared.UserId_G, _globalshared.BranchId_G));
            }
        [HttpPost("PostALLEmpPayrollPayVoucher")]
        public IActionResult PostALLEmpPayrollPayVoucher(List<int> payrollId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_payrollMarchesService.PostALLEmpPayrollPayVoucher(payrollId, _globalshared.UserId_G, _globalshared.BranchId_G));
            }

        /// <summary>
        /// /edited
        /// </summary>
        /// <returns></returns>
        [HttpPost("PostEmployeeCheckBox")]
        public IActionResult PostEmployeeCheckBox([FromBody]List<Int32> payrollid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _payrollMarchesService.PostPayrollMarcheslist(payrollid,_globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }




        [HttpGet("FillSelectEmployee")]
        public IActionResult FillSelectEmployee()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.FillSelectEmployee(_globalshared.Lang_G, _globalshared.BranchId_G));
            }
        [HttpGet("FillSelectEmployeeWorkers")]
        public IActionResult FillSelectEmployeeWorkers()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_employesService.FillSelectEmployeeWorkers(_globalshared.Lang_G, _globalshared.BranchId_G));
            }



        [HttpPost("PrintEmployeesSalaryReport2")]
        public IActionResult PrintEmployeesSalaryReport2([FromBody]EmployeesVM SalarySearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            EmployeesSalaryRptVM_New salaryRptVM_New = new EmployeesSalaryRptVM_New();
            bool IsAllBranch = false;

            if (SalarySearch.MonthNo == null || SalarySearch.MonthNo == 0)
            {
                SalarySearch.MonthNo = DateTime.Now.Month;
            }
            if (SalarySearch.BranchId == null || SalarySearch.BranchId == 0)
            {
                SalarySearch.BranchId = 0;
                 IsAllBranch = true;

            }
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            //var Emps = _employesService.GetAllEmployeesSearch(SalarySearch, UsersData.Lang, _globalshared.UserId_G, _globalshared.BranchId_G, _Con);
            List<PayrollMarchesVM> Emps = new List<PayrollMarchesVM>();
             Emps = _payrollMarchesService.GetPayrollMarches((int)SalarySearch.MonthNo, (int)SalarySearch.BranchId, "",_globalshared.YearId_G).Result.ToList();
            if(Emps==null || Emps.Count() == 0)
            {
                var emp = _employesService.GetEmployeesForPayroll(IsAllBranch, _globalshared.Lang_G, _globalshared.UserId_G, (int)SalarySearch.BranchId, (int)SalarySearch.MonthNo, _globalshared.YearId_G, Con);

                var payrolls = _payrollMarchesService.GetPayrollMarches((int)SalarySearch.MonthNo, (int)SalarySearch.BranchId, "", _globalshared.YearId_G);
                //payrolls = payrolls.ToList();
                Emps = payrolls.Result.Where(x => (emp.Select(y => y.EmployeeId).Contains(x.EmpId))).ToList().DistinctBy(x => x.EmpId).ToList();
            }
            else
            {
                var emp = _employesService.GetEmployeesForPayrollPrint(IsAllBranch, _globalshared.Lang_G, _globalshared.UserId_G, (int)SalarySearch.BranchId, (int)SalarySearch.MonthNo, _globalshared.YearId_G, Con);
                Emps = Emps.Where(x => (emp.Select(y => y.EmployeeId).Contains(x.EmpId))).ToList().DistinctBy(x => x.EmpId).ToList();


            }

            if (SalarySearch.IsAllBranch.Value == false && SalarySearch.BranchId.Value >= 0)
            {
                var rptSource = Emps.Select(s => new EmployeesSalaryRptVM
                {
                    EmployeeNameAr = s.EmpName ?? "",
                    Salary = s.SalaryOfThisMonth.ToString() ?? "0",

                    CommunicationAllawance = s.CommunicationAllawance.HasValue ? s.CommunicationAllawance.Value.ToString() : "0",
                    ProfessionAllawance = s.ProfessionAllawance.HasValue ? s.ProfessionAllawance.Value.ToString() : "0",
                    TransportationAllawance = s.TransportationAllawance.HasValue ? s.TransportationAllawance.Value.ToString() : "0",
                    HousingAllowance = s.HousingAllowance.HasValue ? s.HousingAllowance.Value.ToString() : "0",

                    MonthlyAllowances = s.MonthlyAllowances.ToString() ?? "0",
                    AddAllowances = s.ExtraAllowances.ToString() ?? "0",
                    TotalLoans = s.TotalLoans.ToString() ?? "0",
                    Bonus = s.Bonus.ToString() ?? "0",
                    TotalyDays = s.TotalAbsDays.ToString() ?? "0",
                    TotalDiscounts = s.TotalDiscounts.ToString() ?? "0",
                    TotalRewards = s.TotalRewards.ToString() ?? "0",
                    //TotalViolations = s.TotalViolations.ToString() ?? "0",
                    TotalySalaries = s.TotalSalaryOfThisMonth.ToString() ?? "0",
                    TotalLateDiscount =s.TotalLateDiscount.ToString() ?? "0",   
                    TotalAbsenceDiscount=s.TotalAbsenceDiscount.ToString() ?? "0",
                    
                }).ToList();
                if(Emps !=null && Emps.Count() > 0)
                {
                    var total=new TotalEmployeesSalaryRptVM() { 
                        TSalary = Emps.Sum(x=>x.SalaryOfThisMonth).ToString() ?? "0",

                        TCommunicationAllawance = Emps.Sum(x => x.CommunicationAllawance).ToString() ?? "0",
                        TProfessionAllawance = Emps.Sum(x => x.ProfessionAllawance).ToString() ?? "0",
                        TTransportationAllawance = Emps.Sum(x => x.TransportationAllawance).ToString() ?? "0",
                        THousingAllowance = Emps.Sum(x => x.HousingAllowance).ToString() ?? "0",

                        TMonthlyAllowances = Emps.Sum(x => x.MonthlyAllowances).ToString() ?? "0",
                        TAddAllowances = Emps.Sum(x => x.ExtraAllowances).ToString() ?? "0",
                        TTotalLoans = Emps.Sum(x => x.TotalLoans).ToString() ?? "0",
                        TBonus = Emps.Sum(x => x.Bonus).ToString() ?? "0",
                        TTotalyDays = Emps.Sum(x => x.TotalAbsDays).ToString() ?? "0",
                        TTotalDiscounts = Emps.Sum(x => x.TotalDiscounts).ToString() ?? "0",
                        TTotalRewards = Emps.Sum(x => x.TotalRewards).ToString() ?? "0",
                        //TotalViolations = s.TotalViolations.ToString() ?? "0",
                        TTotalySalaries = Emps.Sum(x => x.TotalSalaryOfThisMonth).ToString() ?? "0",
                        TTotalLateDiscount = Emps.Sum(x => x.TotalLateDiscount).ToString() ?? "0",
                        TTotalAbsenceDiscount = Emps.Sum(x => x.TotalAbsenceDiscount).ToString() ?? "0",
                    };
                    salaryRptVM_New.Total = total;
                }
                var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId);

                //rptSource.Add(totalRow);
                salaryRptVM_New.employeesSalaries = rptSource;
                salaryRptVM_New.OrgData = objOrganization2.Result;

                //ViewData["rptSource"] = rptSource;

                if (SalarySearch.BranchId != null && SalarySearch.BranchId !=0)
                {
                    salaryRptVM_New.BranchName =_branchesService.GetBranchById((int)SalarySearch.BranchId).Result.NameAr;
                }
                else
                {
                    salaryRptVM_New.BranchName = "";

                }
                salaryRptVM_New.MonthName = (SalarySearch.MonthNo==null || SalarySearch.MonthNo ==0) ? "" : getmnth((int)SalarySearch.MonthNo);

                //string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //ViewData["Date"] = Date;

                //if (objOrganization2 != null)
                //    ViewData["Org_VD"] = objOrganization2;
                //else
                //    ViewData["Org_VD"] = null;
            }
            else
            {
                var rptSource = Emps.Select(s => new EmployeesSalaryRptVM
                {
                    EmployeeNameAr = s.EmpName ?? "",
                    Salary = s.SalaryOfThisMonth.ToString() ?? "0",

                    CommunicationAllawance = s.CommunicationAllawance.HasValue ? s.CommunicationAllawance.Value.ToString() : "0",
                    ProfessionAllawance = s.ProfessionAllawance.HasValue ? s.ProfessionAllawance.Value.ToString() : "0",
                    TransportationAllawance = s.TransportationAllawance.HasValue ? s.TransportationAllawance.Value.ToString() : "0",
                    HousingAllowance = s.HousingAllowance.HasValue ? s.HousingAllowance.Value.ToString() : "0",

                    MonthlyAllowances = s.MonthlyAllowances.ToString() ?? "0",
                    AddAllowances = (s.ExtraAllowances.ToString() == null || s.ExtraAllowances.ToString() == "") ? "0" : s.ExtraAllowances.ToString(),
                    TotalLoans = s.TotalLoans.ToString() ?? "0",
                    Bonus = s.Bonus.ToString() ?? "0",
                    TotalyDays = s.TotalAbsDays.ToString() ?? "0",
                    TotalDiscounts = s.TotalDiscounts.ToString() ?? "0",
                    TotalRewards = s.TotalRewards.ToString() ?? "0",
                    TotalLateDiscount =s.TotalLateDiscount.ToString() ?? "0",
                    TotalAbsenceDiscount=s.TotalAbsenceDiscount.ToString() ??  "0",
                    //TotalViolations = s.TotalViolations.ToString() ?? "0",
                    TotalySalaries = s.TotalSalaryOfThisMonth.ToString() ?? "0",
                }).ToList();
                if (Emps != null && Emps.Count() > 0)
                {
                    var total = new TotalEmployeesSalaryRptVM()
                    {
                        TSalary = Emps.Sum(x => x.SalaryOfThisMonth).ToString() ?? "0",

                        TCommunicationAllawance = Emps.Sum(x => x.CommunicationAllawance).ToString() ?? "0",
                        TProfessionAllawance = Emps.Sum(x => x.ProfessionAllawance).ToString() ?? "0",
                        TTransportationAllawance = Emps.Sum(x => x.TransportationAllawance).ToString() ?? "0",
                        THousingAllowance = Emps.Sum(x => x.HousingAllowance).ToString() ?? "0",

                        TMonthlyAllowances = Emps.Sum(x => x.MonthlyAllowances).ToString() ?? "0",
                        TAddAllowances = Emps.Sum(x => x.ExtraAllowances).ToString() ?? "0",
                        TTotalLoans = Emps.Sum(x => x.TotalLoans).ToString() ?? "0",
                        TBonus = Emps.Sum(x => x.Bonus).ToString() ?? "0",
                        TTotalyDays = Emps.Sum(x => x.TotalAbsDays).ToString() ?? "0",
                        TTotalDiscounts = Emps.Sum(x => x.TotalDiscounts).ToString() ?? "0",
                        TTotalRewards = Emps.Sum(x => x.TotalRewards).ToString() ?? "0",
                        //TotalViolations = s.TotalViolations.ToString() ?? "0",
                        TTotalySalaries = Emps.Sum(x => x.TotalSalaryOfThisMonth).ToString() ?? "0",
                        TTotalLateDiscount = Emps.Sum(x=>x.TotalLateDiscount).ToString() ?? "0",
                        TTotalAbsenceDiscount = Emps.Sum(x => x.TotalAbsenceDiscount).ToString() ?? "0",
                    };
                    salaryRptVM_New.Total = total;
                }
                //rptSource.Add(totalRow);
                var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId);

                salaryRptVM_New.employeesSalaries = rptSource;
                salaryRptVM_New.OrgData = objOrganization2.Result;

                //ViewData["rptSource"] = rptSource;
                if (SalarySearch.BranchId != null && SalarySearch.BranchId != 0)
                {
                    salaryRptVM_New.BranchName = _branchesService.GetBranchById((int)SalarySearch.BranchId).Result.NameAr;
                }
                else
                {
                    salaryRptVM_New.BranchName = "";

                }

                salaryRptVM_New.MonthName = (SalarySearch.MonthNo == null || SalarySearch.MonthNo == 0) ? "" : getmnth((int)SalarySearch.MonthNo);


            }

            //return PartialView("_PayRollMarches");
            return Ok(salaryRptVM_New);



        }

        [HttpGet("PrintEmployeeIdentityReports")]

        public IActionResult PrintEmployeeIdentityReports(int EmpId)
        {
            EmployeeReport employeeReport = new EmployeeReport();
            List<loanvm> loanvms = new List<loanvm>();
            List<vacVM> vacs = new List<vacVM>();
           HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            EmployeesVM emp = _employesService.GetEmployeeById(EmpId, "").Result;
            var empVacations = _vacationService.GetAllVacations(EmpId, "").Result.ToList();
            var Allowances = _allowanceService.GetAllAllowances(EmpId, "", false).Result.ToList();
            var discounts = _discountRewardService.GetAllDiscountRewards(EmpId, "").Result.Where(x => x.Type == 1).ToList();
            var loans = _loanService.GetAllLoansDataModel(EmpId).Where(x => x.Status == 2).ToList();
            var rewards = _discountRewardService.GetAllDiscountRewards(EmpId, "").Result.Where(x => x.Type == 2).ToList();

          
        
            var sumallownces = Allowances.Sum(x => x.AllowanceAmount) ?? 0;
            sumallownces += emp.Salary ?? 0;
           

            string bankname = "";
            if (emp.BankId != null)
            {

                var bank = _banksService.GetBankByID((int)emp.BankId).Result;
                if (bank != null)
                {
                    emp.BankName = bank.NameAr;
                }
            }
            if(emp.NationalIdSource != null)
            {
                employeeReport.Nationalsource = _cityService.GetCityById(emp.NationalIdSource.Value).NameAr;
            }
            if (emp.PassportSource != null && emp.PassportSource !="" && emp.PassportSource !="0" && emp.PassportSource != "NaN")
            {
                employeeReport.PassportSource = _cityService.GetCityById(Convert.ToInt32(emp.PassportSource)).NameAr;
            }
            var housingexist = Allowances.Where(x => x.AllowanceTypeName == "بدل سكن").ToList();
            if (housingexist != null && housingexist.Count() > 0)
            {
                emp.Allowances = 0;
            }
            sumallownces += emp.Allowances ?? 0;
            sumallownces += emp.OtherAllownces ?? 0;
            employeeReport.SumAllownces = sumallownces.ToString() ?? "0";
            employeeReport.Employee = emp;
            employeeReport.Vacations = empVacations;
            employeeReport.Allowances = Allowances;
            employeeReport.Loans = loans;
            employeeReport.Rewards = rewards;
            employeeReport.Discounts = discounts;
      
            if (empVacations != null && empVacations.Count() > 0)
            {
             
                    var vacreturn = empVacations.Where(x => !string.IsNullOrEmpty(x.BackToWorkDate));
                    if(vacreturn !=null && vacreturn.Count() > 0)
                {
                    employeeReport.LastVacation= vacreturn.OrderByDescending(x => DateTime.ParseExact(x.BackToWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).FirstOrDefault().BackToWorkDate;


                }
                string StatusName = "";
           decimal  Total = 0;
                int excpecTotalDays = 0;
                int actuallTotalDays = 0;
                foreach (var vacation in empVacations)
                {
                    vacVM vacVM = new vacVM();
                     StatusName = vacation.VacationStatus == 1 ? "تقديم طلب" : vacation.VacationStatus == 2 ? "موافقة" :
                         vacation.VacationStatus == 3 ? "مرفوضة " : vacation.VacationStatus == 4 ? "تحت المراجعة" : vacation.VacationStatus == 5 ? "تم تأجيلها" : "";

                    int expectedNetDays = _vacationService.GetVacationDays_WithoutHolidays(vacation.StartDate, vacation.EndDate, emp.EmployeeId, _globalshared.Lang_G, Con, (int)vacation.VacationTypeId).Count();
                    int actualNetDays = _vacationService.GetVacationDays_WithoutHolidays(vacation.StartDate,

                    string.IsNullOrEmpty(vacation.BackToWorkDate) ? DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : vacation.BackToWorkDate, emp.EmployeeId, _globalshared.Lang_G, Con, (int)vacation.VacationTypeId).Count();

                     excpecTotalDays += expectedNetDays;
                     actuallTotalDays += actualNetDays;
                    var isd = vacation.IsDiscount.Value ? "نعم" : "لا";
                    var ds = (vacation.DiscountAmount.HasValue ? vacation.DiscountAmount.Value.ToString("N2") : "--");

                    vacVM.IsDiscount = isd;
                    vacVM.VacationTypeName = vacation.VacationTypeName;
                    vacVM.StatusName = StatusName;
                    vacVM.StartDate=vacation.StartDate;
                    vacVM.EndDate=vacation.EndDate;
                    vacVM.Date=vacation.Date;
                    vacVM.BackToWorkDate=vacation.BackToWorkDate;
                    vacVM.DiscountAmount = ds;
                    vacVM.expectedNetDays = expectedNetDays.ToString();
                    vacVM.actualNetDays = actualNetDays.ToString();

                    Total += vacation.DiscountAmount.HasValue ? vacation.DiscountAmount.Value : 0;

                    vacs.Add(vacVM);


                }

                employeeReport.VacVMs= vacs;
                employeeReport.Totalvac = Total.ToString();
                employeeReport.excpecTotalDays = excpecTotalDays.ToString();
                employeeReport.actuallTotalDays = actuallTotalDays.ToString();


            }
            employeeReport.remainTotal = 0;
            if (discounts !=null  && discounts.Count() > 0)
            {
                employeeReport.TotalVal = discounts.Sum(x => x.Amount);
                employeeReport.MonthlyTotalloan=discounts.Sum(x => x.Amount);
                employeeReport.PaidTotal = discounts.Sum(x => x.Amount);
            }
            if (loans != null && loans.Count() > 0)
            {

                foreach (var loan in loans)
                {
                    if (loan.LoanDetails != null)
                    {
                        var loanDetails = loan.LoanDetails;
                        if (loanDetails.Count() > 0)
                        {
                            loanvm lonvm = new loanvm();
                            var paidLoans = loanDetails.Where(x => x.Date.Value.Month <= DateTime.Now.Month && x.Date.Value.Year <= DateTime.Now.Year).ToList();
                            var paidValue = paidLoans.Count() * paidLoans[0].Amount.Value;

                            var lonam = (loan.Amount.HasValue ? loan.Amount.Value.ToString("N2") : "--");
                            var loand = (loanDetails[0].Amount.HasValue ? loanDetails[0].Amount.Value.ToString("N2") : "--");
                            var loanp = (loan.Amount.Value - paidValue).ToString("N2");
                            lonvm.lonam = lonam;
                            lonvm.loand = loand;
                            lonvm.loanp = loanp;
                            lonvm.paidValue = paidValue.ToString();
                            lonvm.startdate = loan.StartDate;
                            lonvm.MonthNo = loan.MonthNo.ToString();


                            employeeReport.TotalVal = employeeReport.TotalVal + (loan.Amount.HasValue ? loan.Amount.Value : 0);
                            employeeReport.MonthlyTotalloan = employeeReport.MonthlyTotalloan + (loanDetails[0].Amount.HasValue ? loanDetails[0].Amount.Value : 0);
                            employeeReport.PaidTotal = employeeReport.PaidTotal + paidValue;
                            employeeReport.remainTotal = employeeReport.remainTotal + (loan.Amount.Value - paidValue);

                            loanvms.Add(lonvm);
                        }

                    }
                }
            }
            employeeReport.Loanvms = loanvms;

            if(rewards !=null && rewards.Count > 0)
            {
                employeeReport.TotalRewards = rewards.Sum(x => x.Amount);

            }
            

                return Ok(employeeReport);

           
        }





        [HttpGet("GetEmployeeStatistics")]
        public IActionResult GetEmployeeStatistics()
            {
                //بدون عقد عمل , بدون تأمين طبي, بدون عمل مباشرة, لديهم عهد, لديهم سلف , المجازين
                var statis = _employesService.GetEmployeeStatistics();
                return Ok(statis);
            }

        [HttpGet("GetEmployeesWithoutContract")]
        public IActionResult GetEmployeesWithoutContract(int? DepartmentId)
        {
            //بدون عقد عمل , بدون تأمين طبي, بدون عمل مباشرة, لديهم عهد, لديهم سلف , المجازين
            var statis = _employesService.GetEmployeeWithoutContract(DepartmentId,_globalshared.Lang_G);
            return Ok(statis);
        }



        [HttpGet("GetEmployeesWithoutContract_paging")]
        public IActionResult GetEmployeesWithoutContract_paging(int? DepartmentId, string? SearchText, int page = 1, int pageSize = 10)
        {
            //بدون عقد عمل , بدون تأمين طبي, بدون عمل مباشرة, لديهم عهد, لديهم سلف , المجازين
            var emp = _employesService.GetEmployeeWithoutContract(DepartmentId, _globalshared.Lang_G,SearchText);

            var data = GeneratePagination<EmployeesVM>.ToPagedList(emp.Result.ToList(), page, pageSize);
            var result = new PagedLists<EmployeesVM>(data.MetaData, data);
            return Ok(result);
        }


        [HttpGet("GetEmployeeArchiveStatistics")]
        public IActionResult GetEmployeeArchiveStatistics()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Archivestatis = new
                {
                    Esteqala = _employesService.GetAllArchivesEmployees(_globalshared.Lang_G, _globalshared.BranchId_G).Result.Where(x => x.ResonLeave == "استقالة").Count(),
                    Fasl = _employesService.GetAllArchivesEmployees(_globalshared.Lang_G, _globalshared.BranchId_G).Result.Where(x => x.ResonLeave == "الفصل حسب النظام").Count(),
                    ContractEnd = _employesService.GetAllArchivesEmployees(_globalshared.Lang_G, _globalshared.BranchId_G).Result.Where(x => x.ResonLeave == "عدم تجديد العقد").Count(),
                    Death = _employesService.GetAllArchivesEmployees(_globalshared.Lang_G, _globalshared.BranchId_G).Result.Where(x => x.ResonLeave == "وفاة").Count(),
                    OtherReasins = _employesService.GetAllArchivesEmployees(_globalshared.Lang_G, _globalshared.BranchId_G).Result.Where(x => x.ResonLeave != "استقالة" && x.ResonLeave != "الفصل حسب النظام" && x.ResonLeave != "عدم تجديد العقد" && x.ResonLeave != "وفاة").Count(),
                };
                return Ok(Archivestatis);
            }



        [HttpPost("GetEmpPayrollMarchescvs")]
        public IActionResult GetEmpPayrollMarchescvs(Models.EmployeesVM SalarySearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            bool IsAllBranch = false;
            if (SalarySearch.MonthNo == null || SalarySearch.MonthNo == 0)
            {
                SalarySearch.MonthNo = DateTime.Now.Month;
            }
            if (SalarySearch.BranchId == null || SalarySearch.BranchId == 0)
            {
                SalarySearch.BranchId = 0;
                IsAllBranch = true;
            }

            if(_globalshared.YearId_G < DateTime.Now.Year)
            {
                var emp = _employesService.GetEmployeesForPayroll(IsAllBranch, _globalshared.Lang_G, _globalshared.UserId_G, (int)SalarySearch.BranchId,(int)SalarySearch.MonthNo, _globalshared.YearId_G, Con);

                var payrolls = _payrollMarchesService.GetPayrollMarches((int)SalarySearch.MonthNo, (int)SalarySearch.BranchId, "",_globalshared.YearId_G);
                //payrolls = payrolls.ToList();
                var payroll = payrolls.Result.Where(x => (emp.Select(y => y.EmployeeId).Contains(x.EmpId))).ToList().DistinctBy(x => x.EmpId).ToList();

                return Ok(payroll);
            }
            else
            {
                var emp = _employesService.GetEmployeesForPayroll(IsAllBranch, _globalshared.Lang_G, _globalshared.UserId_G, (int)SalarySearch.BranchId, (int)SalarySearch.MonthNo, Con);

                var payrolls = _payrollMarchesService.GetPayrollMarches((int)SalarySearch.MonthNo, (int)SalarySearch.BranchId, "", _globalshared.YearId_G);
                //payrolls = payrolls.ToList();
                var payroll = payrolls.Result.Where(x => (emp.Select(y => y.EmployeeId).Contains(x.EmpId))).ToList().DistinctBy(x => x.EmpId).ToList();

                return Ok(payroll);
            }

       
        }
        [HttpGet("GetCurrentYear")]
        public IActionResult GetCurrentYear()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_globalshared.YearId_G);

        }



        public class SalaryData
        {
            public string EMployeeName { get; set; }
            public int AccountNumber { get; set; }
            public decimal? Salary { get; set; }
        }




        [HttpPost("GenerateRandomNo")]
        public int GenerateRandomNo()
            {
                int _min = 1000;
                int _max = 9999;
                Random _rdm = new Random();
                return _rdm.Next(_min, _max);
            }
        public static string getmnth(int month)
        {
            switch (month)
            {
                case 1:
                    return "يناير";
                case 2:
                    return "فبراير";
                case 3:
                    return "مارس";
                case 4:
                    return "ابريل";
                case 5:
                    return "مايو";
                case 6:
                    return "يونيه";
                case 7:
                    return "يوليو";
                case 8:
                    return "اغسطس";
                case 9:
                    return "سبتمبر";
                case 10:
                    return "اكتوبر";
                case 11:
                    return "نوفمبر";
                case 12:
                    return "ديسمبر";
                default:
                    return "";
            }
        }
        [HttpPost("SaveEmployeeSalaryToCsv")]
        public void SaveEmployeeSalaryToCsv(List<SalaryData> salaryData, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Write header
                writer.WriteLine("EmployeeId,BankAccountNumber,Salary");

                // Write data
                foreach (var record in salaryData)
                {
                    writer.WriteLine($"{record.EMployeeName},{record.AccountNumber},{record.Salary}");
                }
            }
        }

    }
    public class EmployeeReport
    {
        public EmployeesVM Employee { get; set; }
        public List<VacationVM> Vacations { get; set; }
        public List<Loan> Loans { get; set; }
        public List<AllowanceVM> Allowances { get; set; }
        public List<DiscountRewardVM> Rewards { get; set; }
        public List<DiscountRewardVM> Discounts { get; set; }
        public List<CityVM> Cities { get; set; }
        public List<CityPassVM> CityPasses { get; set; }
        public string? LastVacation { get; set; }
        public string? SumAllownces { get; set; }
        public decimal? TotalLoans { get; set; }
        public decimal? TotalRewards { get; set; }
        public decimal? Totaldiscount { get; set; }
        public decimal? MonthlyTotalloan { get;set; }
        public decimal? PaidTotal{ get; set; }
        public decimal? remainTotal { get; set; }
        public decimal? TotalVal { get; set; }
        public List<loanvm> Loanvms { get; set; }
        public List<vacVM> VacVMs { get; set; }
        public string? Totalvac { get; set; }
        public string? excpecTotalDays { get; set; }
        public string? actuallTotalDays { get; set; }
        public string Nationalsource { get; set; }
        public string? PassportSource { get; set; }

    }
    public class loanvm
    {
        public string? startdate { get; set; }
        public string? lonam { get; set; }
        public string? loand { get; set; }
        public string? paidValue { get; set; }
        public string? loanp { get; set; }
        public string? MonthNo { get; set; }


    }

    public class vacVM
    {
        public string? VacationTypeName { get; set;}
        public string? Date { get; set; }
        public string? IsDiscount { get; set; }
        public string? DiscountAmount { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }

        public string? expectedNetDays { get; set; }

        public string? actualNetDays { get; set; }
        public string? BackToWorkDate { get; set; }
        public string? StatusName { get; set; }

    }


}
