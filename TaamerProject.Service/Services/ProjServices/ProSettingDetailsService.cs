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
    public class ProSettingDetailsService :   IProSettingDetailsService
    {
        
        private readonly IProSettingDetailsRepository _ProSettingDetailsRepository;
        private readonly IProSettingDetailsNewRepository _ProSettingDetailsNewRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly ITasksDependencyRepository _TasksDependencyRepository;
        private readonly ISettingsRepository _SettingsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public ProSettingDetailsService(IProSettingDetailsRepository ProSettingDetailsRepository, IProSettingDetailsNewRepository ProSettingDetailsNewRepository,
            ISys_SystemActionsRepository Sys_SystemActionsRepository,
            ITasksDependencyRepository TasksDependencyRepository,
            ISettingsRepository SettingsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ProSettingDetailsRepository = ProSettingDetailsRepository;
            _ProSettingDetailsNewRepository = ProSettingDetailsNewRepository;

            _Sys_SystemActionsRepository = Sys_SystemActionsRepository;
            _TasksDependencyRepository = TasksDependencyRepository;
            _SettingsRepository = SettingsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;


        }
        public Task<ProSettingDetailsVM> CheckProSettingData(int ProjectTypeId, int ProjectSubTypeId, int BranchId)
        {
            var result = _ProSettingDetailsRepository.CheckProSettingData(ProjectTypeId, ProjectSubTypeId, BranchId);
            return result;
        }
        public Task<ProSettingDetailsNewVM> CheckProSettingDataNew(int ProjectTypeId, int ProjectSubTypeId, int BranchId) 
        {
            var result = _ProSettingDetailsNewRepository.CheckProSettingData(ProjectTypeId, ProjectSubTypeId, BranchId); 
            return result;
        }
        public Task<ProSettingDetailsVM> GetProjectSettingsDetailsIFExist(int ProjectId, int BranchId)
        {

            //  var ProjectIds = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == ProjectId).ToList();
            var ProjectIds = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).ToList();

            int SubTemp = 0;
            if(ProjectIds.Count()>0)
            {
                SubTemp = ProjectIds?.FirstOrDefault()?.ProjSubTypeId??0;
            }
            else
            {
                SubTemp = -1;
            }
            var result = _ProSettingDetailsRepository.CheckProSettingData2(SubTemp, BranchId);

            return result;
        }
        public Task<ProSettingDetailsNewVM> GetProjectSettingsDetailsIFExistNew(int ProjectId, int BranchId) 
        {

            //  var ProjectIds = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == ProjectId).ToList();
            var ProjectIds = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).ToList();

            int SubTemp = 0;
            if (ProjectIds.Count() > 0)
            {
                SubTemp = ProjectIds?.FirstOrDefault()?.ProjSubTypeId ?? 0;
            }
            else
            {
                SubTemp = -1;
            }
            var result = _ProSettingDetailsNewRepository.CheckProSettingData2(SubTemp, BranchId);

            return result;
        }
        public Task<ProSettingDetailsNewVM> GetProjectSettingsDetailsIFExistNew2(int ProjectId, int BranchId)
        {

            //  var ProjectIds = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == ProjectId).ToList();
            var ProjectIds = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).ToList();

            int SubTemp = 0;
            if (ProjectIds.Count() > 0)
            {
                SubTemp = ProjectIds?.FirstOrDefault()?.ProjSubTypeId ?? 0;
            }
            else
            {
                SubTemp = -1;
            }
            var result = _ProSettingDetailsNewRepository.CheckProSettingData2(SubTemp, BranchId);

            return result;
        }

        public Task<ProSettingDetailsVM> CheckProSettingData2(int? ProjectSubTypeId, int BranchId)
        {
            var result = _ProSettingDetailsRepository.CheckProSettingData2(ProjectSubTypeId, BranchId);
            return result;
        }
        public GeneralMessage SaveProSettingData(ProSettingDetails proSettingDetailsVal, int BranchId, int UserId)
        {
            try
            {
                var proSettingDetails = new ProSettingDetails();
                var CheckNo = _ProSettingDetailsRepository.CheckProSettingNo(proSettingDetailsVal).Result;
                if(CheckNo != 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ خيارات مشروع";
                    _SystemAction.SaveAction("SaveProSettingData", "ProSettingDetailsService", 1, Resources.NoFound, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.NoFound};
                }
                if (proSettingDetailsVal.ProSettingId == 0)
                {
                   // proSettingDetails.ProSettingId = NextId;
                    proSettingDetails.AddDate = DateTime.Now;
                    proSettingDetails.AddUser = UserId;
                    proSettingDetails.IsDeleted = false;
                    proSettingDetails.ProSettingNo = proSettingDetailsVal.ProSettingNo;
                    proSettingDetails.ProSettingNote = proSettingDetailsVal.ProSettingNote;
                    proSettingDetails.ProjectTypeId = proSettingDetailsVal.ProjectTypeId;
                    proSettingDetails.ProjectSubtypeId = proSettingDetailsVal.ProjectSubtypeId;
                    _TaamerProContext.ProSettingDetails.Add(proSettingDetails);


                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة خيارات مشروع جديد"+ proSettingDetailsVal.ProSettingNote;
                    _SystemAction.SaveAction("SaveProSettingData", "ProSettingDetailsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
                else
                {
                    //var proSettingDetailsUpdated = _ProSettingDetailsRepository.GetById(proSettingDetailsVal.ProSettingId);
                    ProSettingDetails? proSettingDetailsUpdated = _TaamerProContext.ProSettingDetails.Where(s => s.ProSettingId == proSettingDetailsVal.ProSettingId).FirstOrDefault();
                    if (proSettingDetailsUpdated != null)
                    {
                        proSettingDetailsUpdated.UpdateUser = UserId;
                        proSettingDetailsUpdated.UpdateDate = DateTime.Now;
                        proSettingDetailsUpdated.ProSettingNo = proSettingDetailsVal.ProSettingNo;
                        proSettingDetailsUpdated.ProSettingNote = proSettingDetailsVal.ProSettingNote;
                        proSettingDetailsUpdated.ProjectTypeId = proSettingDetailsVal.ProjectTypeId;
                        proSettingDetailsUpdated.ProjectSubtypeId = proSettingDetailsVal.ProjectSubtypeId;
                        // _ProSettingDetailsRepository.Add(proSettingDetailsVal);

                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل خيارات مشروع  " + proSettingDetailsVal.ProSettingNo;
                        _SystemAction.SaveAction("SaveProSettingData", "ProSettingDetailsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                    }

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ خيارات مشروع";
                _SystemAction.SaveAction("SaveProSettingData", "ProSettingDetailsService", 1, Resources.General_SavedFailedProjectDetail, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailedProjectDetail };
            }
        }
        public GeneralMessage SaveProSettingDataNew(ProSettingDetailsNew proSettingDetailsVal, int BranchId, int UserId)
        {
            try
            {
                var proSettingDetails = new ProSettingDetailsNew();
                var CheckNo = _ProSettingDetailsNewRepository.CheckProSettingNo(proSettingDetailsVal).Result;
                if (CheckNo != 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ خيارات مشروع";
                    _SystemAction.SaveAction("SaveProSettingData", "ProSettingDetailsService", 1, Resources.NoFound, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.NoFound };
                }
                if (proSettingDetailsVal.ProSettingId == 0)
                {
                    // proSettingDetails.ProSettingId = NextId;
                    proSettingDetails.AddDate = DateTime.Now;
                    proSettingDetails.AddUser = UserId;
                    proSettingDetails.IsDeleted = false;
                    proSettingDetails.ProSettingNo = proSettingDetailsVal.ProSettingNo;
                    proSettingDetails.ProSettingNote = proSettingDetailsVal.ProSettingNote;
                    proSettingDetails.ProjectTypeId = proSettingDetailsVal.ProjectTypeId;
                    proSettingDetails.ProjectSubtypeId = proSettingDetailsVal.ProjectSubtypeId;
                    _TaamerProContext.ProSettingDetailsNew.Add(proSettingDetails);


                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة خيارات مشروع جديد" + proSettingDetailsVal.ProSettingNote;
                    _SystemAction.SaveAction("SaveProSettingData", "ProSettingDetailsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var proSettingDetailsUpdated = _ProSettingDetailsRepository.GetById(proSettingDetailsVal.ProSettingId);
                    ProSettingDetailsNew? proSettingDetailsUpdated = _TaamerProContext.ProSettingDetailsNew.Where(s => s.ProSettingId == proSettingDetailsVal.ProSettingId).FirstOrDefault();
                    if (proSettingDetailsUpdated != null)
                    {
                        proSettingDetailsUpdated.UpdateUser = UserId;
                        proSettingDetailsUpdated.UpdateDate = DateTime.Now;
                        proSettingDetailsUpdated.ProSettingNo = proSettingDetailsVal.ProSettingNo;
                        proSettingDetailsUpdated.ProSettingNote = proSettingDetailsVal.ProSettingNote;
                        proSettingDetailsUpdated.ProjectTypeId = proSettingDetailsVal.ProjectTypeId;
                        proSettingDetailsUpdated.ProjectSubtypeId = proSettingDetailsVal.ProjectSubtypeId;
                        // _ProSettingDetailsRepository.Add(proSettingDetailsVal);

                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل خيارات مشروع  " + proSettingDetailsVal.ProSettingNo;
                        _SystemAction.SaveAction("SaveProSettingData", "ProSettingDetailsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                    }

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ خيارات مشروع";
                _SystemAction.SaveAction("SaveProSettingData", "ProSettingDetailsService", 1, Resources.General_SavedFailedProjectDetail, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailedProjectDetail };
            }
        }

        public GeneralMessage EditProSettingsDetails(ProSettingDetails proSettingDetailsVal, int BranchId, int UserId)
        {
            try
            {

                //var proSettingDetailsUpdated = _ProSettingDetailsRepository.GetById(proSettingDetailsVal.ProSettingId);
                ProSettingDetails? proSettingDetailsUpdated = _TaamerProContext.ProSettingDetails.Where(s => s.ProSettingId == proSettingDetailsVal.ProSettingId).FirstOrDefault();

                if(proSettingDetailsUpdated!=null)
                {
                    var CheckNo = _ProSettingDetailsRepository.CheckProSettingNo(proSettingDetailsVal).Result;
                    if (CheckNo == 0)
                    {
                        proSettingDetailsUpdated.ProSettingNo = proSettingDetailsVal.ProSettingNo;
                    }
                    proSettingDetailsUpdated.UpdateUser = UserId;
                    proSettingDetailsUpdated.UpdateDate = DateTime.Now;
                    proSettingDetailsUpdated.ProSettingNote = proSettingDetailsVal.ProSettingNote;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل تفاصيل خيارات مشروع رقم " + proSettingDetailsVal.ProSettingNo;
                    _SystemAction.SaveAction("EditProSettingsDetails", "ProSettingDetailsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل تفاصيل خيارات مشروع";
                _SystemAction.SaveAction("EditProSettingsDetails", "ProSettingDetailsService", 1, Resources.General_SavedFailedProjectDetail, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailedProjectDetail };
            }
        }

        public Task<IEnumerable<ProSettingDetailsVM>> FillProSettingNo()
        {
            return _ProSettingDetailsRepository.FillProSettingNo();
        }
        public Task<IEnumerable<ProSettingDetailsNewVM>> FillProSettingNoNew()
        {
            return _ProSettingDetailsNewRepository.FillProSettingNo();
        }
        public int GenerateNextProSettingNumber()
        {
            var result = _ProSettingDetailsRepository.GenerateNextProSettingNumber().Result;
            return result;
        }
        public int GenerateNextProSettingNumberNew()
        {
            var result = _ProSettingDetailsNewRepository.GenerateNextProSettingNumber().Result;
            return result;
        }
        public ProSettingDetailsVM GetProSettingById(int ProSettingId)
        {
            var result = _ProSettingDetailsRepository.GetProSettingById(ProSettingId).Result;
            return result;
        }
        public GeneralMessage DeleteProjectSettingsDetails(int SettingId, int UserId, int BranchId)
        {
            try
            {
               // ProSettingDetails settings = _ProSettingDetailsRepository.GetById(SettingId);


                ProSettingDetails? settings = _TaamerProContext.ProSettingDetails.Where(s => s.ProSettingId == SettingId).FirstOrDefault();
                if (settings != null)
                {

                    var Projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ProjectTypeId == settings.ProjectTypeId && s.SubProjectTypeId == settings.ProjectSubtypeId).ToList();
                    if(Projects.Count()>0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "لا يمكنك حذف السير لوجود مشاريع مرتبطة بنفس نوع المشروع الفرعي"; 
                        _SystemAction.SaveAction("DeleteProjectSettingsDetails", "ProSettingDetailsService", 3, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك حذف السير لوجود مشاريع مرتبطة بنفس نوع المشروع الفرعي" };
                    }



                    settings.IsDeleted = true;
                    settings.DeleteDate = DateTime.Now;
                    settings.DeleteUser = UserId;

                    try
                    {
                        var valueseet=_TaamerProContext.Settings.Where(s => s.ProjSubTypeId == settings.ProjectSubtypeId);
                        _TaamerProContext.Settings.RemoveRange(valueseet);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف تفاصيل خيارات مشروع رقم " + SettingId;
                    _SystemAction.SaveAction("DeleteProjectSettingsDetails", "ProSettingDetailsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف تفاصيل خيارات مشروع رقم " + SettingId; 
                _SystemAction.SaveAction("DeleteProjectSettingsDetails", "ProSettingDetailsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
                }
        }
        public GeneralMessage DeleteProjectSettingsDetailsNew(int SettingId, int UserId, int BranchId)
        {
            try
            {
                // ProSettingDetails settings = _ProSettingDetailsRepository.GetById(SettingId);
                ProSettingDetailsNew? settings = _TaamerProContext.ProSettingDetailsNew.Where(s => s.ProSettingId == SettingId).FirstOrDefault();
                if (settings != null)
                {

                    var Projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ProjectTypeId == settings.ProjectTypeId && s.SubProjectTypeId == settings.ProjectSubtypeId).ToList();
                    if (Projects.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "لا يمكنك حذف السير لوجود مشاريع مرتبطة بنفس نوع المشروع الفرعي";
                        _SystemAction.SaveAction("DeleteProjectSettingsDetailsNew", "ProSettingDetailsService", 3, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك حذف السير لوجود مشاريع مرتبطة بنفس نوع المشروع الفرعي" };
                    }

                    settings.IsDeleted = true;
                    settings.DeleteDate = DateTime.Now;
                    settings.DeleteUser = UserId;

                    try
                    {
                        var valueseet = _TaamerProContext.SettingsNew.Where(s => s.ProjSubTypeId == settings.ProjectSubtypeId);
                        _TaamerProContext.SettingsNew.RemoveRange(valueseet);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف تفاصيل خيارات مشروع رقم " + SettingId;
                    _SystemAction.SaveAction("DeleteProjectSettingsDetailsNew", "ProSettingDetailsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف تفاصيل خيارات مشروع رقم " + SettingId; ;
                _SystemAction.SaveAction("DeleteProjectSettingsDetailsNew", "ProSettingDetailsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }




    }
}
