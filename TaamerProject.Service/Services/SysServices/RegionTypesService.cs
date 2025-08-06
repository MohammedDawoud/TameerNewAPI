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
    public class RegionTypesService : IRegionTypesService
    {
        private readonly IRegionTypesRepository _RegionTypesRepository;
 
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public RegionTypesService(IRegionTypesRepository RegionTypesRepository,

            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _RegionTypesRepository = RegionTypesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<RegionTypesVM>> GetAllRegionTypes(string SearchText)
        {
            var regionTypes = _RegionTypesRepository.GetAllRegionTypes(SearchText);
            return regionTypes;
        }
        public GeneralMessage SaveRegionTypes(RegionTypes regionTypes, int UserId, int BranchId)
        {
            try
            {
               
                if (regionTypes.RegionTypeId == 0)
                {
                    regionTypes.AddUser = UserId;
                    regionTypes.AddDate = DateTime.Now;
                    _TaamerProContext.RegionTypes.Add(regionTypes);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع منطقة جديد";
                    _SystemAction.SaveAction("SaveRegionTypes", "RegionTypesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                   // var RegionTypesUpdated = _RegionTypesRepository.GetById(regionTypes.RegionTypeId);
                    RegionTypes? RegionTypesUpdated = _TaamerProContext.RegionTypes.Where(s => s.RegionTypeId == regionTypes.RegionTypeId).FirstOrDefault();

                    if (RegionTypesUpdated != null)
                    {
                        RegionTypesUpdated.NameAr = regionTypes.NameAr;
                        RegionTypesUpdated.NameEn = regionTypes.NameEn;
                        RegionTypesUpdated.UpdateUser = UserId;
                        RegionTypesUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع منطقة رقم " + regionTypes.RegionTypeId;
                    _SystemAction.SaveAction("SaveRegionTypes", "RegionTypesService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع المنطقة";
                _SystemAction.SaveAction("SaveRegionTypes", "RegionTypesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteRegionTypes(int RegionTypeId, int UserId, int BranchId)
        {
            try
            {
                // RegionTypes regionTypes = _RegionTypesRepository.GetById(RegionTypeId);
                RegionTypes? regionTypes = _TaamerProContext.RegionTypes.Where(s => s.RegionTypeId == RegionTypeId).FirstOrDefault();
                if (regionTypes != null)
                {
                    regionTypes.IsDeleted = true;
                    regionTypes.DeleteDate = DateTime.Now;
                    regionTypes.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف نوع منطقة رقم " + RegionTypeId;
                    _SystemAction.SaveAction("DeleteRegionTypes", "RegionTypesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في نوع منطقة رقم " + RegionTypeId; ;
                _SystemAction.SaveAction("DeleteRegionTypes", "RegionTypesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

      

    }
}
