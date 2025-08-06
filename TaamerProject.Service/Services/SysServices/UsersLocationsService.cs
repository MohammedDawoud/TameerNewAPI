using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class UsersLocationsService :  IUsersLocationsService
    {
        private readonly IUsersLocationsRepository _UsersLocationsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public UsersLocationsService(IUsersLocationsRepository usersLocationsRepository, TaamerProjectContext dataContext,
            ISystemAction systemAction)
        {
            _UsersLocationsRepository = usersLocationsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        
        public   bool SaveUsersLocations(string ipAddress, int UserId, int BranchId)
        {
            try
            {
                //var userLoc = _UsersLocationsRepository.GetMatching(s => s.UserId == UserId).FirstOrDefault();
                var userLoc =   _TaamerProContext.UsersLocations.Where(s => s.UserId == UserId).FirstOrDefault();
                var loc = GetUserLocation(ipAddress).Result;
                if (userLoc == null)
                {
                    UsersLocations usersLocations = new UsersLocations();
                    usersLocations.UserId = UserId;
                    usersLocations.RegionCode = loc.Region_Code;
                    usersLocations.RegionName = loc.Region_Name;
                    usersLocations.CountryCode = loc.Country_Code;
                    usersLocations.CountryName = loc.Country_Name;
                    usersLocations.ZipCode = loc.Zip_Code;
                    usersLocations.TimeZone = loc.TimeZone;
                    usersLocations.IPType = loc.Type;
                    usersLocations.Latitude = loc.Latitude;
                    usersLocations.Ip = loc.Ip;
                    usersLocations.ContinentCode = loc.Continent_Code;
                    usersLocations.ContinentName = loc.Continent_Name;
                    usersLocations.City = loc.City;
                    usersLocations.Longitude = loc.Longitude;
                    usersLocations.AddUser = UserId;
                    usersLocations.AddDate = DateTime.Now;
                    _TaamerProContext.UsersLocations.Add(usersLocations);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مكان لمستخدم جديد";
                    _SystemAction.SaveAction("SaveUsersLocations", "UsersLocationsService", 1, Resources.General_SavedSuccessfully , "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return true;
                }
                else
                {
                    userLoc.RegionCode = loc.Region_Code;
                    userLoc.RegionName = loc.Region_Name;
                    userLoc.CountryCode = loc.Country_Code;
                    userLoc.CountryName = loc.Country_Name;
                    userLoc.ZipCode = loc.Zip_Code;
                    userLoc.TimeZone = loc.TimeZone;
                    userLoc.IPType = loc.Type;
                    userLoc.Latitude = loc.Latitude;
                    userLoc.Ip = loc.Ip;
                    userLoc.ContinentCode = loc.Continent_Code;
                    userLoc.ContinentName = loc.Continent_Name;
                    userLoc.City = loc.City;
                    userLoc.Longitude = loc.Longitude;
                    userLoc.UpdateUser = UserId;
                    userLoc.UpdateDate = DateTime.Now;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تعديل مكان لمستخدم جديد";
                    _SystemAction.SaveAction("SaveUsersLocations", "UsersLocationsService", 2, Resources.General_SavedSuccessfully , "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return true;
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ مكان لمستخدم";
                _SystemAction.SaveAction("SaveUsersLocations", "UsersLocationsService", 1,Resources.FailedSave, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return false;
            }
        }
        private async Task<UsersLocationsVM> GetUserLocation (string ipAddress)
        {
            string APIKey = "1e694c06752895036df58159b56a91e5";
            string url = string.Format("http://api.ipapi.com/{1}?access_key={0}", APIKey, ipAddress);
            using (WebClient client = new WebClient())
            {
                string jsonstring = client.DownloadString(url);
                UsersLocationsVM dynObj = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<UsersLocationsVM>(jsonstring); //(UsersLocationsVM)JsonConvert.DeserializeObject(jsonstring);
                return  dynObj;
            }
        }

        

    }
}
