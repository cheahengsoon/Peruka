namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using Newtonsoft.Json;

    public class UpdateAttachmentResponse
    {
        [JsonProperty("objectId")]
        public int AttachmentId { get; set; }

        [JsonProperty("globalId")]
        public int? GlobalId { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}