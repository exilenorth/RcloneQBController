namespace RcloneQBController.Models
{
    public class FileTransfer
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public double Progress { get; set; }
    }
}