using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class ContactListRepository : IContactListRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ContactListRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<ContactListVM>> GetContactLists(int Id, int Type,int UserId)
        {
            if (Id != 0 && Id > 0)
            {
                if (Type == 1)
                {
                    var contact = _TaamerProContext.ContactLists.Where(s => s.IsDeleted == false && s.TaskId == Id).Select(x => new ContactListVM
                    {
                        TaskId = x.TaskId,
                        ContactDate = x.ContactDate,
                        ContactListId = x.ContactListId,
                        Contacttxt = x.Contacttxt,
                        OrderId = x.OrderId,
                        ProjectId = x.ProjectId,
                        UserId = x.UserId,
                        UserName = x.Users != null ? x.Users.FullNameAr ?? x.Users.FullName ?? "" : "",
                        IsSender = x.UserId==UserId ? true : false,
                    }).ToList();
                    return contact;
                }
                else if (Type == 3)
                {
                    var contact = _TaamerProContext.ContactLists.Where(s => s.IsDeleted == false && s.ProjectId == Id).Select(x => new ContactListVM
                    {
                        TaskId = x.TaskId,
                        ContactDate = x.ContactDate,
                        ContactListId = x.ContactListId,
                        Contacttxt = x.Contacttxt,
                        OrderId = x.OrderId,
                        ProjectId = x.ProjectId,
                        UserId = x.UserId,
                        UserName = x.Users!=null? x.Users.FullNameAr ?? x.Users.FullName ?? "":"",
                        IsSender = x.UserId == UserId ? true : false,

                    }).ToList();
                    return contact;
                }
                else
                {
                    var contact = _TaamerProContext.ContactLists.Where(s => s.IsDeleted == false && s.OrderId == Id).Select(x => new ContactListVM
                    {
                        TaskId = x.TaskId,
                        ContactDate = x.ContactDate,
                        ContactListId = x.ContactListId,
                        Contacttxt = x.Contacttxt,
                        OrderId = x.OrderId,
                        ProjectId = x.ProjectId,
                        UserId = x.UserId,
                        UserName = x.Users != null ? x.Users.FullNameAr ?? x.Users.FullName ?? "" : "",
                        IsSender = x.UserId == UserId ? true : false,

                    }).ToList();
                    return contact;
                }
            }
            else
            {
                return new List<ContactListVM> ();
            }
        }
    }
}
