using System;
using System.Collections.Generic;
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
    public class WorkOrdersRepository : IWorkOrdersRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public WorkOrdersRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        //public async Task< IEnumerable<WorkOrdersVM>> GetAllProjectSubsByProjectTypeId(int ProjectTypeId, string SearchText, int BranchId)
        //{
        //    if (SearchText == "")
        //    {
        //        var Tas =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ProjectTypeId == ProjectTypeId && s.BranchId == BranchId).Select(x => new ProjectSubTypeVM
        //        {
        //            SubTypeId = x.SubTypeId,
        //            NameAr = x.NameAr,
        //            NameEn = x.NameEn,
        //            ProjectTypeId = x.ProjectTypeId
        //        }).ToList();
        //        return Tas;
        //    }
        //    else
        //    {
        //        var Tas =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.ProjectTypeId == ProjectTypeId && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new ProjectSubTypeVM
        //        {
        //            SubTypeId = x.SubTypeId,
        //            NameAr = x.NameAr,
        //            NameEn = x.NameEn,
        //            ProjectTypeId = x.ProjectTypeId
        //        }).ToList();
        //        return Tas;
        //    }
        //}
        public async Task< IEnumerable<WorkOrdersVM>> GetAllWorkOrdersForAdmin(int BranchId) 
        {
            IEnumerable<WorkOrdersVM> wo = new List<WorkOrdersVM>();
            try { 
            var workorders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId== BranchId).Select(x => new WorkOrdersVM
            {
                WorkOrderId = x.WorkOrderId,

                OrderNo = x.OrderNo,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
                OrderHijriDate = x.OrderHijriDate,
                ResponsibleEng = x.ResponsibleEng,
                ExecutiveEng = x.ExecutiveEng,
                CustomerId = x.CustomerId,
                Mediator = x.Mediator,
                Required = x.Required,
                Note = x.Note== "null"?"":x.Note??"",
                PhasePriority=x.PhasePriority,
                PhasePriorityName=x.PhasePriority==1? "منخفض": x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل":"بدون",
                OrderValue = x.OrderValue,
                OrderPaid = x.OrderPaid,
                OrderRemaining = x.OrderRemaining,
                OrderDiscount = x.OrderDiscount,
                OrderTax = x.OrderTax,
                OrderValueAfterTax = x.OrderValueAfterTax,
                DiscountReason = x.DiscountReason,
                Sketch = x.Sketch,
                District = x.District,
                Location = x.Location,
                PieceNo = x.PieceNo,
                InstrumentNo = x.InstrumentNo,
                ExecutiveType = x.ExecutiveType,
                ContractNo = x.ContractNo,
                AgentId = x.AgentId,
                AgentMobile = x.AgentMobile,
                Social = x.Social,
                BranchId = x.BranchId,
                UserName = x.User!.FullName,
                EndDate = x.EndDate!.ToString(),
                WOStatus = x.WOStatus,
                //TaskStatusName=x.Status.NameAr,
                WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                CustomerName_W = x.Customer!.CustomerNameAr,
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,
                ProjectNo = x.Project!.ProjectNo ?? "بدون مشروع",
                AttatchmentUrl =x.AttatchmentUrl??"",
                ProjectId=x.ProjectId,
                StopProjectType=x.Project!.StopProjectType,
                IsConverted = x.IsConverted??0,
                PlusTime = x.PlusTime??false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ContactLists=x.ContactLists ,
            }).ToList();
            return workorders;
            }
            catch(Exception ex)
            {
                return wo;
            }
        }

        public async Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersForAdminByCustomer(int BranchId,int? CustomerId)
        {
            IEnumerable<WorkOrdersVM> wo = new List<WorkOrdersVM>();
            try
            {
                var workorders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.WOStatus != 3
                && (CustomerId==null || CustomerId==0 || s.CustomerId== CustomerId)).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,

                    OrderNo = x.OrderNo,
                    UserId = x.UserId,
                    OrderDate = x.OrderDate,
                    OrderHijriDate = x.OrderHijriDate,
                    ResponsibleEng = x.ResponsibleEng,
                    ExecutiveEng = x.ExecutiveEng,
                    CustomerId = x.CustomerId,
                    Mediator = x.Mediator,
                    Required = x.Required,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    OrderValue = x.OrderValue,
                    OrderPaid = x.OrderPaid,
                    OrderRemaining = x.OrderRemaining,
                    OrderDiscount = x.OrderDiscount,
                    OrderTax = x.OrderTax,
                    OrderValueAfterTax = x.OrderValueAfterTax,
                    DiscountReason = x.DiscountReason,
                    Sketch = x.Sketch,
                    District = x.District,
                    Location = x.Location,
                    PieceNo = x.PieceNo,
                    InstrumentNo = x.InstrumentNo,
                    ExecutiveType = x.ExecutiveType,
                    ContractNo = x.ContractNo,
                    AgentId = x.AgentId,
                    AgentMobile = x.AgentMobile,
                    Social = x.Social,
                    BranchId = x.BranchId,
                    UserName = x.User!.FullName,
                    EndDate = x.EndDate!.ToString(),
                    WOStatus = x.WOStatus,
                    //TaskStatusName=x.Status.NameAr,
                    WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    CustomerName_W = x.Customer!.CustomerNameAr,
                    CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,

                    ProjectNo = x.Project!.ProjectNo ?? "بدون مشروع",
                    AttatchmentUrl = x.AttatchmentUrl ?? "",
                    ProjectId = x.ProjectId,
                    StopProjectType = x.Project!.StopProjectType,
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                    ContactLists=x.ContactLists,
                }).ToList();
                return workorders;
            }
            catch (Exception ex)
            {
                return wo;
            }
        }


        public async Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersForAdminByCustomer(int BranchId, int? CustomerId,string? SearchText)
        {
            IEnumerable<WorkOrdersVM> wo = new List<WorkOrdersVM>();
            try
            {
                var workorders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.WOStatus != 3
                && (CustomerId == null || CustomerId == 0 || s.CustomerId == CustomerId) && (s.OrderNo.Contains(SearchText)||
                s.Required.Contains(SearchText) || s.Project.ProjectNo.Contains(SearchText) || SearchText ==null ||SearchText =="")).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,

                    OrderNo = x.OrderNo,
                    UserId = x.UserId,
                    OrderDate = x.OrderDate,
                    OrderHijriDate = x.OrderHijriDate,
                    ResponsibleEng = x.ResponsibleEng,
                    ExecutiveEng = x.ExecutiveEng,
                    CustomerId = x.CustomerId,
                    Mediator = x.Mediator,
                    Required = x.Required,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    OrderValue = x.OrderValue,
                    OrderPaid = x.OrderPaid,
                    OrderRemaining = x.OrderRemaining,
                    OrderDiscount = x.OrderDiscount,
                    OrderTax = x.OrderTax,
                    OrderValueAfterTax = x.OrderValueAfterTax,
                    DiscountReason = x.DiscountReason,
                    Sketch = x.Sketch,
                    District = x.District,
                    Location = x.Location,
                    PieceNo = x.PieceNo,
                    InstrumentNo = x.InstrumentNo,
                    ExecutiveType = x.ExecutiveType,
                    ContractNo = x.ContractNo,
                    AgentId = x.AgentId,
                    AgentMobile = x.AgentMobile,
                    Social = x.Social,
                    BranchId = x.BranchId,
                    UserName = x.User!.FullName,
                    EndDate = x.EndDate!.ToString(),
                    WOStatus = x.WOStatus,
                    //TaskStatusName=x.Status.NameAr,
                    WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    CustomerName_W = x.Customer!.CustomerNameAr,
                    CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,

                    ProjectNo = x.Project!.ProjectNo ?? "بدون مشروع",
                    AttatchmentUrl = x.AttatchmentUrl ?? "",
                    ProjectId = x.ProjectId,
                    StopProjectType = x.Project!.StopProjectType,
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                    ContactLists=x.ContactLists,
                }).ToList();
                return workorders;
            }
            catch (Exception ex)
            {
                return wo;
            }
        }



        public async Task< IEnumerable<WorkOrdersVM>> GetAllWorkOrdersyProjectId(int ProjectId)
        {
            var workorders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new WorkOrdersVM
            {
                WorkOrderId = x.WorkOrderId,
                OrderNo = x.OrderNo,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
                OrderHijriDate = x.OrderHijriDate,
                ResponsibleEng = x.ResponsibleEng,
                ExecutiveEng = x.ExecutiveEng,
                CustomerId = x.CustomerId,
                Mediator = x.Mediator,
                Required = x.Required,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                OrderValue = x.OrderValue,
                OrderPaid = x.OrderPaid,
                OrderRemaining = x.OrderRemaining,
                OrderDiscount = x.OrderDiscount,
                OrderTax = x.OrderTax,
                OrderValueAfterTax = x.OrderValueAfterTax,
                DiscountReason = x.DiscountReason,
                Sketch = x.Sketch,
                District = x.District,
                Location = x.Location,
                PieceNo = x.PieceNo,
                InstrumentNo = x.InstrumentNo,
                ExecutiveType = x.ExecutiveType,
                ContractNo = x.ContractNo,
                AgentId = x.AgentId,
                AgentMobile = x.AgentMobile,
                Social = x.Social,
                BranchId = x.BranchId,
                UserName = x.User!.FullName,
                EndDate = x.EndDate!.ToString(),
                WOStatus = x.WOStatus,
                //TaskStatusName=x.Status.NameAr,
                StrTime= x.NoOfDays+ " يوم ",
                WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                CustomerName_W = x.Customer!.CustomerNameAr,
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,

                ProjectNo = x.Project!.ProjectNo,
                AttatchmentUrl = x.AttatchmentUrl??"",
                ProjectId = x.ProjectId,
                StopProjectType = x.Project!.StopProjectType,
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
            }).ToList();
            return workorders;
        }
        public async Task< IEnumerable<WorkOrdersVM>> GetAllWorkOrders(int BranchId)
        {
            var workorders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId==BranchId).Select(x => new WorkOrdersVM
            {
                WorkOrderId = x.WorkOrderId,
                OrderNo = x.OrderNo,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
                OrderHijriDate = x.OrderHijriDate,
                ResponsibleEng = x.ResponsibleEng,
                ExecutiveEng = x.ExecutiveEng,
                CustomerId = x.CustomerId,
                Mediator = x.Mediator,
                Required = x.Required,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                OrderValue = x.OrderValue,
                OrderPaid = x.OrderPaid,
                OrderRemaining = x.OrderRemaining,
                OrderDiscount = x.OrderDiscount,
                OrderTax = x.OrderTax,
                OrderValueAfterTax = x.OrderValueAfterTax,
                DiscountReason = x.DiscountReason,
                Sketch = x.Sketch,
                District = x.District,
                Location = x.Location,
                PieceNo = x.PieceNo,
                InstrumentNo = x.InstrumentNo,
                ExecutiveType = x.ExecutiveType,
                ContractNo = x.ContractNo,
                AgentId = x.AgentId,
                AgentMobile = x.AgentMobile,
                Social = x.Social,
                BranchId = x.BranchId,
                UserName = x.User!.FullName,
                EndDate=x.EndDate!.ToString(),
                WOStatus=x.WOStatus,
                //TaskStatusName=x.Status.NameAr,
                WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                CustomerName_W = x.Customer!.CustomerNameAr,
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,

                ProjectNo = x.Project!.ProjectNo,
                ProjectId = x.ProjectId,
                AttatchmentUrl = x.AttatchmentUrl ?? "",

                StopProjectType = x.Project!.StopProjectType,
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ContactLists = x.ContactLists,

            }).ToList();
            return workorders;
        }

        public async Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersByCustomer(int BranchId,int? CustomerId)
        {
            var workorders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.WOStatus != 3
            && (CustomerId ==null || CustomerId==0 || s.CustomerId== CustomerId)).Select(x => new WorkOrdersVM
            {
                WorkOrderId = x.WorkOrderId,
                OrderNo = x.OrderNo,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
                OrderHijriDate = x.OrderHijriDate,
                ResponsibleEng = x.ResponsibleEng,
                ExecutiveEng = x.ExecutiveEng,
                CustomerId = x.CustomerId,
                Mediator = x.Mediator,
                Required = x.Required,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                OrderValue = x.OrderValue,
                OrderPaid = x.OrderPaid,
                OrderRemaining = x.OrderRemaining,
                OrderDiscount = x.OrderDiscount,
                OrderTax = x.OrderTax,
                OrderValueAfterTax = x.OrderValueAfterTax,
                DiscountReason = x.DiscountReason,
                Sketch = x.Sketch,
                District = x.District,
                Location = x.Location,
                PieceNo = x.PieceNo,
                InstrumentNo = x.InstrumentNo,
                ExecutiveType = x.ExecutiveType,
                ContractNo = x.ContractNo,
                AgentId = x.AgentId,
                AgentMobile = x.AgentMobile,
                Social = x.Social,
                BranchId = x.BranchId,
                UserName = x.User!.FullName,
                EndDate = x.EndDate!.ToString(),
                WOStatus = x.WOStatus,
                //TaskStatusName=x.Status.NameAr,
                WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                CustomerName_W = x.Customer!.CustomerNameAr,
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,

                ProjectNo = x.Project!.ProjectNo,
                ProjectId = x.ProjectId,
                AttatchmentUrl = x.AttatchmentUrl ?? "",
                StopProjectType = x.Project!.StopProjectType,
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
            ContactLists=x.ContactLists,

            }).ToList();
            return workorders;
        }
        public async Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersByCustomer(int BranchId, int? CustomerId, string? SearchText)
        {
            var workorders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.WOStatus != 3
            && (CustomerId == null || CustomerId == 0 || s.CustomerId == CustomerId) && (s.OrderNo.Contains(SearchText) ||
                s.Required.Contains(SearchText) || s.Project.ProjectNo.Contains(SearchText) || SearchText == null || SearchText == "")).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    UserId = x.UserId,
                    OrderDate = x.OrderDate,
                    OrderHijriDate = x.OrderHijriDate,
                    ResponsibleEng = x.ResponsibleEng,
                    ExecutiveEng = x.ExecutiveEng,
                    CustomerId = x.CustomerId,
                    Mediator = x.Mediator,
                    Required = x.Required,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    OrderValue = x.OrderValue,
                    OrderPaid = x.OrderPaid,
                    OrderRemaining = x.OrderRemaining,
                    OrderDiscount = x.OrderDiscount,
                    OrderTax = x.OrderTax,
                    OrderValueAfterTax = x.OrderValueAfterTax,
                    DiscountReason = x.DiscountReason,
                    Sketch = x.Sketch,
                    District = x.District,
                    Location = x.Location,
                    PieceNo = x.PieceNo,
                    InstrumentNo = x.InstrumentNo,
                    ExecutiveType = x.ExecutiveType,
                    ContractNo = x.ContractNo,
                    AgentId = x.AgentId,
                    AgentMobile = x.AgentMobile,
                    Social = x.Social,
                    BranchId = x.BranchId,
                    UserName = x.User!.FullName,
                    EndDate = x.EndDate!.ToString(),
                    WOStatus = x.WOStatus,
                    //TaskStatusName=x.Status.NameAr,
                    WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    CustomerName_W = x.Customer!.CustomerNameAr,
                    CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,

                    ProjectNo = x.Project!.ProjectNo,
                    ProjectId = x.ProjectId,
                    AttatchmentUrl = x.AttatchmentUrl ?? "",
                    StopProjectType = x.Project!.StopProjectType,
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
               ContactLists=x.ContactLists,
                }).ToList();
            return workorders;
        }

        public async Task<IEnumerable<WorkOrdersVM>> SearchWorkOrders(WorkOrdersVM WorkOrdersSearch, string lang, int BranchId)
        {
            if (WorkOrdersSearch.DateFrom != null && WorkOrdersSearch.DateTo != null)
            {
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.OrderNo == WorkOrdersSearch.OrderNo || WorkOrdersSearch.OrderNo == null)
                                             && (s.ResponsibleEng == WorkOrdersSearch.ResponsibleEng || WorkOrdersSearch.ResponsibleEng == null)
                                             && (s.ExecutiveEng == WorkOrdersSearch.ExecutiveEng || WorkOrdersSearch.ExecutiveEng == null)
                                             && (s.CustomerId == WorkOrdersSearch.CustomerId || WorkOrdersSearch.CustomerId == null)
                                             && (s.Required == WorkOrdersSearch.Required || WorkOrdersSearch.Required == null || s.Required.Contains(WorkOrdersSearch.Required))
                                             && ((s.WOStatus == WorkOrdersSearch.WOStatus || WorkOrdersSearch.WOStatus == null || WorkOrdersSearch.WOStatus == 0) && s.WOStatus != 8))
                                                    .Select(x => new WorkOrdersVM
                                                    {
                                                        WorkOrderId = x.WorkOrderId,
                                                        OrderNo = x.OrderNo,
                                                        UserId = x.UserId,
                                                        OrderDate = x.OrderDate,
                                                        OrderHijriDate = x.OrderHijriDate,
                                                        ResponsibleEng = x.ResponsibleEng,
                                                        ExecutiveEng = x.ExecutiveEng,
                                                        CustomerId = x.CustomerId,
                                                        Mediator = x.Mediator,
                                                        Required = x.Required,
                                                        Note = x.Note == "null" ? "" : x.Note ?? "",
                                                        PhasePriority = x.PhasePriority,
                                                        PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                                                        OrderValue = x.OrderValue,
                                                        OrderPaid = x.OrderPaid,
                                                        OrderRemaining = x.OrderRemaining,
                                                        OrderDiscount = x.OrderDiscount,
                                                        OrderTax = x.OrderTax,
                                                        OrderValueAfterTax = x.OrderValueAfterTax,
                                                        DiscountReason = x.DiscountReason,
                                                        Sketch = x.Sketch,
                                                        District = x.District,
                                                        Location = x.Location,
                                                        PieceNo = x.PieceNo,
                                                        InstrumentNo = x.InstrumentNo,
                                                        ExecutiveType = x.ExecutiveType,
                                                        ContractNo = x.ContractNo,
                                                        AgentId = x.AgentId,
                                                        AgentMobile = x.AgentMobile,
                                                        Social = x.Social,
                                                        BranchId = x.BranchId,
                                                        UserName = x.User!.FullName,
                                                        ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                                                        ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                                                        ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                                                        ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                                                        WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                                                        EndDate = x.EndDate!.ToString(),


                                                        CustomerName_W = x.Customer!.CustomerNameAr,
                                                        CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,
                                                        AttatchmentUrl = x.AttatchmentUrl ?? "",

                                                        ProjectNo = x.Project!.ProjectNo,
                                                        ProjectId = x.ProjectId,
                                                        StopProjectType = x.Project!.StopProjectType,
                                                        IsConverted = x.IsConverted ?? 0,
                                                        PlusTime = x.PlusTime ?? false,
                                                        AddOrderName = x.User!.FullNameAr ?? "",
                                                        AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                                                        ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                                                        ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                                                    }).ToList().Where(s => DateTime.ParseExact(s.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(WorkOrdersSearch.DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(WorkOrdersSearch.DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));

                if (WorkOrdersSearch.WOStatus == 8)
                {
                    WorkOrders = WorkOrders.ToList().Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(WorkOrdersSearch.DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    return WorkOrders;
                }
                else {
                    return WorkOrders; }
            }
            else
            {
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.OrderNo == WorkOrdersSearch.OrderNo || WorkOrdersSearch.OrderNo == null)
                                             && (s.ResponsibleEng == WorkOrdersSearch.ResponsibleEng || WorkOrdersSearch.ResponsibleEng == null)
                                             && (s.ExecutiveEng == WorkOrdersSearch.ExecutiveEng || WorkOrdersSearch.ExecutiveEng == null)
                                             && (s.CustomerId == WorkOrdersSearch.CustomerId || WorkOrdersSearch.CustomerId == null)
                                             && (s.Required == WorkOrdersSearch.Required || WorkOrdersSearch.Required == null || s.Required.Contains(WorkOrdersSearch.Required))
                                             && ((s.WOStatus == WorkOrdersSearch.WOStatus || WorkOrdersSearch.WOStatus == null || WorkOrdersSearch.WOStatus == 0) && s.WOStatus != 8))
                                                    .Select(x => new WorkOrdersVM
                                                    {
                                                        WorkOrderId = x.WorkOrderId,
                                                        OrderNo = x.OrderNo,
                                                        UserId = x.UserId,
                                                        OrderDate = x.OrderDate,
                                                        OrderHijriDate = x.OrderHijriDate,
                                                        ResponsibleEng = x.ResponsibleEng,
                                                        ExecutiveEng = x.ExecutiveEng,
                                                        CustomerId = x.CustomerId,
                                                        Mediator = x.Mediator,
                                                        Required = x.Required,
                                                        Note = x.Note == "null" ? "" : x.Note ?? "",
                                                        PhasePriority = x.PhasePriority,
                                                        PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                                                        OrderValue = x.OrderValue,
                                                        OrderPaid = x.OrderPaid,
                                                        OrderRemaining = x.OrderRemaining,
                                                        OrderDiscount = x.OrderDiscount,
                                                        OrderTax = x.OrderTax,
                                                        OrderValueAfterTax = x.OrderValueAfterTax,
                                                        DiscountReason = x.DiscountReason,
                                                        Sketch = x.Sketch,
                                                        District = x.District,
                                                        Location = x.Location,
                                                        PieceNo = x.PieceNo,
                                                        InstrumentNo = x.InstrumentNo,
                                                        ExecutiveType = x.ExecutiveType,
                                                        ContractNo = x.ContractNo,
                                                        AgentId = x.AgentId,
                                                        AgentMobile = x.AgentMobile,
                                                        Social = x.Social,
                                                        BranchId = x.BranchId,
                                                        UserName = x.User!.FullName,
                                                        ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                                                        ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                                                        ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                                                        ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                                                        WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                                                        EndDate = x.EndDate!.ToString(),


                                                        CustomerName_W = x.Customer!.CustomerNameAr,
                                                        CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,
                                                        AttatchmentUrl = x.AttatchmentUrl ?? "",

                                                        ProjectNo = x.Project!.ProjectNo,
                                                        ProjectId = x.ProjectId,
                                                        StopProjectType = x.Project!.StopProjectType,
                                                        IsConverted = x.IsConverted ?? 0,
                                                        PlusTime = x.PlusTime ?? false,
                                                        AddOrderName = x.User!.FullNameAr ?? "",
                                                        AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                                                        ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                                                        ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                                                    }).ToList();

                if (WorkOrdersSearch.WOStatus == 8)
                {
                    WorkOrders = WorkOrders.ToList().Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
                    return WorkOrders;
                }
                else
                {
                    return WorkOrders;
                }
            }
        

        }
    public async Task< int> GetMaxOrderNumber()
        {
            if ( _TaamerProContext.WorkOrders != null)
            {
                var lastRow =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted ==false).OrderByDescending(u => u.WorkOrderId).Take(1).FirstOrDefault();


                if (lastRow != null)
                {
                    try
                    {
                        return int.Parse(lastRow.OrderNo) + 1;
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
        public async Task< IEnumerable<WorkOrdersVM>> GetAllWorkOrdersByDateSearchForAdmin(string DateFrom, string DateTo, int BranchId)
        {
            var Projects =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId==BranchId).Select(x => new
            {
                x.WorkOrderId,
                x.OrderNo,
                x.UserId,
                x.OrderDate,
                x.OrderHijriDate,
                x.ResponsibleEng,
                x.ExecutiveEng,
                x.CustomerId,
                x.Mediator,
                x.Required,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                x.OrderValue,
                x.OrderPaid,
                x.OrderRemaining,
                x.OrderDiscount,
                x.OrderTax,
                x.OrderValueAfterTax,
                x.DiscountReason,
                x.Sketch,
                x.District,
                x.Location,
                x.PieceNo,
                x.InstrumentNo,
                x.ExecutiveType,
                x.ContractNo,
                x.AgentId,
                x.AgentMobile,
                x.Social,
                x.BranchId,
                x.EndDate,
                x.WOStatus,
                UserName = x.User!.FullName,
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                CustomerName_W = x.Customer!.CustomerNameAr,
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,
                AttatchmentUrl = x.AttatchmentUrl ?? "",
                WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl!=null ?x.User.ImgUrl: "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",


            }).Select(m => new WorkOrdersVM
            {
                WorkOrderId = m.WorkOrderId,
                OrderNo = m.OrderNo,
                UserId = m.UserId,
                OrderDate = m.OrderDate,
                OrderHijriDate = m.OrderHijriDate,
                ResponsibleEng = m.ResponsibleEng,
                ExecutiveEng = m.ExecutiveEng,
                CustomerId = m.CustomerId,
                Mediator = m.Mediator,
                Required = m.Required,
                Note = m.Note,
                PhasePriority = m.PhasePriority,
                PhasePriorityName = m.PhasePriorityName,
                OrderValue = m.OrderValue,
                OrderPaid = m.OrderPaid,
                OrderRemaining = m.OrderRemaining,
                OrderDiscount = m.OrderDiscount,
                OrderTax = m.OrderTax,
                OrderValueAfterTax = m.OrderValueAfterTax,
                DiscountReason = m.DiscountReason,
                Sketch = m.Sketch,
                District = m.District,
                Location = m.Location,
                PieceNo = m.PieceNo,
                InstrumentNo = m.InstrumentNo,
                ExecutiveType = m.ExecutiveType,
                ContractNo = m.ContractNo,
                AgentId = m.AgentId,
                AgentMobile = m.AgentMobile,
                Social = m.Social,
                BranchId = m.BranchId,
                UserName = m.UserName,
                EndDate = m.EndDate!.ToString(),
                WOStatus = m.WOStatus,
                ResponsibleEngName = m.ResponsibleEngName,
                ResponsibleEngImg = m.ResponsibleEngImg,
                ExecutiveEngName = m.ExecutiveEngName,
                ExecutiveEngImg = m.ExecutiveEngImg,
                CustomerName = m.CustomerName,
                CustomerName_W=m.CustomerName_W,
                WOStatustxt = m.WOStatustxt,
                AttatchmentUrl = m.AttatchmentUrl ?? "",
                IsConverted = m.IsConverted,
                PlusTime = m.PlusTime,
                AddOrderName = m.AddOrderName,
                AddOrderImg = m.AddOrderImg,
                ProjectMangerName = m.ProjectMangerName,
                ProjectManagerImg = m.ProjectManagerImg,


            }).ToList().Where(s => DateTime.ParseExact(s.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))
            ;
            return Projects;
        }
        public async Task< IEnumerable<WorkOrdersVM>> GetAllWorkOrdersByDateSearch(string DateFrom, string DateTo, int BranchId)
        {
            var Projects =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId==BranchId).Select(x => new
            {
                x.WorkOrderId,
                x.OrderNo,
                x.UserId,
                x.OrderDate,
                x.OrderHijriDate,
                x.ResponsibleEng,
                x.ExecutiveEng,
                x.CustomerId,
                x.Mediator,
                x.Required,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                x.OrderValue,
                x.OrderPaid,
                x.OrderRemaining,
                x.OrderDiscount,
                x.OrderTax,
                x.OrderValueAfterTax,
                x.DiscountReason,
                x.Sketch,
                x.District,
                x.Location,
                x.PieceNo,
                x.InstrumentNo,
                x.ExecutiveType,
                x.ContractNo,
                x.AgentId,
                x.AgentMobile,
                x.Social,
                x.BranchId,
                x.EndDate,
                x.WOStatus,
                UserName = x.User!.FullName,
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                CustomerName_W = x.Customer!.CustomerNameAr,
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,
                AttatchmentUrl = x.AttatchmentUrl ?? "",

                WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",

            }).Select(m => new WorkOrdersVM
            {
                WorkOrderId = m.WorkOrderId,
                OrderNo = m.OrderNo,
                UserId = m.UserId,
                OrderDate = m.OrderDate,
                OrderHijriDate = m.OrderHijriDate,
                ResponsibleEng = m.ResponsibleEng,
                ExecutiveEng = m.ExecutiveEng,
                CustomerId = m.CustomerId,
                Mediator = m.Mediator,
                Required = m.Required,
                Note = m.Note,
                PhasePriority = m.PhasePriority,
                PhasePriorityName = m.PhasePriorityName,
                OrderValue = m.OrderValue,
                OrderPaid = m.OrderPaid,
                OrderRemaining = m.OrderRemaining,
                OrderDiscount = m.OrderDiscount,
                OrderTax = m.OrderTax,
                OrderValueAfterTax = m.OrderValueAfterTax,
                DiscountReason = m.DiscountReason,
                Sketch = m.Sketch,
                District = m.District,
                Location = m.Location,
                PieceNo = m.PieceNo,
                InstrumentNo = m.InstrumentNo,
                ExecutiveType = m.ExecutiveType,
                ContractNo = m.ContractNo,
                AgentId = m.AgentId,
                AgentMobile = m.AgentMobile,
                Social = m.Social,
                BranchId = m.BranchId,
                UserName = m.UserName,
                EndDate=m.EndDate!.ToString(),
                WOStatus=m.WOStatus,
                ResponsibleEngName = m.ResponsibleEngName,
                ResponsibleEngImg = m.ResponsibleEngImg,
                ExecutiveEngName = m.ExecutiveEngName,
                ExecutiveEngImg = m.ExecutiveEngImg,
                CustomerName = m.CustomerName,
                CustomerName_W=m.CustomerName_W,
                WOStatustxt = m.WOStatustxt,
                AttatchmentUrl = m.AttatchmentUrl ?? "",
                IsConverted = m.IsConverted,
                PlusTime = m.PlusTime,
                AddOrderName=m.AddOrderName,
                AddOrderImg = m.AddOrderImg,
                ProjectMangerName = m.ProjectMangerName,
                ProjectManagerImg = m.ProjectManagerImg,

            }).ToList().Where(s => DateTime.ParseExact(s.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))
            ;
            return Projects;
        }
        public async Task< WorkOrdersVM> GetWorkOrderById(int WorkOrderId, string lang)
        {
            try
            {
                var emp =  _TaamerProContext.WorkOrders.Where(s => s.WorkOrderId == WorkOrderId).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    UserId = x.UserId,
                    OrderDate = x.OrderDate,
                    OrderHijriDate = x.OrderHijriDate,
                    ResponsibleEng = x.ResponsibleEng,
                    ExecutiveEng = x.ExecutiveEng,
                    CustomerId = x.CustomerId,
                    Mediator = x.Mediator,
                    Required = x.Required,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    OrderValue = x.OrderValue,
                    OrderPaid = x.OrderPaid,
                    OrderRemaining = x.OrderRemaining,
                    OrderDiscount = x.OrderDiscount,
                    OrderTax = x.OrderTax,
                    OrderValueAfterTax = x.OrderValueAfterTax,
                    DiscountReason = x.DiscountReason,
                    Sketch = x.Sketch,
                    District = x.District,
                    Location = x.Location,
                    PieceNo = x.PieceNo,
                    InstrumentNo = x.InstrumentNo,
                    ExecutiveType = x.ExecutiveType,
                    ContractNo = x.ContractNo,
                    AgentId = x.AgentId ?? "",
                    AgentMobile = x.AgentMobile,
                    Social = x.Social,
                    BranchId = x.BranchId,
                    EndDate = x.EndDate!.ToString(),
                    WOStatus = x.WOStatus,
                    WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                    UserName = x.User!.FullName,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project!.ProjectNo,
                    AttatchmentUrl = x.AttatchmentUrl ?? "",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    StopProjectType = x.Project!.StopProjectType,
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",


                }).First();
                return emp;
            }
            catch (Exception)
            {
                return new WorkOrdersVM();
                
            }
            
        }
        public async Task<decimal?> GetWorkOrderCountByStatus(int? UserId, int Status, int BranchId)
        {
            if (UserId == null)
            {
                decimal AllWorkworder =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false).Count();
                decimal UserWorkOrder =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.WOStatus == Status).Count();
                //decimal result = (UserWorkOrder / AllWorkworder) * 100;
                //return result;
                decimal result = 0;
                if (AllWorkworder != 0)
                {
                    result = (UserWorkOrder / AllWorkworder) * 100;
                }
                return result;
                // /  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.WOStatus == Status).Count();
            }
            else
            {
                decimal TotalWorkOrder =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId).Count();
                decimal UserWorkOrder =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.WOStatus == Status && s.ExecutiveEng == UserId).Count();
                //decimal result = (UserWorkOrder / TotalWorkOrder) * 100;
                //return result;
                decimal result = 0;
                if (TotalWorkOrder != 0)
                {
                    result = (UserWorkOrder / TotalWorkOrder) * 100;
                }
                return result;
                // /  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.WOStatus == Status && s.UserId == UserId).Count();
            }
        }
         public async Task<decimal> GetLateWorkOrdersCount(string EndDateP, int? UserId, int BranchId)
        {
            decimal TotalWorkOrder =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId).Count();

            if (EndDateP != "")
            {
                decimal WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && s.WOStatus != 3).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    EndDate = x.EndDate
                }).ToList().Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Count();
                decimal result = 0;
                if (TotalWorkOrder != 0)
                {
                    result = (WorkOrders / TotalWorkOrder) * 100;
                }
                return result;
            }
            else
            {
                decimal WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate
                }).ToList().Count();
                decimal result = 0;
                if (TotalWorkOrder != 0)
                {
                    result = (WorkOrders / TotalWorkOrder) * 100;
                }
                return result;
            };
        }
        public async Task< IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserId(string EndDateP, int? UserId, int BranchId)
        {
            if (EndDateP != "")
            {
                var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && (s.WOStatus == 1 || s.WOStatus == 2)).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    EndDate = x.EndDate,
                    ProjectNo = x.Project!.ProjectNo,
                    ProjectId = x.ProjectId,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",

                }).ToList().Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                return WorkOrders;
            }
            else
            {
                var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    ProjectNo = x.Project!.ProjectNo,
                    ProjectId = x.ProjectId,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                });
                return WorkOrders;
            };
        }

        public async Task<IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId,int? CustomerId,int? ProjectId)
        {
            if (EndDateP != "")
            {
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && (s.WOStatus == 1 || s.WOStatus == 2)
                && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId)).Select(x => new WorkOrdersVM
                {
                WorkOrderId = x.WorkOrderId,

                OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    EndDate = x.EndDate,
                    ProjectNo = x.Project!.ProjectNo,
                    ProjectId = x.ProjectId,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                ByUser =x.User.FullNameAr,
                    CustomerName=x.Customer.CustomerNameAr,
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                Duration = ((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days).ToString() =="0" ? "1 يوم" : ((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days).ToString() +"يوم",
                    OrderRemaining=x.OrderRemaining,
                }).ToList().Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                return WorkOrders;
            }
            else
            {
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId
                && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId)).Select(x => new WorkOrdersVM
                {
                WorkOrderId = x.WorkOrderId,

                OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    ProjectNo = x.Project!.ProjectNo,
                    ProjectId = x.ProjectId,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                ByUser = x.User.FullNameAr,
                CustomerName = x.Customer.CustomerNameAr,
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                Duration = ((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days).ToString() == "0" ? "1 يوم" : ((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days).ToString() + "يوم",
                EndDate = x.EndDate,
                OrderRemaining = x.OrderRemaining,
            });
                return WorkOrders;
            };
        }



        public async Task<IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? CustomerId, int? ProjectId,string? SearchText)
        {
            if (EndDateP != "")
            {
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && (s.WOStatus == 1 || s.WOStatus == 2)
                && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId) && (s.OrderNo.Contains(SearchText) || s.Required.Contains(SearchText) ||
            SearchText == null || SearchText == "")).Select(x => new WorkOrdersVM
            {
                WorkOrderId = x.WorkOrderId,

                OrderNo = x.OrderNo,
                Required = x.Required,
                OrderDate = x.OrderDate,
                EndDate = x.EndDate,
                ProjectNo = x.Project!.ProjectNo,
                ProjectId = x.ProjectId,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                ByUser = x.User.FullNameAr,
                CustomerName = x.Customer.CustomerNameAr,
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                Duration = ((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days).ToString() == "0" ? "1 يوم" : ((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days).ToString() + "يوم",
                OrderRemaining = x.OrderRemaining,
                ContactLists=x.ContactLists,
            }).ToList().Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                return WorkOrders;
            }
            else
            {
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId
                && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId) && (s.OrderNo.Contains(SearchText) || s.Required.Contains(SearchText) ||
            SearchText == null || SearchText == "")).Select(x => new WorkOrdersVM
            {
                WorkOrderId = x.WorkOrderId,

                OrderNo = x.OrderNo,
                Required = x.Required,
                OrderDate = x.OrderDate,
                ProjectNo = x.Project!.ProjectNo,
                ProjectId = x.ProjectId,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                ByUser = x.User.FullNameAr,
                CustomerName = x.Customer.CustomerNameAr,
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                Duration = ((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days).ToString() == "0" ? "1 يوم" : ((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days).ToString() + "يوم",
                EndDate = x.EndDate,
                OrderRemaining = x.OrderRemaining,
                ContactLists = x.ContactLists,
            });
                return WorkOrders;
            };
        }



        public async Task< IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserId(string EndDateP, int? UserId, int BranchId)
        {
            if (EndDateP != "")
            {
                var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && (s.WOStatus ==1 || s.WOStatus == 2)).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,

                    OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    EndDate = x.EndDate,
                    ProjectNo = x.Project!.ProjectNo ??"بدون مشروع",
                    ProjectId = x.ProjectId,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                }).ToList().Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                
                return WorkOrders;
            }
            else
            {
                var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,

                    OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    ProjectNo = x.Project!.ProjectNo,
                    ProjectId = x.ProjectId,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                });
                return WorkOrders;
            };
        }


        public async Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? CustomerId, int? ProjectId)
        {
            if (EndDateP != "")
            {
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && (s.WOStatus == 1 || s.WOStatus == 2)
                  && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId)).Select(x => new WorkOrdersVM
                {
                    WorkOrderId=x.WorkOrderId,
                OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    EndDate = x.EndDate,
                    ProjectNo = x.Project!.ProjectNo ?? "بدون مشروع",
                    ProjectId = x.ProjectId,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ContactLists=x.ContactLists,
            }).ToList().Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));

                return WorkOrders;
            }
            else
            {
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId
                  && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId)).Select(x => new WorkOrdersVM
                {
                WorkOrderId = x.WorkOrderId,
                OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    ProjectNo = x.Project!.ProjectNo,
                    ProjectId = x.ProjectId,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ContactLists=x.ContactLists,
            });
                return WorkOrders;
            };
        }


        public async Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? CustomerId, int? ProjectId, string? SearchText)
        {
            if (EndDateP != "")
            {
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && (s.WOStatus == 1 || s.WOStatus == 2)
                  && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId) && (s.OrderNo.Contains(SearchText) || s.Required.Contains(SearchText) ||
            SearchText == null || SearchText == "")).Select(x => new WorkOrdersVM
            {
                WorkOrderId = x.WorkOrderId,
                OrderNo = x.OrderNo,
                Required = x.Required,
                OrderDate = x.OrderDate,
                EndDate = x.EndDate,
                ProjectNo = x.Project!.ProjectNo ?? "بدون مشروع",
                ProjectId = x.ProjectId,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ContactLists = x.ContactLists,
            }).ToList().Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));

                return WorkOrders;
            }
            else
            {
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId
                  && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId) && (s.OrderNo.Contains(SearchText) || s.Required.Contains(SearchText) ||
            SearchText == null || SearchText == "")).Select(x => new WorkOrdersVM
            {
                WorkOrderId = x.WorkOrderId,
                OrderNo = x.OrderNo,
                Required = x.Required,
                OrderDate = x.OrderDate,
                ProjectNo = x.Project!.ProjectNo,
                ProjectId = x.ProjectId,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ContactLists = x.ContactLists,
            });
                return WorkOrders;
            };
        }


        public async Task< IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserId(int? UserId, int BranchId)
        {
            var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && s.WOStatus!=3).Select(x => new WorkOrdersVM
            {
               
                WorkOrderId=x.WorkOrderId,
                OrderNo = x.OrderNo,
                Required = x.Required,
                OrderDate = x.OrderDate,
                PercentComplete = x.PercentComplete,
                AttatchmentUrl = x.AttatchmentUrl,
                NoOfDays = x.NoOfDays,
                Note = x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                CustomerName_W = x.Customer!.CustomerNameAr,
                CustomerName = x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer!.CustomerNameAr : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer!.CustomerNameAr + "(*)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer!.CustomerNameAr + "(**)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer!.CustomerNameAr + "(***)" : x.Customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer!.CustomerNameAr + "(VIP)" : x.Customer!.CustomerNameAr,
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
            });
            return WorkOrders;
        }
        public async Task< IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserIdandtask(string task,int? UserId, int BranchId)
        {
            try
            {
                var WorkOrderId = Convert.ToInt32(task);
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && s.WorkOrderId == WorkOrderId).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    EndDate = x.EndDate,
                    PercentComplete = x.PercentComplete,
                    StrTime = x.NoOfDays + " يوم ",
                    WOStatus = x.WOStatus,
                    WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                    ProjectNo = x.Project!.ProjectNo ?? "بدون مشروع",
                    ProjectId = x.ProjectId,
                    Note =  _TaamerProContext.ContactLists.Where(x => x.OrderId.Value == WorkOrderId && x.UserId != UserId).OrderByDescending(x => x.AddDate).FirstOrDefault().Contacttxt ?? "",//x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    UserId = x.UserId,
                    AttatchmentUrl = x.AttatchmentUrl,
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                });
                return WorkOrders;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserIdandtask2(string task, int? UserId, int BranchId)
        {
            try
            {
                var WorkOrderId = Convert.ToInt32(task);
                var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false  && s.WorkOrderId == WorkOrderId).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    Required = x.Required,
                    OrderDate = x.OrderDate,
                    EndDate = x.EndDate,
                    PercentComplete = x.PercentComplete,
                    StrTime = x.NoOfDays + " يوم ",
                    WOStatus = x.WOStatus,
                    WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                    ProjectNo = x.Project!.ProjectNo ?? "بدون مشروع",
                    ProjectId = x.ProjectId,
                    Note = _TaamerProContext.ContactLists.Where(x => x.OrderId.Value == WorkOrderId && x.UserId != UserId).OrderByDescending(x => x.AddDate).FirstOrDefault().Contacttxt ?? "",//x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    UserId = x.UserId,
                    AttatchmentUrl = x.AttatchmentUrl,
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                });
                return WorkOrders;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task< IEnumerable<WorkOrdersVM>> GetWorkOrdersBytask(string task, int BranchId)
        {
            var WorkOrderId = Convert.ToInt32(task);
            var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.WorkOrderId == WorkOrderId).Select(x => new WorkOrdersVM
            {
                WorkOrderId = x.WorkOrderId,
                OrderNo = x.OrderNo,
                Required = x.Required,
                OrderDate = x.OrderDate,
                EndDate = x.EndDate,
                PercentComplete = x.PercentComplete,
                StrTime = x.NoOfDays + " يوم ",
                WOStatus = x.WOStatus,
                WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                ProjectNo = x.Project!.ProjectNo ?? "بدون مشروع",
                ProjectId = x.ProjectId,
                Note = _TaamerProContext.ContactLists.Where(x => x.OrderId.Value == WorkOrderId).OrderByDescending(x => x.AddDate).FirstOrDefault().Contacttxt ?? "",//x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                AttatchmentUrl = x.AttatchmentUrl ?? "",
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
            });
            return WorkOrders;
        }

        public async Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersBytask(string task, int BranchId,int UserId)
        {
            var WorkOrderId = Convert.ToInt32(task);
            var WorkOrders = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.WorkOrderId == WorkOrderId).Select(x => new WorkOrdersVM
            {
                WorkOrderId = x.WorkOrderId,
                OrderNo = x.OrderNo,
                Required = x.Required,
                OrderDate = x.OrderDate,
                EndDate = x.EndDate,
                PercentComplete = x.PercentComplete,
                StrTime = x.NoOfDays + " يوم ",
                WOStatus = x.WOStatus,
                WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                ProjectNo = x.Project!.ProjectNo ?? "بدون مشروع",
                ProjectId = x.ProjectId,
                Note = _TaamerProContext.ContactLists.Where(x => x.OrderId.Value == WorkOrderId && x.UserId != UserId).OrderByDescending(x => x.AddDate).FirstOrDefault().Contacttxt ?? "",//x.Note == "null" ? "" : x.Note ?? "",
                PhasePriority = x.PhasePriority,
                PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                AttatchmentUrl = x.AttatchmentUrl ?? "",
                IsConverted = x.IsConverted ?? 0,
                PlusTime = x.PlusTime ?? false,
                AddOrderName = x.User!.FullNameAr ?? "",
                AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
            });
            return WorkOrders;
        }

        public async Task< IEnumerable<WorkOrdersVM>> GetDayWeekMonth_Orders(int? UserId, int Status, int BranchId, int Flag, string StartDate, string EndDate)
        {
            
            if (Flag == 1)
            {
                var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && (s.WOStatus == 1 || s.WOStatus == 2)).Select(x => new WorkOrdersVM
                {

                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    UserId = x.UserId,
                    OrderDate = x.OrderDate,
                    OrderHijriDate = x.OrderHijriDate,
                    ResponsibleEng = x.ResponsibleEng,
                    ExecutiveEng = x.ExecutiveEng,
                    CustomerId = x.CustomerId,
                    Mediator = x.Mediator,
                    Required = x.Required,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    OrderValue = x.OrderValue,
                    OrderPaid = x.OrderPaid,
                    OrderRemaining = x.OrderRemaining,
                    OrderDiscount = x.OrderDiscount,
                    OrderTax = x.OrderTax,
                    OrderValueAfterTax = x.OrderValueAfterTax,
                    DiscountReason = x.DiscountReason,
                    Sketch = x.Sketch,
                    District = x.District,
                    Location = x.Location,
                    PieceNo = x.PieceNo,
                    InstrumentNo = x.InstrumentNo,
                    ExecutiveType = x.ExecutiveType,
                    ContractNo = x.ContractNo,
                    AgentId = x.AgentId,
                    AgentMobile = x.AgentMobile,
                    Social = x.Social,
                    BranchId = x.BranchId,
                    EndDate = x.EndDate!.ToString(),
                    WOStatus = x.WOStatus,
                    WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                    PercentComplete = x.PercentComplete,
                    AttatchmentUrl = x.AttatchmentUrl ?? "",
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                }).ToList().Where(m => DateTime.Now >= DateTime.ParseExact(m.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                return WorkOrders;
            }
            else if (Flag == 2)
            {
                var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && (s.WOStatus == 1 || s.WOStatus == 2)).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    UserId = x.UserId,
                    OrderDate = x.OrderDate,
                    OrderHijriDate = x.OrderHijriDate,
                    ResponsibleEng = x.ResponsibleEng,
                    ExecutiveEng = x.ExecutiveEng,
                    CustomerId = x.CustomerId,
                    Mediator = x.Mediator,
                    Required = x.Required,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    OrderValue = x.OrderValue,
                    OrderPaid = x.OrderPaid,
                    OrderRemaining = x.OrderRemaining,
                    OrderDiscount = x.OrderDiscount,
                    OrderTax = x.OrderTax,
                    OrderValueAfterTax = x.OrderValueAfterTax,
                    DiscountReason = x.DiscountReason,
                    Sketch = x.Sketch,
                    District = x.District,
                    Location = x.Location,
                    PieceNo = x.PieceNo,
                    InstrumentNo = x.InstrumentNo,
                    ExecutiveType = x.ExecutiveType,
                    ContractNo = x.ContractNo,
                    AgentId = x.AgentId,
                    AgentMobile = x.AgentMobile,
                    Social = x.Social,
                    BranchId = x.BranchId,
                    EndDate = x.EndDate!.ToString(),
                    WOStatus = x.WOStatus,
                    WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                    PercentComplete = x.PercentComplete,
                    AttatchmentUrl = x.AttatchmentUrl ?? "",
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                }).ToList().Where(m => DateTime.Now > DateTime.ParseExact(m.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(6));
                return WorkOrders;
            }
            else
            {
                var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && (s.WOStatus == 1 || s.WOStatus == 2)).Select(x => new WorkOrdersVM
                {
                    WorkOrderId = x.WorkOrderId,
                    OrderNo = x.OrderNo,
                    UserId = x.UserId,
                    OrderDate = x.OrderDate,
                    OrderHijriDate = x.OrderHijriDate,
                    ResponsibleEng = x.ResponsibleEng,
                    ExecutiveEng = x.ExecutiveEng,
                    CustomerId = x.CustomerId,
                    Mediator = x.Mediator,
                    Required = x.Required,
                    Note = x.Note == "null" ? "" : x.Note ?? "",
                    PhasePriority = x.PhasePriority,
                    PhasePriorityName = x.PhasePriority == 1 ? "منخفض" : x.PhasePriority == 2 ? "متوسط" : x.PhasePriority == 3 ? "مرتفع" : x.PhasePriority == 4 ? "عاجل" : "بدون",
                    OrderValue = x.OrderValue,
                    OrderPaid = x.OrderPaid,
                    OrderRemaining = x.OrderRemaining,
                    OrderDiscount = x.OrderDiscount,
                    OrderTax = x.OrderTax,
                    OrderValueAfterTax = x.OrderValueAfterTax,
                    DiscountReason = x.DiscountReason,
                    Sketch = x.Sketch,
                    District = x.District,
                    Location = x.Location,
                    PieceNo = x.PieceNo,
                    InstrumentNo = x.InstrumentNo,
                    ExecutiveType = x.ExecutiveType,
                    ContractNo = x.ContractNo,
                    AgentId = x.AgentId,
                    AgentMobile = x.AgentMobile,
                    Social = x.Social,
                    BranchId = x.BranchId,
                    EndDate = x.EndDate!.ToString(),
                    WOStatus = x.WOStatus,
                    WOStatustxt = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",
                    PercentComplete = x.PercentComplete,
                    AttatchmentUrl = x.AttatchmentUrl ?? "",
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    AddOrderName = x.User!.FullNameAr ?? "",
                    AddOrderImg = x.User!.ImgUrl != null ? x.User.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr ?? "",
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl != null ? x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",
                    ResponsibleEngName = x.ResponsibleEngineer!.FullNameAr ?? "",
                    ResponsibleEngImg = x.ResponsibleEngineer!.ImgUrl != null ? x.ResponsibleEngineer.ImgUrl : "/distnew/images/userprofile.png",
                    ExecutiveEngName = x.ExecutiveEngineer!.FullNameAr ?? "",
                    ExecutiveEngImg = x.ExecutiveEngineer!.ImgUrl != null ? x.ExecutiveEngineer.ImgUrl : "/distnew/images/userprofile.png",
                }).ToList().Where(m => DateTime.Now > DateTime.ParseExact(m.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(29));
                return WorkOrders;
            }

        }
        public async Task< int> GetLateWorkOrdersCountByUserId(string EndDateP, int? UserId, int BranchId)
        {
            try
            {
                if (EndDateP != "")
                {
                    var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && (s.WOStatus == 1 || s.WOStatus == 2)).Select(x => new WorkOrdersVM
                    {
                        WorkOrderId = x.WorkOrderId,

                        OrderNo = x.OrderNo,
                        Required = x.Required,
                        OrderDate = x.OrderDate,
                        EndDate = x.EndDate
                    }).ToList().Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    return WorkOrders.Count();
                }
                else
                {
                    var WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId).Select(x => new WorkOrdersVM
                    {
                        WorkOrderId = x.WorkOrderId,

                        OrderNo = x.OrderNo,
                        Required = x.Required,
                        OrderDate = x.OrderDate
                    }).ToList();
                    return WorkOrders.Count();
                };
            }
            catch(Exception)
            {
                var projectPhasesTasks =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId).Count();
                return projectPhasesTasks;

            }
        }
         public async Task<decimal> GetWorkOrdersPercentByUserIdAndProjectId(int? UserId, int BranchId)
        {
            decimal WorkOrders =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId).Count();
            decimal AllWorkOrder =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false).Count();
            decimal result = 0;
            if (AllWorkOrder != 0)
            {
                result = (WorkOrders / AllWorkOrder) * 100;
            }
            
            return result;
        }
        public async Task< int?> GetUserWorkOrderCount(int? UserId, int BranchId)
        {
            var projects =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.ExecutiveEng == UserId && s.WOStatus != 3).Count();
            return projects;
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport(int? UserId,int BranchId, string lang,int status, string DateFrom, string DateTo, List<int> BranchesList)
        {
            try
            { var statusS = 0;
                if( status == 4)
                {
                    statusS = 3;
                }else if(status ==3)
                {
                    statusS = 0;
                }
                else
                {
                    statusS = status;
                }
                var emp =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && BranchesList.Contains(s.BranchId ?? 0) && (UserId == null || s.UserId == UserId) && (statusS == 0 || s.WOStatus == statusS)).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = "امر عمل",
                    UserId = x.UserId,
                    ProjectId = x.ProjectId,
                
                    Status = x.WOStatus,
                    
                   
                    StartDate = x.OrderDate,
                    EndDate = x.EndDate,
                  
                    Notes = x.Note ?? "",
                    BranchId = x.BranchId,
                    UserName = x.User!.FullNameAr ?? "",
                    
                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.OrderDate != null ? x.OrderDate  : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = "امر عمل",//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    StatusName = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",

                    ProjectTypeName = x.Project!.projecttype!.NameAr,

                    PercentComplete = x.PercentComplete,
                    EndDateCalc=x.EndDate,
                    TaskTimeFrom="",
                    TaskTimeTo="",
                    TimeStr = x.NoOfDays + "يوم",
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    TaskNo=x.OrderNo??null,

                    // TimeStr=((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays == 0 ? 1 +"يوم" : ((DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays  + "يوم",


                }).ToList().Where(s => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(s.StartDate)&& DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(s.EndDate) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)));

                return emp;
            }
            catch (Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();

            }

        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport(int? UserId, int BranchId, string lang, int status, string DateFrom, string DateTo,string? SearchText)
        {
            try
            {
                var statusS = 0;
                if (status == 4)
                {
                    statusS = 3;
                }
                else if (status == 3)
                {
                    statusS = 0;
                }
                else
                {
                    statusS = status;
                }
                var emp = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (UserId == null || s.UserId == UserId) && (statusS == 0 || s.WOStatus == statusS) &&
               ( s.Project.customer.CustomerNameAr.Contains(SearchText) || s.Project.ProjectName.Contains(SearchText) ||
                s.Project.ProjectDescription.Contains(SearchText) || s.Required.Contains(SearchText) || s.Project.ProjectNo.Contains(SearchText) ||
                s.Project.projecttype.NameAr.Contains(SearchText) || SearchText ==null || SearchText =="")).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = "امر عمل",
                    UserId = x.UserId,
                    ProjectId = x.ProjectId,

                    Status = x.WOStatus,


                    StartDate = x.OrderDate,
                    EndDate = x.EndDate,

                    Notes = x.Note ?? "",
                    BranchId = x.BranchId,
                    UserName = x.User!.FullNameAr ?? "",

                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.OrderDate != null ? x.OrderDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = "امر عمل",//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    StatusName = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",

                    ProjectTypeName = x.Project!.projecttype!.NameAr,

                    PercentComplete = x.PercentComplete,
                    EndDateCalc = x.EndDate,
                    TaskTimeFrom = "",
                    TaskTimeTo = "",
                    TimeStr = x.NoOfDays + "يوم",
                    IsConverted = x.IsConverted ?? 0,
                    PlusTime = x.PlusTime ?? false,
                    TaskNo = x.OrderNo ?? null,
                    // TimeStr=((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays == 0 ? 1 +"يوم" : ((DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays  + "يوم",


                }).ToList().Where(s => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(s.StartDate) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(s.EndDate) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)));

                return emp;
            }
            catch (Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();

            }

        }


        public async Task< List<ProjectPhasesTasksVM>> GetWorkOrderReport_print(int? UserId, int BranchId, string lang, int status, string DateFrom, string DateTo)
        {
            try
            {
                var statusS = 0;
                if (status == 4)
                {
                    statusS = 3;
                }
                else if (status == 3)
                {
                    statusS = 0;
                }
                else
                {
                    statusS = status;
                }
                var emp =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false &&s.BranchId==BranchId && (UserId == null || s.UserId == UserId) && (statusS == 0 || s.WOStatus == statusS)).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = "امر عمل",
                    UserId = x.UserId,
                    ProjectId = x.ProjectId,

                    Status = x.WOStatus,


                    StartDate = x.OrderDate,
                    EndDate = x.EndDate,

                    Notes = x.Note ?? "",
                    BranchId = x.BranchId,
                    UserName = x.User!.FullNameAr ?? "",

                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.OrderDate != null ? x.OrderDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = "امر عمل",//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    StatusName = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",

                    ProjectTypeName = x.Project!.projecttype!.NameAr,

                    PercentComplete = x.PercentComplete,
                    EndDateCalc = x.EndDate,
                    TaskTimeFrom = "",
                    TaskTimeTo = "",
                    TaskNo = x.OrderNo ?? null,
                    // TimeStr=((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays == 0 ? 1 +"يوم" : ((DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays  + "يوم",


                }).ToList().Where(s => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(s.StartDate) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(s.EndDate) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)));

                return emp.ToList();
            }
            catch (Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();

            }

        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport_ptoject(string lang, int? ProjectId, string DateFrom, string DateTo, int BranchId)
        {
            try
            {
                var emp =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId==BranchId && (ProjectId == null || s.ProjectId == ProjectId) && s.ProjectId != null).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = "امر عمل",
                    UserId = x.UserId,
                    ProjectId = x.ProjectId,

                    Status = x.WOStatus,


                    StartDate = x.OrderDate,
                    EndDate = x.EndDate,

                    Notes = x.Note ?? "",
                    BranchId = x.BranchId,
                    UserName = x.User!.FullNameAr ?? "",

                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.OrderDate != null ? x.OrderDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = "امر عمل",//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    StatusName = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",

                    ProjectTypeName = x.Project != null ? x.Project!.projecttype!.NameAr??"" : "",
                    ProjectSubTypeName=x.Project!=null? x.Project!.projectsubtype!.NameAr??"":"",
                    FullTaskDescription= "امر عمل",
                    PercentComplete = x.PercentComplete,
                    EndDateCalc = x.EndDate,
                    TaskTimeFrom = "",
                    TaskTimeTo = "",
                    TaskNo = x.OrderNo ?? null,
                    // TimeStr=((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays == 0 ? 1 +"يوم" : ((DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays  + "يوم",


                }).ToList().Where(s => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(s.StartDate) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(s.EndDate) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)));

                return emp;
            }
            catch (Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();

            }

        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport_ptoject(string lang, int? ProjectId, string DateFrom, string DateTo, int BranchId,string? SearchText)
        {
            try
            {
                var emp = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (ProjectId == null || s.ProjectId == ProjectId) && s.ProjectId != null &&(
                s.Project.customer.CustomerNameAr.Contains(SearchText) || s.Project.ProjectDescription.Contains(SearchText) ||
                s.Project.ProjectTypeName.Contains(SearchText) || s.Required.Contains(SearchText) || s.User.FullNameAr.Contains(SearchText) ||
                SearchText==null || SearchText =="")).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = "امر عمل",
                    UserId = x.UserId,
                    ProjectId = x.ProjectId,

                    Status = x.WOStatus,


                    StartDate = x.OrderDate,
                    EndDate = x.EndDate,

                    Notes = x.Note ?? "",
                    BranchId = x.BranchId,
                    UserName = x.User!.FullNameAr ?? "",

                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.OrderDate != null ? x.OrderDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = "امر عمل",//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    StatusName = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",

                    ProjectTypeName = x.Project != null ? x.Project!.projecttype!.NameAr ?? "" : "",
                    ProjectSubTypeName = x.Project != null ? x.Project!.projectsubtype!.NameAr ?? "" : "",
                    FullTaskDescription = "امر عمل",
                    PercentComplete = x.PercentComplete,
                    EndDateCalc = x.EndDate,
                    TaskTimeFrom = "",
                    TaskTimeTo = "",
                    TaskNo = x.OrderNo ?? null,
                    // TimeStr=((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays == 0 ? 1 +"يوم" : ((DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays  + "يوم",


                }).ToList().Where(s => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(s.StartDate) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(s.EndDate) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)));

                return emp;
            }
            catch (Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();

            }

        }



        public async Task< IEnumerable<ProjectPhasesTasksVM>> GetALlWorkOrderReport(string lang, int BranchId)
        {
            try
            {

                var emp =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false &&s.BranchId==BranchId).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = "امر عمل",
                    UserId = x.UserId,
                    ProjectId = x.ProjectId,

                    Status = x.WOStatus,


                    StartDate = x.OrderDate,
                    EndDate = x.EndDate,

                    Notes = x.Note ?? "",
                    BranchId = x.BranchId,
                    UserName = x.User!.FullNameAr ?? "",

                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.OrderDate != null ? x.OrderDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = "امر عمل",//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",

                    StatusName = x.WOStatus == 1 ? "لم تبدأ" : x.WOStatus == 2 ? "قيد التشغيل" : x.WOStatus == 3 ? "منتهية" : x.WOStatus == 4 ? "متوقفة" : "",

                    ProjectTypeName = x.Project != null ? x.Project!.projecttype!.NameAr ?? "" : "",
                    ProjectSubTypeName = x.Project != null ? x.Project!.projectsubtype!.NameAr ?? "" : "",
                    FullTaskDescription = "امر عمل",

                    PercentComplete = x.PercentComplete,
                    EndDateCalc = x.EndDate,
                    TaskTimeFrom = "",
                    TaskTimeTo = "",
                    TimeStr=x.NoOfDays +"يوم",
                    TaskNo=x.OrderNo??null,
                    // TimeStr=((DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays == 0 ? 1 +"يوم" : ((DateTime.ParseExact(x.OrderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) - (DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).TotalDays  + "يوم",


                }).ToList();//.Where(s => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(s.StartDate) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(s.EndDate) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)));

                return emp;
            }
            catch (Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();

            }

        }


        public async Task< List<ProjectPhasesTasksVM>> GetALlWorkOrderReport_NotStarted(ProjectPhasesTasksVM Search, string lang)
        {
            try
            {

                var emp =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (Search.UserId == 0 || s.UserId == Search.UserId) && (Search.BranchId == 0 || s.BranchId == Search.BranchId) &&s.WOStatus==1).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = "امر عمل",
                    UserId = x.UserId,
                    ProjectId = x.ProjectId,
                    Status = x.WOStatus,
                    StartDate = x.OrderDate,
                    EndDate = x.EndDate,
                    Notes = x.Note ?? "",
                    BranchId = x.BranchId,
                    UserName = x.User!.FullNameAr ?? "",
                    TaskEnd = x.EndDate != null ? x.EndDate : "",
                    PercentComplete = x.PercentComplete,
                    EndDateCalc = x.EndDate,
                    TaskTimeFrom = "",
                    TaskTimeTo = "",
                    TaskNo = x.OrderNo ?? null,
                }).ToList();
                return emp;
            }
            catch (Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();

            }

        }


        public async Task< List<ProjectPhasesTasksVM>> GetALlWorkOrderReport_Inptogress(ProjectPhasesTasksVM Search, string lang)
        {
            try
            {

                var emp =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (Search.UserId == 0 || s.UserId == Search.UserId) && (Search.BranchId == 0 || s.BranchId == Search.BranchId) && s.WOStatus == 2).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = "امر عمل",
                    UserId = x.UserId,
                    ProjectId = x.ProjectId,
                    Status = x.WOStatus,
                    StartDate = x.OrderDate,
                    EndDate = x.EndDate,
                    Notes = x.Note ?? "",
                    BranchId = x.BranchId,
                    UserName = x.User!.FullNameAr ?? "",
                    TaskEnd = x.EndDate != null ? x.EndDate : "",
                    PercentComplete = x.PercentComplete,
                    EndDateCalc = x.EndDate,
                    TaskTimeFrom = "",
                    TaskTimeTo = "",
                }).ToList();
                return emp;
            }
            catch (Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();

            }

        }


        public async Task< List<ProjectPhasesTasksVM>> GetALlWorkOrderReport_Finished(ProjectPhasesTasksVM Search, string lang)
        {
            try
            {

                var emp =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (Search.UserId == 0 || s.UserId == Search.UserId) && (Search.BranchId == 0 || s.BranchId == Search.BranchId) && s.WOStatus == 3).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = "امر عمل",
                    UserId = x.UserId,
                    ProjectId = x.ProjectId,
                    Status = x.WOStatus,
                    StartDate = x.OrderDate,
                    EndDate = x.EndDate,
                    Notes = x.Note ?? "",
                    BranchId = x.BranchId,
                    UserName = x.User!.FullNameAr ?? "",
                    TaskEnd = x.EndDate != null ? x.EndDate : "",
                    PercentComplete = x.PercentComplete,
                    EndDateCalc = x.EndDate,
                    TaskTimeFrom = "",
                    TaskTimeTo = "",

                }).ToList();
                return emp;
            }
            catch (Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();

            }

        }
        public async Task< List<ProjectPhasesTasksVM>> GetALlWorkOrderReport_Late(ProjectPhasesTasksVM Search, string Today)
        {
            try
            {

                var emp =  _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (Search.UserId == 0 || s.UserId == Search.UserId) && (Search.BranchId == 0 || s.BranchId == Search.BranchId) && s.WOStatus == 3).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = "امر عمل",
                    UserId = x.UserId,
                    ProjectId = x.ProjectId,
                    Status = x.WOStatus,
                    StartDate = x.OrderDate,
                    EndDate = x.EndDate,
                    Notes = x.Note ?? "",
                    BranchId = x.BranchId,
                    UserName = x.User!.FullNameAr ?? "",
                    TaskEnd = x.EndDate != null ? x.EndDate : "",
                    PercentComplete = x.PercentComplete,
                    EndDateCalc = x.EndDate,
                    TaskTimeFrom = "",
                    TaskTimeTo = "",
                    TaskNo = x.OrderNo ?? null,
                }).ToList().Where(m => (string.IsNullOrEmpty(Today) || (!string.IsNullOrEmpty(Today) && DateTime.ParseExact(m.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(Today, "yyyy-MM-dd", CultureInfo.InvariantCulture))));
                return emp.ToList();
            }
            catch (Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();

            }

        }
        public async Task<IEnumerable<Pro_TaskOperationsVM>> GetTaskOperationsByTaskId(int WorkOrderId)
        {
            var projectPhasesTasks = _TaamerProContext.Pro_TaskOperations.Where(s => s.IsDeleted == false && s.WorkOrderId == WorkOrderId).Select(x => new Pro_TaskOperationsVM
            {
                TaskOperationId = x.TaskOperationId,
                PhaseTaskId = x.PhaseTaskId,
                WorkOrderId = x.WorkOrderId,
                Type = x.Type,
                OperationName = x.OperationName,
                Date = x.Date,
                UserId = x.UserId,
                BranchId = x.BranchId,
                Note = x.Note,
                TaskNo = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.TaskNo ?? null : null,
                DescriptionAr = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? null : null,
                ExtraNote = x.UserId != null ? x.Users != null ? " تم تحويلها الي :  " + (x.Users.FullNameAr ?? x.Users.FullName) : null : null,
                AddUserName = x.AddUsers != null ? (x.AddUsers.FullNameAr ?? x.AddUsers.FullName) ?? null : null,
            }).ToList();

            return projectPhasesTasks;
        }

        public async Task<int> GenerateNextOrderNumber(int BranchId, string codePrefix, int? ProjectId)
        {
            if (_TaamerProContext.ProjectPhasesTasks != null)
            {
                var lastRow = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.OrderNoType == 1 && s.OrderNo!.Contains(codePrefix)).OrderByDescending(u => u.WorkOrderId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {

                        var TaskNumber = 0;

                        if (codePrefix == "")
                        {
                            TaskNumber = int.Parse(lastRow!.OrderNo!) + 1;
                        }
                        else
                        {
                            TaskNumber = int.Parse(lastRow!.OrderNo!.Replace(codePrefix, "").Trim()) + 1;
                        }

                        return TaskNumber;
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



    }
}
