

namespace TaamerProject.Models
{
    public class Dawamyattend : Auditable
    {


        public string? SlNo { get; set; }
        public string? UserID { get; set; }
        public string? RecognitionType { get; set; }
        public string? ProcessType { get; set; }
        public string? PunchDateTime { get; set; }
        public string? DeviceName { get; set; }


    }
}
