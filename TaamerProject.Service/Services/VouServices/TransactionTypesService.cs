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
using Microsoft.EntityFrameworkCore;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class TransactionTypesService :   ITransactionTypesService
    {
        private readonly ITransactionTypesRepository _TransactionTypesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public TransactionTypesService(TaamerProjectContext dataContext, ITransactionTypesRepository transactionTypesRepository,
            ISystemAction systemAction)
        {
            _TransactionTypesRepository = transactionTypesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<TransactionTypesVM>> GetAllTransactionTypes(string SearchText)
        {
            var transactionTypes = _TransactionTypesRepository.GetAllTransactionTypes(SearchText);
            return transactionTypes;
        }
        public  GeneralMessage SaveTransactionTypes(TransactionTypes transactionTypes, int UserId, int BranchId)
        {
            try
            {

                if (transactionTypes.TransactionTypeId == 0)
                {
                    transactionTypes.AddUser = UserId;
                    transactionTypes.AddDate = DateTime.Now;
                    _TaamerProContext.TransactionTypes.Add(transactionTypes);
                     _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع معاملة جديد";
                    _SystemAction.SaveAction("SaveTransactionTypes", "TransactionTypesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //  var TransactionTypesUpdated = _TransactionTypesRepository.GetById(transactionTypes.TransactionTypeId);
                    TransactionTypes? TransactionTypesUpdated =  _TaamerProContext.TransactionTypes.Where(s => s.TransactionTypeId == transactionTypes.TransactionTypeId).FirstOrDefault();
                    if (TransactionTypesUpdated != null)
                    {
                        TransactionTypesUpdated.NameAr = transactionTypes.NameAr;
                        TransactionTypesUpdated.NameEn = transactionTypes.NameEn;
                        TransactionTypesUpdated.UpdateUser = UserId;
                        TransactionTypesUpdated.UpdateDate = DateTime.Now;
                    }
                     _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع معاملة رقم " + transactionTypes.TransactionTypeId;
                    _SystemAction.SaveAction("SaveTransactionTypes", "TransactionTypesService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع المعاملة";
                _SystemAction.SaveAction("SaveTransactionTypes", "TransactionTypesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public  GeneralMessage DeleteTransactionTypes(int TransactionTypesId, int UserId, int BranchId)
        {
            try
            {
                //TransactionTypes transactionTypes = _TransactionTypesRepository.GetById(TransactionTypesId);
                TransactionTypes? transactionTypes =  _TaamerProContext.TransactionTypes.Where(s => s.TransactionTypeId == TransactionTypesId).FirstOrDefault();
                if (transactionTypes != null)
                {
                    transactionTypes.IsDeleted = true;
                    transactionTypes.DeleteDate = DateTime.Now;
                    transactionTypes.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف نوع المعاملة رقم " + TransactionTypesId;
                    _SystemAction.SaveAction("DeleteTransactionTypes", "TransactionTypesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            { 
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف نوع المعاملة رقم " + TransactionTypesId; ;
                _SystemAction.SaveAction("DeleteTransactionTypes", "TransactionTypesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        

        
    }
}
