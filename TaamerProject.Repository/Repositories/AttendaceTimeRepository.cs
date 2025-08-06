using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class AttendaceTimeRepository :  IAttendaceTimeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AttendaceTimeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<AttendaceTimeVM>> GetAllAttendaceTime()
        {
            var AttendaceTime = _TaamerProContext.AttendaceTime.Where(s => s.IsDeleted == false).Select(x => new AttendaceTimeVM
            {
                TimeId = x.TimeId,
                Code = x.Code,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Notes = x.Notes,
                UserId = x.UserId,
                BranchId = x.BranchId,
            });
            return AttendaceTime;
        }
        public async Task<int> GenerateNextAttendaceTimeNumber(int BranchId)
        {
            if (_TaamerProContext.AttendaceTime != null)
            {
                var lastRow = _TaamerProContext.AttendaceTime.Where(s => s.IsDeleted == false&& s.BranchId == BranchId).OrderByDescending(u => u.AddDate).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    return int.Parse(lastRow.Code) + 1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
    }
}
