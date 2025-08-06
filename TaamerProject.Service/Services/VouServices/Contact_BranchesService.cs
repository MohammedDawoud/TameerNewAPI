using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Contact_BranchesService : IContact_BranchesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IContact_BranchesRepository _contact_BranchesRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;


        public Contact_BranchesService(TaamerProjectContext dataContext, ISystemAction systemAction,
            IContact_BranchesRepository contact_BranchesRepository, IEmailSettingRepository emailSettingRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _contact_BranchesRepository = contact_BranchesRepository;
            _EmailSettingRepository = emailSettingRepository;
        }
        public async Task<IEnumerable<Contact_BranchesVM>> GetAllContactBranches()
        {
            var ContBranches =await _contact_BranchesRepository.GetAllContactBranches();


            return ContBranches;

        }

        public GeneralMessage SaveContactBranches(Contact_Branches COntactbranch, int UserId, int BranchId)
        {
            try
            {
                if (COntactbranch.ContactId == 0)
                {
                    COntactbranch.AddUser = UserId;
                    COntactbranch.AddDate = DateTime.Now;
                    _TaamerProContext.Contact_Branches.Add(COntactbranch);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة فرع";
                   _SystemAction.SaveAction("SaveContactBranches", "Contact_BranchesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var CBUpdate = _TaamerProContext.Contact_Branches.Where(x => x.ContactId == COntactbranch.ContactId).FirstOrDefault();
                    if (CBUpdate != null)
                    {
                        CBUpdate.BranchName = COntactbranch.BranchName;
                        CBUpdate.BranchPhone = COntactbranch.BranchPhone;
                        CBUpdate.BranchEmail = COntactbranch.BranchEmail;
                        CBUpdate.BranchCS = COntactbranch.BranchCS;
                        CBUpdate.BranchAddress = COntactbranch.BranchAddress;

                        CBUpdate.UpdateUser = UserId;
                        CBUpdate.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل  الفرع ";
                    _SystemAction.SaveAction("SaveContactBranches", "Contact_BranchesService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            { //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الفرع";
                _SystemAction.SaveAction("SaveContactBranches", "Contact_BranchesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage DeleteContactBranches(int ContactId, int UserId, int BranchId)
        {
            try
            {

                Contact_Branches contact = _TaamerProContext.Contact_Branches.Where(x=>x.ContactId==ContactId).FirstOrDefault();
                contact.IsDeleted = true;
                contact.DeleteDate = DateTime.Now;
                contact.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }



        public GeneralMessage SendMail_SysContact(int branchcontactid, string Name, string textBody, string MobileNumber)
        {
            try
            {
                int BranchId = 1;
                var branch = _TaamerProContext.Branch.Where(x=>x.BranchId==BranchId).FirstOrDefault().OrganizationId;

                var brnchcontct = _TaamerProContext.Contact_Branches.Where(x=>x.ContactId==branchcontactid).FirstOrDefault();
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
                mail.To.Add(new MailAddress(brnchcontct.BranchEmail));
                mail.Subject = Name;
                string Desc = "السلام عليكم ورحمة الله وبركاتة";
                Desc = Desc + "<br/>";
                Desc = Desc + "الاسم :  " + Name;
                Desc = Desc + "<br/>";
                Desc = Desc + "رقم الجوال  : " + MobileNumber;
                Desc = Desc + "<br/>";
                Desc = Desc + textBody;
                mail.Body = Desc;
                mail.IsBodyHtml = true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch).Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Gerneral_send };
            }
            catch (Exception ex)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Not_Send };
            }
        }
    }
}
