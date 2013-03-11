namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using ESRI.ArcGIS.Client;

    using Knet.Phone.Client.ArcGIS.Extensions;

    public class AddAttachementTask : TaskBase<AddAttachementParameters, AddAttachementResults>
    {
        public AddAttachementTask(string url)
        {
            this.Url = url;
        }

        public override async Task<AddAttachementResults> ExecuteAsync(AddAttachementParameters parameter)
        {
            var url = this.Url + "/" + parameter.FeatureId + "/addAttachment";
           
            var parameters = new Dictionary<string, string>
                {
                    { "f", "json" },
                    //{ "attachment", parameter.FeatureId.ToString(CultureInfo.InvariantCulture) + ".jpg" }
                    { "attachment", parameter.Name + ".jpg" }
                };

            this.WebClient.DisableClientCaching = true;

            var results = new AddAttachementResults();

            try
            {
                var contentStreams = new List<ArcGISWebClient.StreamContent>
                    {
                        new ArcGISWebClient.StreamContent()
                            {
                                ContentType = "image/x-jpeg",
                                Filename = parameter.Name.ToString(CultureInfo.InvariantCulture) + ".jpg",
                                Name = parameter.Name,
                                Stream = new MemoryStream(parameter.Attachement)
                            }
                    };

                var result = await this.WebClient.PostMultipartTaskAsync(url, parameters, contentStreams);
                results = AddAttachementResults.Create(result.Result);
                results.FeatureId = parameter.FeatureId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                results.Error = ex;
            }

            return results;
        }
    }
}