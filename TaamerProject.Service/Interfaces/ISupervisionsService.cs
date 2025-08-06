using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ISupervisionsService  
    {
        Task<IEnumerable<SupervisionsVM>> GetAllSupervisions(int? ProjectId);
        IEnumerable<Supervisions> GetAllSupervisionsProject(int BranchId);
        Task<IEnumerable<SupervisionsVM>> GetAllBySupervisionId(int SupervisionId);
        Task<IEnumerable<SupervisionsVM>> GetAllSupervisionsByUserId(int? UserId);
        Task<IEnumerable<SupervisionsVM>> GetAllSupervisionsByUserIdHome(int? UserId);
        Task<IEnumerable<SupervisionsVM>> GetAllSupervision_Search(int? ProjectId, int? UserId, int? EmpId, int? PhaseId, string DateFrom, string Dateto);
        Task<IEnumerable<SupervisionsVM>> GetAllSupervision_Search(int? ProjectId, int? UserId, int? EmpId, int? PhaseId, string DateFrom, string Dateto, string? SearchText);
        //GeneralMessage SupervisionAvailability(Supervisions supervisions, int UserId, int BranchId);
        GeneralMessage SupervisionAvailability(Supervisions supervisions, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage DeleteSupervision(int SupervisionId, int UserId, int BranchId);
        GeneralMessage SendMSupervision(int SupervisionId, int EmailStatusCustomer, int EmailStatusContractor, int EmailStatusOffice, int UserId, int BranchId, string AttachmentFile);
        GeneralMessage SendWSupervision(int SupervisionId, int EmailStatusCustomer, int EmailStatusContractor, int EmailStatusOffice, int UserId, int BranchId, string PDFURL, string environmentURL);


        GeneralMessage ReadSupervision(int SupervisionId, int UserId, int BranchId);
        GeneralMessage ReciveSuper(int SupervisionId, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage OutlineChangeSave(int SupervisionId, string OutlineChangetxt1, string OutlineChangetxt2, string OutlineChangetxt3, int UserId, int BranchId);

        GeneralMessage PointsNotWrittenSave(int SupervisionId, string PointsNotWrittentxt1, string PointsNotWrittentxt2, string PointsNotWrittentxt3, int UserId, int BranchId);


        GeneralMessage NotReciveSuper(int SupervisionId, int UserId, int BranchId);

        GeneralMessage ConfirmSupervision(int SupervisionId, int UserId, int BranchId, int TypeId, int TypeIdAdmin);
        GeneralMessage UploadHeadImageFir(int SupervisionId, string ImageUrl, int UserId, int BranchId);
        GeneralMessage UploadHeadImageSec(int SupervisionId, string ImageUrl, int UserId, int BranchId);


        Task<int> GenerateNextSupNumber();

        //GeneralMessage UpdateExtractAttachment(ProjectExtracts projectExtracts);
        //GeneralMessage UpdateExtractSignature(ProjectExtracts projectExtracts);
    }
}
