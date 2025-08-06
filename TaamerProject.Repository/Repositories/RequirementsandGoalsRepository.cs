using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class RequirementsandGoalsRepository : IRequirementsandGoalsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public RequirementsandGoalsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task< IEnumerable<RequirementsandGoalsVM>> GetAllrequirmentbyProjecttype(string Lang, int projecttypeid)
        {

            var requirments = _TaamerProContext.RequirementsandGoals.Where(s => s.IsDeleted == false && s.ProjectTypeId == projecttypeid).Select(x => new RequirementsandGoalsVM
            {
                ProjectId = x.ProjectId,
                RequirementId=x.RequirementId,
                RequirmentName=x.RequirmentName,
                DepartmentId=x.DepartmentId,
                ProjectTypeId=x.ProjectTypeId,
                EmployeeId=x.EmployeeId,
                LineNumber=x.LineNumber,
                TimeNo=x.TimeNo??"0",
                TimeType=x.TimeType??"1",
                TimeTypeName = x.TimeType == "1" ?  " يوم " :
                          x.TimeType == "2" ?  " اسبوع " :
                          x.TimeType == "3" ? " شهر " : x.TimeType == "4" ?  " ساعه " : "",

                timestr = x.TimeType == "1" ? (x.TimeNo) + " يوم " :
                          x.TimeType == "2" ? (x.TimeNo) + " اسبوع " :
                          x.TimeType == "3" ? (x.TimeNo) + " شهر " : x.TimeType == "4" ? (x.TimeNo) + " ساعه " : "",

                EmployeeName = x.Employees!=null?x.Employees.EmployeeNameAr:"",
                DepartmentName = x.Department != null ? x.Department.DepartmentNameAr : "",
                Name = x.Employees != null ? x.Employees.EmployeeNameAr : x.Department != null ? x.Department.DepartmentNameAr : "",


            }).ToList().OrderBy(x=>x.LineNumber);
          
            return requirments;
        }



    }
}
