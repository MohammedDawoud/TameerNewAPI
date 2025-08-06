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
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Repository.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IImportantProjectRepository _importantProjectRepository;
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectRepository(TaamerProjectContext dataContext, IImportantProjectRepository importantProject)
        {
            _importantProjectRepository = importantProject;
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<ProjectVM>> GetAllProject(string Lang, int BranchId)
        {

            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ContractNo = x.Contracts==null?"":x.Contracts!.ContractNo,
                ContractId = x.ContractId,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
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
                CustomerName_W = x.customer == null ? "" : Lang == "ltr" ? x.customer!.CustomerNameEn : x.customer!.CustomerNameAr,
                CustomerName = x.customer == null ? "" : x.customer!.Projects == null ? "" : Lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
                : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype == null ? "" : Lang == "ltr" ? x.projecttype!.NameEn : x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype == null ? "" : Lang == "ltr" ? x.projectsubtype!.NameEn : x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users == null ? "" : x.Users!.FullName,
                CityName = x.city == null ? "" : Lang == "ltr" ? x.city!.NameEn : x.city!.NameAr,
                TransactionTypeName = x.transactionTypes == null ? "" : x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes == null ? "" : x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles == null ? 0 : x.ProjectFiles!.Count(),
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
                ExpectedTime = x.ProjectPhasesTasks == null ? 0 : x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase == null ? "" : Lang == "ltr" ? x.ActiveMainPhase!.DescriptionEn : x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase == null ? "" : Lang == "ltr" ? x.ActiveSubPhase!.DescriptionEn : x.ActiveSubPhase!.DescriptionAr,
                Cost = x.ProjectPhasesTasks == null ? 0 : x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                CostE = x.Invoices == null ? 0 : x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostE_Credit = x.Invoices == null ? 0 : x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostE_Depit = x.Invoices == null ? 0 : x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostS = x.Invoices == null ? 0 : x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad!=true).Sum(s => s.TotalValue) ?? 0,

                CostE_W = x.Invoices == null ? 0 : x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostE_Credit_W = x.Invoices == null ? 0 : x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostE_Depit_W = x.Invoices == null ? 0 : x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostS_W = x.Invoices == null ? 0 : x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                Oper_expeValue = x.Contracts != null ? x.Contracts!.Oper_expeValue ?? 0 : 0,

                OffersPricesId = x.OffersPricesId ?? 0,

                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                TypeCode = x.projecttype == null ? 0 : x.projecttype!.TypeCode,
                Cons_components=x.Cons_components??"",


            });
            //.OrderBy(s => s.IsImportant ==1).ToList();
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetAllProjectCustomerBranch(string Lang, int BranchId)
        {

            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false 
            && s.customer!.BranchId == BranchId
            && s.BranchId == BranchId && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ContractNo = x.Contracts==null?"":x.Contracts!.ContractNo,
                ContractId = x.ContractId,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
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
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                CostE = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostE_Credit = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostE_Depit = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostS = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,

                CostE_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostE_Credit_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostE_Depit_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostS_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                Oper_expeValue = x.Contracts != null ? x.Contracts!.Oper_expeValue ?? 0 : 0,

                OffersPricesId = x.OffersPricesId ?? 0,

                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                TypeCode = x.projecttype!.TypeCode,
                Cons_components = x.Cons_components ?? "",


            });
            //.OrderBy(s => s.IsImportant ==1).ToList();
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetAllProjectsmartfollow(string Lang, int BranchId,int UserId)
        {

            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == 0 && s.MangerId==UserId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ContractNo = x.Contracts==null?"":x.Contracts!.ContractNo,
                ContractId = x.ContractId,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                //ContractNo = x.Contracts!.ContractNo,
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
                ProjectMangerName = Lang == "rtl" ? x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr : x.Users!.FullName,
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
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                CostE = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostE_Credit = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostE_Depit = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostS = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,

                CostE_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostE_Credit_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostE_Depit_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostS_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                Oper_expeValue = x.Contracts != null ? x.Contracts!.Oper_expeValue ?? 0 : 0,

                OffersPricesId = x.OffersPricesId ?? 0,

                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                TypeCode = x.projecttype!.TypeCode,
                Cons_components = x.Cons_components ?? "",
                ProjectManagerImg=x.Users!.ImgUrl ?? "/distnew/images/userprofile.png",
                Plustimecount=x.Plustimecount??0,
                SkipCount=x.SkipCount??0,

            });
            //.OrderBy(s => s.IsImportant ==1).ToList();
            return projects;
        }


        public async Task< IEnumerable<ProjectVM>> GetAllProjectsmartfollowforadmin(string Lang, int BranchId)
        {

            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ContractNo = x.Contracts==null?"":x.Contracts!.ContractNo,
                ContractId = x.ContractId,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                //ContractNo = x.Contracts!.ContractNo,
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
                ProjectMangerName = Lang == "rtl" ? x.Users!.FullNameAr ==null? x.Users!.FullName : x.Users!.FullNameAr : x.Users!.FullName,
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
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                CostE = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostE_Credit = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostE_Depit = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostS = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,

                CostE_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostE_Credit_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostE_Depit_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostS_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                Oper_expeValue = x.Contracts != null ? x.Contracts!.Oper_expeValue ?? 0 : 0,

                OffersPricesId = x.OffersPricesId ?? 0,

                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                TypeCode = x.projecttype!.TypeCode,
                Cons_components = x.Cons_components ?? "",
                ProjectManagerImg = x.Users!.ImgUrl ?? "/distnew/images/userprofile.png",
                Plustimecount = x.Plustimecount ?? 0,
                SkipCount = x.SkipCount ?? 0,

            });
            //.OrderBy(s => s.IsImportant ==1).ToList();
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetAllProjects3(string Con, string Lang, int BranchId, int UserId)
        {


            try
            {
                List<ProjectVM> lmd = new List<ProjectVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetAllProjectsNew";
                        command.CommandTimeout = 0;
                        command.Connection = con;

                        //---------------------------------------------------------------------
                        if (UserId == 0 || UserId == null)
                            command.Parameters.Add(new SqlParameter("@UserId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@UserId", UserId));
                        //---------------------------------------------------------------------
                        //---------------------------------------------------------------------
                        if (BranchId == 0 || BranchId == null)
                            command.Parameters.Add(new SqlParameter("@BranchId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        //---------------------------------------------------------------------
                    
                            command.Parameters.Add(new SqlParameter("@Status", DBNull.Value));
                  
                        //---------------------------------------------------------------------
                            command.Parameters.Add(new SqlParameter("@ProjectNo", DBNull.Value));
                        //---------------------------------------------------------------------
                        command.Parameters.Add(new SqlParameter("@ProjectType", DBNull.Value));
                        //---------------------------------------------------------------------
                            command.Parameters.Add(new SqlParameter("@ProjectSubType", DBNull.Value));
                        //---------------------------------------------------------------------
                        command.Parameters.Add(new SqlParameter("@CustomerId", DBNull.Value));
                        //---------------------------------------------------------------------
                        command.Parameters.Add(new SqlParameter("@ManagerId", DBNull.Value));
                        //---------------------------------------------------------------------
                        command.Parameters.Add(new SqlParameter("@ProjectId", DBNull.Value));
                        //---------------------------------------------------------------------
                          command.Parameters.Add(new SqlParameter("@ContractNo", DBNull.Value));
                        //---------------------------------------------------------------------
                            command.Parameters.Add(new SqlParameter("@NationalId", DBNull.Value));
                        //---------------------------------------------------------------------
                        //---------------------------------------------------------------------
                            command.Parameters.Add(new SqlParameter("@Mobile", DBNull.Value));
                        //---------------------------------------------------------------------
                        command.Parameters.Add(new SqlParameter("@CityId", DBNull.Value)); 
                        //---------------------------------------------------------------------
                            command.Parameters.Add(new SqlParameter("@ProjectDesc", DBNull.Value));
                        //---------------------------------------------------------------------
                            command.Parameters.Add(new SqlParameter("@PieceNo", DBNull.Value));
                        //---------------------------------------------------------------------
                                command.Parameters.Add(new SqlParameter("@DepartmentId", DBNull.Value));
                        //---------------------------------------------------------------------
                            command.Parameters.Add(new SqlParameter("@startdate", DBNull.Value));
                        //---------------------------------------------------------------------
                            command.Parameters.Add(new SqlParameter("@enddate", DBNull.Value));
                        //---------------------------------------------------------------------

                        command.Parameters.Add(new SqlParameter("@AllUserBranchId", 0));
                        //---------------------------------------------------------------------

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lmd.Add(new ProjectVM
                            {
                                ProjectId = Convert.ToInt32((dr["ProjectId"]).ToString()),
                                ProjectName = (dr["ProjectName"]).ToString(),
                                ContractNo = (dr["ContractNo"]).ToString(),
                                ContractDate = (dr["ContractDate"]).ToString(),
                                ContractId = Convert.ToInt32((dr["ContractId"]).ToString()),
                                ProjectNo = (dr["ProjectNo"]).ToString(),
                                CustomerId = Convert.ToInt32((dr["CustomerId"]).ToString()),
                                ProjectTypeName = (dr["ProjectTypeName"]).ToString(),
                                CityId = Convert.ToInt32((dr["CityId"]).ToString()),
                                ContractorName = (dr["ContractorName"]).ToString(),
                                ProjectDescription = (dr["ProjectDescription"]).ToString(),
                                ProjectDate = (dr["ProjectDate"]).ToString(),
                                ProjectHijriDate = (dr["ProjectHijriDate"]).ToString(),
                                ProjectTypeId = Convert.ToInt32((dr["ProjectTypeId"]).ToString()),
                                MangerId = Convert.ToInt32((dr["MangerId"]).ToString()),
                                ParentProjectId = Convert.ToInt32((dr["ParentProjectId"]).ToString()),
                                BuildingType = Convert.ToInt32((dr["BuildingType"]).ToString()),
                                SubProjectTypeId = Convert.ToInt32((dr["SubProjectTypeId"]).ToString()),
                                TransactionTypeId = Convert.ToInt32((dr["TransactionTypeId"]).ToString()),
                                CustomerName_W = (dr["CustomerName_W"]).ToString(),
                                CustomerName = (dr["CustomerName"]).ToString(),
                                ProjectTypesName = (dr["ProjectTypesName"]).ToString(),
                                ProjectSubTypeName = (dr["ProjectSubTypeName"]).ToString(),
                                ProjectMangerName = (dr["ProjectMangerName"]).ToString(),
                                CityName = (dr["CityName"]).ToString(),
                                TransactionTypeName = (dr["TransactionTypeName"]).ToString(),
                                RegionTypeId = Convert.ToInt32((dr["RegionTypeId"]).ToString()),
                                RegionTypeName = (dr["RegionTypeName"]).ToString(),
                                FileCount = Convert.ToInt32((dr["FileCount"]).ToString()),
                                ProjectExpireDate = (dr["ProjectExpireDate"]).ToString(),
                                ProjectExpireHijriDate = (dr["ProjectExpireHijriDate"]).ToString(),
                                Co_opOfficeName = (dr["Co_opOfficeName"]).ToString(),
                                Co_opOfficeEmail = (dr["Co_opOfficeEmail"]).ToString(),
                                Co_opOfficePhone = (dr["Co_opOfficePhone"]).ToString(),
                                BranchId = Convert.ToInt32((dr["BranchId"]).ToString()),
                                CostCenterId = Convert.ToInt32((dr["CostCenterId"]).ToString()),
                                FirstProjectDate = (dr["FirstProjectDate"]).ToString(),
                                FirstProjectExpireDate = (dr["FirstProjectExpireDate"]).ToString(),
                                StopProjectType = Convert.ToInt32((dr["StopProjectType"]).ToString()),
                                TimeStr = (dr["TimeStr"]).ToString(),
                                ExpectedTime = Convert.ToDecimal((dr["ExpectedTime"]).ToString()),
                                CurrentMainPhase = (dr["CurrentMainPhase"]).ToString(),
                                CurrentSubPhase = (dr["CurrentSubPhase"]).ToString(),
                                Cost = Convert.ToDecimal((dr["Cost"]).ToString()),
                                ContractValue = (dr["ContractValue"]).ToString(),
                                ContractValue_W = (dr["ContractValue_W"]).ToString(),
                                CostE = Convert.ToDecimal((dr["CostE"]).ToString()),
                                CostE_Credit = Convert.ToDecimal((dr["CostE_Credit"]).ToString()),
                                CostE_Depit = Convert.ToDecimal((dr["CostE_Depit"]).ToString()),
                                CostS = Convert.ToDecimal((dr["CostS"]).ToString()),
                                CostE_W = Convert.ToDecimal((dr["CostE_W"]).ToString()),
                                CostE_Credit_W = Convert.ToDecimal((dr["CostE_Credit_W"]).ToString()),
                                CostE_Depit_W = Convert.ToDecimal((dr["CostE_Depit_W"]).ToString()),
                                CostS_W = Convert.ToDecimal((dr["CostS_W"]).ToString()),
                                Oper_expeValue = Convert.ToDecimal((dr["Oper_expeValue"]).ToString()),
                                OffersPricesId = Convert.ToInt32((dr["OffersPricesId"]).ToString()),
                                DepartmentId = Convert.ToInt32((dr["DepartmentId"]).ToString()),
                                MotionProject = Convert.ToInt32((dr["MotionProject"]).ToString()),
                                MotionProjectDate = (dr["MotionProjectDate"]).ToString(),
                                MotionProjectNote = (dr["MotionProjectNote"]).ToString(),
                                TypeCode = Convert.ToInt32((dr["TypeCode"]).ToString()),
                                Cons_components = (dr["Cons_components"]).ToString(),
                                Isimportant = Convert.ToInt32((dr["Isimportant"]).ToString()),
                                importantid = Convert.ToInt32((dr["importantid"]).ToString()),
                                flag = Convert.ToInt32((dr["flag"]).ToString()),
                                AddUser = (dr["addUser"].ToString()),
                                AddedUserImg = (dr["AddedUserImg"].ToString()),
                                UpdateUser = (dr["UpdateUser"].ToString()),
                                FinishDate = (dr["FinishDate"].ToString()),
                                ReasonText = (dr["ReasonText"].ToString()),
                                ContractorSelectId = Convert.ToInt32((dr["ContractorSelectId"]).ToString()),
                                ContractorEmail_T = (dr["ContractorEmail_T"].ToString()),
                                ContractorPhone_T = (dr["ContractorPhone_T"].ToString()),
                                ContractorCom_T = (dr["ContractorCom_T"].ToString()),




                                SiteName = (dr["SiteName"]).ToString(),
                                OrderType = (dr["OrderType"]).ToString(),
                                SketchName = (dr["SketchName"]).ToString(),
                                SketchNo = (dr["SketchNo"]).ToString(),
                                PieceNo = Convert.ToInt32((dr["PieceNo"]).ToString()),
                                AdwAR = Convert.ToInt32((dr["AdwAR"]).ToString()),
                                Status = Convert.ToInt32((dr["Status"]).ToString()),
                                OrderNo = (dr["OrderNo"]).ToString(),
                                OutBoxNo = (dr["OutBoxNo"]).ToString(),
                                OutBoxDate = (dr["OutBoxDate"]).ToString(),
                                OutBoxHijriDate = (dr["OutBoxHijriDate"]).ToString(),
                                Reason1 = (dr["Reason1"]).ToString(),
                                Notes1 = (dr["Notes1"]).ToString(),
                                Subject = (dr["Subject"]).ToString(),
                                XPoint = (dr["XPoint"]).ToString(),
                                YPoint = (dr["YPoint"]).ToString(),
                                Technical = (dr["Technical"]).ToString(),
                                Prosedor = (dr["Prosedor"]).ToString(),
                                ReasonRevers = (dr["ReasonRevers"]).ToString(),
                                EngNotes = (dr["EngNotes"]).ToString(),
                                ReverseDate = (dr["ReverseDate"]).ToString(),
                                ReverseHijriDate = (dr["ReverseHijriDate"]).ToString(),
                                OrderStatus = Convert.ToInt32((dr["OrderStatus"]).ToString()),
                                UserId = Convert.ToInt32((dr["UserId"]).ToString()),
                                Receipt = Convert.ToInt32((dr["Receipt"]).ToString()),
                                PayStatus = Convert.ToBoolean((dr["PayStatus"]).ToString()),
                                RegionName = (dr["RegionName"]).ToString(),
                                DistrictName = (dr["DistrictName"]).ToString(),
                                SiteType = (dr["SiteType"]).ToString(),
                                ContractSource = (dr["ContractSource"]).ToString(),
                                SiteNo = (dr["SiteNo"]).ToString(),
                                PayanNo = (dr["PayanNo"]).ToString(),
                                JehaId = Convert.ToInt32((dr["JehaId"]).ToString()),
                                ZaraaSak = (dr["ZaraaSak"]).ToString(),
                                ZaraaNatural = (dr["ZaraaNatural"]).ToString(),
                                BordersSak = (dr["BordersSak"]).ToString(),
                                BordersNatural = (dr["BordersNatural"]).ToString(),
                                Ertedad = (dr["Ertedad"]).ToString(),
                                Brooz = (dr["Brooz"]).ToString(),
                                AreaSak = (dr["AreaSak"]).ToString(),
                                AreaNatural = (dr["AreaNatural"]).ToString(),
                                AreaArrange = (dr["AreaArrange"]).ToString(),
                                BuildingPercent = (dr["BuildingPercent"]).ToString(),
                                SpaceName = (dr["SpaceName"]).ToString(),
                                Office = (dr["Office"]).ToString(),
                                Usage = (dr["Usage"]).ToString(),
                                Docpath = (dr["Docpath"]).ToString(),
                                elevators = (dr["elevators"]).ToString(),
                                typ1 = (dr["typ1"]).ToString(),
                                brozat = (dr["brozat"]).ToString(),
                                entries = (dr["entries"]).ToString(),
                                Basement = (dr["Basement"]).ToString(),
                                GroundFloor = (dr["GroundFloor"]).ToString(),
                                FirstFloor = (dr["FirstFloor"]).ToString(),
                                Motkrr = (dr["Motkrr"]).ToString(),
                                FirstExtension = (dr["FirstExtension"]).ToString(),
                                ExtensionName = (dr["ExtensionName"]).ToString(),
                                GeneralLocation = (dr["GeneralLocation"]).ToString(),
                                LicenseNo = (dr["LicenseNo"]).ToString(),
                                Licensedate = (dr["Licensedate"]).ToString(),
                                LicenseHijridate = (dr["LicenseHijridate"]).ToString(),
                                DesiningOffice = (dr["DesiningOffice"]).ToString(),
                                estsharyformoslhat = Convert.ToInt32((dr["estsharyformoslhat"]).ToString()),
                                Consultantfinishing = Convert.ToInt32((dr["Consultantfinishing"]).ToString()),
                                Period = (dr["Period"]).ToString(),
                                punshmentamount = Convert.ToInt32((dr["punshmentamount"]).ToString()),
                                FirstPay = Convert.ToDecimal((dr["FirstPay"]).ToString()),
                                LicenseContent = (dr["LicenseContent"]).ToString(),
                                OtherStatus = Convert.ToInt32((dr["OtherStatus"]).ToString()),
                                AreaSpace = (dr["AreaSpace"]).ToString(),
                                SupervisionSatartDate = (dr["SupervisionSatartDate"]).ToString(),
                                SupervisionSatartHijriDate = (dr["SupervisionSatartHijriDate"]).ToString(),
                                SupervisionEndDate = (dr["SupervisionEndDate"]).ToString(),
                                SupervisionEndHijriDate = (dr["SupervisionEndHijriDate"]).ToString(),
                                SupervisionNo = (dr["SupervisionNo"]).ToString(),
                                SupervisionNotes = (dr["SupervisionNotes"]).ToString(),
                                qaboqwaedmostlm = (dr["qaboqwaedmostlm"]).ToString(),
                                qaboreqabmostlm = (dr["qaboreqabmostlm"]).ToString(),
                                qabosaqfmostlm = (dr["qabosaqfmostlm"]).ToString(),
                                molhqalwisaqffash = (dr["molhqalwisaqffash"]).ToString(),
                                molhqalwisaqfdate = (dr["molhqalwisaqfdate"]).ToString(),
                                LimitDays = Convert.ToInt32((dr["LimitDays"]).ToString()),
                                NoteDate = (dr["NoteDate"]).ToString(),
                                NoteHijriDate = (dr["NoteHijriDate"]).ToString(),
                                ResponseEng = (dr["ResponseEng"]).ToString(),
                                ReseveStatus = Convert.ToInt32((dr["ReseveStatus"]).ToString()),
                                kaeedno = (dr["kaeedno"]).ToString(),
                                TechnicalDemands = (dr["TechnicalDemands"]).ToString(),
                                Todoaction = (dr["Todoaction"]).ToString(),
                                Responsible = (dr["Responsible"]).ToString(),
                                ExternalEmpId = Convert.ToInt32((dr["ExternalEmpId"]).ToString()),
                                ContractPeriod = Convert.ToInt32((dr["ContractPeriod"]).ToString()),
                                SpaceNotes = (dr["SpaceNotes"]).ToString(),
                                ContractNotes = (dr["ContractNotes"]).ToString(),
                                SpaceId = Convert.ToInt32((dr["SpaceId"]).ToString()),
                                Paied = Convert.ToInt32((dr["Paied"]).ToString()),
                                Discount = Convert.ToInt32((dr["Discount"]).ToString()),
                                Fees = Convert.ToInt32((dr["Fees"]).ToString()),
                                ProjectRegionName = (dr["ProjectRegionName"]).ToString(),
                                Catego = (dr["Catego"]).ToString(),
                                ContractPeriodType = (dr["ContractPeriodType"]).ToString(),
                                ContractPeriodMinites = Convert.ToInt32((dr["ContractPeriodMinites"]).ToString()),
                                ProjectValue = Convert.ToDecimal((dr["ProjectValue"]).ToString()),
                                ProjectContractTawk = (dr["ProjectContractTawk"]).ToString(),
                                ProjectRecieveLoaction = (dr["ProjectRecieveLoaction"]).ToString(),
                                ProjectObserveMobile = (dr["ProjectObserveMobile"]).ToString(),
                                ProjectObserveMail = (dr["ProjectObserveMail"]).ToString(),
                                ProjectTaslemFirst = (dr["ProjectTaslemFirst"]).ToString(),
                                FDamanID = Convert.ToInt32((dr["FDamanID"]).ToString()),
                                LDamanID = Convert.ToInt32((dr["LDamanID"]).ToString()),
                                NesbaEngaz = Convert.ToDecimal((dr["NesbaEngaz"]).ToString()),
                                Takeem = (dr["Takeem"]).ToString(),
                                ProjectContractTawkCh = Convert.ToBoolean((dr["ProjectContractTawkCh"]).ToString()),
                                ProjectRecieveLoactionCh = Convert.ToBoolean((dr["ProjectRecieveLoactionCh"]).ToString()),
                                ProjectTaslemFirstCh = Convert.ToBoolean((dr["ProjectTaslemFirstCh"]).ToString()),
                                ContractCh = Convert.ToBoolean((dr["ContractCh"]).ToString()),
                                PeriodProject = Convert.ToInt32((dr["PeriodProject"]).ToString()),
                                AgentDate = (dr["AgentDate"]).ToString(),
                                AgentHijriDate = (dr["AgentHijriDate"]).ToString(),
                                StreetName = (dr["StreetName"]).ToString(),
                                MainText = (dr["MainText"]).ToString(),
                                BranchText = (dr["BranchText"]).ToString(),
                                TaskText = (dr["TaskText"]).ToString(),
                                MunicipalId = Convert.ToInt32((dr["MunicipalId"]).ToString()),
                                SubMunicipalityId = Convert.ToInt32((dr["SubMunicipalityId"]).ToString()),
                                ProBuildingDisc = (dr["ProBuildingDisc"]).ToString(),
                                DestinationsUpload = Convert.ToInt32(dr["DestinationsUpload"].ToString()),
                                NewSetting = Convert.ToBoolean((dr["NewSetting"]).ToString()),


                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<ProjectVM> lmd = new List<ProjectVM>();
                return lmd;
            }
        }
        public async Task< IEnumerable<ProjectVM>> GetAllProjects2(string Lang, int BranchId, int UserId)
        {

            //IEnumerable<ProProjects> pro = new List<ProProjects>();// _context.ProProjects ;
           
            List<ProjectVM> pros = new List<ProjectVM>();
            List<Project> project = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == 0).ToList();
            foreach (var item in project)
            {
       
                var import = _TaamerProContext.ImportantProject.Where(x => x.IsDeleted == false && x.ProjectId == item.ProjectId && x.UserId == UserId).FirstOrDefault();

                try
                {
                    ProjectVM pr = new ProjectVM();

                    pr.ProjectId = item.ProjectId;
                    pr.ProjectName = item.ProjectName ?? "";
                    pr.ContractNo = item.Contracts!.ContractNo?? "";
                    pr.ContractId = item.ContractId ?? 0;
                    pr.ProjectNo = item.ProjectNo ?? "";
                    pr.CustomerId = item.CustomerId ?? 0;
                    pr.ProjectTypeName = item.ProjectTypeName ?? "";
                    pr.CityId = item.CityId ?? 0;
                    //ContractNo = item.Contracts!.ContractNo;
                    pr.ContractorName = item.ContractorName ?? "";
                    pr.ProjectDescription = item.ProjectDescription ?? "";
                    pr.ProjectDate = item.ProjectDate ?? "";
                    pr.ProjectHijriDate = item.ProjectHijriDate ?? "";
                    pr.ProjectTypeId = item.ProjectTypeId ?? 0;
                    pr.MangerId = item.MangerId ?? 0;
                    pr.ParentProjectId = item.ParentProjectId ?? 0;
                    pr.BuildingType = item.BuildingType ?? 0;
                    pr.SubProjectTypeId = item.SubProjectTypeId ?? 0;
                    pr.TransactionTypeId = item.TransactionTypeId ?? 0;
                    pr.CustomerName_W = Lang == "ltr" ? item.customer!.CustomerNameEn : item.customer!.CustomerNameAr ?? "";
                    pr.CustomerName = Lang == "ltr" ? item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? item.customer!.CustomerNameEn : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? item.customer!.CustomerNameEn : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? item.customer!.CustomerNameEn + "(*)" : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? item.customer!.CustomerNameEn + "(**)" : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? item.customer!.CustomerNameEn + "(***)" : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? item.customer!.CustomerNameEn + "(VIP)" : item.customer!.CustomerNameEn
                : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? item.customer!.CustomerNameAr : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? item.customer!.CustomerNameAr : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? item.customer!.CustomerNameAr + "(*)" : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? item.customer!.CustomerNameAr + "(**)" : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? item.customer!.CustomerNameAr + "(***)" : item.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? item.customer!.CustomerNameAr + "(VIP)" : item.customer!.CustomerNameAr ?? "";

                    pr.ProjectTypesName = Lang == "ltr" ? item.projecttype!.NameEn : item.projecttype!.NameAr ?? "";
                    pr.ProjectSubTypeName = Lang == "ltr" ? item.projectsubtype!.NameEn : item.projectsubtype!.NameAr ?? "";
                    pr.ProjectMangerName = item.Users!.FullName ?? "";
                    pr.CityName = Lang == "ltr" ? item.city!.NameEn : item.city!.NameAr ?? "";
                    pr.TransactionTypeName = item.transactionTypes!.NameAr ?? "";
                    pr.RegionTypeId = item.RegionTypeId ?? 0;
                    pr.RegionTypeName = item.regionTypes.NameAr ?? "";
                    pr.FileCount = item.ProjectFiles.Count() ;
                    pr.ProjectExpireDate = item.ProjectExpireDate ?? "";
                    pr.ProjectExpireHijriDate = item.ProjectExpireHijriDate ?? "";
                    //ActiveMainPhaseId = item.ActiveMainPhaseId;
                    pr.Co_opOfficeName = item.Co_opOfficeName ?? "";
                    pr.Co_opOfficeEmail = item.Co_opOfficeEmail ?? "";
                    pr.Co_opOfficePhone = item.Co_opOfficePhone ?? "";
                    pr.BranchId = item.BranchId;
                    pr.CostCenterId = item.CostCenterId ?? 0;
                    pr.FirstProjectDate = item.FirstProjectDate ?? "";
                    pr.FirstProjectExpireDate = item.FirstProjectExpireDate ?? "";
                    pr.StopProjectType = item.StopProjectType ?? 0;
                    pr.TimeStr = (item.NoOfDays < 30) ? item.NoOfDays + " يوم " : (item.NoOfDays == 30) ? item.NoOfDays / 30 + " شهر " : (item.NoOfDays > 30) ? ((item.NoOfDays / 30) + " شهر " + (item.NoOfDays % 30) + " يوم  ") : "" ??"";
                    pr.ExpectedTime = item.ProjectPhasesTasks!.Sum(s => s.TimeMinutes) ?? 0;
                    pr.CurrentMainPhase = Lang == "ltr" ? item.ActiveMainPhase!.DescriptionEn : item.ActiveMainPhase!.DescriptionAr ?? "";
                    pr.CurrentSubPhase = Lang == "ltr" ? item.ActiveSubPhase!.DescriptionEn : item.ActiveSubPhase!.DescriptionAr ?? "";
                    pr.Cost = item.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0;
                    pr.ContractValue = item.Contracts != null ? item.Contracts!.TotalValue.ToString() : "0";
                    pr.CostE = item.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0;
                    pr.CostS = item.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0;
                    pr.OffersPricesId = item.OffersPricesId ?? 0;

                    pr.DepartmentId = item.DepartmentId ?? 0;
                    pr.MotionProject = item.MotionProject ;
                    pr.MotionProjectDate = item.MotionProjectDate ?? "";
                    pr.MotionProjectNote = item.MotionProjectNote ?? "";

                    pr.Isimportant = import!.IsImportant ?? 0;
                    pr.importantid = import!.ImportantProId ;
                    pr.flag = import!.Flag ?? 0;
                    pr.Cons_components = item.Cons_components ?? "";


                    pros.Add(pr);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }

            }

            return pros;
        }


        public async Task< IEnumerable<ProjectVM>> GetAllProjectWithout(string Lang, int BranchId)
        {

            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ContractNo = x.Contracts==null?"":x.Contracts!.ContractNo,
                ContractId = x.ContractId,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                //ContractNo = x.Contracts!.ContractNo,
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
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                OffersPricesId = x.OffersPricesId ?? 0,
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",


            });
            //.OrderByDescending(s => s.ProjectId).ToList()
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetAllProjectAllBranches()
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,
                ContractorName = x.ContractorName,
                ProjectDescription = x.ProjectDescription,
                ProjectDate = x.ProjectDate,
                ProjectHijriDate = x.ProjectHijriDate,
                ProjectExpireDate = x.ProjectExpireDate,
                ProjectExpireHijriDate = x.ProjectExpireHijriDate,
                ProjectTypeId = x.ProjectTypeId,
                MangerId = x.MangerId,
                ParentProjectId = x.ParentProjectId,
                BuildingType = x.BuildingType,
                SubProjectTypeId = x.SubProjectTypeId,
                TransactionTypeId = x.TransactionTypeId,
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr  : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FileCount = x.ProjectFiles!.Count(),
                TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                PercentComplete = x.PercentComplete,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                Co_opOfficeName = x.Co_opOfficeName ?? "",
                Co_opOfficeEmail = x.Co_opOfficeEmail ?? "",
                Co_opOfficePhone = x.Co_opOfficePhone ?? "",
                StopProjectType = x.StopProjectType ?? 0,
                Cost = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                OffersPricesId=x.OffersPricesId ?? 0,
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",

                //,
                //RemainingTime = (DateTime.ParseExact(x.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).Subtract(DateTime.ParseExact(x.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Days
            });

            return projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetAllProjectAllBranches2()
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,
                ContractorName = x.ContractorName,
                ProjectDescription = x.ProjectDescription,
                ProjectDate = x.ProjectDate,
                ProjectHijriDate = x.ProjectHijriDate,
                ProjectExpireDate = x.ProjectExpireDate,
                ProjectExpireHijriDate = x.ProjectExpireHijriDate,
                ProjectTypeId = x.ProjectTypeId,
                MangerId = x.MangerId,
                ParentProjectId = x.ParentProjectId,
                BuildingType = x.BuildingType,
                SubProjectTypeId = x.SubProjectTypeId,
                TransactionTypeId = x.TransactionTypeId,
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FileCount = x.ProjectFiles!.Count(),
                TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                PercentComplete = x.PercentComplete,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                Co_opOfficeName = x.Co_opOfficeName ?? "",
                Co_opOfficeEmail = x.Co_opOfficeEmail ?? "",
                Co_opOfficePhone = x.Co_opOfficePhone ?? "",
                StopProjectType = x.StopProjectType ?? 0,
                TaskExecPercentage_Count = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Count(),
                TaskExecPercentage_Sum = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.ExecPercentage) ?? 0,
                Cost = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",

                //RemainingTime = (DateTime.ParseExact(x.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).Subtract(DateTime.ParseExact(x.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Days
            });

            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetAllProjectNumber(string SearchText, int BranchId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,
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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FileCount = x.ProjectFiles!.Count(),
                TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                StopProjectType = x.StopProjectType ?? 0,
                Cost = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",

            });
            if (SearchText != "")
            {
                projects = projects.Where(s => s.ProjectNo!.Contains(SearchText.Trim()) || s.CustomerName.Contains(SearchText.Trim()));
            }
            return projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetAllArchiveProject(int BranchId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 1 && s.BranchId == BranchId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts==null? "": x.Contracts!.ContractNo,
                ContractId = x.ContractId,
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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,
                ProjectTypesName = x.projecttype == null ? "" : x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype == null ? "" : x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users == null ? "" : x.Users!.FullName,
                NationalNumber = x.customer == null ? "" : x.customer!.CustomerNationalId,
                Mobile = x.customer == null ? "" : x.customer!.CustomerMobile,
                CityName = x.city == null ? "" : x.city!.NameAr,
                TransactionTypeName = x.transactionTypes == null ? "" : x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes == null ? "" : x.regionTypes!.NameAr,
                //FileCount = x.ProjectFiles == null ? 0 : x.ProjectFiles!.Count(),
                FileCount = 0,

                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FirstProjectDate = x.FirstProjectDate,
                TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                ExpectedTime = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.TimeMinutes) ?? 0,
                CurrentMainPhase = x.ActiveMainPhase == null ? "" : x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase == null ? "" : x.ActiveSubPhase!.DescriptionAr,
                FinishDate = x.FinishDate,
                ReasonText = x.ReasonText,
                ReasonID = x.ReasonID,
                UpdateUser = x.UpdateUsers == null ? "" : x.UpdateUsers!.FullName,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                StopProjectType = x.StopProjectType??0,
                DateOfFinish = x.DateOfFinish,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",

            });
            return projects;
        }
        public async Task<ProjectVM> GetProjectByNUmber(string Lang, string projectnumber)
        {
            var pro = _TaamerProContext.Project.Where(s => s.ProjectNo == projectnumber).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                CustomerId = x.CustomerId,
                ParentProjectId = x.ParentProjectId,
                MangerId = x.MangerId,
                TransactionTypeId = x.TransactionTypeId,
                ProjectDate = x.ProjectDate,
                ContractId = x.ContractId,
                ProjectHijriDate = x.ProjectHijriDate,
                ProjectExpireDate = x.ProjectExpireDate,
                ProjectExpireHijriDate = x.ProjectExpireHijriDate,
                SiteName = x.SiteName,
                ProjectTypeId = x.ProjectTypeId,
                SubProjectTypeId = x.SubProjectTypeId,
                OrderType = x.OrderType,
                SketchName = x.SketchName,
                SketchNo = x.SketchNo,
                PieceNo = x.PieceNo ?? 0,
                AdwAR = x.AdwAR,
                Status = x.Status,
                OrderNo = x.OrderNo,
                OutBoxNo = x.OutBoxNo,
                OutBoxDate = x.OutBoxDate,
                OutBoxHijriDate = x.OutBoxHijriDate,
                Reason1 = x.Reason1,
                Notes1 = x.Notes1,
                Subject = x.Subject,
                XPoint = x.XPoint,
                YPoint = x.YPoint,
                Technical = x.Technical,
                Prosedor = x.Prosedor,
                ReasonRevers = x.ReasonRevers,
                EngNotes = x.EngNotes,
                ReverseDate = x.ReverseDate,
                ReverseHijriDate = x.ReverseHijriDate,
                OrderStatus = x.OrderStatus,
                UserId = x.UserId,
                Receipt = x.Receipt,
                PayStatus = x.PayStatus,
                RegionName = x.RegionName,
                DistrictName = x.DistrictName,
                SiteType = x.SiteType,
                ContractNo = x.Contracts == null ? "" : x.Contracts!.ContractNo,
                ContractDate = x.Contracts == null ? "" : x.Contracts!.Date,
                ContractHijriDate = x.ContractHijriDate,
                ContractSource = x.ContractSource,
                SiteNo = x.SiteNo,
                PayanNo = x.PayanNo,
                JehaId = x.JehaId,
                ZaraaSak = x.ZaraaSak,
                ZaraaNatural = x.ZaraaNatural,
                BordersSak = x.BordersSak,
                BordersNatural = x.BordersNatural,
                Ertedad = x.Ertedad,
                Brooz = x.Brooz,
                AreaSak = x.AreaSak,
                AreaNatural = x.AreaNatural,
                AreaArrange = x.AreaArrange,
                BuildingType = x.BuildingType,
                BuildingPercent = x.BuildingPercent,
                SpaceName = x.SpaceName,
                Office = x.Office,
                Usage = x.Usage,
                Docpath = x.Docpath,
                RegionTypeId = x.RegionTypeId,
                elevators = x.elevators,
                typ1 = x.typ1,
                brozat = x.brozat,
                entries = x.entries,
                Basement = x.Basement,
                GroundFloor = x.GroundFloor,
                FirstFloor = x.FirstFloor,
                Motkrr = x.Motkrr,
                FirstExtension = x.FirstExtension,
                ExtensionName = x.ExtensionName,
                GeneralLocation = x.GeneralLocation,
                LicenseNo = x.LicenseNo,
                Licensedate = x.Licensedate,
                LicenseHijridate = x.LicenseHijridate,
                DesiningOffice = x.DesiningOffice,
                estsharyformoslhat = x.estsharyformoslhat,
                Consultantfinishing = x.Consultantfinishing,
                Period = x.Period,
                punshmentamount = x.punshmentamount,
                FirstPay = x.FirstPay,
                LicenseContent = x.LicenseContent,
                OtherStatus = x.OtherStatus,
                AreaSpace = x.AreaSpace,
                ContractorName = x.ContractorName,
                ContractorMobile = x.ContractorMobile,
                SupervisionSatartDate = x.SupervisionSatartDate,
                SupervisionSatartHijriDate = x.SupervisionSatartHijriDate,
                SupervisionEndDate = x.SupervisionEndDate,
                SupervisionEndHijriDate = x.SupervisionEndHijriDate,
                SupervisionNo = x.SupervisionNo,
                SupervisionNotes = x.SupervisionNotes,
                qaboqwaedmostlm = x.qaboqwaedmostlm,
                qaboreqabmostlm = x.qaboreqabmostlm,
                qabosaqfmostlm = x.qabosaqfmostlm,
                molhqalwisaqffash = x.molhqalwisaqffash,
                molhqalwisaqfdate = x.molhqalwisaqfdate,
                molhqalwisaqfHijridate = x.molhqalwisaqfHijridate,
                molhqalwisaqfmostlm = x.molhqalwisaqfmostlm,
                molhqardisaqffash = x.molhqardisaqffash,
                molhqardisaqfdate = x.molhqardisaqfdate,
                molhqardisaqfHijridate = x.molhqardisaqfHijridate,
                molhqardisaqfmostlm = x.molhqardisaqfmostlm,
                FinalOrder = x.FinalOrder,
                SpaceBuild = x.SpaceBuild,
                FloorEstablishing = x.FloorEstablishing,
                Roof = x.Roof,
                Electric = x.Electric,
                Takeef = x.Takeef,
                ProjectNo = x.ProjectNo,
                LimitDate = x.LimitDate,
                LimitHijriDate = x.LimitHijriDate,
                LimitDays = x.LimitDays,
                NoteDate = x.NoteDate,
                NoteHijriDate = x.NoteHijriDate,
                ResponseEng = x.ResponseEng,
                ReseveStatus = x.ReseveStatus,
                kaeedno = x.kaeedno,
                TechnicalDemands = x.TechnicalDemands,
                Todoaction = x.Todoaction,
                Responsible = x.Responsible,
                ExternalEmpId = x.ExternalEmpId,
                FinishDate = x.FinishDate,
                FinishHijriDate = x.FinishHijriDate,
                ContractPeriod = x.ContractPeriod,
                SpaceNotes = x.SpaceNotes,
                ContractNotes = x.ContractNotes,
                SpaceId = x.SpaceId,
                CityId = x.CityId,
                ProjectDescription = x.ProjectDescription,
                Paied = x.Paied,
                Discount = x.Discount,
                Fees = x.Fees,
                ProjectTypeName = x.ProjectTypeName,
                ProjectRegionName = x.ProjectRegionName,
                Catego = x.Catego,
                ContractPeriodType = x.ContractPeriodType,
                ContractPeriodMinites = x.ContractPeriodMinites,
                ProjectName = x.ProjectName,
                ProjectValue = x.ProjectValue,
                ProjectContractTawk = x.ProjectContractTawk,
                ProjectRecieveLoaction = x.ProjectRecieveLoaction,
                ProjectObserveMobile = x.ProjectObserveMobile,
                ProjectObserveMail = x.ProjectObserveMail,
                ProjectTaslemFirst = x.ProjectTaslemFirst,
                FDamanID = x.FDamanID,
                LDamanID = x.LDamanID,
                NesbaEngaz = x.NesbaEngaz,
                Takeem = x.Takeem,
                ProjectContractTawkCh = x.ProjectContractTawkCh,
                ProjectRecieveLoactionCh = x.ProjectRecieveLoactionCh,
                ProjectTaslemFirstCh = x.ProjectTaslemFirstCh,
                ContractCh = x.ContractCh,
                PeriodProject = x.PeriodProject,
                AgentDate = x.AgentDate,
                AgentHijriDate = x.AgentHijriDate,
                StreetName = x.StreetName,
                MainText = x.MainText,
                BranchText = x.BranchText,
                TaskText = x.TaskText,
                ProjectObserveName = x.ProjectObserveName,
                CityName = x.city!.NameAr,
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = Lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
                : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectMangerName = x.Users!.FullName,
                ProjectSubTypeName = Lang == "ltr" ? x.projectsubtype!.NameAr : x.projectsubtype!.NameAr,
                ProjectTypesName = Lang == "ltr" ? x.projecttype!.NameEn : x.projecttype!.NameAr,
                TransactionTypeName = Lang == "ltr" ? x.transactionTypes!.NameEn : x.transactionTypes!.NameAr,
                RegionTypeName = Lang == "ltr" ? x.regionTypes!.NameEn : x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                ActiveMainPhaseId = x.ActiveMainPhaseId,
                ActiveSubPhaseId = x.ActiveSubPhaseId,
                AddUser = x.AddUsers!.FullName,
                AddUserJob = x.AddUsers!.Jobs!.JobNameAr,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
                MunicipalId = x.MunicipalId ?? 0,
                SubMunicipalityId = x.SubMunicipalityId ?? 0,
                StopProjectType = x.StopProjectType ?? 0,
                Co_opOfficeName = x.Co_opOfficeName ?? "",
                Co_opOfficeEmail = x.Co_opOfficeEmail ?? "",
                Co_opOfficePhone = x.Co_opOfficePhone ?? "",
                ContractorSelectId = x.ContractorSelectId ?? 0,
                ContractorEmail_T = x.Contractor != null ? x.Contractor.Email : "",
                ContractorPhone_T = x.Contractor != null ? x.Contractor.PhoneNo : "",
                ContractorCom_T = x.Contractor != null ? x.Contractor.CommercialRegister : "",
                ProBuildingDisc = x.ProBuildingDisc ?? "",
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",

            }).FirstOrDefault();
            return pro;
        }



        public async Task<ProjectVM> GetProjectById(string Lang,int ProjectId)
        {
            var pro = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                CustomerId = x.CustomerId,
                ParentProjectId = x.ParentProjectId,
                MangerId = x.MangerId,
                TransactionTypeId = x.TransactionTypeId,
                ProjectDate = x.ProjectDate,
                ContractId = x.ContractId,
                ProjectHijriDate = x.ProjectHijriDate,
                ProjectExpireDate = x.ProjectExpireDate,
                ProjectExpireHijriDate = x.ProjectExpireHijriDate,
                SiteName = x.SiteName,
                ProjectTypeId = x.ProjectTypeId,
                SubProjectTypeId = x.SubProjectTypeId,
                OrderType = x.OrderType,
                SketchName = x.SketchName,
                SketchNo = x.SketchNo,
                PieceNo = x.PieceNo ?? 0,
                AdwAR = x.AdwAR,
                Status = x.Status,
                OrderNo = x.OrderNo,
                OutBoxNo = x.OutBoxNo,
                OutBoxDate = x.OutBoxDate,
                OutBoxHijriDate = x.OutBoxHijriDate,
                Reason1 = x.Reason1,
                Notes1 = x.Notes1,
                Subject = x.Subject,
                XPoint = x.XPoint,
                YPoint = x.YPoint,
                Technical = x.Technical,
                Prosedor = x.Prosedor,
                ReasonRevers = x.ReasonRevers,
                EngNotes = x.EngNotes,
                ReverseDate = x.ReverseDate,
                ReverseHijriDate = x.ReverseHijriDate,
                OrderStatus = x.OrderStatus,
                UserId = x.UserId,
                Receipt = x.Receipt,
                PayStatus = x.PayStatus,
                RegionName = x.RegionName,
                DistrictName = x.DistrictName,
                SiteType = x.SiteType,
                ContractNo = x.Contracts == null ? "" : x.Contracts!.ContractNo,
                ContractDate = x.Contracts == null ? "" : x.Contracts!.Date,
                ContractHijriDate = x.ContractHijriDate,
                ContractSource = x.ContractSource,
                SiteNo = x.SiteNo,
                PayanNo = x.PayanNo,
                JehaId = x.JehaId,
                ZaraaSak = x.ZaraaSak,
                ZaraaNatural = x.ZaraaNatural,
                BordersSak = x.BordersSak,
                BordersNatural = x.BordersNatural,
                Ertedad = x.Ertedad,
                Brooz = x.Brooz,
                AreaSak = x.AreaSak,
                AreaNatural = x.AreaNatural,
                AreaArrange = x.AreaArrange,
                BuildingType = x.BuildingType,
                BuildingPercent = x.BuildingPercent,
                SpaceName = x.SpaceName,
                Office = x.Office,
                Usage = x.Usage,
                Docpath = x.Docpath,
                RegionTypeId = x.RegionTypeId,
                elevators = x.elevators,
                typ1 = x.typ1,
                brozat = x.brozat,
                entries = x.entries,
                Basement = x.Basement,
                GroundFloor = x.GroundFloor,
                FirstFloor = x.FirstFloor,
                Motkrr = x.Motkrr,
                FirstExtension = x.FirstExtension,
                ExtensionName = x.ExtensionName,
                GeneralLocation = x.GeneralLocation,
                LicenseNo = x.LicenseNo,
                Licensedate = x.Licensedate,
                LicenseHijridate = x.LicenseHijridate,
                DesiningOffice = x.DesiningOffice,
                estsharyformoslhat = x.estsharyformoslhat,
                Consultantfinishing = x.Consultantfinishing,
                Period = x.Period,
                punshmentamount = x.punshmentamount,
                FirstPay = x.FirstPay,
                LicenseContent = x.LicenseContent,
                OtherStatus = x.OtherStatus,
                AreaSpace = x.AreaSpace,
                ContractorName = x.ContractorName,
                ContractorMobile = x.ContractorMobile,
                SupervisionSatartDate = x.SupervisionSatartDate,
                SupervisionSatartHijriDate = x.SupervisionSatartHijriDate,
                SupervisionEndDate = x.SupervisionEndDate,
                SupervisionEndHijriDate = x.SupervisionEndHijriDate,
                SupervisionNo = x.SupervisionNo,
                SupervisionNotes = x.SupervisionNotes,
                qaboqwaedmostlm = x.qaboqwaedmostlm,
                qaboreqabmostlm = x.qaboreqabmostlm,
                qabosaqfmostlm = x.qabosaqfmostlm,
                molhqalwisaqffash = x.molhqalwisaqffash,
                molhqalwisaqfdate = x.molhqalwisaqfdate,
                molhqalwisaqfHijridate = x.molhqalwisaqfHijridate,
                molhqalwisaqfmostlm = x.molhqalwisaqfmostlm,
                molhqardisaqffash = x.molhqardisaqffash,
                molhqardisaqfdate = x.molhqardisaqfdate,
                molhqardisaqfHijridate = x.molhqardisaqfHijridate,
                molhqardisaqfmostlm = x.molhqardisaqfmostlm,
                FinalOrder = x.FinalOrder,
                SpaceBuild = x.SpaceBuild,
                FloorEstablishing = x.FloorEstablishing,
                Roof = x.Roof,
                Electric = x.Electric,
                Takeef = x.Takeef,
                ProjectNo = x.ProjectNo,
                LimitDate = x.LimitDate,
                LimitHijriDate = x.LimitHijriDate,
                LimitDays = x.LimitDays,
                NoteDate = x.NoteDate,
                NoteHijriDate = x.NoteHijriDate,
                ResponseEng = x.ResponseEng,
                ReseveStatus = x.ReseveStatus,
                kaeedno = x.kaeedno,
                TechnicalDemands = x.TechnicalDemands,
                Todoaction = x.Todoaction,
                Responsible = x.Responsible,
                ExternalEmpId = x.ExternalEmpId,
                FinishDate = x.FinishDate,
                FinishHijriDate = x.FinishHijriDate,
                ContractPeriod = x.ContractPeriod,
                SpaceNotes = x.SpaceNotes,
                ContractNotes = x.ContractNotes,
                SpaceId = x.SpaceId,
                CityId = x.CityId,
                ProjectDescription = x.ProjectDescription,
                Paied = x.Paied,
                Discount = x.Discount,
                Fees = x.Fees,
                ProjectTypeName = x.ProjectTypeName,
                ProjectRegionName = x.ProjectRegionName,
                Catego = x.Catego,
                ContractPeriodType = x.ContractPeriodType,
                ContractPeriodMinites = x.ContractPeriodMinites,
                ProjectName = x.ProjectName,
                ProjectValue = x.ProjectValue,
                ProjectContractTawk = x.ProjectContractTawk,
                ProjectRecieveLoaction = x.ProjectRecieveLoaction,
                ProjectObserveMobile = x.ProjectObserveMobile,
                ProjectObserveMail = x.ProjectObserveMail,
                ProjectTaslemFirst = x.ProjectTaslemFirst,
                FDamanID = x.FDamanID,
                LDamanID = x.LDamanID,
                NesbaEngaz = x.NesbaEngaz,
                Takeem = x.Takeem,
                ProjectContractTawkCh = x.ProjectContractTawkCh,
                ProjectRecieveLoactionCh = x.ProjectRecieveLoactionCh,
                ProjectTaslemFirstCh = x.ProjectTaslemFirstCh,
                ContractCh = x.ContractCh,
                PeriodProject = x.PeriodProject,
                AgentDate = x.AgentDate,
                AgentHijriDate = x.AgentHijriDate,
                StreetName = x.StreetName,
                MainText = x.MainText,
                BranchText = x.BranchText,
                TaskText = x.TaskText,
                ProjectObserveName = x.ProjectObserveName,
                CityName = x.city!.NameAr,
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = Lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
                : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectMangerName = x.Users!.FullName,
                ProjectSubTypeName = Lang == "ltr" ? x.projectsubtype!.NameAr : x.projectsubtype!.NameAr,
                ProjectTypesName = Lang == "ltr" ? x.projecttype!.NameEn : x.projecttype!.NameAr,
                TransactionTypeName = Lang == "ltr" ? x.transactionTypes!.NameEn : x.transactionTypes!.NameAr,
                RegionTypeName = Lang == "ltr" ? x.regionTypes!.NameEn : x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                ActiveMainPhaseId = x.ActiveMainPhaseId,
                ActiveSubPhaseId = x.ActiveSubPhaseId,
                AddUser = Lang=="ltr"? x.AddUsers!.FullNameAr ==null? x.AddUsers!.FullName : x.AddUsers!.FullName : x.AddUsers!.FullName==null? x.AddUsers!.FullNameAr : x.AddUsers!.FullName,
                AddUserJob = x.AddUsers!.Jobs!.JobNameAr,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
                MunicipalId = x.MunicipalId ?? 0,
                SubMunicipalityId = x.SubMunicipalityId ?? 0,
                StopProjectType = x.StopProjectType ?? 0,
                Co_opOfficeName = x.Co_opOfficeName ?? "",
                Co_opOfficeEmail = x.Co_opOfficeEmail ?? "",
                Co_opOfficePhone = x.Co_opOfficePhone ?? "",
                ContractorSelectId = x.ContractorSelectId ?? 0,
                ContractorEmail_T = x.Contractor != null ? x.Contractor.Email : "",
                ContractorPhone_T = x.Contractor != null ? x.Contractor.PhoneNo : "",
                ContractorCom_T = x.Contractor != null ? x.Contractor.CommercialRegister : "",
                ProBuildingDisc = x.ProBuildingDisc ?? "",
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                TypeCode = x.projecttype!.TypeCode,
                Cons_components = x.Cons_components ?? "",
                TimeStr = (x.NoOfDays < 30) ? x.NoOfDays + " يوم " : (x.NoOfDays == 30) ? x.NoOfDays / 30 + " شهر " : (x.NoOfDays > 30) ? ((x.NoOfDays / 30) + " شهر " + (x.NoOfDays % 30) + " يوم  ") : "",
                AddedUserImg=x.AddUsers!.ImgUrl ?? "/distnew/images/userprofile.png",
                NewSetting=x.NewSetting??false,

            }).First();
            return pro;
        }
        public async Task<ProjectVM> GetProjectByIdSome(string Lang, int ProjectId)
        {
            var pro = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                CustomerId = x.CustomerId,
                MangerId = x.MangerId,
                ProjectNo = x.ProjectNo,
                ProjectDate = x.ProjectDate,
                ActiveMainPhaseId = x.ActiveMainPhaseId,
                ActiveSubPhaseId = x.ActiveSubPhaseId,
                TimeStr = (x.NoOfDays < 30) ? x.NoOfDays + " يوم " : (x.NoOfDays == 30) ? x.NoOfDays / 30 + " شهر " : (x.NoOfDays > 30) ? ((x.NoOfDays / 30) + " شهر " + (x.NoOfDays % 30) + " يوم  ") : "",
                NoOfDays=x.NoOfDays??0,
                ProjectExpireDate = x.ProjectExpireDate,
                StopProjectType = x.StopProjectType ?? 0,
                CustomerName = Lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
                : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,
                CustomerName_W = x.customer == null ? "" : Lang == "ltr" ? x.customer!.CustomerNameEn : x.customer!.CustomerNameAr,
                ProjectMangerName = x.Users!.FullName,
                TaskExecPercentage_Count = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false).Count(),
                TaskExecPercentage_Sum = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false).Sum(s => s.ExecPercentage) ?? 0,
                ProjectTypesName = x.projecttype == null ? "" : Lang == "ltr" ? x.projecttype!.NameEn : x.projecttype!.NameAr,
                ProjectDescription = x.ProjectDescription,
                lastEditUser = x.UpdateUsers!=null? x.UpdateUsers.FullName:x.AddUsers!=null? x.AddUsers.FullName:"",
                lastEditUserTime = x.UpdateDate != null?x.UpdateDate:x.AddDate??null,

            }).First();
            return pro;
        }

        public async Task<ProjectVM> GetCostCenterByProId(string Lang, int ProjectId)
        {
            var pro = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                CostCenterId = x.CostCenterId ?? 0, 
                CostCenterName=x.Costcenter!=null?x.Costcenter.NameAr:"",
                CostCenterCode = x.Costcenter != null ? x.Costcenter.Code : "",
            }).First();
            return pro;
        }

        public async Task<ProjectVM> GetProjectDataOffice(string Lang, int ProjectId)
        {
            var pro = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                Co_opOfficeName = x.Co_opOfficeName ?? "",
                Co_opOfficeEmail = x.Co_opOfficeEmail ?? "",
                Co_opOfficePhone = x.Co_opOfficePhone ?? "",
                ContractorSelectId = x.ContractorSelectId ?? 0,
                ContractorEmail_T = x.Contractor != null ? x.Contractor.Email : "",
                ContractorPhone_T = x.Contractor != null ? x.Contractor.PhoneNo : "",
                ContractorCom_T = x.Contractor != null ? x.Contractor.CommercialRegister : "",
                LicenseNo=x.LicenseNo??"0",
                Catego = x.Catego??"0",
                ProPieceNumber = x.PieceNo==null?"0":x.projectPieces!.PieceNo??"0",

                MunicipalId = x.MunicipalId ?? 0,
                MunicipalName = x.Municipal != null ? x.Municipal.NameAr : "",

                SubMunicipalityId = x.SubMunicipalityId ?? 0,
                SubMunicipalityName = x.SubMunicipality != null ? x.SubMunicipality.NameAr : "",
                BuildingType=x.BuildingType??0,
                DistrictName=x.DistrictName??"",
                AdwAR=x.AdwAR??0,
                ProBuildingDisc = x.ProBuildingDisc ?? "",

            }).First();
            return pro;
        }


        public async Task<ProjectVM> GetProjectByIdStopType(string Lang, int ProjectId)
        {
            var pro = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                StopProjectType = x.StopProjectType ?? 0,


            }).FirstOrDefault();
            return pro;
        }

        public async Task<ProjectVM> GetProjectAddUser(int ProjectId)
        {
            var pro = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectId).Select(x => new ProjectVM
            {
                //dawoud
                ProjectId = x.ProjectId,
                CustomerId = x.CustomerId,
                MangerId = x.MangerId,
                AddUser = x.AddUser.ToString(),
                TimeStr = (x.NoOfDays < 30) ? x.NoOfDays + " يوم " : (x.NoOfDays == 30) ? x.NoOfDays / 30 + " شهر " : (x.NoOfDays > 30) ? ((x.NoOfDays / 30) + " شهر " + (x.NoOfDays % 30) + " يوم  ") : "",
                NoOfDays=x.NoOfDays??0,
                ProjectNo=x.ProjectNo??""

            }).First();
            return pro;
        }

        public async Task< IEnumerable<ProjectVM>> GetProjectsByCustomerId(int? CustomerId, int? Status)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && (CustomerId ==null|| s.CustomerId == CustomerId) && (Status==null||s.Status == Status)).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeId = x.ProjectTypeId,
                SubProjectTypeId = x.SubProjectTypeId,
                GeneralLocation = x.city!.NameAr,
                CityName = x.city!.NameAr,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                DateOfFinish = x.DateOfFinish,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                StopProjectType = x.StopProjectType ?? 0,


            });
            return projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetSomeDataByProjId(int? ProjectId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && s.OffersPricesId != null).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                OffersPricesId = x.OffersPricesId ?? 0,
                OfferPriceNoName =" عرض سعر رقم  " + x.OffersPrices!.OfferNo,
            });
            return projects;
        }

        public async Task<int> GetMaxId()
        {
            return (_TaamerProContext.Project.Count() > 0) ? _TaamerProContext.Project.Max(s => s.ProjectId) : 0;
        }
        public async Task< IEnumerable<ProjectVM>> GetProjectsSearch(ProjectVM ProjectsSearch, int BranchId, string Con, string lang)
        {
            try
            {
                var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && s.BranchId == BranchId && (s.ProjectTypeId == ProjectsSearch.ProjectTypeId || ProjectsSearch.ProjectTypeId == null) &&
                                               (s.SubProjectTypeId == ProjectsSearch.SubProjectTypeId || ProjectsSearch.SubProjectTypeId == null) &&
                                               (s.CustomerId == ProjectsSearch.CustomerId || ProjectsSearch.CustomerId == null) &&
                                               (s.ProjectNo!.Equals(ProjectsSearch.ProjectNo) || ProjectsSearch.ProjectNo == null) &&
                                               (s.MangerId == ProjectsSearch.MangerId || ProjectsSearch.MangerId == null) &&
                                               (s.Contracts!.ContractNo == ProjectsSearch.ContractNo || ProjectsSearch.ContractNo == null) &&
                                               (s.customer!.CustomerNationalId!.Equals(ProjectsSearch.NationalNumber) || ProjectsSearch.NationalNumber == null || s.customer!.CustomerNationalId!.Contains(ProjectsSearch.NationalNumber)) &&
                                               (s.customer!.CustomerMobile!.Equals(ProjectsSearch.Mobile) || ProjectsSearch.Mobile == null || s.customer!.CustomerMobile!.Contains(ProjectsSearch.Mobile)) &&
                                               (s.city!.CityId == ProjectsSearch.CityId || ProjectsSearch.CityId == null) &&
                                               (s.DepartmentId == ProjectsSearch.DepartmentId || ProjectsSearch.DepartmentId == null) &&
                                               (s.ProjectDescription!.Equals(ProjectsSearch.ProjectDescription) || ProjectsSearch.ProjectDescription == null || s.ProjectDescription!.Contains(ProjectsSearch.ProjectDescription)) &&
                                               (s.PieceNo.Equals(ProjectsSearch.PieceNo) || ProjectsSearch.PieceNo == null))
                                               .Select(x => new ProjectVM
                                               {
                                                   ProjectId = x.ProjectId,
                                                   ProjectName = x.ProjectName,
                                                   ProjectNo = x.ProjectNo,
                                                   CustomerId = x.CustomerId,
                                                   ProjectTypeName = x.ProjectTypeName,
                                                   CityId = x.CityId,
                                                   ContractNo = x.Contracts!.ContractNo,
                                                   ContractId = x.ContractId,
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
                                                   CustomerName_W = x.customer!.CustomerNameAr,
                                                   CustomerName = lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
               : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                                                   ProjectTypesName = lang == "ltr" ? x.projecttype!.NameEn : x.projecttype!.NameAr,
                                                   ProjectSubTypeName = lang == "ltr" ? x.projectsubtype!.NameEn : x.projectsubtype!.NameAr,
                                                   ProjectMangerName = lang == "ltr" ? x.Users!.FullName : x.Users!.FullName,
                                                   CityName = lang == "ltr" ? x.city!.NameEn : x.city!.NameAr,
                                                   TransactionTypeName = x.transactionTypes!.NameAr,
                                                   RegionTypeId = x.RegionTypeId,
                                                   RegionTypeName = x.regionTypes!.NameAr,
                                                   FileCount = x.ProjectFiles!.Count(),
                                                   ExpectedTime = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.TimeMinutes) ?? 0,
                                                   CurrentMainPhase = lang == "ltr" ? x.ActiveMainPhase!.DescriptionEn : x.ActiveMainPhase!.DescriptionAr,
                                                   CurrentSubPhase = lang == "ltr" ? x.ActiveSubPhase!.DescriptionEn : x.ActiveSubPhase!.DescriptionAr,

                                                   Cost = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0,
                                                   Income = 0,
                                                   BranchId = x.BranchId,
                                                   CostCenterId = x.CostCenterId ?? 0,

                                                   StopProjectType = x.StopProjectType ?? 0,

                                                   TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                                                   FirstProjectDate = x.FirstProjectDate,
                                                   FirstProjectExpireDate = x.FirstProjectExpireDate,
                                                   TaskExecPercentage_Count = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false).Count(),
                                                   TaskExecPercentage_Sum = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false).Sum(s => s.ExecPercentage) ?? 0,
                                                   ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                                                   CostE = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,
                                                   CostE_Credit = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                                                   CostE_Depit = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                                                   CostS = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,

                                                   CostE_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                                                   CostE_Credit_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                                                   CostE_Depit_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                                                   CostS_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,



                                                   Oper_expeValue = x.Contracts != null ? x.Contracts!.Oper_expeValue ?? 0 : 0,
                                                   DepartmentId = x.DepartmentId,
                                                   MotionProject = x.MotionProject,
                                                   MotionProjectDate = x.MotionProjectDate,
                                                   MotionProjectNote = x.MotionProjectNote ?? "",
                                                   Cons_components = x.Cons_components ?? "",

                                               });
                return projects; ;
            }
            catch (Exception ex)
            {
                var pro = new List<ProjectVM>();
                return pro;
            }
            //&& s.BranchId == BranchId
           
        }
        public async Task<IEnumerable<object>>FillAllUsersByProject(int BranchId, string lang)
        {
            //&& s.BranchId == BranchId
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && s.BranchId == BranchId && s.Users.UserId == s.MangerId)
                                                .Select(x => new 
                                                {
                                                    Id = x.Users!.UserId,
                                                    Name = lang == "ltr" ? x.Users!.FullName : x.Users!.FullName

                                                }).Distinct();
            return projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetAllProjectsByDateSearch(string DateFrom, string DateTo, int BranchId)
        {
            var Projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && s.BranchId == BranchId).Select(x => new
            {
                x.ProjectId,
                x.ProjectName,
                x.ProjectNo,
                x.CustomerId,
                x.ProjectTypeName,
                x.CityId,
                x.Contracts!.ContractNo,
                x.ContractorName,
                x.ProjectDescription,
                x.ProjectDate,
                x.ProjectHijriDate,
                x.ProjectTypeId,
                x.MangerId,
                x.ParentProjectId,
                x.BuildingType,
                x.SubProjectTypeId,
                x.TransactionTypeId,
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FileCount = x.ProjectFiles!.Count(),
                TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                ContractId = x.ContractId,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                StopProjectType = x.StopProjectType ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",


                Cost = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0,
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",
                CostE = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostE_Credit = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostE_Depit = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.TotalValue) ?? 0,
                CostS = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.TotalValue) ?? 0,

                CostE_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 2 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostE_Credit_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 29 && s.Invoices_Credit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostE_Depit_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 30 && s.Invoices_Depit!.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                CostS_W = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost == true && s.Type == 5 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
                ProjectTaskExist = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status != 4).Count() != 0 ? "نعم" : "لا",
                ProjectInvoiceExist = x.Invoices!.Where(s => s.IsDeleted == false && s.Rad == false).Count() != 0 ? "نعم" : "لا",


                Oper_expeValue = x.Contracts != null ? x.Contracts!.Oper_expeValue ?? 0 : 0,

            }).Select(m => new ProjectVM
            {
                ProjectId = m.ProjectId,
                ProjectName = m.ProjectName,
                ProjectNo = m.ProjectNo,
                CustomerId = m.CustomerId,
                ProjectTypeName = m.ProjectTypeName,
                CityId = m.CityId,
                ContractNo = m.ContractNo,
                ContractorName = m.ContractorName,
                ProjectDescription = m.ProjectDescription,
                ProjectDate = m.ProjectDate,
                ProjectHijriDate = m.ProjectHijriDate,
                ProjectTypeId = m.ProjectTypeId,
                MangerId = m.MangerId,
                ParentProjectId = m.ParentProjectId,
                BuildingType = m.BuildingType,
                SubProjectTypeId = m.SubProjectTypeId,
                TransactionTypeId = m.TransactionTypeId,
                CustomerName = m.CustomerName,
                CustomerName_W=m.CustomerName_W,
                ProjectTypesName = m.ProjectTypesName,
                ProjectSubTypeName = m.ProjectSubTypeName,
                ProjectMangerName = m.ProjectMangerName,
                CityName = m.CityName,
                TransactionTypeName = m.TransactionTypeName,
                RegionTypeId = m.RegionTypeId,
                RegionTypeName = m.RegionTypeName,
                ExpectedTime = m.ExpectedTime,
                FileCount = m.FileCount,
                FirstProjectDate = m.FirstProjectDate,
                FirstProjectExpireDate = m.FirstProjectExpireDate,
                CurrentMainPhase = m.CurrentMainPhase,
                CurrentSubPhase = m.CurrentSubPhase,
                Cost = m.Cost,
                ContractId = m.ContractId,
                BranchId = m.BranchId,
                CostCenterId = m.CostCenterId,

                StopProjectType = m.StopProjectType,
                ContractValue = m.ContractValue,
                DepartmentId = m.DepartmentId,
                MotionProject = m.MotionProject,
                MotionProjectDate = m.MotionProjectDate,
                MotionProjectNote = m.MotionProjectNote ?? "",
                Cons_components = m.Cons_components ?? "",
                CostE = m.CostE,
                CostE_Credit = m.CostE_Credit,
                CostE_Depit = m.CostE_Depit,
                CostS = m.CostS,
                CostE_W = m.CostE_W,
                CostE_Credit_W = m.CostE_Credit_W,
                CostE_Depit_W = m.CostE_Depit_W,
                CostS_W = m.CostS_W,
                ProjectTaskExist=m.ProjectTaskExist,
                ProjectInvoiceExist = m.ProjectInvoiceExist,
                Oper_expeValue = m.Oper_expeValue,

                TimeStr = m.TimeStr
            }).ToList().Where(s => DateTime.ParseExact(s.ProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.ProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))
            ;
            return Projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetArchiveProjectsSearch(ProjectVM ProjectsSearch, int BranchId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 1 && s.BranchId == BranchId && (s.ProjectTypeId == ProjectsSearch.ProjectTypeId || ProjectsSearch.ProjectTypeId == null) &&
                                                (s.SubProjectTypeId == ProjectsSearch.SubProjectTypeId || ProjectsSearch.SubProjectTypeId == null) &&
                                                (s.CustomerId == ProjectsSearch.CustomerId || ProjectsSearch.CustomerId == null) &&
                                                (s.ProjectNo!.Equals(ProjectsSearch.ProjectNo) || ProjectsSearch.ProjectNo == null) &&
                                                (s.MangerId == ProjectsSearch.MangerId || ProjectsSearch.MangerId == null) &&
                                                (s.Contracts!.ContractNo == ProjectsSearch.ContractNo || ProjectsSearch.ContractNo == null) &&
                                                (s.customer!.CustomerNationalId!.Equals(ProjectsSearch.NationalNumber) || ProjectsSearch.NationalNumber == null || s.customer!.CustomerNationalId!.Contains(ProjectsSearch.NationalNumber)) &&
                                                (s.customer!.CustomerMobile!.Equals(ProjectsSearch.Mobile) || ProjectsSearch.Mobile == null || s.customer!.CustomerMobile!.Contains(ProjectsSearch.Mobile)) &&
                                                (s.city!.CityId == ProjectsSearch.CityId || ProjectsSearch.CityId == null) &&
                                                (s.DepartmentId == ProjectsSearch.DepartmentId || ProjectsSearch.DepartmentId == null) &&
                                                (s.ProjectDescription!.Equals(ProjectsSearch.ProjectDescription) || ProjectsSearch.ProjectDescription == null || s.ProjectDescription!.Contains(ProjectsSearch.ProjectDescription)))
                                                .Select(x => new ProjectVM
                                                {



                                                    ProjectId = x.ProjectId,
                                                    ProjectName = x.ProjectName,
                                                    ProjectNo = x.ProjectNo,
                                                    CustomerId = x.CustomerId,
                                                    ProjectTypeName = x.ProjectTypeName,
                                                    CityId = x.CityId,
                                                    ContractNo = x.Contracts == null ? "" : x.Contracts!.ContractNo,
                                                    ContractId = x.ContractId,
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
                                                    CustomerName_W = x.customer!.CustomerNameAr,
                                                    CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,
                                                    ProjectTypesName = x.projecttype == null ? "" : x.projecttype!.NameAr,
                                                    ProjectSubTypeName = x.projectsubtype == null ? "" : x.projectsubtype!.NameAr,
                                                    ProjectMangerName = x.Users == null ? "" : x.Users!.FullName,
                                                    NationalNumber = x.customer == null ? "" : x.customer!.CustomerNationalId,
                                                    Mobile = x.customer == null ? "" : x.customer!.CustomerMobile,
                                                    CityName = x.city == null ? "" : x.city!.NameAr,
                                                    TransactionTypeName = x.transactionTypes == null ? "" : x.transactionTypes!.NameAr,
                                                    RegionTypeId = x.RegionTypeId,
                                                    RegionTypeName = x.regionTypes == null ? "" : x.regionTypes!.NameAr,
                                                    //FileCount = x.ProjectFiles == null ? 0 : x.ProjectFiles!.Count(),
                                                    FileCount = 0,
                                                    FirstProjectExpireDate = x.FirstProjectExpireDate,
                                                    FirstProjectDate = x.FirstProjectDate,
                                                    TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                                                    ExpectedTime = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.TimeMinutes) ?? 0,
                                                    CurrentMainPhase = x.ActiveMainPhase == null ? "" : x.ActiveMainPhase!.DescriptionAr,
                                                    CurrentSubPhase = x.ActiveSubPhase == null ? "" : x.ActiveSubPhase!.DescriptionAr,
                                                    FinishDate = x.FinishDate,
                                                    ReasonText = x.ReasonText,
                                                    ReasonID = x.ReasonID,
                                                    UpdateUser = x.UpdateUsers == null ? "" : x.UpdateUsers!.FullName,
                                                    BranchId = x.BranchId,
                                                    CostCenterId = x.CostCenterId ?? 0,

                                                    StopProjectType = x.StopProjectType ?? 0,

                                                    DateOfFinish = x.DateOfFinish,
                                                    DepartmentId = x.DepartmentId,
                                                    MotionProject = x.MotionProject,
                                                    MotionProjectDate = x.MotionProjectDate,
                                                    MotionProjectNote = x.MotionProjectNote ?? "",
                                                    Cons_components = x.Cons_components ?? "",


                                                }).ToList();
            return projects; ;
        }
        public async Task< IEnumerable<ProjectVM>> GetAllArchiveProjectsByDateSearch(string DateFrom, string DateTo, int BranchId)
        {
            var Projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 1 && s.BranchId == BranchId).Select(x => new
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts == null ? "" : x.Contracts!.ContractNo,
                ContractId = x.ContractId,
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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype == null ? "" : x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype == null ? "" : x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users == null ? "" : x.Users!.FullName,
                NationalNumber = x.customer == null ? "" : x.customer!.CustomerNationalId,
                Mobile = x.customer == null ? "" : x.customer!.CustomerMobile,
                CityName = x.city == null ? "" : x.city!.NameAr,
                TransactionTypeName = x.transactionTypes == null ? "" : x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes == null ? "" : x.regionTypes!.NameAr,
                //FileCount = x.ProjectFiles == null ? 0 : x.ProjectFiles!.Count(),
                FileCount = 0,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FirstProjectDate = x.FirstProjectDate,
                TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                ExpectedTime = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.TimeMinutes) ?? 0,
                CurrentMainPhase = x.ActiveMainPhase == null ? "" : x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase == null ? "" : x.ActiveSubPhase!.DescriptionAr,
                FinishDate = x.FinishDate,
                ReasonText = x.ReasonText,
                ReasonID = x.ReasonID,
                UpdateUser = x.UpdateUsers == null ? "" : x.UpdateUsers!.FullName,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                StopProjectType = x.StopProjectType ?? 0,

                DateOfFinish = x.DateOfFinish,
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",

            }).Select(m => new ProjectVM
            {
                ProjectId = m.ProjectId,
                ProjectName = m.ProjectName,
                ProjectNo = m.ProjectNo,
                CustomerId = m.CustomerId,
                ProjectTypeName = m.ProjectTypeName,
                CityId = m.CityId,
                ContractNo = m.ContractNo,
                ContractorName = m.ContractorName,
                ProjectDescription = m.ProjectDescription,
                ProjectDate = m.ProjectDate,
                ProjectHijriDate = m.ProjectHijriDate,
                ProjectTypeId = m.ProjectTypeId,
                MangerId = m.MangerId,
                ParentProjectId = m.ParentProjectId,
                BuildingType = m.BuildingType,
                SubProjectTypeId = m.SubProjectTypeId,
                TransactionTypeId = m.TransactionTypeId,
                CustomerName = m.CustomerName,
                CustomerName_W=m.CustomerName_W,
                ProjectTypesName = m.ProjectTypesName,
                ProjectSubTypeName = m.ProjectSubTypeName,
                ProjectMangerName = m.ProjectMangerName,
                CityName = m.CityName,
                TransactionTypeName = m.TransactionTypeName,
                RegionTypeId = m.RegionTypeId,
                RegionTypeName = m.RegionTypeName,
                ExpectedTime = m.ExpectedTime,
                FileCount = m.FileCount,
                FirstProjectDate = m.FirstProjectDate,
                FirstProjectExpireDate = m.FirstProjectExpireDate,
                BranchId = m.BranchId,
                CostCenterId = m.CostCenterId,

                CurrentMainPhase = m.CurrentMainPhase,
                CurrentSubPhase = m.CurrentSubPhase,
                UpdateUser = m.UpdateUser,
                ContractId = m.ContractId,
                DateOfFinish = m.DateOfFinish,
                FinishDate = m.FinishDate,
                ReasonText = m.ReasonText,
                ReasonID = m.ReasonID,
                StopProjectType = m.StopProjectType,
                DepartmentId = m.DepartmentId,
                MotionProject = m.MotionProject,
                MotionProjectDate = m.MotionProjectDate,
                MotionProjectNote = m.MotionProjectNote ?? "",
                Cons_components = m.Cons_components ?? "",


                TimeStr = m.TimeStr
            }).ToList().Where(s => DateTime.ParseExact(s.ProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.ProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))
            ;
            return Projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetUserProjects(int UserId, int BranchId, string DateNow)
        {

            if (DateNow == "")
            {
                var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && (s.ProjectWorkers.Where(i => i.IsDeleted == false).Select(x => x.UserId).Contains(UserId))).Select(x => new ProjectVM
                {
                    ProjectId = x.ProjectId,
                    ProjectName = x.ProjectName,
                    ProjectNo = x.ProjectNo,
                    CustomerId = x.CustomerId,
                    ProjectTypeName = x.ProjectTypeName,
                    CityId = x.CityId,
                    ContractNo = x.Contracts!.ContractNo,
                    ContractId = x.ContractId,

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
                    CustomerName_W = x.customer!.CustomerNameAr,
                    CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                    ProjectTypesName = x.projecttype!.NameAr,
                    ProjectSubTypeName = x.projectsubtype!.NameAr,
                    ProjectMangerName = x.Users!.FullName,
                    CityName = x.city!.NameAr,
                    TransactionTypeName = x.transactionTypes!.NameAr,
                    RegionTypeId = x.RegionTypeId,
                    RegionTypeName = x.regionTypes!.NameAr,
                    FileCount = x.ProjectFiles!.Count(),
                    FirstProjectDate = x.FirstProjectDate,
                    FirstProjectExpireDate = x.FirstProjectExpireDate,
                    BranchId = x.BranchId,
                    CostCenterId = x.CostCenterId ?? 0,

                    TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                    ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                    CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                    CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                    PercentComplete = x.PercentComplete,
                    StopProjectType = x.StopProjectType ?? 0,
                    ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                    DepartmentId = x.DepartmentId,
                    MotionProject = x.MotionProject,
                    MotionProjectDate = x.MotionProjectDate,
                    MotionProjectNote = x.MotionProjectNote ?? "",
                    Cons_components = x.Cons_components ?? "",

                    //TaskNotStartedCount = x.ProjectPhasesTasks!.Where(m => m.Status == 1 && m.UserId == UserId).Count(),
                    //TaskNotStarted = x.ProjectPhasesTasks!.Where(m => m.Status == 1 && m.UserId == UserId).Sum(s => s.PercentComplete) / x.ProjectPhasesTasks!.Where(m => m.Status == 1 && m.UserId == UserId).Count() ?? 0,
                    //TaskInProgress = x.ProjectPhasesTasks!.Where(m => m.Status == 2 && m.UserId == UserId).Sum(s => s.PercentComplete) / x.ProjectPhasesTasks!.Where(m => m.Status == 2 && m.UserId == UserId).Count() ?? 0,
                    //TaskDone = x.ProjectPhasesTasks!.Where(m => m.Status == 4 && m.UserId == UserId).Sum(s => s.PercentComplete) / x.ProjectPhasesTasks!.Where(m => m.Status == 4 && m.UserId == UserId).Count() ?? 0,
                    //TaskLate = x.ProjectPhasesTasks!.Where(m => DateTime.ParseExact(m.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture) && m.UserId == UserId).Sum(s => s.PercentComplete) / x.ProjectPhasesTasks!.Where(m => DateTime.ParseExact(m.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture) && m.UserId == UserId).Count() ?? 0,
                    //TaskNotStarted = x.ProjectPhasesTasks!.Where(m => m.Status == 4 && m.UserId == UserId).Sum(s => s.TimeMinutes) ?? 0,
                });
                return projects;
            }
            else
            {
                var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && (s.ProjectWorkers.Where(i => i.IsDeleted == false).Select(x => x.UserId).Contains(UserId))).Select(x => new ProjectVM
                {
                    ProjectId = x.ProjectId,
                    ProjectName = x.ProjectName,
                    ProjectNo = x.ProjectNo,
                    CustomerId = x.CustomerId,
                    ProjectTypeName = x.ProjectTypeName,
                    CityId = x.CityId,
                    ContractNo = x.Contracts!.ContractNo,
                    ContractId = x.ContractId,
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
                    CustomerName_W = x.customer!.CustomerNameAr,
                    CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                    ProjectTypesName = x.projecttype!.NameAr,
                    ProjectSubTypeName = x.projectsubtype!.NameAr,
                    ProjectMangerName = x.Users!.FullName,
                    CityName = x.city!.NameAr,
                    TransactionTypeName = x.transactionTypes!.NameAr,
                    RegionTypeId = x.RegionTypeId,
                    RegionTypeName = x.regionTypes!.NameAr,
                    FileCount = x.ProjectFiles!.Count(),
                    FirstProjectDate = x.FirstProjectDate,
                    FirstProjectExpireDate = x.FirstProjectExpireDate,
                    BranchId = x.BranchId,
                    CostCenterId = x.CostCenterId ?? 0,

                    TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                    ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                    CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                    CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                    //EDate = Convert.ToDateTime(x.ProjectPhasesTasks!.Where(m => m.UserId == UserId).Select(s => s.EndDate)),
                    //TaskNotStartedCount = x.ProjectPhasesTasks!.Where(m => m.Status == 1 && m.UserId == UserId).Count(),
                    TaskNotStarted = x.ProjectPhasesTasks!.Where(m => m.Status == 1 && m.UserId == UserId).Sum(s => s.PercentComplete) / x.ProjectPhasesTasks!.Where(m => m.Status == 1 && m.UserId == UserId).Count() ?? 0,
                    TaskInProgress = x.ProjectPhasesTasks!.Where(m => m.Status == 2 && m.UserId == UserId).Sum(s => s.PercentComplete) / x.ProjectPhasesTasks!.Where(m => m.Status == 2 && m.UserId == UserId).Count() ?? 0,
                    TaskDone = x.ProjectPhasesTasks!.Where(m => m.Status == 4 && m.UserId == UserId).Sum(s => s.PercentComplete) / x.ProjectPhasesTasks!.Where(m => m.Status == 4 && m.UserId == UserId).Count() ?? 0,
                    DepartmentId = x.DepartmentId,
                    MotionProject = x.MotionProject,
                    MotionProjectDate = x.MotionProjectDate,
                    MotionProjectNote = x.MotionProjectNote ?? "",
                    Cons_components = x.Cons_components ?? "",

                    //TaskLate = x.ProjectPhasesTasks!.Select(m => m.EDate >= Convert.ToDateTime(DateNow) && m.UserId == UserId).Sum(s => s.PercentComplete) / x.ProjectPhasesTasks!.Where(m => m.UserId == UserId).Count() ?? 0,//.Sum(s => s.PercentComplete) ?? 0,// / x.ProjectPhasesTasks!.Where(m => (DateTime.ParseExact(m.EndDate ?? DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && m.UserId == UserId    //.Where(m => (DateTime.ParseExact(m.EndDate != null ? m.EndDate : DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && m.UserId == UserId).Sum(s => s.PercentComplete) ?? 0,// / x.ProjectPhasesTasks!.Where(m => (DateTime.ParseExact(m.EndDate ?? DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && m.UserId == UserId).Count() ?? 0,
                    //TaskNotStarted = x.ProjectPhasesTasks!.Where(m => m.Status == 4 && m.UserId == UserId).Sum(s => s.TimeMinutes) ?? 0,
                });
                return projects;
            }
        }

        public async Task< IEnumerable<ProjectVM>> GetUserProjects2(int UserId, int BranchId, string DateNow)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && s.MangerId == UserId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,
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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                NationalNumber = x.customer!.CustomerNationalId,
                Mobile = x.customer!.CustomerMobile,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FirstProjectDate = x.FirstProjectDate,
                TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                FinishDate = x.FinishDate,
                ReasonText = x.ReasonText,
                ReasonID = x.ReasonID,
                UpdateUser = x.UpdateUsers!.FullName,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                StopProjectType = x.StopProjectType ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",

                DateOfFinish = x.DateOfFinish,
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",

            });
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetUserProjectsReport(int UserId, int BranchId, string DateNow)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false &&s.BranchId==BranchId && s.Status == 0 && s.MangerId == UserId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,

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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                NationalNumber = x.customer!.CustomerNationalId,
                Mobile = x.customer!.CustomerMobile,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FirstProjectDate = x.FirstProjectDate,
                //TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                TimeStr = (x.NoOfDays < 30) ? x.NoOfDays + " يوم " : (x.NoOfDays == 30) ? x.NoOfDays / 30 + " شهر " : (x.NoOfDays > 30) ? ((x.NoOfDays / 30) + " شهر " + (x.NoOfDays % 30) + " يوم  ") : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                FinishDate = x.FinishDate,
                ReasonText = x.ReasonText,
                ReasonID = x.ReasonID,
                UpdateUser = x.UpdateUsers!.FullName,
                DateOfFinish = x.DateOfFinish,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                StopProjectType = x.StopProjectType ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",

                TaskExecPercentage_Count = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Count(),
                WorkOrder_Count = x.WorkOrders!.Where(w => w.IsDeleted == false).Count(),
                WorkOrder_Sum = x.WorkOrders!.Where(w => w.IsDeleted == false).AsEnumerable().Sum(w => w.PercentComplete) ?? 0,

                TaskExecPercentage_Sum = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.ExecPercentage) ?? 0,

                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",

            });
            return projects;


        }

        public async Task< IEnumerable<ProjectVM>> GetUserProjectsReport(int? UserId, int? CustomerId, int BranchId, string DateFrom, string DateTo, List<int> BranchesList)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && BranchesList.Contains(s.BranchId) && s.Status == 0 && (UserId == null || UserId == 0 || s.MangerId == UserId) && (CustomerId == null || CustomerId == 0 || s.CustomerId == CustomerId)
            ).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,

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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                NationalNumber = x.customer!.CustomerNationalId,
                Mobile = x.customer!.CustomerMobile,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FirstProjectDate = x.FirstProjectDate,
                //TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                TimeStr = (x.NoOfDays < 30) ? x.NoOfDays + " يوم " : (x.NoOfDays == 30) ? x.NoOfDays / 30 + " شهر " : (x.NoOfDays > 30) ? ((x.NoOfDays / 30) + " شهر " + (x.NoOfDays % 30) + " يوم  ") : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                FinishDate = x.FinishDate,
                ReasonText = x.ReasonText,
                ReasonID = x.ReasonID,
                UpdateUser = x.UpdateUsers!.FullName,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
                ProjectExpireDate=x.ProjectExpireDate,

                StopProjectType = x.StopProjectType ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",

                DateOfFinish = x.DateOfFinish,
                TaskExecPercentage_Count = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Count(),
                WorkOrder_Count = x.WorkOrders!.Where(w => w.IsDeleted == false).Count(),
                WorkOrder_Sum = x.WorkOrders!.Where(w => w.IsDeleted == false).AsEnumerable().Sum(w => w.PercentComplete) ?? 0,
                TaskExecPercentage_Sum = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.ExecPercentage) ?? 0,
            }).ToList().Where(s => ((string.IsNullOrEmpty(DateFrom) || DateTime.ParseExact(s.ProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                && ((string.IsNullOrEmpty(DateTo) || DateTime.ParseExact(s.ProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))))).ToList();

            return projects;


        }

        public async Task<IEnumerable<ProjectVM>> GetUserProjectsReport(int? UserId, int? CustomerId, int BranchId, string DateFrom, string DateTo,string? SearchText)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == 0 && (UserId == null || UserId == 0 || s.MangerId == UserId) && (CustomerId == null || CustomerId == 0 || s.CustomerId == CustomerId)
           &&(s.ProjectDescription.Contains(SearchText) || s.customer.CustomerNameAr.Contains(SearchText)||s.ProjectNo.Contains(SearchText)
           || s.ProjectTypeName.Contains(SearchText) || s.projectsubtype.NameAr.Contains(SearchText) || SearchText ==null || SearchText =="")).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,

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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                NationalNumber = x.customer!.CustomerNationalId,
                Mobile = x.customer!.CustomerMobile,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FirstProjectDate = x.FirstProjectDate,
                //TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                TimeStr = (x.NoOfDays < 30) ? x.NoOfDays + " يوم " : (x.NoOfDays == 30) ? x.NoOfDays / 30 + " شهر " : (x.NoOfDays > 30) ? ((x.NoOfDays / 30) + " شهر " + (x.NoOfDays % 30) + " يوم  ") : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                FinishDate = x.FinishDate,
                ReasonText = x.ReasonText,
                ReasonID = x.ReasonID,
                UpdateUser = x.UpdateUsers!.FullName,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
                ProjectExpireDate = x.ProjectExpireDate,

                StopProjectType = x.StopProjectType ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                Cons_components = x.Cons_components ?? "",

                DateOfFinish = x.DateOfFinish,
                TaskExecPercentage_Count = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Count(),
                WorkOrder_Count = x.WorkOrders!.Where(w => w.IsDeleted == false).Count(),
                WorkOrder_Sum = x.WorkOrders!.Where(w => w.IsDeleted == false).AsEnumerable().Sum(w => w.PercentComplete) ?? 0,
                TaskExecPercentage_Sum = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.ExecPercentage) ?? 0,
            }).ToList().Where(s => ((string.IsNullOrEmpty(DateFrom) || DateTime.ParseExact(s.ProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                && ((string.IsNullOrEmpty(DateTo) || DateTime.ParseExact(s.ProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))))).ToList();

            return projects;


        }

        public async Task< IEnumerable<ProjectVM>> GetUserProjectsReportW(int BranchId, string DateNow)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId==BranchId && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,
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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                NationalNumber = x.customer!.CustomerNationalId,
                Mobile = x.customer!.CustomerMobile,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FirstProjectDate = x.FirstProjectDate,
                //TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                TimeStr = (x.NoOfDays < 30) ? x.NoOfDays + " يوم " : (x.NoOfDays == 30) ? x.NoOfDays / 30 + " شهر " : (x.NoOfDays > 30) ? ((x.NoOfDays / 30) + " شهر " + (x.NoOfDays % 30) + " يوم  ") : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                FinishDate = x.FinishDate,
                ReasonText = x.ReasonText,
                ReasonID = x.ReasonID,
                UpdateUser = x.UpdateUsers!.FullName,
                DateOfFinish = x.DateOfFinish,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
                ProjectExpireDate=x.ProjectExpireDate,

                StopProjectType = x.StopProjectType ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                TaskExecPercentage_Count = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Count(),
                WorkOrder_Count= x.WorkOrders!.Where(w => w.IsDeleted == false).Count(),
                WorkOrder_Sum= x.WorkOrders!.Where(w => w.IsDeleted == false).AsEnumerable().Sum(w => w.PercentComplete) ?? 0,

                TaskExecPercentage_Sum = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.ExecPercentage) ?? 0 ,
                Cons_components = x.Cons_components ?? "",

            });
            return projects;


        }
        public async Task< IEnumerable<ProjectVM>> GetUserProjectsReportW2(int BranchId, string DateFrom, string DateTo)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId==BranchId && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,

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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                NationalNumber = x.customer!.CustomerNationalId,
                Mobile = x.customer!.CustomerMobile,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FirstProjectDate = x.FirstProjectDate,
                //TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                TimeStr = (x.NoOfDays < 30) ? x.NoOfDays + " يوم " : (x.NoOfDays == 30) ? x.NoOfDays / 30 + " شهر " : (x.NoOfDays > 30) ? ((x.NoOfDays / 30) + " شهر " + (x.NoOfDays % 30) + " يوم  ") : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                FinishDate = x.FinishDate,
                ReasonText = x.ReasonText,
                ReasonID = x.ReasonID,
                UpdateUser = x.UpdateUsers!.FullName,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
                ProjectExpireDate=x.ProjectExpireDate,
                StopProjectType = x.StopProjectType ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                DateOfFinish = x.DateOfFinish,
                TaskExecPercentage_Count = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Count(),
                TaskExecPercentage_Sum = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.ExecPercentage) ?? 0,
                Cons_components = x.Cons_components ?? "",

            }).ToList().Where(s => ((string.IsNullOrEmpty(DateFrom) || DateTime.ParseExact(s.ProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                && ((string.IsNullOrEmpty(DateTo) || DateTime.ParseExact(s.ProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))))).ToList();

            return projects;


        }

        public async Task<decimal?> GetProjectCountByStatus(int? UserId, int BranchId)
        {
            if (UserId == null)
            {
                return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0).Sum(x => x.PercentComplete) / _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0).Count();
            }
            else
            {
                return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && s.ProjectWorkers!.Where(m => m.IsDeleted == false && m.UserId == UserId).Count() >= 1).Sum(x => x.PercentComplete) / _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && s.ProjectWorkers.Where(m => m.IsDeleted == false && m.UserId == UserId).Count() >= 1).Count();
            }
        }
        public async Task<int> GetProjectAreaCount(int BranchId)
        {
            return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ProjectTypeId == 1 && s.Status == 0).Count();
        }
        public async Task<int> GetProjectWorkOrdersCount(int BranchId)
        {
            return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ProjectTypeId == 2 && s.Status == 0).Count();
        }
        public async Task<int> GetProjectGovernmentCount(int BranchId)
        {
            return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ProjectTypeId == 3 && s.Status == 0).Count();
        }
        public async Task<int> GetProjectDesignCount(int BranchId)
        {
            return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ProjectTypeId == 4 && s.Status == 0).Count();
        }
        public async Task<int> GetProjectPlanningCount(int BranchId)
        {
            return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ProjectTypeId == 5 && s.Status == 0).Count();
        }
        public async Task<int> GetProjectSupervisionCount(int BranchId)
        {
            return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ProjectTypeId == 6 && s.Status == 0).Count();

        }
        public async Task<int> GetAllProjectCount(int BranchId)
        {
            return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && s.BranchId == BranchId).Count();
        }
        public async Task<decimal> GetUserProjectsCount(int UserId, int BranchId)
        {
            //return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0).Count();

            decimal ProjectU = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && s.MangerId == UserId).Count();
            decimal AllProject = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0).Count();
            decimal result = 0;
            if (AllProject != 0)
            {
                result = (ProjectU / AllProject) * 100;
            }

            return result;

            //return _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && (s.ProjectWorkers.Where(i => i.IsDeleted == false).Select(x => x.UserId).Contains(UserId))).Count();
        }
        public async Task<int> GenerateNextProjectNumber(int BranchId, string codePrefix)
        {
            if (_TaamerProContext.Project != null)
            {
                var lastRow = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ProjectNoType == 1 && s.ProjectNo!.Contains(codePrefix)).OrderByDescending(u => u.ProjectId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {

                        var ProjectNumber = 0;

                        if (codePrefix == "")
                        {
                            ProjectNumber = int.Parse(lastRow!.ProjectNo!) + 1;
                        }
                        else
                        {
                            ProjectNumber = int.Parse(lastRow!.ProjectNo!.Replace(codePrefix, "").Trim()) + 1;
                        }

                        return ProjectNumber;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
        public async Task< IEnumerable<ProjectVM>> GetAllHirearchialProject(int BranchId, int UserId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 && s.ProjectWorkers.Where(p => p.IsDeleted == false).Select(t => t.UserId).Contains(UserId)).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,
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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FileCount = x.ProjectFiles!.Count(),
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                StopProjectType = x.StopProjectType ?? 0,
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
                TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                ProjectWorkerCount = x.ProjectWorkers!.Where(s => s.IsDeleted == false).Count(),
                TaskRemainingCount = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status != 4).Count()

            });
            return projects;
        }
        public async Task<decimal> GetProjectsPercentByUserIdAndProjectId(int? UserId, int BranchId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.UserId == UserId).Sum(s => s.PercentComplete) / _TaamerProContext.Project.Where(s => s.UserId == UserId).Count() ?? 0;
            return projects;
        }

        public async Task<IEnumerable<object>>GetAllWOStatuses(string Con)
        {
            // //var Statuses = _ProjectRepository.GetAllArchiveProjectsByDateSearch(DateFrom, DateTo, BranchId);

            // SqlConnection con = new SqlConnection(Con);
            // SqlDataAdapter da = new SqlDataAdapter("select TaskStatusID,NameAr from Pro_TaskStatus", Con);
            // da.SelectCommand.CommandType = CommandType.Text;
            // DataSet ds = new DataSet();
            // da.Fill(ds);

            // DataTable myDataTable = ds.Tables[0];
            // con.Close();

            // return myDataTable.AsEnumerable().Select(row => new
            // {
            //     Id = int.Parse(row[0].ToString()),
            //     Name = row[1].ToString()
            // }
            //);

            return null;

        }
        public async Task< IEnumerable<ProjectVM>> GetAllProjByCustId(string lang, int BranchId, int? CustId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.CustomerId == CustId && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeId = x.ProjectTypeId,
                SubProjectTypeId = x.SubProjectTypeId,
                GeneralLocation = x.GeneralLocation,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
            });
            return projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetAllProjByCustIdWithout(string lang, int BranchId, int? CustId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.CustomerId == CustId).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeId = x.ProjectTypeId,
                SubProjectTypeId = x.SubProjectTypeId,
                GeneralLocation = x.GeneralLocation,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
            });
            return projects;
        }


        public async Task< IEnumerable<ProjectVM>> GetAllProjByBranch(string lang, int BranchId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeId = x.ProjectTypeId,
                SubProjectTypeId = x.SubProjectTypeId,
                GeneralLocation = x.GeneralLocation,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
            });
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetAllProjByBranchWithout(string lang, int BranchId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId ).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeId = x.ProjectTypeId,
                SubProjectTypeId = x.SubProjectTypeId,
                GeneralLocation = x.GeneralLocation,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
            });
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetAllProjByCustomerId(int customerId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.CustomerId == customerId && s.Status == 0).Select(x => new ProjectVM
            {
                Id = x.ProjectId,
                Name = x.ProjectNo
            
            });
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetAllProjByCustomerId2(int customerId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.CustomerId == customerId && s.Status == 0).Select(x => new ProjectVM
            {
                Id = x.ProjectId,
                Name = x.ProjectNo,
                OffersPricesId = x.OffersPricesId
            });
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByCustomerId2Pro(int customerId, int ProjectId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.CustomerId == customerId && s.Status == 0 &&(s.ProjectId!= ProjectId || ProjectId==0)).Select(x => new ProjectVM
            {
                Id = x.ProjectId,
                Name = x.ProjectNo,
                OffersPricesId = x.OffersPricesId
            });
            return projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetAllProjByCustomerIdWithout(int customerId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.CustomerId == customerId).Select(x => new ProjectVM
            {
                Id = x.ProjectId,
                Name = x.ProjectNo
            });
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetAllProjByCustomerIdHaveTasks(int customerId)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.CustomerId == customerId && s.Status == 0 && 
            s.ProjectPhasesTasks!.Where(x=> x.IsDeleted == false && x.IsMerig == -1 &&
                x.Type == 3 && (x.Status == 1 || x.Status == 2 || x.Status == 3)).Count() > 0).Select(x => new ProjectVM
            {
                Id = x.ProjectId,
                Name = x.ProjectNo
            });
            return projects;
        }

        public async Task<IEnumerable<ProjectVM>> GetAllProjByCustomerIdandbranchHaveTasks(int customerId,int branchid)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false &&( s.CustomerId == customerId || customerId==0) && s.Status == 0 &&
            s.ProjectPhasesTasks!.Where(x => x.IsDeleted == false && x.IsMerig == -1 &&
                x.Type == 3 && (x.Status == 1 || x.Status == 2 || x.Status == 3) && s.BranchId==branchid || branchid==0).Count() > 0).Select(x => new ProjectVM
                {
                    Id = x.ProjectId,
                    Name = x.ProjectNo
                });
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetAllProjByFawater(string lang, int BranchId)
        {

            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeId = x.ProjectTypeId,
                SubProjectTypeId = x.SubProjectTypeId,
                GeneralLocation = x.GeneralLocation,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,


                //dawoud

            });
            return projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetAllProjByMrdod(string lang, int BranchId)
        {

            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeId = x.ProjectTypeId,
                SubProjectTypeId = x.SubProjectTypeId,
                GeneralLocation = x.GeneralLocation,
                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

            });
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetProjectsStoppedVM()
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.StopProjectType == 1 && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
            });
            return projects;
        }

        public async Task<IEnumerable<ProjectVM>> GetdestinationsUploadVM()
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.DestinationsUpload == 1).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
            });
            return projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetProjectsWithoutContractVM()
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.ContractId == null && s.Status == 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
            });
            return projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetProjectsLateVM(string Today)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.Status == 0 ).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
            }).ToList().Where(s => DateTime.ParseExact(s.FirstProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(Today, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.FirstProjectExpireDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(Today, "yyyy-MM-dd", CultureInfo.InvariantCulture)); ;
            return projects;
        }
        public async Task< IEnumerable<ProjectVM>> GetMaxCosEManagerName()
        {
            var pro = _TaamerProContext.Project.Where(s => s.IsDeleted==false).Select(x => new ProjectVM
            {
                ProjectMangerName = x.Users !=null? x.Users!.FullName:"",
                CostE = x.Invoices!.Where(s => s.IsDeleted == false && s.IsPost==true && s.Type == 2 && s.Rad != true).Sum(s => s.InvoiceValue) ?? 0,
            }).ToList();
            return pro;
        }

        public async Task< IEnumerable<ProjectVM>> GetProjectsLatereport(int userid,int branchid,string startdate,string enddate)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false &&(branchid ==0 || s.BranchId== branchid) && s.Status == 0 && s.MangerId ==userid).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
            }).ToList().Where(s => DateTime.ParseExact(s.FirstProjectDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(enddate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.FirstProjectExpireDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(enddate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetProjectsStopped(int userid, int? branchid)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId==userid && s.StopProjectType == 1 && s.Status == 0 && ( branchid == 0 ||s.BranchId== branchid) ).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
            });
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetProjectsInprogress(int userid, int? branchid)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId==userid && s.StopProjectType != 1 && s.Status == 0 && (branchid == 0 || s.BranchId == branchid)).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
            });
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> GetProjectsWithoutaction(int userid, int? branchid)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId == userid && s.StopProjectType != 1 && s.ActiveMainPhase==null &&s.ActiveSubPhase==null && (branchid == 0 || s.BranchId == branchid)).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
            });
            return projects;
        }

        public async Task< IEnumerable<ProjectVM>> Getprojectalmostfinish(int userid, int? branchid)
        {
            var projects = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId == userid  && s.Status == 0 && (branchid == 0 || s.BranchId == branchid)).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                ProjectTypeName = x.ProjectTypeName,
                CityId = x.CityId,
                ContractNo = x.Contracts!.ContractNo,
                ContractId = x.ContractId,
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
                CustomerName_W = x.customer!.CustomerNameAr,
                CustomerName = x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,

                ProjectTypesName = x.projecttype!.NameAr,
                ProjectSubTypeName = x.projectsubtype!.NameAr,
                ProjectMangerName = x.Users!.FullName,
                CityName = x.city!.NameAr,
                TransactionTypeName = x.transactionTypes!.NameAr,
                RegionTypeId = x.RegionTypeId,
                RegionTypeName = x.regionTypes!.NameAr,
                FirstProjectDate = x.FirstProjectDate,
                FirstProjectExpireDate = x.FirstProjectExpireDate,
                FileCount = x.ProjectFiles!.Count(),
                TimeStr = (x.NoOfDays) != 0.0 ? (x.NoOfDays) + " يوم " : "",
                ExpectedTime = x.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                CurrentMainPhase = x.ActiveMainPhase!.DescriptionAr,
                CurrentSubPhase = x.ActiveSubPhase!.DescriptionAr,
                BranchId = x.BranchId,
                CostCenterId = x.CostCenterId ?? 0,

                StopProjectType = x.StopProjectType ?? 0,
                Cost = x.ProjectPhasesTasks!.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0,
                ContractValue = x.Contracts != null ? x.Contracts!.TotalValue.ToString() : "0",
                DepartmentId = x.DepartmentId,
                MotionProject = x.MotionProject,
                MotionProjectDate = x.MotionProjectDate,
                MotionProjectNote = x.MotionProjectNote ?? "",
            });
            return projects;
        }


        public async Task<List<ProjectVM>> GetAllProjectsNew(string Con,ProjectVM _project, int? UserId, int AllUserBranchId,int FilterType, int? BranchId)
        {
            try
            {
                List<ProjectVM> lmd = new List<ProjectVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetAllProjectsNew";
                        command.CommandTimeout = 0;
                        command.Connection = con;

                        //---------------------------------------------------------------------
                        if (UserId == 0 || UserId == null)
                            command.Parameters.Add(new SqlParameter("@UserId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@UserId", UserId));
                        //---------------------------------------------------------------------
                        //---------------------------------------------------------------------
                        if (BranchId == 0 || BranchId == null)
                            command.Parameters.Add(new SqlParameter("@BranchId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        //---------------------------------------------------------------------
                        if (_project.Status == null)
                            command.Parameters.Add(new SqlParameter("@Status", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@Status", _project.Status));
                        //---------------------------------------------------------------------
                        if (_project.ProjectNo == "" || _project.ProjectNo == null)
                            command.Parameters.Add(new SqlParameter("@ProjectNo", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ProjectNo", _project.ProjectNo));
                        //---------------------------------------------------------------------
                        if (_project.ProjectTypeId == 0 || _project.ProjectTypeId == null)
                            command.Parameters.Add(new SqlParameter("@ProjectType", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ProjectType", _project.ProjectTypeId));
                        //---------------------------------------------------------------------
                        if (_project.SubProjectTypeId == 0 || _project.SubProjectTypeId == null)
                            command.Parameters.Add(new SqlParameter("@ProjectSubType", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ProjectSubType", _project.SubProjectTypeId));
                        //---------------------------------------------------------------------
                        if (_project.CustomerId == 0 || _project.CustomerId == null)
                            command.Parameters.Add(new SqlParameter("@CustomerId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@CustomerId", _project.CustomerId));                        
                        //---------------------------------------------------------------------
                        if (_project.MangerId == 0 || _project.MangerId == null)
                            command.Parameters.Add(new SqlParameter("@ManagerId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ManagerId", _project.MangerId));
                        //---------------------------------------------------------------------
                        if (_project.ProjectId == 0 || _project.ProjectId == null)
                            command.Parameters.Add(new SqlParameter("@ProjectId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ProjectId", _project.ProjectId));
                        //---------------------------------------------------------------------
                        if (_project.ContractNo == "" || _project.ContractNo == null)
                            command.Parameters.Add(new SqlParameter("@ContractNo", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ContractNo", _project.ContractNo));
                        //---------------------------------------------------------------------
                        if (_project.NationalNumber == "" || _project.NationalNumber == null)
                            command.Parameters.Add(new SqlParameter("@NationalId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@NationalId", _project.NationalNumber));
                        //---------------------------------------------------------------------
                        //---------------------------------------------------------------------
                        if (_project.Mobile == "" || _project.Mobile == null)
                            command.Parameters.Add(new SqlParameter("@Mobile", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@Mobile", _project.Mobile));
                        //---------------------------------------------------------------------
                        if (_project.CityId == 0 || _project.CityId == null)
                            command.Parameters.Add(new SqlParameter("@CityId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@CityId", _project.CityId));
                        //---------------------------------------------------------------------
                        if (_project.ProjectDescription == "" || _project.ProjectDescription == null)
                            command.Parameters.Add(new SqlParameter("@ProjectDesc", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ProjectDesc", _project.ProjectDescription));
                        //---------------------------------------------------------------------
                        if (_project.PieceNo == 0 || _project.PieceNo == null)
                            command.Parameters.Add(new SqlParameter("@PieceNo", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@PieceNo", _project.PieceNo));
                        //---------------------------------------------------------------------
                        if (_project.DepartmentId == 0 || _project.DepartmentId == null)
                            command.Parameters.Add(new SqlParameter("@DepartmentId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@DepartmentId", _project.DepartmentId));
                        //---------------------------------------------------------------------
                        if (_project.ProjectDate == "" || _project.ProjectDate == null)
                            command.Parameters.Add(new SqlParameter("@startdate", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@startdate", _project.ProjectDate));
                        //---------------------------------------------------------------------
                        if (_project.ProjectExpireDate == "" || _project.ProjectExpireDate == null)
                            command.Parameters.Add(new SqlParameter("@enddate", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@enddate", _project.ProjectExpireDate));
                        //---------------------------------------------------------------------
                        if (_project.ProjectDateF == "" || _project.ProjectDateF == null)
                            command.Parameters.Add(new SqlParameter("@startdateF", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@startdateF", _project.ProjectDateF));
                        //---------------------------------------------------------------------
                        if (_project.ProjectExpireDateF == "" || _project.ProjectExpireDateF == null)
                            command.Parameters.Add(new SqlParameter("@enddateF", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@enddateF", _project.ProjectExpireDateF));
                        //---------------------------------------------------------------------

                        command.Parameters.Add(new SqlParameter("@AllUserBranchId", AllUserBranchId));
                        //---------------------------------------------------------------------
                        command.Parameters.Add(new SqlParameter("@FilterType", FilterType));
                        //---------------------------------------------------------------------

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lmd.Add(new ProjectVM
                            {
                                ProjectId = Convert.ToInt32((dr["ProjectId"]).ToString()),
                                ProjectName = (dr["ProjectName"]).ToString(),
                                ContractNo = (dr["ContractNo"]).ToString(),
                                ContractDate = (dr["ContractDate"]).ToString(),
                                ContractId = Convert.ToInt32((dr["ContractId"]).ToString()),
                                ProjectNo = (dr["ProjectNo"]).ToString(),
                                CustomerId = Convert.ToInt32((dr["CustomerId"]).ToString()),
                                ProjectTypeName = (dr["ProjectTypeName"]).ToString(),
                                CityId = Convert.ToInt32((dr["CityId"]).ToString()),
                                ContractorName = (dr["ContractorName"]).ToString(),
                                ProjectDescription = (dr["ProjectDescription"]).ToString(),
                                ProjectDate = (dr["ProjectDate"]).ToString(),
                                ProjectHijriDate = (dr["ProjectHijriDate"]).ToString(),
                                ProjectTypeId = Convert.ToInt32((dr["ProjectTypeId"]).ToString()),
                                MangerId = Convert.ToInt32((dr["MangerId"]).ToString()),
                                ParentProjectId = Convert.ToInt32((dr["ParentProjectId"]).ToString()),
                                BuildingType = Convert.ToInt32((dr["BuildingType"]).ToString()),
                                SubProjectTypeId = Convert.ToInt32((dr["SubProjectTypeId"]).ToString()),
                                TransactionTypeId = Convert.ToInt32((dr["TransactionTypeId"]).ToString()),
                                CustomerName_W = (dr["CustomerName_W"]).ToString(),
                                CustomerName = (dr["CustomerName"]).ToString(),
                                ProjectTypesName = (dr["ProjectTypesName"]).ToString(),
                                ProjectSubTypeName = (dr["ProjectSubTypeName"]).ToString(),
                                ProjectMangerName = (dr["ProjectMangerName"]).ToString(),
                                CityName = (dr["CityName"]).ToString(),
                                TransactionTypeName = (dr["TransactionTypeName"]).ToString(),
                                RegionTypeId = Convert.ToInt32((dr["RegionTypeId"]).ToString()),
                                RegionTypeName = (dr["RegionTypeName"]).ToString(),
                                FileCount = Convert.ToInt32((dr["FileCount"]).ToString()),
                                ProjectExpireDate = (dr["ProjectExpireDate"]).ToString(),
                                ProjectExpireHijriDate = (dr["ProjectExpireHijriDate"]).ToString(),
                                Co_opOfficeName = (dr["Co_opOfficeName"]).ToString(),
                                Co_opOfficeEmail = (dr["Co_opOfficeEmail"]).ToString(),
                                Co_opOfficePhone = (dr["Co_opOfficePhone"]).ToString(),
                                BranchId = Convert.ToInt32((dr["BranchId"]).ToString()),
                                CostCenterId = Convert.ToInt32((dr["CostCenterId"]).ToString()),
                                FirstProjectDate = (dr["FirstProjectDate"]).ToString(),
                                FirstProjectExpireDate = (dr["FirstProjectExpireDate"]).ToString(),
                                StopProjectType = Convert.ToInt32((dr["StopProjectType"]).ToString()),
                                TimeStr = (dr["TimeStr"]).ToString(),
                                ExpectedTime = Convert.ToDecimal((dr["ExpectedTime"]).ToString()),
                                CurrentMainPhase = (dr["CurrentMainPhase"]).ToString(),
                                CurrentSubPhase = (dr["CurrentSubPhase"]).ToString(),
                                Cost = Convert.ToDecimal((dr["Cost"]).ToString()),
                                ContractValue = (dr["ContractValue"]).ToString(),
                                ContractValue_W = (dr["ContractValue_W"]).ToString(),
                                CostE = Convert.ToDecimal((dr["CostE"]).ToString()),
                                CostE_Credit = Convert.ToDecimal((dr["CostE_Credit"]).ToString()),
                                CostE_Depit = Convert.ToDecimal((dr["CostE_Depit"]).ToString()),
                                CostS = Convert.ToDecimal((dr["CostS"]).ToString()),
                                CostE_W = Convert.ToDecimal((dr["CostE_W"]).ToString()),
                                CostE_Credit_W = Convert.ToDecimal((dr["CostE_Credit_W"]).ToString()),
                                CostE_Depit_W = Convert.ToDecimal((dr["CostE_Depit_W"]).ToString()),
                                CostS_W = Convert.ToDecimal((dr["CostS_W"]).ToString()),
                                Oper_expeValue = Convert.ToDecimal((dr["Oper_expeValue"]).ToString()),
                                OffersPricesId = Convert.ToInt32((dr["OffersPricesId"]).ToString()),
                                DepartmentId = Convert.ToInt32((dr["DepartmentId"]).ToString()),
                                MotionProject = Convert.ToInt32((dr["MotionProject"]).ToString()),
                                MotionProjectDate = (dr["MotionProjectDate"]).ToString(),
                                MotionProjectNote = (dr["MotionProjectNote"]).ToString(),
                                TypeCode = Convert.ToInt32((dr["TypeCode"]).ToString()),
                                Cons_components = (dr["Cons_components"]).ToString(),
                                Isimportant = Convert.ToInt32((dr["Isimportant"]).ToString()),
                                importantid = Convert.ToInt32((dr["importantid"]).ToString()),
                                flag = Convert.ToInt32((dr["flag"]).ToString()),
                                AddUser = (dr["addUser"].ToString()),
                                AddedUserImg = (dr["AddedUserImg"].ToString()),
                                UpdateUser = (dr["UpdateUser"].ToString()),
                                FinishDate = (dr["FinishDate"].ToString()),
                                ReasonText = (dr["ReasonText"].ToString()),
                                FinishReason = (dr["FinishReason"].ToString()),
                                ContractorSelectId = Convert.ToInt32((dr["ContractorSelectId"]).ToString()),
                                ContractorEmail_T = (dr["ContractorEmail_T"].ToString()),
                                ContractorPhone_T = (dr["ContractorPhone_T"].ToString()),
                                ContractorCom_T = (dr["ContractorCom_T"].ToString()),
                                DestinationsUpload = Convert.ToInt32(dr["DestinationsUpload"].ToString()),


                                SiteName = (dr["SiteName"]).ToString(),
                                OrderType = (dr["OrderType"]).ToString(),
                                SketchName = (dr["SketchName"]).ToString(),
                                SketchNo = (dr["SketchNo"]).ToString(),
                                PieceNo = Convert.ToInt32((dr["PieceNo"]).ToString()),
                                AdwAR = Convert.ToInt32((dr["AdwAR"]).ToString()),
                                Status = Convert.ToInt32((dr["Status"]).ToString()),
                                OrderNo = (dr["OrderNo"]).ToString(),
                                OutBoxNo = (dr["OutBoxNo"]).ToString(),
                                OutBoxDate = (dr["OutBoxDate"]).ToString(),
                                OutBoxHijriDate = (dr["OutBoxHijriDate"]).ToString(),
                                Reason1 = (dr["Reason1"]).ToString(),
                                Notes1 = (dr["Notes1"]).ToString(),
                                Subject = (dr["Subject"]).ToString(),
                                XPoint = (dr["XPoint"]).ToString(),
                                YPoint = (dr["YPoint"]).ToString(),
                                Technical = (dr["Technical"]).ToString(),
                                Prosedor = (dr["Prosedor"]).ToString(),
                                ReasonRevers = (dr["ReasonRevers"]).ToString(),
                                EngNotes = (dr["EngNotes"]).ToString(),
                                ReverseDate = (dr["ReverseDate"]).ToString(),
                                ReverseHijriDate = (dr["ReverseHijriDate"]).ToString(),
                                OrderStatus = Convert.ToInt32((dr["OrderStatus"]).ToString()),
                                UserId = Convert.ToInt32((dr["UserId"]).ToString()),
                                Receipt = Convert.ToInt32((dr["Receipt"]).ToString()),
                                PayStatus = Convert.ToBoolean((dr["PayStatus"]).ToString()),
                                RegionName = (dr["RegionName"]).ToString(),
                                DistrictName = (dr["DistrictName"]).ToString(),
                                SiteType = (dr["SiteType"]).ToString(),
                                ContractSource = (dr["ContractSource"]).ToString(),
                                SiteNo = (dr["SiteNo"]).ToString(),
                                PayanNo = (dr["PayanNo"]).ToString(),
                                JehaId = Convert.ToInt32((dr["JehaId"]).ToString()),
                                ZaraaSak = (dr["ZaraaSak"]).ToString(),
                                ZaraaNatural = (dr["ZaraaNatural"]).ToString(),
                                BordersSak = (dr["BordersSak"]).ToString(),
                                BordersNatural = (dr["BordersNatural"]).ToString(),
                                Ertedad = (dr["Ertedad"]).ToString(),
                                Brooz = (dr["Brooz"]).ToString(),
                                AreaSak = (dr["AreaSak"]).ToString(),
                                AreaNatural = (dr["AreaNatural"]).ToString(),
                                AreaArrange = (dr["AreaArrange"]).ToString(),
                                BuildingPercent = (dr["BuildingPercent"]).ToString(),
                                SpaceName = (dr["SpaceName"]).ToString(),
                                Office = (dr["Office"]).ToString(),
                                Usage = (dr["Usage"]).ToString(),
                                Docpath = (dr["Docpath"]).ToString(),
                                elevators = (dr["elevators"]).ToString(),
                                typ1 = (dr["typ1"]).ToString(),
                                brozat = (dr["brozat"]).ToString(),
                                entries = (dr["entries"]).ToString(),
                                Basement = (dr["Basement"]).ToString(),
                                GroundFloor = (dr["GroundFloor"]).ToString(),
                                FirstFloor = (dr["FirstFloor"]).ToString(),
                                Motkrr = (dr["Motkrr"]).ToString(),
                                FirstExtension = (dr["FirstExtension"]).ToString(),
                                ExtensionName = (dr["ExtensionName"]).ToString(),
                                GeneralLocation = (dr["GeneralLocation"]).ToString(),
                                LicenseNo = (dr["LicenseNo"]).ToString(),
                                Licensedate = (dr["Licensedate"]).ToString(),
                                LicenseHijridate = (dr["LicenseHijridate"]).ToString(),
                                DesiningOffice = (dr["DesiningOffice"]).ToString(),
                                estsharyformoslhat = Convert.ToInt32((dr["estsharyformoslhat"]).ToString()),
                                Consultantfinishing = Convert.ToInt32((dr["Consultantfinishing"]).ToString()),
                                Period = (dr["Period"]).ToString(),
                                punshmentamount = Convert.ToInt32((dr["punshmentamount"]).ToString()),
                                FirstPay = Convert.ToDecimal((dr["FirstPay"]).ToString()),
                                LicenseContent = (dr["LicenseContent"]).ToString(),
                                OtherStatus = Convert.ToInt32((dr["OtherStatus"]).ToString()),
                                AreaSpace = (dr["AreaSpace"]).ToString(),
                                SupervisionSatartDate = (dr["SupervisionSatartDate"]).ToString(),
                                SupervisionSatartHijriDate = (dr["SupervisionSatartHijriDate"]).ToString(),
                                SupervisionEndDate = (dr["SupervisionEndDate"]).ToString(),
                                SupervisionEndHijriDate = (dr["SupervisionEndHijriDate"]).ToString(),
                                SupervisionNo = (dr["SupervisionNo"]).ToString(),
                                SupervisionNotes = (dr["SupervisionNotes"]).ToString(),
                                qaboqwaedmostlm = (dr["qaboqwaedmostlm"]).ToString(),
                                qaboreqabmostlm = (dr["qaboreqabmostlm"]).ToString(),
                                qabosaqfmostlm = (dr["qabosaqfmostlm"]).ToString(),
                                molhqalwisaqffash = (dr["molhqalwisaqffash"]).ToString(),
                                molhqalwisaqfdate = (dr["molhqalwisaqfdate"]).ToString(),
                                LimitDays = Convert.ToInt32((dr["LimitDays"]).ToString()),
                                NoteDate = (dr["NoteDate"]).ToString(),
                                NoteHijriDate = (dr["NoteHijriDate"]).ToString(),
                                ResponseEng = (dr["ResponseEng"]).ToString(),
                                ReseveStatus = Convert.ToInt32((dr["ReseveStatus"]).ToString()),
                                kaeedno = (dr["kaeedno"]).ToString(),
                                TechnicalDemands = (dr["TechnicalDemands"]).ToString(),
                                Todoaction = (dr["Todoaction"]).ToString(),
                                Responsible = (dr["Responsible"]).ToString(),
                                ExternalEmpId = Convert.ToInt32((dr["ExternalEmpId"]).ToString()),
                                ContractPeriod = Convert.ToInt32((dr["ContractPeriod"]).ToString()),
                                SpaceNotes = (dr["SpaceNotes"]).ToString(),
                                ContractNotes = (dr["ContractNotes"]).ToString(),
                                SpaceId = Convert.ToInt32((dr["SpaceId"]).ToString()),
                                Paied = Convert.ToInt32((dr["Paied"]).ToString()),
                                Discount = Convert.ToInt32((dr["Discount"]).ToString()),
                                Fees = Convert.ToInt32((dr["Fees"]).ToString()),
                                ProjectRegionName = (dr["ProjectRegionName"]).ToString(),
                                Catego = (dr["Catego"]).ToString(),
                                ContractPeriodType = (dr["ContractPeriodType"]).ToString(),
                                ContractPeriodMinites = Convert.ToInt32((dr["ContractPeriodMinites"]).ToString()),
                                ProjectValue = Convert.ToDecimal((dr["ProjectValue"]).ToString()),
                                ProjectContractTawk = (dr["ProjectContractTawk"]).ToString(),
                                ProjectRecieveLoaction = (dr["ProjectRecieveLoaction"]).ToString(),
                                ProjectObserveMobile = (dr["ProjectObserveMobile"]).ToString(),
                                ProjectObserveMail = (dr["ProjectObserveMail"]).ToString(),
                                ProjectTaslemFirst = (dr["ProjectTaslemFirst"]).ToString(),
                                FDamanID = Convert.ToInt32((dr["FDamanID"]).ToString()),
                                LDamanID = Convert.ToInt32((dr["LDamanID"]).ToString()),
                                NesbaEngaz = Convert.ToDecimal((dr["NesbaEngaz"]).ToString()),
                                Takeem = (dr["Takeem"]).ToString(),
                                ProjectContractTawkCh = Convert.ToBoolean((dr["ProjectContractTawkCh"]).ToString()),
                                ProjectRecieveLoactionCh = Convert.ToBoolean((dr["ProjectRecieveLoactionCh"]).ToString()),
                                ProjectTaslemFirstCh = Convert.ToBoolean((dr["ProjectTaslemFirstCh"]).ToString()),
                                ContractCh = Convert.ToBoolean((dr["ContractCh"]).ToString()),
                                PeriodProject = Convert.ToInt32((dr["PeriodProject"]).ToString()),
                                AgentDate = (dr["AgentDate"]).ToString(),
                                AgentHijriDate = (dr["AgentHijriDate"]).ToString(),
                                StreetName = (dr["StreetName"]).ToString(),
                                MainText = (dr["MainText"]).ToString(),
                                BranchText =(dr["BranchText"]).ToString(),
                                TaskText = (dr["TaskText"]).ToString(),
                                MunicipalId = Convert.ToInt32((dr["MunicipalId"]).ToString()),
                                SubMunicipalityId = Convert.ToInt32((dr["SubMunicipalityId"]).ToString()),
                                ProBuildingDisc = (dr["ProBuildingDisc"]).ToString(),
                                select = Convert.ToBoolean((dr["select"]).ToString()),
                                Insert = Convert.ToBoolean((dr["Insert"]).ToString()),
                                Update = Convert.ToBoolean((dr["Update"]).ToString()),
                                Delete = Convert.ToBoolean((dr["Delete"]).ToString()),
                                NewSetting = Convert.ToBoolean((dr["NewSetting"]).ToString()),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
                    {
                List<ProjectVM> lmd = new List<ProjectVM>();
                return lmd;
            }

        }

        public async Task<List<ProjectVMNewCounting>> GetProjectVMNew(string Lang, string Con, int BranchId)
        {
            try
            {
                List<ProjectVMNewCounting> lmd = new List<ProjectVMNewCounting>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Rpt_ProjectStatistics_Proc";
                        command.CommandTimeout = 180;
                        command.Connection = con;

                        //if (Search.StartDate == "")
                        //    command.Parameters.Add(new SqlParameter("@From_Search", DBNull.Value));
                        //else
                        //    command.Parameters.Add(new SqlParameter("@From_Search", Search.StartDate));


                        //if (Search.EndDate == "")
                        //    command.Parameters.Add(new SqlParameter("@To_Search", DBNull.Value));
                        //else
                        //    command.Parameters.Add(new SqlParameter("@To_Search", Search.EndDate));


                        command.Parameters.Add(new SqlParameter("@From_Search", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@To_Search", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lmd.Add(new ProjectVMNewCounting
                            {
                                
                                GetProjectsStoppedVMCount = (dr["GetProjectsStoppedVMCount"]).ToString(),
                                GetProjectsWithoutContractVMVMCount = (dr["GetProjectsWithoutContractVMVMCount"]).ToString(),
                                GetLateProjectsVMCount = (dr["GetLateProjectsVMCount"]).ToString(),
                                GetProjectsWithoutProSettingVMCount = (dr["GetProjectsWithoutProSettingVMCount"]).ToString(),
                                GetProjectsWithProSettingVMCount = (dr["GetProjectsWithProSettingVMCount"]).ToString(),
                                GetProjectsSupervisionVMVMCount = (dr["GetProjectsSupervisionVMVMCount"]).ToString(),
                                GetdestinationsUploadVMCount = (dr["GetdestinationsUploadVMCount"]).ToString(),
                                GetProjectsInProgressCount = (dr["GetProjectsInProgressCount"]).ToString(),
                                GetProjectsNaturalCount = (dr["GetProjectsNaturalCount"]).ToString(),
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<ProjectVMNewCounting> lmd = new List<ProjectVMNewCounting>();
                return lmd;
            }

        }
        public async Task<List<ProjectVMNewStat>> GetProjectVMStatNew(int ProjectId, string Lang, string Con, int BranchId)
        {
            try
            {
                List<ProjectVMNewStat> lmd = new List<ProjectVMNewStat>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetProjectStat_Proc";
                        command.CommandTimeout = 180;
                        command.Connection = con;

                        command.Parameters.Add(new SqlParameter("@ProjectId", ProjectId));


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        var GetProjectsContractId_txt = ""; var GetProjectsPhases_txt = "";
                        var GetProjectsInvoice_txt = ""; var GetProjectsVouchers_txt = "";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(dr["GetProjectsContractId"]) > 0) GetProjectsContractId_txt = "نعم";
                            else GetProjectsContractId_txt = "لا يوجد";

                            if (Convert.ToInt32(dr["GetProjectsPhases"]) > 0) GetProjectsPhases_txt = "نعم";
                            else GetProjectsPhases_txt = "لا يوجد";

                            if (Convert.ToInt32(dr["GetProjectsInvoice"]) > 0) GetProjectsInvoice_txt = "نعم";
                            else GetProjectsInvoice_txt = "لا يوجد";

                            if (Convert.ToInt32(dr["GetProjectsVouchers"]) > 0) GetProjectsVouchers_txt = "نعم";
                            else GetProjectsVouchers_txt = "لا يوجد";

                            lmd.Add(new ProjectVMNewStat
                            {
                                GetProjectsContractId = GetProjectsContractId_txt,
                                GetProjectsPhases = GetProjectsPhases_txt,
                                GetProjectsInvoice = GetProjectsInvoice_txt,
                                GetProjectsVouchers = GetProjectsVouchers_txt,

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<ProjectVMNewStat> lmd = new List<ProjectVMNewStat>();
                return lmd;
            }

        }

        public async Task<List<ProjectVMPhasesDetails>> GetPhasesDetails(string Lang, string Con, int ProjectId)
        {
            try
            {
                List<ProjectVMPhasesDetails> lmd = new List<ProjectVMPhasesDetails>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetPhasesDetails";
                        command.CommandTimeout = 180;
                        command.Connection = con;

                        command.Parameters.Add(new SqlParameter("@ProjectId", ProjectId));

                        decimal phasePercentV = 0;
                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if(Convert.ToDecimal((dr["alltaskscount"]).ToString())!=0)
                            {
                                phasePercentV = Math.Round(Convert.ToDecimal((Convert.ToDecimal((dr["alltaskspercent"]).ToString())) / (Convert.ToDecimal((dr["alltaskscount"]).ToString()) * 100)),4)*100;
                            }
                            lmd.Add(new ProjectVMPhasesDetails
                            {

                                PhaseTaskId =Convert.ToInt32((dr["PhaseTaskId"]).ToString()),
                                DescriptionAr = (dr["DescriptionAr"]).ToString(),
                                DescriptionEn = (dr["DescriptionEn"]).ToString(),
                                ParentId = Convert.ToInt32((dr["ParentId"]).ToString()),
                                Type = Convert.ToInt32((dr["Type"]).ToString()),
                                alltaskscount = Convert.ToDecimal((dr["alltaskscount"]).ToString()),
                                alltaskspercent = Convert.ToDecimal((dr["alltaskspercent"]).ToString()),
                                phasePercent = phasePercentV,
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<ProjectVMPhasesDetails> lmd = new List<ProjectVMPhasesDetails>();
                return lmd;
            }

        }


        public IEnumerable<Project> GetAll()
        {
            throw new NotImplementedException();
        }

        public Project GetById(int Id)
        {
          return  _TaamerProContext.Project.Where(x => x.ProjectId == Id).FirstOrDefault();
        }

        public IEnumerable<Project> GetMatching(Func<Project, bool> where)
        {
            return _TaamerProContext.Project.Where(where).ToList<Project>();
        }





        public async Task<List<ProjectVM>> GetAllProjectsNew_DashBoard(string Con, ProjectVM _project, int? UserId, int AllUserBranchId, int? BranchId)
        {
            try
            {
                List<ProjectVM> lmd = new List<ProjectVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetAllProjectsNew_DashBoard";
                        command.CommandTimeout = 0;
                        command.Connection = con;

                        //---------------------------------------------------------------------
                        if (UserId == 0 || UserId == null)
                            command.Parameters.Add(new SqlParameter("@UserId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@UserId", UserId));
                        //---------------------------------------------------------------------
                        //---------------------------------------------------------------------
                        if (BranchId == 0 || BranchId == null)
                            command.Parameters.Add(new SqlParameter("@BranchId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        //---------------------------------------------------------------------
                        if (_project.Status == null)
                            command.Parameters.Add(new SqlParameter("@Status", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@Status", _project.Status));
                        //---------------------------------------------------------------------
                        if (_project.ProjectNo == "" || _project.ProjectNo == null)
                            command.Parameters.Add(new SqlParameter("@ProjectNo", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ProjectNo", _project.ProjectNo));
                        //---------------------------------------------------------------------
                        if (_project.ProjectTypeId == 0 || _project.ProjectTypeId == null)
                            command.Parameters.Add(new SqlParameter("@ProjectType", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ProjectType", _project.ProjectTypeId));
                        //---------------------------------------------------------------------
                        if (_project.SubProjectTypeId == 0 || _project.SubProjectTypeId == null)
                            command.Parameters.Add(new SqlParameter("@ProjectSubType", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ProjectSubType", _project.SubProjectTypeId));
                        //---------------------------------------------------------------------
                        if (_project.CustomerId == 0 || _project.CustomerId == null)
                            command.Parameters.Add(new SqlParameter("@CustomerId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@CustomerId", _project.CustomerId));
                        //---------------------------------------------------------------------
                        if (_project.MangerId == 0 || _project.MangerId == null)
                            command.Parameters.Add(new SqlParameter("@ManagerId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ManagerId", _project.MangerId));
                        //---------------------------------------------------------------------
                        if (_project.ProjectId == 0 || _project.ProjectId == null)
                            command.Parameters.Add(new SqlParameter("@ProjectId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ProjectId", _project.ProjectId));
                        //---------------------------------------------------------------------
                        if (_project.ContractNo == "" || _project.ContractNo == null)
                            command.Parameters.Add(new SqlParameter("@ContractNo", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ContractNo", _project.ContractNo));
                        //---------------------------------------------------------------------
                        if (_project.NationalNumber == "" || _project.NationalNumber == null)
                            command.Parameters.Add(new SqlParameter("@NationalId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@NationalId", _project.NationalNumber));
                        //---------------------------------------------------------------------
                        //---------------------------------------------------------------------
                        if (_project.Mobile == "" || _project.Mobile == null)
                            command.Parameters.Add(new SqlParameter("@Mobile", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@Mobile", _project.Mobile));
                        //---------------------------------------------------------------------
                        if (_project.CityId == 0 || _project.CityId == null)
                            command.Parameters.Add(new SqlParameter("@CityId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@CityId", _project.CityId));
                        //---------------------------------------------------------------------
                        if (_project.ProjectDescription == "" || _project.ProjectDescription == null)
                            command.Parameters.Add(new SqlParameter("@ProjectDesc", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@ProjectDesc", _project.ProjectDescription));
                        //---------------------------------------------------------------------
                        if (_project.PieceNo == 0 || _project.PieceNo == null)
                            command.Parameters.Add(new SqlParameter("@PieceNo", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@PieceNo", _project.PieceNo));
                        //---------------------------------------------------------------------
                        if (_project.DepartmentId == 0 || _project.DepartmentId == null)
                            command.Parameters.Add(new SqlParameter("@DepartmentId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@DepartmentId", _project.DepartmentId));
                        //---------------------------------------------------------------------
                        if (_project.ProjectDate == "" || _project.ProjectDate == null)
                            command.Parameters.Add(new SqlParameter("@startdate", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@startdate", _project.ProjectDate));
                        //---------------------------------------------------------------------
                        if (_project.ProjectExpireDate == "" || _project.ProjectExpireDate == null)
                            command.Parameters.Add(new SqlParameter("@enddate", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@enddate", _project.ProjectExpireDate));
                        //---------------------------------------------------------------------

                        command.Parameters.Add(new SqlParameter("@AllUserBranchId", AllUserBranchId));
                        //---------------------------------------------------------------------

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lmd.Add(new ProjectVM
                            {
                                ProjectId = Convert.ToInt32((dr["ProjectId"]).ToString()),
                                ProjectName = (dr["ProjectName"]).ToString(),
                                ContractNo = (dr["ContractNo"]).ToString(),
                                ContractDate = (dr["ContractDate"]).ToString(),
                                ContractId = Convert.ToInt32((dr["ContractId"]).ToString()),
                                ProjectNo = (dr["ProjectNo"]).ToString(),
                                CustomerId = Convert.ToInt32((dr["CustomerId"]).ToString()),
                                ProjectTypeName = (dr["ProjectTypeName"]).ToString(),
                                CityId = Convert.ToInt32((dr["CityId"]).ToString()),
                                ContractorName = (dr["ContractorName"]).ToString(),
                                ProjectDescription = (dr["ProjectDescription"]).ToString(),
                                ProjectDate = (dr["ProjectDate"]).ToString(),
                                ProjectHijriDate = (dr["ProjectHijriDate"]).ToString(),
                                ProjectTypeId = Convert.ToInt32((dr["ProjectTypeId"]).ToString()),
                                MangerId = Convert.ToInt32((dr["MangerId"]).ToString()),
                                ParentProjectId = Convert.ToInt32((dr["ParentProjectId"]).ToString()),
                                BuildingType = Convert.ToInt32((dr["BuildingType"]).ToString()),
                                SubProjectTypeId = Convert.ToInt32((dr["SubProjectTypeId"]).ToString()),
                                TransactionTypeId = Convert.ToInt32((dr["TransactionTypeId"]).ToString()),
                                CustomerName_W = (dr["CustomerName_W"]).ToString(),
                                CustomerName = (dr["CustomerName"]).ToString(),
                                ProjectTypesName = (dr["ProjectTypesName"]).ToString(),
                                ProjectSubTypeName = (dr["ProjectSubTypeName"]).ToString(),
                                ProjectMangerName = (dr["ProjectMangerName"]).ToString(),
                                CityName = (dr["CityName"]).ToString(),
                                TransactionTypeName = (dr["TransactionTypeName"]).ToString(),
                                RegionTypeId = Convert.ToInt32((dr["RegionTypeId"]).ToString()),
                                RegionTypeName = (dr["RegionTypeName"]).ToString(),
                                FileCount = Convert.ToInt32((dr["FileCount"]).ToString()),
                                ProjectExpireDate = (dr["ProjectExpireDate"]).ToString(),
                                ProjectExpireHijriDate = (dr["ProjectExpireHijriDate"]).ToString(),
                                Co_opOfficeName = (dr["Co_opOfficeName"]).ToString(),
                                Co_opOfficeEmail = (dr["Co_opOfficeEmail"]).ToString(),
                                Co_opOfficePhone = (dr["Co_opOfficePhone"]).ToString(),
                                BranchId = Convert.ToInt32((dr["BranchId"]).ToString()),
                                CostCenterId = Convert.ToInt32((dr["CostCenterId"]).ToString()),
                                FirstProjectDate = (dr["FirstProjectDate"]).ToString(),
                                FirstProjectExpireDate = (dr["FirstProjectExpireDate"]).ToString(),
                                StopProjectType = Convert.ToInt32((dr["StopProjectType"]).ToString()),
                                TimeStr = (dr["TimeStr"]).ToString(),
                                ExpectedTime = Convert.ToDecimal((dr["ExpectedTime"]).ToString()),
                                CurrentMainPhase = (dr["CurrentMainPhase"]).ToString(),
                                CurrentSubPhase = (dr["CurrentSubPhase"]).ToString(),
                                Cost = Convert.ToDecimal((dr["Cost"]).ToString()),
                                ContractValue = (dr["ContractValue"]).ToString(),
                                ContractValue_W = (dr["ContractValue_W"]).ToString(),
                                CostE = Convert.ToDecimal((dr["CostE"]).ToString()),
                                CostE_Credit = Convert.ToDecimal((dr["CostE_Credit"]).ToString()),
                                CostE_Depit = Convert.ToDecimal((dr["CostE_Depit"]).ToString()),
                                CostS = Convert.ToDecimal((dr["CostS"]).ToString()),
                                CostE_W = Convert.ToDecimal((dr["CostE_W"]).ToString()),
                                CostE_Credit_W = Convert.ToDecimal((dr["CostE_Credit_W"]).ToString()),
                                CostE_Depit_W = Convert.ToDecimal((dr["CostE_Depit_W"]).ToString()),
                                CostS_W = Convert.ToDecimal((dr["CostS_W"]).ToString()),
                                Oper_expeValue = Convert.ToDecimal((dr["Oper_expeValue"]).ToString()),
                                OffersPricesId = Convert.ToInt32((dr["OffersPricesId"]).ToString()),
                                DepartmentId = Convert.ToInt32((dr["DepartmentId"]).ToString()),
                                MotionProject = Convert.ToInt32((dr["MotionProject"]).ToString()),
                                MotionProjectDate = (dr["MotionProjectDate"]).ToString(),
                                MotionProjectNote = (dr["MotionProjectNote"]).ToString(),
                                TypeCode = Convert.ToInt32((dr["TypeCode"]).ToString()),
                                Cons_components = (dr["Cons_components"]).ToString(),
                                Isimportant = Convert.ToInt32((dr["Isimportant"]).ToString()),
                                importantid = Convert.ToInt32((dr["importantid"]).ToString()),
                                flag = Convert.ToInt32((dr["flag"]).ToString()),
                                AddUser = (dr["addUser"].ToString()),
                                AddedUserImg = (dr["AddedUserImg"].ToString()),
                                UpdateUser = (dr["UpdateUser"].ToString()),
                                FinishDate = (dr["FinishDate"].ToString()),
                                ReasonText = (dr["ReasonText"].ToString()),
                                FinishReason = (dr["FinishReason"].ToString()),
                                ContractorSelectId = Convert.ToInt32((dr["ContractorSelectId"]).ToString()),
                                ContractorEmail_T = (dr["ContractorEmail_T"].ToString()),
                                ContractorPhone_T = (dr["ContractorPhone_T"].ToString()),
                                ContractorCom_T = (dr["ContractorCom_T"].ToString()),
                                DestinationsUpload = Convert.ToInt32(dr["DestinationsUpload"].ToString()),


                                SiteName = (dr["SiteName"]).ToString(),
                                OrderType = (dr["OrderType"]).ToString(),
                                SketchName = (dr["SketchName"]).ToString(),
                                SketchNo = (dr["SketchNo"]).ToString(),
                                PieceNo = Convert.ToInt32((dr["PieceNo"]).ToString()),
                                AdwAR = Convert.ToInt32((dr["AdwAR"]).ToString()),
                                Status = Convert.ToInt32((dr["Status"]).ToString()),
                                OrderNo = (dr["OrderNo"]).ToString(),
                                OutBoxNo = (dr["OutBoxNo"]).ToString(),
                                OutBoxDate = (dr["OutBoxDate"]).ToString(),
                                OutBoxHijriDate = (dr["OutBoxHijriDate"]).ToString(),
                                Reason1 = (dr["Reason1"]).ToString(),
                                Notes1 = (dr["Notes1"]).ToString(),
                                Subject = (dr["Subject"]).ToString(),
                                XPoint = (dr["XPoint"]).ToString(),
                                YPoint = (dr["YPoint"]).ToString(),
                                Technical = (dr["Technical"]).ToString(),
                                Prosedor = (dr["Prosedor"]).ToString(),
                                ReasonRevers = (dr["ReasonRevers"]).ToString(),
                                EngNotes = (dr["EngNotes"]).ToString(),
                                ReverseDate = (dr["ReverseDate"]).ToString(),
                                ReverseHijriDate = (dr["ReverseHijriDate"]).ToString(),
                                OrderStatus = Convert.ToInt32((dr["OrderStatus"]).ToString()),
                                UserId = Convert.ToInt32((dr["UserId"]).ToString()),
                                Receipt = Convert.ToInt32((dr["Receipt"]).ToString()),
                                PayStatus = Convert.ToBoolean((dr["PayStatus"]).ToString()),
                                RegionName = (dr["RegionName"]).ToString(),
                                DistrictName = (dr["DistrictName"]).ToString(),
                                SiteType = (dr["SiteType"]).ToString(),
                                ContractSource = (dr["ContractSource"]).ToString(),
                                SiteNo = (dr["SiteNo"]).ToString(),
                                PayanNo = (dr["PayanNo"]).ToString(),
                                JehaId = Convert.ToInt32((dr["JehaId"]).ToString()),
                                ZaraaSak = (dr["ZaraaSak"]).ToString(),
                                ZaraaNatural = (dr["ZaraaNatural"]).ToString(),
                                BordersSak = (dr["BordersSak"]).ToString(),
                                BordersNatural = (dr["BordersNatural"]).ToString(),
                                Ertedad = (dr["Ertedad"]).ToString(),
                                Brooz = (dr["Brooz"]).ToString(),
                                AreaSak = (dr["AreaSak"]).ToString(),
                                AreaNatural = (dr["AreaNatural"]).ToString(),
                                AreaArrange = (dr["AreaArrange"]).ToString(),
                                BuildingPercent = (dr["BuildingPercent"]).ToString(),
                                SpaceName = (dr["SpaceName"]).ToString(),
                                Office = (dr["Office"]).ToString(),
                                Usage = (dr["Usage"]).ToString(),
                                Docpath = (dr["Docpath"]).ToString(),
                                elevators = (dr["elevators"]).ToString(),
                                typ1 = (dr["typ1"]).ToString(),
                                brozat = (dr["brozat"]).ToString(),
                                entries = (dr["entries"]).ToString(),
                                Basement = (dr["Basement"]).ToString(),
                                GroundFloor = (dr["GroundFloor"]).ToString(),
                                FirstFloor = (dr["FirstFloor"]).ToString(),
                                Motkrr = (dr["Motkrr"]).ToString(),
                                FirstExtension = (dr["FirstExtension"]).ToString(),
                                ExtensionName = (dr["ExtensionName"]).ToString(),
                                GeneralLocation = (dr["GeneralLocation"]).ToString(),
                                LicenseNo = (dr["LicenseNo"]).ToString(),
                                Licensedate = (dr["Licensedate"]).ToString(),
                                LicenseHijridate = (dr["LicenseHijridate"]).ToString(),
                                DesiningOffice = (dr["DesiningOffice"]).ToString(),
                                estsharyformoslhat = Convert.ToInt32((dr["estsharyformoslhat"]).ToString()),
                                Consultantfinishing = Convert.ToInt32((dr["Consultantfinishing"]).ToString()),
                                Period = (dr["Period"]).ToString(),
                                punshmentamount = Convert.ToInt32((dr["punshmentamount"]).ToString()),
                                FirstPay = Convert.ToDecimal((dr["FirstPay"]).ToString()),
                                LicenseContent = (dr["LicenseContent"]).ToString(),
                                OtherStatus = Convert.ToInt32((dr["OtherStatus"]).ToString()),
                                AreaSpace = (dr["AreaSpace"]).ToString(),
                                SupervisionSatartDate = (dr["SupervisionSatartDate"]).ToString(),
                                SupervisionSatartHijriDate = (dr["SupervisionSatartHijriDate"]).ToString(),
                                SupervisionEndDate = (dr["SupervisionEndDate"]).ToString(),
                                SupervisionEndHijriDate = (dr["SupervisionEndHijriDate"]).ToString(),
                                SupervisionNo = (dr["SupervisionNo"]).ToString(),
                                SupervisionNotes = (dr["SupervisionNotes"]).ToString(),
                                qaboqwaedmostlm = (dr["qaboqwaedmostlm"]).ToString(),
                                qaboreqabmostlm = (dr["qaboreqabmostlm"]).ToString(),
                                qabosaqfmostlm = (dr["qabosaqfmostlm"]).ToString(),
                                molhqalwisaqffash = (dr["molhqalwisaqffash"]).ToString(),
                                molhqalwisaqfdate = (dr["molhqalwisaqfdate"]).ToString(),
                                LimitDays = Convert.ToInt32((dr["LimitDays"]).ToString()),
                                NoteDate = (dr["NoteDate"]).ToString(),
                                NoteHijriDate = (dr["NoteHijriDate"]).ToString(),
                                ResponseEng = (dr["ResponseEng"]).ToString(),
                                ReseveStatus = Convert.ToInt32((dr["ReseveStatus"]).ToString()),
                                kaeedno = (dr["kaeedno"]).ToString(),
                                TechnicalDemands = (dr["TechnicalDemands"]).ToString(),
                                Todoaction = (dr["Todoaction"]).ToString(),
                                Responsible = (dr["Responsible"]).ToString(),
                                ExternalEmpId = Convert.ToInt32((dr["ExternalEmpId"]).ToString()),
                                ContractPeriod = Convert.ToInt32((dr["ContractPeriod"]).ToString()),
                                SpaceNotes = (dr["SpaceNotes"]).ToString(),
                                ContractNotes = (dr["ContractNotes"]).ToString(),
                                SpaceId = Convert.ToInt32((dr["SpaceId"]).ToString()),
                                Paied = Convert.ToInt32((dr["Paied"]).ToString()),
                                Discount = Convert.ToInt32((dr["Discount"]).ToString()),
                                Fees = Convert.ToInt32((dr["Fees"]).ToString()),
                                ProjectRegionName = (dr["ProjectRegionName"]).ToString(),
                                Catego = (dr["Catego"]).ToString(),
                                ContractPeriodType = (dr["ContractPeriodType"]).ToString(),
                                ContractPeriodMinites = Convert.ToInt32((dr["ContractPeriodMinites"]).ToString()),
                                ProjectValue = Convert.ToDecimal((dr["ProjectValue"]).ToString()),
                                ProjectContractTawk = (dr["ProjectContractTawk"]).ToString(),
                                ProjectRecieveLoaction = (dr["ProjectRecieveLoaction"]).ToString(),
                                ProjectObserveMobile = (dr["ProjectObserveMobile"]).ToString(),
                                ProjectObserveMail = (dr["ProjectObserveMail"]).ToString(),
                                ProjectTaslemFirst = (dr["ProjectTaslemFirst"]).ToString(),
                                FDamanID = Convert.ToInt32((dr["FDamanID"]).ToString()),
                                LDamanID = Convert.ToInt32((dr["LDamanID"]).ToString()),
                                NesbaEngaz = Convert.ToDecimal((dr["NesbaEngaz"]).ToString()),
                                Takeem = (dr["Takeem"]).ToString(),
                                ProjectContractTawkCh = Convert.ToBoolean((dr["ProjectContractTawkCh"]).ToString()),
                                ProjectRecieveLoactionCh = Convert.ToBoolean((dr["ProjectRecieveLoactionCh"]).ToString()),
                                ProjectTaslemFirstCh = Convert.ToBoolean((dr["ProjectTaslemFirstCh"]).ToString()),
                                ContractCh = Convert.ToBoolean((dr["ContractCh"]).ToString()),
                                PeriodProject = Convert.ToInt32((dr["PeriodProject"]).ToString()),
                                AgentDate = (dr["AgentDate"]).ToString(),
                                AgentHijriDate = (dr["AgentHijriDate"]).ToString(),
                                StreetName = (dr["StreetName"]).ToString(),
                                MainText = (dr["MainText"]).ToString(),
                                BranchText = (dr["BranchText"]).ToString(),
                                TaskText = (dr["TaskText"]).ToString(),
                                MunicipalId = Convert.ToInt32((dr["MunicipalId"]).ToString()),
                                SubMunicipalityId = Convert.ToInt32((dr["SubMunicipalityId"]).ToString()),
                                ProBuildingDisc = (dr["ProBuildingDisc"]).ToString(),
                                select = Convert.ToBoolean((dr["select"]).ToString()),
                                Insert = Convert.ToBoolean((dr["Insert"]).ToString()),
                                Update = Convert.ToBoolean((dr["Update"]).ToString()),
                                Delete = Convert.ToBoolean((dr["Delete"]).ToString()),
                                NewSetting = Convert.ToBoolean((dr["NewSetting"]).ToString()),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<ProjectVM> lmd = new List<ProjectVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<ProjectVM>> GetprojectNewTasks(int UserId,int BranchId,string Lang)
        {
            var projects = _TaamerProContext.Project.Where(x => x.IsDeleted == false && x.ProjectPhasesTasks.Where(y => y.IsDeleted == false && y.IsMerig == -1 &&
y.Type == 3 && (y.Status == 1 || y.Status == 2 || y.Status == 3) && y.UserId == UserId &&y.BranchId== BranchId && (y.Remaining > 0 || y.Remaining == null)).Count()>0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                OffersPricesId = x.OffersPricesId ?? 0,
                OfferPriceNoName = " عرض سعر رقم  " + x.OffersPrices!.OfferNo,
    CustomerName = x.customer == null ? "" : x.customer!.Projects == null ? "" : Lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
                : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,



});
            return projects;
        }

        public async Task<IEnumerable<ProjectVM>> GetprojectLateTasks(int UserId, int BranchId, string Lang)
        {
            var projects = _TaamerProContext.Project.Where(x => x.IsDeleted == false && x.ProjectPhasesTasks.Where(y => y.IsDeleted == false && y.IsMerig == -1 &&
y.Type == 3  && y.Remaining < 0 && y.Status != 4 && y.UserId == UserId && y.BranchId == BranchId ).Count() > 0).Select(x => new ProjectVM
{
    ProjectId = x.ProjectId,
    ProjectName = x.ProjectName,
    ProjectNo = x.ProjectNo,
    CustomerId = x.CustomerId,
    OffersPricesId = x.OffersPricesId ?? 0,
    OfferPriceNoName = " عرض سعر رقم  " + x.OffersPrices!.OfferNo,
    CustomerName = x.customer == null ? "" : x.customer!.Projects == null ? "" : Lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
                : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,



});
            return projects;
        }





        public async Task<IEnumerable<ProjectVM>> GetprojectNewWorkOrder(int UserId, int BranchId, string Lang)
        {
            var projects = _TaamerProContext.Project.Where(x => x.IsDeleted == false && x.WorkOrders.Where(x=>x.IsDeleted==false && x.ExecutiveEng == UserId && (x.WOStatus == 1 || x.WOStatus == 2)).Count()>0).Select(x => new ProjectVM
                {
                    ProjectId = x.ProjectId,
                    ProjectName = x.ProjectName,
                    ProjectNo = x.ProjectNo,
                    CustomerId = x.CustomerId,
                    OffersPricesId = x.OffersPricesId ?? 0,
                    OfferPriceNoName = " عرض سعر رقم  " + x.OffersPrices!.OfferNo,
                    CustomerName = x.customer == null ? "" : x.customer!.Projects == null ? "" : Lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
                                : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,



                });
            return projects;
        }


        public async Task<IEnumerable<ProjectVM>> GetprojectNewWorkOrder(int UserId, int BranchId, string Lang, string EndDateP)
        {
            var projects = _TaamerProContext.Project.Where(x => x.IsDeleted == false && x.WorkOrders.Where(x => x.IsDeleted == false && x.ExecutiveEng == UserId && (x.WOStatus == 1 || x.WOStatus == 2)

           //&&( DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture))
           ).Count() > 0).Select(x => new ProjectVM
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectNo = x.ProjectNo,
                CustomerId = x.CustomerId,
                OffersPricesId = x.OffersPricesId ?? 0,
                OfferPriceNoName = " عرض سعر رقم  " + x.OffersPrices!.OfferNo,
                CustomerName = x.customer == null ? "" : x.customer!.Projects == null ? "" : Lang == "ltr" ? x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameEn : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameEn + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameEn + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameEn + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameEn + "(VIP)" : x.customer!.CustomerNameEn
                                : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.customer!.CustomerNameAr : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.customer!.CustomerNameAr + "(*)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.customer!.CustomerNameAr + "(**)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.customer!.CustomerNameAr + "(***)" : x.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.customer!.CustomerNameAr + "(VIP)" : x.customer!.CustomerNameAr,



            });
            return projects;
        }


        //public async Task<rptProjectStatus> GetTaskDataAsync(int projectId,string con)
        //{
        //    try
        //    {
        //        var projectData = new rptProjectStatus();
        //        var parentTaskDict = new Dictionary<int, Phase>(); // To keep track of parent tasks

        //        using (var connection = new SqlConnection(con))
        //        {
        //            using (var command = new SqlCommand("Rpt_ProjectStatus", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add(new SqlParameter("@ProjectId", projectId));

        //                await connection.OpenAsync(); // Open the connection asynchronously

        //                using (var reader = await command.ExecuteReaderAsync()) // Execute the query asynchronously
        //                {
        //                    while (await reader.ReadAsync()) // Read the data asynchronously
        //                    {
        //                        // Extract Project and Customer information
        //                        if (string.IsNullOrEmpty(projectData.ProjectName))
        //                        {
        //                            projectData.ProjectName = reader.GetString(reader.GetOrdinal("ProjectName"));
        //                            projectData.CustomerName = reader.GetString(reader.GetOrdinal("CustomerName"));
        //                        }

        //                        // Extract Parent Task details
        //                        var parentTaskId = reader.GetInt32(reader.GetOrdinal("ParentTaskId"));
        //                        if (!parentTaskDict.ContainsKey(parentTaskId))
        //                        {
        //                            var parentTask = new Phase
        //                            {
        //                                ParentTaskId = parentTaskId,
        //                                ParentTaskName = reader.GetString(reader.GetOrdinal("ParentTaskName")),
        //                                ParentStartDate = reader.GetDateTime(reader.GetOrdinal("ParentStartDate")),
        //                                ParentEndDate = reader.GetDateTime(reader.GetOrdinal("ParentEndDate")),
        //                                ParentExecPercentage = reader.GetDecimal(reader.GetOrdinal("ParentExecPercentage")),
        //                                ExpectedEndPhase = reader.GetDateTime(reader.GetOrdinal("ExpectedEndPhase")),
        //                            };
        //                            parentTaskDict[parentTaskId] = parentTask;
        //                            projectData.ParentTasks.Add(parentTask);
        //                        }

        //                        // Extract Child Task details and add to the corresponding parent
        //                        var childTask = new proTask
        //                        {
        //                            ChildTaskId = reader.GetInt32(reader.GetOrdinal("ChildTaskId")),
        //                            ChildTaskName = reader.GetString(reader.GetOrdinal("ChildTaskName")),
        //                            ChildStartDate = reader.GetDateTime(reader.GetOrdinal("ChildStartDate")),
        //                            ChildEndDate = reader.GetDateTime(reader.GetOrdinal("ChildEndDate")),
        //                            ChildEndDateCalc = reader.GetDateTime(reader.GetOrdinal("ChildEndDateCalc")),
        //                            ChildExecPercentage = reader.GetDecimal(reader.GetOrdinal("ChildExecPercentage")),
        //                            TaskStatus = reader.GetString(reader.GetOrdinal("TaskStatus")),
        //                            TimeType = reader.GetString(reader.GetOrdinal("TimeType")),
        //                            TimeMinutes = reader.GetInt32(reader.GetOrdinal("TimeMinutes")),
        //                            AssignedUserFullName = reader.GetString(reader.GetOrdinal("AssignedUserFullName")),
        //                        };

        //                        // Add the child task to the correct parent task
        //                        parentTaskDict[parentTaskId].ChildTasks.Add(childTask);
        //                    }
        //                }
        //            }
        //        }

        //        return projectData;
        //    }catch(Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        public rptProjectStatus GetTaskData(int projectId, string con)
        {
            try
            {
                var projectData = new rptProjectStatus();
                var parentTaskDict = new Dictionary<int, Phase>(); 

                using (var connection = new SqlConnection(con))
                {
                    using (var command = new SqlCommand("Rpt_ProjectStatus", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@ProjectId", projectId));

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();

                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            DataTable dataTable = dataSet.Tables[0];

                            foreach (DataRow row in dataTable.Rows)
                            {
                                if (string.IsNullOrEmpty(projectData.ProjectName))
                                {
                                    projectData.ProjectName = row["ProjectName"].ToString();
                                    projectData.CustomerName = row["CustomerName"].ToString();

                                    projectData.ProjectNo = row["ProjectNo"].ToString();
                                    projectData.PieceNo = row["PieceNo"].ToString();
                                    projectData.District = row["District"].ToString();
                                    projectData.ProjectManager = row["ProjectManager"].ToString();
                                }

                                int parentTaskId = Convert.ToInt32(row["ParentTaskId"]);
                                if (!parentTaskDict.ContainsKey(parentTaskId))
                                {
                                    var parentTask = new Phase
                                    {
                                        ParentTaskId = parentTaskId.ToString(),
                                        ParentTaskName = row["ParentTaskName"].ToString(),
                                        ParentStartDate = row["ParentStartDate"].ToString(),
                                        ParentEndDate = row["ParentEndDate"].ToString(),
                                        ParentExecPercentage = row["ParentExecPercentage"].ToString(),
                                        ExpectedEndPhase = row["ExpectedEndPhase"].ToString()
                                    };

                                    parentTaskDict[parentTaskId] = parentTask;
                                    projectData.ParentTasks.Add(parentTask);
                                }

                                var childTask = new proTask
                                {
                                    ChildTaskId = row["ChildTaskId"].ToString(),
                                    ChildTaskName = row["ChildTaskName"].ToString(),
                                    ChildStartDate = row["ChildStartDate"].ToString(),
                                    ChildEndDate = row["ChildEndDate"].ToString(),
                                    ChildEndDateCalc = row["ChildEndDateCalc"].ToString(),
                                    ChildExecPercentage = row["ChildExecPercentage"].ToString(),
                                    TaskStatus = row["TaskStatus"].ToString(),
                                    TimeType = row["TimeType"].ToString(),
                                    TimeMinutes = row["TimeMinutes"].ToString(),
                                    AssignedUserFullName = row["AssignedUserFullName"].ToString(),
                                    TimeStr = row["TimeStr"].ToString() ,
                                };

                                parentTaskDict[parentTaskId].ChildTasks.Add(childTask);
                            }
                        }
                    }
                }

                return projectData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public rptProjectStatus_phases GetTaskData_phases(int projectId, string con)
        {
            try
            {
                var projectData = new rptProjectStatus_phases();
                var parentPhaseDict = new Dictionary<int, ParentPhase>();
                var childPhaseDict = new Dictionary<int, ChildPhase>();

                using (var connection = new SqlConnection(con))
                {
                    using (var command = new SqlCommand("Rpt_ProjectStatus_phases", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@ProjectId", projectId));

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            DataTable dataTable = dataSet.Tables[0];

                            foreach (DataRow row in dataTable.Rows)
                            {
                                // Set project-level details
                                if (string.IsNullOrEmpty(projectData.ProjectName))
                                {
                                    projectData.ProjectName = row["ProjectName"].ToString();
                                    projectData.CustomerName = row["CustomerName"].ToString();
                                    projectData.ProjectNo = row["ProjectNo"].ToString();
                                    projectData.District = row["District"].ToString();
                                    projectData.ProjectManager = row["ProjectManager"].ToString();
                                }

                                // Process Parent Phase
                                int parentPhaseId = Convert.ToInt32(row["ParentPhaseId"]);
                                if (!parentPhaseDict.ContainsKey(parentPhaseId))
                                {
                                    var parentPhase = new ParentPhase
                                    {
                                        ParentPhaseId = parentPhaseId.ToString(),
                                        ParentPhaseName = row["ParentPhaseName"].ToString(),
                                        ParentStartDate = row["ParentPhaseStartDate"].ToString(),
                                        ParentEndDate = row["ParentPhaseEndDate"].ToString(),
                                        ChildTaskCount = "0" // Initialize to 0; we'll calculate it later
                                    };

                                    parentPhaseDict[parentPhaseId] = parentPhase;
                                    projectData.ParentPhases.Add(parentPhase);
                                }

                                // Process Child Phase
                                int childPhaseId = Convert.ToInt32(row["ChildPhaseId"]);
                                if (!childPhaseDict.ContainsKey(childPhaseId))
                                {
                                    var childPhase = new ChildPhase
                                    {
                                        ChildPhaseId = childPhaseId.ToString(),
                                        ChildPhaseName = row["ChildPhaseName"].ToString(),
                                        ChildStartDate = row["ChildPhaseStartDate"].ToString(),
                                        ChildEndDate = row["ChildPhaseEndDate"].ToString(),
                                        SubTaskCount = row["SubTaskCount"].ToString()
                                    };

                                    childPhaseDict[childPhaseId] = childPhase;
                                    parentPhaseDict[parentPhaseId].ChildPhases.Add(childPhase);
                                }

                                // Process Task
                                var task = new ChildPhaseTask
                                {
                                    TaskId = row["TaskId"].ToString(),
                                    TaskName = row["TaskName"].ToString(),
                                    TaskStartDate = row["TaskStartDate"].ToString(),
                                    TaskEndDate = row["TaskEndDate"].ToString(),
                                    TaskEndDateCalc = row["TaskEndDateCalc"].ToString(),
                                    TaskExecPercentage = row["TaskExecPercentage"].ToString(),
                                    TaskStatus = row["TaskStatus"].ToString(),
                                    AssignedUserFullName = row["AssignedUserFullName"].ToString(),
                                    TimeStr = row["TimeStr"].ToString()
                                };

                                childPhaseDict[childPhaseId].Tasks.Add(task);

                                // Increment Child Task Count for the Parent Phase
                                if (parentPhaseDict.ContainsKey(parentPhaseId))
                                {
                                    int currentCount = int.Parse(parentPhaseDict[parentPhaseId].ChildTaskCount);
                                    parentPhaseDict[parentPhaseId].ChildTaskCount = (currentCount + 1).ToString();
                                }
                            }
                        }
                    }
                }

                return projectData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public ProjectLocationVM GetProjectLocation(int ProjectId)
        {
            var ProjectLocationVM = _TaamerProContext.Project.Where(x => x.ProjectId == ProjectId).Select(x => new ProjectLocationVM
            {
                ProjectId=x.ProjectId,
                Latitude=x.Latitude??"0",
                Longitude=x.Longitude??"0",
                xmax=x.xmax??"0",
                xmin=x.xmin??"0",
                ymax=x.ymax??"0",
                ymin=x.ymin??"0",


            }).FirstOrDefault();
            return ProjectLocationVM;

        }


    }
}
