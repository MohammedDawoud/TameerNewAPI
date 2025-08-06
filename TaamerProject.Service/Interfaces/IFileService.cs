using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<FileVM>> GetAllFiles(int? ProjectId, string SearchText, string DateFrom, string DateTo, int? Filetype, int BranchId);
        Task<IEnumerable<TasksLoadVM>> GetAllFilesTree(int? ProjectId, string? SearchText,bool? IsCertified, string DateFrom, string DateTo, int? Filetype, int BranchId);

        FileVM GetFileByBarcode(string Barcode, string taxCode);
        Task<IEnumerable<FileVM>> GetFileByBarcodeShare(string ProjectNo, string taxCode);

        FileVM GetFileByBarcode2(string Barcode, string taxCode);


        Task<IEnumerable<FileVM>> GetAllCertificateFiles(int? ProjectId, bool IsCertified, int BranchId);
        Task<IEnumerable<FileVM>> GetAllFilesByDateSearch(int? ProjectId, DateTime DateFrom, DateTime DateTo, int BranchId);
        GeneralMessage SaveFile(ProjectFiles file, int UserId, int BranchId);
        GeneralMessage UpdateFileShare(ProjectFiles file, int BranchId, string Link, int UserId, string imgurl, string url);
        GeneralMessage UpdateFileUploadFileId(int FileId,string UploadFileId,int type);

        GeneralMessage NotUpdateFileShare(ProjectFiles file, int BranchId, string Link, int UserId, string imgurl, string url);


        GeneralMessage SaveFile_Bar(ProjectFiles file, int UserId, int BranchId);

        GeneralMessage DeleteFile(int FileId, int UserId, int BranchId);
        GeneralMessage DeleteFileDrive(int FileId, int UserId, int BranchId);
        GeneralMessage DeleteFileOneDrive(int FileId, int UserId, int BranchId);

        Task<IEnumerable<FileVM>> GetAllTaskFiles(int TaskId, string SearchText);
        Task<FileVM> GetFilesById(int FileId);
        Task<int> GetUserFileUploadCount(int? UserId);
        GeneralMessage ADDFileComment(int FileId, int UserId, int BranchId, string Comment, string Url, string ImgUrl);
    }
}
