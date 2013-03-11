namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Knet.Phone.Client.ArcGIS.Extensions;
    using Knet.Phone.Client.ArcGIS.Serialization;

    /// <summary>
    ///     Task for adding or updating features to target FeatureService.
    ///     Using REST specification from http://servicesbeta2.esri.com/arcgis/sdk/rest/index.html
    ///     TODO : think about edit / delete / add structure behavior and structure
    /// </summary>
    public class AddFeatureTask : TaskBase<AddFeatureParameters, AddFeaturesResult>
    {
        public AddFeatureTask(string url)
        {
            this.Url = url;
        }

        public override async Task<AddFeaturesResult> ExecuteAsync(AddFeatureParameters parameter)
        {
            var url = this.Url + "/applyEdits";

            var parameters = new Dictionary<string, string>
                {
                    { "id", "0" },
                    { "version", "defaultVersion" },
                    { "rollbackOnFailure", "true" },
                    { "f", "json" }
                };

            var serializer = new ArcGISSerializationUtils();

            if (parameter.AddGraphics.Count > 0)
            {
                var graphicJson = serializer.ToJson(parameter.AddGraphics);
                parameters.Add("adds", graphicJson);
            }

            if (parameter.UpdateGraphics.Count > 0)
            {
                var updatesJson = serializer.ToJson(parameter.UpdateGraphics, true, false);
                parameters.Add("updates", updatesJson);
            }

            this.WebClient.DisableClientCaching = true;
            var results = new AddFeaturesResult();

            try
            {
                var result = await this.WebClient.DownloadStringTaskAsync(url, parameters);
                results = AddFeaturesResult.Create(result.Result);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                results.Error = exception;
            }

            return results;
        }
    }
}