
using System.Globalization;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class CustodyService : ICustodyService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICustodyRepository _CustodyRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IItemRepository _ItemRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly INotificationService _notificationService;
        private readonly ICustomerMailService _customerMailService;
        private readonly IEmployeesService _employeesService;

        public CustodyService(TaamerProjectContext dataContext, ISystemAction systemAction, ICustodyRepository custodyRepository, IEmployeesRepository employeesRepository, IItemRepository itemRepository
            , IUsersRepository usersRepository, IUserNotificationPrivilegesService userNotificationPrivilegesService, IBranchesRepository branchesRepository, IOrganizationsRepository organizationsRepository
            , IEmailSettingRepository emailSettingRepository, INotificationService notificationService, ICustomerMailService customerMailService,
            IEmployeesService employeesService)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _CustodyRepository = custodyRepository;
            _employeesRepository = employeesRepository;
            _ItemRepository = itemRepository;
            _usersRepository = usersRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _BranchesRepository = branchesRepository;
            _OrganizationsRepository = organizationsRepository;
            _EmailSettingRepository = emailSettingRepository;
            _notificationService = notificationService;
            _customerMailService = customerMailService;
            this._employeesService = employeesService;
        }
        public async Task<IEnumerable<CustodyVM>> GetAllCustody(int BranchId)
        {
            var Custody = await _CustodyRepository.GetAllCustody(BranchId);
            return Custody;
        }


        public async Task<IEnumerable<CustodyVM>> SearchCustody(CustodyVM CustodySearch, string lang, int BranchId)
        {
            var Custody =await _CustodyRepository.SearchCustody(CustodySearch, lang, BranchId);
            return Custody.ToList();
        }
        public async Task<IEnumerable<CustodyVM>> SearchCustodyVoucher(CustodyVM CustodySearch, string lang, int BranchId)
        {
            var Custody = await _CustodyRepository.SearchCustodyVoucher(CustodySearch, lang, BranchId);
            return Custody.ToList();
        }

        public async Task<IEnumerable<object>> FillCustodySelect(string lang, int BranchId)
        {
            List<selectDataClass> selectDataClass = new List<selectDataClass>();
            List<CustodyVM> All = (List<CustodyVM>)await _CustodyRepository.GetDistinctCustody(BranchId);
            for (int i = 0; i < All.Count; i++)
            {
                int index = selectDataClass.FindIndex(f => f.Id == All[i].ItemId.ToString());
                if (index < 0)
                {
                    selectDataClass.Add(new selectDataClass
                    {
                        Id = All[i].ItemId.ToString(),
                        Name = All[i].ItemName,
                    });
                }
            }

            return selectDataClass;
        }

        public async Task<IEnumerable<CustodyVM>> GetSomeCustody(int BranchId, bool Status)
        {
            var Custody = await _CustodyRepository.GetSomeCustody(BranchId, Status);
            return Custody;
        }
        public async Task<string> GetEmployeeCustodies(int EmployeeId)
        {
            var allcus=_CustodyRepository.GetCustodiesByEmployeeId(EmployeeId);
            var custmony = allcus.Result.Sum(x => (x.ItemPrice)).ToString();
            return custmony;
        }

        public async Task<IEnumerable<CustodyVM>> GetSomeCustodyVoucher(int BranchId, bool Status)
        {
            var Custody = (List<CustodyVM>)await _CustodyRepository.GetSomeCustodyVoucher(BranchId, Status);
            return Custody;
        }
        public GeneralMessage FreeCustody(int CustodyId, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {
            try
            {
                var CustodyUpdated = _CustodyRepository.GetById(CustodyId);
                if (CustodyUpdated != null)
                {
                    var Emp = _employeesRepository.GetById((int)CustodyUpdated.EmployeeId);


                    CustodyUpdated.Status = true;


                    int? ResponsibleUserId = Emp.UserId;
                    if (Emp !=null)
                    {
                        var CustItem = _ItemRepository.GetById((int)CustodyUpdated.ItemId);
                        var CustodyTypeName = "";
                        decimal CustodyPrice = 0;
                        if (CustodyUpdated.Type == 1)
                        {
                            CustodyTypeName = CustItem.NameAr;
                            CustodyPrice = CustItem.Price;
                        }
                        else
                        {
                            CustodyTypeName = "عهدة نقدية";
                            CustodyPrice = CustodyUpdated.CustodyValue ?? 0;
                        }
                        string NotStr = string.Format("تم فك عهدة الموظف {0} ({1}) ", Emp.EmployeeNameAr, CustodyTypeName);
                        NotStr = NotStr + string.Format(" الكمية {0}, السعر {1}, ميعاد استلام العهدة {2}", CustodyUpdated.Quantity, CustodyPrice, CustodyUpdated.Date);
                      
                        var UserNotification = new Notification();
                        if (Emp.UserId != null && Emp.UserId > 0)
                        {
                            var user = Emp.UserId.Value;
                            string htmlBody = "";
                            var userObj = _usersRepository.GetById(user);
                            //mail
                            string OrgName = _OrganizationsRepository.GetBranchOrganization().Result.NameAr;

                            htmlBody = @"<!DOCTYPE html><html lang = ''><head><meta name='viewport' content='width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=0'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
                                    <meta charset = 'utf-8><meta name = 'description' content = ''><meta name = 'keywords' content = ''><meta name = 'csrf-token' content = ''><title></title><link rel = 'icon' type = 'image/x-icon' href = ''></head>
                                    <body style = 'background:#f9f9f9;direction:rtl'><div class='container' style='max-width:630px;padding-right: var(--bs-gutter-x, .75rem); padding-left: var(--bs-gutter-x, .75rem); margin-right: auto;  margin-left: auto;'>
                                    <style> .bordered {width: 550px; height: 700px; padding: 20px;border: 3px solid yellowgreen; background-color:lightgray;} </style>
                                    <div class= 'row' style = 'font-family: Cairo, sans-serif'>  <div class= 'card' style = 'padding: 2rem;background:#fff'> <div style = 'width: 550px; height: 700px; padding: 20px; border: 3px solid yellowgreen; background-color: lightgray;'> <p style='text-align:center'></p>
                                    <h4> عزيزي الموظف " + Emp.EmployeeNameAr + "</h4> <h4> السلام عليكم ورحمة الله وبركاتة</h4> <h3 style = 'text-align:center;' > تم فك العهدة المبين تفاصيلها في الجدول التالي</h3><table align = 'center' border = '1' ><tr> <td>  الموظف</td><td>" + Emp.EmployeeNameAr + @"</td> </tr> <tr> <td> العهدة </td> <td>" + CustodyTypeName + @"</td>
                                     </tr> <tr> <td>  الكمية</td> <td>" + CustodyUpdated.Quantity + @"</td> </tr><tr> <td>   السعر</td> <td>" + CustodyPrice + @"</td> </tr><tr> <td>   ميعاد استلام العهدة</td> <td>" + CustodyUpdated.Date + @"</td> </tr> </table> <p style = 'text-align:center'> " + OrgName + @" </p> <h7> مع تحيات قسم ادارة الموارد البشرية</h7>

                                    </div> </div></div></div></body></html> ";

                            var subject = "فك العهدة";// Resources.ResourceManager.GetString("Emp_ReceiveCustody", CultureInfo.CreateSpecificCulture("ar"));
                            var config = _employeesService.GetNotificationRecipients(Models.Enums.NotificationCode.HR_UnassignAsset, Emp.EmployeeId);

                            if (config.Description != null && config.Description != "")
                                subject = config.Description;

                            if (config.Users != null && config.Users.Count() > 0)
                            {
                                foreach (var usr in config.Users)
                                {
                                    _customerMailService.SendMail_SysNotification((int)Emp.BranchId, usr, usr, subject, htmlBody, true);
                                    UserNotification.ReceiveUserId = usr;
                                    UserNotification.Name = subject;// Resources.ResourceManager.GetString("Notice_CustodyFinish", CultureInfo.CreateSpecificCulture("ar"));
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                                    UserNotification.SendUserId = 1;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = NotStr;
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = 0;
                                    UserNotification.TaskId = 0;
                                    UserNotification.IsHidden = false;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _TaamerProContext.SaveChanges();
                                    _notificationService.sendmobilenotification(usr,subject, NotStr);

                                }
                            }
                            else
                            {
                                if (Emp.Email != null)

                                {
                                    _customerMailService.SendMail_SysNotification((int)Emp.BranchId, 0, 0, subject, htmlBody, true, Emp.Email);
                                }

                                //Notification
                                try
                                {
                                    //if (UserNotifPriv.Count() != 0 && UserNotifPriv.Contains(162))
                                    //{
                                    UserNotification.ReceiveUserId = user;
                                    UserNotification.Name = subject;// Resources.ResourceManager.GetString("Notice_CustodyFinish", CultureInfo.CreateSpecificCulture("ar"));
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                                    UserNotification.SendUserId = 1;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = NotStr;
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = 0;
                                    UserNotification.TaskId = 0;
                                    UserNotification.IsHidden = false;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _TaamerProContext.SaveChanges();

                                    if (Emp.DirectManager != null && Emp.DirectManager != 0)
                                    {
                                        var direectmanager = _TaamerProContext.Employees.Where(x => x.EmployeeId == Emp.DirectManager).FirstOrDefault();

                                        if (direectmanager.Email != null)
                                        {
                                            _customerMailService.SendMail_SysNotification((int)direectmanager.BranchId, 0, 0, subject, htmlBody, true, direectmanager.Email);
                                        }

                                        if (direectmanager != null)
                                        {
                                            var Not_directmanager = new Notification();
                                            Not_directmanager = UserNotification;
                                            Not_directmanager.ReceiveUserId = direectmanager.UserId ?? 0;
                                            Not_directmanager.NotificationId = 0;
                                            _TaamerProContext.Notification.Add(Not_directmanager);
                                            _TaamerProContext.SaveChanges();

                                        }
                                        _notificationService.sendmobilenotification(direectmanager.UserId.Value, subject, NotStr);
                                    }
                                    _notificationService.sendmobilenotification(user, subject, NotStr);

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }

                        }
                    //}


                }

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم فك العهدة";
               _SystemAction.SaveAction("FreeCustody", "CustodyService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage ConvertStatusCustody(int CustodyId, int UserId, int BranchId, string Lang)
        {
            try
            {
                var CustodyUpdated = _CustodyRepository.GetById(CustodyId);
                if (CustodyUpdated != null)
                {
                    CustodyUpdated.ConvertStatus = true;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم التحويل الي الحسابات";
                _SystemAction.SaveAction("ConvertStatusCustody", "CustodyService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage ReturnConvetCustody(int CustodyId, int UserId, int BranchId, string Lang)
        {
            try
            {
                var CustodyUpdated = _CustodyRepository.GetById(CustodyId);
                if (CustodyUpdated != null)
                {
                    if (CustodyUpdated.InvoiceId > 0)
                    {
                        var Inv = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == CustodyUpdated.InvoiceId);
                        if (Inv.Count() > 0)
                        {
                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.transfer_cannot_be_returned };
                        }
                    }
                    CustodyUpdated.ConvertStatus = false;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم رجوع التحويل";
                _SystemAction.SaveAction("ReturnConvetCustody", "CustodyService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage SaveCustody(Custody custody, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {
            try
            {
                bool IsSignedNew = false;
                string returnedMessage = "";
                Custody CustodyUpdated = null;

                if (custody.Type == 2)
                {
                    custody.ItemId = 0;
                    custody.Quantity = 0;
                }
                var CustItemTemp = _ItemRepository.GetById((int)custody.ItemId);
                if (CustItemTemp != null)
                {
                    var qtyitemTemp = 0;
                    var qtyitemRemTemp = 0;
                    qtyitemTemp = custody.Quantity ?? 0;
                    qtyitemRemTemp = CustItemTemp.Ramainder ?? 0;
                    if (qtyitemRemTemp < qtyitemTemp)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في حفظ العهدة";
                        _SystemAction.SaveAction("SaveCustody", "CustodyService", 1, Resources.Not_Find_quantity, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Not_Find_quantity };
                    }
                }
                if (custody.CustodyId == 0)
                {
                    if (custody.EmployeeId != null)
                    {
                        IsSignedNew = true;
                        CustodyUpdated = custody;
                    }


                    custody.BranchId = BranchId;
                    custody.AddUser = UserId;
                    custody.AddDate = DateTime.Now;
                    custody.Status = false;
                    DateTime inputDate = DateTime.ParseExact(custody.Date, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);
                    string formattedDate = inputDate.ToString("yyyy-MM-dd");


                    custody.Date = formattedDate;
                        //if(custody.CustodyValue==null)
                    //{
                    //    custody.CustodyValue = 0;
                    //}
                    //else
                    //{
                    //    custody.CustodyValue = custody.CustodyValue;
                    //}
                    _TaamerProContext.Custody.Add(custody);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة عهدة جديدة";
                    _SystemAction.SaveAction("SaveCustody", "CustodyService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    returnedMessage = Resources.General_SavedSuccessfully;
                    //return new GeneralMessage { Result = true, Message = Resources.General_SavedSuccessfully };
                }
                else
                {
                    CustodyUpdated = _CustodyRepository.GetById(custody.CustodyId);
                    if (CustodyUpdated != null)
                    {
                        //It was free, and assigned to new employee
                        if (CustodyUpdated.Status.HasValue && CustodyUpdated.Status.Value && custody.EmployeeId.HasValue)
                        {
                            CustodyUpdated.EmployeeId = custody.EmployeeId;
                            CustodyUpdated.Status = false;
                            IsSignedNew = true;
                        }
                        CustodyUpdated.ItemId = custody.ItemId;
                        CustodyUpdated.Date = custody.Date;
                        CustodyUpdated.HijriDate = custody.HijriDate;
                        CustodyUpdated.Quantity = custody.Quantity;
                        CustodyUpdated.Type = custody.Type;
                        CustodyUpdated.BranchId = BranchId;
                        CustodyUpdated.UpdateUser = UserId;
                        CustodyUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل عهدة رقم " + custody.CustodyId;
                    _SystemAction.SaveAction("SaveCustody", "CustodyService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    returnedMessage = Resources.General_EditedSuccessfully;
                }

                if (IsSignedNew)
                {
                    var Emp = _employeesRepository.GetById((int)custody.EmployeeId);
                    int? ResponsibleUserId = Emp.UserId;
                    if (Emp !=null)
                    {
                        var CustItem = _ItemRepository.GetById((int)custody.ItemId);
                        var CustodyTypeName = "";
                        decimal CustodyPrice = 0;
                        if (custody.Type == 1)
                        {
                            CustodyTypeName = CustItem.NameAr;
                            CustodyPrice = CustItem.Price;
                        }
                        else
                        {
                            CustodyTypeName = "عهدة نقدية";
                            CustodyPrice = custody.CustodyValue ?? 0;
                        }
                        string NotStr = string.Format(" تم استلام الموظف {0} للعهدة ({1}) ", Emp.EmployeeNameAr, CustodyTypeName);
                        NotStr = NotStr + string.Format(" الكمية {0}, السعر {1}, بتاريخ {2}", custody.Quantity, CustodyPrice, custody.Date);
                        //var UsersWithPriv = _TaamerProContext.UserPrivileges.Where(x => x.IsDeleted == false && x.PrivilegeId == 1414 && x.UserId == ResponsibleUserId).Select(x => x.UserId.Value).ToList();


                            string UserName = "";
                            var UserNotification = new Notification();
                           var user = Emp.UserId.Value;
                           
                                string htmlBody = "";
                                var userObj = _usersRepository.GetById(user);
                        //mail
                        
                        string OrgName = _OrganizationsRepository.GetBranchOrganization().Result.NameAr;
                        var direectmanager = _TaamerProContext.Employees.Where(x => x.EmployeeId == Emp.DirectManager).FirstOrDefault();

                        htmlBody = @"<!DOCTYPE html><html lang = ''><head><meta name='viewport' content='width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=0'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
                                    <meta charset = 'utf-8><meta name = 'description' content = ''><meta name = 'keywords' content = ''><meta name = 'csrf-token' content = ''><title></title><link rel = 'icon' type = 'image/x-icon' href = ''></head>
                                    <body style = 'background:#f9f9f9;direction:rtl'><div class='container' style='max-width:630px;padding-right: var(--bs-gutter-x, .75rem); padding-left: var(--bs-gutter-x, .75rem); margin-right: auto;  margin-left: auto;'>
                                    <style> .bordered {width: 550px; height: 700px; padding: 20px;border: 3px solid yellowgreen; background-color:lightgray;} </style>
                                    <div class= 'row' style = 'font-family: Cairo, sans-serif'>  <div class= 'card' style = 'padding: 2rem;background:#fff'> <div style = 'width: 550px; height: 700px; padding: 20px; border: 3px solid yellowgreen; background-color: lightgray;'> <p style='text-align:center'></p>
                                    <h4> عزيزي الموظف " + Emp.EmployeeNameAr + "</h4> <h4> السلام عليكم ورحمة الله وبركاتة</h4> <h3 style = 'text-align:center;' > تم استلام العهدة المبين تفاصيلها في الجدول التالي</h3><table align = 'center' border = '1' ><tr> <td>  الموظف</td><td>" + Emp.EmployeeNameAr + @"</td> </tr> <tr> <td> العهدة </td> <td>" + CustodyTypeName + @"</td>
                                     </tr> <tr> <td>  الكمية</td> <td>" + custody.Quantity + @"</td> </tr><tr> <td>   السعر</td> <td>" + CustodyPrice + @"</td> </tr><tr> <td>   ميعاد استلام العهدة</td> <td>" + custody.Date + @"</td> </tr> </table> <p style = 'text-align:center'> " + OrgName + @" </p> <h7> مع تحيات قسم ادارة الموارد البشرية</h7>

                                    </div> </div></div></div></body></html> ";
                        var subject = Resources.ResourceManager.GetString("Emp_ReceiveCustody", CultureInfo.CreateSpecificCulture("ar"));
                        var config = _employeesService.GetNotificationRecipients(Models.Enums.NotificationCode.HR_AssignAsset, custody.EmployeeId);

                        if (config.Description != null && config.Description != "")
                            subject = config.Description;

                        if (config.Users != null && config.Users.Count() > 0)
                        {
                            foreach (var usr in config.Users)
                            {
                                _customerMailService.SendMail_SysNotification((int)Emp.BranchId, usr, usr, subject, htmlBody, true);
                                var not = new Notification();
                                not.ReceiveUserId = usr;
                                not.Name = subject;// Resources.ResourceManager.GetString("Emp_ReceiveCustody", CultureInfo.CreateSpecificCulture("ar"));
                                not.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                not.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                                not.SendUserId = 1;
                                not.Type = 1; // notification
                                not.Description = NotStr;
                                not.AllUsers = false;
                                not.SendDate = DateTime.Now;
                                not.ProjectId = 0;
                                not.TaskId = 0;
                                not.IsHidden = false;
                                not.AddUser = UserId;
                                not.AddDate = DateTime.Now;
                                _TaamerProContext.Notification.Add(not);
                                _TaamerProContext.SaveChanges();
                                _notificationService.sendmobilenotification(usr, subject, NotStr);

                            }
                        }
                        else
                        {



                            if (Emp.Email != null)
                            {
                                _customerMailService.SendMail_SysNotification((int)Emp.BranchId, 0, 0, subject, htmlBody, true, Emp.Email);
                            }

                            if (direectmanager != null && direectmanager?.Email != null)
                            {
                                _customerMailService.SendMail_SysNotification((int)direectmanager.BranchId, 0, 0, subject, htmlBody, true, direectmanager.Email);
                            }


                            try
                            {
                                UserNotification.ReceiveUserId = user;
                                UserNotification.Name = Resources.ResourceManager.GetString("Emp_ReceiveCustody", CultureInfo.CreateSpecificCulture("ar"));
                                UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                                UserNotification.SendUserId = 1;
                                UserNotification.Type = 1; // notification
                                UserNotification.Description = NotStr;
                                UserNotification.AllUsers = false;
                                UserNotification.SendDate = DateTime.Now;
                                UserNotification.ProjectId = 0;
                                UserNotification.TaskId = 0;
                                UserNotification.IsHidden = false;
                                UserNotification.AddUser = UserId;
                                UserNotification.AddDate = DateTime.Now;
                                _TaamerProContext.Notification.Add(UserNotification);
                                _TaamerProContext.SaveChanges();

                                if (direectmanager != null)
                                {
                                    var Not_directmanager = new Notification();
                                    Not_directmanager = UserNotification;
                                    Not_directmanager.ReceiveUserId = direectmanager.UserId;
                                    Not_directmanager.AddDate = DateTime.Now.AddMinutes(1);
                                    Not_directmanager.NotificationId = 0;
                                    _TaamerProContext.Notification.Add(Not_directmanager);
                                    _TaamerProContext.SaveChanges();

                                }
                                _TaamerProContext.SaveChanges();
                                _notificationService.sendmobilenotification(user, Resources.ResourceManager.GetString("Emp_ReceiveCustody", CultureInfo.CreateSpecificCulture("ar")), NotStr);
                                if (direectmanager != null)
                                {
                                    _notificationService.sendmobilenotification(direectmanager.UserId.Value, Resources.ResourceManager.GetString("Emp_ReceiveCustody", CultureInfo.CreateSpecificCulture("ar")), NotStr);
                                }

                            }
                            catch (Exception ex)
                            {
                            }



                            //SMS
                            try
                            {
                                var res = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);

                            }
                            catch (Exception ex3)
                            {

                            }
                        }
                            }
                    //    }
                    //}

                    _TaamerProContext.SaveChanges();
                }
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = returnedMessage };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العهدة";
                _SystemAction.SaveAction("SaveCustody", "CustodyService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveCustodyVoucher(Custody custody, int UserId, int BranchId, string Lang)
        {
            try
            {
                var CustodyUpdated = _CustodyRepository.GetById(custody.CustodyId);
                if (CustodyUpdated != null)
                {
                    CustodyUpdated.InvoiceId = custody.InvoiceId;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل عهدة رقم " + custody.CustodyId;
                _SystemAction.SaveAction("SaveCustodyVoucher", "CustodyService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = ActionNote };
            }
            catch (Exception ex)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteCustody(int CustodyId, int UserId, int BranchId)
        {
            try
            {
                Custody custody = _CustodyRepository.GetById(CustodyId);
                custody.IsDeleted = true;
                custody.DeleteDate = DateTime.Now;
                custody.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف عهدة رقم " + CustodyId;
                _SystemAction.SaveAction("DeleteCustody", "CustodyService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف عهدة رقم " + CustodyId; ;
                _SystemAction.SaveAction("DeleteCustody", "CustodyService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public async Task<EmployeesVM> GetEmployeeByItemId(int Item, int BranchId)
        {
            return await _CustodyRepository.GetEmployeeByItemId(Item, BranchId);
        }



        public bool SendMail_recivecustoday(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false,string empmail=null)
        {
            try
            {
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;

                var org = _TaamerProContext.Organizations.Where(x=>x.OrganizationId==branch).FirstOrDefault();
                var mail = new MailMessage();
                var email = _EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail;
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch).Result.Password);
                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                if (_EmailSettingRepository.GetEmailSetting(branch).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(email, _EmailSettingRepository.GetEmailSetting(branch).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(email, "لديك اشعار من نظام تعمير السحابي");
                }
                var title = "";
                var body = "";
                if (type == 1)
                {
                    title = "تم استلام العهدة المبين تفاصيلها في الجدول التالي";
                    body = PopulateBody(textBody, _employeesRepository.GetEmployeeById(ReceivedUser,"rtl").Result.EmployeeNameAr, title, "مع تحيات قسم ادارة الموارد البشرية", Url, org.NameAr);
                }
                else if (type == 2)
                {
                    title = "تم فك العهدة المبين تفاصيلها في الجدول التالي";
                    body = PopulateBody(textBody, _employeesRepository.GetEmployeeById(ReceivedUser, "rtl").Result.EmployeeNameAr, title, "مع تحيات قسم ادارة الموارد البشرية", Url, org.NameAr);
                }

                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                if (empmail != null && empmail != "")
                {
                    mail.To.Add(new MailAddress(empmail));

                }
                else
                {

                    mail.To.Add(new MailAddress(_usersRepository.GetById(ReceivedUser).Email));
                }

                mail.Subject = Subject;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch).Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string PopulateBody(string bodytxt, string fullname, string header, string footer, string url, string orgname)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(url))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FullName}", fullname);
            body = body.Replace("{Body}", bodytxt);
            body = body.Replace("{Header}", header);
            body = body.Replace("{Footer}", footer);

            body = body.Replace("{orgname}", orgname);




            return body;
        }



    }

    public class selectDataClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
