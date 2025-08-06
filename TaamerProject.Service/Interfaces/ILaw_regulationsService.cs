using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Service.Interfaces
{
    public interface ILaw_regulationsService
    {
        Task<List<Emp_LateListVM>> GetLateLaw();
        Task<List<Emp_AbsenceListVM>> GetAbsenceLate();
        Law_Regulations GetLaw_Regulations();

        GeneralMessage saveAbsenceLaw(List<Emp_AbsenceList> Absencelist, int UserId, int BranchId);
        GeneralMessage saveLateLaw(List<Emp_LateList> latelist, int UserId, int BranchId);

        GeneralMessage saveAbsenceLaw(Emp_AbsenceList absence, int UserId, int BranchId);
        GeneralMessage saveLateLaw(Emp_LateList absence, int UserId, int BranchId);

    }
}
