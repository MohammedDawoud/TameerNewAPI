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
    public  class BuildTypeService : IBuildTypesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IBuildTypesRepository _buildTypesRepository;
        private readonly IUsersRepository _usersRepository;


        public BuildTypeService(TaamerProjectContext dataContext, ISystemAction systemAction, IBuildTypesRepository buildTypesRepository, IUsersRepository usersRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            this._usersRepository = usersRepository;
            _buildTypesRepository = buildTypesRepository;
        }
        public async Task<IEnumerable<BuildTypesVM>> GetAllBuildTypes(string SearchText)
        {
            var risks =await _buildTypesRepository.GetAllBuildTypes(SearchText);
            return risks;
        }
        public GeneralMessage SaveBuildType(BuildTypes buildTypes, int UserId, int BranchId)
        {
            try
            {
                if (buildTypes.BuildTypeId == 0)
                {
                    buildTypes.AddUser = UserId;
                    buildTypes.AddDate = DateTime.Now;
                    _TaamerProContext.BuildTypes.Add(buildTypes);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع مبني جديد" + buildTypes.NameAr;
                   _SystemAction.SaveAction("SaveBuildType", "BuildTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var RiskUpdated = _TaamerProContext.BuildTypes.Where(x=>x.BuildTypeId==buildTypes.BuildTypeId).FirstOrDefault();
                    if (RiskUpdated != null)
                    {
                        RiskUpdated.NameAr = buildTypes.NameAr;
                        RiskUpdated.NameEn = buildTypes.NameEn;
                        RiskUpdated.Description = buildTypes.Description;
                        RiskUpdated.UpdateUser = UserId;
                        RiskUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع مبني رقم " + buildTypes.NameAr;
                    _SystemAction.SaveAction("SaveBuildType", "BuildTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع المبني" + buildTypes.NameAr;
                _SystemAction.SaveAction("SaveBuildType", "BuildTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteBuildTypes(int BuildTypeId, int UserId, int BranchId)
        {
            try
            {
                BuildTypes buildTypes = _TaamerProContext.BuildTypes.Where(x => x.BuildTypeId == BuildTypeId).FirstOrDefault();
                buildTypes.IsDeleted = true;
                buildTypes.DeleteDate = DateTime.Now;
                buildTypes.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف نوع مبني " + buildTypes.NameAr;
                _SystemAction.SaveAction("DeleteBuildTypes", "BuildTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف نوع مبني رقم " + BuildTypeId; ;
                _SystemAction.SaveAction("DeleteBuildTypes", "BuildTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
