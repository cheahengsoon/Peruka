namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using Newtonsoft.Json;

    public class AddAttachementResults : TaskResultsBase<AddAttachementResults>
    {
        [JsonProperty("addAttachmentResult")]
        public UpdateAttachmentResponse Results { get; set; }

        public int FeatureId { get; set; }
    }
}