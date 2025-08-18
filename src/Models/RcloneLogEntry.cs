using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class RcloneLogEntry
    {
        [JsonPropertyName("level")]
        public string Level { get; set; } = string.Empty;

        [JsonPropertyName("msg")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;

        [JsonPropertyName("stats")]
        public RcloneStats? Stats { get; set; }
    }

    public class RcloneStats
    {
        [JsonPropertyName("bytes")]
        public long Bytes { get; set; }

        [JsonPropertyName("checks")]
        public int Checks { get; set; }

        [JsonPropertyName("deletes")]
        public int Deletes { get; set; }

        [JsonPropertyName("elapsedTime")]
        public double ElapsedTime { get; set; }

        [JsonPropertyName("errors")]
        public int Errors { get; set; }

        [JsonPropertyName("eta")]
        public int? Eta { get; set; }

        [JsonPropertyName("fatalError")]
        public bool FatalError { get; set; }

        [JsonPropertyName("renames")]
        public int Renames { get; set; }

        [JsonPropertyName("retryError")]
        public bool RetryError { get; set; }

        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("totalBytes")]
        public long TotalBytes { get; set; }

        [JsonPropertyName("totalChecks")]
        public int TotalChecks { get; set; }

        [JsonPropertyName("totalTransfers")]
        public int TotalTransfers { get; set; }

        [JsonPropertyName("transferTime")]
        public double TransferTime { get; set; }

        [JsonPropertyName("transfers")]
                public int Transfers { get; set; }
        
                [JsonPropertyName("transferring")]
                public RcloneTransferringFile[]? Transferring { get; set; }
            }
        }