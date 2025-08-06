using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using TaamerProject.Service.Generic;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class TaskTypeService :   ITaskTypeService
    {
        private readonly ITaskTypeRepository _TaskTypeRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public TaskTypeService(TaamerProjectContext dataContext
            , ISystemAction systemAction, ITaskTypeRepository taskTypeRepository)
        {
            _TaskTypeRepository = taskTypeRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<TaskTypeVM>> GetAllTaskType(int BranchId)
        {
            var taskType = _TaskTypeRepository.GetAllTaskType( BranchId);
            return taskType;
        }
        public Task<IEnumerable<TaskTypeVM>> GetAllTaskType2(string SearchText)
        {
            var taskType = _TaskTypeRepository.GetAllTaskType2(SearchText);
            return taskType;
        }
        public GeneralMessage SaveTaskType(TaskType taskType, int UserId, int BranchId)
        {
            try
            {
                if (taskType.TaskTypeId == 0)
                {
                    taskType.AddUser = UserId;
                    taskType.BranchId = BranchId;
                    taskType.AddDate = DateTime.Now;
                    _TaamerProContext.TaskType.Add(taskType);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع مهمة جديد" + taskType.NameAr;
                    _SystemAction.SaveAction("SaveTaskType", "TaskTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                   // var TaskTypeUpdated = _TaskTypeRepository.GetById(taskType.TaskTypeId);
                    TaskType? TaskTypeUpdated =  _TaamerProContext.TaskType.Where(s => s.TaskTypeId == taskType.TaskTypeId).FirstOrDefault();

                    if (TaskTypeUpdated != null)
                    {
                        TaskTypeUpdated.NameAr = taskType.NameAr;
                        TaskTypeUpdated.NameEn = taskType.NameEn;
                        TaskTypeUpdated.UpdateUser = UserId;
                        TaskTypeUpdated.UpdateDate = DateTime.Now;
                    }
                     _TaamerProContext.SaveChanges();  
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع مهمة  " + taskType.NameAr;
                    _SystemAction.SaveAction("SaveTaskType", "TaskTypeService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
               
            }
            catch (Exception)
            { 
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع المهمة";
                _SystemAction.SaveAction("SaveTaskType", "TaskTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public  GeneralMessage DeleteTaskType(int TaskTypeId, int UserId, int BranchId)
        {
            try
            {
                //TaskType taskType = _TaskTypeRepository.GetById(TaskTypeId);
                TaskType? taskType =   _TaamerProContext.TaskType.Where(s => s.TaskTypeId == TaskTypeId).FirstOrDefault();
                if (taskType != null)
                {
                    taskType.IsDeleted = true;
                    taskType.DeleteDate = DateTime.Now;
                    taskType.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                }
              
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف نوع مهمة  " + taskType.NameAr;
                _SystemAction.SaveAction("DeleteTaskType", "TaskTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف نوع مهمة رقم " + TaskTypeId; ;
                _SystemAction.SaveAction("DeleteTaskType", "TaskTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        
        public Task<IEnumerable<TaskTypeVM>> FillTaskTypeSelect(int BranchId)
        {

            var taskType = _TaskTypeRepository.GetAllTaskType(BranchId);
            return taskType;


            //return _TaskTypeRepository.GetAllTaskType(BranchId).Result.Select(s => new
            //{
            //    Id = s.TaskTypeId,
            //    Name = s.NameAr
            //});
        }
        public  Task<IEnumerable<TaskTypeVM>> FillTaskTypeSelectAE(int BranchId)
        {
            var taskType = _TaskTypeRepository.GetAllTaskType(BranchId);
            return taskType;

            //return  _TaskTypeRepository.GetAllTaskType(BranchId).Result.Select(s => new
            //{
            //    Id = s.TaskTypeId,
            //    Name = s.NameAr,
            //    NameE = s.NameEn
            //});
        }


    }
}
