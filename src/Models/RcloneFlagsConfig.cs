namespace RcloneQBController.Models
{
    public class RcloneFlagsConfig
    {
        public string? MinAge { get; set; }
        public int Transfers { get; set; } = 4;
        public int Checkers { get; set; } = 8;
    }
}