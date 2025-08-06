
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Bayanatech.TameerPro.Repository;

namespace TaamerProject.Repository.Repositories
{
    public class GuideDepartmentDetailsRepository :  IGuideDepartmentDetailsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public GuideDepartmentDetailsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<GuideDepartmentDetailsVM>> GetAllDepDetails(int DepId, string searchStr, int? DepDetailId = null)
        {
            IEnumerable<GuideDepartmentDetailsVM>  DetailsTypes = null;
            if (string.IsNullOrEmpty(searchStr))
            {
                DetailsTypes = _TaamerProContext.GuideDepartmentDetails.Where(s => s.IsDeleted == false).Select(x => new GuideDepartmentDetailsVM
                {
                    DepId = x.DepId,
                    DepDetailsId = x.DepDetailsId,
                    Header = x.Header,
                    Type = x.Type,
                    Link = x.Link,
                    Text = x.Text,
                    DepartmentName=x.GuideDepartments.DepNameAr??"",
                });
            }
            else {
                DetailsTypes = _TaamerProContext.GuideDepartmentDetails.Where(s => s.IsDeleted == false && (s.Header.Contains(searchStr) || s.Link.Contains(searchStr) || s.Text.Contains(searchStr))).Select(x => new GuideDepartmentDetailsVM
                {
                    DepId = x.DepId,
                    DepDetailsId = x.DepDetailsId,
                    Header = x.Header,
                    Type = x.Type,
                    Link = x.Link,
                    Text = x.Text,
                    DepartmentName = x.GuideDepartments.DepNameAr ?? "",

                });
            }
            return DetailsTypes;

        }


        public async Task<IEnumerable<GuideDepartmentDetailsVM>> GetAllDepDetailsSearch(string searchStr)
        {
            IEnumerable<GuideDepartmentDetailsVM> DetailsTypes = null;
            if (string.IsNullOrEmpty(searchStr))
            {
                DetailsTypes = _TaamerProContext.GuideDepartmentDetails.Where(s => s.IsDeleted == false).Select(x => new GuideDepartmentDetailsVM
                {
                    DepId = x.DepId,
                    DepDetailsId = x.DepDetailsId,
                    Header = x.Header,
                    Type = x.Type,
                    Link = x.Link,
                    Text = x.Text,
                    DepartmentName = x.GuideDepartments.DepNameAr ?? "",
                    NameAR=x.NameAR,
                    NameEn=x.NameEn,
                });
            }
            else
            {
                DetailsTypes = _TaamerProContext.GuideDepartmentDetails.Where(s => s.IsDeleted == false && (s.Header.Contains(searchStr) || s.Link.Contains(searchStr) || s.Text.Contains(searchStr) 
                ||s.NameAR.Contains(searchStr) ||s.NameEn.Contains(searchStr))).Select(x => new GuideDepartmentDetailsVM
                {
                    DepId = x.DepId,
                    DepDetailsId = x.DepDetailsId,
                    Header = x.Header,
                    Type = x.Type,
                    Link = x.Link,
                    Text = x.Text,
                    DepartmentName = x.GuideDepartments.DepNameAr ?? "",
                    NameAR = x.NameAR,
                    NameEn = x.NameEn,
                });
            }
            return DetailsTypes;

        }

    }
}
