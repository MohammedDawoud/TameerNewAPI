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
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly IAccountsRepository _accountsRepository;
        private readonly ITransactionsRepository _TransactionsRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly IUsersRepository _usersRepository;
        private readonly IContractRepository _ContractRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IUserBranchesRepository _userBranchesRepository;
        public CustomerService(TaamerProjectContext dataContext, ISystemAction systemAction, ICustomerRepository customerRepository, IAccountsRepository accountsRepository
            , ITransactionsRepository transactionsRepository, IProjectRepository projectRepository, IBranchesRepository branchesRepository
            , IUserNotificationPrivilegesService userNotificationPrivilegesService, IUsersRepository usersRepository,IContractRepository contractRepository, IOrganizationsRepository organizationsRepository, IEmailSettingRepository emailSettingRepository
            , IUserBranchesRepository userBranchesRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _CustomerRepository = customerRepository;
            _accountsRepository = accountsRepository;
            _TransactionsRepository = transactionsRepository;
            _ProjectRepository = projectRepository;
            _BranchesRepository = branchesRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _usersRepository = usersRepository;
            _ContractRepository = contractRepository;
            _OrganizationsRepository = organizationsRepository;
            _EmailSettingRepository = emailSettingRepository;
            _userBranchesRepository = userBranchesRepository;
        }
        public async Task<IQueryable<CustomerVM>> GetAllCustomers(string lang, int BranchId, bool isPrivate)
        {
            var customers =await _CustomerRepository.GetAllCustomers(lang, BranchId, isPrivate);
            return customers;
        }

        public async Task<IQueryable<CustomerVM>> GetAllCustomersSearch(string searchtext, string lang, int BranchId, bool isPrivate)
        {
            var customers = await _CustomerRepository.GetAllCustomersSearch(searchtext, lang, BranchId, isPrivate);
            return customers;
        }

        public async Task<IQueryable<CustomerVM>> GetAllCustomerExist(string lang)
        {
            var customers =await _CustomerRepository.GetAllCustomersexist(lang);
            return customers;
        }

        public async Task<IQueryable<CustomerVM>> GetCustomersArchiveProjects(string lang, int BranchId, bool isPrivate)
        {
            var customers =await _CustomerRepository.GetCustomersArchiveProjects(lang, BranchId, isPrivate);
            return customers;
        }

        public async Task<IQueryable<CustomerVM>> GetCustomersOwnProjects(string lang, int BranchId, bool isPrivate)
        {
            var customers =await _CustomerRepository.GetCustomersOwnProjects(lang, BranchId, isPrivate);
            return customers;
        }
        public async Task<IQueryable<CustomerVM>> GetCustomersOwnNotArcheivedProjects(string lang, int BranchId, bool isPrivate)
        {
            var customers =await _CustomerRepository.GetCustomersOwnNotArcheivedProjects(lang, BranchId, isPrivate);
            return customers;
        }
        public async Task<List<CustomerVM>> GetCustomersNewTask(string lang, int BranchId, int UserId)
        {
            var customers = await _CustomerRepository.GetCustomersNewTask(lang, BranchId,UserId);
            return customers;
        }
        public async Task<IQueryable<CustomerVM>> GetAllCustomersW(string lang, int BranchId, bool isPrivate)
        {
            var customers =await _CustomerRepository.GetAllCustomersW(lang, BranchId, isPrivate);
            return customers;
        }
        public async Task<IQueryable<CustomerVM>> GetAllCustomers(string lang, int BranchId)
        {
            var customers =await _CustomerRepository.GetAllCustomers(lang, BranchId);
            return customers;
        }
        public async Task<IQueryable<CustomerVM>> GetAllCustomersCount(int BranchId)
        {
            var customers =await _CustomerRepository.GetAllCustomersCount(BranchId);
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> CustomerInterval(string FromDate, string ToDate, int BranchId, string lang)
        {
            var customers =await _CustomerRepository.CustomerInterval(FromDate, ToDate, BranchId, lang);
            return customers;
        }

        public async Task<IEnumerable<CustomerVM>> CustomerIntervalByCustomerType(string FromDate, string ToDate, int ByCustomerTypeId, int BranchId, string lang)
        {
            var customers =await _CustomerRepository.CustomerIntervalByCustomerType(FromDate, ToDate, ByCustomerTypeId, BranchId, lang);
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> GetCustomerExpensesRevenue(string FromDate, string ToDate, int BranchId, string lang)
        {
            var customers =await _CustomerRepository.GetCustomerExpensesRevenue(FromDate, ToDate, BranchId, lang);
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> GetCustomerExpensesRevenueDGV(string FromDate, string ToDate, int BranchId, string lang, string Con)
        {
            var customers =await _CustomerRepository.GetCustomerExpensesRevenueDGV(FromDate, ToDate, BranchId, lang, Con);
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> GetAllCustomersByCustomerTypeId(int? CustomerTypeId, string lang, int BranchId, bool isPrivate)
        {
            var projectTypeUpdated =  _TaamerProContext.ProjectType.Where(x=>x.TypeId==CustomerTypeId).FirstOrDefault();
            if (projectTypeUpdated == null && CustomerTypeId == 0)
            {
                var allcustomers =await _CustomerRepository.GetAllCustomersW(lang, BranchId, isPrivate);
                return allcustomers;
            }
            else
            {
                if (projectTypeUpdated.Typeum != null)
                {
                    var customers =await _CustomerRepository.GetAllCustomersByCustomerTypeId(projectTypeUpdated.Typeum, lang, BranchId, isPrivate);

                    return customers;
                }
                else
                {
                    var customer =await _CustomerRepository.GetAllCustomersByCustomerTypeId(CustomerTypeId, lang, BranchId, isPrivate);

                    return customer;

                }
            }
        }
        public async Task<IEnumerable<CustomerVM>> GetAllPrivateCustomers(string lang, int BranchId)
        {
            var customers =await _CustomerRepository.GetAllPrivateCustomers(lang, BranchId);
            return customers;
        }
        public async Task<IEnumerable<object>> GetCustFinancialDetails(string FromDate, string ToDate, int CustomerId, int? yearid)
        {
            return await _TransactionsRepository.GetAllTransByCustomerId(CustomerId, FromDate, ToDate, yearid ?? default(int));
        }
        public async Task<IEnumerable<TransactionsVM>> GetAllCustomerTrans(string FromDate, string ToDate, int BranchId, int? yearid)
        {
            var Transactions = await _TransactionsRepository.GetAllCustomerTrans(FromDate, ToDate, yearid ?? default(int), BranchId);
            return Transactions;
        }
        public string GetAccountCodeNewValue(int parentid, int accountid)
        {
            try
            {
                var AccountCodeNew = "";
                var CodeNewList = new List<decimal>();
                var Accounts = _accountsRepository.GetMatching(s => s.ParentId == parentid && s.IsDeleted == false).Where(a => a.AccountId != accountid);
                if (Accounts.Count() > 0)
                {
                    foreach (var v in Accounts)
                    {
                        if (v.AccountCodeNew != "")
                        {
                            decimal ValueDec = Convert.ToDecimal(v.AccountCodeNew);
                            CodeNewList.Add(ValueDec);
                        }

                    }
                    var getmax = CodeNewList.Max();
                    AccountCodeNew = (getmax + 1).ToString();

                }
                else
                {
                    var Accounts2 = _accountsRepository.GetById(parentid);
                    if (((Accounts2.Level ?? 0) + 1 == 1) || ((Accounts2.Level ?? 0) + 1 == 2))
                    {
                        AccountCodeNew = Accounts2.AccountCodeNew + "01";
                    }
                    else
                    {
                        AccountCodeNew = Accounts2.AccountCodeNew + "0001";

                    }
                }
                return AccountCodeNew;
            }
            catch (Exception ex)
            {

                return "";
            }


        }

        public GeneralMessage SaveCustomer(Customer customer, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                int customerAccCode = 0;

                if(customer.CustomerTypeId!=1)
                {
                    customer.CustomerNationalId = null;
                }

                if (customer.CustomerId == 0)
                {
                    if (customer.CustomerTypeId==1 && customer.CustomerNationalId!=null && customer.CustomerNationalId != "")
                    {
                        var NationalIdExist = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerNationalId == customer.CustomerNationalId).ToList();

                        if (NationalIdExist.Count() > 0)
                        {
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "من فضلك أدخل رقم هوية أخر هذا الرقم موجود من قبل" };
                        }
                    }


                    customer.BranchId = customer.BranchId;
                    customer.AddUser = UserId;
                    customer.AddDate = DateTime.Now;
                    //customer Acc
                    //  if (customer.AccountId == null)
                    /// {
                    try
                    {
                        if (customer.CustomerTypeId == 2)
                        {
                            if (customer.CommercialRegister != null && customer.CommercialRegister != "")
                            {
                                var CommercialRegister = _CustomerRepository.GetCustomersByCommercialRegister(customer.CommercialRegister, "ar").Result;

                                if (CommercialRegister != null)
                                {
                                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Change_the_CR_it_already_exists };
                                }
                            }
                        }
                        var Branch = _BranchesRepository.GetById(customer.BranchId);
                        if (Branch != null && Branch.CustomersAccId != null)
                        {
                            var parentCustAcc = _accountsRepository.GetById((int)Branch.CustomersAccId);
                            var newCustAcc = new Accounts();

                            var AccCode=_accountsRepository.GetNewCodeByParentId(Branch.CustomersAccId??0,1).Result;
                            newCustAcc.Code = AccCode;


                            newCustAcc.Classification = parentCustAcc.Classification?? 15;
                            newCustAcc.ParentId = parentCustAcc.AccountId;
                            newCustAcc.IsMain = false;
                            newCustAcc.Level = parentCustAcc.Level + 1;
                            newCustAcc.Nature = 1; //depit
                            newCustAcc.Halala = true;
                            newCustAcc.NameAr = "حساب العميل" + "  " + customer.CustomerNameAr;
                            newCustAcc.NameEn = customer.CustomerNameEn + " " + "Customer Account";
                            newCustAcc.Type = 2; //bugget
                            newCustAcc.IsMainCustAcc = false;
                            newCustAcc.Active = true;
                            newCustAcc.AddUser = UserId;
                            newCustAcc.BranchId = customer.BranchId;
                            newCustAcc.AddDate = DateTime.Now;

                            _TaamerProContext.Accounts.Add(newCustAcc);
                            parentCustAcc.IsMain = true; // update main acc
                            _TaamerProContext.SaveChanges();
                            customer.AccountId = newCustAcc.AccountId;//_accountsRepository.GetMaxId() + 1;
                            var cutAcc = _accountsRepository.GetById((int)customer.AccountId);
                            if (cutAcc != null)
                            {
                                // customerAccCode = Convert.ToInt32(cutAcc.Code);
                            }
                            //update main acc
                            if (cutAcc != null)
                            {
                                if (cutAcc.ParentId != null)
                                {
                                    var ParentAccount = _TaamerProContext.Accounts.Where(x => x.AccountId == cutAcc.ParentId).FirstOrDefault();
                                    ParentAccount.IsMain = true;
                                }
                            }
                            var branch2 = _BranchesRepository.GetById(customer.BranchId);
                            var ListOfPrivNotify = new List<Notification>();
                            //foreach (var userCounter in _userPrivilegesRepository.GetMatching(s => s.IsDeleted == false && s.PrivilegeId == 1210).Where(w => w.Users.IsDeleted == false))  //add tasks notifications
                            var CustNoti = _TaamerProContext.UserNotificationPrivileges.Where(s => s.IsDeleted == false && s.PrivilegeId == 21).Where(w => w.Users.IsDeleted == false);
                            foreach (var userCounter in CustNoti) //New Customer
                            {
                                var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(userCounter.UserId.Value).Result;


                                if (UserNotifPriv.Count() != 0)
                                {

                                    try
                                    {
                                        string NotStr = " تم اضافة عميل جديد باسم : " + customer.CustomerNameAr + " فرع  : " + branch2.NameAr + "";
                                        if (UserNotifPriv.Count() != 0 && UserNotifPriv.Contains(212))
                                        {
                                            //Notification
                                            ListOfPrivNotify.Add(new Notification
                                            {
                                                ReceiveUserId = userCounter.UserId,
                                                Name = " عميل جديد",
                                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                                SendUserId = 1,
                                                Type = 1, // notification
                                                Description = NotStr,
                                                AllUsers = false,
                                                SendDate = DateTime.Now,
                                                ProjectId = 0,
                                                TaskId = 0,
                                                AddUser = UserId,
                                                BranchId = BranchId,
                                                AddDate = DateTime.Now,
                                                IsHidden = false
                                            });
                                          //  _notificationService.sendmobilenotification((int)userCounter.UserId, " عميل جديد", NotStr);
                                        }
                                    }
                                    catch (Exception ex1)
                                    {

                                    }


                                    try
                                    {
                                        var userObj = _usersRepository.GetById((int)userCounter.UserId);
                                        var customerType = customer.CustomerTypeId.Value == 1 ? ("مواطن") : customer.CustomerTypeId.Value == 2 ?
                                            ("مستثمرون وشركات") : customer.CustomerTypeId.Value == 3 ? ("جهة حكومية") : "";
                                        //mail
                                        if (UserNotifPriv.Count() != 0 && UserNotifPriv.Contains(211))
                                        {
                                            string htmlBody = @"<!DOCTYPE html>
                                    <html>

                                    <head></head>

                                    <body style='direction: rtl;'>
                                
                                        <table style=' border: 1px solid black; border-collapse: collapse;'>
                                            <thead>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>العميل</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الفرع</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>نوع العميل</th>
                                            <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تاريخ الإضافة</th>
                                          </thead>
                                          <tbody>
                                            <tr>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer.CustomerNameAr + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + branch2.NameAr + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customerType + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")) + @"</td>
                                            </tr>
                                          </tbody>
                                        </table>

                                      
                                    </body>
                                    </html>";
                                            // bool mail = _customerMailService.SendMail_SysNotification(BranchId, UserId, userCounter.UserId.Value, "عميل جديد", htmlBody, true);
                                            bool mail = SendMail_ProjectStamp(BranchId, UserId, userCounter.UserId.Value, "عميل جديد", htmlBody, Url, ImgUrl, 1, true);
                                        }
                                        //SMS
                                        if (UserNotifPriv.Count() != 0 && UserNotifPriv.Contains(213))
                                        {
                                            string NotStr2 = " تم اضافة عميل جديد باسم : " + customer.CustomerNameAr + " فرع  : " + branch2.NameAr + "";

                                            var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr2, UserId, BranchId);
                                        }
                                    }
                                    catch (Exception ex2)
                                    {

                                    }



                                }

                            }
                            try
                            {

                            }
                            catch (Exception ex3)
                            {
                                _TaamerProContext.Notification.AddRange(ListOfPrivNotify);
                            }

                            //_uow.SaveChanges();
                            //-----------------------------------------------------------------------------------------------------------------


                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في حفظ العميل";
                           _SystemAction.SaveAction("SaveCustomer", "CustomerService", 1, Resources.error_saving_financial_account_for_clients, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.error_saving_financial_account_for_clients };
                        }
                    }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "فشل في حفظ العميل";
                       _SystemAction.SaveAction("SaveCustomer", "CustomerService", 1, Resources.Not_saved_financial_account_for_clients, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Not_saved_financial_account_for_clients };
                    }
                    List<Customer_Branches> customer_s = new List<Customer_Branches>();
                    if (customer.OtherBranches != null && customer.OtherBranches.Count() > 0)
                    {
                        foreach (var item in customer.OtherBranches)
                        {
                            var custbranches = new Customer_Branches();
                            custbranches.BranchId = item;
                            customer_s.Add(custbranches);


                        }
                        customer.Customer_Branches = customer_s;
                    }
                    // }
                    //customer.AccountId = _accountsRepository.GetMaxId() + 1;
                    _TaamerProContext.Customer.Add(customer);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة عميل جديد" +"باسم "+ customer.CustomerNameAr;
                   _SystemAction.SaveAction("SaveCustomer", "CustomerService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully,ReturnedParm= customer.CustomerId };
                }
                else
                {
                    var customerUpdated = _CustomerRepository.GetById(customer.CustomerId);
                    if (customerUpdated != null)
                    {

                        if (customer.CustomerTypeId == 1 && customer.CustomerNationalId != null && customer.CustomerNationalId != "")
                        {
                            var NationalIdExist = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerNationalId == customer.CustomerNationalId && s.CustomerId != customer.CustomerId).ToList();

                            if (NationalIdExist.Count() > 0)
                            {
                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "من فضلك أدخل رقم هوية أخر هذا الرقم موجود من قبل" };
                            }
                        }


                        customerUpdated.CustomerCode = customer.CustomerCode;
                        customerUpdated.CustomerNameAr = customer.CustomerNameAr;
                        customerUpdated.CustomerNameEn = customer.CustomerNameEn;
                        customerUpdated.CustomerNationalId = customer.CustomerNationalId;
                        customerUpdated.NationalIdSource = customer.NationalIdSource;
                        customerUpdated.CustomerAddress = customer.CustomerAddress;
                        customerUpdated.CustomerEmail = customer.CustomerEmail;
                        customerUpdated.CustomerPhone = customer.CustomerPhone;
                        customerUpdated.CustomerMobile = customer.CustomerMobile;
                        customerUpdated.Notes = customer.Notes;
                        customerUpdated.CustomerTypeId = customer.CustomerTypeId;
                        customerUpdated.LogoUrl = customer.LogoUrl;
                        customerUpdated.AttachmentUrl = customer.AttachmentUrl;
                        customerUpdated.CommercialActivity = customer.CommercialActivity;
                        customerUpdated.CommercialRegister = customer.CommercialRegister;
                        customerUpdated.CommercialRegDate = customer.CommercialRegDate;
                        customerUpdated.CommercialRegHijriDate = customer.CommercialRegHijriDate;
                        //customerUpdated.AccountId = customer.AccountId;
                        customerUpdated.GeneralManager = customer.GeneralManager;
                        customerUpdated.AgentName = customer.AgentName;
                        customerUpdated.AgentType = customer.AgentType;
                        customerUpdated.AgentNumber = customer.AgentNumber;
                        customerUpdated.ResponsiblePerson = customer.ResponsiblePerson;
                        customerUpdated.CompAddress = customer.CompAddress;
                        customerUpdated.PostalCodeFinal = customer.PostalCodeFinal;
                        customerUpdated.Country = customer.Country;
                        customerUpdated.Neighborhood = customer.Neighborhood;
                        customerUpdated.StreetName = customer.StreetName;
                        customerUpdated.BuildingNumber = customer.BuildingNumber;
                        customerUpdated.CityId = customer.CityId;

                        customerUpdated.ExternalPhone = customer.ExternalPhone;
                        if (customer.AgentAttachmentUrl != null)
                        {
                            customerUpdated.AgentAttachmentUrl = customer.AgentAttachmentUrl;
                        }
                        if (customerUpdated.BranchId != customer.BranchId)
                        {
                            var cutAccs = _accountsRepository.GetById((int)customerUpdated.AccountId);

                            cutAccs.BranchId = customer.BranchId;
                        }

                        customerUpdated.BranchId = customer.BranchId;

                        customerUpdated.UpdateUser = UserId;
                        customerUpdated.UpdateDate = DateTime.Now;
                        var cutAcc = _accountsRepository.GetById((int)customerUpdated.AccountId);
                        if (cutAcc != null)
                        {
                            //customerAccCode = Convert.ToInt32(cutAcc.Code);
                            cutAcc.BranchId = customer.BranchId;
                            cutAcc.NameAr = "حساب العميل" + "  " + customerUpdated.CustomerNameAr;
                            cutAcc.NameEn = customerUpdated.CustomerNameEn + " " + "Customer Account";
                        }


                        var costcenter = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.CustomerId == customer.CustomerId);
                        if (costcenter.Count() > 0)
                        {
                            foreach (var cost in costcenter)
                            {
                                cost.NameAr = customerUpdated.CustomerNameAr;
                                cost.NameEn = customerUpdated.CustomerNameEn;
                            }

                        }

                        List<Customer_Branches> customer_s = new List<Customer_Branches>();
                        var oldbranches = _TaamerProContext.Customer_Branches.Where(x => x.CustomerId == customer.CustomerId).ToList();
                        if(oldbranches !=null && oldbranches.Count() > 0)
                        {
                            _TaamerProContext.Customer_Branches.RemoveRange(oldbranches);
                        }
                        if (customer.OtherBranches != null && customer.OtherBranches.Count() > 0)
                        {
                            foreach (var item in customer.OtherBranches)
                            {
                                var custbranches = new Customer_Branches();
                                custbranches.BranchId = item;
                                custbranches.CustomerId = customer.CustomerId;
                                _TaamerProContext.Customer_Branches.Add(custbranches);


                            }
                        }


                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل عميل  " + customer.CustomerNameAr;
                   _SystemAction.SaveAction("SaveCustomer", "CustomerService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully, ReturnedParm = customer.CustomerId };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العميل";
               _SystemAction.SaveAction("SaveCustomer", "CustomerService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteCustomer(int customerId, int UserId, int BranchId)
        {
            try
            {
                Customer customer = _CustomerRepository.GetById(customerId);

                var customerProject = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.CustomerId == customerId && s.Status != 1).Count();
                if (customerProject > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف عميل رقم " + customer.CustomerNameAr; ;
                   _SystemAction.SaveAction("DeleteCustomer", "CustomerService", 3, Resources.delete_project , "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.delete_project };
                }

                try
                {
                    //Customer customer = _CustomerRepository.GetById(customerId);
                    //var Inv=_InvoicesRepository.GetMatching(s=>s.IsDeleted==false && s.ToAccountId)
                    var AccTrans = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.AccountId == customer.AccountId);
                    if (AccTrans != null && AccTrans.Count() > 0)
                    {

                        foreach (var item in AccTrans)
                        {
                            var invv = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == item.InvoiceId && s.Rad == false);
                            if (invv != null && invv.Count() > 0)
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = " فشل في حذف عميل رقم " + customer.CustomerNameAr; ;
                                _SystemAction.SaveAction("DeleteCustomer", "CustomerService", 3, Resources.Financial_Transactions_Message_Error, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Financial_Transactions_Message_Error };
                            }
                        }

                    }

                    int Count = _accountsRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == customer.AccountId).Count();
                    if (Count > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = " فشل في حذف عميل رقم " + customer.CustomerNameAr; ;
                       _SystemAction.SaveAction("DeleteCustomer", "Acc_ClausesService", 3, Resources.AccountDeletedError, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.AccountDeletedError };
                    }
                    Accounts account = _accountsRepository.GetById((int)customer.AccountId);
                    account.IsDeleted = true;
                    account.DeleteDate = DateTime.Now;
                    account.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    customer.IsDeleted = true;
                    customer.DeleteDate = DateTime.Now;
                    customer.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف عميل  " + customer.CustomerNameAr;
                   _SystemAction.SaveAction("DeleteCustomer", "Acc_ClausesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
                }
                catch (Exception)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف عميل رقم " + customer.CustomerNameAr; ;
                   _SystemAction.SaveAction("DeleteCustomer", "Acc_ClausesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedFailed };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف عميل رقم " + customerId; ;
               _SystemAction.SaveAction("DeleteCustomer", "Acc_ClausesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public async Task<IEnumerable<CustomerVM>> SearchCustomers(CustomerVM CustomersSearch, string lang, int BranchId)
        {
            var customers =await _CustomerRepository.SearchCustomers(CustomersSearch, lang, BranchId);
            return customers;
        }
        public async Task<int?> GenerateNextCustomerCodeNumber()
        {
            return await _CustomerRepository.GenerateNextCustomerCodeNumber();
        }

        public async Task<IEnumerable<CustomerVM>> GetAllCustomersProj(string lang, int BranchId)
        {
            var customers =await _CustomerRepository.GetAllCustomersProj(lang, BranchId);
            return customers;
        }
        public async Task<IEnumerable<ContractsVM>> GetAllCustHaveRemainingMoney(CustomerVM CustomersSearch, string lang, int BranchId)
        {
            var customers = await _ContractRepository.GetAllCustHaveRemainingMoney(CustomersSearch, lang, BranchId);
            return customers;
        }
        public async Task< CustomerVM> GetCustomersByCustomerId(int? CustomerId, string lang)
        {
            var customers =await _CustomerRepository.GetCustomersByCustomerId(CustomerId, lang);
            return customers;
        }
        public async Task<CustomerVM> GetCustomersByProjectId(int? ProjectId, string lang)
        {
            var project = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectId).FirstOrDefault()!;
            var customers = await _CustomerRepository.GetCustomersByCustomerId(project.CustomerId, lang);
            return customers;
        }
        public async Task<CustomerVM> GetCustomersByCustomerIdInvoice(int? CustomerId, string lang)
        {
            var customers = await _CustomerRepository.GetCustomersByCustomerIdInvoice(CustomerId, lang);
            return customers;
        }
        public async Task<CustomerVM> GetCustomersByAccountId(int? AccountId, string lang)
        {
            var customers =await _CustomerRepository.GetCustomersByAccountId(AccountId, lang);
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> GetCustomersByCustId(int? CustomerId)
        {
            var customers = await _CustomerRepository.GetCustomersByCustId(CustomerId);
            return customers;
        }

        public string GetAccountByCustomerId(int CustomerId, string lang)
        {
            var cust = _CustomerRepository.GetCustomersByCustomerIdOnly(CustomerId, lang).Result;
            if (cust != null)
            {
                return cust.AccountName;
            }
            else
            {
                return "";
            }
        }

        public async Task<IEnumerable<CustomerVM>> GetAllCustomer()
        {
            var Customer =await _CustomerRepository.GetAllCustomer();
            return Customer;
        }
        public async Task<CustomerVM> GetCustomerInfo(string SearchText)
        {
            var Customer =await _CustomerRepository.GetCustomerInfo(SearchText);
            return Customer;
        }
        public async Task<CustomerVM> GetCustomersByNationalId(string NationalId, string lang)
        {
            var customers =await _CustomerRepository.GetCustomersByNationalId(NationalId, lang);
            return customers;
        }

        public async Task<CustomerVM> GetCustomersByCommercialRegister(string CommercialRegister, string lang)
        {
            var customers =await _CustomerRepository.GetCustomersByCommercialRegister(CommercialRegister, lang);
            return customers;
        }

        public IEnumerable<object> FillAllCustomerSelect(string SearchText = "")
        {
            return _CustomerRepository.GetAllCustomers(SearchText).Result.Select(s => new
            {
                Id = s.CustomerId,
                Name = s.CustomerNameAr
            });
        }
        public IEnumerable<object> FillAllCustomerSelectWithBranch(string SearchText, int BranchId)
        {
            return _CustomerRepository.FillAllCustomerSelectWithBranch(SearchText, BranchId).Result.Select(s => new
            {
                Id = s.CustomerId,
                Name = s.CustomerNameAr
            });
        }
        public IEnumerable<object> FillAllCustomerSelectNotHaveProj(string lang, int BranchId)
        {
            return _CustomerRepository.GetAllCustomersNotHaveProj(lang, BranchId).Result.Select(s => new
            {
                Id = s.CustomerId,
                Name = s.CustomerNameAr
            });
        }
        public IEnumerable<object> FillAllCustomerSelectNotHaveProjWithBranch(string lang, int BranchId)
        {
            return _CustomerRepository.FillAllCustomerSelectNotHaveProjWithBranch(lang, BranchId).Result.Select(s => new
            {
                Id = s.CustomerId,
                Name = s.CustomerNameAr
            });
        }
        public IEnumerable<object> FillAllCustomerSelectNotHaveProjWithout(string lang, int BranchId)
        {
            return _CustomerRepository.GetAllCustomersNotHaveProjWithout(lang, BranchId).Result.Select(s => new
            {
                Id = s.CustomerId,
                Name = s.CustomerNameAr
            });
        }

        public async Task<IEnumerable<CustomerVM>> GetAllCustomersByCustomerTypeId(string FromDate, string ToDate, int CustomerTypeId, int BranchId, string lang)
        {
            var customers =await _CustomerRepository.CustomerIntervalByCustomerType(FromDate, ToDate, CustomerTypeId, BranchId, lang);
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> GetAllCustomerForDrop(string lang)
        {
            var customers = await _CustomerRepository.GetAllCustomerForDrop(lang);
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> GetAllCustomerForDropWithBranch(string lang, int BranchId)
        {
            var customers = await _CustomerRepository.GetAllCustomerForDropWithBranch(lang, BranchId);
            return customers;
        }

        public bool SendMail_ProjectStamp(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false)
        {
            try
            {
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                var org = _TaamerProContext.Organizations.Where(x=>x.OrganizationId==branch).FirstOrDefault();
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

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
                    title = "تم اضافة عميل جديد بالبيانات التالية";
                    body = PopulateBody(textBody, _usersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, Resources.Greetings_from_the_customer_department, Url, org.NameAr);
                }

                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                mail.To.Add(new MailAddress(_usersRepository.GetById(ReceivedUser).Email));


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
}
