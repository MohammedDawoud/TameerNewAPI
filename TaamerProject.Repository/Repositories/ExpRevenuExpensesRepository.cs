using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class ExpRevenuExpensesRepository :  IExpRevenuExpensesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ExpRevenuExpensesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<ExpRevenuExpensesVM>> GetAllExpRevenuExpenses(int BranchId)
        {
            var revenuExpenses = _TaamerProContext.ExpRevenuExpenses.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new ExpRevenuExpensesVM
            {
                ExpecteId = x.ExpecteId,
                AccountId = x.AccountId,
                Type  = x.Type,
                ToAccountId = x.ToAccountId,
                CostCenterId = x.CostCenterId,
                Amount = x.Amount,
                IsDone = x.IsDone,
                CollectionDate = x.CollectionDate,
                UserId = x.UserId,
                BranchId = x.BranchId,
                TypeName = x.Type == 1 ? "  مصروفات " : x.Type == 2 ? " ايرادات " : "" ,
                AccountName = x.Accounts.Code + " - " + x.Accounts.NameAr,
                ToAccountName = x.ToAccounts.Code+ " - " + x.ToAccounts.NameAr,
                Notes = x.Notes,
                CostCenterName =x.CostCenters.Code + " - " + x.CostCenters.NameAr,
                StatusName = x.IsDone ? "تم الانهاء" : "لم يتم الانهاء"
            }).ToList();
            return revenuExpenses;
        }
        public int GetExpensesCount(int BranchId)
        {
            return _TaamerProContext.ExpRevenuExpenses.Where(s => s.IsDeleted == false && s.Type == 1 && s.BranchId == BranchId).Count();
        }
        public int GetRevenuesCount(int BranchId)
        {
            return _TaamerProContext.ExpRevenuExpenses.Where(s => s.IsDeleted == false && s.Type == 2 && s.BranchId == BranchId).Count();
        }
         public decimal GetExpensesAmountAllBranches()
        {
            try
            {
                return _TaamerProContext.ExpRevenuExpenses.Where(s => s.IsDeleted == false && s.Type == 1).Sum(x => x.Amount);
            }
            catch(Exception)
            { return 0; }
        }
         public decimal GetRevenuesAmountAllBranches()
        {
            try
            {
                return _TaamerProContext.ExpRevenuExpenses.Where(s => s.IsDeleted == false && s.Type == 2).Sum(x => x.Amount);
            }
            catch (Exception) { return 0; }
        }
        public async Task<IEnumerable<ExpRevenuExpensesVM>> GetAllExpBysearchObject(ExpRevenuExpensesVM expsearch, int BranchId)
        {
            if (expsearch.IsChecked == false)
            {
                var Finances = _TaamerProContext.ExpRevenuExpenses.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.AccountId == expsearch.AccountId || expsearch.AccountId == null)
                && (s.Type == expsearch.Type || expsearch.Type == null)
                  ).Select(x => new ExpRevenuExpensesVM
                  {
                      AccountId = x.AccountId,
                      ExpecteId = x.ExpecteId,
                      ToAccountId = x.ToAccountId,
                      CostCenterId = x.CostCenterId,
                      Amount = x.Amount,
                      Notes = x.Notes,
                      Type = x.Type,
                      IsDone = x.IsDone,
                      CollectionDate = x.CollectionDate,
                      UserId = x.UserId,
                      BranchId = x.BranchId,
                      TypeName = x.Type == 1 ? "مصروفات" : x.Type == 2 ? "ايرادات" : "اختر النوع",
                      AccountName = x.Accounts.NameAr,
                      ToAccountName = x.ToAccounts.NameAr,
                      CostCenterName = x.CostCenters.NameAr,
                      StatusName = x.IsDone ? "تم الانهاء" : "لم يتم الانهاء",

                      //}).Select(s => new ExpRevenuExpensesVM
                      //{

                      //    AccountId = s.AccountId,
                      //    ExpecteId = s.ExpecteId,
                      //    ToAccountId = s.ToAccountId,
                      //    CostCenterId = s.CostCenterId,
                      //    Amount = s.Amount,
                      //    Notes = s.Notes,
                      //    Type = s.Type,
                      //    IsDone = s.IsDone,
                      //    CollectionDate = s.CollectionDate,
                      //    UserId = s.UserId,
                      //    BranchId = s.BranchId,
                      //    TypeName = s.TypeName,
                      //    AccountName = s.AccountName,
                      //    ToAccountName = s.ToAccountName,
                      //    CostCenterName = s.CostCenterName,
                      //    StatusName = s.StatusName,

                  }).ToList();
                return Finances;

            }
            else
            {

                var Finances1 = _TaamerProContext.ExpRevenuExpenses.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.AccountId == expsearch.AccountId || expsearch.AccountId == null)
                 && (s.Type == expsearch.Type || expsearch.Type == null)).Select(x => new ExpRevenuExpensesVM
                 {
                     AccountId = x.AccountId,
                     ExpecteId = x.ExpecteId,
                     ToAccountId = x.ToAccountId,
                     CostCenterId = x.CostCenterId,
                     Amount = x.Amount,
                     Notes = x.Notes,
                     Type = x.Type,
                     IsDone = x.IsDone,
                     CollectionDate = x.CollectionDate,
                     UserId = x.UserId,
                     BranchId = x.BranchId,
                     TypeName = x.Type == 1 ? "مصروفات" : x.Type == 2 ? "ايرادات" : "اختر النوع",
                     AccountName = x.Accounts.NameAr,
                     ToAccountName = x.ToAccounts.NameAr,
                     CostCenterName = x.CostCenters.NameAr,
                     StatusName = x.IsDone ? "تم الانهاء" : "لم يتم الانهاء",

                 }).Select(s => new ExpRevenuExpensesVM
                 {

                     AccountId = s.AccountId,
                     ExpecteId = s.ExpecteId,
                     ToAccountId = s.ToAccountId,
                     CostCenterId = s.CostCenterId,
                     Amount = s.Amount,
                     Notes = s.Notes,
                     Type = s.Type,
                     IsDone = s.IsDone,
                     CollectionDate = s.CollectionDate,
                     UserId = s.UserId,
                     BranchId = s.BranchId,
                     TypeName = s.TypeName,
                     AccountName = s.AccountName,
                     ToAccountName = s.ToAccountName,
                     CostCenterName = s.CostCenterName,
                     StatusName = s.StatusName,

                 }).ToList().Where(s => DateTime.ParseExact(expsearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                  <= DateTime.ParseExact(s.CollectionDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                  && DateTime.ParseExact(expsearch.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >=
                  DateTime.ParseExact(s.CollectionDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture));
                return Finances1;
            }




        }


        public async Task<IEnumerable<GetTotalExpRevByCCVM>> GetTotalExpRevByCC(string Con)
        {
            try
            {
                List<GetTotalExpRevByCCVM> lmd = new List<GetTotalExpRevByCCVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetTotalExpRevByCC";
                        command.Connection = con;


                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new GetTotalExpRevByCCVM
                            {
                                TotalExp = (dr[0]).ToString(),
                                TotalRev = dr[1].ToString(),
                               

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List<GetTotalExpRevByCCVM> lmd = new List<GetTotalExpRevByCCVM>();
                return lmd;
            }

        }
    }
}

