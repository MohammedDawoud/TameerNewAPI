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
    public class OutInBoxRepository : IOutInBoxRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public OutInBoxRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
       public  async Task<IEnumerable<OutInBoxVM>> GetAllOutInbox(int Type,int BranchId)
        {
            var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false && s.Type == Type && s.BranchId == BranchId).Select(x => new OutInBoxVM
            {
                OutInBoxId = x.OutInBoxId,
                Number = x.Number,
                Date = x.Date,
                HijriDate = x.HijriDate,
                TypeId = x.TypeId,
                SideFromId = x.SideFromId,
                SideToId = x.SideToId,
                InnerId = x.InnerId,
                Topic = x.Topic,
                ArchiveFileId = x.ArchiveFileId,
                RelatedToId = x.RelatedToId,
                Type = x.Type,
                OutInType = x.OutInType,
                FileCount = x.ContacFiles.Count,
                AttachmentUrl = x.AttachmentUrl,
                ProjectId = x.ProjectId,
                BranchId = x.BranchId,
                Priority = x.Priority,
                CustomerName_W = x.Project.customer.CustomerNameAr,
                CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                OutInTypeName = x.OutInType == 1 ? "داخلي" : "خارجي",
                OutInBoxTypeName = x.OutInBoxType.NameAr,
                ArchiveFilesName = x.ArchiveFiles.NameAr,
                //SideFromName = x.SideFromId == 1 ? "الاستقبال" : x.SideFromId == 2 ? "تخطيط"
                //                                               : x.SideFromId == 3 ? "تصميم"
                //                                               : x.SideFromId == 4 ? "إشراف"
                //                                               : x.SideFromId == 5 ? "السكرتاريا"
                //                                               : x.SideFromId == 6 ? "الإدارة"
                //                                               : x.SideFromId == 7 ? "المحاسبة"
                //                                               : x.SideFromId == 8 ? "مساحة" : "عمال",
                //SideToName = x.SideToId == 1 ? "الاستقبال" : x.SideToId == 2 ? "تخطيط"
                //                                             : x.SideFromId == 3 ? "تصميم"
                //                                             : x.SideFromId == 4 ? "إشراف"
                //                                             : x.SideFromId == 5 ? "السكرتاريا"
                //                                             : x.SideFromId == 6 ? "الإدارة"
                //                                             : x.SideFromId == 7 ? "المحاسبة"
                //                                             : x.SideFromId == 8 ? "مساحة" : "عمال",
                SideFromName = x.FromDepartment.DepartmentNameAr,
                SideToName = x.ToDepartment.DepartmentNameAr,
                RelatedToName = x.RelatedToOutIn.Number,
                ProjectNumber=x.Project.ProjectNo??"",
                Code =x.Code,
            }).ToList();
            return OutInBoxes;
        }
       public  async Task<IEnumerable<OutInBoxVM>> GetAllOutInboxList(int Type, int BranchId)
        {
            var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.Type == Type && s.BranchId == BranchId).Select(x => new OutInBoxVM
            {
                OutInBoxId = x.OutInBoxId,
                Number = x.Number,
                Date = x.Date,
                HijriDate = x.HijriDate,
                TypeId = x.TypeId,
                SideFromId = x.SideFromId,
                SideToId = x.SideToId,
                InnerId = x.InnerId,
                Topic = x.Topic,
                ArchiveFileId = x.ArchiveFileId,
                RelatedToId = x.RelatedToId,
                Type = x.Type,
                OutInType = x.OutInType,
                FileCount = x.ContacFiles.Count,
                AttachmentUrl = x.AttachmentUrl,
                ProjectId = x.ProjectId,
                BranchId = x.BranchId,
                Priority = x.Priority,
                CustomerName_W = x.Project.customer.CustomerNameAr,
                CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                OutInTypeName = x.OutInType == 1 ? "داخلي" : "خارجي",
                OutInBoxTypeName = x.OutInBoxType.NameAr,
                ArchiveFilesName = x.ArchiveFiles.NameAr,
                //SideFromName = x.SideFromId == 1 ? "الاستقبال" : x.SideFromId == 2 ? "تخطيط"
                //                                               : x.SideFromId == 3 ? "تصميم"
                //                                               : x.SideFromId == 4 ? "إشراف"
                //                                               : x.SideFromId == 5 ? "السكرتاريا"
                //                                               : x.SideFromId == 6 ? "الإدارة"
                //                                               : x.SideFromId == 7 ? "المحاسبة"
                //                                               : x.SideFromId == 8 ? "مساحة" : "عمال",
                //SideToName = x.SideToId == 1 ? "الاستقبال" : x.SideToId == 2 ? "تخطيط"
                //                                             : x.SideFromId == 3 ? "تصميم"
                //                                             : x.SideFromId == 4 ? "إشراف"
                //                                             : x.SideFromId == 5 ? "السكرتاريا"
                //                                             : x.SideFromId == 6 ? "الإدارة"
                //                                             : x.SideFromId == 7 ? "المحاسبة"
                //                                             : x.SideFromId == 8 ? "مساحة" : "عمال",
                SideFromName = x.FromDepartment.DepartmentNameAr,
                SideToName = x.ToDepartment.DepartmentNameAr,
                RelatedToName = x.RelatedToOutIn.Number,
                ProjectNumber = x.Project.ProjectNo ?? "",
                Code = x.Code,
            }).ToList();
            return OutInBoxes;
        }
       public  async Task<IEnumerable<OutInBoxVM>> GetAllOutInboxSearch(int BranchId)
        {
            var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false  && s.BranchId == BranchId).Select(x => new OutInBoxVM
            {
                OutInBoxId = x.OutInBoxId,
                Number = x.Number,
                Date = x.Date,
                HijriDate = x.HijriDate,
                TypeId = x.TypeId,
                SideFromId = x.SideFromId,
                SideToId = x.SideToId,
                InnerId = x.InnerId,
                Topic = x.Topic,
                ArchiveFileId = x.ArchiveFileId,
                RelatedToId = x.RelatedToId,
                Type = x.Type,
                OutInType = x.OutInType,
                FileCount = x.ContacFiles.Count,
                AttachmentUrl = x.AttachmentUrl,
                ProjectId = x.ProjectId,
                BranchId = x.BranchId,
                Priority = x.Priority,
                CustomerName_W = x.Project.customer.CustomerNameAr,
                CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                OutInTypeName = (x.OutInType == 1 && x.Type == 1) ? "صادر داخلي" : (x.OutInType == 0 && x.Type == 1) ? "صادر خارجي" : (x.OutInType == 1 && x.Type == 2) ? "وارد داخلي" : (x.OutInType == 0 && x.Type == 2) ? "وارد خارجي" : "",
                OutInBoxTypeName = x.OutInBoxType.NameAr,
                ArchiveFilesName = x.ArchiveFiles.NameAr,
                //SideFromName = x.SideFromId == 1 ? "الاستقبال" : x.SideFromId == 2 ? "تخطيط"
                //                                               : x.SideFromId == 3 ? "تصميم"
                //                                               : x.SideFromId == 4 ? "إشراف"
                //                                               : x.SideFromId == 5 ? "السكرتاريا"
                //                                               : x.SideFromId == 6 ? "الإدارة"
                //                                               : x.SideFromId == 7 ? "المحاسبة"
                //                                               : x.SideFromId == 8 ? "مساحة" : "عمال",
                //SideToName = x.SideToId == 1 ? "الاستقبال" : x.SideToId == 2 ? "تخطيط"
                //                                             : x.SideFromId == 3 ? "تصميم"
                //                                             : x.SideFromId == 4 ? "إشراف"
                //                                             : x.SideFromId == 5 ? "السكرتاريا"
                //                                             : x.SideFromId == 6 ? "الإدارة"
                //                                             : x.SideFromId == 7 ? "المحاسبة"
                //                                             : x.SideFromId == 8 ? "مساحة" : "عمال",
                SideFromName = x.FromDepartment.DepartmentNameAr,
                SideToName = x.ToDepartment.DepartmentNameAr,
                ProjectNumber = x.Project.ProjectNo ?? "",
                Code = x.Code,
            }).ToList();
            return OutInBoxes;
        }
       public  async Task<OutInBoxVM> GetOutInboxById(int OutInBoxId)
        {
            var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.OutInBoxId == OutInBoxId).Select(x => new OutInBoxVM
            {
                OutInBoxId = x.OutInBoxId,
                Number = x.Number,
                Date = x.Date,
                HijriDate = x.HijriDate,
                TypeId = x.TypeId,
                SideFromId = x.SideFromId,
                SideToId = x.SideToId,
                InnerId = x.InnerId,
                Topic = x.Topic,
                ArchiveFileId = x.ArchiveFileId,
                RelatedToId = x.RelatedToId,
                Type = x.Type,
                OutInType = x.OutInType,
                FileCount = x.ContacFiles.Count,
                AttachmentUrl = x.AttachmentUrl,
                ProjectId = x.ProjectId,
                BranchId = x.BranchId,
                Priority = x.Priority,
                CustomerName_W = x.Project.customer.CustomerNameAr,
                CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                OutInTypeName = x.OutInType == 1 ? "داخلي" : "خارجي",
                OutInBoxTypeName = x.OutInBoxType.NameAr,
                OutInBoxSerialCode = x.OutInBoxSerial.Code,
                NumberType = x.NumberType,
                ArchiveFilesName = x.ArchiveFiles.NameAr,
                RelatedToSideTo = x.RelatedToOutIn.ToDepartment.DepartmentNameAr, //
                InnerOutInDate = x.InnerToOutIn.Date,
                RelatedToTopic = x.RelatedToOutIn.Topic,
                RelatedToDate = x.RelatedToOutIn.Date,
                SideFromName = x.FromDepartment.DepartmentNameAr,
                SideToName = x.ToDepartment.DepartmentNameAr,
                ProjectNumber = x.Project.ProjectNo ?? "",
                Code = x.Code,
            }).First();
            return OutInBoxes;
        }
       public  async Task<OutInBoxVM> GetOutInboxByRelatedToId(int OutInBoxId)
        {
            var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.RelatedToId == OutInBoxId).Select(x => new OutInBoxVM
            {
                OutInBoxId = x.OutInBoxId,
                Number = x.Number,
                Date = x.Date,
                HijriDate = x.HijriDate,
                TypeId = x.TypeId,
                SideFromId = x.SideFromId,
                SideToId = x.SideToId,
                InnerId = x.InnerId,
                Topic = x.Topic,
                ArchiveFileId = x.ArchiveFileId,
                RelatedToId = x.RelatedToId,
                Type = x.Type,
                OutInType = x.OutInType,
                FileCount = x.ContacFiles.Count,
                AttachmentUrl = x.AttachmentUrl??"",
                ProjectId = x.ProjectId??0,
                BranchId = x.BranchId??0,
                Priority = x.Priority??0,
                CustomerName_W = x.Project.customer.CustomerNameAr,
                CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                OutInTypeName = x.OutInType == 1 ? "داخلي" : "خارجي",
                OutInBoxTypeName = x.OutInBoxType.NameAr??"",
                OutInBoxSerialCode = x.OutInBoxSerial.Code??"",
                NumberType = x.NumberType??0,
                ArchiveFilesName = x.ArchiveFiles.NameAr??"",
                RelatedToSideTo = x.RelatedToOutIn.ToDepartment.DepartmentNameAr??"", //
                InnerOutInDate = x.InnerToOutIn.Date??"",
                RelatedToTopic = x.RelatedToOutIn.Topic??"",
                RelatedToDate = x.RelatedToOutIn.Date??"",
                SideFromName = x.FromDepartment.DepartmentNameAr??"",
                SideToName = x.ToDepartment.DepartmentNameAr??"",
                ProjectNumber = x.Project.ProjectNo ?? "",
                Code = x.Code,
            }).FirstOrDefault();
            return OutInBoxes;
        }
       public  async Task<IEnumerable<OutInBoxVM>> GetOutInboxFiles(int? Type, int? OutInType, int? ArchiveFileId, int BranchId)
        {
            var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false &&
            s.Type == Type && s.OutInType == OutInType && s.ArchiveFileId == ArchiveFileId && s.BranchId == BranchId).Select(x => new OutInBoxVM
            {
                OutInBoxId = x.OutInBoxId,
                Number = x.Number,
                ArchiveFileId = x.ArchiveFileId,
                Type = x.Type,
                OutInType = x.OutInType,
                FileCount = x.ContacFiles.Count,
                Code = x.Code,
            }).ToList();
            return OutInBoxes;
        }
       public  async Task<IEnumerable<OutInBoxVM>> GetOutInboxProjectFiles(int Type, int? ProjectId, int BranchId)
        {
            var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false && s.Type == Type && s.ProjectId == ProjectId && s.BranchId == BranchId).Select(x => new OutInBoxVM
            {
                OutInBoxId = x.OutInBoxId,
                Number = x.Number,
                Date = x.Date,
                HijriDate = x.HijriDate,
                TypeId = x.TypeId,
                SideFromId = x.SideFromId,
                SideToId = x.SideToId,
                InnerId = x.InnerId,
                Topic = x.Topic,
                ArchiveFileId = x.ArchiveFileId,
                RelatedToId = x.RelatedToId,
                Type = x.Type,
                OutInType = x.OutInType,
                FileCount = x.ContacFiles.Count,
                AttachmentUrl = x.AttachmentUrl,
                ProjectId = x.ProjectId,
                BranchId = x.BranchId,
                Priority = x.Priority,
                CustomerName_W = x.Project.customer.CustomerNameAr,
                CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                OutInTypeName = (x.OutInType == 1 && x.Type == 1) ? "صادر داخلي" : (x.OutInType == 0 && x.Type == 1) ? "صادر خارجي" : (x.OutInType == 1 && x.Type == 2) ? "وارد داخلي" : (x.OutInType == 0 && x.Type == 2) ? "وارد خارجي" : "",
                OutInBoxTypeName = x.OutInBoxType.NameAr,
                ArchiveFilesName = x.ArchiveFiles.NameAr,
                //SideFromName = x.SideFromId == 1 ? "الاستقبال" : x.SideFromId == 2 ? "تخطيط"
                //                                               : x.SideFromId == 3 ? "تصميم"
                //                                               : x.SideFromId == 4 ? "إشراف"
                //                                               : x.SideFromId == 5 ? "السكرتاريا"
                //                                               : x.SideFromId == 6 ? "الإدارة"
                //                                               : x.SideFromId == 7 ? "المحاسبة"
                //                                               : x.SideFromId == 8 ? "مساحة" : "عمال",
                //SideToName = x.SideToId == 1 ? "الاستقبال" : x.SideToId == 2 ? "تخطيط"
                //                                             : x.SideFromId == 3 ? "تصميم"
                //                                             : x.SideFromId == 4 ? "إشراف"
                //                                             : x.SideFromId == 5 ? "السكرتاريا"
                //                                             : x.SideFromId == 6 ? "الإدارة"
                //                                             : x.SideFromId == 7 ? "المحاسبة"
                //                                             : x.SideFromId == 8 ? "مساحة" : "عمال",
                SideFromName = x.FromDepartment.DepartmentNameAr,
                SideToName = x.ToDepartment.DepartmentNameAr,
                PriorityName = x.Priority == 1 ? "عادي" : x.SideFromId == 2 ? "سري" : "",
                ProjectNumber = x.Project.ProjectNo ?? "",
                Code = x.Code,
            }).ToList();
            return OutInBoxes;
        }
       public  async Task<IEnumerable<OutInBoxVM>> SearchOutbox(OutInBoxVM OutInBoxesSearch, string DateFrom, string DateTo, int BranchId)
        {
            try
            {
                if (DateTo == null || DateFrom == null || DateTo == "" || DateFrom == "")
                {
                    var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == OutInBoxesSearch.Type
                    && ((s.Number == OutInBoxesSearch.Number || OutInBoxesSearch.Number == null) && (s.Topic == OutInBoxesSearch.Topic ||
                    s.Topic.Contains(OutInBoxesSearch.Topic) || OutInBoxesSearch.Topic == null)
                       && (s.ToDepartment.DepartmentNameAr == OutInBoxesSearch.SideToName || s.ToDepartment.DepartmentNameAr.Contains(OutInBoxesSearch.SideToName)
                       || OutInBoxesSearch.SideToName == null)
                       && (s.FromDepartment.DepartmentNameAr == OutInBoxesSearch.SideFromName ||
                       s.FromDepartment.DepartmentNameAr.Contains(OutInBoxesSearch.SideFromName) || OutInBoxesSearch.SideFromName == null)
                       && (s.OutInBoxType.NameAr == OutInBoxesSearch.OutInBoxTypeName || s.OutInBoxType.NameAr.Contains(OutInBoxesSearch.OutInBoxTypeName) || OutInBoxesSearch.OutInBoxTypeName == null)))
                                                        .Select(x => new OutInBoxVM
                                                        {
                                                            OutInBoxId = x.OutInBoxId,
                                                            Number = x.Number,
                                                            Date = x.Date,
                                                            HijriDate = x.HijriDate,
                                                            TypeId = x.TypeId,
                                                            SideFromId = x.SideFromId,
                                                            SideToId = x.SideToId,
                                                            InnerId = x.InnerId,
                                                            Topic = x.Topic,
                                                            ArchiveFileId = x.ArchiveFileId,
                                                            RelatedToId = x.RelatedToId,
                                                            Type = x.Type,
                                                            OutInType = x.OutInType,
                                                            FileCount = x.ContacFiles.Count,
                                                            AttachmentUrl = x.AttachmentUrl,
                                                            ProjectId = x.ProjectId,
                                                            BranchId = x.BranchId,
                                                            Priority = x.Priority,
                                                            CustomerName_W = x.Project.customer.CustomerNameAr,
                                                            CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                                                            OutInTypeName = x.OutInType == 1 ? "داخلي" : "خارجي",
                                                            OutInBoxTypeName = x.OutInBoxType.NameAr,
                                                            ArchiveFilesName = x.ArchiveFiles.NameAr,
                                                            RelatedToSideTo = x.RelatedToOutIn.ToDepartment.DepartmentNameAr, //
                                                            InnerOutInDate = x.InnerToOutIn.Date,
                                                            RelatedToTopic = x.RelatedToOutIn.Topic,
                                                            RelatedToDate = x.RelatedToOutIn.Date,
                                                            SideFromName = x.FromDepartment.DepartmentNameAr,
                                                            SideToName = x.ToDepartment.DepartmentNameAr,
                                                            ProjectNumber = x.Project.ProjectNo ?? "",
                                                            Code = x.Code,
                                                        }).ToList();
                    return OutInBoxes;
                }
                else
                {
                    var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == OutInBoxesSearch.Type && ((s.Number == OutInBoxesSearch.Number || OutInBoxesSearch.Number == null) && (s.Topic == OutInBoxesSearch.Topic || s.Topic.Contains(OutInBoxesSearch.Topic) || OutInBoxesSearch.Topic == null)
                       && (s.ToDepartment.DepartmentNameAr == OutInBoxesSearch.SideToName || s.ToDepartment.DepartmentNameAr.Contains(OutInBoxesSearch.SideToName) || OutInBoxesSearch.SideToName == null)
                       && (s.FromDepartment.DepartmentNameAr == OutInBoxesSearch.SideFromName || s.FromDepartment.DepartmentNameAr.Contains(OutInBoxesSearch.SideFromName) || OutInBoxesSearch.SideFromName == null)
                       && (s.OutInBoxType.NameAr == OutInBoxesSearch.OutInBoxTypeName || s.OutInBoxType.NameAr.Contains(OutInBoxesSearch.OutInBoxTypeName) || OutInBoxesSearch.OutInBoxTypeName == null)))
                                                        .Select(x => new OutInBoxVM
                                                        {
                                                            OutInBoxId = x.OutInBoxId,
                                                            Number = x.Number,
                                                            Date = x.Date,
                                                            HijriDate = x.HijriDate,
                                                            TypeId = x.TypeId,
                                                            SideFromId = x.SideFromId,
                                                            SideToId = x.SideToId,
                                                            InnerId = x.InnerId,
                                                            Topic = x.Topic,
                                                            ArchiveFileId = x.ArchiveFileId,
                                                            RelatedToId = x.RelatedToId,
                                                            Type = x.Type,
                                                            OutInType = x.OutInType,
                                                            FileCount = x.ContacFiles.Count,
                                                            AttachmentUrl = x.AttachmentUrl,
                                                            ProjectId = x.ProjectId,
                                                            BranchId = x.BranchId,
                                                            Priority = x.Priority,
                                                            CustomerName_W = x.Project.customer.CustomerNameAr,
                                                            CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                                                            OutInTypeName = x.OutInType == 1 ? "داخلي" : "خارجي",
                                                            OutInBoxTypeName = x.OutInBoxType.NameAr,
                                                            ArchiveFilesName = x.ArchiveFiles.NameAr,
                                                            RelatedToSideTo = x.RelatedToOutIn.ToDepartment.DepartmentNameAr, //
                                                            InnerOutInDate = x.InnerToOutIn.Date,
                                                            RelatedToTopic = x.RelatedToOutIn.Topic,
                                                            RelatedToDate = x.RelatedToOutIn.Date,
                                                            SideFromName = x.FromDepartment.DepartmentNameAr,
                                                            SideToName = x.ToDepartment.DepartmentNameAr,
                                                            ProjectNumber = x.Project.ProjectNo ?? "",
                                                            Code = x.Code,
                                                        }).ToList().Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    return OutInBoxes;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
      public  async Task<IEnumerable<OutInBoxVM>> SearchOutInbox(OutInBoxVM OutInBoxesSearch, string DateFrom, string DateTo, int BranchId)
       {
            if (DateTo == null || DateFrom == null || DateTo == "" || DateFrom == "")
            {
                var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.Number == OutInBoxesSearch.Number || OutInBoxesSearch.Number == null) && (s.Topic == OutInBoxesSearch.Topic || s.Topic.Contains(OutInBoxesSearch.Topic) || OutInBoxesSearch.Topic == null)
               && (s.ToDepartment.DepartmentNameAr == OutInBoxesSearch.SideToName || s.ToDepartment.DepartmentNameAr.Contains(OutInBoxesSearch.SideToName) || OutInBoxesSearch.SideToName == null) && (s.FromDepartment.DepartmentNameAr == OutInBoxesSearch.SideFromName || s.FromDepartment.DepartmentNameAr.Contains(OutInBoxesSearch.SideFromName) || OutInBoxesSearch.SideFromName == null)
               && ((s.OutInType == OutInBoxesSearch.OutInType && s.Type == OutInBoxesSearch.Type) || OutInBoxesSearch.OutInType == null) && ((s.OutInType == OutInBoxesSearch.OutInType && s.Type == OutInBoxesSearch.Type) || OutInBoxesSearch.OutInType == null))
                                                .Select(x => new OutInBoxVM
                                                {
                                                    OutInBoxId = x.OutInBoxId,
                                                    Number = x.Number,
                                                    Date = x.Date,
                                                    HijriDate = x.HijriDate,
                                                    TypeId = x.TypeId,
                                                    SideFromId = x.SideFromId,
                                                    SideToId = x.SideToId,
                                                    InnerId = x.InnerId,
                                                    Topic = x.Topic,
                                                    ArchiveFileId = x.ArchiveFileId,
                                                    RelatedToId = x.RelatedToId,
                                                    Type = x.Type,
                                                    OutInType = x.OutInType,
                                                    FileCount = x.ContacFiles.Count,
                                                    AttachmentUrl = x.AttachmentUrl,
                                                    ProjectId = x.ProjectId,
                                                    BranchId = x.BranchId,
                                                    Priority = x.Priority,
                                                    CustomerName_W = x.Project.customer.CustomerNameAr,
                                                    CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                                                    OutInTypeName = (x.OutInType == 1 && x.Type == 1) ? "صادر داخلي" : (x.OutInType == 0 && x.Type == 1) ? "صادر خارجي" : (x.OutInType == 1 && x.Type == 2) ? "وارد داخلي" : (x.OutInType == 0 && x.Type == 2) ? "وارد خارجي" : "",
                                                    OutInBoxTypeName = x.OutInBoxType.NameAr,
                                                    ArchiveFilesName = x.ArchiveFiles.NameAr,
                                                    RelatedToSideTo = x.RelatedToOutIn.ToDepartment.DepartmentNameAr, //
                                                    InnerOutInDate = x.InnerToOutIn.Date,
                                                    RelatedToTopic = x.RelatedToOutIn.Topic,
                                                    RelatedToDate = x.RelatedToOutIn.Date,
                                                    SideFromName = x.FromDepartment.DepartmentNameAr,
                                                    SideToName = x.ToDepartment.DepartmentNameAr,
                                                    ProjectNumber = x.Project.ProjectNo ?? "",
                                                    Code = x.Code,
                                                });
                return OutInBoxes;
            }
            else
            {
                var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.Number == OutInBoxesSearch.Number || OutInBoxesSearch.Number == null) && (s.Topic == OutInBoxesSearch.Topic || s.Topic.Contains(OutInBoxesSearch.Topic) || OutInBoxesSearch.Topic == null)
               && (s.ToDepartment.DepartmentNameAr == OutInBoxesSearch.SideToName || s.ToDepartment.DepartmentNameAr.Contains(OutInBoxesSearch.SideToName) || OutInBoxesSearch.SideToName == null) && (s.FromDepartment.DepartmentNameAr == OutInBoxesSearch.SideFromName || s.FromDepartment.DepartmentNameAr.Contains(OutInBoxesSearch.SideFromName) || OutInBoxesSearch.SideFromName == null)
               && ((s.OutInType == OutInBoxesSearch.OutInType && s.Type == OutInBoxesSearch.Type) || OutInBoxesSearch.OutInType == null) && ((s.OutInType == OutInBoxesSearch.OutInType && s.Type == OutInBoxesSearch.Type) || OutInBoxesSearch.OutInType == null))
                                                .Select(x => new OutInBoxVM
                                                {
                                                    OutInBoxId = x.OutInBoxId,
                                                    Number = x.Number,
                                                    Date = x.Date,
                                                    HijriDate = x.HijriDate,
                                                    TypeId = x.TypeId,
                                                    SideFromId = x.SideFromId,
                                                    SideToId = x.SideToId,
                                                    InnerId = x.InnerId,
                                                    Topic = x.Topic,
                                                    ArchiveFileId = x.ArchiveFileId,
                                                    RelatedToId = x.RelatedToId,
                                                    Type = x.Type,
                                                    OutInType = x.OutInType,
                                                    FileCount = x.ContacFiles.Count,
                                                    AttachmentUrl = x.AttachmentUrl,
                                                    ProjectId = x.ProjectId,
                                                    BranchId = x.BranchId,
                                                    Priority = x.Priority,
                                                    CustomerName_W = x.Project.customer.CustomerNameAr,
                                                    CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                                                    OutInTypeName = (x.OutInType == 1 && x.Type == 1) ? "صادر داخلي" : (x.OutInType == 0 && x.Type == 1) ? "صادر خارجي" : (x.OutInType == 1 && x.Type == 2) ? "وارد داخلي" : (x.OutInType == 0 && x.Type == 2) ? "وارد خارجي" : "",
                                                    OutInBoxTypeName = x.OutInBoxType.NameAr,
                                                    ArchiveFilesName = x.ArchiveFiles.NameAr,
                                                    RelatedToSideTo = x.RelatedToOutIn.ToDepartment.DepartmentNameAr, //
                                                    InnerOutInDate = x.InnerToOutIn.Date,
                                                    RelatedToTopic = x.RelatedToOutIn.Topic,
                                                    RelatedToDate = x.RelatedToOutIn.Date,
                                                    SideFromName = x.FromDepartment.DepartmentNameAr,
                                                    SideToName = x.ToDepartment.DepartmentNameAr,
                                                    ProjectNumber = x.Project.ProjectNo ?? "",
                                                    Code = x.Code,
                                                }).ToList().Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                return OutInBoxes;
            }
        }
       public  async Task<IEnumerable<OutInBoxVM>> GetAllOutboxByDateSearch(string DateFrom, string DateTo, int BranchId, int Type)
        {
            var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type).Select(x => new OutInBoxVM
            {
                OutInBoxId = x.OutInBoxId,
                Number = x.Number,
                Date = x.Date,
                HijriDate = x.HijriDate,
                TypeId = x.TypeId,
                SideFromId = x.SideFromId,
                SideToId = x.SideToId,
                InnerId = x.InnerId,
                Topic = x.Topic,
                ArchiveFileId = x.ArchiveFileId,
                RelatedToId = x.RelatedToId,
                Type = x.Type,
                OutInType = x.OutInType,
                FileCount = x.ContacFiles.Count,
                AttachmentUrl = x.AttachmentUrl,
                ProjectId = x.ProjectId,
                BranchId = x.BranchId,
                Priority = x.Priority,
                CustomerName_W = x.Project.customer.CustomerNameAr,
                CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                OutInTypeName = x.OutInType == 1 ? "داخلي" : "خارجي",
                OutInBoxTypeName = x.OutInBoxType.NameAr,
                ArchiveFilesName = x.ArchiveFiles.NameAr,
                RelatedToSideTo = x.RelatedToOutIn.ToDepartment.DepartmentNameAr, //
                InnerOutInDate = x.InnerToOutIn.Date,
                RelatedToTopic = x.RelatedToOutIn.Topic,
                RelatedToDate = x.RelatedToOutIn.Date,
                SideFromName = x.FromDepartment.DepartmentNameAr,
                SideToName = x.ToDepartment.DepartmentNameAr,
                ProjectNumber = x.Project.ProjectNo ?? "",
                Code = x.Code,
            }).ToList().Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            return OutInBoxes;
        }
       public  async Task<IEnumerable<OutInBoxVM>> GetAllOutInboxByDateSearch(string DateFrom, string DateTo, int BranchId)
        {
            var OutInBoxes = _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false && s.BranchId == BranchId ).Select(x => new OutInBoxVM
            {
                OutInBoxId = x.OutInBoxId,
                Number = x.Number,
                Date = x.Date,
                HijriDate = x.HijriDate,
                TypeId = x.TypeId,
                SideFromId = x.SideFromId,
                SideToId = x.SideToId,
                InnerId = x.InnerId,
                Topic = x.Topic,
                ArchiveFileId = x.ArchiveFileId,
                RelatedToId = x.RelatedToId,
                Type = x.Type,
                OutInType = x.OutInType,
                FileCount = x.ContacFiles.Count,
                AttachmentUrl = x.AttachmentUrl,
                ProjectId = x.ProjectId,
                BranchId = x.BranchId,
                Priority = x.Priority,
                CustomerName_W = x.Project.customer.CustomerNameAr,
                CustomerName = x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project.customer.CustomerNameAr : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project.customer.CustomerNameAr + "(*)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project.customer.CustomerNameAr + "(**)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project.customer.CustomerNameAr + "(***)" : x.Project.customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project.customer.CustomerNameAr + "(VIP)" : x.Project.customer.CustomerNameAr,

                OutInTypeName = x.OutInType == 1 ? "داخلي" : "خارجي",
                OutInBoxTypeName = x.OutInBoxType.NameAr,
                ArchiveFilesName = x.ArchiveFiles.NameAr,
                RelatedToSideTo = x.RelatedToOutIn.ToDepartment.DepartmentNameAr, //
                InnerOutInDate = x.InnerToOutIn.Date,
                RelatedToTopic = x.RelatedToOutIn.Topic,
                RelatedToDate = x.RelatedToOutIn.Date,
                SideFromName = x.FromDepartment.DepartmentNameAr,
                SideToName = x.ToDepartment.DepartmentNameAr,
                ProjectNumber = x.Project.ProjectNo ?? "",
                Code = x.Code,
            }).ToList().Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            return OutInBoxes;
        }
        public async Task<int> GetMaxId()
        {
            return (_TaamerProContext.OutInBox.Count() > 0) ? _TaamerProContext.OutInBox.Max(s => s.OutInBoxId) :  0;
        }
        public async Task<int> GetOutboxCount(int BranchId)
        {
            return _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false && s.Type == 1 && s.BranchId == BranchId).Count();
        }
        public async Task<int> GetInboxCount(int BranchId)
        {
            return _TaamerProContext.OutInBox.Where(s => s.IsDeleted == false && s.Type == 2 && s.BranchId == BranchId).Count();
        }

     
    }
}
