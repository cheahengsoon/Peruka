namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Knet.Phone.Client.ArcGIS.Extensions;

    public class GetAttachementInfoTask : TaskBase<GetAttachementInfoParameters, GetAttachementInfoResults>
    {
        public GetAttachementInfoTask(string url)
        {
            this.Url = url;
        }

        public override async Task<GetAttachementInfoResults> ExecuteAsync(GetAttachementInfoParameters parameter)
        {
            var url = this.Url + "/" + parameter.FeatureId + "/attachments";

            var parameters = new Dictionary<string, string> { { "f", "json" }, };

            this.WebClient.DisableClientCaching = true;

            var results = new GetAttachementInfoResults();

            try
            {
                var result = await this.WebClient.DownloadStringTaskAsync(url, parameters);

                results = GetAttachementInfoResults.Create(result.Result);
                results.FeatureId = parameter.FeatureId;
            }
            catch (Exception exception) 
            {
                results.Error = exception;
            }

            return results;
        }
    }
}