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
using TaamerP.Service.LocalResources;
using Twilio.TwiML.Voice;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net.Mime;
using TaamerProject.Repository.Repositories;
//using Org.BouncyCastle.Bcpg;

namespace TaamerProject.Service.Services
{
    public class OffersPricesService : IOffersPricesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICustomerPaymentsRepository _customerPaymentsRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly IOffersPricesRepository _offersPricesRepository;
        private readonly IOfferpriceconditionRepository _offerpriceconditionRepository;
        private readonly IOfferServiceRepository _offerServiceRepository;
        private readonly IBranchesRepository _branchesRepository;
        private readonly ISystemSettingsRepository _SystemSettingsRepository;
        private readonly ICustomerMailService _customerMailService;



        public OffersPricesService(TaamerProjectContext dataContext, ISystemAction systemAction, ICustomerPaymentsRepository customerPaymentsRepository,
            ISystemSettingsRepository systemSettingsRepository, IBranchesRepository branchesRepository,
            IProjectRepository projectRepository, IOffersPricesRepository offersPricesRepository, IOfferpriceconditionRepository offerpriceconditionRepository,
            IOfferServiceRepository offerServiceRepository, ICustomerMailService customerMailService)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _customerPaymentsRepository = customerPaymentsRepository;
            _ProjectRepository = projectRepository;
            _offersPricesRepository = offersPricesRepository;
            _offerpriceconditionRepository = offerpriceconditionRepository;
            _offerServiceRepository = offerServiceRepository;
            _branchesRepository = branchesRepository;
            _SystemSettingsRepository = systemSettingsRepository;
            _customerMailService = customerMailService;
        }


        public async Task<IEnumerable<OffersPricesVM>> GetAllOffers(int BranchId)
        {
            var offers = await _offersPricesRepository.GetAllOffers(BranchId);
            return offers;
        }

        public IEnumerable<object> Fillcustomerhavingoffer(int BranchId)
        {

            var offerCustSelect = _offersPricesRepository.GetAllOffers(BranchId).Result.Where(w => w.CustomerId != 0).Select(x => new

            {
                Id = x.CustomerId,
                Name = x.CustomerName,
            }).Distinct();

            return offerCustSelect;
        }

        public async Task<IEnumerable<OffersPricesVM>> GetOfferByid(int offerid)
        {
            var offers = await _offersPricesRepository.GetOfferByid(offerid);
            return offers;
        }
        public async Task<IEnumerable<OffersPricesVM>> GetOfferByCustomerData(int offerid, string NationalId, int ActivationCode)
        {
            var offers = await _offersPricesRepository.GetOfferByCustomerData(offerid, NationalId, ActivationCode);
            return offers;
        }

        public async Task<IEnumerable<OffersPricesVM>> GetOfferByid2(int offerid)
        {
            var offers = await _offersPricesRepository.GetOfferByid2(offerid);
            return offers;
        }

        public async Task<IEnumerable<OffersPricesVM>> Getofferconestintroduction(int BranchId)
        {
            var offers = await _offersPricesRepository.Getofferconestintroduction(BranchId);
            return offers;
        }

        public async Task<IEnumerable<OffersPricesVM>> GetAllOfferByCustomerId(int CustomerId)
        {
            var offers = await _offersPricesRepository.GetOfferByCustomerId(CustomerId);
            return offers;
        }
        public async Task<IEnumerable<OffersPricesVM>> GetAllOffersByProjectId(int ProjectId)
        {
            var offers = await _offersPricesRepository.GetAllOffersByProjectId(ProjectId);
            return offers;
        }
        public IEnumerable<OffersPricesVM> GetOfferByCustomerId(int CustomerId)
        {
            var offers = _offersPricesRepository.GetOfferByCustomerId(CustomerId);

            var prooffer = _ProjectRepository.GetAllProjByCustomerId2(CustomerId).Result.Where(s => s.OffersPricesId != 0).Select(x => new { OffersPricesId = x.OffersPricesId, Id = x.OffersPricesId }).ToList();

            var filter = prooffer.Select(x => x.OffersPricesId).ToArray();

            var UnmatchedIds = offers.Result.Where(x => !filter.Contains(x.OffersPricesId)).ToList();


            return UnmatchedIds;
        }
        public IEnumerable<OffersPricesVM> GetOfferByCustomerIdProject(int CustomerId, int ProjectId)
        {
            var offers = _offersPricesRepository.GetOfferByCustomerId(CustomerId);

            var prooffer = _ProjectRepository.GetAllProjByCustomerId2Pro(CustomerId, ProjectId).Result.Where(s => s.OffersPricesId != 0).Select(x => new { OffersPricesId = x.OffersPricesId, Id = x.OffersPricesId }).ToList();

            var filter = prooffer.Select(x => x.OffersPricesId).ToArray();

            var UnmatchedIds = offers.Result.Where(x => !filter.Contains(x.OffersPricesId)).ToList();


            return UnmatchedIds;
        }
        public async Task<IEnumerable<OffersPricesVM>> GetOfferByCustomerIdOld(int CustomerId)
        {
            var offers = await _offersPricesRepository.GetOfferByCustomerId(CustomerId);

            return offers;
        }
        public IEnumerable<OffersPricesVM> GetOfferByCustomerId2(int CustomerId, int offerpriceid)
        {
            var offers = _offersPricesRepository.GetOfferByCustomerId(CustomerId);

            var prooffer = _ProjectRepository.GetAllProjByCustomerId2(CustomerId).Result.Where(s => s.OffersPricesId != 0).Select(x => new { OffersPricesId = x.OffersPricesId, Id = x.OffersPricesId }).ToList();

            var filter = prooffer.Select(x => x.OffersPricesId).ToArray();

            var UnmatchedIds = offers.Result.Where(x => !filter.Contains(x.OffersPricesId)).ToList();

            var offerofpro = _offersPricesRepository.GetOfferByid(offerpriceid).Result;
            var alloffer = UnmatchedIds.Union(offerofpro);


            return alloffer;
        }
        public async Task<IEnumerable<OffersPricesVM>> GetOfferPrice_Search(string offerno, string Date, string customername, string presenter, decimal? Amount, int BranchId)
        {
            return await _offersPricesRepository.GetOfferPrice_Search(offerno, Date, customername, customername, Amount, BranchId);
        }
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        public GeneralMessage Intoduceoffer(int offerpriceid, int UserId, int BranchId, string Url, string Link)
        {
            try
            {
                var offerno = "";
                if (offerpriceid != 0)
                {
                    var offer = _offersPricesRepository.GetById(offerpriceid);
                    offerno = offer.OfferNo;
                    offer.UpdateUser = UserId;
                    offer.UpdateDate = DateTime.Now;
                    offer.OfferStatus = 1;
                    Customer customer = new Customer();
                    var LinkString = "";
                    if (offer.CustomerId != null)
                    {
                        customer = _TaamerProContext.Customer.Where(s => s.CustomerId == offer.CustomerId)!.FirstOrDefault()!;
                        LinkString = "<h3><a style='font-size: 20px;color:red' href=" + Link + @">اضغط هنا للذهاب للرابط</a></h3>";
                    }
                    else
                    {

                        customer.CustomerNameAr = offer.CustomerName ?? "";
                        customer.CustomerEmail = offer.CustomerEmail ?? "";
                        LinkString = "";
                    }
                    var custAddress = "----";
                    if (customer.CustomerTypeId == 1) custAddress = customer.CustomerNationalId ?? "----";
                    else if (customer.CustomerTypeId == 2) custAddress = customer.CommercialRegister ?? "----";
                    else if (customer.CustomerTypeId == 3) custAddress = customer.CommercialRegister ?? "----";
                    else custAddress = "----";

                    customer.CustomerNationalId = customer.CustomerNationalId ?? "----";
                    var code = GenerateRandomNo(); var strbody = ""; string subject = ""; bool issent;
                    subject = "الموافقة علي عرض السعر";
                    strbody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                           <label style='font-size:23px;'>  رقم الهوية\السجل التجاري هو : <input type='text' name='name' value=" + custAddress + @" disabled style='width: 40%;font-size: 30px;text-align: center;border-radius: 17px;'/></label>
                                                                    <br/>
                                           <label style='font-size:23px;'>  رقم المرجع هو : <input type='text' name='name' value=" + offer.OffersPricesId + @" disabled style='margin-right: 19%;width: 38%;font-size: 30px;text-align: center;border-radius: 17px;'/></label>
                                                                    <br/>
                                           <label style='font-size:23px;'>  كود الموافقة هو : <input type='text' name='name' value=" + code + @" disabled style='margin-right: 18%;width: 40%;font-weight: bold;color: red;font-size: 30px;text-align: center;border-radius: 17px;'/></label>
                                                                    <br/>
                                                                " + LinkString + @"
                                                </table>
                                            </body>
                                            </html>";
                    issent = SendMailCustomerCodeAccept(BranchId, customer, subject, strbody, Url, true).Result;


                    if (issent == true)
                    {
                        offer.CustomerMailCode = code;
                        _TaamerProContext.SaveChanges();
                    }
                    else
                    {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في ارسال الميل" };
                    }

                }
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم الموافقة علي عرض سعر رقم" + offerno;
                _SystemAction.SaveAction("Intoduceoffer", "OffersPricesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحفظ " };
            }
            catch (Exception ex)
            {
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في الموافقة علي عرض سعر";
                _SystemAction.SaveAction("Intoduceoffer", "OffersPricesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------


                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
        public GeneralMessage IntoduceofferManual(int offerpriceid, int UserId, int BranchId, string Url, string Link)
        {
            try
            {
                if (offerpriceid != 0)
                {
                    var offer = _offersPricesRepository.GetById(offerpriceid);

                    offer.UpdateUser = UserId;
                    offer.UpdateDate = DateTime.Now;
                    offer.OfferStatus = 1;
                    _TaamerProContext.SaveChanges();

                }
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم الحفظ ";
                _SystemAction.SaveAction("Intoduceoffer", "OffersPricesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحفظ " };
            }
            catch (Exception ex)
            {
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في الحفظ";
                _SystemAction.SaveAction("Intoduceoffer", "OffersPricesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

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
        public async Task<bool> SendMailCustomerCodeAccept(int BranchId, Customer customer, string Subject, string textBody, string Url, bool IsBodyHtml = false)
        {
            try
            {
                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                Organizations? org = _TaamerProContext.Organizations.Where(s => s.OrganizationId == branch.OrganizationId).FirstOrDefault();
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                var mail = new MailMessage();
                var email = org.Email;
                var loginInfo = new NetworkCredential(org.Email, org.Password);


                var title = "وللموافقة علي عرض السعر ، يرجى إدخال كود الموافقة المكون من أربعة أرقام من خلال هذا الرابط بالاسفل";
                var body = PopulateBody(textBody, customer.CustomerNameAr ?? "", title, "مع خالص الشكر والتقدير", Url, org.NameAr);
                var img = org.LogoUrl.Remove(0, 1);
                var ImgUrl = Path.Combine(img);
                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                var displaynm = "";
                if (org.SenderName != null)
                {
                    displaynm = org.SenderName;

                }
                else
                {
                    displaynm = org.NameAr;

                }
                mail.From = new MailAddress(email ?? "", displaynm);
                mail.To.Add(new MailAddress(customer.CustomerEmail ?? ""));

                mail.Subject = Subject;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(org.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(org.Port);
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public GeneralMessage Customeraccept(int offerpriceid, int UserId, int BranchId)
        {
            try
            {
                if (offerpriceid != 0)
                {
                    var offer = _offersPricesRepository.GetById(offerpriceid);
                    if (offer.OfferStatus != 1)
                    {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "قدم العرض اولا" };
                    }

                    //offer.UpdateUser = UserId;
                    offer.UpdateDate = DateTime.Now;
                    offer.CustomerStatus = 1;

                    _TaamerProContext.SaveChanges();

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteOffer(int offerpriceid, int UserId, int BranchId)
        {
            try
            {
                if (offerpriceid != 0)
                {
                    var offer = _offersPricesRepository.GetById(offerpriceid);


                    offer.DeleteUser = UserId;
                    offer.DeleteDate = DateTime.Now;
                    offer.IsDeleted = true;
                    //_offersPricesRepository.Add(offer);
                    var offercondition = _offerpriceconditionRepository.GetOfferconditionByid(offerpriceid).Result;
                    foreach (var item in offercondition)
                    {
                        var offcondition = _TaamerProContext.OffersConditions.Where(x => x.OffersConditionsId == item.OffersConditionsId).FirstOrDefault();
                        offcondition.IsDeleted = true;
                        offcondition.DeleteDate = DateTime.Now;
                        offcondition.DeleteUser = UserId;
                    }

                    var payment = _customerPaymentsRepository.GetAllCustomerPaymentsbyofferis(offerpriceid).Result;
                    foreach (var item in payment)
                    {
                        var paym = _customerPaymentsRepository.GetById(item.PaymentId);
                        paym.IsDeleted = true;
                        paym.DeleteUser = UserId;
                        paym.DeleteDate = DateTime.Now;
                    }


                    _TaamerProContext.SaveChanges();
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تم حذف عرض سعر رقم : " + offer.OfferNo;
                    _SystemAction.SaveAction("DeleteOffer", "OffersPricesService", 1, "تم حذف عرض سعر رقم : " + offer.OfferNo, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحذف " };
            }
            catch (Exception ex)
            {
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في الحذف";
                _SystemAction.SaveAction("DeleteOffer", "OffersPricesService", 1, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
        public GeneralMessage SaveOffer(OffersPrices offerprice, int UserId, int BranchId, int? yearid)
        {
            try
            {

                var codeExist = _offersPricesRepository.GetMatching(s => s.IsDeleted == false && s.OfferNo == offerprice.OfferNo && s.OffersPricesId != offerprice.OffersPricesId).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ عرض السعر الكود موجود مسبقا";
                    _SystemAction.SaveAction("SaveOffer", "OffersPricesService", 1, "رقم العرض موجود من قبل", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "رقم العرض موجود من قبل" };
                }
                var save = 1;
                if (offerprice.OffersPricesId == 0)
                {
                    save = 1;

                    var offer = new OffersPrices();

                    //offer.OffersPricesId = offerprice.OffersPricesId;
                    offer.OfferNo = offerprice.OfferNo;
                    offer.UserId = offerprice.UserId;
                    offer.CustomerId = offerprice.CustomerId;
                    if (offerprice.CustomerId > 0)
                    {
                        var Customer = _TaamerProContext.Customer.Where(s => s.CustomerId == offerprice.CustomerId).FirstOrDefault()!;
                        offer.CustomerName = Customer.CustomerNameAr;
                        offer.CUstomerName_EN = Customer.CustomerNameEn;
                        offer.CustomerEmail = Customer.CustomerEmail;
                        offer.Customerphone = Customer.CustomerPhone;

                    }
                    else
                    {
                        offer.CustomerName = offerprice.CustomerName;
                        offer.CUstomerName_EN = offerprice.CUstomerName_EN;
                        offer.CustomerEmail = offerprice.CustomerEmail;
                        offer.Customerphone = offerprice.Customerphone;
                    }
                    offer.OfferDate = offerprice.OfferDate;
                    offer.OfferValue = offerprice.OfferValue;
                    offer.OfferValueTxt = offerprice.OfferValueTxt;
                    offer.ServiceId = offerprice.ServiceId;
                    offer.Department = offerprice.Department;
                    offer.IsContainSign = offerprice.IsContainSign;
                    offer.IsContainLogo = offerprice.IsContainLogo;
                    offer.printBankAccount = offerprice.printBankAccount;
                    offer.IsEnglish = offerprice.IsEnglish;
                    offer.Description = offerprice.Description;
                    offer.Introduction = offerprice.Introduction;
                    offer.NickName = offerprice.NickName;
                    offer.setIntroduction = offerprice.setIntroduction;
                    offer.ServQty = offerprice.ServQty;
                    offer.OfferNoType = offerprice.OfferNoType;
                    offer.NotDisCustPrint = offerprice.NotDisCustPrint;
                    offer.ProjectName = offerprice.ProjectName;
                    offer.ImplementationDuration = offerprice.ImplementationDuration;
                    offer.OfferValidity = offerprice.OfferValidity;

                    if (offerprice.setIntroduction == 1)
                    {
                        var offers = _offersPricesRepository.GetMatching(x => x.IsDeleted == false && x.setIntroduction == 1).ToList();
                        foreach (var item in offers)
                        {
                            item.setIntroduction = 0;
                        }

                    }
                    if (offerprice.RememberDate == null || offerprice.RememberDate == "")
                    {
                        offer.RememberDate = null;
                        offer.OfferAlarmCheck = null;
                        offer.ISsent = null;
                    }
                    else
                    {
                        offer.RememberDate = offerprice.RememberDate;
                        offer.OfferAlarmCheck = true;
                        offer.ISsent = 0;
                    }

                    offer.BranchId = BranchId;
                    offer.AddUser = UserId;
                    offer.AddDate = DateTime.Now;

                    _TaamerProContext.OffersPrices.Add(offer);

                    _TaamerProContext.SaveChanges();

                    if (offerprice.setIntroduction == 1)
                    {
                        var offers = _offersPricesRepository.GetMatching(x => x.IsDeleted == false && x.setIntroduction == 1).ToList();
                        foreach (var item in offers)
                        {
                            item.setIntroduction = 0;
                        }

                    }

                    if (offerprice.CustomerPayments != null && offerprice.CustomerPayments.Count > 0)
                    {
                        foreach (var item in offerprice.CustomerPayments)
                        {
                            //decimal amout ;
                            //amout =  decimal.Parse(item.Amount);
                            var any = string.Format("{0:0.00}", item.Amount);
                            decimal amout = decimal.Parse(any);
                            var customerpay = _TaamerProContext.CustomerPayments.Where(s => s.IsDeleted == false && s.Amount == amout && s.AmountValueText == item.AmountValueText).ToList();
                            foreach (var item1 in customerpay)
                            {
                                var custpay = _customerPaymentsRepository.GetById(item1.PaymentId);
                                custpay.Isconst = item.Isconst;
                            }
                        }

                        foreach (var item in offerprice.CustomerPayments.ToList())
                        {
                            item.BranchId = BranchId;
                            item.OfferId = offer.OffersPricesId;
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;

                            _TaamerProContext.CustomerPayments.Add(item);


                        }

                    }

                    if (offerprice.OffersConditions != null && offerprice.OffersConditions.Count > 0)
                    {
                        foreach (var item in offerprice.OffersConditions)
                        {
                            var offercondition = _TaamerProContext.OffersConditions.Where(s => s.IsDeleted == false && s.OfferConditiontxt == item.OfferConditiontxt);
                            foreach (var item1 in offercondition)
                            {
                                var offcondition = _TaamerProContext.OffersConditions.Where(x => x.OffersConditionsId == item1.OffersConditionsId).FirstOrDefault();
                                offcondition.Isconst = item.Isconst;
                            }
                        }
                        foreach (var item in offerprice.OffersConditions)
                        {
                            item.BranchId = BranchId;
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;
                            item.OfferId = offer.OffersPricesId;

                            _TaamerProContext.OffersConditions.Add(item);


                        }

                    }
                    if (offerprice.OfferService != null && offerprice.OfferService.Count > 0)
                    {
                        foreach (var item in offerprice.OfferService)
                        {
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;
                            item.OfferId = offer.OffersPricesId;

                            _TaamerProContext.OfferService.Add(item);


                        }

                    }

                    if (offerprice.ServicesPriceOffer != null && offerprice.ServicesPriceOffer.Count > 0)
                    {
                        foreach (var item in offerprice.ServicesPriceOffer)
                        {
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;
                            item.OfferId = offer.OffersPricesId;

                            _TaamerProContext.Acc_Services_PriceOffer.Add(item);

                        }

                    }


                    _TaamerProContext.SaveChanges();

                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                    string ActionNote2 = "   حفظ عرض السعر رقم : " + offerprice.OfferNo;
                    _SystemAction.SaveAction("saveoffer", "OffersPricesService", 1, "   حفظ عرض السعر رقم : " + offerprice.OfferNo, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                else
                {
                    save = 2;
                    var offer = _offersPricesRepository.GetById(offerprice.OffersPricesId);

                    offer.OffersPricesId = offerprice.OffersPricesId;
                    offer.OfferNo = offerprice.OfferNo;
                    offer.UserId = offerprice.UserId;
                    offer.CustomerId = offerprice.CustomerId;
                    if (offerprice.CustomerId > 0)
                    {
                        var Customer = _TaamerProContext.Customer.Where(s => s.CustomerId == offerprice.CustomerId).FirstOrDefault()!;
                        offer.CustomerName = Customer.CustomerNameAr;
                        offer.CUstomerName_EN = Customer.CustomerNameEn;
                        offer.CustomerEmail = Customer.CustomerEmail;
                        offer.Customerphone = Customer.CustomerPhone;

                    }
                    else
                    {
                        offer.CustomerName = offerprice.CustomerName;
                        offer.CUstomerName_EN = offerprice.CUstomerName_EN;
                        offer.CustomerEmail = offerprice.CustomerEmail;
                        offer.Customerphone = offerprice.Customerphone;
                    }
                    offer.OfferDate = offerprice.OfferDate;
                    offer.OfferValue = offerprice.OfferValue;
                    offer.OfferValueTxt = offerprice.OfferValueTxt;
                    offer.ServiceId = offerprice.ServiceId;
                    offer.Department = offerprice.Department;
                    offer.IsContainSign = offerprice.IsContainSign;
                    offer.IsContainLogo = offerprice.IsContainLogo;
                    offer.printBankAccount = offerprice.printBankAccount;
                    offer.IsEnglish = offerprice.IsEnglish;
                    offer.Description = offerprice.Description;
                    offer.Introduction = offerprice.Introduction;
                    offer.NickName = offerprice.NickName;
                    offer.setIntroduction = offerprice.setIntroduction;
                    offer.ServQty = offerprice.ServQty;
                    offer.NotDisCustPrint = offerprice.NotDisCustPrint;
                    offer.ProjectName = offerprice.ProjectName;
                    offer.ImplementationDuration = offerprice.ImplementationDuration;
                    offer.OfferValidity = offerprice.OfferValidity;
                    if (offerprice.setIntroduction == 1)
                    {
                        var offers = _offersPricesRepository.GetMatching(x => x.IsDeleted == false && x.setIntroduction == 1).ToList();
                        foreach (var item in offers)
                        {
                            item.setIntroduction = 0;
                        }

                    }
                    if (offerprice.RememberDate == null || offerprice.RememberDate == "")
                    {
                        offer.RememberDate = null;
                        offer.OfferAlarmCheck = null;
                        offer.ISsent = null;
                    }
                    else
                    {
                        offer.RememberDate = offerprice.RememberDate;
                        offer.OfferAlarmCheck = true;
                        offer.ISsent = 0;
                    }

                    var offercondition = _offerpriceconditionRepository.GetOfferconditionByid(offerprice.OffersPricesId).Result;
                    foreach (var item in offercondition)
                    {
                        var offcondition = _TaamerProContext.OffersConditions.Where(x => x.OffersConditionsId == item.OffersConditionsId).FirstOrDefault();
                        offcondition.IsDeleted = true;
                    }

                    var payment = _customerPaymentsRepository.GetAllCustomerPaymentsbyofferis(offerprice.OffersPricesId).Result;
                    foreach (var item in payment)
                    {
                        var paym = _customerPaymentsRepository.GetById(item.PaymentId);
                        paym.IsDeleted = true;
                    }


                    var offersrvice = _offerServiceRepository.GetOfferservicenByid(offerprice.OffersPricesId).Result;
                    foreach (var item in offersrvice)
                    {
                        var offservice = _TaamerProContext.OfferService.Where(x => x.OffersServicesId == item.OffersServicesId).FirstOrDefault();
                        offservice.IsDeleted = true;
                    }

                    if (offerprice.CustomerPayments != null && offerprice.CustomerPayments.Count > 0)
                    {
                        foreach (var item in offerprice.CustomerPayments)
                        {

                            var any = string.Format("{0:0.00}", item.Amount);
                            decimal amout = decimal.Parse(any);
                            var customerpay = _TaamerProContext.CustomerPayments.Where(s => s.IsDeleted == false && s.Amount == amout && s.AmountValueText == item.AmountValueText).ToList();

                            foreach (var item1 in customerpay)
                            {
                                var custpay = _customerPaymentsRepository.GetById(item1.PaymentId);
                                custpay.Isconst = item.Isconst;

                            }
                        }
                        foreach (var item in offerprice.CustomerPayments.ToList())
                        {
                            item.BranchId = BranchId;
                            item.OfferId = offerprice.OffersPricesId;
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;

                            _TaamerProContext.CustomerPayments.Add(item);
                        }

                    }

                    if (offerprice.OffersConditions != null && offerprice.OffersConditions.Count > 0)
                    {

                        foreach (var item in offerprice.OffersConditions)
                        {
                            var offercondition1 = _TaamerProContext.OffersConditions.Where(s => s.IsDeleted == false && s.OfferConditiontxt == item.OfferConditiontxt);
                            foreach (var item1 in offercondition1)
                            {
                                var offcondition = _TaamerProContext.OffersConditions.Where(x => x.OffersConditionsId == item1.OffersConditionsId).FirstOrDefault();
                                offcondition.Isconst = item.Isconst;
                            }
                        }
                        foreach (var item in offerprice.OffersConditions)
                        {
                            item.BranchId = BranchId;
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;
                            item.OfferId = offerprice.OffersPricesId;

                            _TaamerProContext.OffersConditions.Add(item);
                        }
                    }

                    if (offerprice.OfferService != null && offerprice.OfferService.Count > 0)
                    {
                        foreach (var item in offerprice.OfferService)
                        {
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;
                            item.OfferId = offerprice.OffersPricesId;

                            _TaamerProContext.OfferService.Add(item);

                        }
                    }
                    //remove prev ServicesPriceOffer
                    var OldDataDetails = _TaamerProContext.Acc_Services_PriceOffer.Where(s => s.OfferId == offerprice.OffersPricesId).ToList();
                    if (OldDataDetails.Count() > 0)
                    {
                        _TaamerProContext.Acc_Services_PriceOffer.RemoveRange(OldDataDetails);
                    }
                    if (offerprice.ServicesPriceOffer != null && offerprice.ServicesPriceOffer.Count > 0)
                    {
                        foreach (var item in offerprice.ServicesPriceOffer)
                        {
                            item.AddUser = UserId;
                            item.SureService = item.SureService ?? 0;
                            item.AddDate = DateTime.Now;
                            item.OfferId = offer.OffersPricesId;
                            _TaamerProContext.Acc_Services_PriceOffer.Add(item);
                        }
                    }
                    _TaamerProContext.SaveChanges();

                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                    string ActionNote2 = "   تعديل عرض السعر رقم : " + offerprice.OfferNo;
                    _SystemAction.SaveAction("saveoffer", "OffersPricesService", 2, "   تعديل عرض السعر رقم : " + offerprice.OfferNo, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------


                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عرض السعر";
                _SystemAction.SaveAction("saveoffer", "OffersPricesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }

        }

        public async Task<string> GenerateNextOfferNumber(int BranchId)
        {

            var OrganzationId = _branchesRepository.GetById(BranchId).OrganizationId;
            var sysSetting = _SystemSettingsRepository.GetSystemSettingsByBranchId(OrganzationId).Result;
            var codePrefix = "";
            var prostartcode = _branchesRepository.GetById(BranchId).OfferStartCode;
            if (prostartcode != null && prostartcode != "")
            {
                codePrefix = prostartcode;
            }
            else if (sysSetting != null && sysSetting.OfferGenerateCode != null)
            {
                codePrefix = sysSetting.OfferGenerateCode;
            }
            else if (sysSetting != null && sysSetting.OfferGenerateCode != null)
            {
                codePrefix = sysSetting.OfferGenerateCode;
            }
            return (codePrefix + await _offersPricesRepository.GenerateNextOfferNumber(BranchId, codePrefix));

            //return (await _offersPricesRepository.GenerateNextOfferNumber());
        }

        public async Task<string> GetOfferCode_S(int BranchId)
        {
            var OrganzationId = _branchesRepository.GetById(BranchId).OrganizationId;
            var sysSetting = _SystemSettingsRepository.GetSystemSettingsByBranchId(OrganzationId).Result;
            //var codePrefix = "";
            //if (sysSetting != null && sysSetting.OfferGenerateCode != null)
            //{
            //    codePrefix = sysSetting.OfferGenerateCode;
            //}

            var codePrefix = "";
            var prostartcode = _branchesRepository.GetById(BranchId).OfferStartCode;
            if (prostartcode != null && prostartcode != "")
            {
                codePrefix = prostartcode;
            }
            else if (sysSetting != null && sysSetting.OfferGenerateCode != null)
            {
                codePrefix = sysSetting.OfferGenerateCode;
            }
            else if (sysSetting != null && sysSetting.OfferGenerateCode != null)
            {
                codePrefix = sysSetting.OfferGenerateCode;
            }

            return codePrefix;
        }
        public async Task<bool> SendMaiCertifyOffer (int BranchId,int UserId, string Subject, string textBody, string Url, bool IsBodyHtml = false)
        {
            try
            {
                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                Organizations? org = _TaamerProContext.Organizations.Where(s => s.OrganizationId == branch.OrganizationId).FirstOrDefault();
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                var mail = new MailMessage();
                var email = org.Email;
                var loginInfo = new NetworkCredential(org.Email, org.Password);
                var user = _TaamerProContext.Users.Where(x => x.UserId == UserId).FirstOrDefault();

                var title = " لاعتماد عرض السعر ، يرجى إدخال كود الموافقة المكون من أربعة أرقام من خلال هذا الرابط بالاسفل";
                var body = PopulateBody(textBody, user .FullNameAr?? "", title, "مع خالص الشكر والتقدير", Url, org.NameAr);
                var img = org.LogoUrl.Remove(0, 1);
                var ImgUrl = Path.Combine(img);
                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                var displaynm = "";
                if (org.SenderName != null)
                {
                    displaynm = org.SenderName;

                }
                else
                {
                    displaynm = org.NameAr;

                }
                mail.From = new MailAddress(email ?? "", displaynm);
                mail.To.Add(new MailAddress(user.Email ?? ""));

                mail.Subject = Subject;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(org.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(org.Port);
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }





        public GeneralMessage CertifyOffer(int offerpriceid, int UserId, int BranchId, string Url)
        {
            try
            {
                string message = "";
                if (offerpriceid != 0)
                {
                    var offer = _offersPricesRepository.GetById(offerpriceid);
                    var code = GenerateRandomNo();

                    offer.UpdateDate = DateTime.Now;
                    offer.CertifiedCode = code.ToString();
                   var strbody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                    
                                           <label style='font-size:23px;'>  رقم عرض السعر : <input type='text' name='name' value=" + offer.OfferNo + @" disabled style='margin-right: 19%;width: 38%;font-size: 30px;text-align: center;border-radius: 17px;'/></label>
                                                                    <br/>
                                           <label style='font-size:23px;'>   مقدم للعميل  : <span>"+offer.CustomerName+@"</span></label>
                                                                    <br/>
                                           <label style='font-size:23px;'>  كود الاعتماد هو : <input type='text' name='name' value=" + code + @" disabled style='margin-right: 18%;width: 40%;font-weight: bold;color: red;font-size: 30px;text-align: center;border-radius: 17px;'/></label>
                                                                    <br/>
                                                </table>
                                            </body>
                                            </html>";
                   _customerMailService.SendMail_SysNotification(BranchId, UserId, UserId, "كود تأكيد إعتماد عرض السعر", strbody, true);
                    _TaamerProContext.SaveChanges();

                }
                var user = _TaamerProContext.Users.Where(x => x.UserId == UserId).FirstOrDefault();
                if(user !=null && user.Email != null)
                {
                  message="لإعتماد عرض السعر يرجي ادخال كود التحقق المرسل إلي البريد " +"  " +  MaskEmail(user.Email);
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = message };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage ConfirmCertifyOffer(int offerpriceid, int UserId, int BranchId, string Code)
        {
            try
            {
                if (offerpriceid != 0)
                {
                    var offer = _offersPricesRepository.GetById(offerpriceid);
                    if(Code != offer.CertifiedCode)
                    {
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في اعتماد  عرض سعر";
                        _SystemAction.SaveAction("Intoduceoffer", "ConfirmCertifyOffer", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------


                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "الكود غير صحيح" };

                    }

                    offer.UpdateDate = DateTime.Now;
                    offer.IsCertified = true;
                    _TaamerProContext.SaveChanges();

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public string MaskEmail(string email)
        {
            var parts = email.Split('@');
            if (parts.Length != 2) return email; // invalid email

            string local = parts[0];
            string domain = parts[1];

            if (local.Length <= 4)
                return email; // too short to mask meaningfully

            string firstTwo = local.Substring(0, 2);
            string lastVisible = "";

            // Get last segment after last dot or last 2-3 letters
            int lastDotIndex = local.LastIndexOf('.');
            if (lastDotIndex != -1 && lastDotIndex < local.Length - 1)
            {
                lastVisible = local.Substring(lastDotIndex);
            }
            else
            {
                lastVisible = local.Substring(local.Length - 3); // last 3 letters
            }

            int starsCount = local.Length - (firstTwo.Length + lastVisible.Length);
            if (starsCount < 1) starsCount = 1;

            string stars = new string('*', starsCount);

            return $"{firstTwo}{stars}{lastVisible}@{domain}";
        }

    }
}
