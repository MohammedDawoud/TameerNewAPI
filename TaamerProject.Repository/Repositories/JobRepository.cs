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
    public class JobRepository : IJobRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public JobRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public IEnumerable<Job> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JobVM>> GetAllJobs(string SearchText)
        {
            if (SearchText == "")
            {
                var jobs = _TaamerProContext.Job.Where(s => s.IsDeleted == false).Select(x => new JobVM
                {
                    JobId = x.JobId,
                    JobCode = x.JobCode,
                    JobNameAr = x.JobNameAr,
                    JobNameEn = x.JobNameEn,
                    Notes = x.Notes
                }).ToList();
                return jobs;
            }
            else
            {
                var jobs = _TaamerProContext.Job.Where(s => s.IsDeleted == false && (s.JobNameAr.Contains(SearchText) || s.JobNameEn.Contains(SearchText))).Select(x => new JobVM
                {
                    JobId = x.JobId,
                    JobCode = x.JobCode,
                    JobNameAr = x.JobNameAr,
                    JobNameEn = x.JobNameEn,
                    Notes = x.Notes
                }).ToList();
                return jobs;
            }
        }

        public Job GetById(int Id)
        {
            return _TaamerProContext.Job.Where(x => x.JobId == Id).FirstOrDefault();
        }

        public IEnumerable<Job> GetMatching(Func<Job, bool> where)
        {
            return _TaamerProContext.Job.Where(where).ToList<Job>();

        }

        public async Task< int> GetMaxOrderNumber()
        {
            if (_TaamerProContext.Job != null)
            {
                //var lastRow = DbSet.Where(s => s.JobId == BranchId).OrderByDescending(u => u.AddDate).Take(1).FirstOrDefault();
                var lastRow= _TaamerProContext.Job.Where(w=>w.IsDeleted==false).OrderByDescending(u => u.AddDate).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {
                        return int.Parse(lastRow.JobCode) + 1;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
         




    }
}


