using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_SupervisionDetailsService  
    {
        Task<IEnumerable<Pro_SupervisionDetailsVM>> GetAllSupervisionDetails(string SearchText);
        Task<IEnumerable<Pro_SupervisionDetailsVM>> GetAllSupervisionDetailsBySuperId(int? SupervisionId);

        GeneralMessage SaveSupervisionDetails(Pro_SupervisionDetails SupervisionDetails, int UserId, int BranchId);
        GeneralMessage SaveSuperDet(List<Pro_SupervisionDetails> Det, int UserId, int BranchId);

        GeneralMessage DeleteSupervisionDetails(int SupervisionDetId, int UserId, int BranchId);
        GeneralMessage ReciveDetails(int SuperDetId, int UserId, int BranchId);
        GeneralMessage NotReciveDetails(int SuperDetId,string Note, int UserId, int BranchId);
        GeneralMessage TheNumberSuperDet(int SuperDetId, string Note, int UserId, int BranchId);
        GeneralMessage TheLocationSuperDet(int SuperDetId, string Note, int UserId, int BranchId);

        GeneralMessage NotFoundDetails(int SuperDetId, string Note, int UserId, int BranchId);
        GeneralMessage AddNumberSuperDet(int SuperDetId, string Note,int Type, int UserId, int BranchId);

        GeneralMessage UploadImageSuperDet(int SuperDetId, string ImageUrl, int UserId, int BranchId);

    }
}
