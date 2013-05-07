namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using Newtonsoft.Json;

    public class TaskError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}