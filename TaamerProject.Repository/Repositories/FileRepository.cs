using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using System.Globalization;

namespace TaamerProject.Repository.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public FileRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<FileVM>> GetAllFiles(int? ProjectId, string SearchText, string DateFrom, string DateTo, int? Filetype, int BranchId)
        {
          
            if (DateFrom=="")
            {
                DateFrom = null;
            }
            if (DateTo == "")
            {
                DateTo = null;
            }
            if (DateFrom != null|| DateTo != null )
            {
                var Files1 = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && s.BranchId == BranchId  && (s.ProjectId == ProjectId || ProjectId == null)).Select(x => new FileVM
                {
                    FileId = x.FileId,
                    FileName = x.FileName ?? "",
                    FileUrl = x.FileUrl,
                    Extension = x.Extension,
                    TypeId = x.TypeId,
                    Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                    IsCertified = x.IsCertified ?? false,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                    TaskId = x.TaskId,
                    Brand = x.Brand,
                    FileSize = x.FileSize,
                    UserId = x.UserId,
                    NotificationId = x.NotificationId,
                    BranchId = x.BranchId,
                    Type = x.Type ?? "",
                    DeleteUrl = x.DeleteUrl,
                    ThumbnailUrl = x.ThumbnailUrl,
                    DeleteType = x.DeleteType,
                    FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                    UploadDate = x.UploadDate,
                    UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                    ProStatus = x.Project!.Status,
                    IsShare = x.IsShare == null ? false : x.IsShare,
                    ViewShare = x.ViewShare == null ? false : x.ViewShare,
                    DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                    TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                    TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                    TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                    CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                    CustomerName = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerNameAr ?? "",
                    ProjectType = x.Project == null ? "" : x.Project!.projecttype == null ? "" : x.Project!.projecttype.NameAr ?? "",
                    FileUrlW = x.FileUrlW ?? "",
                    CustomeComment = x.CustomeComment ?? "",
                    PageInsert = x.PageInsert ?? 0,
                    PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                    UploadType = x.UploadType ?? 0,
                    UploadName = x.UploadName ?? "",
                    UploadFileId = x.UploadFileId ?? "",
                    UploadFileIdB = x.UploadFileIdB ?? "",


                }).ToList().Where(s => DateTime.ParseExact(s.UploadDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.UploadDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                //.Where(s => DateTime.ParseExact(s.UploadDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.UploadDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                //Where(s => DateTime.ParseExact(s.UploadDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.UploadDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                //Where(s => s.UploadDate >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) &&  s.UploadDate <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)); 

                if (SearchText != "")
                {

                    Files1 = Files1.Where(s => s.FileName.Contains(SearchText.Trim())).ToList();
                }
                return Files1;
            }
            else
            {
                var Files2 = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.ProjectId == ProjectId || ProjectId == null) && (s.TypeId == Filetype || Filetype == null)).Select(x => new FileVM
                {
                    FileId = x.FileId,
                    FileName = x.FileName ?? "",
                    FileUrl = x.FileUrl,
                    Extension = x.Extension,
                    TypeId = x.TypeId,
                    Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                    IsCertified = x.IsCertified ?? false,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                    TaskId = x.TaskId,
                    Brand = x.Brand,
                    FileSize = x.FileSize,
                    UserId = x.UserId,
                    NotificationId = x.NotificationId,
                    BranchId = x.BranchId,
                    Type = x.Type ?? "",
                    DeleteUrl = x.DeleteUrl,
                    ThumbnailUrl = x.ThumbnailUrl,
                    DeleteType = x.DeleteType,
                    FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                    UploadDate = x.UploadDate,
                    UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    AddedFileImg = x.Users != null ? x.Users.ImgUrl == "/distnew/images/userprofile.png" ? "/distnew/images/userprofile.png" : x.Users!.ImgUrl : "/distnew/images/userprofile.png",
                    ProjectMangerName = x.Project!.Users!.FullNameAr == null ? x.Project!.Users!.FullName : x.Project!.Users!.FullNameAr,
                    ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl == "/distnew/images/userprofile.png" ? "/distnew/images/userprofile.png" : x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png",
                    TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                    ProStatus = x.Project!.Status,
                    IsShare = x.IsShare == null ? false : x.IsShare,
                    ViewShare = x.ViewShare == null ? false : x.ViewShare,
                    DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                    TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                    TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                    TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                    CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                    CustomerName = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerNameAr ?? "",
                    ProjectType = x.Project == null ? "" : x.Project!.projecttype == null ? "" : x.Project!.projecttype.NameAr ?? "",
                    FileUrlW = x.FileUrlW ?? "",
                    CustomeComment = x.CustomeComment ?? "",
                    PageInsert = x.PageInsert ?? 0,
                    PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                    UploadType = x.UploadType ?? 0,
                    UploadName = x.UploadName ?? "",
                    UploadFileId = x.UploadFileId ?? "",
                    UploadFileIdB = x.UploadFileIdB ?? "",

                }).ToList();
                if (SearchText != "")
                {

                    Files2 = Files2.Where(s => s.FileName.Contains(SearchText.Trim())).ToList();
                }
                return Files2;
            }
             
           
        }
        public async Task<IEnumerable<FileVM>> GetAllFilesTree(int? ProjectId, string? SearchText, bool? IsCertified, string? DateFrom, string? DateTo, int? Filetype, int BranchId)
        {
            if (DateFrom == ""){DateFrom = null;} 
            if (DateTo == ""){ DateTo = null;}
            if (DateFrom != null || DateTo != null)
            {
                var Files1 = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.ProjectId == ProjectId || ProjectId == null) && (s.IsCertified == IsCertified || IsCertified == null) && (s.TypeId == Filetype || Filetype == null)).Select(x => new FileVM
                {
                    FileId = x.FileId,
                    FileName = x.FileName ?? "",
                    FileUrl = x.FileUrl,
                    Extension = x.Extension,
                    TypeId = x.TypeId,
                    Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                    IsCertified = x.IsCertified ?? false,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                    TaskId = x.TaskId,
                    Brand = x.Brand,
                    FileSize = x.FileSize,
                    UserId = x.UserId,
                    NotificationId = x.NotificationId,
                    BranchId = x.BranchId,
                    Type = x.Type ?? "",
                    DeleteUrl = x.DeleteUrl,
                    ThumbnailUrl = x.ThumbnailUrl,
                    DeleteType = x.DeleteType,
                    FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                    UploadDate = x.UploadDate,
                    UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                    ProStatus = x.Project!.Status,
                    IsShare = x.IsShare == null ? false : x.IsShare,
                    ViewShare = x.ViewShare == null ? false : x.ViewShare,
                    DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                    TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                    TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                    TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                    CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                    CustomerName = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerNameAr ?? "",
                    ProjectType = x.Project == null ? "" : x.Project!.projecttype == null ? "" : x.Project!.projecttype.NameAr ?? "",
                    FileUrlW = x.FileUrlW ?? "",
                    CustomeComment = x.CustomeComment ?? "",
                    PageInsert = x.PageInsert ?? 0,
                    PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                    UploadType = x.UploadType ?? 0,
                    UploadName = x.UploadName ?? "",
                    UploadFileId = x.UploadFileId ?? "",
                    UploadFileIdB = x.UploadFileIdB ?? "",

                }).ToList().Where(s => DateTime.ParseExact(s.UploadDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.UploadDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                if (SearchText != "" && SearchText != null)
                {

                    Files1 = Files1.Where(s => s.FileName.Contains(SearchText.Trim())).ToList();
                }
                return Files1;
            }
            else
            {
                var Files2 = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.ProjectId == ProjectId || ProjectId == null) && (s.IsCertified == IsCertified || IsCertified == null) && (s.TypeId == Filetype || Filetype == null)).Select(x => new FileVM
                {
                    FileId = x.FileId,
                    FileName = x.FileName ?? "",
                    FileUrl = x.FileUrl,
                    Extension = x.Extension,
                    TypeId = x.TypeId,
                    Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                    IsCertified = x.IsCertified ?? false,
                    ProjectId = x.ProjectId,
                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                    TaskId = x.TaskId,
                    Brand = x.Brand,
                    FileSize = x.FileSize,
                    UserId = x.UserId,
                    NotificationId = x.NotificationId,
                    BranchId = x.BranchId,
                    Type = x.Type ?? "",
                    DeleteUrl = x.DeleteUrl,
                    ThumbnailUrl = x.ThumbnailUrl,
                    DeleteType = x.DeleteType,
                    FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                    UploadDate = x.UploadDate,
                    UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                    ProStatus = x.Project!.Status,
                    IsShare = x.IsShare == null ? false : x.IsShare,
                    ViewShare = x.ViewShare == null ? false : x.ViewShare,
                    DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                    TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                    TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                    TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                    CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                    CustomerName = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerNameAr ?? "",
                    ProjectType = x.Project == null ? "" : x.Project!.projecttype == null ? "" : x.Project!.projecttype.NameAr ?? "",
                    FileUrlW = x.FileUrlW ?? "",
                    CustomeComment = x.CustomeComment ?? "",
                    PageInsert = x.PageInsert ?? 0,
                    PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                    UploadType = x.UploadType ?? 0,
                    UploadName = x.UploadName ?? "",
                    UploadFileId = x.UploadFileId ?? "",
                    UploadFileIdB = x.UploadFileIdB ?? "",

                }).ToList();
                if (SearchText != "" && SearchText != null)
                {

                    Files2 = Files2.Where(s => s.FileName.Contains(SearchText.Trim())).ToList();
                }
                return Files2;
            }


        }

        public async Task<FileVM> GetFileByBarcode(string Barcode,string taxCode)
        {
            var Files = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && s.BarcodeFileNum == Barcode && s.CompanyTaxNo==taxCode && s.Project.Status == 1).Select(x => new FileVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                TypeId = x.TypeId,
                Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                IsCertified = x.IsCertified ?? false,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                TaskId = x.TaskId,
                Brand = x.Brand,
                FileSize = x.FileSize,
                UserId = x.UserId,
                NotificationId = x.NotificationId,
                BranchId = x.BranchId,
                Type = x.Type ?? "",
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                UploadDate = x.UploadDate,
                UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                ProStatus = x.Project!.Status,
                IsShare = x.IsShare == null ? false : x.IsShare,
                ViewShare = x.ViewShare == null ? false : x.ViewShare,
                DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                CustomerName=x.Project ==null ?"" :x.Project!.customer ==null ?"" :x.Project!.customer!.CustomerNameAr ??"",
                ProjectType= x.Project ==null ?"" :x.Project!.projecttype ==null ?"" :x.Project!.projecttype.NameAr ??"",
                FileUrlW = x.FileUrlW ?? "",
                CustomeComment=x.CustomeComment??"",
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                UploadType = x.UploadType ?? 0,
                UploadName = x.UploadName ?? "",
                UploadFileId = x.UploadFileId ?? "",
                UploadFileIdB = x.UploadFileIdB ?? "",

            }).FirstOrDefault();

            return Files;

        }

        public async Task< IEnumerable<FileVM>> GetFileByBarcodeShare(string ProjectNo, string taxCode)
        {
            var Files = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && s.Project.ProjectNo == ProjectNo && s.CompanyTaxNo == taxCode /*&& s.Project.Status == 1*/).Select(x => new FileVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                TypeId = x.TypeId,
                Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                IsCertified = x.IsCertified ?? false,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                TaskId = x.TaskId,
                Brand = x.Brand,
                FileSize = x.FileSize,
                UserId = x.UserId,
                NotificationId = x.NotificationId,
                BranchId = x.BranchId,
                Type = x.Type ?? "",
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                UploadDate = x.UploadDate,
                UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                ProStatus = x.Project!.Status,
                IsShare = x.IsShare == null ? false : x.IsShare,
                ViewShare = x.ViewShare == null ? false : x.ViewShare,
                DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                CustomerName=x.Project==null?"":x.Project!.customer ==null ?"" :x.Project!.customer!.CustomerNameAr??"",
                ProjectType = x.Project == null ? "" : x.Project!.projecttype == null ? "" : x.Project!.projecttype.NameAr ?? "",
                FileUrlW = x.FileUrlW ?? "",
                CustomeComment = x.CustomeComment ?? "",
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                UploadType = x.UploadType ?? 0,
                UploadName = x.UploadName ?? "",
                UploadFileId = x.UploadFileId ?? "",
                UploadFileIdB = x.UploadFileIdB ?? "",

            }).ToList();

            return Files;

        }


        public async Task<FileVM> GetFileByBarcode2(string Barcode, string taxCode)
        {
            var Files = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && s.BarcodeFileNum == Barcode && s.CompanyTaxNo == taxCode && s.Project.Status == 1).Select(x => new FileVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                TypeId = x.TypeId,
                Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                IsCertified = x.IsCertified ?? false,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                TaskId = x.TaskId,
                Brand = x.Brand,
                FileSize = x.FileSize,
                UserId = x.UserId,
                NotificationId = x.NotificationId,
                BranchId = x.BranchId,
                Type = x.Type ?? "",
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                UploadDate = x.UploadDate,
                UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                ProStatus = x.Project!.Status,
                IsShare = x.IsShare == null ? false : x.IsShare,
                ViewShare = x.ViewShare == null ? false : x.ViewShare,
                DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                CustomerName = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerNameAr ?? "",
                ProjectType = x.Project == null ? "" : x.Project!.projecttype == null ? "" : x.Project!.projecttype.NameAr ?? "",
                FileUrlW = x.FileUrlW ?? "",
                CustomeComment = x.CustomeComment ?? "",
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                UploadType = x.UploadType ?? 0,
                UploadName = x.UploadName ?? "",
                UploadFileId = x.UploadFileId ?? "",
                UploadFileIdB = x.UploadFileIdB ?? "",

            }).FirstOrDefault();

            return Files;

        }


        public async Task<IEnumerable<FileVM>> GetAllTaskFiles(int TaskId, string SearchText)
        {
            var Files = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && s.TaskId == TaskId).Select(x => new FileVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                TypeId = x.TypeId,
                Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                IsCertified = x.IsCertified ?? false,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                TaskId = x.TaskId,
                Brand = x.Brand,
                FileSize = x.FileSize,
                UserId = x.UserId,
                NotificationId = x.NotificationId,
                BranchId = x.BranchId,
                Type = x.Type ?? "",
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                UploadDate = x.UploadDate,
                UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                ProStatus = x.Project!.Status,
                IsShare = x.IsShare == null ? false : x.IsShare,
                ViewShare = x.ViewShare == null ? false : x.ViewShare,
                DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                CustomerName = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerNameAr ?? "",
                ProjectType = x.Project == null ? "" : x.Project!.projecttype == null ? "" : x.Project!.projecttype.NameAr ?? "",
                FileUrlW = x.FileUrlW ?? "",
                CustomeComment = x.CustomeComment ?? "",
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                UploadType = x.UploadType ?? 0,
                UploadName = x.UploadName ?? "",
                UploadFileId = x.UploadFileId ?? "",
                UploadFileIdB = x.UploadFileIdB ?? "",

            }).ToList();
            if (SearchText != "")
            {
                Files = Files.Where(s => s.FileName.Contains(SearchText.Trim())).ToList();
            }
            return Files;
        }

        public async Task<IEnumerable<FileVM>> GetAllCertificateFiles(int? ProjectId, bool IsCertified, int BranchId)
        {
            var Files = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && (s.ProjectId == ProjectId || ProjectId == null) && s.IsCertified == IsCertified && s.BranchId == BranchId).Select(x => new FileVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                TypeId = x.TypeId,
                Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                IsCertified = x.IsCertified ?? false,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                TaskId = x.TaskId,
                Brand = x.Brand,
                FileSize = x.FileSize,
                UserId = x.UserId,
                NotificationId = x.NotificationId,
                BranchId = x.BranchId,
                Type = x.Type ?? "",
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                UploadDate = x.UploadDate,
                UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                ProStatus = x.Project!.Status,
                IsShare = x.IsShare == null ? false : x.IsShare,
                ViewShare = x.ViewShare == null ? false : x.ViewShare,
                DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                CustomerName = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerNameAr ?? "",
                ProjectType = x.Project == null ? "" : x.Project!.projecttype == null ? "" : x.Project!.projecttype.NameAr ?? "",
                FileUrlW = x.FileUrlW ?? "",
                CustomeComment = x.CustomeComment ?? "",
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                UploadType = x.UploadType ?? 0,
                UploadName = x.UploadName ?? "",
                UploadFileId = x.UploadFileId ?? "",
                UploadFileIdB = x.UploadFileIdB ?? "",

            }).ToList();
            return Files;
        }
         
        public async Task<IEnumerable<FileVM>> GetAllFilesByDateSearch(int? ProjectId, DateTime DateFrom, DateTime DateTo, int BranchId)
        {
            var Files = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && 
           // s.UploadDate >= DateFrom && s.UploadDate <= DateTo && 
            (s.ProjectId == ProjectId || ProjectId == null) && s.BranchId == BranchId).Select(x => new FileVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                TypeId = x.TypeId,
                Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                IsCertified = x.IsCertified ?? false,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                TaskId = x.TaskId,
                Brand = x.Brand,
                FileSize = x.FileSize,
                UserId = x.UserId,
                NotificationId = x.NotificationId,
                BranchId = x.BranchId,
                Type = x.Type ?? "",
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                UploadDate = x.UploadDate,
                UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                ProStatus = x.Project!.Status,
                IsShare = x.IsShare == null ? false : x.IsShare,
                ViewShare = x.ViewShare == null ? false : x.ViewShare,
                DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                CustomerName = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerNameAr ?? "",
                ProjectType = x.Project == null ? "" : x.Project!.projecttype == null ? "" : x.Project!.projecttype.NameAr ?? "",
                FileUrlW = x.FileUrlW ?? "",
                CustomeComment = x.CustomeComment ?? "",
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                UploadType = x.UploadType ?? 0,
                UploadName = x.UploadName ?? "",
                UploadFileId = x.UploadFileId ?? "",
                UploadFileIdB = x.UploadFileIdB ?? "",

            }).ToList();
            return Files;
        }
        public async Task<FileVM> GetFilesById(int FileId)
        {
            var Files = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && s.FileId == FileId).Select(x => new FileVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                TypeId = x.TypeId,
                Notes = x.Notes == "undefined" ? "" : x.Notes ?? "",
                IsCertified = x.IsCertified ?? false,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                TaskId = x.TaskId,
                Brand = x.Brand,
                FileSize = x.FileSize,
                UserId = x.UserId,
                NotificationId = x.NotificationId,
                BranchId = x.BranchId,
                Type = x.Type ?? "",
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                FileTypeName = x.FileType != null ? x.FileType.NameAr ?? "" : "",
                UploadDate = x.UploadDate,
                UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                AddedFileImg = x.Users != null ? x.Users.ImgUrl == "/distnew/images/userprofile.png" ? "/distnew/images/userprofile.png" : x.Users!.ImgUrl : "/distnew/images/userprofile.png",
                ProjectMangerName = x.Project!.Users!.FullNameAr == null ? x.Project!.Users!.FullName : x.Project!.Users!.FullNameAr,
                ProjectManagerImg = x.Project!.Users != null ? x.Project!.Users.ImgUrl == "/distnew/images/userprofile.png" ? "/distnew/images/userprofile.png" : x.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png",
                TaskName = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? "" : "",
                ProStatus = x.Project!.Status,
                IsShare = x.IsShare == null ? false : x.IsShare,
                ViewShare = x.ViewShare == null ? false : x.ViewShare,
                DonwloadShare = x.DonwloadShare == null ? false : x.DonwloadShare,
                TimeShare = x.TimeShare == null ? 0 : x.TimeShare,
                TimeTypeShare = x.TimeTypeShare == null ? 2 : x.TimeTypeShare,   //1 - hour     2 - day
                TimeShareDate = x.TimeShareDate == null ? "" : x.TimeShareDate,
                CustomerEmail = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerEmail ?? "",
                CustomerName = x.Project == null ? "" : x.Project!.customer == null ? "" : x.Project!.customer!.CustomerNameAr ?? "",
                ProjectType = x.Project == null ? "" : x.Project!.projecttype == null ? "" : x.Project!.projecttype.NameAr ?? "",
                FileUrlW = x.FileUrlW ?? "",
                CustomeComment = x.CustomeComment ?? "",
                PageInsert = x.PageInsert ?? 0,
                PageInsertName = x.PageInsert == 1 ? "من مهمة" : x.PageInsert == 2 ? "من مركز رفع الملفات" : x.PageInsert == 3 ? "من استمارة المشروع" : x.PageInsert == 4 ? "من مهمة ادارية" : x.PageInsert == 5 ? "من عقد" : "",
                UploadType = x.UploadType ?? 0,
                UploadName = x.UploadName ?? "",
                UploadFileId = x.UploadFileId ?? "",
                UploadFileIdB = x.UploadFileIdB ?? "",

            }).First();
            return Files;
        }
        public async Task<int> GetUserFileUploadCount(int? UserId)
        {
            var UserFileUploadCount = _TaamerProContext.ProjectFiles.Where(s => s.IsDeleted == false && s.UserId == UserId);
            return UserFileUploadCount.Count();
        }

        public IEnumerable<ProjectFiles> GetAll()
        {
            throw new NotImplementedException();
        }

        public ProjectFiles GetById(int Id)
        {
          return  _TaamerProContext.ProjectFiles.Where(x => x.FileId == Id).FirstOrDefault();
        }

        public IEnumerable<ProjectFiles> GetMatching(Func<ProjectFiles, bool> where)
        {
            return _TaamerProContext.ProjectFiles.Where(where).ToList<ProjectFiles>();
        }
    }
}


