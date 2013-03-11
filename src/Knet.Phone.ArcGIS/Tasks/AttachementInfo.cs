namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using Newtonsoft.Json;

    public class AttachementInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}