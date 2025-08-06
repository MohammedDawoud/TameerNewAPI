using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICurrencyRepository _CurrencyRepository;



        public CurrencyService(TaamerProjectContext dataContext, ISystemAction systemAction, ICurrencyRepository currencyRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _CurrencyRepository = currencyRepository;
        }
        public async Task<IEnumerable<CurrencyVM>> GetAllCurrency(int BranchId)
        {
            var Currencies = await _CurrencyRepository.GetAllCurrency(BranchId);
            return Currencies.ToList();
        }
        public async Task<IEnumerable<CurrencyVM>> GetAllCurrency2(string SearchText)
        {
            var Currency = await _CurrencyRepository.GetAllCurrency2(SearchText);
            return Currency;
        }
        public GeneralMessage SaveCurrency(Currency currency, int UserId, int BranchId)
        {
            try
            {
                var codeExist = _TaamerProContext.Currency.Where(s => s.IsDeleted == false && s.CurrencyId != currency.CurrencyId && s.CurrencyCode == currency.CurrencyCode).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ العملة";
                    _SystemAction.SaveAction("SaveCurrency", "CurrencyService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TheCodeAlreadyExists };
                }
                if (currency.CurrencyId == 0)
                {
                    currency.BranchId = BranchId;
                    currency.AddUser = UserId;
                    currency.AddDate = DateTime.Now;
                    _TaamerProContext.Currency.Add(currency);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة عملة جديدة";
                    _SystemAction.SaveAction("SaveCurrency", "CurrencyService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var CurrencyUpdated = _TaamerProContext.Currency.Where(x=>x.CurrencyId==currency.CurrencyId).FirstOrDefault();
                    if (CurrencyUpdated != null)
                    {
                        CurrencyUpdated.CurrencyCode = currency.CurrencyCode;
                        CurrencyUpdated.CurrencyNameAr = currency.CurrencyNameAr;
                        CurrencyUpdated.CurrencyNameEn = currency.CurrencyNameEn;
                        CurrencyUpdated.PartCount = currency.PartCount;
                        CurrencyUpdated.PartNameAr = currency.PartNameAr;
                        CurrencyUpdated.PartNameEn = currency.PartNameEn;
                        CurrencyUpdated.ExchangeRate = currency.ExchangeRate;
                        CurrencyUpdated.UpdateUser = UserId;
                        CurrencyUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل عملة رقم " + currency.CurrencyId;
                    _SystemAction.SaveAction("SaveCurrency", "CurrencyService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العملة";
                _SystemAction.SaveAction("SaveCurrency", "CurrencyService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteCurrency(int CurrencyId, int UserId, int BranchId)
        {
            try
            {
                Currency curc = _TaamerProContext.Currency.Where(x=>x.CurrencyId==CurrencyId).FirstOrDefault();
                curc.IsDeleted = true;
                curc.DeleteDate = DateTime.Now;
                curc.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف عملة رقم " + CurrencyId;
                _SystemAction.SaveAction("DeleteCurrency", "CurrencyService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف عملة رقم " + CurrencyId; ;
                _SystemAction.SaveAction("DeleteCurrency", "CurrencyService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public async Task<int> GenerateCurrencyNumber(int BranchId)
        {
            return await _CurrencyRepository.GenerateCurrencyNumber(BranchId);
        }
    }
}
