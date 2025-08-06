using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.Generic;
using TaamerProject.Repository.Repositories;
using System.Net;
using TaamerProject.Service.IGeneric;
using System.Net.Mail;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class WorkOrdersService :  IWorkOrdersService
    {
        private readonly IWorkOrdersRepository _workordersRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly ISystemAction _SystemAction;
        private readonly IProjectRepository _projectRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly IUsersRepository _UsersRepository;
        public WorkOrdersService(IWorkOrdersRepository workordersRepository, INotificationRepository notificationRepository,
           IEmailSettingRepository emailSettingRepository, IBranchesRepository branchesRepository,
           ICustomerRepository customerRepository, TaamerProjectContext dataContext
            , ISystemAction systemAction, IProjectRepository projectRepository, IUsersRepository usersRepository)
        {
            _CustomerRepository = customerRepository;
            _workordersRepository = workordersRepository;
            _NotificationRepository = notificationRepository;
            _EmailSettingRepository = emailSettingRepository;
            _BranchesRepository = branchesRepository;
            _projectRepository = projectRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _UsersRepository = usersRepository;
        }
        public async Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrders(int BranchId, int UserId)
        {
            // var user = _UsersRepository.GetById(UserId);
            Users? user = await _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefaultAsync();

            if (user !=null && user.UserName == "admin")
            {
                var workorders = await _workordersRepository.GetAllWorkOrdersForAdmin(BranchId);
                return workorders;
            }
            else
            {
                var workorders =await _workordersRepository.GetAllWorkOrders(BranchId);
                return workorders;
            }

        }

        public async Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersFilterd(int BranchId, int UserId,int? CustomerId)
        {
            // var user = _UsersRepository.GetById(UserId);
            Users? user = await _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefaultAsync();

            if (user != null && user.UserName == "admin")
            {
                var workorders = await _workordersRepository.GetAllWorkOrdersForAdminByCustomer(BranchId, CustomerId);
                return workorders;
            }
            else
            {
                var workorders = await _workordersRepository.GetAllWorkOrdersByCustomer(BranchId, CustomerId);
                return workorders;
            }

        }

        public async Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersFilterd(int BranchId, int UserId, int? CustomerId,string? SearchText)
        {
            // var user = _UsersRepository.GetById(UserId);
            Users? user = await _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefaultAsync();

            if (user != null && user.UserName == "admin")
            {
                var workorders = await _workordersRepository.GetAllWorkOrdersForAdminByCustomer(BranchId, CustomerId, SearchText);
                return workorders;
            }
            else
            {
                var workorders = await _workordersRepository.GetAllWorkOrdersByCustomer(BranchId, CustomerId, SearchText);
                return workorders;
            }

        }

        public Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersyProjectId(int ProjectId)
        {

                var workorders = _workordersRepository.GetAllWorkOrdersyProjectId(ProjectId);
                return workorders;
           

        }
        public GeneralMessage SaveWorkOrders(WorkOrders workOrders, int UserId, int BranchId)
        {
            try
            {

                //var BranchIdOfUser = _UsersRepository.GetById(workOrders.ExecutiveEng);
                var BranchIdOfUser =  _TaamerProContext.Users.Where(s => s.UserId == workOrders.ExecutiveEng).FirstOrDefault()!.BranchId??0;
                var UserVacation = _TaamerProContext.Vacation.AsEnumerable().Where(s => s.IsDeleted == false && s.UserId == workOrders.ExecutiveEng && s.VacationStatus == 2 && s.DecisionType == 1 && (s.BackToWorkDate == null || (s.BackToWorkDate ?? "") == "")).ToList();
                UserVacation = UserVacation.Where(s =>
                // أو عنده إجازة في نفس وقت المهمة
                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(workOrders.OrderDate == null || workOrders.OrderDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(workOrders.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                (!(s.StartDate == null || s.StartDate.Equals("")) && !(workOrders.EndDate == null || workOrders.EndDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(workOrders.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                ||
                ((!(s.EndDate == null || s.EndDate.Equals("")) && !(workOrders.OrderDate == null || workOrders.OrderDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(workOrders.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                (!(s.EndDate == null || s.EndDate.Equals("")) && !(workOrders.EndDate == null || workOrders.EndDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(workOrders.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                ||
                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(workOrders.OrderDate == null || workOrders.OrderDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(workOrders.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                (!(s.EndDate == null || s.EndDate.Equals("")) && !(workOrders.OrderDate == null || workOrders.OrderDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(workOrders.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                ||
                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(workOrders.EndDate == null || workOrders.EndDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(workOrders.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                (!(s.EndDate == null || s.EndDate.Equals("")) && !(workOrders.EndDate == null || workOrders.EndDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(workOrders.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                ).ToList();

                if (UserVacation.Count() != 0)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.UserVac };
                }

                // Users? BranchIdOfUser = _TaamerProContext.Users.Where(s => s.ExecutiveEng == ).FirstOrDefault();
                var totaldays = 0.0;
                if(workOrders.ProjectId!=null && workOrders.ProjectId != 0) {
                    //  var project = _projectRepository.GetById(workOrders.ProjectId);
                Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == workOrders.ProjectId).FirstOrDefault();
                if (project != null)
                {
                    BranchIdOfUser = project.BranchId;
                }



                    if (project != null && project.StopProjectType == 1)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ أمر العمل";
                    _SystemAction.SaveAction("SaveWorkOrders", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.workOrderStoppedProject };

                }
               }



                DateTime resultEnd = DateTime.ParseExact(workOrders.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime resultStart = DateTime.ParseExact(workOrders.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                totaldays = (resultEnd - resultStart).TotalDays + 1;

                if (workOrders.WOStatus == null)
                    workOrders.WOStatus = 1;




                var ResultnowString = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                DateTime resultNow = DateTime.ParseExact(ResultnowString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (resultStart == resultNow)
                {
                    workOrders.WOStatus = 2;
                }
                else
                {
                    workOrders.WOStatus = 1;
                }


                string MyWOStatus = "";
                if (workOrders.WOStatus == 1)
                    MyWOStatus = "لم تبدأ";
                else if (workOrders.WOStatus == 2)
                    MyWOStatus = "قيد التشغيل";
                else if (workOrders.WOStatus == 3)
                    MyWOStatus = "منتهية";
                else //if (workOrders.WOStatus == 4)
                    MyWOStatus = "ملغية";

                if (workOrders.WorkOrderId == 0)
                {

                    var exist = _TaamerProContext.WorkOrders.Where(x => x.IsDeleted == false && x.OrderNo == workOrders.OrderNo).FirstOrDefault();
                    if (exist != null)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ أمر العمل";
                        _SystemAction.SaveAction("SaveWorkOrders", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "رقم امر العمل موجود مسبقا" };
                    }

                    //workOrders.OrderNo = Convert.ToString(_workordersRepository.GetMaxOrderNumber(BranchId) + 1);
                    workOrders.UserId = UserId;
                    workOrders.BranchId = BranchIdOfUser;
                    workOrders.AddUser = UserId;
                    workOrders.AddDate = DateTime.Now;
                    workOrders.IsDeleted = false;
                    workOrders.WOStatustxt = MyWOStatus;
                    workOrders.NoOfDays = Convert.ToInt32(totaldays);
                    //  workOrders.WOStatus=
                    _TaamerProContext.WorkOrders.Add(workOrders);
                    try
                    {
                        SendMail(workOrders, BranchId, UserId);
                    }
                    catch (Exception ex)
                    {
                    }
                    var UserNotification = new Notification();
                    UserNotification.ReceiveUserId = workOrders.ExecutiveEng;
                    UserNotification.Name = "work order";
                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                    UserNotification.SendUserId = UserId;
                    UserNotification.Type = 1; // notification
                    UserNotification.Description = "لديك امر عمل : " + workOrders.Required + " رقم " + workOrders.OrderNo;
                    UserNotification.AllUsers = false;
                    UserNotification.SendDate = DateTime.Now;
                    UserNotification.AddUser = UserId;
                    UserNotification.AddDate = DateTime.Now;
                    UserNotification.BranchId = BranchIdOfUser;
                    UserNotification.IsHidden = false;
                    _TaamerProContext.Notification.Add(UserNotification);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافةأمر عمل جديد";
                    _SystemAction.SaveAction("SaveWorkOrders", "WorkOrdersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully,ReturnedParm= workOrders.WorkOrderId };
                }
                else
                {
                    //var workOrdersUpdated = _workordersRepository.GetById(workOrders.WorkOrderId);
                    WorkOrders? workOrdersUpdated = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == workOrders.WorkOrderId).FirstOrDefault();
                    if (workOrdersUpdated != null)
                    {
                        workOrdersUpdated.WorkOrderId = workOrders.WorkOrderId;
                        workOrdersUpdated.OrderNo = workOrders.OrderNo;
                        workOrdersUpdated.UserId = UserId;
                        workOrdersUpdated.UpdateUser = UserId;
                        // workOrdersUpdated.UpdatedDate = DateTime.Now;
                        workOrdersUpdated.UpdateDate = DateTime.Now;

                        workOrdersUpdated.ExecutiveEng = workOrders.ExecutiveEng;
                        workOrdersUpdated.ResponsibleEng = workOrders.ResponsibleEng;
                        workOrdersUpdated.CustomerId = workOrders.CustomerId;

                        workOrdersUpdated.Note = workOrders.Note;

                        workOrdersUpdated.OrderDiscount = workOrders.OrderDiscount;
                        workOrdersUpdated.OrderPaid = workOrders.OrderPaid;
                        workOrdersUpdated.OrderTax = workOrders.OrderTax;
                        workOrdersUpdated.OrderValue = workOrders.OrderValue;
                        workOrdersUpdated.OrderRemaining = workOrders.OrderRemaining;
                        workOrdersUpdated.OrderValueAfterTax = workOrders.OrderValueAfterTax;
                        workOrdersUpdated.Sketch = workOrders.Sketch;
                        workOrdersUpdated.Location = workOrders.Location;
                        workOrdersUpdated.District = workOrders.District;
                        workOrdersUpdated.PieceNo = workOrders.PieceNo;
                        workOrdersUpdated.InstrumentNo = workOrders.InstrumentNo;
                        workOrdersUpdated.ExecutiveType = workOrders.ExecutiveType;
                        workOrdersUpdated.ContractNo = workOrders.ContractNo;
                        workOrdersUpdated.AgentId = workOrders.AgentId;
                        workOrdersUpdated.AgentMobile = workOrders.AgentMobile;
                        workOrdersUpdated.Social = workOrders.Social;

                        workOrdersUpdated.DiscountReason = workOrders.DiscountReason;
                        workOrdersUpdated.ProjectId = workOrders.ProjectId;

                        workOrdersUpdated.EndDate = workOrders.EndDate;
                        workOrdersUpdated.WOStatus = workOrders.WOStatus;
                        workOrdersUpdated.WOStatustxt = MyWOStatus;
                        workOrdersUpdated.BranchId = BranchIdOfUser;
                        workOrders.NoOfDays = Convert.ToInt32(totaldays);
                        workOrdersUpdated.Required = workOrders.Required;
                        workOrdersUpdated.PhasePriority = workOrders.PhasePriority;


                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل أمر عمل رقم " + workOrders.WorkOrderId;
                        _SystemAction.SaveAction("SaveWorkOrders", "WorkOrdersService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully, ReturnedParm = workOrders.WorkOrderId };
                    }
                    else
                    { 
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في حفظ أمر العمل";
                        _SystemAction.SaveAction("SaveWorkOrders", "WorkOrdersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ أمر العمل";
                _SystemAction.SaveAction("SaveWorkOrders", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage EditWorkOrdersFile(WorkOrders workOrders, int UserId, int BranchId)
        {
            try
            {
                WorkOrders? workOrdersUpdated = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == workOrders.WorkOrderId).FirstOrDefault();
                if (workOrdersUpdated != null)
                {
                    workOrdersUpdated.AttatchmentUrl = workOrders.AttatchmentUrl;

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل ملف المهمة الادارية رقم " + workOrders.WorkOrderId;
                    _SystemAction.SaveAction("SaveWorkOrders", "WorkOrdersService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully, ReturnedParm = workOrders.WorkOrderId };
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ ملف المهمة الادارية";
                    _SystemAction.SaveAction("SaveWorkOrders", "WorkOrdersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ ملف المهمة الادارية ";
                _SystemAction.SaveAction("SaveWorkOrders", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        private bool SendMail(WorkOrders workOrders, int BranchId, int UserId)
        {
            try
            {
                DateTime date = new DateTime();
                //var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();

                //var DateOfTask = ProjectPhasesTasks.AddDate.Value.ToString("yyyy-MM-dd HH:MM");
                var DateOfTask = workOrders.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                var customerName = "";
                if (workOrders.CustomerId != null) {
                    //customerName = _CustomerRepository.GetById(workOrders.CustomerId).CustomerNameAr;
                    Customer? customer =   _TaamerProContext.Customer.Where(s=>s.CustomerId == workOrders.CustomerId).FirstOrDefault();
                    if (customer != null)
                    {
                        customerName = customer.CustomerNameAr;
                    }
                }
                string textBody = "   السيد/ة   " + workOrders?.ExecutiveEngineer?.FullName +  "   المحترم  <br/> السلام عليكم ورحمة الله وبركاتة <br/> لديك مهمة ادارية جديدة حسب البيانات الواردة ادناة  <br/> " + "<table border='1'style='text-align:center;padding:3px;'><tr><td style='border=1px solid #eee'>المطلوب تنفيذة</td><td>" + workOrders.Required + "</td></tr><tr><td>اسم العميل </td><td>" + customerName + "</td></tr><tr><td>تاريخ البداية</td><td>" + workOrders.OrderDate + "</td></tr><tr><td>المدة</td><td>" + workOrders.NoOfDays + " days </td></tr></table> <br/> مع تحيات الادارة";
                var mail = new MailMessage();
                
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId??0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Password);

                if (_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0 ).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }

                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                var Worker = _TaamerProContext.Users.Where(s => s.UserId == workOrders.ExecutiveEng).FirstOrDefault();
                mail.To.Add(new MailAddress(Worker?.Email ?? ""));
                //mail.Subject = "لديك مهمة جديده علي مشروع رقم " + project.ProjectNo + "";
                mail.Subject = "لديك مهمة ادارية جديدة  ";
                try
                {
                    mail.Body = textBody;// "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + " علي مشروع رقم " + ProjectPhasesTasks.Project.ProjectNo + " للعميل " + ProjectPhasesTasks.Project.customer.CustomerNameAr;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = workOrders.Required + " : " + workOrders.Note ?? "";
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo; 
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Port);

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                return false;
            }
        }

        public GeneralMessage DeleteWorkOrders(int WorkOrderId, int UserId, int BranchId)
        {
            try
            {
               // WorkOrders constraint = _workordersRepository.GetById(WorkOrderId);
                WorkOrders? constraint = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == WorkOrderId).FirstOrDefault();
                if (constraint != null)
                {
                    constraint.IsDeleted = true;
                    constraint.DeleteDate = DateTime.Now;
                    constraint.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف أمر عمل رقم " + WorkOrderId;
                    _SystemAction.SaveAction("DeleteWorkOrders", "WorkOrdersService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف أمر العمل";
                _SystemAction.SaveAction("SaveWorkOrders", "WorkOrdersService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public Task<IEnumerable<WorkOrdersVM>> SearchWorkOrders(WorkOrdersVM WorkOrdersSearch, string lang, int BranchId)
        {      
            var workorders =  _workordersRepository.SearchWorkOrders(WorkOrdersSearch, lang, BranchId);
                       
            return workorders;
        }
        public Task<int> GetMaxOrderNumber(int BranchId)
        {
            var workorders = _workordersRepository.GetMaxOrderNumber();
            return workorders;
        }
        public async Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersByDateSearch(string DateFrom, string DateTo, int BranchId, int UserId)
        {
           // var user = _UsersRepository.GetById(UserId);
            Users? user = _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefault();

            if (user != null && user.UserName == "admin")
            {
                var workorders = _workordersRepository.GetAllWorkOrdersByDateSearchForAdmin(DateFrom, DateTo, BranchId).Result;
                return workorders;
            }
            else
            {
                var workorders = _workordersRepository.GetAllWorkOrdersByDateSearch(DateFrom, DateTo, BranchId).Result;
                return workorders;
            }
        }
        public Task<WorkOrdersVM> GetWorkOrderById(int WorkOrderId, string lang)
        {
            return _workordersRepository.GetWorkOrderById(WorkOrderId, lang);
        }
        public Task<IEnumerable<WorkOrdersVM>>  GetLateWorkOrdersByUserId(string EndDateP, int? UserId, int BranchId)
        {
            var workorders = _workordersRepository.GetLateWorkOrdersByUserId(EndDateP, UserId, BranchId);
            return workorders;
        }

        public Task<IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId,int? customerid,int? ProjectId)
        {
            var workorders = _workordersRepository.GetLateWorkOrdersByUserIdFilterd(EndDateP, UserId, BranchId, customerid, ProjectId);
            return workorders;
        }

        public Task<IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? customerid, int? ProjectId,string? Searchtext)
        {
            var workorders = _workordersRepository.GetLateWorkOrdersByUserIdFilterd(EndDateP, UserId, BranchId, customerid, ProjectId, Searchtext);
            return workorders;
        }
        public Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserId(string EndDateP, int? UserId, int BranchId)
        {
            var workorders = _workordersRepository.GetNewWorkOrdersByUserId(EndDateP, UserId, BranchId);
            return workorders;
        }

        public Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? customerid, int? ProjectId)
        {
            var workorders = _workordersRepository.GetNewWorkOrdersByUserIdFilterd(EndDateP, UserId, BranchId, customerid, ProjectId);
            return workorders;
        }

        public Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? customerid, int? ProjectId,string? Searchtext)
        {
            var workorders = _workordersRepository.GetNewWorkOrdersByUserIdFilterd(EndDateP, UserId, BranchId, customerid, ProjectId, Searchtext);
            return workorders;
        }
        public Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserId(int? UserId, int BranchId)
        {
            var workorders = _workordersRepository.GetWorkOrdersByUserId(UserId, BranchId);
            return workorders;
        }
        public Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserIdandtask(string task, int? UserId, int BranchId)
        {
            var workorders = _workordersRepository.GetWorkOrdersByUserIdandtask(task, UserId, BranchId);
            return workorders;
        }
        public Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserIdandtask2(string task, int? UserId, int BranchId)
        {
            var workorders = _workordersRepository.GetWorkOrdersByUserIdandtask2(task, UserId, BranchId);
            return workorders;
        }

        public Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersBytask(string task, int BranchId)
        {
            var workorders = _workordersRepository.GetWorkOrdersBytask(task, BranchId);
            return workorders;
        }
        public Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersBytask(string task, int BranchId,int UserId)
        {
            var workorders = _workordersRepository.GetWorkOrdersBytask(task, BranchId, UserId);
            return workorders;
        }
        public Task<IEnumerable<WorkOrdersVM>> GetDayWeekMonth_Orders(int? UserId, int Status, int BranchId, int Flag, string StartDate, string EndDate)
        {
            var WorkOrders = _workordersRepository.GetDayWeekMonth_Orders(UserId, Status, BranchId, Flag, StartDate, EndDate);
            return WorkOrders;
        }

        public Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport(int? UserId, int Status, int BranchId, string Lang, string StartDate, string EndDate, List<int> BranchesList)
        {
            var WorkOrders = _workordersRepository.GetWorkOrderReport(UserId,  BranchId, Lang, Status, StartDate, EndDate, BranchesList);
            return WorkOrders;
        }
        public Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport(int? UserId, int Status, int BranchId, string Lang, string StartDate, string EndDate,string? Searchtext)
        {
            var WorkOrders = _workordersRepository.GetWorkOrderReport(UserId, BranchId, Lang, Status, StartDate, EndDate, Searchtext);
            return WorkOrders;
        }
        public Task<List<ProjectPhasesTasksVM>> GetWorkOrderReport_print(int? UserId, int Status, int BranchId, string Lang, string StartDate, string EndDate)
        {
            var WorkOrders = _workordersRepository.GetWorkOrderReport_print(UserId, BranchId, Lang, Status, StartDate, EndDate);
            return WorkOrders;
        }

        public Task<IEnumerable<ProjectPhasesTasksVM>> GetALlWorkOrderReport(string Lang, int BranchId)
        {
            var WorkOrders = _workordersRepository.GetALlWorkOrderReport(Lang,BranchId);
            return WorkOrders;
        }
        public Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport_ptoject(int? projectid,string Lang, string StartDate, string EndDate, int BranchId)
        {
            var WorkOrders = _workordersRepository.GetWorkOrderReport_ptoject(Lang,projectid,StartDate, EndDate,BranchId);
            return WorkOrders;
        }

        public Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport_ptoject(int? projectid, string Lang, string StartDate, string EndDate, int BranchId,string? SeachText)
        {
            var WorkOrders = _workordersRepository.GetWorkOrderReport_ptoject(Lang, projectid, StartDate, EndDate, BranchId, SeachText);
            return WorkOrders;
        }
        public GeneralMessage FinishOrder(WorkOrders workOrders, int UserId, int BranchId)
        {
            try
            {
              //  var ProTaskUpdated = _workordersRepository.GetById(workOrders.WorkOrderId);
                WorkOrders? ProTaskUpdated = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == workOrders.WorkOrderId).FirstOrDefault();           
                if (ProTaskUpdated != null)
                {
                    ProTaskUpdated.WOStatus = workOrders.WOStatus;
                    ProTaskUpdated.WOStatustxt = workOrders.WOStatustxt;
                    ProTaskUpdated.PercentComplete = workOrders.PercentComplete;

                    //ProTaskUpdated.Active = false;
                    ProTaskUpdated.UpdateUser = UserId;
                    //ProTaskUpdated.UpdatedDate = DateTime.Now;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges(); 
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تم انهاء أمر العمل رقم " + workOrders.WorkOrderId;
                _SystemAction.SaveAction("FinishOrder", "WorkOrdersService", 2, Resources.work_order_terminated, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.WorkOrderId = ProTaskUpdated!.WorkOrderId;
                if (ProTaskUpdated.WOStatus == 3) Pro_TaskOperation.OperationName = "انهاء المهمة";
                else Pro_TaskOperation.OperationName = "اعطاء نسب للمهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.work_order_terminated };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("FinishOrder", "WorkOrdersService", 2, Resources.work_order_Faild, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.work_order_Faild };
            }
        }

        //---------------------------------------------------------------------------------------------
        public GeneralMessage ConvertUserTasksSome(int WorkOrderId, int FromUserId, int ToUserId, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var FromUserTasksNotStarted = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.WorkOrderId == WorkOrderId && s.UserId == FromUserId && s.WOStatus != 3);
                if (FromUserTasksNotStarted != null && FromUserTasksNotStarted.Count() > 0)
                {
                    foreach (var item in FromUserTasksNotStarted)
                    {
                        var projectData = _workordersRepository.GetWorkOrderById(WorkOrderId, "rtl").Result;

                        var userFrom = FromUserId;
                        item.UserId = ToUserId;
                        item.IsConverted = 2;
                        item.UpdateUser = UserId;
                        item.UpdateDate = DateTime.Now;

                        #region
                        //SaveOperationsForTask
                        Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                        Pro_TaskOperation.WorkOrderId = item.WorkOrderId;
                        Pro_TaskOperation.OperationName = "تم تحويل المهمة";
                        Pro_TaskOperation.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        Pro_TaskOperation.UserId = item.UserId;
                        Pro_TaskOperation.BranchId = BranchId;
                        Pro_TaskOperation.AddUser = UserId;
                        Pro_TaskOperation.AddDate = DateTime.Now;
                        _TaamerProContext.Pro_TaskOperations.Add(Pro_TaskOperation);
                        #endregion

                        //try
                        //{
                        //    var branch = _BranchesRepository.GetById(BranchId);

                        //    var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ToUserId).Result;
                        //    if (UserNotifPriv.Count() != 0)
                        //    {
                        //        if (UserNotifPriv.Contains(362))
                        //        {
                        //            var UserNotification = new Notification();
                        //            UserNotification.ReceiveUserId = ToUserId;
                        //            UserNotification.Name = "مهمة محولة";
                        //            UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                        //            UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                        //            UserNotification.SendUserId = UserId;
                        //            UserNotification.Type = 1; // notification
                        //            UserNotification.Description = "مهمه : " + item.DescriptionAr + ":" + item.Notes + " علي مشروع رقم " + projectData?.ProjectNo + " للعميل " + projectData?.CustomerName_W + " بعد تحويلها من " + _UsersRepository.GetById(userFrom).FullName + " " + " فرع " + branch.NameAr + "";
                        //            UserNotification.AllUsers = false;
                        //            UserNotification.SendDate = DateTime.Now;
                        //            UserNotification.ProjectId = item.ProjectId;
                        //            UserNotification.TaskId = item.PhaseTaskId;
                        //            UserNotification.AddUser = UserId;
                        //            UserNotification.IsHidden = false;
                        //            UserNotification.NextTime = null;
                        //            UserNotification.AddDate = DateTime.Now;
                        //            _TaamerProContext.Notification.Add(UserNotification);
                        //            _notificationService.sendmobilenotification(ToUserId, Resources.General_Newtasks, "لديك مهمه جديدة : " + item.DescriptionAr + ":" + item.Notes + " علي مشروع رقم " + projectData?.ProjectNo + " للعميل " + projectData?.CustomerName_W + " بعد تحويلها من " + _UsersRepository.GetById(userFrom).FullName + " " + " فرع " + branch.NameAr + "");

                        //        }


                        //        if (UserNotifPriv.Contains(363))
                        //        {
                        //            var userObj = _UsersRepository.GetById(ToUserId);

                        //            var NotStr = _UsersRepository.GetById(userFrom).FullName + "  بعد تحويلها من  " + projectData?.CustomerName_W + " للعميل  " + projectData?.ProjectNo + " علي مشروع رقم " + item.DescriptionAr + " لديك مهمه جديدة  ";
                        //            if (userObj.Mobile != null && userObj.Mobile != "")
                        //            {
                        //                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                        //            }
                        //        }

                        //        if (UserNotifPriv.Contains(361))
                        //        {
                        //            var Desc = branch.NameAr + " فرع " + _UsersRepository.GetById(userFrom).FullName + " بعد تحويلها من " + projectData?.CustomerName_W + " للعميل " + projectData?.ProjectNo + " علي مشروع رقم " + item.DescriptionAr + " لديك مهمه جديدة : ";

                        //            //SendMailNoti(item.Project.ProjectId, Desc, "اضافة مهمة جديدة", BranchId, UserId, ToUserId);
                        //            SendMailFinishTask2(item, "اضافة مهمة جديدة", BranchId, UserId, Url, ImgUrl, 5, FromUserId, projectData.CustomerName ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");
                        //        }
                        //    }

                        //}
                        //catch (Exception)
                        //{

                        //}
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل مهمة";
                _SystemAction.SaveAction("ConvertUserTasksSome", "WorkOrdersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TranfareTask };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تحويل المهمة";
                _SystemAction.SaveAction("ConvertUserTasksSome", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedTranfareTask };
            }
        }

        public GeneralMessage RequestConvertTask(WorkOrders workOrders, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var ProTaskUpdated = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == workOrders.WorkOrderId).FirstOrDefault();
                if (ProTaskUpdated != null)
                {
                    var userFrom = ProTaskUpdated.UserId;
                    ProTaskUpdated.IsConverted = workOrders.IsConverted;
                }

                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);


                //try
                //{
                //    if (ProTaskUpdated != null && ProTaskUpdated.ProjectId > 0)
                //    {
                //        var projectData = _ProjectRepository.GetProjectByIdSome("rtl", ProTaskUpdated.ProjectId ?? 0).Result;
                //        if (projectData != null && projectData.CustomerId > 0)
                //        {

                //            var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(projectData.MangerId ?? 0).Result;
                //            if (UserNotifPriv.Count() != 0)
                //            {
                //                if (UserNotifPriv.Contains(3202))
                //                {
                //                    var UserNotification = new Notification();
                //                    UserNotification.ReceiveUserId = projectData.MangerId;
                //                    UserNotification.Name = "طلب تحويل مهمة";
                //                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                //                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                //                    UserNotification.SendUserId = UserId;
                //                    UserNotification.Type = 1; // notification
                //                    UserNotification.Description = "طلب تحويل  : " + ProTaskUpdated.DescriptionAr + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName ?? "";
                //                    UserNotification.AllUsers = false;
                //                    UserNotification.SendDate = DateTime.Now;
                //                    UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                //                    UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                //                    UserNotification.AddUser = UserId;
                //                    UserNotification.IsHidden = false;
                //                    UserNotification.NextTime = null;
                //                    UserNotification.AddDate = DateTime.Now;
                //                    _TaamerProContext.Notification.Add(UserNotification);
                //                    _notificationService.sendmobilenotification(projectData.MangerId ?? 0, Resources.General_Newtasks, "طلب تحويل المهمة : " + ProTaskUpdated.DescriptionAr + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName ?? "");

                //                }
                //                if (UserNotifPriv.Contains(3201))
                //                {
                //                    var Desc = formattedDate + " بتاريخ " + ProTaskUpdated.DescriptionAr + " طلب تحويل المهمة ";

                //                    //  SendMailNoti(ProTaskUpdated.ProjectId ?? 0, Desc, "طلب تحويل المهمة", BranchId, UserId, ProTaskUpdated.Project.MangerId ?? 0);
                //                    SendMailFinishTask2(ProTaskUpdated, "لديك طلب تحويل المهمة", BranchId, UserId, Url, ImgUrl, 3, 0, projectData.CustomerName ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                //                }
                //                if (UserNotifPriv.Contains(3203))
                //                {
                //                    var userObj = _UsersRepository.GetById(projectData.MangerId ?? 0);
                //                    var NotStr = formattedDate + " بتاريخ " + ProTaskUpdated.DescriptionAr + " طلب تحويل المهمة ";
                //                    if (userObj.Mobile != null && userObj.Mobile != "")
                //                    {
                //                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                //                    }

                //                }
                //            }
                //        }

                //    }
                //}
                //catch (Exception ex)
                //{

                //}


                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "طلب تحويل مهمة";
                _SystemAction.SaveAction("RequestConvertTask", "WorkOrdersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.WorkOrderId = ProTaskUpdated!.WorkOrderId;
                Pro_TaskOperation.OperationName = "طلب تحويل المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.askTranfareSucc };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في طلب تحويل مهمة";
                _SystemAction.SaveAction("RequestConvertTask", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.askTranfareFailed };
            }
        }

        public GeneralMessage PlayPauseTask(WorkOrders workOrders, int UserId, string Lang, int BranchId)
        {
            try
            {
                var ProTaskUpdated = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == workOrders.WorkOrderId).FirstOrDefault();
                if (ProTaskUpdated != null)
                {

                    ProTaskUpdated.WOStatus = workOrders.WOStatus;
                    //if (ProTaskUpdated.WOStatus == 4)
                    //{
                    //    ProTaskUpdated.StopCount += 1;
                    //}
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تشغيل /ايقاف المهمة" + ProTaskUpdated.WorkOrderId;
                _SystemAction.SaveAction("PlayPauseTask", "WorkOrdersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.WorkOrderId = ProTaskUpdated.WorkOrderId;
                if (ProTaskUpdated.WOStatus == 2) Pro_TaskOperation.OperationName = "تشغيل المهمة";
                else Pro_TaskOperation.OperationName = "إيقاف المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = workOrders.WOStatus == 2 ? Resources.Pro_taskStart : Resources.Pro_stopTask };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تشغيل /ايقاف المهمة";
                _SystemAction.SaveAction("PlayPauseTask", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = workOrders.WOStatus == 2 ? Resources.taskDontrun : Resources.taskDontstop };
            }
        }

        public GeneralMessage ChangeTaskTime(WorkOrders workOrders, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var ProTaskUpdated = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == workOrders.WorkOrderId).FirstOrDefault();
                //var projectData = _projectRepository.GetProjectByIdSome("rtl", ProTaskUpdated.ProjectId ?? 0).Result;
                if (ProTaskUpdated != null)
                {

                    ProTaskUpdated.OrderDate = workOrders.OrderDate;
                    ProTaskUpdated.EndDate = workOrders.EndDate;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                    ProTaskUpdated.PlusTime = false;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تغيير وقت المهمة";
                _SystemAction.SaveAction("ChangeTaskTime", "WorkOrdersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.WorkOrderId = ProTaskUpdated!.WorkOrderId;
                Pro_TaskOperation.OperationName = "تم تمديد المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion

                // SendMailChangeTaskTime(ProTaskUpdated, BranchId, UserId, 1);
                //SendMailFinishTask2(ProTaskUpdated, "تمديد مهمة", BranchId, UserId, Url, ImgUrl, 4, 0, projectData.CustomerName ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Pro_taskPluseTime };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تغيير وقت المهمة";
                _SystemAction.SaveAction("ChangeTaskTime", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.failedTaskPlusTime };
            }
        }
        public GeneralMessage ConvertTask(WorkOrders workOrders, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var ProTaskUpdated = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == workOrders.WorkOrderId).FirstOrDefault();
                if (ProTaskUpdated != null)
                {
                    var BranchIdOfUser = _TaamerProContext.Users.Where(s => s.UserId == workOrders.ExecutiveEng).FirstOrDefault()!.BranchId ?? 0;

                    if (ProTaskUpdated.ProjectId != null && ProTaskUpdated.ProjectId != 0)
                    {
                        Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == ProTaskUpdated.ProjectId).FirstOrDefault();
                        if (project != null)
                        {
                            BranchIdOfUser = project.BranchId;
                        }
                    }
                    var userFrom = ProTaskUpdated.ExecutiveEng;
                    ProTaskUpdated.ExecutiveEng = workOrders.ExecutiveEng;
                    ProTaskUpdated.IsConverted = 2;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.BranchId = BranchIdOfUser;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                    //try
                    //{
                    //    var branch = _BranchesRepository.GetById(BranchId);

                    //    if (ProTaskUpdated != null && ProTaskUpdated.ProjectId > 0)
                    //    {
                    //        var projct = _ProjectRepository.GetById((int)ProTaskUpdated.ProjectId);
                    //        if (projct != null && projct.CustomerId > 0)
                    //        {
                    //            var cust = _CustomerRepository.GetById((int)projct.CustomerId);

                    //            var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ProjectPhasesTasks.UserId ?? 0).Result;
                    //            if (UserNotifPriv.Count() != 0)
                    //            {
                    //                if (UserNotifPriv.Contains(362))
                    //                {
                    //                    var UserNotification = new Notification();
                    //                    UserNotification.ReceiveUserId = ProjectPhasesTasks.UserId;
                    //                    UserNotification.Name = "مهمة محولة";
                    //                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    //                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                    //                    UserNotification.SendUserId = UserId;
                    //                    UserNotification.Type = 1; // notification
                    //                    UserNotification.Description = " مهمه : " + ProTaskUpdated.DescriptionAr + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projct.ProjectNo + " للعميل " + cust.CustomerNameAr + " بعد تحويلها من " + _UsersRepository.GetById(userFrom ?? 0).FullName + " " + "فرع  " + branch.NameAr + "";
                    //                    UserNotification.AllUsers = false;
                    //                    UserNotification.SendDate = DateTime.Now;
                    //                    UserNotification.ProjectId = ProjectPhasesTasks.ProjectId;
                    //                    UserNotification.TaskId = ProjectPhasesTasks.PhaseTaskId;
                    //                    UserNotification.AddUser = UserId;
                    //                    UserNotification.BranchId = BranchIdOfUserOrDepartment;
                    //                    UserNotification.IsHidden = false;
                    //                    UserNotification.NextTime = null;
                    //                    UserNotification.AddDate = DateTime.Now;
                    //                    _TaamerProContext.Notification.Add(UserNotification);
                    //                    _notificationService.sendmobilenotification(ProjectPhasesTasks.UserId ?? 0, Resources.General_Newtasks, "لديك مهمه جديدة : " + ProTaskUpdated.DescriptionAr + ":" + cust.CustomerNameAr + " بعد تحويلها من " + _UsersRepository.GetById(userFrom ?? 0).FullName + " " + "فرع  " + branch.NameAr + "");
                    //                }


                    //                if (UserNotifPriv.Contains(363))
                    //                {
                    //                    var userObj = _UsersRepository.GetById(ProjectPhasesTasks.UserId ?? 0);

                    //                    var NotStr = _UsersRepository.GetById(userFrom ?? 0).FullName + "  بعد تحويلها من  " + cust.CustomerNameAr + " للعميل  " + projct.ProjectNo + " علي مشروع رقم " + ProTaskUpdated.DescriptionAr + " لديك مهمه جديدة  ";
                    //                    if (userObj.Mobile != null && userObj.Mobile != "")
                    //                    {
                    //                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                    //                    }
                    //                }

                    //                //if (UserNotifPriv.Contains(361))
                    //                //{
                    //                //    var Desc = branch.NameAr + " فرع " + _UsersRepository.GetById(userFrom).FullName + " بعد تحويلها من " + ProTaskUpdated.Project.customer.CustomerNameAr + " للعميل " + ProTaskUpdated.Project.ProjectNo + " علي مشروع رقم " + ProjectPhasesTasks.DescriptionAr + " لديك مهمه جديدة : ";

                    //                //    //SendMailNoti(ProTaskUpdated.Project.ProjectId, Desc, "اضافة مهمة جديدة", BranchId, UserId, ProjectPhasesTasks.UserId ?? 0);
                    //                //    SendMailFinishTask2(ProTaskUpdated, " تحويل المهمة", BranchId, UserId, Url, ImgUrl, 5, (int)userFrom);

                    //                //}
                    //            }

                    //        }
                    //    }
                    //}
                    //catch (Exception)
                    //{
                    //}
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل مهمة" + ProTaskUpdated!.WorkOrderId;
                _SystemAction.SaveAction("ConvertTask", "WorkOrdersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.WorkOrderId = ProTaskUpdated!.WorkOrderId;
                Pro_TaskOperation.OperationName = "تم تحويل المهمة";
                Pro_TaskOperation.UserId = ProTaskUpdated.ExecutiveEng;
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TranfareTask };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تحويل المهمة";
                _SystemAction.SaveAction("ConvertTask", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedTranfareTask };
            }
        }
        public GeneralMessage SaveTaskLongDesc(int WorkOrderId, string taskLongDesc, int UserId, int BranchId)
        {
            var task = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == WorkOrderId).FirstOrDefault();
            if (task != null)
            {
                task.Note = taskLongDesc;
                task.UpdateDate = DateTime.Now;
                task.UpdateUser = UserId;

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = "حفظ تفاصيل المهمة" + task.WorkOrderId;
                _SystemAction.SaveAction("SaveTaskLongDesc", "WorkOrdersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            else
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ تفاصيل المهمة";
                _SystemAction.SaveAction("ConvertTask", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage PlustimeTask(WorkOrders workOrders, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var ProTaskUpdated = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == workOrders.WorkOrderId).FirstOrDefault();
                if (ProTaskUpdated != null)
                {
                    //ProTaskUpdated.Status = ProjectPhasesTasks.Status;
                    ProTaskUpdated.PlusTime = true;
                    //ProTaskUpdated.Active = false;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                }
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                //try
                //{
                //    if (ProTaskUpdated != null && ProTaskUpdated.ProjectId > 0)
                //    {
                //        var projectData = _ProjectRepository.GetProjectByIdSome("rtl", ProTaskUpdated.ProjectId ?? 0).Result;
                //        if (projectData != null && projectData.CustomerId > 0)
                //        {
                //            //var cust = _CustomerRepository.GetById((int)projectData.CustomerId);

                //            var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(projectData.MangerId ?? 0).Result;

                //            if (UserNotifPriv.Count() != 0)
                //            {
                //                if (UserNotifPriv.Contains(3192))
                //                {
                //                    var UserNotification = new Notification();
                //                    UserNotification.ReceiveUserId = projectData.MangerId;
                //                    UserNotification.Name = "تمديد مهمة";
                //                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                //                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                //                    UserNotification.SendUserId = UserId;
                //                    UserNotification.Type = 1; // notification
                //                    UserNotification.Description = "طلب تمديد : " + ProTaskUpdated.DescriptionAr + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName;
                //                    UserNotification.AllUsers = false;
                //                    UserNotification.SendDate = DateTime.Now;
                //                    UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                //                    UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                //                    UserNotification.AddUser = UserId;
                //                    UserNotification.IsHidden = false;
                //                    UserNotification.NextTime = null;
                //                    UserNotification.AddDate = DateTime.Now;
                //                    _TaamerProContext.Notification.Add(UserNotification);
                //                    _notificationService.sendmobilenotification(projectData.MangerId ?? 0, Resources.General_Newtasks, "طلب تمديد المهمة : " + ProTaskUpdated.DescriptionAr + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName);
                //                }
                //                if (UserNotifPriv.Contains(3191))
                //                {
                //                    var Desc = formattedDate + " بتاريخ " + ProTaskUpdated.DescriptionAr + " طلب تمديد المهمة ";


                //                    SendMailFinishTask2(ProTaskUpdated, "لديك طلب تمديد المهمة", BranchId, UserId, Url, ImgUrl, 2, 0, projectData.CustomerName ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                //                }
                //                if (UserNotifPriv.Contains(3193))
                //                {
                //                    var userObj = _UsersRepository.GetById(projectData.MangerId ?? 0);
                //                    var NotStr = formattedDate + " بتاريخ " + ProTaskUpdated.DescriptionAr + " طلب تمديد المهمة ";
                //                    if (userObj.Mobile != null && userObj.Mobile != "")
                //                    {
                //                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                //                    }

                //                }
                //            }
                //        }
                //    }
                //}
                //catch (Exception)
                //{

                //}




                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تمديد وقت المهمة";
                _SystemAction.SaveAction("PlustimeTask", "WorkOrdersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.WorkOrderId = ProTaskUpdated!.WorkOrderId;
                Pro_TaskOperation.OperationName = "طلب تمديد المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Pro_PlusTasktime };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تمديد وقت المهمة";
                _SystemAction.SaveAction("PlustimeTask", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Pro_faildPlustimeTask };
            }
        }
        public GeneralMessage RefusePlustimeTask(WorkOrders workOrders, int UserId, int BranchId)
        {
            try
            {
                var ProTaskUpdated = _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == workOrders.WorkOrderId).FirstOrDefault();
                if (ProTaskUpdated != null)
                {
                    //ProTaskUpdated.Status = ProjectPhasesTasks.Status;
                    ProTaskUpdated.PlusTime = false;
                    //ProTaskUpdated.Active = false;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "رفض تمديد وقت المهمة";
                _SystemAction.SaveAction("RefusePlustimeTask", "WorkOrdersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.WorkOrderId = ProTaskUpdated!.WorkOrderId;
                Pro_TaskOperation.OperationName = "رفض تمديد المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Pro_PlusTasktime };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في رفض تمديد وقت المهمة";
                _SystemAction.SaveAction("RefusePlustimeTask", "WorkOrdersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Pro_faildPlustimeTask };
            }
        }

        public Task<IEnumerable<Pro_TaskOperationsVM>> GetTaskOperationsByTaskId(int WorkOrderId)
        {
            var Tasks = _workordersRepository.GetTaskOperationsByTaskId(WorkOrderId);
            return Tasks;
        }

        public GeneralMessage SaveTaskOperations(Pro_TaskOperations TaskOperations, int UserId, int BranchId)
        {
            try
            {
                TaskOperations.UserId = UserId;
                TaskOperations.BranchId = BranchId;
                TaskOperations.AddUser = UserId;
                TaskOperations.AddDate = DateTime.Now;
                _TaamerProContext.Pro_TaskOperations.Add(TaskOperations);
                _TaamerProContext.SaveChanges();
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public async Task<string> GenerateNextOrderNumber(int BranchId, int? ProjectId)
        {
            var codePrefix = "MGT#";
            //var prostartcode = _BranchesRepository.GetById(BranchId).OrderStartCode;
            //if (prostartcode != null && prostartcode != "")
            //{
            //    codePrefix = prostartcode;
            //}

            //var Value = _workordersRepository.GenerateNextOrderNumber(BranchId, codePrefix, 0).Result;
            //var NewValue = string.Format("{0:000000}", Value);
            //if (codePrefix != "")
            //{
            //    NewValue = codePrefix + NewValue;
            //}

            var Value = _workordersRepository.GenerateNextOrderNumber(BranchId, codePrefix, 0).Result;
            var NewValue = string.Format("{0:000000}", Value);
            NewValue = codePrefix + NewValue;
            return (NewValue);
        }

        public async Task<string> GetOrderCode_S(int BranchId, int? ProjectId)
        {
            var codePrefix = "";
            var prostartcode = _BranchesRepository.GetById(BranchId).OrderStartCode;
            if (prostartcode != null && prostartcode != "")
            {
                codePrefix = prostartcode;
            }
            return codePrefix;
        }

    }


}
