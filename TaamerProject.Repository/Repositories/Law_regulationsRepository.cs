using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class Law_regulationsRepository : ILaw_regulationsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Law_regulationsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<List<Emp_AbsenceListVM>> GetAbsenceLate()
        {
            var _AbsenceList = _TaamerProContext.AbsenceLists.Where(s => s.IsDeleted == false).Select(x => new Emp_AbsenceListVM
            {
                ID = x.ID,
                AbsenceTime = x.AbsenceTime,
                First = x.First,
                Second = x.Second,
                Third = x.Third,
                Fourth = x.Fourth,
            }).ToList();
            return _AbsenceList;
        }

        public async Task<List<Emp_LateListVM>> GetLateLaw()
        {
            var _LateList = _TaamerProContext.LateLists.Where(s => s.IsDeleted == false).Select(x => new Emp_LateListVM
            {
                ID = x.ID,
                LateTime = x.LateTime,
                First = x.First,
                Second = x.Second,
                Third = x.Third,
                Fourth = x.Fourth,
            }).ToList();
            return _LateList;
        }

        public Law_Regulations Getlaw_Regulations()
        {
            Law_Regulations law =new Law_Regulations();
            law.emp_LateLists = _TaamerProContext.LateLists.Where(s => s.IsDeleted == false).Select(x => new Emp_LateListVM
            {
                ID = x.ID,
                LateTime = x.LateTime,
                First = x.First,
                Second = x.Second,
                Third = x.Third,
                Fourth = x.Fourth,
            }).ToList();
            law.emp_AbsenceLists = _TaamerProContext.AbsenceLists.Where(s => s.IsDeleted == false).Select(x => new Emp_AbsenceListVM
            {
                ID = x.ID,
                AbsenceTime = x.AbsenceTime,
                First = x.First,
                Second = x.Second,
                Third = x.Third,
                Fourth = x.Fourth,
            }).ToList();
            return law;

        }

    }
}
