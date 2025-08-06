using Dropbox.Api.Users;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Graph.Models.TermStore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using static Dropbox.Api.Files.PathOrLink;
using static Dropbox.Api.Files.SearchMatchType;
using Twilio.TwiML.Messaging;
using TaamerProject.Models.Enums;

namespace TaamerProject.Service.Services
{
    public class EmpContractService :  IEmpContractService
    {


        private readonly IEmpContractRepository _empContractRepository;
        private readonly IEmpContractDetailRepository _empContractDetailRepository;
        private readonly IEmployeesRepository _EmpRepository;
        private readonly INationalityRepository _NationalityRepository;
        private readonly IBranchesRepository _BranchRepository;
        private readonly ICustomerMailService _customerMailService;
        private readonly IUserPrivilegesRepository _UserPrivilegesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IOrganizationsService _organizationsService;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISettingsRepository _SettingsRepository;
        private readonly IProjectPhasesTasksRepository _ProjectPhasesTasksRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly IWorkOrdersRepository _workordersRepository;
        private readonly ICustodyRepository _CustodyRepository;
         private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IEmployeesService _employeesService;
        private readonly IPayrollMarchesRepository _payrollMarches;
        private readonly IAllowanceRepository _allowanceRepository;
        private readonly INotificationService _notificationService;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public EmpContractService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IEmpContractRepository empContractRepository,
            IEmpContractDetailRepository empContractDetailRepository, IEmployeesRepository EmpRepository,
            INationalityRepository NationalityRepository, IBranchesRepository BranchRepository, ICustomerMailService customerMailService
            , IUserPrivilegesRepository UserPrivilegesRepository, IUsersRepository usersRepository, IOrganizationsService organizationsService
            , IDepartmentRepository departmentRepository, ISettingsRepository SettingsRepository, IProjectPhasesTasksRepository ProjectPhasesTasksRepository,
            IProjectRepository ProjectRepository, IWorkOrdersRepository workordersRepository, ICustodyRepository CustodyRepository
           , IUserNotificationPrivilegesService userNotificationPrivilegesService, INotificationRepository NotificationRepository,
            IEmployeesService employeesService, IPayrollMarchesRepository payrollMarches, IAllowanceRepository allowanceRepository,
            INotificationService notificationService

            )
        {
            _empContractRepository = empContractRepository;
            _empContractDetailRepository = empContractDetailRepository;
            _EmpRepository = EmpRepository;
            _NationalityRepository = NationalityRepository;
            _BranchRepository = BranchRepository;
            _customerMailService = customerMailService;
            _UserPrivilegesRepository = UserPrivilegesRepository;
            _usersRepository = usersRepository;
            _organizationsService = organizationsService;
            _departmentRepository = departmentRepository;
            _SettingsRepository = SettingsRepository; 
            _ProjectPhasesTasksRepository = ProjectPhasesTasksRepository;
            _ProjectRepository = ProjectRepository;
            _workordersRepository = workordersRepository;
            _CustodyRepository = CustodyRepository;
          
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _NotificationRepository = NotificationRepository;
            _payrollMarches = payrollMarches;
            _employeesService = employeesService;
            _allowanceRepository= allowanceRepository;
            _notificationService = notificationService;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<IEnumerable<EmpContractVM>> GetAllEmpContract(string lang,int BranchId)
        {
            var employees = _empContractRepository.GetAllEmpContract(lang,BranchId);
            return employees;
        }

        public Task<EmpContractVM> GetEMployeeContractByEmp(int EmpId)
        {
            var employees = _empContractRepository.GetEMployeeContractByEmp(EmpId);
            return employees;
        }
        public Task<IEnumerable<EmpContractVM>> GetAllEmpContract(string lang, int SearchAll, int Branch)
        {
            var employees = _empContractRepository.GetAllEmpContract(lang, SearchAll, Branch);
            return employees;
        }


        public Task<IEnumerable<EmpContractVM>> GetLastEmpContractSearch(int contractid, string lang)
        {
           
                return _empContractRepository.GetEmpcoById(contractid, lang);
           
        }


        public async Task<IEnumerable<EmpContractVM>> GetAllEmpContractSearch(EmpContractVM Search, string lang, int BranchId)
        
        {
            if ((bool)Search.IsSearch)
            {
                return await _empContractRepository.GetAllEmpContractBySearchObject(Search, lang, BranchId);
            }
            else
            {
                return await _empContractRepository.GetAllEmpContractSearch(lang,BranchId);
            }
        }
        public GeneralMessage SaveEmpContract(EmpContract data, int UserId, int BranchId,int? Year, string lang)
        {
            int Bra =Convert.ToInt32( _EmpRepository.GetEmployeeById(data.EmpId, lang).Result.BranchId);
            int Nat = Convert.ToInt32(_EmpRepository.GetEmployeeById(data.EmpId, lang).Result.NationalityId);
            int? branchid = _BranchRepository.GetAllBranches(lang).Result.Where(w => w.BranchId == Bra).Select(s => s.BranchId).FirstOrDefault();
            int? NationalityId = _NationalityRepository.GetAllNationalitiesById(Nat).Result.Select(s => s.NationalityId).FirstOrDefault();
            int? orgId = _BranchRepository.GetAllBranches(lang).Result.Where(w => w.BranchId == Bra).Select(s => s.OrganizationId).FirstOrDefault();
            if (branchid <= 0)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عقد موظف";
                 _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.MC_EnterBranchForEachEmp, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.MC_EnterBranchForEachEmp };
                }
            if (NationalityId <= 0)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عقد موظف";
                 _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1,  Resources.MC_EnternationalityForEachEmp, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.MC_EnternationalityForEachEmp };
                }

            var EmpContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && data.ContractId == 0 && s.EmpId == data.EmpId).FirstOrDefault();
            if(EmpContract != null)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عقد موظف";
                 _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.MC_EmpContractCodeExist, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.MC_EmpContractAlreadyExists };
                }

          //  var codeExist = _empContractRepository.GetMatching(s => s.IsDeleted == false && s.ContractId != data.ContractId && s.ContractCode == data.ContractCode).FirstOrDefault();
            var codeExist = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.ContractId != data.ContractId && s.ContractCode == data.ContractCode).FirstOrDefault();


            if (codeExist != null)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عقد موظف";
                 _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.MC_EmpContractCodeExist, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.MC_EmpContractCodeExist };
                }

            try
            {
               
                if (Year == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ عقد موظف";
                     _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.choosefinYear, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.choosefinYear };
                }               
                if (data.ContractId == 0)
                {
                  
                    data.AddUser = UserId;
                    data.AddDate = DateTime.Now;
                    data.IsDeleted = false;
                    data.BranchId =Convert.ToInt32( branchid);
                    data.NationalityId = Convert.ToInt32(NationalityId);
                    data.OrgId =  Convert.ToInt32(orgId);
                    data.EmpHourlyCost =(data.DailyEmpCost / data.Dailyworkinghours);
                    _TaamerProContext.EmpContract.Add(data);
                    //-----------------------------------------------------
                    // var EmployeeUpdated = _EmpRepository.GetById(data.EmpId);
                    Employees? EmployeeUpdated = _TaamerProContext.Employees.Where(s => s.EmployeeId == data.EmpId).FirstOrDefault();

                    EmployeeUpdated.ContractNo = data.ContractCode;
                    EmployeeUpdated.ContractStartDate = data.StartDatetxt;
                    EmployeeUpdated.ContractEndDate = data.EndDatetxt;
                    EmployeeUpdated.Salary = data.FreelanceAmount;
                    EmployeeUpdated.DailyWorkinghours = data.Dailyworkinghours;
                    EmployeeUpdated.EmpHourlyCost =data.DailyEmpCost ;
                    //EmployeeUpdated.VacationsCount = data.Durationofannualleave;
                    //-------------------------------------------------------
                    try {
                        var ObjList = new List<object>();
                        foreach (var item in data.EmpContractDetails.ToList())
                        {

                            ObjList.Add(new { item.ContractId });
                            item.ContractId = data.ContractId;
                            item.SerialId = item.SerialId;
                            item.Clause = item.Clause;
                            item.AddDate = DateTime.Now;
                            item.AddUser = UserId;
                            item.IsDeleted = false;
                            _TaamerProContext.EmpContractDetail.Add(item);
                        }
                    }
                    catch {
                        //string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        //string ActionNote = "فشل في حفظ عقد موظف";
                        // _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        ////-----------------------------------------------------------------------------------------------------------------

                        //return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed};
                    }

                    _TaamerProContext.SaveChanges();
                    //To Make or update payrolls
                    //_employeesService.GetAllEmployeesSearch( new EmployeesVM() { IsSearch= true, MonthNo = DateTime.Now.Month},lang, UserId, branchid.Value, Con);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة عقد موظف جديد";
                     _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1,Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };


                }
                else
                {
                   // var Updated = _empContractRepository.GetById(data.ContractId);
                    EmpContract? Updated = _TaamerProContext.EmpContract.Where(s => s.ContractId == data.ContractId).FirstOrDefault();

                    if (Updated != null)
                    {
                        Updated.CompanyRepresentativeId = data.CompanyRepresentativeId;
                        Updated.Compensation = data.Compensation;
                        Updated.CompensationBothParty = data.CompensationBothParty;
                        Updated.ContTypeId = data.ContTypeId;
                        Updated.ContDuration = data.ContDuration;
                        Updated.ContractTerminationNotice = data.ContractTerminationNotice;
                        Updated.Dailyworkinghours = data.Dailyworkinghours;
                        Updated.Durationofannualleave = data.Durationofannualleave;
                        
                        Updated.EndDatetxt = data.EndDatetxt;
                        Updated.Firstpartycompensation = data.Firstpartycompensation;
                        Updated.FreelanceAmount = data.FreelanceAmount;
                        Updated.HijriDate = data.HijriDate;
                        Updated.NotTodivulgeSecrets = data.NotTodivulgeSecrets;
                        Updated.NotTodivulgeSecretsDuration = data.NotTodivulgeSecretsDuration;
                        Updated.Paycase = data.Paycase;
                        Updated.PerSe = data.PerSe;
                        Updated.ProbationDuration = data.ProbationDuration;
                        Updated.ProbationTypeId = data.ProbationTypeId;
                        Updated.Restrictedmode = data.Restrictedmode;
                        Updated.RestrictionDuration = data.RestrictionDuration;
                        Updated.Secondpartycompensation = data.Secondpartycompensation;
                        Updated.SecretsIdentifyplaces = data.SecretsIdentifyplaces;
                        Updated.SecretsWithregardtowork = data.SecretsWithregardtowork;
                        Updated.StartDatetxt = data.StartDatetxt;
                        Updated.Withregardtowork = data.Withregardtowork;
                        Updated.Workingdaysperweek = data.Workingdaysperweek;
                        Updated.Workinghoursperweek = data.Workinghoursperweek;
                        Updated.DailyEmpCost = data.DailyEmpCost;
                        data.EmpHourlyCost = (data.DailyEmpCost / data.Dailyworkinghours);

                        Updated.UpdateDate = DateTime.Now;
                        Updated.UpdateUser = UserId;

                        //-----------------------------------------------------
                        //var EmployeeUpdated = _EmpRepository.GetById(data.EmpId);
                        Employees? EmployeeUpdated = _TaamerProContext.Employees.Where(s => s.EmployeeId == data.EmpId).FirstOrDefault();

                        //if (Updated.EmpId != data.EmpId)
                        //{
                        //    var oldEmp = _EmpRepository.GetById(data.EmpId);
                        //    oldEmp.ContractNo = null;
                        //    oldEmp.ContractStartDate = null;
                        //    oldEmp.ContractEndDate = null;
                        //    oldEmp.Salary = null;
                        //}

                        //Updated.EmpId = data.EmpId;
                        Updated.ContractCode = data.ContractCode;
                        EmployeeUpdated.ContractNo = data.ContractCode;
                        EmployeeUpdated.ContractStartDate = data.StartDatetxt;
                        EmployeeUpdated.ContractEndDate = data.EndDatetxt;
                        EmployeeUpdated.Salary = data.FreelanceAmount;
                        EmployeeUpdated.DailyWorkinghours = data.Dailyworkinghours;
                        EmployeeUpdated.EmpHourlyCost = data.DailyEmpCost;
                        //EmployeeUpdated.VacationsCount = data.Durationofannualleave;
                    }

                    try
                    {
                        //delete existing details 
                        if (Updated.EmpContractDetails != null) {
                        _TaamerProContext.EmpContractDetail.RemoveRange(Updated.EmpContractDetails.ToList());
                        }
                        try
                        {
                            // add new details
                            var ObjList = new List<object>();
                            foreach (var item in data.EmpContractDetails.ToList())
                            {

                                ObjList.Add(new { item.ContractId });
                                item.ContractId = data.ContractId;
                                item.SerialId = item.SerialId;
                                item.Clause = item.Clause;
                                item.AddUser = UserId;
                                item.AddDate = DateTime.Now;
                                item.IsDeleted = false;
                                _TaamerProContext.EmpContractDetail.Add(item);
                            }


                        }
                        catch (Exception ex)
                        {
                            ////-----------------------------------------------------------------------------------------------------------------
                            //string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            //string ActionNote = "فشل في حفظ عقد موظف";
                            // _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                            ////-----------------------------------------------------------------------------------------------------------------

                            //return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed};
                        }
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل عقد موظف رقم " + data.ContractId;
                         _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };

                    }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في حفظ عقد موظف";
                         _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed};
                    }
                }


            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عقد موظف";
                 _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed};
            }


            
        }
        public GeneralMessage BeginNewEmployeeWork(EmpContract data, int User,int BranchId) {
            try
            {
                var EmpContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.ContractId == data.ContractId && s.EmpId == data.EmpId).FirstOrDefault();
                //var EmployeeUpdated = _EmpRepository.GetMatching(x => x.EmployeeId == data.EmpId).FirstOrDefault();
                var EmployeeUpdated = _TaamerProContext.Employees.Where(x => x.EmployeeId == data.EmpId).FirstOrDefault();

                if (EmployeeUpdated == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل مباشرة عمل";
                     _SystemAction.SaveAction("BeginNewEmployeeWork", "EmpContractService", 1, "لا يوجد موظف لهذا العقد", "", "", ActionDate2, User, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                EmpContract.StartWorkDate = data.StartWorkDate;

                EmployeeUpdated.WorkStartDate = data.StartWorkDate;
                EmployeeUpdated.UpdateDate = DateTime.Now;
                EmployeeUpdated.UpdateUser = User;

                _TaamerProContext.SaveChanges();

                #region Notifications

                if (EmployeeUpdated.Email != null)
                {
                    var NewUser = EmployeeUpdated.UserId.Value;


                    string OrgName = _organizationsService.GetBranchOrganization().Result.NameAr;
                    string DepartmentNameAr = "";
                    Department? DepName = _TaamerProContext.Department.Where(s => s.DepartmentId == EmployeeUpdated.DepartmentId).FirstOrDefault();
                    if (DepName != null)
                    {
                        DepartmentNameAr = DepName.DepartmentNameAr;
                    }

                    //string BranchName = _BranchRepository.GetById(EmployeeUpdated.BranchId).NameAr;
                    string NameAr = "";
                    Branch? BranchName = _TaamerProContext.Branch.Where(s => s.BranchId == EmployeeUpdated.BranchId).FirstOrDefault();
                    var job=_TaamerProContext.Job.FirstOrDefault(x=>x.JobId== EmployeeUpdated.JobId);
                    if (BranchName != null)
                    {
                        NameAr = BranchName.NameAr;
                    }
                    var directmanager = _TaamerProContext.Employees.Where(x => x.EmployeeId == EmployeeUpdated.DirectManager).FirstOrDefault();

                    var htmlBody = @"<!DOCTYPE html><html lang = ''><head><meta name='viewport' content='width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=0'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
                        <meta charset = 'utf-8><meta name = 'description' content = ''><meta name = 'keywords' content = ''><meta name = 'csrf-token' content = ''><title></title><link rel = 'icon' type = 'image/x-icon' href = ''></head>
                        <body style = 'background:#f9f9f9;direction:rtl'><div class='container' style='max-width:630px;padding-right: var(--bs-gutter-x, .75rem); padding-left: var(--bs-gutter-x, .75rem); margin-right: auto;  margin-left: auto;'>
                        <style> .bordered {width: 550px; height: 700px; padding: 20px;border: 3px solid yellowgreen; background-color:lightgray;} </style>
                        <div class= 'row' style = 'font-family: Cairo, sans-serif'>  <div class= 'card' style = 'padding: 2rem;background:#fff'> <div style = 'width: 550px; height: 700px; padding: 20px; border: 3px solid yellowgreen; background-color: lightgray;'> <p style='text-align:center'></p>
                        <h4> عزيزي الموظف "+ EmployeeUpdated.EmployeeNameAr+"</h4> <h4> السلام عليكم ورحمة الله وبركاتة</h4> <h3 style = 'text-align:center;' > يسر " + OrgName + "  ان يعبر عن سعادته بانضمامكم اليه ونسال الله لك التوفيق</h3><table align = 'center' border = '1' ><tr> <td>  الموظف</td><td>" + EmployeeUpdated.EmployeeNameAr + @"</td> </tr> <tr> <td>   الوظيفه</td><td>" + job.JobNameAr + @"</td> </tr><tr> <td>   القسم  </td> <td>" + DepartmentNameAr + @"</td>
                         </tr> <tr> <td>   الفرع</td> <td>" + NameAr + @"</td> </tr> <tr> <td>  تاريخ المباشرة  </td> <td>" + EmpContract.StartWorkDate + @"</td> </tr> <tr> <td>   المدير المباشر  </td> <td>"+ directmanager?.EmployeeNameAr + @"</td> </tr></table><h4>صورة مع التحية للمدير المباشر للموظف</h4> <p style = 'text-align:center'> " + OrgName + @" </p> <h7> مع تحيات قسم ادارة الموارد البشرية</h7>
	
                        </div> </div></div></div></body></html> ";

                    
                    {
                        var user = (NewUser==null ||NewUser ==0) ?User : NewUser;
                       // var userObj = _usersRepository.GetById(user);
                        Users? userObj = _TaamerProContext.Users.Where(s => s.UserId == user).FirstOrDefault();
                        //Mail

                        // Get Configurations for employee  
                        var Note_Cinfig = _employeesService.GetNotificationRecipients(NotificationCode.HR_EmployeeStart, EmployeeUpdated.EmployeeId);
                        var desc = Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar"));
                        if (Note_Cinfig.Description != null && Note_Cinfig.Description != "")
                            desc = Note_Cinfig.Description;
                        if (Note_Cinfig.Users != null && Note_Cinfig.Users.Count() > 0)
                        {
                            foreach (var usr in Note_Cinfig.Users)
                            {
                                string NotStr = "تم انضمام الموظف " + EmployeeUpdated.EmployeeNameAr + " إلى فريق " + OrgName + ", الوظيفة: " + EmpContract.PerSe + " قسم : " + DepartmentNameAr + " فرع: " + NameAr;
                                //Notifications
                                var UserNotification = new Notification();
                                UserNotification.ReceiveUserId = usr;
                                UserNotification.Name = desc;// Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar"));
                                UserNotification.Date = EmpContract.StartWorkDate;
                                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                                UserNotification.SendUserId = 1;
                                UserNotification.Type = 1; // notification
                                UserNotification.Description = NotStr;
                                UserNotification.AllUsers = false;
                                UserNotification.SendDate = DateTime.Now;
                                UserNotification.ProjectId = 0;
                                UserNotification.TaskId = 0;
                                UserNotification.IsHidden = false;
                                UserNotification.AddUser = User;
                                UserNotification.AddDate = DateTime.Now;
                                _TaamerProContext.Notification.Add(UserNotification);
                                _TaamerProContext.SaveChanges();
                                _notificationService.sendmobilenotification(usr,desc, NotStr);
                                bool mail = _customerMailService.SendMail_SysNotification((int)EmpContract?.BranchId, User, usr, desc, htmlBody, true);

                            }
                        }
                        else
                        {
                            if (EmployeeUpdated.Email != null && EmployeeUpdated.Email != "")
                            {
                                bool mail = _customerMailService.SendMail_SysNotification((int)EmpContract?.BranchId, User, user, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), htmlBody, true, EmployeeUpdated.Email);
                                if (directmanager != null)
                                {
                                    _customerMailService.SendMail_SysNotification((int)EmpContract.BranchId, User, user, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), htmlBody, true, directmanager?.Email);

                                }
                            }
                            string NotStr = "تم انضمام الموظف " + EmployeeUpdated.EmployeeNameAr + " إلى فريق " + OrgName + ", الوظيفة: " + EmpContract.PerSe + " قسم : " + DepartmentNameAr + " فرع: " + NameAr;
                            //Notifications
                            var UserNotification = new Notification();
                            UserNotification.ReceiveUserId = user;
                            UserNotification.Name = Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar"));
                            UserNotification.Date = EmpContract.StartWorkDate;
                            UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                            UserNotification.SendUserId = 1;
                            UserNotification.Type = 1; // notification
                            UserNotification.Description = NotStr;
                            UserNotification.AllUsers = false;
                            UserNotification.SendDate = DateTime.Now;
                            UserNotification.ProjectId = 0;
                            UserNotification.TaskId = 0;
                            UserNotification.IsHidden = false;
                            UserNotification.AddUser = User;
                            UserNotification.AddDate = DateTime.Now;
                            _TaamerProContext.Notification.Add(UserNotification);
                            if (directmanager != null)
                            {
                                var directmngernot = new Notification();
                                directmngernot = UserNotification;
                                directmngernot.ReceiveUserId = directmanager.UserId;
                                _TaamerProContext.Notification.Add(directmngernot);

                            }
                            _TaamerProContext.SaveChanges();
                            _notificationService.sendmobilenotification(user, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), NotStr);
                            if (directmanager != null)
                            {
                                _notificationService.sendmobilenotification(directmanager.UserId.Value, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), NotStr);
                            }
                            //SMS
                            var res = _userNotificationPrivilegesService.SendSMS(EmployeeUpdated.Mobile, NotStr, User, BranchId);
                        }
                    }
                }

                #endregion
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "مباشرة عمل ";
                 _SystemAction.SaveAction("BeginNewEmployeeWork", "EmpContractService", 1,Resources.General_SavedSuccessfully, "", "", ActionDate, User, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل مباشرة عمل";
                 _SystemAction.SaveAction("BeginNewEmployeeWork", "EmpContractService", 1, "فشل في إرسال البريد الإلكتروني", "", "", ActionDate, User, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.FailedToSendEmail };
            }
        }

        
        public rptEmpEndWorkVM GetEmpdatatoendwork(EmpContract data, int User, string Lang, int BranchId)
        {
           
            rptEmpEndWorkVM empdata = new rptEmpEndWorkVM();
           
            try {
            var Employee = _EmpRepository.GetEmployeeById(data.EmpId,Lang);
            // var EmployeeUpdated = _EmpRepository.GetById(data.EmpId);
            Employees? EmployeeUpdated = _TaamerProContext.Employees.Where(s => s.EmployeeId == data.EmpId).FirstOrDefault();
                int LoansCount = 0;
                var branch = _BranchRepository.GetBranchByBranchId(Lang, (int)Employee.Result.BranchId).Result.FirstOrDefault();
                if (EmployeeUpdated.Loans != null) { 
            var loans = EmployeeUpdated.Loans.Where(x => !x.IsDeleted).ToList();
          
            DateTime Today = DateTime.Now.Date;
            foreach (var loan in loans)
            {
                if (loan.LoanDetails.Count > 0)
                {
                    DateTime MaxDate = loan.LoanDetails.Select(x => x.Date.HasValue ? x.Date.Value : DateTime.MinValue).Max();
                    if (MaxDate.Year >= Today.Year && MaxDate.Month >= Today.Month)
                        LoansCount= (int)(LoansCount + loan.Amount);
                }
            }
                }
                //var CustodyCount = _CustodyRepository.GetMatching(x => !x.IsDeleted && x.EmployeeId == EmployeeUpdated.EmployeeId && x.Status == false);
                var CustodyCount = _CustodyRepository.GetSomeCustodyByEmployeeId(EmployeeUpdated.EmployeeId, false);
                string cust2 = "";
            var cutemp = 0;
                int count = 1;
            if(CustodyCount.Result.Count() > 0) { 
            foreach (var cust in CustodyCount.Result)
            {
                       
                        if(cust.CustodyValue != null) { 
                cutemp = (int)(cutemp + cust.CustodyValue);
                        }
                        else
                        {
                            cust2 = cust2 +count.ToString()+"-"+ cust.ItemName;
                        }
                        count++;

                    }
                }
                var totalallownces = 0;
                var allownc = _allowanceRepository.GetAllAllowances(Employee.Result.EmployeeId, "", false);
                if (allownc.Result.Count() > 0) { 
                foreach (var item in allownc.Result)
                {
                        if(item.AllowanceAmount !=null)
                        totalallownces = (int)(totalallownces + item.AllowanceAmount);

                }
                }
                var today = DateTime.Now.Date;
                var startdate = DateTime.ParseExact(Employee.Result.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var servduration = (today - startdate).TotalDays;
                var totalYears = Math.Truncate(servduration / 365);
                var totalMonths = Math.Truncate((servduration % 365) / 30);
                var remainingDays = Math.Truncate((servduration % 365) % 30);
                var Empservtotaldayes = totalYears + "سنه"+ totalMonths + "شهر" +remainingDays + " يوم ";



                //var latesalary = _payrollMarches.GetMatching(x => x.IsDeleted == false && x.IsPostVoucher == false && x.EmpId == EmployeeUpdated.EmployeeId);
                var latesalary = _TaamerProContext.PayrollMarches.Where(x => x.IsDeleted == false && x.IsPostVoucher == false && x.EmpId == EmployeeUpdated.EmployeeId);

                var totallatesalary = 0;
            if(latesalary.Count() > 0) {
            foreach (var sal in latesalary)
            {
                        if(sal.TotalSalaryOfThisMonth !=null)
                totallatesalary = (int)(totallatesalary + sal.TotalSalaryOfThisMonth);

            }
            }
            empdata.EmpCustoday = cutemp.ToString();
            empdata.Custoday2 = cust2.ToString();
            empdata.EmpJob = Employee.Result.JobName;
            empdata.EmpJobNo = Employee.Result.EmployeeNo;
            empdata.EmpLateSalary = totallatesalary.ToString();
            empdata.EmpStartWork = Employee.Result.WorkStartDate.ToString();
            empdata.EmpName = Employee.Result.EmployeeName;
            empdata.EmpTotalServe = Empservtotaldayes.ToString();
            empdata.EmpLoan = LoansCount.ToString();
            empdata.EmpNetSalary = Employee.Result.Salary.ToString();
            empdata.Empbranch = branch.NameAr.ToString();
            empdata.EmpEndallowance = totalallownces.ToString();
            empdata.EndContractDate = Employee.Result.ContractEndDate.ToString();









            return empdata;
            }
            catch (Exception ex)
            {
                return empdata;
            }
        }
        public GeneralMessage EndWorkforAnEmployee(EmpContract data,string Reason,string Duration, int User, string Lang,int BranchId)
        {
            try
            {
                var EmpContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.ContractId == data.ContractId && s.EmpId == data.EmpId).FirstOrDefault();
                // var EmployeeUpdated = _EmpRepository.GetById(data.EmpId);
                Employees? EmployeeUpdated = _TaamerProContext.Employees.Where(s => s.EmployeeId == data.EmpId).FirstOrDefault();

                //check Loans, Tasks and Projects
                if (EmployeeUpdated.Loans != null)
                {
                    //1: loans
                    var loans = EmployeeUpdated.Loans.Where(x => !x.IsDeleted).ToList();
                    int LoansCount = 0;
                    DateTime Today = DateTime.Now.Date;
                    foreach (var loan in loans)
                    {
                        if (loan.LoanDetails.Count > 0)
                        {
                            DateTime MaxDate = loan.LoanDetails.Select(x => x.Date.HasValue ? x.Date.Value : DateTime.MinValue).Max();
                            if (MaxDate.Year >= Today.Year && MaxDate.Month >= Today.Month)
                                LoansCount++;
                        }
                    }
                    if (LoansCount > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في إنهاء خدمات الموظف";
                        _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن إنهاء خدمات الموظف بسبب وجود سلف", "", "", ActionDate2, User, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = LoansCount + Resources.loansHaventFinished };

                    }
                }
                //2: Custody
                //var CustodyCount = _CustodyRepository.GetMatching(x => !x.IsDeleted && x.EmployeeId == EmployeeUpdated.EmployeeId && x.Status == false).Count();
                var CustodyCount = _TaamerProContext.Custody.Where(x => !x.IsDeleted && x.EmployeeId == EmployeeUpdated.EmployeeId && x.Status == false).Count();
                if (CustodyCount > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في إنهاء خدمات الموظف";
                     _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate3, User, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ReasonPhrase = Lang == "rtl" ? "لا يمكن إنهاء خدمات الموظف بسبب وجود عُهد"
                       //"الموظف لديه " + CustodyCount + " عُهد لم يتم فكها" 
                       : "Employee has " + CustodyCount + " custodies haven't free yet"
                    };
                }


                // 3: Tasks, projects,workOrders
                if (EmployeeUpdated.UserId != null && EmployeeUpdated.UserId != 0)
                {
                    if (EmployeeUpdated.UserId.HasValue)
                    {
                        var vUser = _usersRepository.GetById(EmployeeUpdated.UserId.Value);
                        int UserId = vUser.UserId;
                        // var UserF = _usersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin == true && s.UserId == UserId);
                        var UserF = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.IsAdmin == true && s.UserId == UserId);


                        if (UserF != null && UserF.Count() > 0)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في إنهاء خدمات الموظف";
                            _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.cannotDeactivateAdminAccount, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.cannotDeactivateAdminAccount };
                        }

                        // var SettingProjUser = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId);
                        var SettingProjUser = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId);


                        if (SettingProjUser != null && SettingProjUser.Count() > 0)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote5 = "فشل في إنهاء خدمات الموظف";
                            _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "المستخدم موجود علي  سير مشروع لا يمكن إيقاف حسابه", "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "المستخدم موجود علي  سير مشروع لا يمكن إيقاف حسابه" };
                        }
                        // var userTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == UserId && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).Count();
                        var userTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == UserId && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).Count();

                        if (userTasks > 0)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote6 = "فشل في إنهاء خدمات الموظف";
                            _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن إنهاء خدمات الموظف بسبب وجود مهام", "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                ReasonPhrase = "لا يمكن إنهاء خدمات الموظف بسبب وجود مهام " //Resources.userHave + userTasks + Resources.userTasks 
                            };
                        }
                        //var userProject = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.MangerId == UserId && s.Status != 1).Count();
                        var userProject = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId == UserId && s.Status != 1).Count();


                        if (userProject > 0)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote7 = "فشل في إنهاء خدمات الموظف";
                            _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن إنهاء خدمات الموظف بسبب وجود مشاريع", "", "", ActionDate7, UserId, BranchId, ActionNote7, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                ReasonPhrase = "لا يمكن إنهاء خدمات الموظف بسبب وجود مشاريع"
                                //Resources.userHave + userProject + Resources.UserProjects 
                            };
                        }
                        //  var userWorkOrder = _workordersRepository.GetMatching(s => s.IsDeleted == false && (s.ExecutiveEng == UserId || s.ResponsibleEng == UserId) && (s.WOStatus == 1 || s.WOStatus == 2)).Count();

                        var userWorkOrder = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (s.ExecutiveEng == UserId || s.ResponsibleEng == UserId) && (s.WOStatus == 1 || s.WOStatus == 2)).Count();


                        if (userWorkOrder > 0)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate8 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote8 = "فشل في إنهاء خدمات الموظف";
                            _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن إنهاء خدمات الموظف بسبب وجود أوامر عمل", "", "", ActionDate8, UserId, BranchId, ActionNote8, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                ReasonPhrase = "لا يمكن إنهاء خدمات الموظف بسبب وجود أوامر عمل"
                                //Resources.userHave + userWorkOrder + Resources.userWorkOrder 
                            };
                        }

                        //Disable this user
                        vUser.Status = 0; // disactive
                    }

                }
                EmpContract.EndWorkDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                EmployeeUpdated.EndWorkDate = EmpContract.EndWorkDate;
                EmployeeUpdated.UpdateDate = DateTime.Now;
                EmployeeUpdated.UpdateUser = User;
                EmployeeUpdated.ResonLeave = Reason;
                EmployeeUpdated.EmpServiceDuration = Duration;

                _TaamerProContext.SaveChanges();


                string DepartmentNameAr = "";
                Department? DepName = _TaamerProContext.Department.Where(s => s.DepartmentId == EmployeeUpdated.DepartmentId).FirstOrDefault();
                if (DepName != null)
                {
                    DepartmentNameAr = DepName.DepartmentNameAr;
                }

                //string BranchName = _BranchRepository.GetById(EmployeeUpdated.BranchId).NameAr;
                string NameAr = "";
                Branch? BranchName = _TaamerProContext.Branch.Where(s => s.BranchId == EmployeeUpdated.BranchId).FirstOrDefault();
                if (BranchName != null)
                {
                    NameAr = BranchName.NameAr;
                }


                if (EmployeeUpdated.Email !=null )
                {

                    string OrgName = _organizationsService.GetBranchOrganization(EmpContract.BranchId).Result.NameAr;
                    // string DepName = _departmentRepository.GetById(EmployeeUpdated.DepartmentId).DepartmentNameAr;

                
                    string htmlBody = @"<!DOCTYPE html>
                                    <html>

                                    <head></head>

                                    <body style='direction: rtl;'>
                                        <p> السيد /ة " +
                                            EmployeeUpdated.EmployeeNameAr
                                            +
                                            @" المحترم</p>
                                        <p>السلام عليكم ورحمة الله وبركاته</p>
                                        <p>نشكرك على حسن تعاونك و نسأل الله لك التوفيق و النجاح </p>
                                        <table style=' border: 1px solid black; border-collapse: collapse;'>
                                            <thead>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الموظف</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الوظيفة</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>القسم</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الفرع</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تاريخ انتهاء علاقة العمل</th>
                                          </thead>
                                          <tbody>
                                            <tr>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + EmployeeUpdated.EmployeeNameAr + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + EmpContract.PerSe + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + DepartmentNameAr + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + NameAr + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + EmpContract.EndWorkDate + @"</td>
                                            </tr>
                                          </tbody>
                                        </table>

                                        <p>
                                            مع تحيات قسم الموارد البشرية
                                        </p>
                                    </body>
                                    </html>";
                    //Send Mail
                    bool mail = _customerMailService.SendMail_SysNotification((int)EmpContract.BranchId, User, EmployeeUpdated.UserId.Value, Resources.ResourceManager.GetString("ContNotEndWork", CultureInfo.CreateSpecificCulture("en")), htmlBody, true,EmployeeUpdated.Email);


                }
                if (EmployeeUpdated.UserId != null && EmployeeUpdated.UserId != 0)
                {
                    if (EmployeeUpdated.UserId.HasValue)
                    {
                        var NewUser = EmployeeUpdated.UserId.Value;

                        //var UsersWithPriv = _UserPrivilegesRepository.GetMatching(x => x.IsDeleted == false && (x.PrivilegeId == 1419)).Select(x => x.UserId.Value).ToList();
                        //if (EmployeeUpdated.UserId.HasValue)
                        //    UsersWithPriv.Add(EmployeeUpdated.UserId.Value);

                        //UsersWithPriv = UsersWithPriv.Distinct().ToList();

                      


                        
                        //foreach (var user in UsersWithPriv)
                        {
                            var user = NewUser;
                            //var userObj = _usersRepository.GetById(user);
                            Users? userObj = _TaamerProContext.Users.Where(s => s.UserId == user).FirstOrDefault();

                          
                            string NotStr = Resources.ContNotEndWork + ": نشكر الموظف " + EmployeeUpdated.EmployeeNameAr + " على حسن تعاونه" + ", الوظيفة: " + EmpContract.PerSe + " قسم: " + DepName + " فرع: " + BranchName;

                            //Notifications
                            #region Notifications
                            var UserNotification = new Notification();
                            UserNotification.ReceiveUserId = user;
                            UserNotification.Name = Resources.ResourceManager.GetString("ContNotEndWork", CultureInfo.CreateSpecificCulture("en"));
                            UserNotification.Date = EmpContract.EndWorkDate;
                            UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                            UserNotification.SendUserId = 1;
                            UserNotification.Type = 1; // notification
                            UserNotification.Description = NotStr;
                            UserNotification.AllUsers = false;
                            UserNotification.SendDate = DateTime.Now;
                            UserNotification.ProjectId = 0;
                            UserNotification.TaskId = 0;
                            UserNotification.IsHidden = false;
                            UserNotification.AddUser = User;
                            UserNotification.AddDate = DateTime.Now;
                            _TaamerProContext.Notification.Add(UserNotification);

                            _notificationService.sendmobilenotification(user, Resources.ResourceManager.GetString("ContNotEndWork", CultureInfo.CreateSpecificCulture("en")), NotStr);
                            #endregion

                            //SMS
                            var res = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, User, BranchId);

                        }
                    }
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم إنهاء خدمات الموظف";
                 _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1,Resources.General_SavedSuccessfully, "", "", ActionDate, User, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في إنهاء خدمات الموظف";
                 _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed};
            }
        }


        public GeneralMessage EndWorkforAnEmployeeQuaContract(int EmpId, string Reason, string Duration, int User, string Lang, int BranchId)
        {
            try
            {
                //var EmpContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.ContractId == data.ContractId && s.EmpId == data.EmpId).FirstOrDefault();
                // var EmployeeUpdated = _EmpRepository.GetById(data.EmpId);
                Employees? EmployeeUpdated = _TaamerProContext.Employees.Where(s => s.EmployeeId == EmpId).FirstOrDefault();

                //check Loans, Tasks and Projects
                if (EmployeeUpdated.Loans != null)
                {
                    //1: loans
                    var loans = EmployeeUpdated.Loans.Where(x => !x.IsDeleted).ToList();
                    int LoansCount = 0;
                    DateTime Today = DateTime.Now.Date;
                    foreach (var loan in loans)
                    {
                        if (loan.LoanDetails.Count > 0)
                        {
                            DateTime MaxDate = loan.LoanDetails.Select(x => x.Date.HasValue ? x.Date.Value : DateTime.MinValue).Max();
                            if (MaxDate.Year >= Today.Year && MaxDate.Month >= Today.Month)
                                LoansCount++;
                        }
                    }
                    if (LoansCount > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في إنهاء خدمات الموظف";
                        _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن إنهاء خدمات الموظف بسبب وجود سلف", "", "", ActionDate2, User, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = LoansCount + Resources.loansHaventFinished };

                    }
                }
                //2: Custody
                //var CustodyCount = _CustodyRepository.GetMatching(x => !x.IsDeleted && x.EmployeeId == EmployeeUpdated.EmployeeId && x.Status == false).Count();
                var CustodyCount = _TaamerProContext.Custody.Where(x => !x.IsDeleted && x.EmployeeId == EmployeeUpdated.EmployeeId && x.Status == false).Count();
                if (CustodyCount > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في إنهاء خدمات الموظف";
                    _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate3, User, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ReasonPhrase = Lang == "rtl" ? "لا يمكن إنهاء خدمات الموظف بسبب وجود عُهد"
                       //"الموظف لديه " + CustodyCount + " عُهد لم يتم فكها" 
                       : "Employee has " + CustodyCount + " custodies haven't free yet"
                    };
                }


                // 3: Tasks, projects,workOrders
                if (EmployeeUpdated.UserId.HasValue)
                {
                    var vUser = _usersRepository.GetById(EmployeeUpdated.UserId.Value);
                    int UserId = vUser.UserId;
                    // var UserF = _usersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin == true && s.UserId == UserId);
                    var UserF = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.IsAdmin == true && s.UserId == UserId);


                    if (UserF != null && UserF.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في إنهاء خدمات الموظف";
                        _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.cannotDeactivateAdminAccount, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.cannotDeactivateAdminAccount };
                    }

                    // var SettingProjUser = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId);
                    var SettingProjUser = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId);


                    if (SettingProjUser != null && SettingProjUser.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote5 = "فشل في إنهاء خدمات الموظف";
                        _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "المستخدم موجود علي  سير مشروع لا يمكن إيقاف حسابه", "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "المستخدم موجود علي  سير مشروع لا يمكن إيقاف حسابه" };
                    }
                    // var userTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == UserId && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).Count();
                    var userTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == UserId && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).Count();

                    if (userTasks > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote6 = "فشل في إنهاء خدمات الموظف";
                        _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن إنهاء خدمات الموظف بسبب وجود مهام", "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = "لا يمكن إنهاء خدمات الموظف بسبب وجود مهام " //Resources.userHave + userTasks + Resources.userTasks 
                        };
                    }
                    //var userProject = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.MangerId == UserId && s.Status != 1).Count();
                    var userProject = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId == UserId && s.Status != 1).Count();


                    if (userProject > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote7 = "فشل في إنهاء خدمات الموظف";
                        _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن إنهاء خدمات الموظف بسبب وجود مشاريع", "", "", ActionDate7, UserId, BranchId, ActionNote7, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = "لا يمكن إنهاء خدمات الموظف بسبب وجود مشاريع"
                            //Resources.userHave + userProject + Resources.UserProjects 
                        };
                    }
                    //  var userWorkOrder = _workordersRepository.GetMatching(s => s.IsDeleted == false && (s.ExecutiveEng == UserId || s.ResponsibleEng == UserId) && (s.WOStatus == 1 || s.WOStatus == 2)).Count();

                    var userWorkOrder = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (s.ExecutiveEng == UserId || s.ResponsibleEng == UserId) && (s.WOStatus == 1 || s.WOStatus == 2)).Count();


                    if (userWorkOrder > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate8 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote8 = "فشل في إنهاء خدمات الموظف";
                        _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن إنهاء خدمات الموظف بسبب وجود أوامر عمل", "", "", ActionDate8, UserId, BranchId, ActionNote8, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = "لا يمكن إنهاء خدمات الموظف بسبب وجود أوامر عمل"
                            //Resources.userHave + userWorkOrder + Resources.userWorkOrder 
                        };
                    }

                    //Disable this user
                    vUser.Status = 0; // disactive
                }


                var EndWorkDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                EmployeeUpdated.EndWorkDate = EndWorkDate;
                EmployeeUpdated.UpdateDate = DateTime.Now;
                EmployeeUpdated.UpdateUser = User;
                EmployeeUpdated.ResonLeave = Reason;
                EmployeeUpdated.EmpServiceDuration = Duration;

                _TaamerProContext.SaveChanges();

                if (EmployeeUpdated.UserId.HasValue)
                {
                    var NewUser = EmployeeUpdated.UserId.Value;

                    //var UsersWithPriv = _UserPrivilegesRepository.GetMatching(x => x.IsDeleted == false && (x.PrivilegeId == 1419)).Select(x => x.UserId.Value).ToList();
                    //if (EmployeeUpdated.UserId.HasValue)
                    //    UsersWithPriv.Add(EmployeeUpdated.UserId.Value);

                    //UsersWithPriv = UsersWithPriv.Distinct().ToList();

                    string OrgName = _organizationsService.GetBranchOrganization(EmployeeUpdated.BranchId).Result.NameAr;
                    // string DepName = _departmentRepository.GetById(EmployeeUpdated.DepartmentId).DepartmentNameAr;

                    string DepartmentNameAr = "";
                    Department? DepName = _TaamerProContext.Department.Where(s => s.BranchId == EmployeeUpdated.BranchId).FirstOrDefault();
                    if (DepName != null)
                    {
                        DepartmentNameAr = DepName.DepartmentNameAr;
                    }

                    //string BranchName = _BranchRepository.GetById(EmployeeUpdated.BranchId).NameAr;
                    string NameAr = "";
                    Branch? BranchName = _TaamerProContext.Branch.Where(s => s.BranchId == EmployeeUpdated.BranchId).FirstOrDefault();
                    if (BranchName != null)
                    {
                        NameAr = BranchName.NameAr;
                    }



                    string htmlBody = @"<!DOCTYPE html>
                                    <html>

                                    <head></head>

                                    <body style='direction: rtl;'>
                                        <p> السيد /ة " +
                                        EmployeeUpdated.EmployeeNameAr
                                        +
                                        @" المحترم</p>
                                        <p>السلام عليكم ورحمة الله وبركاته</p>
                                        <p>نشكرك على حسن تعاونك و نسأل الله لك التوفيق و النجاح </p>
                                        <table style=' border: 1px solid black; border-collapse: collapse;'>
                                            <thead>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الموظف</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الوظيفة</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>القسم</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الفرع</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تاريخ انتهاء علاقة العمل</th>
                                          </thead>
                                          <tbody>
                                            <tr>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + EmployeeUpdated.EmployeeNameAr + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + EmployeeUpdated.Job.JobNameAr + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + DepName + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + BranchName + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + EndWorkDate + @"</td>
                                            </tr>
                                          </tbody>
                                        </table>

                                        <p>
                                            مع تحيات قسم الموارد البشرية
                                        </p>
                                    </body>
                                    </html>";
                    //foreach (var user in UsersWithPriv)
                    {
                        var user = NewUser;
                        //var userObj = _usersRepository.GetById(user);
                        Users? userObj = _TaamerProContext.Users.Where(s => s.UserId == user).FirstOrDefault();

                        //Send Mail
                        bool mail = _customerMailService.SendMail_SysNotification((int)EmployeeUpdated.BranchId, User, user, Resources.ContNotEndWork, htmlBody, true);

                        string NotStr = Resources.ContNotEndWork + ": نشكر الموظف " + EmployeeUpdated.EmployeeNameAr + " على حسن تعاونه" + ", الوظيفة: " + EmployeeUpdated.Job.JobNameAr + " قسم: " + DepName + " فرع: " + BranchName;

                        //Notifications
                        #region Notifications
                        var UserNotification = new Notification();
                        UserNotification.ReceiveUserId = user;
                        UserNotification.Name = Resources.ResourceManager.GetString("ContNotEndWork", CultureInfo.CreateSpecificCulture("en"));
                        UserNotification.Date = EndWorkDate;
                        UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                        UserNotification.SendUserId = 1;
                        UserNotification.Type = 1; // notification
                        UserNotification.Description = NotStr;
                        UserNotification.AllUsers = false;
                        UserNotification.SendDate = DateTime.Now;
                        UserNotification.ProjectId = 0;
                        UserNotification.TaskId = 0;
                        UserNotification.IsHidden = false;
                        UserNotification.AddUser = User;
                        UserNotification.AddDate = DateTime.Now;
                        _TaamerProContext.Notification.Add(UserNotification);

                        _notificationService.sendmobilenotification(user, Resources.ResourceManager.GetString("ContNotEndWork", CultureInfo.CreateSpecificCulture("en")), NotStr);
                        #endregion

                        //SMS
                        var res = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, User, BranchId);

                    }
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم إنهاء خدمات الموظف";
                _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, User, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في إنهاء خدمات الموظف";
                _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public Task<int> GenerateNextEmpContractNumber(int BranchId)
        {
            return _empContractRepository.GenerateNextEmpContractNumber(BranchId);
        }
       
        public GeneralMessage DeleteEmpContract(int ContractId, int UserId,int BranchId)
        {
            try
            {

                // var empid = _empContractRepository.GetMatching(s => s.IsDeleted == false && s.ContractId == ContractId).FirstOrDefault().EmpId;

                var empid = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.ContractId == ContractId)?.FirstOrDefault()?.EmpId;

                var EmpContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.ContractId == ContractId && s.EmpId == empid).FirstOrDefault();
                // var EmployeeUpdated = _EmpRepository.GetById(empid);
                Employees? EmployeeUpdated = _TaamerProContext.Employees.Where(s => s.EmployeeId == empid).FirstOrDefault();

                //check Loans, Tasks and Projects

                //1: loans
                if(EmployeeUpdated.Loans != null) { 
                var loans = EmployeeUpdated.Loans.Where(x => !x.IsDeleted).ToList();
                int LoansCount = 0;
                DateTime Today = DateTime.Now.Date;
                foreach (var loan in loans)
                {
                    if (loan.LoanDetails.Count > 0)
                    {
                        DateTime MaxDate = loan.LoanDetails.Select(x => x.Date.HasValue ? x.Date.Value : DateTime.MinValue).Max();
                        if (MaxDate.Year >= Today.Year && MaxDate.Month >= Today.Month)
                            LoansCount++;
                    }
                }
                if (LoansCount > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في إنهاء خدمات الموظف";
                     _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن حذف هقد الموظف بسبب وجود سلف", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.contractCannotDeletedDueAdvance };

                }
                }
                //2: Custody
                // var CustodyCount = _CustodyRepository.GetMatching(x => !x.IsDeleted && x.EmployeeId == EmployeeUpdated.EmployeeId && x.Status == false).Count();
                var CustodyCount = _TaamerProContext.Custody.Where(x => !x.IsDeleted && x.EmployeeId == EmployeeUpdated.EmployeeId && x.Status == false).Count();



                if (CustodyCount > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في حذف العقد";
                     _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ReasonPhrase =  "لا يمكن  حذف عقد الموظف بسبب وجود عُهد"
                    };
                }


                // 3: Tasks, projects,workOrders
                if (EmployeeUpdated.UserId.HasValue && EmployeeUpdated.UserId !=0)
                {
                    var vUser = _usersRepository.GetById(EmployeeUpdated.UserId.Value);
                    int UserId2 = vUser.UserId;
                    // var UserF = _usersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin == true && s.UserId == UserId2);
                    var UserF = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.IsAdmin == true && s.UserId == UserId2);


                    if (UserF != null && UserF.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في حذف عقدالموظف";
                         _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.cannotDeactivateAdminAccount, "", "", ActionDate4, UserId2, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.cannotDeactivateAdminAccount };
                    }

                  //  var SettingProjUser = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId2);
                    var SettingProjUser = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId2);



                    if (SettingProjUser != null && SettingProjUser.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote5 = "فشل في حذف عقد الموظف";
                         _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.account_cannot_be_suspended, "", "", ActionDate5, UserId2, BranchId, ActionNote5, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.account_cannot_be_suspended };
                    }
                   // var userTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == UserId2 && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).Count();
                    var userTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == UserId2 && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).Count();


                    if (userTasks > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote6 = "فشل في حذف عقد الموظف";
                         _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.empContractCannotBeDeleted, "", "", ActionDate6, UserId2, BranchId, ActionNote6, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase =  Resources.empContractCannotBeDeleted //Resources.userHave + userTasks + Resources.userTasks 
                        };
                    }
                    //var userProject = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.MangerId == UserId && s.Status != 1).Count();

                    var userProject = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId == UserId && s.Status != 1).Count();


                    if (userProject > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote7 = "فشل في حذف عقد الموظف";
                         _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.empContractCannotBeDeletedBczProjects, "", "", ActionDate7, UserId2, BranchId, ActionNote7, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = Resources.empContractCannotBeDeletedBczProjects
                            //Resources.userHave + userProject + Resources.UserProjects 
                        };
                    }
                    // var userWorkOrder = _workordersRepository.GetMatching(s => s.IsDeleted == false && (s.ExecutiveEng == UserId2 || s.ResponsibleEng == UserId2) && (s.WOStatus == 1 || s.WOStatus == 2)).Count();
                    var userWorkOrder = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (s.ExecutiveEng == UserId2 || s.ResponsibleEng == UserId2) && (s.WOStatus == 1 || s.WOStatus == 2)).Count();

                    if (userWorkOrder > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate8 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote8 = "فشل في حذف عقد الموظف";
                         _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.empContractCannotBeDeletedBczWorkOrder, "", "", ActionDate8, UserId2, BranchId, ActionNote8, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = Resources.empContractCannotBeDeletedBczWorkOrder
                            //Resources.userHave + userWorkOrder + Resources.userWorkOrder 
                        };
                    }


                }




                // EmpContract emp = _empContractRepository.GetById(ContractId);
                EmpContract? emp = _TaamerProContext.EmpContract.Where(s => s.ContractId == ContractId).FirstOrDefault();
                if (emp != null)
                {
                    emp.IsDeleted = true;
                    emp.DeleteDate = DateTime.Now;
                    emp.DeleteUser = UserId;
                    var EmployeeUpdated2 = _EmpRepository.GetById(emp.EmpId);
                    EmployeeUpdated2.ContractNo = null;
                    EmployeeUpdated2.ContractSource = null;
                    EmployeeUpdated2.ContractStartDate = null;
                    EmployeeUpdated2.ContractStartHijriDate = null;
                    EmployeeUpdated2.ContractEndDate = null;
                    EmployeeUpdated2.ContractEndHijriDate = null;
                    EmployeeUpdated2.Salary = null;
                    EmployeeUpdated2.Bonus = null;
                    EmployeeUpdated2.VacationsCount = null;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف عقد موظف رقم " + ContractId;
                    _SystemAction.SaveAction("DeleteEmpContract", "EmpContractService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
                }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف عقد موظف رقم " + ContractId;
                 _SystemAction.SaveAction("DeleteEmpContract", "EmpContractService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
                }
        }
        public Task<IEnumerable<EmpContractDetailVM>> GetAllDetailsByContractId(int? ContractId)
        {
            return _empContractDetailRepository.GetAllDetailsByContractId(ContractId);
        }
        //heba
        public Task<DataTable> GetAllContractDetailsByContractId(int? ContractId, string Con)
        {
            return _empContractDetailRepository.GetAllContractDetailsByContractId(ContractId, Con);
        }
    

    }
}
