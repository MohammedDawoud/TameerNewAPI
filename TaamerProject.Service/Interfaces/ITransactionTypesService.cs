using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface ITransactionTypesService  
    {
        Task<IEnumerable<TransactionTypesVM>> GetAllTransactionTypes(string SearchText);
        GeneralMessage SaveTransactionTypes(TransactionTypes transactionTypes, int UserId, int BranchId);
        GeneralMessage DeleteTransactionTypes(int TransactionTypesId, int UserId, int BranchId);
    }
}
