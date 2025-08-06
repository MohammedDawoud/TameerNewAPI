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
    public class Pro_MunicipalService :   IPro_MunicipalService
    {
        private readonly IPro_MunicipalRepository _Pro_MunicipalRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public Pro_MunicipalService(IPro_MunicipalRepository Pro_MunicipalRepository,TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _Pro_MunicipalRepository = Pro_MunicipalRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<IEnumerable<Pro_MunicipalVM>> GetAllMunicipals(string SearchText)
        {
            var Clauses = _Pro_MunicipalRepository.GetAllMunicipals(SearchText);
            return Clauses;
        }
        public GeneralMessage SaveMunicipal(Pro_Municipal Municipal, int UserId, int BranchId)
        {
            try
            {

                if (Municipal.MunicipalId == 0)
                {


                    Municipal.AddUser = UserId;
                    Municipal.AddDate = DateTime.Now;
                    _TaamerProContext.Pro_Municipal.Add(Municipal);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة أمانة جديدة";
                    _SystemAction.SaveAction("SaveMunicipal", "Pro_MunicipalService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
                }
                else
                {
                    // var MunicipalUpdated = _Pro_MunicipalRepository.GetById(Municipal.MunicipalId);
                    Pro_Municipal? MunicipalUpdated = _TaamerProContext.Pro_Municipal.Where(s => s.MunicipalId == Municipal.MunicipalId).FirstOrDefault();

                    if (MunicipalUpdated != null)
                    {

                        MunicipalUpdated.NameAr = Municipal.NameAr;
                        MunicipalUpdated.NameEn = Municipal.NameEn;
                        MunicipalUpdated.UpdateUser = UserId;
                        MunicipalUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل أمانة رقم " + Municipal.MunicipalId;
                    _SystemAction.SaveAction("SaveMunicipal", "Pro_MunicipalService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الأمانة";
                _SystemAction.SaveAction("SaveMunicipal", "Pro_MunicipalService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteMunicipal(int MunicipalId, int UserId, int BranchId)
        {
            try
            {

                // Pro_Municipal Municipal = _Pro_MunicipalRepository.GetById(MunicipalId);
                Pro_Municipal? Municipal = _TaamerProContext.Pro_Municipal.Where(s => s.MunicipalId == MunicipalId ).FirstOrDefault();
                if (Municipal != null)
                {
                    Municipal.IsDeleted = true;
                    Municipal.DeleteDate = DateTime.Now;
                    Municipal.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف أمانة رقم " + MunicipalId;
                    _SystemAction.SaveAction("DeleteMunicipal", "Pro_MunicipalService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف أمانة رقم " + MunicipalId; ;
                _SystemAction.SaveAction("DeleteMunicipal", "Pro_MunicipalService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_DeletedFailed };
            }
        }



    
    }
}
