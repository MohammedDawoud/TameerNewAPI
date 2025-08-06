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
    public class ProjectCommentsRepository : IProjectCommentsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectCommentsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task< IEnumerable<ProjectCommentsVM>> GetAllProjectComments(int ProjectId)
        {
            var projectComments = _TaamerProContext.ProjectComments.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new ProjectCommentsVM
            {
                CommentId = x.CommentId,
                Comment = x.Comment ?? "",
                ProjectId = x.ProjectId,
                Date = x.Date,
                UserId = x.UserId,
                UserName = x.Users.FullName,
                UserImgUrl = x.Users.ImgUrl ?? "/distnew/images/userprofile.png",
            }).ToList();
            return projectComments;
        }
    }
}
