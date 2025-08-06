using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ProjectPieces : Auditable
    {
        public int PieceId { get; set; }
        public string? PieceNo { get; set; }
        public string? Notes { get; set; }
        public int ProjectId { get; set; }
    }
}
