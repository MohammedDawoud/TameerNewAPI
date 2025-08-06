using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAttTimeDetailsRepository
    {
        Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails(string SearchText,int AttTimeId);
        Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails();
        Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails2(int AttTimeId);
        Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails2bybranchid(int branchid);
        Task<IEnumerable<AttTimeDetailsVM>> CheckUserPerDawamUserExist(int UserId, string TimeFrom, string TimeTo, int DayNo,int AttTimeId);
        Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetailsByid(int AttTimeId);

    }
}
