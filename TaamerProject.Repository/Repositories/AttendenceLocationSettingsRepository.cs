using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class AttendenceLocationSettingsRepository : IAttendenceLocationSettingsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AttendenceLocationSettingsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<AttendenceLocationSettingsVM>> GetAllAttendencelocations(string SearchText)
        {
            try
            {
                var locations = _TaamerProContext.AttendenceLocations.Where(s => s.IsDeleted == false).Select(x => new AttendenceLocationSettingsVM
                {
                    AttendenceLocationSettingsId = x.AttendenceLocationSettingsId,
                    Name = x.Name,
                    Distance = x.Distance,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    xmax = x.xmax,
                    xmin = x.xmin,
                    ymax = x.ymax,
                    ymin = x.ymin,
                    //EmployeeCount = x.Employees==null?0: x.Employees.Count(),
                    EmployeeCount = x.EmpLocations == null ? 0 : x.EmpLocations.Where(s=>s.IsDeleted==false).Count(),

                });
                if (SearchText != "")
                {
                    locations = locations.Where(s => s.Name.Contains(SearchText.Trim()) || s.Name.Contains(SearchText.Trim()));
                }
                return locations;
            }catch (Exception ex)
            {
                List<AttendenceLocationSettingsVM> lem = new List<AttendenceLocationSettingsVM>();
                return lem;
            }
        }
        public async Task<AttendenceLocationSettingsVM> GetlAttendencelocationbyId(int attendenceLocationSettingsId)
        {
            try
            {
                
                var locations = _TaamerProContext.AttendenceLocations.Where(s => s.IsDeleted == false && s.AttendenceLocationSettingsId== attendenceLocationSettingsId).Select(x => new AttendenceLocationSettingsVM
                {
                    AttendenceLocationSettingsId = x.AttendenceLocationSettingsId,
                    Name = x.Name,
                    Distance = x.Distance,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    xmax = x.xmax,
                    xmin = x.xmin,
                    ymax = x.ymax,
                    ymin = x.ymin,
                    //EmployeeCount = x.Employees==null?0: x.Employees.Count(),
                    EmployeeCount = x.EmpLocations == null ? 0 : x.EmpLocations.Where(s => s.IsDeleted == false).Count(),
                }).ToList().FirstOrDefault();
                return locations;
            }
            catch (Exception ex)
            {
                AttendenceLocationSettingsVM lem = new AttendenceLocationSettingsVM();
                return lem;
            }
        }

    }
}
