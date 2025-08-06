using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Service.Interfaces.UsersF;

namespace TaamerProject.API.Controllers.UsersF
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;
        public UsersController(IUsersService usersService, IConfiguration configuration)
        {
            _usersService = usersService;
            _configuration = configuration;
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers(string username)
        {
            var users = _usersService.GetUser(username);
            return users == null ? NotFound() : Ok(users);
        }
        [HttpGet("Getcategory_test")]
        public IActionResult Getcategory_test()
        {
            var cate = _usersService.GetAllCategories();
            return cate == null ? NotFound() : Ok(cate);
        }


        [HttpPost("GetAllUserFilterd")]
        public async Task<ApiResponse<PagedLists<UsersVM>>> GetAllFilterd([FromBody] RequestParam<UsersFilterDTO> param)
        {
            var apiResponse = new ApiResponse<PagedLists<UsersVM>>();

            try
            {
                var data = await _usersService.GetAllAsync(param);
                apiResponse.Success = true;
                apiResponse.Result = data;
            }
            catch (SqlException ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return apiResponse;
        }
        [HttpGet("Login")]
        public IActionResult Login(string username, string password, string? activationCode, string Lang, int? userid, string? DeviceToken, int? DeviceType)
        {
            try { 
            //IActionResult response = Unauthorized();
            GeneralMessage result = _usersService.Login(username, password, activationCode, Lang);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = _usersService.GetUserWithPrivilliges(username);
                    data.Result.tokenkey = Token(data.Result.UserId, data.Result.Password, data.Result.UserName).ToString();
                //////////////////////////////////////
               
                    
                    
                return Ok(data);// response;
                // return Ok(data);
            }
            else
            {
                return BadRequest(result);
            }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        

        [HttpGet("Token")]
        
        public string Token(int UserId, string Password,string UserName)
        {
            var apiResponse = "";

            try
            {
                
                        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId",UserId.ToString()),
                        new Claim("Password", Password.ToString()),
                        new Claim("UserName", UserName.ToString())


                    };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(500),
                            signingCredentials: signIn);

                        string _token = new JwtSecurityTokenHandler().WriteToken(token);

                apiResponse = _token.ToString();
                    
             
            }
            catch (SqlException ex)
            {
                apiResponse = ex.Message;
            }
            catch (Exception ex)
            {
                apiResponse = ex.Message;

            }

            return apiResponse;
        }



    }
}
