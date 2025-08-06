using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Service.IGeneric
{
    public interface ISystemAction
    {
        public void SaveAction(string FunctionName, string ServiceName, int ActionType, string MessageName,
            string ModuleName, string PageName, string ActionDate, int UserId, int BranchId, string Note, int Success);
        public void SaveTaskOperations(Pro_TaskOperations TaskOperations, int UserId, int BranchId);

    }
}
