using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Generic;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net.Mime;
using TaamerP.Service.LocalResources;
using static Dropbox.Api.UsersCommon.AccountType;
using Dropbox.Api.Users;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Dropbox.Api.Files.PathOrLink;
using static Dropbox.Api.Files.SearchMatchType;
using System.Runtime.InteropServices;
using Twilio.TwiML.Messaging;
using TaamerProject.Repository.Repositories;
using Microsoft.Identity.Client;

namespace TaamerProject.Service.Services
{
    public class SystemSettingsService :  ISystemSettingsService
    {
        private readonly ISystemSettingsRepository _SystemSettingsRepository;
        private readonly IFiscalyearsRepository _FiscalyearsRepository;
        private readonly IUsersRepository _IUsersRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly INotificationService _notificationService;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IProjectRepository _ProjectRepository;

        public SystemSettingsService(TaamerProjectContext dataContext, ISystemAction systemAction, ISystemSettingsRepository systemSettingsRepository
            , IFiscalyearsRepository fiscalyearsRepository, IUsersRepository usersRepository, IEmailSettingRepository emailSettingRepository
            , IBranchesRepository branchesRepository, IProjectRepository projectRepository, INotificationService notificationService, IOrganizationsRepository organizationsRepository)
        {
            _SystemSettingsRepository = systemSettingsRepository;
            _FiscalyearsRepository = fiscalyearsRepository;
            _IUsersRepository = usersRepository;          
            _EmailSettingRepository = emailSettingRepository;
            _BranchesRepository = branchesRepository;
            _OrganizationsRepository = organizationsRepository;
            _notificationService = notificationService;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _ProjectRepository = projectRepository;

        }


        public Task<SystemSettingsVM> GetSystemSettingsByBranchId(int BranchId)
        {
            var SystemSettings = _SystemSettingsRepository.GetSystemSettingsByBranchId(BranchId);
            return SystemSettings;
        }


        public  SystemSettingsVM GetSystemSettingsByUserId(int BranchId, int UserID, string Con)
        {
            try
            {
                var SystemSettings = _SystemSettingsRepository.GetSystemSettingsByUserId(BranchId, UserID, Con).Result;

                //SystemSettings.DefaultUserSession = _IUsersRepository.GetById(UserID).Session;
                SystemSettings.DefaultUserSession = _TaamerProContext.Users.Where(s => s.UserId == UserID)?.FirstOrDefault()?.Session;

                return SystemSettings;
            }
            catch (Exception ex)
            {
                var SystemSettings = _SystemSettingsRepository.GetSystemSettingsByUserId(BranchId, UserID, Con).Result;

                SystemSettings.DefaultUserSession = _TaamerProContext.Users.Where(s => s.UserId == UserID)?.FirstOrDefault()?.Session;
                return SystemSettings;

            }


        }

