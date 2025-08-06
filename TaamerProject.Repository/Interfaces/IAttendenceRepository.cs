using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAttendenceRepository
    {
        Attendence GetById(long AttId);
        Task<IEnumerable<AttendenceVM>> GetAllAttendence(int BranchId);
        Task<IEnumerable<AttendenceVM>> GetAllAttendence_Device(int BranchId); 
        Task<bool> IsExist(int EmpId, string Date, int Hour, int BranchId);
        Task<IEnumerable<AttendenceVM>> EmpAttendenceSearch(AttendenceVM AttendenceSearch, int BranchId);

        Task<IEnumerable<AttendenceVM>> GetAllAttendenceSearch(int BranchId);
        Task<IEnumerable<AttendenceVM>> GetAllAttendenceBySearchObject(AttendenceVM Search, int BranchId);

        Task<IEnumerable<AbsenceVM>> GetAbsenceData(string FromDate, string ToDate, int EmpId, int? YearId, int BranchId, string lang, string Con);
        Task<IEnumerable<AbsenceVM>> GetAbsenceDataToday(string TodayDate, int? YearId, int BranchId, string lang, string Con);

        Task<IEnumerable<LateVM>> GetLateData(string FromDate, string ToDate, int EmpId, int? YearId,int Shift, int BranchId, string lang, string Con);
        Task<IEnumerable<LateVM>> GetLateDataToday(string TodayDate, int? YearId, int Shift, int BranchId, string lang, string Con);

        Task<IEnumerable<LateVM>> GetEarlyDepartureData(string FromDate, string ToDate, int EmpId, int? YearId, int Shift, int BranchId, string lang, string Con);
        Task<IEnumerable<LateVM>> GetEarlyDepartureDataToday(string TodayDate, int? YearId, int Shift, int BranchId, string lang, string Con);
        Task<IEnumerable<NotLoggedOutVM>> GetNotLoggedOutData(string FromDate, string ToDate,  int? YearId,  int BranchId, string lang, string Con);

        Task<IEnumerable<LateVM>> GetAttendanceData(string FromDate, string ToDate, int? YearId, int Shift, int BranchId, string lang, string Con);
         Task<IEnumerable<LateVM>> GetAttendanceData(  string FromDate, string ToDate, int? YearId, int Shift, int BranchId, string lang, string Con, int pageNumber, int pageSize);
        Task<IEnumerable<LateVM>> GetAttendance_Screen(string FromDate, string ToDate, int? YearId, int Shift, int BranchId,int SwType, string lang, string Con, int UserIDF);
        Task<IEnumerable<Attendance_M_VM>> GetAttendance_Screen_M(int Year, int Month,  int? YearId, int Shift, int BranchId, int SwType, string lang, string Con, int UserIDF);
        Task<IEnumerable<Attendance_W_VM>> GetAttendance_Screen_W(int Year, int Month, int? YearId, int Shift, int BranchId, int SwType, string lang, string Con, int UserIDF);

        Task<IEnumerable<LateVM>> GetAttendanceData_Application(string FromDate, string ToDate, int? YearId, int Shift, int BranchId, string lang, string Con);
        Task<IEnumerable<AbsenceVM>> GetAbsenceData_withWeekEnd(string FromDate, string ToDate, int EmpId, int? YearId, int BranchId, string lang, string Con);


    }
}
