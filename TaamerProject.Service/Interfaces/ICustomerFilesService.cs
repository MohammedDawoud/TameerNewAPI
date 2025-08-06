using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICustomerFilesService
    {
        Task<IEnumerable<CustomerFilesVM>> GetAllCustomerFilesUploaded(int CustomerId, string SearchText);
        GeneralMessage UploadCustomerFiles(CustomerFiles customerFiles, int UserId, int BranchId);
        GeneralMessage DeleteUpoladCustomerFiles(int FileId, int UserId, int BranchId);
        GeneralMessage SaveCustomerFiles(CustomerFiles customerFiles, int UserId, int BranchId);
        Task<IEnumerable<CustomerFilesVM>?> GetAllCustomerFiles(int? CustomerId);
    }
}
