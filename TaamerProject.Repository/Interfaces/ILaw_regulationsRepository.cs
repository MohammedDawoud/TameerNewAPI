using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Repository.Interfaces
{
    public interface ILaw_regulationsRepository
    {
        Task<List<Emp_LateListVM>> GetLateLaw();
        Task<List<Emp_AbsenceListVM>> GetAbsenceLate();
        Law_Regulations Getlaw_Regulations();

    }
}
