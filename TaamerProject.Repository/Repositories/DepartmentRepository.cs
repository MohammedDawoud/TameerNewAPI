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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public DepartmentRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<DepartmentVM>> GetAllDepartment(string SearchText,int BranchId)
        {
            var departments = _TaamerProContext.Department.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new DepartmentVM
            {
                DepartmentId = x.DepartmentId,
                DepartmentNameAr = x.DepartmentNameAr,
                DepartmentNameEn = x.DepartmentNameEn,
                Type = x.Type,
                BranchId=x.BranchId,
                TypeName = x.Type == 1 ? "داخلية" : "خارجية"
            }).ToList();
            if (SearchText != "")
            {
                departments = departments.Where(s => s.DepartmentNameAr.Contains(SearchText.Trim()) || s.DepartmentNameEn.Contains(SearchText.Trim())).ToList();
            }
            return departments;
        }

        public async Task<IEnumerable<DepartmentVM>> GetAllDepartmentbyTypeUser(int? Type, int BranchId, string SearchText)
        {
            var departments = _TaamerProContext.Department.Where(s => s.IsDeleted == false && s.Type == Type).Select(x => new DepartmentVM
            {
                DepartmentId = x.DepartmentId,
                DepartmentNameAr = x.DepartmentNameAr,
                DepartmentNameEn = x.DepartmentNameEn,
                Type = x.Type,
                BranchId = x.BranchId,
                TypeName = x.Type == 1 ? "داخلية" : "خارجية"
            }).ToList();
            if (SearchText != "")
            {
                departments = departments.Where(s => s.DepartmentNameAr.Contains(SearchText.Trim()) || s.DepartmentNameEn.Contains(SearchText.Trim())).ToList();
            }
            return departments;
        }

        public async Task<IEnumerable<DepartmentVM>> GetExternalDepartment()
        {
            var departments = _TaamerProContext.Department.Where(s => s.IsDeleted == false  && s.Type == 2).Select(x => new DepartmentVM
            {
                DepartmentId = x.DepartmentId,
                DepartmentNameAr = x.DepartmentNameAr,
                DepartmentNameEn = x.DepartmentNameEn,
                Type = x.Type,
                BranchId = x.BranchId,
                TypeName = x.Type == 1 ? "داخلية" : "خارجية"
            }).ToList();
           
            return departments;
        }
        public async Task<IEnumerable<DepartmentVM>> GetAllDepartmentbyType(int? Type, int BranchId, string SearchText)
        {
            var departments = _TaamerProContext.Department.Where(s => s.IsDeleted == false && (s.Type == Type || Type==0 || Type==null) && s.BranchId == BranchId).Select(x => new DepartmentVM
            {
                DepartmentId = x.DepartmentId,
                DepartmentNameAr = x.DepartmentNameAr,
                DepartmentNameEn = x.DepartmentNameEn,
                Type = x.Type,
                BranchId = x.BranchId,
                TypeName = x.Type == 1 ? "داخلية" : "خارجية"
            }).ToList();
            if (SearchText != "" && SearchText !=null)
            {
                departments = departments.Where(s => s.DepartmentNameAr.Contains(SearchText.Trim()) || s.DepartmentNameEn.Contains(SearchText.Trim())).ToList();
            }
            return departments;
        }



        public async Task<IEnumerable<DepartmentVM>> GetDepartmentbyid(int DeptId)
        {
            var departments = _TaamerProContext.Department.Where(s => s.IsDeleted == false && s.DepartmentId == DeptId).Select(x => new DepartmentVM
            {
                DepartmentId = x.DepartmentId,
                DepartmentNameAr = x.DepartmentNameAr,
                DepartmentNameEn = x.DepartmentNameEn,
                Type = x.Type,
                BranchId = x.BranchId,
                TypeName = x.Type == 1 ? "داخلية" : "خارجية"
            }).ToList();

            return departments;
        }
    }
}
