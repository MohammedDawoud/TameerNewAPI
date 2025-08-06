using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class CarMovementsRepository : ICarMovementsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CarMovementsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<CarMovementsVM>> GetAllCarMovements()
        {
            var employees = _TaamerProContext.CarMovements.Where(s => s.IsDeleted == false).Select(x => new CarMovementsVM
            {
                MovementId = x.MovementId,
                ItemId = x.ItemId,
                Type = x.Type,
                EmpId = x.EmpId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                EmpAmount = x.EmpAmount,
                OwnerAmount = x.OwnerAmount,
                Notes = x.Notes,
                EmployeeName = x.Employees.EmployeeNameAr,
                ItemName = x.Item.NameAr,
                TypeName = x.Types.NameAr,
                AccountId=x.AccountId??0,
                //TypeName = x.Type == 1 ? "بنزين" : x.Type == 2 ? "زيت" : x.Type == 3 ? "صيانة" : x.Type == 4 ? "مخالفة مرورية " : "تامين",
            });
            return employees;

        }

        public async Task<IEnumerable<CarMovementsVM>> GetAllCarMovementsSearchObject(CarMovementsVM Search, int BranchId)
        {
            try
            {

                if (Search.EndDate == null || Search.Date == null)
                {
                    var Vacations1 = _TaamerProContext.CarMovements.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.EmpId == Search.EmpId || Search.EmpId == null) &&
                  (s.Type == Search.Type || Search.Type == null) && (s.ItemId == Search.ItemId || Search.ItemId == null)).Select(x => new CarMovementsVM
                  {
                      MovementId = x.MovementId,
                      ItemId = x.ItemId,
                      Type = x.Type,
                      EmpId = x.EmpId,
                      Date = x.Date,
                      HijriDate = x.HijriDate,
                      EmpAmount = x.EmpAmount,
                      OwnerAmount = x.OwnerAmount,
                      Notes = x.Notes,
                      EmployeeName = x.Employees.EmployeeNameAr,
                      ItemName = x.Item.NameAr,
                      TypeName = x.Types.NameAr,
                      AccountId = x.AccountId ?? 0,

                      //TypeName = x.Type == 1 ? "بنزين" : x.Type == 2 ? "زيت" : x.Type == 3 ? "صيانة" : x.Type == 4 ? "مخالفة مرورية " : "تامين",
                  }).Select(s => new CarMovementsVM
                  {
                      MovementId = s.MovementId,
                      ItemId = s.ItemId,
                      Type = s.Type,
                      EmpId = s.EmpId,
                      Date = s.Date,
                      HijriDate = s.HijriDate,
                      EmpAmount = s.EmpAmount,
                      OwnerAmount = s.OwnerAmount,
                      Notes = s.Notes,
                      EmployeeName = s.EmployeeName,
                      ItemName = s.ItemName,
                      TypeName = s.TypeName,
                      AccountId = s.AccountId,

                  }).ToList();
                    return Vacations1;
                }
                else
                {
                    var Vacations = _TaamerProContext.CarMovements.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.EmpId == Search.EmpId || Search.EmpId == null) &&
                    (s.Type == Search.Type || Search.Type == null) && (s.ItemId == Search.ItemId || Search.ItemId == null)
                   ).Select(x => new CarMovementsVM
                   {
                       MovementId = x.MovementId,
                       ItemId = x.ItemId,
                       Type = x.Type,
                       EmpId = x.EmpId,
                       Date = x.Date,
                       HijriDate = x.HijriDate,
                       EmpAmount = x.EmpAmount,
                       OwnerAmount = x.OwnerAmount,
                       Notes = x.Notes,
                       EmployeeName = x.Employees.EmployeeNameAr,
                       ItemName = x.Item.NameAr,
                       TypeName = x.Types.NameAr,
                       AccountId = x.AccountId ?? 0,

                   }).Select(s => new CarMovementsVM
                   {
                       MovementId = s.MovementId,
                       ItemId = s.ItemId,
                       Type = s.Type,
                       EmpId = s.EmpId,
                       Date = s.Date,
                       HijriDate = s.HijriDate,
                       EmpAmount = s.EmpAmount,
                       OwnerAmount = s.OwnerAmount,
                       Notes = s.Notes,
                       EmployeeName = s.EmployeeName,
                       ItemName = s.ItemName,
                        TypeName = s.TypeName,
                       AccountId = s.AccountId,

                   }).ToList().Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));

                    return Vacations;
                }
            }
            catch
            {
                var Vacations = _TaamerProContext.CarMovements.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.EmpId == Search.EmpId ||
                s.Type == Search.Type ||
                s.ItemId == Search.ItemId
               )).Select(x => new CarMovementsVM
               {
                   MovementId = x.MovementId,
                   ItemId = x.ItemId,
                   Type = x.Type,
                   EmpId = x.EmpId,
                   Date = x.Date,
                   HijriDate = x.HijriDate,
                   EmpAmount = x.EmpAmount,
                   OwnerAmount = x.OwnerAmount,
                   Notes = x.Notes,
                   EmployeeName = x.Employees.EmployeeNameAr,
                   ItemName = x.Item.NameAr,
                   TypeName = x.Types.NameAr,
                   AccountId = x.AccountId ?? 0,
               });
                return Vacations;
            }

        }
        public async Task<IEnumerable<CarMovementsVM>> SearchCarMovements(CarMovementsVM CarMovementsSearch, int BranchId)
        {

            var CarMovements = _TaamerProContext.CarMovements.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.ItemId == CarMovementsSearch.ItemId || CarMovementsSearch.ItemId == null) &&
                                              (s.Type == CarMovementsSearch.Type || CarMovementsSearch.Type == 0))
                                                .Select(x => new CarMovementsVM
                                                {
                                                    MovementId = x.MovementId,
                                                    ItemId = x.ItemId,
                                                    Type = x.Type,
                                                    EmpId = x.EmpId,
                                                    BranchId = x.BranchId,
                                                    Date = x.Date,
                                                    HijriDate = x.HijriDate,
                                                    EmpAmount = x.EmpAmount,
                                                    OwnerAmount = x.OwnerAmount,
                                                    Notes = x.Notes,
                                                    EmployeeName = x.Employees.EmployeeNameAr,
                                                    ItemName = x.Item.NameAr,
                                                    AccountId = x.AccountId ?? 0,
                                                    TypeName = x.Type == 1 ? "بنزين" : x.Type == 2 ? "زيت" : x.Type == 3 ? "صيانة" : x.Type == 4 ? "مخالفة مرورية " : "",
                                                }).ToList();
            return CarMovements;
        }
        public async Task<IEnumerable<CarMovementsVM>> GetAllCarMovementsByDateSearch(string FromDate, string ToDate)
        {
            List<CarMovementsVM> CarMovement = _TaamerProContext.CarMovements.Where(s => s.IsDeleted == false).Select(x => new
            {
                x.MovementId,
                x.ItemId,
                x.Type,
                x.EmpId,
                x.BranchId,
                x.Date,
                x.HijriDate,
                x.EmpAmount,
                x.OwnerAmount,
                x.Notes,
                EmployeeName = x.Employees.EmployeeNameAr,
                ItemName = x.Item.NameAr,
                AccountId = x.AccountId ?? 0,
                TypeName = x.Type == 1 ? "بنزين" : x.Type == 2 ? "زيت" : x.Type == 3 ? "صيانة" : x.Type == 4 ? "مخالفة مرورية " : "",
                //TypeName = x.Type == 1 ? "بنزين" : x.Type == 2 ? "زيت" : x.Type == 3 ? "صيانة" : x.Type == 4 ? "مخالفة مرورية " : "",
            }).Select(m => new CarMovementsVM
            {
                MovementId = m.MovementId,
                ItemId = m.ItemId,
                Type = m.Type,
                EmpId = m.EmpId,
                BranchId = m.BranchId,
                Date = m.Date,
                HijriDate = m.HijriDate,
                EmpAmount = m.EmpAmount,
                OwnerAmount = m.OwnerAmount,
                Notes = m.Notes,
                EmployeeName = m.EmployeeName,
                ItemName = m.ItemName,
                AccountId = m.AccountId,
                TypeName = m.Type == 1 ? "بنزين" : m.Type == 2 ? "زيت" : m.Type == 3 ? "صيانة" : m.Type == 4 ? "مخالفة مرورية " : "",
            }).ToList().Where(s => 
            DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) 
            && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
            return CarMovement;
        }
        //heba
        public async Task<DataTable> GetAllCarMovementsByDateSearch(string carType, string carId, string FromDate, string ToDate,string EmpId, string Con)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CarMpovement";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int? Car_Type = (int?)null;
                        int? Car_Id = (int?)null;
                        int? Emp_ID = (int?)null;
                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add (new SqlParameter("@From", DBNull.Value));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", from));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", DBNull.Value));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@to",to));
                        }
                        if (carType != "")
                        {
                            Car_Type = Convert.ToInt32(carType);
                            command.Parameters.Add(new SqlParameter("@type", Car_Type));
                        }
                        else command.Parameters.Add(new SqlParameter("@type", DBNull.Value));
                        if (carId != "")
                        {
                            Car_Id = Convert.ToInt32(carId);
                            command.Parameters.Add(new SqlParameter("@Itemid", Car_Id));
                        }
                        else command.Parameters.Add(new SqlParameter("@Itemid", DBNull.Value));

                        if (EmpId != "")
                        {
                            Emp_ID = Convert.ToInt32(EmpId);
                            command.Parameters.Add(new SqlParameter("@EmpId", Emp_ID));
                        }
                        else command.Parameters.Add(new SqlParameter("@EmpId", DBNull.Value));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }


                return dt;
            }
            catch(Exception ex)
            {
                return new DataTable();
            }

        }

        public async Task<DataTable> GetAllCarMovementsByDate(string FromDate, string ToDate, string Con)
        {
            try
            {
   
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "AllCarMovement";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@Date1", DBNull.Value));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@Date1", from));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@Date2", DBNull.Value));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@Date2", to));
                        }
                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }


                return dt;
            }
            catch
            {
                return new DataTable();
            }


        }
    }
}
