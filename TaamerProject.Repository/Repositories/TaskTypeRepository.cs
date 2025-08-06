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
    public class TaskTypeRepository : ITaskTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public TaskTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<TaskTypeVM>> GetAllTaskType(int BranchId)
        {
            var taskType = _TaamerProContext.TaskType.Where(s => s.IsDeleted == false).Select(x => new TaskTypeVM
            {
                TaskTypeId = x.TaskTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                BranchId = x.BranchId
            }).ToList();
            return taskType;
        }


        public async Task< IEnumerable<TaskTypeVM>> GetAllTaskType2(string SearchText)
        {
            if (SearchText == "")
            {
                var taskType = _TaamerProContext.TaskType.Where(s => s.IsDeleted == false).Select(x => new TaskTypeVM
                {
                    TaskTypeId = x.TaskTypeId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    BranchId = x.BranchId
                }).ToList();
                return taskType;
            }
            else

            {
                var taskType = _TaamerProContext.TaskType.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new TaskTypeVM
                {
                    TaskTypeId = x.TaskTypeId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    BranchId = x.BranchId
                }).ToList();
                return taskType;
            }
        }


    }
}


