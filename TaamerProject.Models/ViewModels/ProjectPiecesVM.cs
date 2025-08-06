using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ProjectPiecesVM
    {
        public int PieceId { get; set; }
        public string? PieceNo { get; set; }
        public string? Notes { get; set; }
        public int ProjectId { get; set; }
    }
}
