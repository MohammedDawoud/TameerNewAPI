using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class AccountTreeVM
    {
        public string? id { get; set; }
        public string? parent { get; set; }
        public string? text { get; set; }
        public AccountTreeVM(string? ID, string? Parent, string? Text)
        {
            id = ID;
            parent = Parent;
            text = Text;
        }
    }
    public class TasksVM
    {
        public string? id { get; set; }
        public string? parent { get; set; }
        public string? name { get; set; }
        public string? phaseTaskId { get; set; }
        public bool? plusTime { get; set; }
        public int? isConverted { get; set; }

        public List<TasksVM>? children { get; set; }


        //public TasksVM()
        //{
           
        //}
        public TasksVM(string? ID, string? Parent, string? Text, string? phaseid, bool? PlusTime, int? IsConverted)
        {
            id = ID;
            parent = Parent;
            name = Text;
            phaseTaskId = phaseid;
            plusTime = PlusTime ?? false;
            isConverted = IsConverted ?? 0;

        }
        public TasksVM(string? ID, string? Parent, string? Text, List<TasksVM>? Children, string? phaseid, bool? PlusTime, int? IsConverted)
        {
            id = ID;
            parent = Parent;
            name = Text;
            children = Children;
            phaseTaskId = phaseid;
            plusTime = PlusTime??false;
            isConverted = IsConverted??0;
        }

    }

    public class TasksLoadVM
    {
        public string? id { get; set; }
        public string? parent { get; set; }
        public string? name { get; set; }
        public string? phaseTaskId { get; set; }
        public bool? plusTime { get; set; }
        public int? isConverted { get; set; }

        public List<ChildrenVM>? children { get; set; }


        public TasksLoadVM()
        {

        }
        public TasksLoadVM(string? ID, string? Parent, string? Text, string? phaseid,bool? PlusTime,int? IsConverted)
        {
            id = ID;
            parent = Parent;
            name = Text;
            phaseTaskId = phaseid;
            plusTime = PlusTime??false;
            isConverted = IsConverted??0;

        }
        public TasksLoadVM(string? ID, string? Parent, string? Text, List<ChildrenVM>? Children)
        {
            id = ID;
            parent = Parent;
            name = Text;
            children = Children;
        }

    }

    public class ChildrenVM
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public itemVM? item { get; set; }
        public bool? plusTime { get; set; }
        public int? isConverted { get; set; }
    }
    public class itemVM
    {
        public string? phaseid { get; set; }
        public string? phrase { get; set; }
        public string? phaseTaskId { get; set; }
        public bool? plusTime { get; set; }
        public int? isConverted { get; set; }

    }

    public class TrainBalanceVM
    {

        public int AccountId { get; set; }
        public string? AccCode { get; set; }

        public string? Acc_NameAr { get; set; }

        public string? OpDipet { get; set; }

        public string?  OpCredit { get; set; }
        public string? AhDipet { get; set; }

        public string? AhCredit { get; set; }

        public string? CreditTotal { get; set; }

        public string? DebitTotal { get; set; }
        public string? NetCreditTotal { get; set; }

        public string? NetDebitTotal { get; set; }

        public string? AccNature { get; set; }

        public string? TotalDebitEnd { get; set; }

        public string? TotalCriditEnd { get; set; }
        public string? TotalFinal { get; set; }


        public string? AccID { get; set; }
        //public string? ParentID { get; set; }
        public string? Level { get; set; }
        public int? LineNumber { get; set; }
        public int? Classification { get; set; }
        public int? AccountIdAhlak { get; set; }
        public int? ParentId { get; set; }


    }

    public class DetailsMonitorVM
    {

        public int TransactionId { get; set; }
        public string? TransactionDate { get; set; }

        public int InvoiceId { get; set; }

        public int Type { get; set; }

        public int AccountId { get; set; }
        public string? CostCenterId { get; set; }


        public string? credit { get; set; }

        public string? depit { get; set; }
        public string? details { get; set; }

        public string? notes { get; set; }

        public int branchid { get; set; }

        public string? TypeName { get; set; }

        public string? ProjectNo { get; set; }

        public string? customername { get; set; }
        public string? ServicesName { get; set; }
        public string? AccountName { get; set; }
        public string? BranchName { get; set; }
        public int Cus_Emp_Sup { get; set; }
        public string? AccountCodeNew { get; set; }
        public string? suppliername { get; set; }
        public string? employeename { get; set; }



    }


    public class CostCenterExpRevVM
    {
        public string? CashIncome { get; set; }
        public string? BankIncome { get; set; }
        public string? TotalIncome { get; set; }
        public string? OperationalExpenses { get; set; }
        public string? GeneralExpenses { get; set; }
        public string? TotalExpenses { get; set; }

    }

    public class IncomeStatmentVM
    {

        public string? ID { get; set; }
        public string? CatName { get; set; }

        public string? IncomePartual { get; set; }

        public string? IncomeTotal { get; set; }

    }
    public class IncomeStatmentVMWithLevels
    {

        public int ID { get; set; }
        public string? AccountID { get; set; }

        public string? AccountCode { get; set; }
        public string? AccountCodeNew { get; set; }
        public string? AccountNameCode { get; set; }

        public string? AccountName { get; set; }
        public string? AccountLevel { get; set; }        
        public List<string?> TotalResult { get; set; }


    }

    public class GeneralBudgetVM
    {

        public string? AccCode { get; set; }

        public string? GBName { get; set; }

        public string? GBPartual { get; set; }

        public string? GBTotal { get; set; }
        public string? GBBalance { get; set; }
        public string? acclvl { get; set; }
        public string? parid { get; set; }
        public string? isfixed { get; set; }




    }
    public class GeneralmanagerRevVM
    {

        public string? InvId { get; set; }

        public string? ProjNum { get; set; }

        public string? Date { get; set; }

        public string? amount { get; set; }
        public string? Taxes { get; set; }
    }


    public class AccountStatmentVM
    {

        public string? TransID { get; set; }

        public string? CDate { get; set; }

        public string? Hdate { get; set; }

        public string? Description{ get; set; }

        public string? Debit { get; set; }

        public string? Credit { get; set; }

        public string? CostCenter { get; set; }

        public string? Balance { get; set; }

    }

    public class GetTotalExpRevByCCVM
    {
        public string? TotalExp { get; set; }
        public string? TotalRev { get; set; }
     
    }

      
    //public class InvoiceReturnDataALL
    //{
    //    public int AccountId { get; set; }
    //    public decimal Total { get; set; }
    //    public decimal Credit { get; set; }
    //    public decimal Depit { get; set; }
    //}


}
