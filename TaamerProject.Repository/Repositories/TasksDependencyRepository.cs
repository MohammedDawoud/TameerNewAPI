using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace TaamerProject.Repository.Repositories
{
    public class TasksDependencyRepository : ITasksDependencyRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public TasksDependencyRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<TasksDependencyVM>> GetAllTasksDependencies(int BranchId)
        {
            var Dependencies = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new TasksDependencyVM
            {
                DependencyId = x.DependencyId,
                PredecessorId = x.PredecessorId,
                SuccessorId = x.SuccessorId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                BranchId = x.BranchId
            }).ToList();
            return Dependencies;
        }
        public async Task<IEnumerable<TasksDependencyVM>> GetAllDependencyByProjectId(int ProjectId)
        {
            var Dependencies = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && (s.PredecessorId != 0 && s.SuccessorId != 0)).Select(x => new TasksDependencyVM
            {
                DependencyId = x.DependencyId,
                PredecessorId = x.PredecessorId,
                SuccessorId = x.SuccessorId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                BranchId = x.BranchId
            }).ToList();
            return Dependencies;
        }
        public async Task<IEnumerable<TasksDependencyVM>> GetAllDependencyByProjectIdNew(int ProjectId)
        {
            var Dependencies = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && (s.PredecessorId != 0 && s.SuccessorId != 0)).Select(x => new TasksDependencyVM
            {
                DependencyId = x.DependencyId,
                PredecessorId = x.PredecessorId,
                SuccessorId = x.SuccessorId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                BranchId = x.BranchId,
                PredecessorIdindex = x.SettingsPredecessor != null ? x.SettingsPredecessor.taskindex : 0,
                SuccessorIdindex = x.SettingsSuccessor != null ? x.SettingsSuccessor.taskindex : 0,
            }).ToList();
            return Dependencies;
        }

        public async Task<TasksDependencyVM> GetDependencyByProjSubTypeId(int ProjectId)
        {
            var Dependencies = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new TasksDependencyVM
            {
                DependencyId = x.DependencyId,
                PredecessorId = x.PredecessorId,
                SuccessorId = x.SuccessorId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                BranchId = x.BranchId
            }).FirstOrDefault();
            return Dependencies;
        }

        public IEnumerable<TasksDependency> GetAll()
        {
            return _TaamerProContext.TasksDependency.ToList<TasksDependency>();

        }

        public TasksDependency GetById(int Id)
        {
           return _TaamerProContext.TasksDependency.Where(x => x.DependencyId == Id).FirstOrDefault();
        }

        public IEnumerable<TasksDependency> GetMatching(Func<TasksDependency, bool> where)
        {
            return _TaamerProContext.TasksDependency.Where(where).ToList<TasksDependency>();
        }
    }
}
