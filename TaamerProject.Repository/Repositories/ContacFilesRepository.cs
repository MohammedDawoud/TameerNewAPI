using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class ContacFilesRepository :  IContacFilesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ContacFilesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<ContacFilesVM>> GetAllFiles(int? OutInBoxId)
        {
            var Files = _TaamerProContext.ContacFiles.Where(s => s.IsDeleted == false && s.OutInBoxId == OutInBoxId).Select(x => new ContacFilesVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                Notes = x.Notes ?? "",
                IsCertified = x.IsCertified,
                OutInBoxId = x.OutInBoxId,
                FileSize = x.FileSize,
                UserId = x.UserId,
                BranchId = x.BranchId,
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                UploadDate = x.UploadDate,
                OutInBoxNumber = x.OutInBox.Number ?? "",
                OutInBoxDate = x.OutInBox.Date,
                UserName = x.Users.FullName ?? "",
            }).ToList();
            return Files;
        }
        public async Task<IEnumerable<ContacFilesVM>> GetAllContacFiles()
        {
            var Files = _TaamerProContext.ContacFiles.Where(s => s.IsDeleted == false).Select(x => new ContacFilesVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                Notes = x.Notes ?? "",
                IsCertified = x.IsCertified,
                OutInBoxId = x.OutInBoxId,
                FileSize = x.FileSize,
                UserId = x.UserId,
                BranchId = x.BranchId,
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                UploadDate = x.UploadDate,
                OutInBoxNumber = x.OutInBox.Number ?? "",
                OutInBoxDate = x.OutInBox.Date,
                UserName = x.Users.FullName ?? "",
            }).ToList();
            return Files;
        }
        public async Task<IEnumerable<ContacFilesVM>> GetAllFilesByParams (int? ArchiveFileId , int? Type , int? OutInType,int BranchId)
        {
            var Files = _TaamerProContext.ContacFiles.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.OutInBox.ArchiveFileId == ArchiveFileId || ArchiveFileId == null) && ((s.OutInBox.Type == Type && s.OutInBox.OutInType == OutInType)|| Type == null) ).Select(x => new ContacFilesVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                Notes = x.Notes ?? "",
                IsCertified = x.IsCertified,
                OutInBoxId = x.OutInBoxId,
                FileSize = x.FileSize,
                UserId = x.UserId,
                BranchId = x.BranchId,
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                UploadDate = x.UploadDate,
                OutInBoxNumber = x.OutInBox.Number ?? "",
                OutInBoxDate = x.OutInBox.Date,
                UserName = x.Users.FullName ?? "",
            }).ToList();
            return Files;
        }

        public async Task<IEnumerable<ContacFilesVM>> GetAllFilesByParams(int? ArchiveFileId, int? Type, int? OutInType,int? OutInboxId, int BranchId)
        {
            var Files = _TaamerProContext.ContacFiles.Where(s => s.IsDeleted == false && 
            (s.OutInBox.ArchiveFileId == ArchiveFileId || ArchiveFileId == null) &&
            ((s.OutInBox.Type == Type && s.OutInBox.OutInType == OutInType))
            &&(s.OutInBoxId==OutInboxId ||OutInboxId==null || OutInboxId==0)).Select(x => new ContacFilesVM
            {
                FileId = x.FileId,
                FileName = x.FileName ?? "",
                FileUrl = x.FileUrl,
                Extension = x.Extension,
                Notes = x.Notes ?? "",
                IsCertified = x.IsCertified,
                OutInBoxId = x.OutInBoxId,
                FileSize = x.FileSize,
                UserId = x.UserId,
                BranchId = x.BranchId,
                DeleteUrl = x.DeleteUrl,
                ThumbnailUrl = x.ThumbnailUrl,
                DeleteType = x.DeleteType,
                UploadDate = x.UploadDate,
                OutInBoxNumber = x.OutInBox.Number ?? "",
                OutInBoxDate = x.OutInBox.Date,
                UserName = x.Users.FullName ?? "",
            }).ToList();
            return Files;
        }

    }
}


