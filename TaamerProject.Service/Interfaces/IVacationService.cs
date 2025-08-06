using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IVacationService 
    {
        Task<IEnumerable<VacationVM>> GetAllVacations(int? EmpId, string SearchText);
        Task<IEnumerable<VacationVM>> GetAllVacationsArchived(int? EmpId, string SearchText);
        Task<IEnumerable<VacationVM>> GetAllVacations2(int? UserId, string SearchText);

        Task<IEnumerable<VacationVM>> GetAllVacationsw(string SearchText);

        Task<IEnumerable<VacationVM>> GetAllVacationsSearch(VacationVM VacationSearch, int BranchId);
        GeneralMessage SaveVacation(Vacation vacation, int UserId, int BranchId, string Lang, string Url, string ImgUrl);
        GeneralMessage SaveVacation_Management(Vacation vacation, int UserId, int BranchId, string Lang, string Url, string ImgUrl);
        GeneralMessage SaveVacation2(Vacation vacation, int UserId, int BranchId, string Lang, string Url, string ImgUrl);

        GeneralMessage SaveVacationWorkers(Vacation vacation, int UserId, int BranchId, string Lang, string Url, string ImgUrl);
        GeneralMessage CheckIfHaveTasks(int VacationId, string Lang);
        //GeneralMessage UpdateVacation(int vacationId, int UserId, int BranchId, string Lang,int Type, string Con);
        //GeneralMessage UpdateVacation(int vacationId, int UserId, int BranchId, string Lang, int Type, string Con, string Url, string ImgUrl);
        GeneralMessage UpdateVacation(int vacationId, int UserId, int BranchId, string Lang, int Type, string Con, string Url, string ImgUrl, string? reason);
        GeneralMessage UpdateDecisionType_V(int vacationId, int UserId, int BranchId, string Lang, int DecisionType);
        GeneralMessage UpdateBackToWork_V(int vacationId, int UserId, int BranchId);
        GeneralMessage DeleteVacation(int VacationId, int UserId, int BranchId);
        IEnumerable<rptGetAboutToStartVacationsVM> GetVacationsAboutToStart(string Con);
        List<string> GetVacationDays_WithoutHolidays(string StartDate, string EndDate, int EmpId, string Lang, string Con, int vacationtypeid);
        //GeneralMessage CheckLoan(int vacationId, int UserId, int BranchId, string Lang, int Type, string Con);
       // GeneralMessage CheckLoan(int vacationId, int UserId, int BranchId, string Lang, int Type, string Con, string Url, string ImgUrl);
        GeneralMessage CheckLoan(int vacationId, int UserId, int BranchId, string Lang, int Type, string Con, string Url, string ImgUrl, string? reason);
        Task<IEnumerable<VacationVM>> GetAllVacationsw2(string SearchText, int status);
        GeneralMessage SaveVacationImage(int vacationid, int UserId, int BranchId, int? yearid, string FileName, string FileUrl);
    }
}
