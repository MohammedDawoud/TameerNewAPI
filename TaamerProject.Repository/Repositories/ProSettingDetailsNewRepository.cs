
using System;
using System.Collections;
using System.Collections.Generic;
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
    public class ProSettingDetailsNewRepository : IProSettingDetailsNewRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProSettingDetailsNewRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<ProSettingDetailsNewVM> CheckProSettingData(int ProjectTypeId, int ProjectSubTypeId, int BranchId) 
        {
            var result = _TaamerProContext.ProSettingDetailsNew.Where(s => s.IsDeleted == false && s.ProjectSubtypeId == ProjectSubTypeId && s.ProjectTypeId == ProjectTypeId).Select(x => new ProSettingDetailsNewVM
            {
                ProSettingId = x.ProSettingId,
                ProjectSubtypeId = x.ProjectSubtypeId,
                ProjectTypeId = x.ProjectTypeId,
                ProSettingNo = x.ProSettingNo,
                ProSettingNote = x.ProSettingNote,
                ProjectTypeName = x.ProjectType.NameAr,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr
            }).FirstOrDefault();
            return result;
        }
        public async Task<ProSettingDetailsNewVM> CheckProSettingData2( int? ProjectSubTypeId, int BranchId) 
        {
            var result = _TaamerProContext.ProSettingDetailsNew.Where(s => s.IsDeleted == false && s.ProjectSubtypeId == ProjectSubTypeId).Select(x => new ProSettingDetailsNewVM
            {
                ProSettingId = x.ProSettingId,
                ProjectSubtypeId = x.ProjectSubtypeId,
                ProjectTypeId = x.ProjectTypeId,
                ProSettingNo = x.ProSettingNo,
                ProSettingNote = x.ProSettingNote,
                ProjectTypeName = x.ProjectType.NameAr,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr
            }).FirstOrDefault();
            return result;
        }

        public async Task<int> CheckProSettingNo(ProSettingDetailsNew proSettingDetails)
        {
            if (_TaamerProContext.ProSettingDetails != null)
            {
                var lastRow = _TaamerProContext.ProSettingDetailsNew.Where(s => s.IsDeleted == false && s.ProSettingNo == proSettingDetails.ProSettingNo && s.ProSettingId != proSettingDetails.ProSettingId).Count();
                return lastRow;
            }
            else
            {
                return 0;
            }
        }
        public async Task<IEnumerable<ProSettingDetailsNewVM>> FillProSettingNo()
        {
            try {
            var ProSettingNo = _TaamerProContext.ProSettingDetailsNew.Where(s => s.IsDeleted == false).Select(x => new ProSettingDetailsNewVM
            {
                ProSettingId = x.ProSettingId,
                ProjectSubtypeId = x.ProjectSubtypeId,
                ProjectTypeId = x.ProjectTypeId,
                ProSettingNo = x.ProSettingNo,
                ProSettingNote = x.ProSettingNote,
                ProjectTypeName = x.ProjectType == null?"": x.ProjectType.NameAr !=null? x.ProjectType.NameAr:"",
                ProjectSubTypeName = x.ProjectSubTypes == null ? "" : x.ProjectSubTypes.NameAr,
                UserName= x.Users == null ? "" : x.Users.FullName??"",
               // AddDate= DateTime.ParseExact(x.AddDate.ToString(), "MM-dd-yyyy", CultureInfo.InvariantCulture).ToString()
               AddDate =x.AddDate,
               ExpectedTime= x.ProjectSubTypes == null ? "" : x.ProjectSubTypes.TimePeriod, //x.ProjectSubTypes.TimePeriod==null?"": (Convert.ToInt32(x.ProjectSubTypes.TimePeriod) < 30) ? x.ProjectSubTypes.TimePeriod +"يوم" : (Convert.ToInt32(x.ProjectSubTypes.TimePeriod) == 30) ? "شهر" :Convert.ToInt32(Convert.ToInt32(x.ProjectSubTypes.TimePeriod) / 30) +"شهر" + Convert.ToInt32(Convert.ToInt32(x.ProjectSubTypes.TimePeriod) % 30) + "يوم",

            }).ToList();
            return ProSettingNo;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public async Task<ProSettingDetailsNewVM> GetProSettingById(int ProSettingId)
        {
            var ProSettingNo = _TaamerProContext.ProSettingDetailsNew.Where(s => s.IsDeleted == false && s.ProSettingId == ProSettingId).Select(x => new ProSettingDetailsNewVM
            {
                ProSettingId = x.ProSettingId,
                ProjectSubtypeId = x.ProjectSubtypeId,
                ProjectTypeId = x.ProjectTypeId,
                ProSettingNo = x.ProSettingNo,
                ProSettingNote = x.ProSettingNote,
                ProjectTypeName = x.ProjectType.NameAr,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr
            }).FirstOrDefault();
            return ProSettingNo;
        }
        public async Task< int> GenerateNextProSettingNumber()
        {
            if (_TaamerProContext.ProSettingDetails != null)
            {
                var lastRow = _TaamerProContext.ProSettingDetailsNew.Where(s => s.IsDeleted == false).OrderByDescending(u => u.ProSettingId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {
                        var ProjectNumber = int.Parse(lastRow.ProSettingNo) + 1;
                        return ProjectNumber;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
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
