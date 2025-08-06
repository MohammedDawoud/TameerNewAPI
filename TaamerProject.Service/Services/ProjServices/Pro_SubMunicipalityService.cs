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
    public class Pro_SubMunicipalityService :  IPro_SubMunicipalityService
    {
        private readonly IPro_SubMunicipalityRepository _Pro_SubMunicipalityRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public Pro_SubMunicipalityService(IPro_SubMunicipalityRepository Pro_SubMunicipalityRepository,TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _Pro_SubMunicipalityRepository = Pro_SubMunicipalityRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<IEnumerable<Pro_SubMunicipalityVM>> GetAllSubMunicipalitys(string SearchText)
        {
            var SubMunicipalitys = _Pro_SubMunicipalityRepository.GetAllSubMunicipalitys(SearchText);
            return SubMunicipalitys;
        }
        public GeneralMessage SaveSubMunicipality(Pro_SubMunicipality SubMunicipality, int UserId, int BranchId)
        {
            try
            {

                if (SubMunicipality.SubMunicipalityId == 0)
                {


                    SubMunicipality.AddUser = UserId;
                    SubMunicipality.AddDate = DateTime.Now;
                    _TaamerProContext.Pro_SubMunicipality.Add(SubMunicipality);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة بلدية جديدة";
                    _SystemAction.SaveAction("SaveSubMunicipality", "Pro_SubMunicipalityService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
                }
                else
                {
                    // var SubMunicipalityUpdated = _Pro_SubMunicipalityRepository.GetById(SubMunicipality.SubMunicipalityId);
                    Pro_SubMunicipality? SubMunicipalityUpdated = _TaamerProContext.Pro_SubMunicipality.Where(s => s.SubMunicipalityId == SubMunicipality.SubMunicipalityId).FirstOrDefault();

                    if (SubMunicipalityUpdated != null)
                    {

                        SubMunicipalityUpdated.NameAr = SubMunicipality.NameAr;
                        SubMunicipalityUpdated.NameEn = SubMunicipality.NameEn;
                        SubMunicipalityUpdated.UpdateUser = UserId;
                        SubMunicipalityUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل بلدية رقم " + SubMunicipality.SubMunicipalityId;
                    _SystemAction.SaveAction("SaveSubMunicipality", "Pro_SubMunicipalityService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ البلدية";
                _SystemAction.SaveAction("SaveSubMunicipality", "Pro_SubMunicipalityService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteSubMunicipality(int SubMunicipalityId, int UserId, int BranchId)
        {
            try
            {

                // Pro_SubMunicipality SubMunicipality = _Pro_SubMunicipalityRepository.GetById(SubMunicipalityId);
                Pro_SubMunicipality? SubMunicipality = _TaamerProContext.Pro_SubMunicipality.Where(s => s.SubMunicipalityId ==  SubMunicipalityId).FirstOrDefault();
                if (SubMunicipality != null)
                {
                    SubMunicipality.IsDeleted = true;
                    SubMunicipality.DeleteDate = DateTime.Now;
                    SubMunicipality.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف بلدية رقم " + SubMunicipalityId;
                    _SystemAction.SaveAction("DeleteSubMunicipality", "Pro_SubMunicipalityService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف بلدية رقم " + SubMunicipalityId; ;
                _SystemAction.SaveAction("DeleteSubMunicipality", "Pro_SubMunicipalityService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_DeletedFailed };
            }
        }


 
    }
}