        public GeneralMessage ValidateZatcaRequests(bool iszatcacheck,int UserId,int BranchId, string Url, string ImgUrl)
        {
            try
            {
                //var settings = _SystemSettingsRepository.GetMatching(s => s.IsDeleted == false && s.BranchId == BranchId).FirstOrDefault();
                var settings = _TaamerProContext.SystemSettings.Where(s => s.IsDeleted == false).FirstOrDefault();

                var code = GenerateRandomNo();
                var strbody ="";
                string subject = "";
              
               bool issent;
                if (iszatcacheck == true)
                {
                    subject = "كود تفعيل  منصة فاتورة";
                    strbody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                           <label style='font-size:25px;'>  كود التفعيل هو : <input type='text' name='name' value=" + code + @" disabled style='width: 80px;'/></label>
                                                                    <br/>
                                                                <h5>ولربط مع منصة فاتورة ، يرجى إدخال كود التفعيل المكون من أربعة أرقام في شاشة تفعيل الخدمة .</h5>
                                                        <h5>ملاحظة هامة: إن تفعيل منصة فاتورة أوإيقاف تفعيل منصة فاتورة  تقع تحت مسؤوليتكم الكاملة ، حيث لن يتم تفعيل المنصة إلا من خلال إدخال كود التفعيل ، وكذلك لن يتم إيقاف تفعيل المنصة إلا من خلال إدخال كود إلغاء التفعيل ،لذلك يجب الانتباه لذلك</h5>
                                                </table>
                                            </body>
                                            </html>";
                     issent = SendMail_Stamp(BranchId, UserId, UserId, subject, strbody, Url, ImgUrl, 1, true).Result;


                }
                else
                {
                    subject = "كود الغاء تفعيل  منصة فاتورة";
                    strbody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                           <label style='font-size:25px;'>  كود الغاء التفعيل هو : <input type='text' name='name' value=" + code + @" disabled style='width: 80px;'/></label>
                                                                    <br/>
                                                                <h5>ولربط مع منصة فاتورة ، يرجى إدخال كود الغاء التفعيل المكون من أربعة أرقام في شاشة تفعيل الخدمة .</h5>
                                                        <h5>ملاحظة هامة: إن تفعيل منصة فاتورة أوإيقاف تفعيل منصة فاتورة  تقع تحت مسؤوليتكم الكاملة ، حيث لن يتم تفعيل المنصة إلا من خلال إدخال كود التفعيل ، وكذلك لن يتم إيقاف تفعيل المنصة إلا من خلال إدخال كود إلغاء التفعيل ،لذلك يجب الانتباه لذلك</h5>
                                                </table>
                                            </body>
                                            </html>";
                     issent = SendMail_Stamp(BranchId, UserId, UserId, subject, strbody, Url, ImgUrl, 2, true).Result;


                }


                //var issent = SendMail_SysNotification(BranchId, UserId, UserId, "كود تفعيل رفع الفاتورة للزكاة", strbody, false);

                if (issent == true)
                {
                    settings.ZatcaCheckCode = code.ToString();
                    _TaamerProContext.SaveChanges();
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في ارسال الميل";
                     _SystemAction.SaveAction("ValidateZatcaRequest", "SystemSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase ="فشل في ارسال الميل" };

                }



               // _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل خيارات النظام ";
                 _SystemAction.SaveAction("ValidateZatcaRequest", "SystemSettingsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };


            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ خيارات النظام";
                 _SystemAction.SaveAction("ValidateZatcaRequest", "SystemSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }

        }

        public GeneralMessage ValidateDestinationRequest(int UserId, OrganizationsVM Organization, Branch Branch, string Url, string ImgUrl,int ProjectId, int UploadType, string DesName)
        {
            try
            {
                var settings = _TaamerProContext.SystemSettings.Where(s => s.IsDeleted == false).FirstOrDefault();
                var projectData = _ProjectRepository.GetProjectByIdSome("rtl", ProjectId).Result;
                var userReciver = _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefault()!;

                var code = GenerateRandomNo();
                string input = code.ToString();
                char[] charArray = new char[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    charArray[i] = input[i];
                }
                var strbody = "";
                string subject = "";
                string notitxt = "";
                string nametxt = "";
                string headertxt = "";

                if (UploadType == 1)
                {
                    notitxt = " لرفع المشروع  " + projectData.ProjectNo + " لجهة خارجية " + " يرجي إدخال الكود " + "[" + charArray[3] + "-" + charArray[2] + "-" + charArray[1] + "-" + charArray[0] + @"]";
                    nametxt = "رفع مشروع لجهة";
                    subject = "كود تفعيل  رفع لجهة خارجية";
                    headertxt = "لجعل المشروع يظهر بإنه موجود لدي الجهة الخارجية وإيقاف عداد احتساب مدة المشروع بعد العودة من الجهة الخارجية";
                }
                else
                {
                    notitxt = " سوف يتم إضافة الوقت بدل الضائع علي المشروع  " + projectData.ProjectNo + " نظرا لإستلامة من الجهة الخارجية " + " يرجي إدخال الكود " + "[" + charArray[3] + "-" + charArray[2] + "-" + charArray[1] + "-" + charArray[0] + @"]";
                    nametxt = "إستلام من جهة خارجية";
                    subject = "كود تفعيل  إستلام من جهة خارجية";
                    headertxt = "سوف يتم إضافة الوقت بدل الضائع علي المشروع نظرا لإستلامة من الجهة الخارجية";
                }

                bool issent;
                var ListOfPrivNotify = new List<Notification>();
                ListOfPrivNotify.Add(new Notification
                {
                    ReceiveUserId = UserId,
                    Name = nametxt,
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                    HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                    SendUserId = 1,
                    Type = 1,
                    Description = notitxt,
                    AllUsers = false,
                    SendDate = DateTime.Now,
                    ProjectId = ProjectId,
                    TaskId = 0,
                    AddUser = UserId,
                    BranchId = Branch.BranchId,
                    AddDate = DateTime.Now,
                    IsHidden = false,
                    NextTime = null,
                });
                _TaamerProContext.Notification.AddRange(ListOfPrivNotify);

                            strbody= @"<!DOCTYPE html><html lang = '' ><head>
                                                <style>
                                                .square {
                                                    height: 35px;width: 35px;background-color: #ffffff;border: ridge;
                                                    text-align: center;align-content: center;font-size: 28px;}
                                                </style>
                                                </ head >
                            <body>                  
                            <h3 style = 'text-align:center;' > "+ headertxt + @"</h3>
                            <h4 style = 'text-align:center;' > فضلا أدخل الكود  </h4>
                            <div class='row' style='display:flex;justify-content: center !important;margin-bottom: 12px;'>
                            <div class='square'>" + charArray[3] + @"</div>
                            <div class='square'>"+ charArray[2] + @"</div>
                            <div class='square'>"+ charArray[1] + @"</div>
                            <div class='square'>"+ charArray[0] + @"</div>
                            </div>  
                            <table align = 'center' border = '1' ><tr> <td>  رقم المشروع</td><td>" + projectData.ProjectNo+ @"</td> </tr> <tr> <td> اسم العميل  </td> <td>"+ projectData.CustomerName_W+ @"</td>
                             </tr>  <tr> <td> الفرع</td> <td>"+ Branch.NameAr + @"</td></tr><tr> <td> مدير المشروع</td> <td>"+projectData.ProjectMangerName + @"</td></tr><tr> <td> إسم الجهة الخارجية</td> <td>"+ DesName+ @"</td></tr></table> <h7> مع تحيات قسم ادارة المشاريع</h7>                         
                            </ body ></ html > ";

                issent = SendMail_Destination(Organization, Branch, UserId, UserId, subject, strbody, Url, ImgUrl, 1, true).Result;
                _notificationService.sendmobilenotification(UserId, subject, notitxt);

                if (issent == true)
                {
                    settings.DestinationCheckCode = code.ToString();
                    _TaamerProContext.SaveChanges();
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في ارسال الميل";
                    _SystemAction.SaveAction("ValidateZatcaRequest", "SystemSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, Branch.BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في ارسال الميل" };

                }

                // _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل خيارات النظام ";
                _SystemAction.SaveAction("ValidateZatcaRequest", "SystemSettingsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, Branch.BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };


            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ خيارات النظام";
                _SystemAction.SaveAction("ValidateZatcaRequest", "SystemSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, Branch.BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }

        }

        public GeneralMessage ValidateZatcaCode(bool iszatcacheck,string Sentcode, int UserId, int BranchId)
        {
            try
            {
                //var settings = _SystemSettingsRepository.GetMatching(s => s.IsDeleted == false && s.BranchId == BranchId).FirstOrDefault();
                var settings = _TaamerProContext.SystemSettings.Where(s => s.IsDeleted == false).FirstOrDefault();

                if (settings.ZatcaCheckCode == Sentcode)
                {
                    settings.UploadInvZatca = iszatcacheck;
                    settings.ZatcaCheckCode = null;
                    _TaamerProContext.SaveChanges();
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في التحقق من الكود";
                     _SystemAction.SaveAction("ValidateZatcaRequest", "SystemSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };

                }



                // _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل خيارات النظام ";
                 _SystemAction.SaveAction("ValidateZatcaRequest", "SystemSettingsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };


            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ خيارات النظام";
                 _SystemAction.SaveAction("ValidateZatcaRequest", "SystemSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }

        }


        public GeneralMessage UpdateOrgDataRequired(bool isreq, int UserId, int BranchId)
        {
            try
            {
               // var settings = _SystemSettingsRepository.GetMatching(s => s.IsDeleted == false && s.BranchId == BranchId).FirstOrDefault();

                var settings = _TaamerProContext.SystemSettings.Where(s => s.IsDeleted == false).FirstOrDefault();

                if (settings != null)
                {
                    settings.OrgDataIsRequired = isreq;
                    _TaamerProContext.SaveChanges();
                }

                
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل خيارات النظام ";
                 _SystemAction.SaveAction("SaveSystemSettings", "SystemSettingsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };


            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ خيارات النظام";
                 _SystemAction.SaveAction("SaveSystemSettings", "SystemSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }
        }



        public GeneralMessage SaveSystemSettings(SystemSettings systemSettings, int UserId, int BranchId)
        {
            try
            {
                //var settings = _SystemSettingsRepository.GetMatching(s => s.IsDeleted == false && s.BranchId == BranchId);
                var settings = _TaamerProContext.SystemSettings.Where(s => s.IsDeleted == false).FirstOrDefault();

                if (settings == null)
                {
                    systemSettings.BranchId = BranchId;
                    systemSettings.AddUser = UserId;
                    systemSettings.AddDate = DateTime.Now;
                    _TaamerProContext.SystemSettings.Add(systemSettings);

                    if (systemSettings.FiscalYear != null)
                    {
                        //var ActiveYears = _FiscalyearsRepository.GetMatching(s => s.IsDeleted == false && s.IsActive == true);
                        var ActiveYears = _TaamerProContext.FiscalYears.Where(s => s.IsDeleted == false && s.IsActive == true);

                        foreach (var item in ActiveYears)
                        {
                            item.IsActive = false;
                        }
                        //  var FiscalyearsUpdated = _FiscalyearsRepository.GetById(systemSettings.FiscalYear);
                        FiscalYears? FiscalyearsUpdated =  _TaamerProContext.FiscalYears.Where(s => s.YearId == systemSettings.FiscalYear).FirstOrDefault();

                        if (FiscalyearsUpdated != null)
                        {
                            FiscalyearsUpdated.IsActive = true;
                            FiscalyearsUpdated.UpdateUser = UserId;
                            FiscalyearsUpdated.UpdateDate = DateTime.Now;
                        }
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة خيارات نظام جديدة";
                     _SystemAction.SaveAction("SaveSystemSettings", "SystemSettingsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var SystemSettingsUpdated = _SystemSettingsRepository.GetById(systemSettings.SettingId);
                    var SystemSettingsUpdated =  _TaamerProContext.SystemSettings.Where(s => s.SettingId == systemSettings.SettingId).FirstOrDefault();

                    if (SystemSettingsUpdated != null)
                    {
                        SystemSettingsUpdated.FiscalYear = systemSettings.FiscalYear;
                        SystemSettingsUpdated.CustGenerateCode = systemSettings.CustGenerateCode;
                        SystemSettingsUpdated.DecimalPoints = systemSettings.DecimalPoints;
                        SystemSettingsUpdated.CurrencyId = systemSettings.CurrencyId;
                        SystemSettingsUpdated.ContractGenerateCode = systemSettings.ContractGenerateCode;
                        SystemSettingsUpdated.NoReplyMail = systemSettings.NoReplyMail;
                        SystemSettingsUpdated.ProjGenerateCode = systemSettings.ProjGenerateCode;
                        SystemSettingsUpdated.OfferGenerateCode = systemSettings.OfferGenerateCode;
                        SystemSettingsUpdated.ActiveUserNumber = systemSettings.ActiveUserNumber;
                        SystemSettingsUpdated.ActiveCodeInterval = systemSettings.ActiveCodeInterval;
                        SystemSettingsUpdated.AttendenceId = systemSettings.AttendenceId;
                        SystemSettingsUpdated.BranchGenerateCode = systemSettings.BranchGenerateCode;
                        SystemSettingsUpdated.EmpGenerateCode = systemSettings.EmpGenerateCode;
                        SystemSettingsUpdated.VoucherGenerateCode = systemSettings.VoucherGenerateCode;
                        SystemSettingsUpdated.LogErrors = systemSettings.LogErrors;
                        SystemSettingsUpdated.EnableNotification = systemSettings.EnableNotification;
                        SystemSettingsUpdated.EnableSMS = systemSettings.EnableSMS;
                        SystemSettingsUpdated.SMTPPort = systemSettings.SMTPPort;
                        SystemSettingsUpdated.DefaultUserSession = systemSettings.DefaultUserSession;
                        SystemSettingsUpdated.MobileNoDigits = systemSettings.MobileNoDigits;
                        SystemSettingsUpdated.PhoneNoDigits = systemSettings.PhoneNoDigits;
                        SystemSettingsUpdated.NationalIDDigits = systemSettings.NationalIDDigits;
                        SystemSettingsUpdated.UpdateUser = UserId;
                        SystemSettingsUpdated.UpdateDate = DateTime.Now;
                       
                        SystemSettingsUpdated.ContractGenerateCode2 = systemSettings.ContractGenerateCode2;
                        SystemSettingsUpdated.CustomerMailIsRequired = systemSettings.CustomerMailIsRequired;
                        SystemSettingsUpdated.CustomerNationalIdIsRequired = systemSettings.CustomerNationalIdIsRequired;
                        SystemSettingsUpdated.OrgDataIsRequired = systemSettings.OrgDataIsRequired;
                        SystemSettingsUpdated.Contract_Con_Code = systemSettings.Contract_Con_Code;
                        SystemSettingsUpdated.Contract_Sup_Code = systemSettings.Contract_Sup_Code;
                        SystemSettingsUpdated.Contract_Des_Code = systemSettings.Contract_Des_Code;

                        SystemSettingsUpdated.CustomerphoneIsRequired = systemSettings.CustomerphoneIsRequired;
                        SystemSettingsUpdated.ResedentEndNote = systemSettings.ResedentEndNote;
                        SystemSettingsUpdated.ContractEndNote = systemSettings.ContractEndNote;
                        SystemSettingsUpdated.ValueAddedSeparated = systemSettings.ValueAddedSeparated;

                    }

                    if (systemSettings.FiscalYear != null)
                    {
                        //var ActiveYears = _FiscalyearsRepository.GetMatching(s => s.IsDeleted == false && s.IsActive == true);
                        var ActiveYears = _TaamerProContext.FiscalYears.Where(s => s.IsDeleted == false && s.IsActive == true);
                        foreach (var item in ActiveYears)
                        {
                            item.IsActive = false;
                        }
                        //var FiscalyearsUpdated = _FiscalyearsRepository.GetById(systemSettings.FiscalYear);
                        FiscalYears? FiscalyearsUpdated =  _TaamerProContext.FiscalYears.Where(s => s.YearId == systemSettings.FiscalYear).FirstOrDefault();

                        if (FiscalyearsUpdated != null)
                        {
                            FiscalyearsUpdated.IsActive = true;
                            FiscalyearsUpdated.UpdateUser = UserId;
                            FiscalyearsUpdated.UpdateDate = DateTime.Now;
                        }
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تعديل خيارات النظام ";
                     _SystemAction.SaveAction("SaveSystemSettings", "SystemSettingsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };
                }
                
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ خيارات النظام";
                 _SystemAction.SaveAction("SaveSystemSettings", "SystemSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }
        }


        public GeneralMessage MaintenanceFunc(string con, string Lang, int BranchId, int UserId, int Status)
        {
            var Res = _SystemSettingsRepository.MaintenanceFunc(con, Lang, BranchId, UserId, Status).Result;
            if(Res == true)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_EditedSuccessfully };
            }
            else
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        /*********/

        public async Task<bool> SendMail_Stamp(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false)
        {
            try
            {
               // var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                Branch? branch =   _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();

                //var org = _OrganizationsRepository.GetById(branch);
                Organizations? org =   _TaamerProContext.Organizations.Where(s => s.BranchId == branch.BranchId).FirstOrDefault();

                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                var mail = new MailMessage();
                var email = _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail;
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Password);
                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                if (_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(email, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(email, "لديك اشعار من نظام تعمير السحابي");
                }
                var title = "لقد طلبت تفعيل منصة فاتورة التابعة ليهئة الزكاة والضريبة والجمارك - (فاتورة)";
                var body = "";
                if (type == 1)
                {
                    title = "لقد طلبت تفعيل منصة فاتورة التابعة لهيئة الزكاة والضريبة والجمارك - (فاتورة)";
                    body = PopulateBody(textBody, _IUsersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تمنياتنا لكم بالتوفيق", Url, org.NameAr);
                }
                else
                {
                    title = "لقد طلبت الغاء تفعيل منصة فاتورة التابعة لهيئة الزكاة والضريبة والجمارك - (فاتورة)";
                    body = PopulateBody(textBody, _IUsersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تمنياتنا لكم بالتوفيق", Url, org.NameAr);
                }


                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);


                var userReciver =   _TaamerProContext.Users.Where(s => s.UserId == ReceivedUser).FirstOrDefault();
                mail.To.Add(new MailAddress(userReciver?.Email ?? ""));

                mail.Subject = Subject;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> SendMail_Destination(OrganizationsVM org, Branch branch, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false)
        {
            try
            {
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                var mail = new MailMessage();
                var email = _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail;
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Password);
                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                if (_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(email, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(email, "لديك اشعار من نظام تعمير السحابي");
                }
                var title = "لقد طلبت رفع ملف لجهة خارجية";
                var body = "";
                title = "";
                body = PopulateBody(textBody, _IUsersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تمنياتنا لكم بالتوفيق", Url, org.NameAr);


                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);


                var userReciver = _TaamerProContext.Users.Where(s => s.UserId == ReceivedUser).FirstOrDefault();
                mail.To.Add(new MailAddress(userReciver?.Email ?? ""));

                mail.Subject = Subject;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Port);

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


        public async Task<bool> SendMail_SysNotification(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, bool IsBodyHtml = false)
        {
            try
            {
                // var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                int OrganizationId ;
                Branch? branch =   _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                if (branch != null)
                {
                    OrganizationId = branch.OrganizationId;
                }

                var mail = new MailMessage();
                var email = _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail;
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Password);
                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                if (_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(email, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(email, "لديك اشعار من نظام تعمير السحابي");
                }
                // mail.To.Add(new MailAddress(_IUsersRepository.GetById(ReceivedUser).Email));

                var receivedUser =   _TaamerProContext.Users.Where(s => s.UserId == ReceivedUser).FirstOrDefault();
                mail.To.Add(new MailAddress(receivedUser?.Email ?? ""));

                mail.Subject = Subject;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        public void  SaveAction(string FunctionName, string ServiceName, int ActionType, string MessageName,
            string ModuleName, string PageName, string ActionDate, int UserId, int BranchId, string Note, int Success)
        {
            try
            {
                Sys_SystemActions SysAction = new Sys_SystemActions();
                SysAction.FunctionName = FunctionName;
                SysAction.ServiceName = ServiceName;
                SysAction.ActionType = ActionType;
                SysAction.MessageName = MessageName;
                SysAction.ModuleName = ModuleName;
                SysAction.PageName = PageName;
                SysAction.ActionDate = ActionDate;
                SysAction.UserId = UserId;
                SysAction.BranchId = BranchId;
                SysAction.Note = Note;
                SysAction.Success = Success;
                SysAction.AddUser = UserId;
                SysAction.AddDate = DateTime.Now;
                SysAction.IsDeleted = false;
                _TaamerProContext.Sys_SystemActions.Add(SysAction);
                _TaamerProContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exx = ex.Message;
            }

        }


    }
}
