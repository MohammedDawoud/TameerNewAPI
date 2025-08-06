using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaamerProject.Models;

namespace TaamerProject.API.Helper
{
    public class GlobalShared
    {
        public int UserId_G { get; set; }
        public int BranchId_G { get; set; }
        public string Lang_G { get; set; }
        public int YearId_G { get; set; }
        public List<int> Privilliges_G { get; set; }


        HttpContext? _httpContext;
        public GlobalShared(HttpContext httpContext)
        {
            var userid = ""; var Branch = ""; var lng = ""; var yearid = "";
            List<int> userprivligs=new List<int>();
            if(httpContext!=null)
            {
                _httpContext = httpContext;
                var identity = _httpContext.User.Identity as ClaimsIdentity;

                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    userid = identity.FindFirst("UserId")?.Value;

                    ///////////////////////////////////////////////////////
                    string claimName = "Userpriv";

                    // Find all the claims with the specified name
                    var claimsWithName = identity.Claims
                        .Where(c => c.Type == claimName)
                        .ToList();

                    // Extract the integer values from the claims
                    foreach (var claim in claimsWithName)
                    {
                        if (int.TryParse(claim.Value, out var intValue))
                        {
                            userprivligs.Add(intValue);
                        }
                    }
                    ///////////////////////////////////////////////////////
                    Branch = _httpContext.Request.Headers["BranchId"].ToString();
                    lng = _httpContext.Request.Headers["Lang"].ToString();
                    yearid = _httpContext.Request.Headers["YearId"].ToString();
                }
                UserId_G = userid == "" ? 0 : Convert.ToInt32(userid);
                BranchId_G = Branch == "" ? 0 : Convert.ToInt32(Branch);
                Lang_G = lng == "" ? "rtl" : lng;
                YearId_G = yearid == "" ? 0 : yearid == "null" ?0: Convert.ToInt32(yearid);
                Privilliges_G = userprivligs.ToList();
            }
            else
            {
                UserId_G = 0;
                BranchId_G = 0;
                Lang_G = "rtl";
                YearId_G = 0;
            }

        }

    }


    public  class UsersData
    {
        public UsersData()
        {
            //UserPrivileges = new List<int>();
            UserNotifications = new List<NotificationVM>();
        }
        public  string? UserName { get; set; }
        public int? Session { get; set; }

        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }


        public string? FullName { get; set; }

        public string? License { get; set; }

        public  int? UserId { get; set; }
        public  int? BranchId { get; set; }
        public  string? BranchName { get; set; }
        public  string? CompanyName { get; set; }
        public  int? CompanyID { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public decimal? OrgVAT { get; set; }

        public List<int>? UserPrivileges { get; set; }
        public  List<int>? UserNotifPrivileges { get; set; }
        public  List<NotificationVM>? UserNotifications { get; set; }
        public  List<ProjectPhasesTasksVM>? InProgressTask { get; set; }
        public  List<UsersVM>? AllUsers { get; set; }
        public  List<UserMailsVM>? UserMails { get; set; }
        public  string? Lang { get; internal set; }
        public  VersionVM? Version { get; internal set; }
        public  int? FiscalId_G { get; set; }
        public  int? YearId_G { get; set; }
        public  string? LogoUrl { get; set; }

        public  string? CurrentBranch { get; set; }
        public string? Token { get; set; }
    }
}
