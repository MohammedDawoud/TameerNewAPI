namespace TaamerProject.Models
{
    public class CostCenterTreeVM
    {
        public string? id { get; set; }
        public string? parent { get; set; }
        public string? text { get; set; }
        public CostCenterTreeVM(string? ID, string? Parent, string? Text)
        {
            id = ID;
            parent = Parent;
            text = Text;
        }
    }
}
