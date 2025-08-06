using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Acc_EmpFinYearsService : IAcc_EmpFinYearsService
    {


        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAcc_EmpFinYearsRepository _FiscalyearsPrivRepository;
        public Acc_EmpFinYearsService(IAcc_EmpFinYearsRepository acc_EmpFinYears
            , TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _FiscalyearsPrivRepository = acc_EmpFinYears;
        }


        public async Task<GeneralMessage> SaveFiscalyearsPriv(Acc_EmpFinYears fiscalyearsPriv, int UserId, int BranchId)
        {

            var Fiscalyears = await _FiscalyearsPrivRepository.CheckPriv(fiscalyearsPriv.EmpID, fiscalyearsPriv.BranchID, fiscalyearsPriv.YearID);
            if (Fiscalyears > 0)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.AlreadyFind };

            }
            else
            {
                try
                {

                    if (fiscalyearsPriv.Acc_EmpFinYearID == 0)
                    {
                        fiscalyearsPriv.AddUser = UserId;
                        fiscalyearsPriv.AddDate = DateTime.Now;
                        _TaamerProContext.Acc_EmpFinYears.Add(fiscalyearsPriv);
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة صلاحية مالية جديد";
                        _SystemAction.SaveAction("SaveFiscalyearsPriv", "Acc_EmpFinYearsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                      
                        
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};


                    }
                    else
                    {
                        var FiscalyearsPrivUpdated = _TaamerProContext.Acc_EmpFinYears.Where(x=>x.Acc_EmpFinYearID==fiscalyearsPriv.Acc_EmpFinYearID).FirstOrDefault();
                        if (FiscalyearsPrivUpdated != null)
                        {
                            FiscalyearsPrivUpdated.EmpID = fiscalyearsPriv.EmpID;
                            FiscalyearsPrivUpdated.BranchID = fiscalyearsPriv.BranchID;
                            FiscalyearsPrivUpdated.YearID = fiscalyearsPriv.YearID;
                            FiscalyearsPrivUpdated.UpdateUser = UserId;
                            FiscalyearsPrivUpdated.UpdateDate = DateTime.Now;
                        }
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل صلاحية سنة مالية رقم " + fiscalyearsPriv.Acc_EmpFinYearID;
                        _SystemAction.SaveAction("SaveFiscalyearsPriv", "Acc_EmpFinYearsService", 2,Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------


                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};


                    }

                }
                catch (Exception)
                {

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الصلاحية";
                    _SystemAction.SaveAction("SaveFiscalyearsPriv", "Acc_EmpFinYearsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

                }
            }


        }

        public async Task<IEnumerable<Acc_EmpFinYearsVM>> GetAllFiscalyearsPriv()
        {
            var Fiscalyears =await _FiscalyearsPrivRepository.GetAllFiscalyearsPriv();
            return Fiscalyears;
        }
        public async Task<int> CheckPriv(int? EmpID_P, int? BranchID_P, int? YearID_P)
        {
            var Fiscalyears =await _FiscalyearsPrivRepository.CheckPriv(EmpID_P, BranchID_P, YearID_P);
            return Fiscalyears;
        }
        public async Task<IEnumerable<Acc_EmpFinYearsVM>> GetAllBranchesByUserId(int UserId)
        {
            var Branches =await _FiscalyearsPrivRepository.GetAllBranchesByUserId(UserId);
            return Branches;
        }


        public GeneralMessage AccountRecycle(int YearID, string Con, int UserId, int BranchId)
        {


            int CountYearin_OPB = 0;
            int CountYearin_OPBthis = 0;
            int CountYearin_Transaction = 0;


            //try
            //{
            //    string selectTopics = "select COUNT(*) from Acc_Transactions where YearID=" + YearID.ToString();
            //    // Define the ADO.NET Objects
            //    using (SqlConnection con = new SqlConnection(Con))
            //    {
            //        SqlCommand topiccmd = new SqlCommand(selectTopics, con);
            //        con.Open();
            //        CountYearin_Transaction = (int)topiccmd.ExecuteScalar();
            //    }
            //}
            //catch (Exception)
            //{

            //    CountYearin_Transaction=0;
            //}



            try
            {
                string selectTopics2 = "select COUNT(*) from Acc_OpenBalance where YearID=" + (YearID + 1).ToString();
                // Define the ADO.NET Objects
                using (SqlConnection con = new SqlConnection(Con))
                {
                    SqlCommand topiccmd2 = new SqlCommand(selectTopics2, con);
                    con.Open();
                    CountYearin_OPB = (int)topiccmd2.ExecuteScalar();
                }
            }
            catch (Exception)
            {

                CountYearin_OPB = 0;
            }


            //try
            //{
            //    string selectTopics3 = "select COUNT(*) from Acc_OpenBalance where YearID=" + (YearID).ToString();
            //    // Define the ADO.NET Objects
            //    using (SqlConnection con = new SqlConnection(Con))
            //    {
            //        SqlCommand topiccmd3 = new SqlCommand(selectTopics3, con);
            //        con.Open();
            //        CountYearin_OPBthis = (int)topiccmd3.ExecuteScalar();
            //    }
            //}
            //catch (Exception)
            //{

            //    CountYearin_OPBthis = 0;
            //}



            if (CountYearin_OPB == 0)
            {
                try
                {

                    SqlConnection con = new SqlConnection(Con);
                    con.Open();

                    SqlCommand _cmd = new SqlCommand("exec AccountRecycle " + YearID.ToString());
                    _cmd.Connection = con;
                    _cmd.ExecuteNonQuery();

                    con.Close();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.BalancesHaveBeenSuccessfullyRotated;
                    _SystemAction.SaveAction("SaveClause", "Acc_ClausesService", 1, Resources.BalancesHaveBeenSuccessfullyRotated, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.BalancesHaveBeenSuccessfullyRotated};

                }
                catch (Exception ex)
                {

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.BalancesHaveBeenFaildRotated;
                    _SystemAction.SaveAction("AccountRecycle", "Acc_EmpFinYearsService", 1, Resources.BalancesHaveBeenFaildRotated, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.BalancesHaveBeenFaildRotated };

                }
            }
            else
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.BalancesHaveBeenFaildRotated;
                _SystemAction.SaveAction("AccountRecycle", "Acc_EmpFinYearsService", 1, Resources.Make_sure_of_the_year, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Make_sure_of_the_year };

            }


        }



        public GeneralMessage AccountRecycleDeleteYear(int YearID, string Con, int UserId, int BranchId)
        {
            int CountYearin_OPB2 = 0;
            try
            {
                string selectTopics2 = "select COUNT(*) from Acc_OpenBalance where YearID=" + (YearID).ToString();
                // Define the ADO.NET Objects
                using (SqlConnection con = new SqlConnection(Con))
                {
                    SqlCommand topiccmd2 = new SqlCommand(selectTopics2, con);
                    con.Open();
                    CountYearin_OPB2 = (int)topiccmd2.ExecuteScalar();
                }
            }
            catch (Exception)
            {

                CountYearin_OPB2 = 0;
            }
            SqlConnection con2 = new SqlConnection(Con);
            if (CountYearin_OPB2 != 0)
            {

                con2.Open();
                SqlCommand _cmd = new SqlCommand("delete from Acc_OpenBalance where YearID=" + YearID.ToString(), con2);
                try
                {
                    _cmd.ExecuteNonQuery();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تم الاسترجاع التدوير";
                    _SystemAction.SaveAction("AccountRecycleDeleteYear", "Acc_EmpFinYearsService", 1, Resources.RetrievalFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.RetrievalFailed };

                }
                catch (Exception x)
                {
                    con2.Close();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في استرجاع التدوير";
                    _SystemAction.SaveAction("AccountRecycleDeleteYear", "Acc_EmpFinYearsService", 1, Resources.RetrievalFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.RetrievalFailed };


                }
                finally
                {
                    con2.Close();
                }
            }
            else
            {
                con2.Close();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في استرجاع التدوير";
                _SystemAction.SaveAction("AccountRecycleDeleteYear", "Acc_EmpFinYearsService", 1, Resources.Make_sure_of_the_year, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Make_sure_of_the_year };

            }


        }


        public int AccountRecycleCheckYear(int YearID, string Con, int UserId, int BranchId)
        {
            try
            {

                var Count = 0;
                string selectTopics = "select COUNT(*) from Acc_OpenBalance where YearID=" + YearID.ToString();
                // Define the ADO.NET Objects
                using (SqlConnection con = new SqlConnection(Con))
                {
                    SqlCommand topiccmd = new SqlCommand(selectTopics, con);
                    con.Open();
                    int numrows = (int)topiccmd.ExecuteScalar();

                    return numrows;
                }

            }
            catch (Exception)
            {
                return 0;

            }
        }



        public async Task< IEnumerable<Acc_EmpFinYearsVM>> FillYearByUserIdandBranchSelect(int UserId, int? BranchID)
        {
            var Years = await _FiscalyearsPrivRepository.FillYearByUserIdandBranchSelect(UserId, BranchID);
            return Years;
        }
        public GeneralMessage DeleteFiscalyearsPriv(int ID, int UserId, int BranchId)
        {
            try
            {
                Acc_EmpFinYears EmpFinYears = _TaamerProContext.Acc_EmpFinYears.Where(s=>s.Acc_EmpFinYearID==ID).FirstOrDefault();
                EmpFinYears.IsDeleted = true;
                EmpFinYears.DeleteDate = DateTime.Now;
                EmpFinYears.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف صلاحية سنة مالية رقم " + ID;
                _SystemAction.SaveAction("DeleteFiscalyearsPriv", "Acc_EmpFinYearsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف صلاحية سنة مالية رقم " + ID; ;
                _SystemAction.SaveAction("DeleteFiscalyearsPriv", "Acc_EmpFinYearsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };

            }
        }


    }
}
