using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class ProjectRequirementsRepository : IProjectRequirementsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectRequirementsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirement(int BranchId)
        {
            var projectRequirements = _TaamerProContext.ProjectRequirements.Where(s => s.IsDeleted == false  && s.PhasesTaskID == null&&s.OrderId==null).Select(x => new ProjectRequirementsVM
            {
                RequirementId = x.RequirementId,
                ProjectTypeId = x.ProjectTypeId,
                ProjectSubTypeId = x.ProjectSubTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Cost = x.Cost,
                AttachmentUrl = x.AttachmentUrl,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",

            });
            return projectRequirements;
        }
        public async Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementByTaskId(int BranchId, int PhasesTaskID)
        {
            var projectRequirements = _TaamerProContext.ProjectRequirements.Where(s => s.IsDeleted == false && s.PhasesTaskID == PhasesTaskID).Select(x => new ProjectRequirementsVM
            {
                RequirementId = x.RequirementId,
                ProjectTypeId = x.ProjectTypeId,
                ProjectSubTypeId = x.ProjectSubTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Cost = x.Cost,
                AttachmentUrl = x.AttachmentUrl,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                //UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                PhasesTaskID = x.PhasesTaskID ?? 0,

            });

            return projectRequirements;
        }
        public async Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementById(int BranchId, int RequirementId)
        {
            var projectRequirements = _TaamerProContext.ProjectRequirements.Where(s => s.IsDeleted == false && s.RequirementId == RequirementId).Select(x => new ProjectRequirementsVM
            {
                RequirementId = x.RequirementId,
                ProjectTypeId = x.ProjectTypeId,
                ProjectSubTypeId = x.ProjectSubTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Cost = x.Cost,
                AttachmentUrl = x.AttachmentUrl,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                //UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                PhasesTaskID = x.PhasesTaskID ?? 0,
                AddDate = x.AddDate.ToString(),

            });

            return projectRequirements;
        }

        public async Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementOrderId(int BranchId, int Orderid)
        {
            var projectRequirements = _TaamerProContext.ProjectRequirements.Where(s => s.IsDeleted == false && s.OrderId == Orderid).Select(x => new ProjectRequirementsVM
            {
                RequirementId = x.RequirementId,
                ProjectTypeId = x.ProjectTypeId,
                ProjectSubTypeId = x.ProjectSubTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Cost = x.Cost,
                AttachmentUrl = x.AttachmentUrl,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                OrderId=x.OrderId,
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",

            });

            return projectRequirements;
        }
        public async Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementByProjectSubTypeId(int ProjectSubTypeId , string SearchText, int BranchId)
        {
            var projectRequirements = _TaamerProContext.ProjectRequirements.Where(s => s.IsDeleted == false && s.ProjectSubTypeId== ProjectSubTypeId).Select(x => new ProjectRequirementsVM
            {
                RequirementId = x.RequirementId,
                ProjectTypeId = x.ProjectTypeId,
                ProjectSubTypeId = x.ProjectSubTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Cost = x.Cost != null ? x.Cost : 0,
                AttachmentUrl = x.AttachmentUrl,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",

            });
            if (SearchText != "")
            {
                projectRequirements = projectRequirements.Where(s => s.NameAr.Contains(SearchText.Trim()));
            }
            return projectRequirements;
        }


        public async Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementByTaskId(int TaskId, int BranchId)
        {
            var projectRequirements = _TaamerProContext.ProjectRequirements.Where(s => s.IsDeleted == false  && s.PhasesTaskID == TaskId).Select(x => new ProjectRequirementsVM
            {
                RequirementId = x.RequirementId,
                ProjectTypeId = x.ProjectTypeId,
                ProjectSubTypeId = x.ProjectSubTypeId,
                OrderId=x.OrderId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Cost = x.Cost != null ? x.Cost : 0,
                AttachmentUrl = x.AttachmentUrl,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",

            });
        
            return projectRequirements;
        }

        public async Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementByOrderId(int orderid, int BranchId)
        {
            var projectRequirements = _TaamerProContext.ProjectRequirements.Where(s => s.IsDeleted == false && s.OrderId == orderid).Select(x => new ProjectRequirementsVM
            {
                RequirementId = x.RequirementId,
                ProjectTypeId = x.ProjectTypeId,
                ProjectSubTypeId = x.ProjectSubTypeId,
                OrderId = x.OrderId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Cost = x.Cost != null ? x.Cost : 0,
                AttachmentUrl = x.AttachmentUrl,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",

            });

            return projectRequirements;
        }

    }
}


