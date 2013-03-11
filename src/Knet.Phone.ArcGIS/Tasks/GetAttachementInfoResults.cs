namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class GetAttachementInfoResults : TaskResultsBase<GetAttachementInfoResults>
    {
        public GetAttachementInfoResults()
        {
            this.Attachements = new List<AttachementInfo>();
        }

        [JsonProperty("attachmentInfos")]
        public List<AttachementInfo> Attachements { get; set; }

        public int FeatureId { get; set; }
    }
}