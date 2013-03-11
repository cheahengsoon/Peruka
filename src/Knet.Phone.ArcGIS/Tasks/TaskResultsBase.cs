namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using System;
    using System.IO;

    using Newtonsoft.Json;

    public abstract class TaskResultsBase<TResult>
        where TResult : new()
    {
        public Exception Error { get; set; }

        public static TResult Create(string json)
        {
            return JsonConvert.DeserializeObject<TResult>(json);
        }

        public static TResult Create(Stream stream)
        {
            var streamText = new StreamReader(stream).ReadToEnd();
            streamText = streamText.Replace("\r\n", string.Empty);

            return JsonConvert.DeserializeObject<TResult>(streamText);
        }
    }
}