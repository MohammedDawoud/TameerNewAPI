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
    public class ProjectTypeService :   IProjectTypeService
    {
        private readonly IProjectTypeRepository _ProjectTypeRepository;
         private readonly IProSettingDetailsRepository _ProSettingDetailsRepository;
        private readonly IRequirementsandGoalsRepository _requirementsandGoalsRepository;
        private readonly IProjectRequirementsGoalsRepository _projectRequirementsGoalsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public ProjectTypeService(IProjectTypeRepository ProjectTypeRepository, IProSettingDetailsRepository ProSettingDetailsRepository
            , IRequirementsandGoalsRepository requirementsandGoalsRepository, IProjectRequirementsGoalsRepository projectRequirementsGoalsRepository
            , TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ProjectTypeRepository = ProjectTypeRepository;
             _ProSettingDetailsRepository = ProSettingDetailsRepository;
            _requirementsandGoalsRepository = requirementsandGoalsRepository;
            _projectRequirementsGoalsRepository = projectRequirementsGoalsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public IEnumerable<ProjectTypeVM> GetAllProjectType(string SearchText)
        {
            var ProjectTypes = _ProjectTypeRepository.GetAllProjectType(SearchText).Result;
            return ProjectTypes;
        }
        public GeneralMessage SaveProjectType(ProjectType projectType, int UserId, int BranchId)
        {
            try
            {
                //var codeExist = _ProjectTypeRepository.GetMatching(s => s.IsDeleted == false && s.TypeId != projectType.TypeId).FirstOrDefault();
                //if (codeExist != null)
                //{
                //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.TheCodeAlreadyExists };
                //}
                if (projectType.TypeId == 0)
                {
                    if (projectType.Typeum == 0)
                    {
                        projectType.Typeum = 1;

                    }
                  
                        if (projectType.TypeId == 0)
                        {
                            projectType.Typeum = 1;

                        }
                        projectType.AddUser = UserId;
                    projectType.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectType.Add(projectType);
                    if (projectType.RequirementsandGoals != null)
                    {

                        foreach (var item in projectType.RequirementsandGoals.ToList())
                        {
                            item.AddDate = DateTime.Now;
                            item.AddUser = UserId;

                            item.ProjectTypeId = projectType.TypeId;
                            _TaamerProContext.RequirementsandGoals.Add(item);
                        }

                    }
                    _TaamerProContext.SaveChanges();
                    projectType.TypeCode = projectType.TypeId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع مشروع ";
                    _SystemAction.SaveAction("SaveProjectType", "ProjectTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var projectTypeUpdated = _ProjectTypeRepository.GetById(projectType.TypeId);
                    ProjectType? projectTypeUpdated = _TaamerProContext.ProjectType.Where(s => s.TypeId == projectType.TypeId).FirstOrDefault();

                    if (projectTypeUpdated != null)
                    {
                        projectTypeUpdated.NameAr = projectType.NameAr;
                        projectTypeUpdated.NameEn = projectType.NameEn;
                        projectTypeUpdated.Typeum = projectType.Typeum;
                        projectTypeUpdated.UpdateUser = UserId;
                        projectTypeUpdated.UpdateDate = DateTime.Now;
                    }


                    //delete existing requirments 
                    //_requirementsandGoalsRepository.RemoveRange(projectTypeUpdated.RequirementsandGoals.ToList());

                    if (projectType.RequirementsandGoals != null)
                    {
                        foreach (var item in projectType.RequirementsandGoals.ToList())
                        {
                            //var projectgoalUpdated = _requirementsandGoalsRepository.GetById(item.RequirementId);
                            RequirementsandGoals? projectgoalUpdated = _TaamerProContext.RequirementsandGoals.Where(s => s.RequirementId == item.RequirementId).FirstOrDefault();

                            if (projectgoalUpdated == null)
                            {
                                item.AddDate = DateTime.Now;
                                item.AddUser = UserId;

                                item.ProjectTypeId = projectType.TypeId;
                                _TaamerProContext.RequirementsandGoals.Add(item);
                            }
                            else
                            {

                                projectgoalUpdated.UpdateDate = DateTime.Now;
                                projectgoalUpdated.UpdateUser = UserId;
                                projectgoalUpdated.ProjectTypeId = projectType.TypeId;
                                projectgoalUpdated.LineNumber = item.LineNumber;
                                projectgoalUpdated.TimeNo = item.TimeNo;
                                projectgoalUpdated.TimeType = item.TimeType;
                                projectgoalUpdated.EmployeeId = item.EmployeeId;
                                projectgoalUpdated.DepartmentId = item.DepartmentId;
                                projectgoalUpdated.RequirmentName = item.RequirmentName;
                            }

                        }
                    }

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل نوع مشروع رقم " + projectType.TypeId;
                    _SystemAction.SaveAction("SaveProjectType", "ProjectTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع مشروع";
                _SystemAction.SaveAction("SaveProjectType", "ProjectTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteProjectType(int projectTypeId,  int UserId, int BranchId)
        {
            try
            {

                //var Sett = _ProSettingDetailsRepository.GetMatching(s => s.IsDeleted == false && s.ProjectTypeId == projectTypeId);
                var Sett = _TaamerProContext.ProSettingDetails.Where(s => s.IsDeleted == false && s.ProjectTypeId == projectTypeId);
                if (Sett.Count() > 0)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.project_has_unpaid_bills };
                }

                //ProjectType projectType = _ProjectTypeRepository.GetById(projectTypeId);
                ProjectType? projectType = _TaamerProContext.ProjectType.Where(s => s.TypeId == projectTypeId).FirstOrDefault();
                if(projectType!=null)
                {

                    if (projectType.TypeCode == 1 || projectType.TypeCode == 2 || projectType.TypeCode == 3 || projectType.TypeCode == 4 || projectType.TypeCode == 5 || projectType.TypeCode == 6 || projectType.TypeCode == 7)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = Resources.General_SavedFailed;
                        _SystemAction.SaveAction("SaveProjectType", "ProjectTypeService", 1, Resources.project_type_main_project_types, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.project_type_main_project_types };
                    }
                    else
                    {
                        projectType.IsDeleted = true;
                        projectType.DeleteDate = DateTime.Now;
                        projectType.DeleteUser = UserId;
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " حذف نوع مشروع رقم " + projectTypeId;
                        _SystemAction.SaveAction("DeleteProjectType", "ProjectTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
                    }
                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
                }


            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف التعليق رقم " + projectTypeId; ;
                _SystemAction.SaveAction("DeleteProjectType", "ProjectTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        

    }
}
