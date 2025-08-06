using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ISupervisionsRepository
    {
        Task<IEnumerable<SupervisionsVM>> GetAllSupervisions(int? ProjectId);
        Task<IEnumerable<SupervisionsVM>> GetAllBySupervisionId(int SupervisionId);
        Task<IEnumerable<SupervisionsVM>> GetAllSupervisionsByUserId(int? UserId);
        Task<IEnumerable<SupervisionsVM>> GetAllSupervisionsByUserIdHome(int? UserId);
        Task<IEnumerable<SupervisionsVM>> GetAllSupervisions_Search(int? ProjectId, int? UserId, int? EmpId, int? PhaseId, string DateFrom, string Dateto);
        Task<IEnumerable<SupervisionsVM>> GetAllSupervisions_Search(int? ProjectId, int? UserId, int? EmpId, int? PhaseId, string DateFrom, string Dateto, string? Searchtext);
        Task<int> GenerateNextSupNumber();


    }
}
