using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IFileRepository : IRepository<ProjectFiles>
    {
        Task<IEnumerable<FileVM>> GetAllFiles(int? ProjectId, string SearchText, string DateFrom, string DateTo, int? Filetype, int BranchId);
        Task<IEnumerable<FileVM>> GetAllFilesTree(int? ProjectId, string? SearchText, bool? IsCertified, string? DateFrom, string? DateTo, int? Filetype, int BranchId);

        Task<FileVM> GetFileByBarcode(string Barcode, string taxCode);
        Task<IEnumerable<FileVM>> GetFileByBarcodeShare(string ProjectNo, string taxCode);

        Task<FileVM> GetFileByBarcode2(string Barcode, string taxCode);


        Task<IEnumerable<FileVM>> GetAllCertificateFiles(int? ProjectId, bool IsCertified, int BranchId);
        Task<IEnumerable<FileVM>> GetAllFilesByDateSearch(int? ProjectId, DateTime DateFrom, DateTime DateTo, int BranchId);
        Task<IEnumerable<FileVM>> GetAllTaskFiles(int TaskId, string SearchText);
        Task<FileVM> GetFilesById(int FileId);
        Task<int> GetUserFileUploadCount(int? UserId);
    }
}
