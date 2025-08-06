using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class AttendenceVM
    {
        public long AttendenceId { get; set; }
        public int? EmpId { get; set; }
        public int? RealEmpId { get; set; }
        
        public string? Day { get; set; }
        public DateTime? CheckTime { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? CheckIn { get; set; }
        public bool? IsLate { get; set; }
        public int? LateDuration { get; set; }
        public bool? IsOverTime { get; set; }
        public string? SameDate { get; set; }
        public bool? IsDone { get; set; }
        public int? BranchId { get; set; }
        public string?  EmployeeName { get; set; }
        public string? BranchName { get; set; }
        public string? AttendenceDate { get; set; }
        public string? AttendenceHijriDate { get; set; }
        public string? CheckType { get; set; }
        public bool? IsSearch { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int? ShiftTime { get; set; }

        public string? WorkStartDate { get; set; }
        public string? EndWorkDate { get; set; }
    }



    public class AbsenceVM
    {

       

        public string? EmpNo { get; set; }
        //public int? E_RealEmpId { get; set; }
        public string? E_FullName { get; set; }

        public string? DayNOfWeek { get; set; }
        public string? Mdate { get; set; }
       
        //public string? HjriDate { get; set; }

        //public bool? E_Active { get; set; }
        //public int? E_Shift { get; set; }
       
        //public string? E_EmpIdentity { get; set; }
        //public string? E_Tel { get; set; }


        //public DateTime? E_StartYear { get; set; }
        //public int E_Gender { get; set; }
        //public string? E_Address { get; set; }
        public string? E_BranchId { get; set; }

      

    }



    public class LateVM
    {



        public string? EmpNo { get; set; }
        public string? EmpId { get; set; }

        public string? MAXSER { get; set; }

        //public int? E_RealEmpId { get; set; }
        public string? FullName { get; set; }

        public string? DawamId { get; set; }
        public string? TimeLeave1 { get; set; }
        public string? TimeJoin1 { get; set; }
        public string? TimeJoin2 { get; set; }
        public string? TimeLeave2 { get; set; }
        public string? MoveTimeIntLeave1 { get; set; }
        public string? MoveTimeStringJoin1 { get; set; }
        public string? MoveTimeIntJoin2 { get; set; }
        public string? MoveTimeStringJoin2 { get; set; }
        public string? DateDay { get; set; }

        public string? MoveTimeIntJoin1 { get; set; }
        public string? MoveTimeStringLeave1 { get; set; }
        public string? MoveTimeIntLeave2 { get; set; }
        public string? MoveTimeStringLeave2 { get; set; }
        public string? StartWorkDate { get; set; }

        public string? TimeLeave1_1 { get; set; }
        public string? TimeJoin1_1 { get; set; }
        public string? TimeJoin2_1 { get; set; }
        public string? TimeLeave2_1 { get; set; }


        //public string? HjriDate { get; set; }

        //public bool? E_Active { get; set; }
        //public int? E_Shift { get; set; }

        //public string? E_EmpIdentity { get; set; }
        //public string? E_Tel { get; set; }


        //public DateTime? E_StartYear { get; set; }
        //public int E_Gender { get; set; }
        //public string? E_Address { get; set; }
        public string? BranchId { get; set; }
        public string? PhotoUrl { get; set; }
        public string? JobName { get; set; }


        public string? Location { get; set; }

        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? FromApplication { get; set; }

        public string? Comment { get; set; }
        public int? Type { get; set; }
        public int? ShiftTime { get; set; }

        public string Late { get; set; }
        public string absence { get; set; }
        public string attend { get; set; }
        public string BranchName { get; set; }

        public List<OfficalHolidayVM>? Isholiday { get; set; }
        public List<AttTimeDetailsVM>? Isworkday { get; set; }

        public string? status { get; set; }

        // New fields for discounts
        public decimal? Discount1 { get; set; }
        public decimal? Discount2 { get; set; }
        public decimal? EmpSalary { get; set; }
    }


    public class Attendance_M_VM
    {



        public string? EmpNo { get; set; }
        public string? EmpId { get; set; }

        public string? FullName { get; set; }
        public string? DawamId { get; set; }

        public string? StartWorkDate { get; set; }
        public int? M_1 { get; set; }
        public int? M_2 { get; set; }
        public int? M_3 { get; set; }
        public int? M_4 { get; set; }
        public int? M_5 { get; set; }
        public int? M_6 { get; set; }
        public int? M_7 { get; set; }
        public int? M_8 { get; set; }
        public int? M_9 { get; set; }
        public int? M_10 { get; set; }
        public int? M_11 { get; set; }
        public int? M_12 { get; set; }
        public int? M_13 { get; set; }
        public int? M_14 { get; set; }
        public int? M_15 { get; set; }
        public int? M_16 { get; set; }
        public int? M_17 { get; set; }
        public int? M_18 { get; set; }
        public int? M_19 { get; set; }
        public int? M_20 { get; set; }
        public int? M_21 { get; set; }
        public int? M_22 { get; set; }
        public int? M_23 { get; set; }
        public int? M_24 { get; set; }
        public int? M_25 { get; set; }
        public int? M_26 { get; set; }
        public int? M_27 { get; set; }
        public int? M_28 { get; set; }
        public int? M_29 { get; set; }
        public int? M_30 { get; set; }
        public int? M_31 { get; set; }
        public int? M_Total { get; set; }
        public List<string>? M_status { get; set; }

    }

    public class Attendance_W_VM
    {



        public string? EmpNo { get; set; }
        public string? EmpId { get; set; }

        public string? FullName { get; set; }
        public string? DawamId { get; set; }
        public string? StartWorkDate { get; set; }
        public int? M_1 { get; set; }
        public int? M_2 { get; set; }
        public int? M_3 { get; set; }
        public int? M_4 { get; set; }
        public int? M_5 { get; set; }
        public int? M_6 { get; set; }
        public int? M_7 { get; set; }
        public List<string>? M_status { get; set; }

    }


    public class NotLoggedOutVM
    {



        public string? EmpNo { get; set; }      
        public string? FullName { get; set; }
        public string? BranchName { get; set; }        
        public string? CheckTime { get; set; }
        public int? Day { get; set; }
        public string? DayName { get; set; }

    }

    }
