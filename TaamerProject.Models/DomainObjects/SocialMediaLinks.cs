using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class SocialMediaLinks : Auditable
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
