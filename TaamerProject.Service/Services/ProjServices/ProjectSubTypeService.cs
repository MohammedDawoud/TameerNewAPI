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
    public class ProjectSubTypeService :   IProjectSubTypesService
    {
        private readonly IProjectSubTypeRepository _projectSubTypesRepository;
         private readonly IProSettingDetailsRepository _ProSettingDetailsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ProjectSubTypeService(IProjectSubTypeRepository projectSubTypesRepository, IProSettingDetailsRepository ProSettingDetailsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _projectSubTypesRepository = projectSubTypesRepository;
             _ProSettingDetailsRepository = ProSettingDetailsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<ProjectSubTypeVM>> GetAllProjectSubType(int BranchId)
        {
            var projectSubTypes = _projectSubTypesRepository.GetAllProjectSubType(BranchId);
            return projectSubTypes;
        }
        public GeneralMessage SaveProjectSubTypes(ProjectSubTypes subTypes, int UserId, int BranchId)
        {
            try
            {
                if (subTypes.SubTypeId == 0)
                {
                    subTypes.BranchId = BranchId;
                    subTypes.AddUser = UserId;
                    subTypes.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectSubTypes.Add(subTypes);
                    _TaamerProContext.SaveChanges();

                    var TaskName = "";

                    for (int i = 0; i < 4; i++)
                    {
                        if (i == 0) TaskName = "بدأ المشروع(1)initiation";
                        else if (i == 1) TaskName = "تخطيط المشروع(2)planning";
                        else if (i == 2) TaskName = "تنفيذ المشروع(3)Executing";
                        else if (i == 3) TaskName = "إغلاق المشروع(4)Closing";
                        else TaskName = "";
                        _TaamerProContext.Settings.Add(new Settings
                        {
                            DescriptionAr = TaskName,
                            DescriptionEn = TaskName,
                            ProjSubTypeId = subTypes.SubTypeId,
                            Type = 1,
                            IsUrgent = true,//only 4 steps have IsUrgent=true and type=1
                            BranchId = BranchId,
                            IsMerig = -1,
                            AddUser = UserId,
                            AddDate = DateTime.Now,
                        });
                    }


                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع مشروع فرعي " + subTypes.NameAr;
                    _SystemAction.SaveAction("SaveProjectSubTypes", "ProjectSubTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    // var subtypeUpdated = _projectSubTypesRepository.GetById(subTypes.SubTypeId);
                    ProjectSubTypes? subtypeUpdated = _TaamerProContext.ProjectSubTypes.Where(s => s.SubTypeId == subTypes.SubTypeId).FirstOrDefault();


                    if (subtypeUpdated != null)
                    {
                        subtypeUpdated.ProjectTypeId = subTypes.ProjectTypeId;
                        subtypeUpdated.NameAr = subTypes.NameAr;
                        subtypeUpdated.NameEn = subTypes.NameEn;
                        subtypeUpdated.TimePeriod = subTypes.TimePeriod;

                        subtypeUpdated.UpdateUser = UserId;
                        subtypeUpdated.UpdateDate = DateTime.Now;
                    }
                    var settingList= _TaamerProContext.Settings.Where(s=>s.IsDeleted==false && s.ProjSubTypeId== subTypes.SubTypeId && s.IsUrgent == true).ToList();
                    if (settingList.Count()>0)
                    {

                    }
                    else
                    {
                        var TaskName = "";
                        for (int i = 0; i < 4; i++)
                        {
                            if (i == 0) TaskName = "بدأ المشروع(1)initiation";
                            else if (i == 1) TaskName = "تخطيط المشروع(2)planning";
                            else if (i == 2) TaskName = "تنفيذ المشروع(3)Executing";
                            else if (i == 3) TaskName = "إغلاق المشروع(4)Closing";
                            else TaskName = "";
                            _TaamerProContext.Settings.Add(new Settings
                            {
                                DescriptionAr = TaskName,
                                DescriptionEn = TaskName,
                                ProjSubTypeId = subTypes.SubTypeId,
                                Type = 1,
                                IsUrgent = true,//only 4 steps have IsUrgent=true and type=1
                                BranchId = BranchId,
                                IsMerig = -1,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                            });
                        }
                    }


                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل نوع مشروع فرعي  " + subTypes.NameAr;
                    _SystemAction.SaveAction("SaveProjectSubTypes", "ProjectSubTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع مشروع فرعي";
                _SystemAction.SaveAction("SaveProjectSubTypes", "ProjectSubTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteSubTypes(int SubTypeId, int UserId, int BranchId)
        {
            try
            {

                //var Sett=_ProSettingDetailsRepository.GetMatching(s => s.IsDeleted == false && s.ProjectSubtypeId == SubTypeId);
                var Sett = _TaamerProContext.ProSettingDetails.Where(s => s.IsDeleted == false && s.ProjectSubtypeId == SubTypeId);
                if (Sett.Count()>0)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.SaveFailedStoppedTasksCompletedFirst };
                }

                //ProjectSubTypes constraint = _projectSubTypesRepository.GetById(SubTypeId);
                ProjectSubTypes? constraint = _TaamerProContext.ProjectSubTypes.Where(s => s.SubTypeId ==  SubTypeId).FirstOrDefault();
                if (constraint != null)
                {
                    constraint.IsDeleted = true;
                    constraint.DeleteDate = DateTime.Now;
                    constraint.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف نوع مشروع فرعي رقم " + SubTypeId;
                    _SystemAction.SaveAction("DeleteSubTypes", "ProjectSubTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف نوع مشروع فرعي رقم " + SubTypeId; ;
                _SystemAction.SaveAction("DeleteSubTypes", "ProjectSubTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public Task<IEnumerable<ProjectSubTypeVM>> GetAllProjectSubsByProjectTypeId(int ProjectTypeId, string SearchText,int BranchId)
        {
            var ProSubs = _projectSubTypesRepository.GetAllProjectSubsByProjectTypeId(ProjectTypeId,SearchText, BranchId);
            return ProSubs;
        }
        public ProjectSubTypeVM GetTimePeriordBySubTypeId(int SubTypeId)
        {
            var ProSubs = _projectSubTypesRepository.GetTimePeriordBySubTypeId(SubTypeId).Result.FirstOrDefault();
            return ProSubs??new ProjectSubTypeVM();
        }
        

    }
}
