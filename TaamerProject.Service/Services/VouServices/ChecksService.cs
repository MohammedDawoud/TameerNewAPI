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

namespace TaamerProject.Service.Services
{
    public class ChecksService : IChecksService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IChecksRepository _ChecksRepository;



        public ChecksService(TaamerProjectContext dataContext, ISystemAction systemAction, IChecksRepository checksRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _ChecksRepository = checksRepository;
        }

        public async Task<IEnumerable<ChecksVM>> GetAllChecks(int Type, int BranchId)
        {
            var checks =await _ChecksRepository.GetAllChecks(Type, BranchId);
            return checks;
        }
        public GeneralMessage SaveCheck(Checks check, int UserId, int BranchId)
        {
            try
            {
                var codeExist = _TaamerProContext.Checks.Where(s => s.IsDeleted == false && s.CheckId != check.CheckId && s.CheckNumber == check.CheckNumber && s.Type == check.Type).FirstOrDefault();
                if (codeExist != null)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.CheckNumberAlreadyExists };
                }
                if (check.CheckId == 0)
                {
                    check.AddDate = DateTime.Now;
                    check.BranchId = BranchId;
                    check.AddUser = UserId;
                    _TaamerProContext.Checks.Add(check);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة شيك جديد";
                   _SystemAction.SaveAction("SaveCheck", "ChecksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var CheckUpdated = _TaamerProContext.Checks.Where(x => x.CheckId == check.CheckId).FirstOrDefault();
                    if (CheckUpdated != null)
                    {
                        CheckUpdated.CheckNumber = check.CheckNumber;
                        CheckUpdated.Date = check.Date;
                        CheckUpdated.HijriDate = check.HijriDate;
                        CheckUpdated.IsFinished = check.IsFinished;
                        CheckUpdated.ReceivedName = check.ReceivedName;
                        CheckUpdated.Amount = check.Amount;
                        CheckUpdated.BankId = check.BankId;
                        CheckUpdated.Notes = check.Notes;
                        CheckUpdated.BeneficiaryName = check.BeneficiaryName;
                        CheckUpdated.UpdateUser = UserId;
                        CheckUpdated.UpdateUser = BranchId;
                        CheckUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل شيك رقم " + check.CheckId;
                    _SystemAction.SaveAction("SaveCheck", "ChecksService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الشيك";
                _SystemAction.SaveAction("SaveCheck", "ChecksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteCheck(int CheckId, int UserId, int BranchId)
        {
            try
            {
                Checks city = _TaamerProContext.Checks.Where(x=>x.CheckId==CheckId).FirstOrDefault();
                city.IsDeleted = true;
                city.DeleteDate = DateTime.Now;
                city.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف شيك رقم " + CheckId;
                _SystemAction.SaveAction("DeleteCheck", "ChecksService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف شيك رقم " + CheckId; ;
                _SystemAction.SaveAction("DeleteCheck", "ChecksService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public async Task<IEnumerable<ChecksVM>> GetAllCheckSearch(ChecksVM checkSearch, int BranchId)
        {
            var checks = await _ChecksRepository.GetAllCheckSearch(checkSearch, BranchId);
            return checks;
        }


    }
}
