using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class ProjectStatusTasksRepository : IProjectStatusTasksRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectStatusTasksRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }


        public async Task<IEnumerable<ProjectVM>> GetAllProjectsStatusTasks(string Lang, int BranchId)
        {

            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ContractNo = x.Contracts!.ContractNo??"",
                ContractId = x.ContractId,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                //ContractNo = x.Contracts.ContractNo,
                ContractorName = x.ContractorName,
                ProjectDescription = x.ProjectDescription,
                ProjectDate = x.ProjectDate,
                ProjectHijriDate = x.ProjectHijriDate,
                ProjectTypeId = x.ProjectTypeId,
                MangerId = x.MangerId,
                ParentProjectId = x.ParentProjectId,
                BuildingType = x.BuildingType,
                SubProjectTypeId = x.SubProjectTypeId,
                TransactionTypeId = x.TransactionTypeId,
                CustomerName_W = Lang == "ltr" ? x.customer!.CustomerNameEn : x.customer!.CustomerNameAr,
                CustomerName = Lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
                : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = Lang == "ltr" ? x.projecttype!.NameEn : x.projecttype!.NameAr,
                ProjectSubTypeName = Lang == "ltr" ? x.projectsubtype!.NameEn : x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                CityName = Lang == "ltr" ? x.city!.NameEn : x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                ProjectExpireDate = x.ProjectExpireDate,
                ProjectExpireHijriDate = x.ProjectExpireHijriDate,
                //ActiveMainPhaseId = x.ActiveMainPhaseId,
                Co_opOfficeName = x.Co_opOfficeName ?? "",
                Co_opOfficeEmail = x.Co_opOfficeEmail ?? "",
                Co_opOfficePhone = x.Co_opOfficePhone ?? "",
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                StopProjectType = x.StopProjectType ?? 0,
                TimeStr = (x.NoOfDays < 30) ? x.NoOfDays + " يوم " : (x.NoOfDays == 30) ? x.NoOfDays / 30 + " شهر " : (x.NoOfDays > 30) ? ((x.NoOfDays / 30) + " شهر " + (x.NoOfDays % 30) + " يوم  ") : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = Lang == "ltr" ? x.ActiveMainPhase!.DescriptionEn : x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = Lang == "ltr" ? x.ActiveSubPhase!.DescriptionEn : x.ActiveSubPhase!.DescriptionAr,
                Cost = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0,
                ProjectTaskExist = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status != 4).Count() != 0 ? "نعم" : "لا",
                ProjectInvoiceExist = x.Invoices!.Where(s => s.IsDeleted == false && s.Rad == false).Count() != 0 ? "نعم" : "لا"
            });
            //.OrderByDescending(s => s.ProjectId).ToList()
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetProjectsStatusTasksSearch(ProjectVM ProjectsSearch, string Lang, string Con, int BranchId)
        {

            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && s.BranchId == BranchId && (s.ProjectTypeId == ProjectsSearch.ProjectTypeId || ProjectsSearch.ProjectTypeId == null) &&
                                                (s.SubProjectTypeId == ProjectsSearch.SubProjectTypeId || ProjectsSearch.SubProjectTypeId == null) &&
                                                (s.CustomerId == ProjectsSearch.CustomerId || ProjectsSearch.CustomerId == null) &&
                                                (s.ProjectNo!.Equals(ProjectsSearch.ProjectNo) || ProjectsSearch.ProjectNo == null) &&
                                                (s.MangerId == ProjectsSearch.MangerId || ProjectsSearch.MangerId == null) &&
                                                (s.Contracts!.ContractNo == ProjectsSearch.ContractNo || ProjectsSearch.ContractNo == null) &&
                                                (s.customer!.CustomerNationalId!.Equals(ProjectsSearch.NationalNumber) || ProjectsSearch.NationalNumber == null || s.customer.CustomerNationalId.Contains(ProjectsSearch.NationalNumber)) &&
                                                (s.customer!.CustomerMobile!.Equals(ProjectsSearch.Mobile) || ProjectsSearch.Mobile == null || s.customer.CustomerMobile.Contains(ProjectsSearch.Mobile)) &&
                                                (s.city!.CityId == ProjectsSearch.CityId || ProjectsSearch.CityId == null) &&
                                                (s.ProjectDescription!.Equals(ProjectsSearch.ProjectDescription) || ProjectsSearch.ProjectDescription == null || s.ProjectDescription.Contains(ProjectsSearch.ProjectDescription)) &&
                                                (s.PieceNo.Equals(ProjectsSearch.PieceNo) || ProjectsSearch.PieceNo == null) &&
                                                (ProjectsSearch.ProjectTaskExist != "null" ? (s.ProjectPhasesTasks!.Where(p => p.IsDeleted == false && p.Type == 3 && p.Status != 4).Count() != 0) : ProjectsSearch.ProjectTaskExist == "null") &&
                                                (ProjectsSearch.ProjectInvoiceExist != "null" ? (s.Invoices!.Where(p => p.IsDeleted == false && p.Rad == false).Count() != 0) : ProjectsSearch.ProjectInvoiceExist == "null")).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ContractNo = x.Contracts!.ContractNo??"",
                ContractId = x.ContractId,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                //ContractNo = x.Contracts.ContractNo,
                ContractorName = x.ContractorName,
                ProjectDescription = x.ProjectDescription,
                ProjectDate = x.ProjectDate,
                ProjectHijriDate = x.ProjectHijriDate,
                ProjectTypeId = x.ProjectTypeId,
                MangerId = x.MangerId,
                ParentProjectId = x.ParentProjectId,
                BuildingType = x.BuildingType,
                SubProjectTypeId = x.SubProjectTypeId,
                TransactionTypeId = x.TransactionTypeId,
                                                    CustomerName_W = Lang == "ltr" ? x.customer!.CustomerNameEn : x.customer!.CustomerNameAr,
                                                    CustomerName = Lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
                : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                                                    ProjectTypesName = Lang == "ltr" ? x.projecttype!.NameEn : x.projecttype!.NameAr,
                ProjectSubTypeName = Lang == "ltr" ? x.projectsubtype!.NameEn : x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                CityName = Lang == "ltr" ? x.city!.NameEn : x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                ProjectExpireDate = x.ProjectExpireDate,
                ProjectExpireHijriDate = x.ProjectExpireHijriDate,
                //ActiveMainPhaseId = x.ActiveMainPhaseId,
                Co_opOfficeName = x.Co_opOfficeName ?? "",
                Co_opOfficeEmail = x.Co_opOfficeEmail ?? "",
                Co_opOfficePhone = x.Co_opOfficePhone ?? "",
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                StopProjectType = x.StopProjectType ?? 0,
                TimeStr = (x.NoOfDays < 30) ? x.NoOfDays + " يوم " : (x.NoOfDays == 30) ? x.NoOfDays / 30 + " شهر " : (x.NoOfDays > 30) ? ((x.NoOfDays / 30) + " شهر " + (x.NoOfDays % 30) + " يوم  ") : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = Lang == "ltr" ? x.ActiveMainPhase!.DescriptionEn : x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = Lang == "ltr" ? x.ActiveSubPhase!.DescriptionEn : x.ActiveSubPhase!.DescriptionAr,
                Cost = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0,
                ProjectTaskExist = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status != 4).Count() != 0 ? "نعم" : "لا",
                ProjectInvoiceExist = x.Invoices!.Where(s => s.IsDeleted == false && s.Rad == false).Count() != 0 ? "نعم" : "لا"
            });
            //.OrderByDescending(s => s.ProjectId).ToList()
            return projects;
        }

    }
}
