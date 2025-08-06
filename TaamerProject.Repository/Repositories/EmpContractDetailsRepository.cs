using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using System.Data.SqlClient;
using System.Data;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class EmpContractDetailsRepository : IEmpContractDetailRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public EmpContractDetailsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<EmpContractDetailVM>> GetAllEmpConDetailsByContractId(int? ContractId)
        {
            try
            {
                return _TaamerProContext.EmpContractDetail.Where(s => s.IsDeleted == false && s.ContractId == ContractId ).Select(x => new
                {
                    x.ContractDetailId,
                    x.ContractId,
                    x.SerialId,
                    x.Clause,
                   
                }).Select(s => new EmpContractDetailVM
                {
                    ContractDetailId = s.ContractDetailId,
                    ContractId = s.ContractId,
                    SerialId = s.SerialId,
                    Clause = s.Clause,
                   
                }).ToList();
            }
            catch (Exception)
            {
                return _TaamerProContext.EmpContractDetail.Where(s => s.IsDeleted == false && s.ContractId == ContractId).Select(x => new EmpContractDetailVM
                {
                    ContractDetailId = x.ContractDetailId,
                    ContractId = x.ContractId,
                    SerialId = x.SerialId,
                    Clause = x.Clause,
                });
            }
        }

        public async Task<IEnumerable<EmpContractDetailVM>> GetAllDetailsByContractId(int? ContractId)
        {
            var details = _TaamerProContext.EmpContractDetail.Where(s => s.IsDeleted == false && s.ContractId == ContractId).Select(x => new EmpContractDetailVM
            {
              ContractDetailId=  x.ContractDetailId,
                ContractId=  x.ContractId,
                SerialId=  x.SerialId,
                Clause=  x.Clause,                
            }).ToList();
            return details;
        }
        public async Task<DataTable> GetAllContractDetailsByContractId(int? ContractId,string Con)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Sp_EmpContract";
                        command.Connection = con;
                        command.Parameters.Add(new SqlParameter("@ContractId", ContractId));
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
                DataTable dt = new DataTable();
                return dt;
            }
        }

    }
}
