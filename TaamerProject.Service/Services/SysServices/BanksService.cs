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
    public class BanksService : IBanksService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IBanksRepository _BanksRepository;
        private readonly IUsersRepository _usersRepository;


        public BanksService(TaamerProjectContext dataContext, ISystemAction systemAction, IBanksRepository banksRepository, IUsersRepository usersRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _BanksRepository = banksRepository;
            this._usersRepository = usersRepository;
        }
        public async Task<IEnumerable<BanksVM>> GetAllBanks(string SearchText)
        {
            var Banks = await _BanksRepository.GetAllBanks(SearchText);
            return Banks;
        }

        public async Task<BanksVM> GetBankByID(int BankId)
        {
            var Banks = await _BanksRepository.GetBankByID(BankId);
            return Banks;
        }
        public GeneralMessage SaveBanks(Banks banks, int UserId, int BranchId)
        {
            try
            {
                if (banks.BankId == 0)
                {
                    banks.AddUser = UserId;
                    banks.AddDate = DateTime.Now;
                    _TaamerProContext.Banks.Add(banks);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة بنك جديد";
                   _SystemAction.SaveAction("SaveBanks", "BanksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var BanksUpdated = _TaamerProContext.Banks.Where(x=>x.BankId==banks.BankId).FirstOrDefault();
                    if (BanksUpdated != null)
                    {
                        BanksUpdated.Code = banks.Code;
                        BanksUpdated.NameAr = banks.NameAr;
                        BanksUpdated.NameEn = banks.NameEn;
                        BanksUpdated.Notes = banks.Notes;
                        BanksUpdated.UpdateUser = UserId;
                        BanksUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل بنك رقم " + banks.BankId;
                    _SystemAction.SaveAction("SaveBanks", "BanksService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ البنك";
                _SystemAction.SaveAction("SaveBanks", "BanksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------


                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteBanks(int BankId, int UserId, int BranchId)
        {
            try
            {
                Banks banks = _TaamerProContext.Banks.Where(x => x.BankId == BankId).FirstOrDefault();
                banks.IsDeleted = true;
                banks.DeleteDate = DateTime.Now;
                banks.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف بنك رقم " + BankId;
                _SystemAction.SaveAction("DeleteBanks", "BanksService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف بنك رقم " + BankId; ;
                _SystemAction.SaveAction("DeleteBanks", "BanksService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillBankSelect(string SearchText = "")
        {
            return _BanksRepository.GetAllBanks(SearchText).Result.Select(s => new
            {
                Id = s.BankId,
                Name = s.NameAr
            });
        }
    }
}
