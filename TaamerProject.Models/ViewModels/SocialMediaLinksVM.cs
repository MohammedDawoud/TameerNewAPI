using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{

    public class SocialMediaLinksVM
    {

        public int LinksId { get; set; }
        public string? FaceBookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? GooglePlusLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? LinkedInLink { get; set; }
        public string? SnapchatLink { get; set; }

    }
}
