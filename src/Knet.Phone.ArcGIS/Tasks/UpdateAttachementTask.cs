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

    public class UpdateAttachementTask : TaskBase<UpdateAttachmentParameters, UpdateAttachmentResults>
    {
        public UpdateAttachementTask(string url)
        {
            this.Url = url;
        }

        public override async Task<UpdateAttachmentResults> ExecuteAsync(UpdateAttachmentParameters parameter)
        {
            var url = this.Url + "/" + parameter.FeatureId + "/updateAttachment";

            var parameters = new Dictionary<string, string>
                {
                    { "f", "json" },
                    { "attachmentId", parameter.AttachmentId.ToString(CultureInfo.InvariantCulture) },
                    { "attachment", parameter.Name }
                };

            this.WebClient.DisableClientCaching = true;

            var results = new UpdateAttachmentResults();

            try
            {
                var contentStreams = new List<ArcGISWebClient.StreamContent>
                    {
                        new ArcGISWebClient.StreamContent
                            {
                                ContentType = "image/x-jpeg",
                                Filename = parameter.Name.ToString(CultureInfo.InvariantCulture) + ".jpg",
                                Name = parameter.Name,
                                Stream = new MemoryStream(parameter.Attachment)
                            }
                    };

                var result = await this.WebClient.PostMultipartTaskAsync(url, parameters, contentStreams);

                results = UpdateAttachmentResults.Create(result.Result);
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