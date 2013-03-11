namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using Newtonsoft.Json;

    public class UpdateAttachmentResults : TaskResultsBase<UpdateAttachmentResults>
    {
        [JsonProperty("updateAttachmentResult")]
        public UpdateAttachmentResponse Results { get; set; }

        public int FeatureId { get; set; }
    }
}