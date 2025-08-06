using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces.UsersF;
using TaamerProject.Repository.Interfaces;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace TaamerProject.Service.Services.UsersF
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IAcc_CategoriesRepository _categoriesRepository;
        private readonly IFiscalyearsRepository _fiscalyears;
        private readonly IUserPrivilegesRepository _userPrivileges;

        public UsersService(IUsersRepository usersRepository,IAcc_CategoriesRepository categoriesRepository, IFiscalyearsRepository fiscalyears, IUserPrivilegesRepository userPrivileges) 
        {
            _usersRepository= usersRepository;
            _categoriesRepository = categoriesRepository;
            _fiscalyears = fiscalyears;
            _userPrivileges= userPrivileges;
        }
        public Task<UsersVM> GetUser(string username)
        {
            return _usersRepository.GetUser(username);
        }
        public Task<UsersVM> GetUserWithPrivilliges(string username)
        {
            try
            {
                var user = _usersRepository.GetUser(username);
                user.Result.UserPrivilliges = _userPrivileges.GetPrivilegesIdsByUserId(user.Result.UserId);
                return user;
            } 
            catch (Exception ex)
            {
                throw;
            } 
        }
        public Task<IEnumerable<Acc_CategoriesVM>> GetAllCategories()
        {
            return _categoriesRepository.GetAllCategories("");
        }

        public async Task<PagedLists<UsersVM>> GetAllAsync(RequestParam<UsersFilterDTO> Param)
        {
            return await _usersRepository.GetAllAsync(Param);
        }

        #region Service Functions
        
        public GeneralMessage Login(string username, string password, string activationCode, string Lang)
        {
            GeneralMessage response = new GeneralMessage();

            string r = "";


            //return response;
            try
            {
                var user = GetUser(username);
                if (user == null)
                {
                    if (Lang == "Ar" || Lang == "rtl" || Lang == "ar")
                    {
                        r = "هذا المستخدم غير موجود ";
                    }
                    else
                    {
                        r = "User Not Found";
                    }



                    response.StatusCode = HttpStatusCode.NoContent;
                    response.ReasonPhrase = r;
                    return response;

                }

                //if(user)
                //if (user.Result.UserId != userid)
                //{
                //    if (Lang == "Ar" || Lang == "rtl" || Lang == "ar")
                //    {
                //        r = "هذا المستخدم غير مفعل ";
                //    }
                //    else
                //    {
                //        r = "this user is not activated";
                //    }


                //    response.StatusCode = HttpStatusCode.NoContent;
                //    response.ReasonPhrase = r;
                //    return response;
                //}
                //else
                //{

                    bool IsOnline = false;
                    var result = ValidateUserCofidential(username, password, activationCode);
                    string DateNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    var min = 0;
                    var min2 = 0;
                    if (result)
                    {
                        if (user.Result.ActiveTime != null)
                        {
                            DateTime ActiveUsertime = user.Result.ActiveTime ?? DateTime.Now;
                            TimeSpan ts = DateTime.Now - ActiveUsertime;
                            min2 = ts.Minutes;
                            min = Math.Abs(min2);

                            if (min >= user.Result.Session)
                            {
                                IsOnline = false;
                            }
                            else
                            {
                                if (user.Result.ISOnlineNew == null)
                                {
                                    IsOnline = false;
                                }
                                else
                                {
                                    IsOnline = false;// user.ISOnlineNew ?? false;
                                }
                            }
                        }
                        else
                        {
                            if (user.Result.ISOnlineNew == null)
                            {
                                IsOnline = false;
                            }
                            else
                            {
                                IsOnline = false;// user.ISOnlineNew ?? false;
                                //IsOnline = false;
                            }
                        }
                        //var ActiveYear = _fiscalyears.GetActiveYear();
                        //UserDataDto.FiscalId_G = ActiveYear.FiscalId;
                        //UserDataDto.YearId_G = ActiveYear.YearId;
                        //string ExpireUserDate = user.ExpireDate;
                        //if (ExpireUserDate != "0")
                        //{
                        //    if (DateTime.ParseExact(ExpireUserDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                        //    {

                        //        if (Lang == "Ar" || Lang == "rtl" || Lang == "ar")
                        //        {
                        //            r = "صلاحية الحساب منتهية، الرجاء مراجعة مدير النظام";

                        //        }
                        //        else
                        //        {
                        //            r = "the account is expaired, please connect with the admin of the system";
                        //        }
                        //        response.Success = false;
                        //        response.Result = r;
                        //        response.Message = HttpStatusCode.Unauthorized.ToString();
                        //        //response.Content = new ObjectContent<UsersDataDto>(user, new JsonMediaTypeFormatter(), "application/json"); 
                        //        return response;
                        //    }
                        //    else
                        //    {
                        //        ClearExpireDate(user.UserId);
                        //    }

                        //}
                        if (user.Status == 0)
                        {
                            if (Lang == "Ar" || Lang == "rtl" || Lang == "ar")
                            {
                                r = "الحساب تم إيقافه، الرجاء مراجعة مدير النظام";
                            }
                            else
                            {
                                r = "the account is stopped, please connect with the admin of the system";
                            }
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.ReasonPhrase = r;
                            return response;
                        }
                        //if (IsOnline != false)
                        //{
                        //    if (Lang == "Ar" || Lang == "rtl" || Lang == "ar")
                        //    {
                        //        r = "الحساب الذي تحاول الدخول به مستخدم الآن الرجاء مراجعه مدير النظام";
                        //    }
                        //    else
                        //    {
                        //        r = "the account is Used now, please connect with the admin of the system";
                        //    }
                        //    response.StatusCode = HttpStatusCode.NoContent;
                        //    response.ReasonPhrase = r;
                        //    //response.Content = new ObjectContent<UsersDataDto>(user, new JsonMediaTypeFormatter(), "application/json");
                        //    return response;
                        //}
                 
                        // FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.UserId.ToString(), DateTime.Now, DateTime.Now.AddDays(1), (remember != null ? true : false), Convert.ToString(BranchId));
                        ////HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                        string json = JsonConvert.SerializeObject(user);



                       
                      
                            response.StatusCode = HttpStatusCode.OK;
                            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                            return response;
                        


                    }
                    else
                    {
                        //TempData["username"] = username;
                        if (Lang == "Ar" || Lang == "rtl" || Lang == "ar")
                        {
                            r = "تاكد من إدخال البيانات بشكل صحيح";
                        }
                        else
                        {
                            r = "Please, Insure the data you entered is correct";
                        }
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.ReasonPhrase = r;
                        return response;


                    }
                

            }
            catch (Exception e)
            {


                if (Lang == "Ar" || Lang == "rtl" || Lang == "ar")
                {
                    r = "تاكد من ادخال البيانات بشكل صحيح";
                }
                else
                {
                    r = "Please, Insure the data you entered is correct";
                }

                response.StatusCode = HttpStatusCode.NoContent;
                response.ReasonPhrase = r;
                return response;
            }

        }






        public bool ValidateUserCofidential(string UserName, string Password, string activationCode)
        {
            if (UserName == "admin")
            {
                var user = _usersRepository.GetMatchingUserWithPassword(UserName, activationCode);
                if (DecryptValue(user.Password) == Password)
                {
                    return true;
                }

                return false;

            }
            else
            {

                //Status == 0 --> disactive
                var user = _usersRepository.GetMatchingUserWithPassword(UserName, activationCode);
                if (DecryptValue(user.Password) == Password)
                {
                    return true;
                }

                return false;

            }

        }
        public string DecryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Convert.FromBase64String(value); ;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateDecryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(result);
                }
            }
        }
        public string EncryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Encoding.UTF8.GetBytes(value);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateEncryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }

        #endregion
    }

}
