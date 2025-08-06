using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ContactPerson : Auditable
    {
        public int PersonId { get; set; }
        public string? PersonCode { get; set; }
        public string? PersonName { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
