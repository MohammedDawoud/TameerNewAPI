using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class EmpLocationsRepository : IEmpLocationsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public EmpLocationsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<EmpLocationsVM>> GetAllLocationsByEmpId(string Lang, int empId)
        {
            var locations = _TaamerProContext.EmpLocations.Where(s => s.EmpId == empId).Select(s => new EmpLocationsVM
            {
                EmpLocationId = s.EmpLocationId,
                EmpId = s.EmpId,
                LocationId = s.LocationId,
            }).ToList();
            return locations;

        }
        public async Task<IEnumerable<EmpLocationsVM>> GetLocationByLocationId(string Lang, int locationId)
        {
            var locations = _TaamerProContext.EmpLocations.Where(s => s.LocationId == locationId && s.IsDeleted == false).Select(s => new EmpLocationsVM
            {
                EmpLocationId = s.EmpLocationId,
                EmpId = s.EmpId,
                LocationId = s.LocationId,
                EmployeeName = s.Employee != null ? (Lang == "ltr" ? s.Employee.EmployeeNameEn : s.Employee.EmployeeNameAr):"",

                DepartmentName = s.Employee != null ? s.Employee.Department != null ? s.Employee.Department.DepartmentNameAr : "":"",
                JobName = s.Employee != null ? s.Employee.Job != null ? s.Employee.Job.JobNameAr : "":"",
                AttendenceLocationName = s.AttendenceLocation != null ? s.AttendenceLocation.Name : "",
                allowoutsidesite = s.Employee != null ? s.Employee.allowoutsidesite ?? false : false,
                allowallsite = s.Employee != null ? s.Employee.allowallsite ?? false:false,


            });
            return locations;
        }
    }
}
