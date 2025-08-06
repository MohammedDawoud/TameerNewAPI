using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.Common
{
    public enum ProjectStatus
    {
        New = 0,
        Approved = 1,
        Cancelled = 2,
    }
    

    public enum TaskStatus
    {
        New = 0,
        InProgress = 1,
        Stopped = 2,
        Finished = 3,
        Cancelled = 4,
        Dropped = 5,
        Transfered = 6,
    };
    public enum TaskAssignmentStatus
    {
        New = 0,
        InProgress = 1,
        Stopped = 2,
        Finished = 3,
        Cancelled = 4,
        Dropped = 5,
        Transfered = 6,
    };
    public enum AccountNature
    {
        Depit = 1,
        Credit = 2 ,
    }
    public enum AccountClassifyTypes
    {
        Box = 1,
        Customer = 2,
        Supplier = 3,
        Purchases = 4,
        PurchasesReturns = 5,
        Bank = 6,
        Employee = 7 ,
        Otehers = 8, 
        Tax = 9,
        Sales = 10 ,
        SalesReturns = 11 ,
        ProfitablePofits = 12,
        FirstTimeGoods = 13,
        GeneralCustomer = 14,
        Assets = 15,
        Liabilities = 16,
        TaxIn = 17,
        TaxOut = 18,
        CostS = 19,
        CostE = 20,
        Rights = 21,

    }
    public enum AccountTypes
    {
        None = 1,
        Budget = 2,
        IncomeStatment = 3,
        Commerce = 4,
        ProfitLoss = 5,
    };
}
