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
    public class AttTimeDetailsRepository : IAttTimeDetailsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AttTimeDetailsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails(string SearchText,int AttTimeId)
        {
            var AttTimeDetails = _TaamerProContext.AttTimeDetails.Where(s => s.IsDeleted == false && s.AttTimeId == AttTimeId).Select(x => new AttTimeDetailsVM
            {
                TimeDetailsId = x.TimeDetailsId,
                AttTimeId = x.AttTimeId,
                Day = x.Day,
                DayName = x.Day == 1 ? "السبت" : x.Day ==2 ? "الاحد" : x.Day ==3 ? "الاتنين" : x.Day == 4 ? "الثلاثاء" : x.Day == 5 ? "الاربعاء" : x.Day==6 ? " الخميس" : "الجمعه",
                DayDate = x.DayDate,
                _1StFromHour = x._1StFromHour,
                _1StToHour = x._1StToHour,
                _2ndFromHour = x._2ndFromHour,
                _2ndToHour = x._2ndToHour,
                IsWeekDay = x.IsWeekDay,
                BranchId = x.BranchId,
            });
            if (SearchText != "")
            {
              //  AttTimeDetails = AttTimeDetails.Where(s => s.Day.Contains(SearchText.Trim()));
            }
            

            return AttTimeDetails;
        }

        public async Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails2(int AttTimeId)
        {
            var AttTimeDetails = _TaamerProContext.AttTimeDetails.Where(s => s.IsDeleted == false && s.AttTimeId == AttTimeId).Select(x => new AttTimeDetailsVM
            {
                TimeDetailsId = x.TimeDetailsId,
                AttTimeId = x.AttTimeId,
                Day = x.Day,
                DayName = x.Day == 1 ? "السبت" : x.Day == 2 ? "الاحد" : x.Day == 3 ? "الاتنين" : x.Day == 4 ? "الثلاثاء" : x.Day == 5 ? "الاربعاء" : x.Day == 6 ? " الخميس" : "الجمعه",
                DayDate = x.DayDate,
                _1StFromHour = x._1StFromHour,
                _1StToHour = x._1StToHour,
                _2ndFromHour = x._2ndFromHour,
                _2ndToHour = x._2ndToHour,
                IsWeekDay = x.IsWeekDay,
                BranchId = x.BranchId,
            }).ToList();
          


            return AttTimeDetails;
        }

        public async Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails2bybranchid(int branchid)
        {
            var AttTimeDetails = _TaamerProContext.AttTimeDetails.Where(s => s.IsDeleted == false && s.BranchId == branchid).Select(x => new AttTimeDetailsVM
            {
                TimeDetailsId = x.TimeDetailsId,
                AttTimeId = x.AttTimeId,
                Day = x.Day,
                DayName = x.Day == 1 ? "السبت" : x.Day == 2 ? "الاحد" : x.Day == 3 ? "الاتنين" : x.Day == 4 ? "الثلاثاء" : x.Day == 5 ? "الاربعاء" : x.Day == 6 ? " الخميس" : "الجمعه",
                DayDate = x.DayDate,
                _1StFromHour = x._1StFromHour,
                _1StToHour = x._1StToHour,
                _2ndFromHour = x._2ndFromHour,
                _2ndToHour = x._2ndToHour,
                IsWeekDay = x.IsWeekDay,
                BranchId = x.BranchId,
            }).ToList();



            return AttTimeDetails;
        }

        public async Task<IEnumerable<AttTimeDetailsVM>> CheckUserPerDawamUserExist(int UserId, string TimeFrom, string TimeTo, int DayNo, int AttTimeId)
        {
            var AttTimeDetails = _TaamerProContext.AttTimeDetails.Where(s => s.IsDeleted == false && s.AttTimeId == AttTimeId && s.Day== DayNo).Select(x => new AttTimeDetailsVM
            {
                TimeDetailsId = x.TimeDetailsId,
                AttTimeId = x.AttTimeId,
                Day = x.Day,
                DayName = x.Day == 1 ? "السبت" : x.Day == 2 ? "الاحد" : x.Day == 3 ? "الاتنين" : x.Day == 4 ? "الثلاثاء" : x.Day == 5 ? "الاربعاء" : x.Day == 6 ? " الخميس" : "الجمعه",
                DayDate = x.DayDate,
                _1StFromHour = x._1StFromHour,
                _1StToHour = x._1StToHour,
                _2ndFromHour = x._2ndFromHour,
                _2ndToHour = x._2ndToHour,
                IsWeekDay = x.IsWeekDay,
                BranchId = x.BranchId,
            }).ToList();



            return AttTimeDetails;
        }

        public async Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails()
        {
            var AttTimeDetails = _TaamerProContext.AttTimeDetails.Where(s => s.IsDeleted == false).Select(x => new AttTimeDetailsVM
            {
                TimeDetailsId = x.TimeDetailsId,
                AttTimeId = x.AttTimeId,
                Day = x.Day,
                DayName = x.Day == 1 ? "السبت" : x.Day == 2 ? "الاحد" : x.Day == 3 ? "الاتنين" : x.Day == 4 ? "الثلاثاء" : x.Day == 5 ? "الاربعاء" : x.Day == 6 ? " الخميس" : "الجمعه",
                DayDate = x.DayDate,
                _1StFromHour = x._1StFromHour,
                _1StToHour = x._1StToHour,
                _2ndFromHour = x._2ndFromHour,
                _2ndToHour = x._2ndToHour,
                IsWeekDay = x.IsWeekDay,
                BranchId = x.BranchId,
            });
            return AttTimeDetails;
        }


        public async Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetailsByid(int AttTimeId)
        {
            var AttTimeDetails = _TaamerProContext.AttTimeDetails.Where(s => s.IsDeleted == false && s.AttTimeId== AttTimeId).Select(x => new AttTimeDetailsVM
            {
                TimeDetailsId = x.TimeDetailsId,
                AttTimeId = x.AttTimeId,
                Day = x.Day,
                DayName = x.Day == 1 ? "السبت" : x.Day == 2 ? "الاحد" : x.Day == 3 ? "الاتنين" : x.Day == 4 ? "الثلاثاء" : x.Day == 5 ? "الاربعاء" : x.Day == 6 ? " الخميس" : "الجمعه",
                DayDate = x.DayDate,
                _1StFromHour = x._1StFromHour,
                _1StToHour = x._1StToHour,
                _2ndFromHour = x._2ndFromHour,
                _2ndToHour = x._2ndToHour,
                IsWeekDay = x.IsWeekDay,
                BranchId = x.BranchId,
            });
            return AttTimeDetails;
        }
    }
}
