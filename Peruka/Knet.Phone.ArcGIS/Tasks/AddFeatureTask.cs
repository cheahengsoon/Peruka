namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Knet.Phone.Client.ArcGIS.Extensions;
    using Knet.Phone.Client.ArcGIS.Serialization;

    /// <summary>
    /// Task for adding features to target FeatureService.
    /// Using REST specification from http://servicesbeta2.esri.com/arcgis/sdk/rest/index.html
    /// TODO : support for multiple graphics
    /// TODO : support for other than 0 index service - could be parsed from the url
    /// TODO : think about edit / delete / add structure behavior and structure
    /// </summary>
    public class AddFeatureTask : TaskBase<AddFeatureParameters>
    {
        public AddFeatureTask(string url)
        {
            this.Url = url;
        }   
        
        public async override Task ExecuteAsync(AddFeatureParameters parameter)
        {
            var url = this.Url + "/applyEdits";

            var graphic = parameter.Graphics[0];

            var serializer = new ArcGISSerializationUtils();
            var graphicJson = serializer.ToJson(graphic);

            var parameters = new Dictionary<string, string>
                                 {
                                     { "id", "0" },
                                     { "adds",  graphicJson},
                                     { "version", "defaultVersion" },
                                     { "rollbackOnFailure", "true" },
                                     { "f", "json" }
                                 };

            WebClient.DisableClientCaching = true;

            try
            {
                // TODO exception handling from result.Result
                var result = await this.WebClient.DownloadStringAsync(url, parameters);              
                Debug.WriteLine(result.Result);
            }
            catch (Exception)
            {
                // TODO handle
                throw;
            }
       }
    }
}
