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
    public class NationalityService : INationalityService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly INationalityRepository _NationalityRepository;



        public NationalityService(TaamerProjectContext dataContext, ISystemAction systemAction,INationalityRepository nationalityRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _NationalityRepository = nationalityRepository;
        }
        public async Task< IEnumerable<NationalityVM>> GetAllNationalities(string SearchText)
        {
            var nationalities =await _NationalityRepository.GetAllNationalities(SearchText);
            return nationalities;
        }
        public GeneralMessage SaveNationality(Nationality nationality, int UserId, int BranchId)
        {
            try
            {
                //var codeExist = _TaamerProContext.Nationality.Where(s => s.IsDeleted == false && s.NationalityId != nationality.NationalityId && s.NationalityCode == nationality.NationalityCode).FirstOrDefault();
                //if (codeExist != null)
                //{
                //    //-----------------------------------------------------------------------------------------------------------------
                //    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //    string ActionNote2 = Resources.General_SavedFailed;
                //   _SystemAction.SaveAction("SaveNationality", "NationalityService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //    //-----------------------------------------------------------------------------------------------------------------
                //    return new GeneralMessage { StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = Resources.TheCodeAlreadyExists };
                //}
                if (nationality.NationalityId == 0)
                {
                    nationality.AddUser = UserId;
                    nationality.AddDate = DateTime.Now;
                    _TaamerProContext.Nationality.Add(nationality);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة جنسية جديدة";
                    _SystemAction.SaveAction("SaveNationality", "NationalityService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };


                }
                else
                {
                    var NationalityUpdated = _TaamerProContext.Nationality.Where(x=>x.NationalityId==nationality.NationalityId).FirstOrDefault();
                    if (NationalityUpdated != null)
                    {
                        NationalityUpdated.NationalityCode = nationality.NationalityCode;
                        NationalityUpdated.NameAr = nationality.NameAr;
                        NationalityUpdated.NameEn = nationality.NameEn;
                        NationalityUpdated.Notes = nationality.Notes;
                        NationalityUpdated.UpdateUser = UserId;
                        NationalityUpdated.UpdateDate = DateTime.Now;
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل الجنسية رقم " + nationality.NationalityId;
                    _SystemAction.SaveAction("SaveNationality", "NationalityService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الجنسية";
                _SystemAction.SaveAction("SaveNationality", "NationalityService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteNationality(int NationalityId, int UserId, int BranchId)
        {
            try
            {
                Nationality nationality = _TaamerProContext.Nationality.Where(x=>x.NationalityId==NationalityId).FirstOrDefault();
                nationality.IsDeleted = true;
                nationality.DeleteDate = DateTime.Now;
                nationality.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف جنسية رقم " + NationalityId;
                _SystemAction.SaveAction("DeleteNationality", "NationalityService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف جنسية رقم " + NationalityId; ;
                _SystemAction.SaveAction("DeleteNationality", "NationalityService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillNationalitySelect(string SearchText = "")
        {
            return _NationalityRepository.GetAllNationalities(SearchText).Result.Select(s => new
            {
                Id = s.NationalityId,
                Name = s.NameAr
            });
        }
    }

}
