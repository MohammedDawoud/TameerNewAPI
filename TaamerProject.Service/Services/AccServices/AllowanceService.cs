using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Repository.Repositories;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class AllowanceService : IAllowanceService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAllowanceRepository _AllowanceRepository;
        private readonly IPayrollMarchesRepository _payrollMarchesRepository;
        private readonly IEmployeesRepository _employeesRepository;
        public AllowanceService(IAllowanceRepository allowanceRepository
            , TaamerProjectContext dataContext
            , ISystemAction systemAction, IPayrollMarchesRepository payrollMarches, IEmployeesRepository employeesRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _AllowanceRepository = allowanceRepository;
            _payrollMarchesRepository = payrollMarches;
            _employeesRepository = employeesRepository;
        }

        public async Task<IEnumerable<AllowanceVM>> GetAllAllowances(int? EmpId, string SearchText, bool? IsSalaryPart = null)
        {
            var Allowances =await _AllowanceRepository.GetAllAllowances(EmpId, SearchText, IsSalaryPart);
            return Allowances;
        }
        public Task<IEnumerable<AllowanceVM>> GetAllAllowancesSearch(AllowanceVM AllowanceSearch)
        {
            if (AllowanceSearch.IsSearch??false)
            {
                return _AllowanceRepository.GetAllAllowancesBySearchObject(AllowanceSearch);
            }
            else
            {
                return _AllowanceRepository.GetAllAllowancesSearch();
            }
        }

        public GeneralMessage SaveAllowance(Allowance allowance, int UserId, int BranchId, string Lang)
        {
            try
            {
                var Payroll = _payrollMarchesRepository.GetPayrollMarches(allowance.EmployeeId.Value, DateTime.Now.Month).Result;
                if (Payroll != null && Payroll.PostDate.HasValue &&
                    (allowance.StartDate.HasValue && allowance.StartDate <= DateTime.Now) &&
                    (!allowance.EndDate.HasValue || ((allowance.EndDate.HasValue && allowance.EndDate >= DateTime.Now))))
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في تعديل مسير الرواتب (البدلات)) ";
                   _SystemAction. SaveAction("UpdatePayrollWithAllowances", "AllowanceService", 2, Resources.Posted_payroll_cannot_be_modified, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Posted_payroll_cannot_be_modified };

                }

                if (allowance.AllowanceId == 0)
                {
                    allowance.AddUser = UserId;
                    allowance.AddDate = DateTime.Now;

                    //if (!allowance.IsFixed)
                    //{
                    //    var date = DateTime.ParseExact(allowance.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    //    allowance.Month = date.Month;
                    //}
                    _TaamerProContext.Allowance.Add(allowance);
                    _TaamerProContext.SaveChanges();

                    // UpdatePayrollWithAllowances(allowance.EmployeeId.Value, UserId, BranchId, Lang);

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة بدل جديد";
                    _SystemAction.SaveAction("SaveAllowance", "AllowanceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var AllowanceUpdated = _TaamerProContext.Allowance.Where(s=>s.AllowanceId==allowance.AllowanceId).FirstOrDefault();
                    if (AllowanceUpdated != null)
                    {
                        AllowanceUpdated.EmployeeId = allowance.EmployeeId;
                        AllowanceUpdated.AllowanceTypeId = allowance.AllowanceTypeId;
                        AllowanceUpdated.Date = allowance.Date;
                        //AllowanceUpdated.StartDate = allowance.StartDate;
                        //AllowanceUpdated.EndDate = allowance.EndDate;
                        AllowanceUpdated.AllowanceMonthNo = allowance.AllowanceMonthNo;
                        //if (!allowance.IsFixed)
                        //{
                        //    var date = DateTime.ParseExact(allowance.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        //    AllowanceUpdated.Month = date.Month;
                        //}
                        //AllowanceUpdated.IsFixed = allowance.IsFixed;
                        AllowanceUpdated.AllowanceAmount = allowance.AllowanceAmount;
                        AllowanceUpdated.UserId = allowance.UserId;
                        AllowanceUpdated.UpdateUser = UserId;
                        AllowanceUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    if(AllowanceUpdated.AllowanceTypeId != null)
                    {
                        var allotype=_TaamerProContext.AllowanceType.Where(x=>x.AllowanceTypeId== AllowanceUpdated.AllowanceTypeId).FirstOrDefault();
                        if (!allotype.IsSalaryPart.Value)
                            UpdatePayrollWithAllowances(allowance.EmployeeId.Value, UserId, BranchId, Lang);
                    }
                    

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تعديل بدل رقم" + allowance.AllowanceId;
                    _SystemAction.SaveAction("SaveAllowance", "AllowanceService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ البدل";
                _SystemAction.SaveAction("SaveAllowance", "AllowanceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }

        private GeneralMessage UpdatePayrollWithAllowances(int EmpId, int UserId, int BranchId, string Lang)
        {
            var EmpVM = _employeesRepository.GetEmployeeById(EmpId, Lang).Result;
            var Payroll = _payrollMarchesRepository.GetPayrollMarches(EmpId, DateTime.Now.Month).Result;

            if (Payroll != null && Payroll.PostDate.HasValue)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل مسير الرواتب (البدلات)) ";
               _SystemAction.SaveAction("UpdatePayrollWithAllowances", "AllowanceService", 2, Resources.Posted_payroll_cannot_be_modified, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Posted_payroll_cannot_be_modified };

            }

            if (EmpVM != null && Payroll != null)
            {
                Payroll.MonthlyAllowances = EmpVM.MonthlyAllowances;
                Payroll.ExtraAllowances = EmpVM.ExtraAllowances;

                if (int.Parse(EmpVM.WorkStartDate.Split('-')[0]) == DateTime.Now.Year && int.Parse(EmpVM.WorkStartDate.Split('-')[1]) == DateTime.Now.Month)
                {
                    int DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    int RestDays = DaysInMonth - int.Parse(EmpVM.WorkStartDate.Split('-')[2]);

                    decimal SalaryWithAllows = EmpVM.Salary.Value +
                        //EmpVM.CommunicationAllawance.Value + EmpVM.TransportationAllawance.Value + EmpVM.ProfessionAllawance.Value +
                        EmpVM.HousingAllowance.Value
                    + EmpVM.MonthlyAllowances.Value + EmpVM.ExtraAllowances.Value;

                    EmpVM.ThisMonthSalary = Math.Round((decimal)((SalaryWithAllows / 30) * RestDays), 2);
                }
                else
                    EmpVM.ThisMonthSalary = EmpVM.Salary;

                Payroll.UpdateDate = DateTime.Now;
                Payroll.UpdateUser = UserId;

            }
            _TaamerProContext.SaveChanges();
            //-----------------------------------------------------------------------------------------------------------------
            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            string ActionNote2 = "تعديل مسير الرواتب (البدلات) ";
            _SystemAction.SaveAction("UpdatePayrollWithAllowances", "AllowanceService", 2,"saved", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
            //-----------------------------------------------------------------------------------------------------------------

            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

        }

        public GeneralMessage SaveSalaryParts(EmpSalaryPartsVM salaryParts, int UserId, int BranchId, string Lang)
        {
            try
            {
                var payroll = _payrollMarchesRepository.GetPayrollMarches(salaryParts.HousingAllowance.EmployeeId.Value, DateTime.Now.Month).Result;
                if (payroll != null)
                {
                    if (payroll.PostDate.HasValue &&
                        (
                            //payroll.CommunicationAllawance != salaryParts.Communication.AllowanceAmount ||
                            //payroll.ProfessionAllawance != salaryParts.Profession.AllowanceAmount ||
                            //payroll.TransportationAllawance != salaryParts.Transportation.AllowanceAmount ||
                            payroll.HousingAllowance != salaryParts.HousingAllowance.AllowanceAmount
                        )
                       )
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ بدلات الراتب";
                        _SystemAction.SaveAction("SaveSalaryParts", "AllowanceService", 2, Resources.Posted_payroll_cannot_be_modified, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Posted_payroll_cannot_be_modified };
                }

                    }
                //var result1 = SaveAllowance(salaryParts.Communication, UserId, BranchId, Lang);
                //var result2 = SaveAllowance(salaryParts.Profession, UserId, BranchId, Lang);
                //var result3 = SaveAllowance(salaryParts.Transportation, UserId, BranchId, Lang);
                var result4 = SaveAllowance(salaryParts.HousingAllowance, UserId, BranchId, Lang);

                //if (result1.Result && result2.Result && result3.Result && result4.Result)
                if (result4.StatusCode==HttpStatusCode.OK)
                {

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ بدلات الراتب";
                    _SystemAction.SaveAction("SaveSalaryParts", "AllowanceService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    result4.ReasonPhrase = Resources.General_SavedSuccessfully;

                    ///---------Payroll Marches----------

                    //payroll.CommunicationAllawance = salaryParts.Communication.AllowanceAmount;
                    //payroll.ProfessionAllawance = salaryParts.Profession.AllowanceAmount;
                    //payroll.TransportationAllawance = salaryParts.Transportation.AllowanceAmount;
                    //payroll.HousingAllowance = salaryParts.HousingAllowance.AllowanceAmount;


                    //var Emp = _EmployeesRepository.GetEmployeeById(salaryParts.Communication.EmployeeId.Value, Lang);
                    //if (Emp != null)
                    //{
                    //    if (int.Parse(Emp.WorkStartDate.Split('-')[0]) == DateTime.Now.Year && int.Parse(Emp.WorkStartDate.Split('-')[1]) == DateTime.Now.Month)
                    //    {
                    //        int DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    //        int RestDays = DaysInMonth - int.Parse(Emp.WorkStartDate.Split('-')[2]);

                    //        decimal SalaryWithAllows = Emp.Salary.Value + Emp.CommunicationAllawance.Value + Emp.TransportationAllawance.Value + Emp.ProfessionAllawance.Value + Emp.HousingAllowance.Value
                    //        + Emp.MonthlyAllowances.Value + Emp.ExtraAllowances.Value;

                    //        Emp.ThisMonthSalary = Math.Round((decimal)((SalaryWithAllows / 30) * RestDays), 2);
                    //    }
                    //    else
                    //        Emp.ThisMonthSalary = Emp.Salary;
                    //}

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "(تعديل مسير الرواتب (بدلات الراتب";
                    _SystemAction.SaveAction("SaveSalaryParts", "AllowanceService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------


                }
                //else if (result1.Result && result2.Result && result3.Result && result4.Result)
                else if (result4.StatusCode == HttpStatusCode.BadRequest)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ بدلات الراتب";
                    _SystemAction.SaveAction("SaveSalaryParts", "AllowanceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    result4.ReasonPhrase = Resources.General_SavedFailed;
                }
                return result4;
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل ف حفظ بدالات الراتب";
                _SystemAction.SaveAction("SaveSalaryParts", "AllowanceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }

        public GeneralMessage DeleteAllowance(int AllowanceId, int UserId, int BranchId)
        {
            try
            {
                Allowance allowance = _TaamerProContext.Allowance.Where(X=>X.AllowanceId==AllowanceId).FirstOrDefault();

                var Payroll = _payrollMarchesRepository.GetPayrollMarches(allowance.EmployeeId.Value, DateTime.Now.Month).Result;
                if (Payroll != null && Payroll.PostDate.HasValue &&
                    (allowance.StartDate.HasValue && allowance.StartDate <= DateTime.Now) &&
                    (!allowance.EndDate.HasValue || ((allowance.EndDate.HasValue && allowance.EndDate >= DateTime.Now))))
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في تعديل مسير الرواتب (البدلات)) ";
                   _SystemAction.SaveAction("DeleteAllowance", "AllowanceService", 2, Resources.Posted_payroll_cannot_be_modified, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Posted_payroll_cannot_be_modified };

                }

                allowance.IsDeleted = true;
                allowance.DeleteDate = DateTime.Now;
                allowance.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف بدل رقم " + AllowanceId;
                _SystemAction.SaveAction("DeleteAllowance", "AllowanceService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف بدل رقم " + AllowanceId; ;
                _SystemAction.SaveAction("DeleteAllowance", "AllowanceService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = Resources.General_DeletedFailed };

            }
        }

        public async Task<decimal> GetAllownacesSumForPayroll(int EmpId, DateTime StartDate, DateTime EndDate)
        {
            return await _AllowanceRepository.GetAllownacesSumForPayroll(EmpId, StartDate, EndDate);
        }
    }
}
