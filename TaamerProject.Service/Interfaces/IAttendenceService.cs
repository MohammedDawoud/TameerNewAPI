using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAttendenceService
    {
        Task<IEnumerable<AttendenceVM>> GetAllAttendence(int BranchId, string Con = "");
        Task<IEnumerable<AttendenceVM>> GetAllAttendenceSearch(AttendenceVM Search, int BranchId, string Con = "");
        Task<IEnumerable<AttendenceVM>> GetAllAttendence_Device(int BranchId);
        GeneralMessage SaveAttendence(Attendence attendence, int UserId, int BranchId);
        GeneralMessage SaveAttendence_N(Attendence attendence, int UserId, int BranchId);
        GeneralMessage DeleteAttendencesByDate(string DateTime);
        GeneralMessage SaveAttendence_FromDevice(Attendence attendence, int UserId, int BranchId);
        GeneralMessage SaveListAttendence(List<Attendence> attendence, int UserId, int BranchId);
        GeneralMessage ConFirmMonthAttendence(Attendence attendence, int UserId, int BranchId, string Lang);
        GeneralMessage DeleteAttendence(int AttendenceId, int UserId, int BranchId);
        Task<IEnumerable<AttendenceVM>> EmpAttendenceSearch(AttendenceVM AttendenceSearch, int BranchId);

        IEnumerable<AbsenceVM> GetAbsenceData(string FromDate, string ToDate, int EmpId, int BranchId, string lang, string Con, int? YearId);
        IEnumerable<EmployeesVM> GetAbsentEmployeesNote(int UserId, int BranchId, string Lang, string Con);
        GeneralMessage InsertAbsentEmpSee(int UserId, int BranchId);
        Task<IEnumerable<AbsenceVM>> GetAbsenceDataToday(string TodayDate, int BranchId, string lang, string Con, int? YearId);

        Task<IEnumerable<LateVM>> GetLateData(string FromDate, string ToDate, int EmpId, int Shift, int BranchId, string lang, string Con, int? YearId);

        Task<IEnumerable<LateVM>> GetLateDataToday(string TodayDate, int Shift, int BranchId, string lang, string Con, int? YearId);


        Task<IEnumerable<LateVM>> GetEarlyDepartureData(string FromDate, string ToDate, int EmpId, int Shift, int BranchId, string lang, string Con, int? YearId);

        Task<IEnumerable<LateVM>> GetEarlyDepartureDataToday(string TodayDate, int Shift, int BranchId, string lang, string Con, int? YearId);

        Task<IEnumerable<NotLoggedOutVM>> GetNotLoggedOutData(string FromDate, string ToDate, int BranchId, string lang, string Con, int? YearId);
        Task<IEnumerable<LateVM>> GetAttendanceData(string FromDate, string ToDate, int Shift, int BranchId, string lang, string Con, int? YearId);
        Task<IEnumerable<LateVM>> GetAttendanceData(string FromDate, string ToDate, int Shift, int BranchId, string lang, string Con, int? yearid, int pageNumber, int pageSize);
        Task<IEnumerable<LateVM>> GetAttendance_Screen(string FromDate, string ToDate, int Shift, int BranchId, int SwType, string lang, string Con, int? YearId, int UserIDF);
        Task<IEnumerable<Attendance_M_VM>> GetAttendance_Screen_M(int Year, int Month, int Shift, int BranchId, int SwType, string lang, string Con, int? YearId, int UserIDF);
        Task<IEnumerable<Attendance_W_VM>> GetAttendance_Screen_W(int Year, int Month, int Shift, int BranchId, int SwType, string lang, string Con, int? YearId, int UserIDF);

        Task<IEnumerable<LateVM>> GetAttendanceData_Application(string FromDate, string ToDate, int Shift, int BranchId, string lang, string Con, int? yearid);


        //  GeneralMessage SENDWhatsap();

        string GetLateData_2(string FromDate, string ToDate, int EmpId, int Shift, int BranchId, string lang, string Con, int? yearid);

    }
}
