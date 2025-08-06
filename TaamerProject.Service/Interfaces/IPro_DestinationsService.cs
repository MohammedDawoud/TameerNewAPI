using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_DestinationsService
    {
        Task<IEnumerable<Pro_DestinationsVM>> GetAllDestinations(int BranchId, List<int> BranchesList);
        Task<Pro_DestinationsVM> GetDestinationByProjectId(int projectId);
        Task<Pro_DestinationsVM> GetDestinationByProjectIdToReplay(int projectId);

        //GeneralMessage SaveDestination(Pro_Destinations Destination, int UserId, int BranchId);
        //GeneralMessage SaveDestinationReplay(Pro_Destinations Destination, int UserId, int BranchId);
        GeneralMessage DeleteDestination(int DestinationId, int UserId, int BranchId);
        GeneralMessage SaveDestination(Pro_Destinations Destination, int UserId, int BranchId, OrganizationsVM Organization, string Url, string ImgUrl);
        GeneralMessage SaveDestinationNotifi(Pro_Destinations Destination, int UserId, int BranchId, OrganizationsVM Organization, string Url, string ImgUrl);
        GeneralMessage SaveDestinationReplay(Pro_Destinations Destination, int UserId, int BranchId, OrganizationsVM Organization, string Url, string ImgUrl);
        GeneralMessage SaveDestinationReplayNotifi(Pro_Destinations Destination, int UserId, int BranchId, OrganizationsVM Organization, string Url, string ImgUrl);

    }
}
