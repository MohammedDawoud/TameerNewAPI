using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactListController : ControllerBase
    {
        private IContactListsService _contactLists;
        public GlobalShared _globalshared;
        public ContactListController(IContactListsService contactLists)
        {
            _contactLists = contactLists;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetContactLists")]
        public async Task<IActionResult> GetContactLists(int Id,int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok( _contactLists.GetContactLists(Id,Type, _globalshared.UserId_G));
        }


        [HttpPost("SaveContact")]
        public IActionResult SaveContact([FromBody] ContactList contact)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _contactLists.SaveContact(contact, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("DeleteContact")]
        public IActionResult DeleteContact(int Id)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _contactLists.DeleteContact(Id, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}
