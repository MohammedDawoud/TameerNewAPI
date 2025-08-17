using System.Globalization;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class AllowanceTypeService : IAllowanceTypeService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAllowanceTypeRepository _AllowanceTypeRepository;
        private readonly IPayrollMarchesRepository _payrollMarchesRepository;
        private readonly IEmployeesRepository _employeesRepository;
        public AllowanceTypeService(IAllowanceTypeRepository allowanceRepository
            , TaamerProjectContext dataContext
            , ISystemAction systemAction, IPayrollMarchesRepository payrollMarches, IEmployeesRepository employeesRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _AllowanceTypeRepository = allowanceRepository;
            _payrollMarchesRepository = payrollMarches;
            _employeesRepository = employeesRepository;
        }
        public async Task<IEnumerable<AllowanceTypeVM>> GetAllAllowancesTypes(string SearchText, bool? IsSalaryPart = false)
        {
            var AllowancesTypes = await _AllowanceTypeRepository.GetAllAllowancesTypes(SearchText, IsSalaryPart);
            return AllowancesTypes;
        }
        public GeneralMessage SaveAllowanceType(AllowanceType allowanceType, int UserId, int BranchId)
        {
            try
            {
                var codeExist = _TaamerProContext.AllowanceType.Where(s => s.IsDeleted == false && s.AllowanceTypeId != allowanceType.AllowanceTypeId && s.Code == allowanceType.Code).FirstOrDefault();

                if (allowanceType.AllowanceTypeId == 0)
                {
                    allowanceType.AddUser = UserId;
                    allowanceType.AddDate = DateTime.Now;
                    allowanceType.IsSalaryPart = false;
                    allowanceType.Code = (_TaamerProContext.AllowanceType.AsQueryable().Count() + 1).ToString();
                    _TaamerProContext.AllowanceType.Add(allowanceType);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع بدل جديد";
                    _SystemAction.SaveAction("SaveAllowanceType", "AllowanceTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var AllowanceTypeUpdated = _TaamerProContext.AllowanceType.Where(x=>x.AllowanceTypeId==allowanceType.AllowanceTypeId).FirstOrDefault();
                    if (AllowanceTypeUpdated != null)
                    {
                        AllowanceTypeUpdated.Code = allowanceType.Code;
                        AllowanceTypeUpdated.NameAr = allowanceType.NameAr;
                        AllowanceTypeUpdated.NameEn = allowanceType.NameEn;
                        AllowanceTypeUpdated.Notes = allowanceType.Notes;
                        AllowanceTypeUpdated.UserId = allowanceType.UserId;
                        AllowanceTypeUpdated.UpdateUser = UserId;
                        allowanceType.IsSalaryPart = false;
                        AllowanceTypeUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل  نوع بدل رقم " + allowanceType.AllowanceTypeId;
                    _SystemAction.SaveAction("SaveAllowanceType", "AllowanceTypeService", 2,Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ  نوع البدل";
                _SystemAction.SaveAction("SaveAllowanceType", "AllowanceTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
        public GeneralMessage DeleteAllowanceType(int AllowanceTypeId, int UserId, int BranchId)
        {
            try
            {
                AllowanceType allowanceType = _TaamerProContext.AllowanceType.Where(x => x.AllowanceTypeId == AllowanceTypeId).FirstOrDefault();
                allowanceType.IsDeleted = true;
                allowanceType.DeleteDate = DateTime.Now;
                allowanceType.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف  نوع بدل رقم " + AllowanceTypeId;
                _SystemAction.SaveAction("DeleteAllowanceType", "AllowanceTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف  نوع بدل رقم " + AllowanceTypeId; ;
                _SystemAction.SaveAction("DeleteAllowanceType", "AllowanceTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };

            }
        }
        public IEnumerable<object> FillAllowanceTypeSelect(string SearchText = "")
        {
            return _AllowanceTypeRepository.GetAllAllowancesTypes(SearchText, false).Result.Select(s => new
            {
                Id = s.AllowanceTypeId,
                Name = s.NameAr
            });
        }
    }
}
