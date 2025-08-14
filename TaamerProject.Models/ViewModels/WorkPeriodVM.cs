using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerP.Models.ViewModels
{
    public class WorkPeriodVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Years { get; set; }
        public int Months { get; set; }
        public int Days { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Allowances { get; set; }
        public int WorkingDaysPerWeek { get; set; }
        public int EndType { get; set; }
        public decimal EndOfServiceReward { get; set; }

        public decimal FirstFiveYearsReward { get; set; }
        public decimal AfterFiveYearsReward { get; set; }
    }

    public class EmployeeEndWork
    {
        // بيانات الموظف
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string Nationality { get; set; }
        public string? BirthDate { get; set; }
        public string? Age { get; set; }
        public int? IdNumber { get; set; }
        public string? IdExpiryDate { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? CurrentContractEndDate { get; set; }

        // بيانات مالية إضافية
        public decimal? LastBasicSalary { get; set; } // آخر راتب أساسي
        public decimal TotalEndOfServiceReward { get; set; } // مجموع مكافأة نهاية الخدمة
        public decimal LastMonthDueSalary { get; set; } // راتب مستحق عن الشهر الأخير
        public decimal VacationEncashment { get; set; } // بدل إجازات نقدي
        public decimal TotalDue { get; set; } // مجموع المستحقات قبل السلف
        public decimal LastMonthLoans { get; set; } // إجمالي سلف الشهر الأخير
        public decimal NetPayable { get; set; } // الصافي المستحق

        public List<WorkPeriodVM> WorkPeriods { get; set; } = new List<WorkPeriodVM>();
    }

}
