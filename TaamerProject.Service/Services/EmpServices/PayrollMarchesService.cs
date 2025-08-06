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
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class PayrollMarchesService :   IPayrollMarchesService
    {
        private readonly IPayrollMarchesRepository _payrollMarchesRepository;
         private readonly IEmployeesRepository _employeesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IOrganizationsService _organizationsService;
        private readonly ICustomerMailService _customerMailService;
        private readonly INotificationService _notificationService;
        public PayrollMarchesService(IPayrollMarchesRepository payrollMarchesRepository 
            , IEmployeesRepository employeesRepository
            , TaamerProjectContext dataContext, ISystemAction systemAction, IOrganizationsService organizationsService
            ,ICustomerMailService customerMailService, INotificationService notificationService)
        {
            _payrollMarchesRepository = payrollMarchesRepository;
             _employeesRepository = employeesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _organizationsService = organizationsService;
            _customerMailService = customerMailService;
            _notificationService = notificationService;
        }
        
        public Task<PayrollMarches> GetPayrollMarches(int EmpId, int MonthId) {
            return _payrollMarchesRepository.GetPayrollMarches(EmpId, MonthId);
        }
        public Task<IEnumerable<PayrollMarchesVM>> GetPayrollMarches(int MonthId, int BranchSearch, string SearchText) {
            var poyrolls = _payrollMarchesRepository.GetPayrollMarches(MonthId, BranchSearch, SearchText);
            return poyrolls;
        }


        public Task<IEnumerable<PayrollMarchesVM>> GetPayrollMarches(int MonthId, int BranchSearch, string SearchText,int YearId)
        {
            var poyrolls = _payrollMarchesRepository.GetPayrollMarches(MonthId, BranchSearch, SearchText, YearId);
            return poyrolls;
        }
        public GeneralMessage SavePayrollMarches(PayrollMarches payroll, int UserId, int BranchId) {
            try
            {
                // var Employee = _employeesRepository.GetById(payroll.EmpId);
                Employees? Employee = _TaamerProContext.Employees.Where(s => s.EmployeeId == payroll.EmpId).FirstOrDefault();
                
                if (Employee == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ  مسير رواتب";
                    _SystemAction.SaveAction("SavePayrollMarches", "PayrollMarchesService", 1, Resources.NoEmployee, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.NoEmployee };
                }

                if (payroll.PayrollId == 0)
                {
                    //payroll.MonthNo = DateTime.Now.Month;
                    payroll.AddUser = UserId;
                    payroll.AddDate = DateTime.Now;
                    payroll.IsPostVoucher = false;

                    _TaamerProContext.PayrollMarches.Add(payroll);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مسير رواتب جديد";
                    _SystemAction.SaveAction("SavePayrollMarches", "PayrollMarchesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage() {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                {
                    //var UpdatedPayroll = _payrollMarchesRepository.GetById(payroll.PayrollId);
                    PayrollMarches? UpdatedPayroll = _TaamerProContext.PayrollMarches.Where(s => s.PayrollId == payroll.PayrollId).FirstOrDefault();

                    if (UpdatedPayroll == null)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "فشل في حفظ  مسير رواتب";
                        _SystemAction.SaveAction("SavePayrollMarches", "PayrollMarchesService", 2, Resources.noPayrollThisNumber, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.noPayrollThisNumber };
                    }
                    if (UpdatedPayroll.PostDate.HasValue)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ  مسير رواتب";
                        _SystemAction.SaveAction("SavePayrollMarches", "PayrollMarchesService", 2, "لا يمكن التعديل على مسير مُرحل", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.notPossibleModifyRelayPath };
                    }
                    
                    UpdatedPayroll.MonthNo = payroll.MonthNo;
                    UpdatedPayroll.MainSalary = payroll.MainSalary;
                    UpdatedPayroll.SalaryOfThisMonth = payroll.SalaryOfThisMonth;
                    UpdatedPayroll.Bonus = payroll.Bonus;
                    UpdatedPayroll.CommunicationAllawance = payroll.CommunicationAllawance;
                    UpdatedPayroll.ProfessionAllawance = payroll.ProfessionAllawance;
                    UpdatedPayroll.TransportationAllawance = payroll.TransportationAllawance;
                    UpdatedPayroll.HousingAllowance = payroll.HousingAllowance;
                    UpdatedPayroll.MonthlyAllowances = payroll.MonthlyAllowances;
                    UpdatedPayroll.ExtraAllowances = payroll.ExtraAllowances;
                    UpdatedPayroll.TotalRewards = payroll.TotalRewards;
                    UpdatedPayroll.TotalDiscounts = payroll.TotalDiscounts;
                    UpdatedPayroll.TotalLoans = payroll.TotalLoans;
                    UpdatedPayroll.TotalSalaryOfThisMonth = payroll.TotalSalaryOfThisMonth;
                    UpdatedPayroll.UpdateUser = payroll.UpdateUser;
                    UpdatedPayroll.UpdateDate = payroll.UpdateDate;
                    UpdatedPayroll.TotalAbsDays = payroll.TotalAbsDays;
                    UpdatedPayroll.TotalVacations = payroll.TotalVacations;
                    UpdatedPayroll.Taamen = payroll.Taamen;
                    UpdatedPayroll.YearId= payroll.YearId;
                    UpdatedPayroll.TotalLateDiscount = payroll.TotalLateDiscount;
                    UpdatedPayroll.TotalAbsenceDiscount = payroll.TotalAbsenceDiscount;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مسير رواتب رقم " + payroll.PayrollId;
                    _SystemAction.SaveAction("SavePayrollMarches", "PayrollMarchesService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage() {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = "فشل في حفظ  مسير رواتب";
                _SystemAction.SaveAction("SavePayrollMarches", "PayrollMarchesService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage PostPayrollMarches(int PayrollId, int UserId, int BranchId)
        {
            try
            {
                //var UpdatedPayroll = _payrollMarchesRepository.GetById(PayrollId);
                PayrollMarches? UpdatedPayroll = _TaamerProContext.PayrollMarches.Where(s => s.PayrollId == PayrollId).FirstOrDefault();

                if (UpdatedPayroll == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في حفظ  مسير رواتب";
                    _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.noPayrollThisNumber, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.noPayrollThisNumber };
                }
                //
                //--------الحسابات-------
                //
                if (UpdatedPayroll.PostDate != null)
                {
                    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.payrollPathAlreadyMigrated };
                }
                UpdatedPayroll.PostDate = DateTime.Now;

                
                UpdatedPayroll.UpdateDate = DateTime.Now;
                UpdatedPayroll.UpdateUser = UserId;
                UpdatedPayroll.IsPostVoucher = false;
                _TaamerProContext.SaveChanges();
                sendemployeemail(UpdatedPayroll);
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " ترحيل مسير رواتب رقم " + PayrollId;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.PayrollProcessNo + PayrollId, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
               
                return new GeneralMessage() {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.ResourceManager.GetString("PayrollProcessNo", CultureInfo.CreateSpecificCulture("ar")) + PayrollId };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.ResourceManager.GetString("PayrollProcessNoFaild", CultureInfo.CreateSpecificCulture("ar")) + PayrollId;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.PayrollProcessNoFaild + PayrollId, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.ResourceManager.GetString("PayrollProcessNoFaild", CultureInfo.CreateSpecificCulture("ar")) + PayrollId };
            }
        }
        public GeneralMessage PostEmpPayroll_Back(int PayrollId, int UserId, int BranchId)
        {
            try
            {
               // var UpdatedPayroll = _payrollMarchesRepository.GetById(PayrollId);
                PayrollMarches? UpdatedPayroll = _TaamerProContext.PayrollMarches.Where(s => s.PayrollId == PayrollId).FirstOrDefault();

                if (UpdatedPayroll == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في حفظ  مسير رواتب";
                    _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.noPayrollThisNumber, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.noPayrollThisNumber };
                }

                if (UpdatedPayroll.PostDate == null)
                {
                    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = "تم فك ترحيل مسير الرواتب هذا مسبقا " };
                }
                if (UpdatedPayroll.IsPostVoucher == true)
                {
                    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = "لا يمكن فك الترحيل لانه تم ترحيل القيود " };
                }

                if (UpdatedPayroll.IsPostPayVoucher == true)
                {
                    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = "لا يمكن فك الترحيل لانه تم اضافة صرف للمسير " };
                }
                UpdatedPayroll.PostDate = null;
                UpdatedPayroll.UpdateDate = DateTime.Now;
                UpdatedPayroll.UpdateUser = UserId;
                UpdatedPayroll.IsPostVoucher = false;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فك ترحيل مسير رواتب رقم " + PayrollId;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, "تم فك ترحيل مسير الرواتب رقم" + PayrollId, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
              
                return new GeneralMessage() {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.ResourceManager.GetString("PayrollProcessNo", CultureInfo.CreateSpecificCulture("ar")) + PayrollId };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = " فشل في فك ترحيل مسير رواتب رقم" + PayrollId;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, " فشل في فك ترحيل مسير رواتب رقم" + PayrollId, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                
                return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.ResourceManager.GetString("PayrollProcessNoFaild", CultureInfo.CreateSpecificCulture("ar")) + PayrollId };
            }
        }

        
        public GeneralMessage PostPayrollMarcheslist(List<Int32> payrollid, int UserId, int BranchId)
        {
            try
            {
                int CountNMora7l = 0;
                int CountMora7l = 0;
                for (int i = 0; i < payrollid.Count; i++)
                {
                   // var UpdatedPayroll = _payrollMarchesRepository.GetById(payrollid[i]);
                    PayrollMarches? UpdatedPayroll = _TaamerProContext.PayrollMarches.Where(s => s.PayrollId == payrollid[i]).FirstOrDefault();


                    if (UpdatedPayroll != null) {
                    //if (UpdatedPayroll == null)
                    //{
                    //    //-----------------------------------------------------------------------------------------------------------------
                    //    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //    string ActionNote3 = "فشل في حفظ  مسير رواتب";
                    //    _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.noPayrollThisNumber, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //    //-----------------------------------------------------------------------------------------------------------------

                    //    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.noPayrollThisNumber };
                    //}
                    //
                    //--------الحسابات-------
                    //
                    CountNMora7l += 1;
                    UpdatedPayroll.PostDate = DateTime.Now;
                    UpdatedPayroll.UpdateDate = DateTime.Now;
                    UpdatedPayroll.UpdateUser = UserId;
                    UpdatedPayroll.IsPostVoucher = false;
                    _TaamerProContext.SaveChanges();
                        sendemployeemail(UpdatedPayroll);
                    }
                    else
                    {
                        CountMora7l += 1;
                    }
                }

                string Message = "";
                if (CountMora7l == 0 && CountNMora7l > 0)
                {
                    Message = "Resources.Deported";
                    //return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.Deported };
                }
                else if (CountMora7l > 0 && CountNMora7l == 0)
                {
                    // _TaamerProContext.SaveChanges();
                    Message = "Resources.Restrictionspreviouslyposted";
                    //return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.Restrictionspreviouslyposted };
                }
                else if (CountMora7l > 0 && CountNMora7l > 0)
                {
                    //_TaamerProContext.SaveChanges();
                    string Msg = String.Format("Resources.posted", CountNMora7l, CountMora7l);
                    Message = Msg;
                    //return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Msg };
                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " ترحيل مسيرات رواتب  " ;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.payrollPathAlreadyMigrated, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.payrollPathAlreadyMigrated };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.PayrollProcessNoFaild ;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.PayrollProcessFaild , "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.PayrollProcessFaild };
            }
        }

        public GeneralMessage PostEmpPayrollVoucher(int PayrollId, int UserId, int BranchId)
        {
            try
            {
                // var UpdatedPayroll = _payrollMarchesRepository.GetById(PayrollId);
                PayrollMarches? UpdatedPayroll = _TaamerProContext.PayrollMarches.Where(s => s.PayrollId == PayrollId).FirstOrDefault();

                if (UpdatedPayroll == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في ترحيل قيود  مسير رواتب";
                    _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.noPayrollThisNumber, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.noPayrollThisNumber };
                }
                //
                //--------الحسابات-------
                //
                UpdatedPayroll.IsPostVoucher = true;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " ترحيل مسير رواتب رقم " + PayrollId;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.PayrollProcessNo + PayrollId, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.PayrollProcessNo + PayrollId };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.PayrollProcessNoFaild + PayrollId;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.PayrollProcessNoFaild + PayrollId, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.PayrollProcessNoFaild + PayrollId };
            }
        }



        public GeneralMessage PostAllEmpPayrollVoucher(List<int> PayrollId, int UserId, int BranchId)
        {
            try
            {
                foreach(var pyrol in PayrollId) {
                    // var UpdatedPayroll = _payrollMarchesRepository.GetById(pyrol);
                    PayrollMarches? UpdatedPayroll = _TaamerProContext.PayrollMarches.Where(s => s.PayrollId == pyrol).FirstOrDefault();

                    if (UpdatedPayroll == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في ترحيل قيود  مسير رواتب";
                    _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.noPayrollThisNumber, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.noPayrollThisNumber };
                }
                //
                //--------الحسابات-------
                //
                UpdatedPayroll.IsPostVoucher = true;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " ترحيل مسيرات رواتب  ";
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.payrollPathAlreadyMigrated, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage() 
                {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.payrollPathAlreadyMigrated };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.PayrollProcessNoFaild + PayrollId;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.PayrollProcessNoFaild + PayrollId, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.PayrollProcessNoFaild + PayrollId };
            }
        }
        public GeneralMessage PostALLEmpPayrollPayVoucher(List<int> PayrollId, int UserId, int BranchId)
        {
            try
            {
                foreach (var pyrol in PayrollId)
                {
                    // var UpdatedPayroll = _payrollMarchesRepository.GetById(pyrol);
                    PayrollMarches? UpdatedPayroll = _TaamerProContext.PayrollMarches.Where(s => s.PayrollId == pyrol).FirstOrDefault();

                    if (UpdatedPayroll == null)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "فشل في اضافة صرف  بمسير رواتب";
                        _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.noPayrollThisNumber, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.noPayrollThisNumber };
                    }
                    //
                    //--------الحسابات-------
                    //
                    UpdatedPayroll.IsPostPayVoucher = true;
                    _TaamerProContext.SaveChanges();
                    //sendemployeemail(UpdatedPayroll);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " ترحيل مسيرات رواتب  " ;
                    _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.payrollPathAlreadyMigrated, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage() {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.payrollPathAlreadyMigrated };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = " فشل في ترحيل مسيرات رواتب " ;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.PayrollProcessFaild, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.PayrollProcessFaild };
            }
        }




        public GeneralMessage PostEmpPayrollPayVoucher(int PayrollId, int UserId, int BranchId)
        {
            try
            {
                // var UpdatedPayroll = _payrollMarchesRepository.GetById(PayrollId);
                PayrollMarches? UpdatedPayroll = _TaamerProContext.PayrollMarches.Where(s => s.PayrollId == PayrollId).FirstOrDefault();

                if (UpdatedPayroll == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في اضافة صرف  بمسير رواتب";
                    _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.noPayrollThisNumber, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.noPayrollThisNumber };
                }
                //
                //--------الحسابات-------
                //
                UpdatedPayroll.IsPostPayVoucher = true;
                _TaamerProContext.SaveChanges();
                //sendemployeemail(UpdatedPayroll);
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " ترحيل مسير رواتب رقم " + PayrollId;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.PayrollProcessNo + PayrollId, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.PayrollProcessNo + PayrollId };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.PayrollProcessNoFaild + PayrollId;
                _SystemAction.SaveAction("PostPayrollMarches", "PayrollMarchesService", 2, Resources.PayrollProcessNoFaild + PayrollId, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.PayrollProcessNoFaild + PayrollId };
            }
        }



        public bool sendemployeemail(PayrollMarches payroll)
        {
            bool IsSent = false;
            string OrgName = _organizationsService.GetBranchOrganization().Result.NameAr;
            var EmployeeUpdated = _TaamerProContext.Employees.Where(x => x.EmployeeId == payroll.EmpId).FirstOrDefault();
            string DepartmentNameAr = "";
            Department? DepName = _TaamerProContext.Department.Where(s => s.DepartmentId == EmployeeUpdated.DepartmentId).FirstOrDefault();
            if (DepName != null)
            {
                DepartmentNameAr = DepName.DepartmentNameAr;
            }

            //string BranchName = _BranchRepository.GetById(EmployeeUpdated.BranchId).NameAr;
            string NameAr = "";
            Branch? BranchName = _TaamerProContext.Branch.Where(s => s.BranchId == EmployeeUpdated.BranchId).FirstOrDefault();
            var job = _TaamerProContext.Job.FirstOrDefault(x => x.JobId == EmployeeUpdated.JobId);
            if (BranchName != null)
            {
                NameAr = BranchName.NameAr;
            }
            var directmanager = _TaamerProContext.Employees.Where(x => x.EmployeeId == EmployeeUpdated.DirectManager).FirstOrDefault();

            var directmanagername = directmanager == null ? "" : directmanager.EmployeeNameAr;
            var htmlBody = @"<!DOCTYPE html><html lang = ''><head><meta name='viewport' content='width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=0'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
                            <meta charset = 'utf-8><meta name = 'description' content = ''><meta name = 'keywords' content = ''><meta name = 'csrf-token' content = ''><title></title><link rel = 'icon' type = 'image/x-icon' href = ''></head>
                            <body style = 'background:#f9f9f9;direction:rtl'><div class='container' style='max-width:630px;padding-right: var(--bs-gutter-x, .75rem); padding-left: var(--bs-gutter-x, .75rem); margin-right: auto;  margin-left: auto;'>
                            <style> .bordered {width: 550px; height: 700px; padding: 20px;border: 3px solid yellowgreen; background-color:lightgray;} </style>
                            <div class= 'row' style = 'font-family: Cairo, sans-serif'>  <div class= 'card' style = 'padding: 2rem;background:#fff'> <div style = 'width: 550px; height: 700px; padding: 20px; border: 3px solid yellowgreen; background-color: lightgray;'> <p style='text-align:center'></p>
                            <h4>  اسم الموظف :  " + EmployeeUpdated.EmployeeNameAr + "</h4> <h4>  رقم الموظف :   "+EmployeeUpdated.EmployeeNo+" </h4> <h4 >   المسمي الوظيفي :   " + job .JobNameAr+ " </h4><h3 align = 'center'> تفاصيل الراتب</h3><h3 align = 'center'>" + GetArabicMonthName(payroll.MonthNo)+ "  " +DateTime.Now.Year+ "</h3><table align = 'center' border = '1' > <tr> <td colspan='2'> الايرادات</td><td colspan='2'>الخصومات</td> </tr> <tr> <td> الراتب</td> <td>" + payroll.SalaryOfThisMonth + @"</td><td> السلف  </td> <td>" + payroll.TotalLoans + @"</td></tr><tr><td>   بدل السكن  </td> <td>" + payroll.HousingAllowance + @"</td>
                            <td> خصومات  </td> <td>" + payroll.TotalDiscounts + @"</td></tr><tr><td>البدلات</td><td>"+payroll.MonthlyAllowances+"</td><td>تأمينات</td><td>"+payroll.Taamen+"</td></tr><tr><td>علاوات</td><td>"+payroll.Bonus+"</td><td>أيام غياب</td><td>"+payroll.TotalAbsDays+"</td></tr><tr><td>مكافئات</td><td>"+payroll.TotalRewards+"</td><td>إجازات مخصومة من الراتب</td><td>"+payroll.TotalVacations+ "</td></tr>" +
                            "<tr><td></td><td></td><td>التاخير</td><td>"+payroll.TotalLateDiscount+"</td></tr><tr><td></td><td></td><td>الغياب</td><td>"+payroll.TotalAbsenceDiscount+ "</td></tr><tr><td colspan='2'>الصافي</td><td colspan='2'>"+payroll.TotalSalaryOfThisMonth+"</td></tr></table><p style = 'text-align:center'> " + OrgName + @" </p> <h7> مع تحيات قسم ادارة الموارد البشرية</h7>
	
                            </div> </div></div></div></body></html> ";

            //Mail
            if (EmployeeUpdated.Email != null && EmployeeUpdated.Email != "")
            {
                IsSent = _customerMailService.SendMail_SysNotification((int)EmployeeUpdated.BranchId, 0, 0, "تفاصيل الراتب", htmlBody, true, EmployeeUpdated.Email);
            }
            //if (directmanager != null)
            //{
            //    _customerMailService.SendMail_SysNotification((int)EmployeeUpdated.BranchId, 0, 0, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), htmlBody, true, directmanager.Email);

            //}
            //string NotStr = "تم انضمام الموظف " + EmployeeUpdated.EmployeeNameAr + " إلى فريق " + OrgName + ", الوظيفة: " + job.JobNameAr + " قسم : " + DepartmentNameAr + " فرع: " + NameAr;
            //Notification UserNotification = new Notification();
            //UserNotification.ReceiveUserId = EmployeeUpdated.UserId.Value;
            //UserNotification.Name = Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar"));
            //UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
            //UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
            //UserNotification.SendUserId = 1;
            //UserNotification.Type = 1; // notification
            //UserNotification.Description = NotStr;
            //UserNotification.AllUsers = false;
            //UserNotification.SendDate = DateTime.Now;
            //UserNotification.ProjectId = 0;
            //UserNotification.TaskId = 0;
            //UserNotification.IsHidden = false;
            //UserNotification.AddUser = EmployeeUpdated.UserId.Value;
            //UserNotification.AddDate = DateTime.Now;
            //UserNotification.IsRead = false;
            //_TaamerProContext.Notification.Add(UserNotification);
            //_TaamerProContext.SaveChanges();
            //if (directmanager != null)
            //{
            //    var Not_directmanager = new Notification();
            //    Not_directmanager = UserNotification;
            //    Not_directmanager.ReceiveUserId = directmanager.UserId.Value;
            //    Not_directmanager.NotificationId = 0;
            //    _TaamerProContext.Notification.Add(Not_directmanager);
            //    _TaamerProContext.SaveChanges();
            //}
            //_notificationService.sendmobilenotification(EmployeeUpdated.UserId.Value, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), NotStr);
            //if (directmanager != null)
            //{
            //    _notificationService.sendmobilenotification(directmanager.UserId.Value, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), NotStr);
            //}

            return IsSent;
        }
        public static string GetArabicMonthName(int monthNumber)
        {
            var months = new Dictionary<int, string>
    {
        { 1, "يناير" },
        { 2, "فبراير" },
        { 3, "مارس" },
        { 4, "أبريل" },
        { 5, "مايو" },
        { 6, "يونيو" },
        { 7, "يوليو" },
        { 8, "أغسطس" },
        { 9, "سبتمبر" },
        { 10, "أكتوبر" },
        { 11, "نوفمبر" },
        { 12, "ديسمبر" }
    };

            return months.TryGetValue(monthNumber, out var name) ? name : "شهر غير صالح";
        }



    }

}