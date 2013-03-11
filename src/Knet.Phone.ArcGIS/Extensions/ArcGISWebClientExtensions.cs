namespace Knet.Phone.Client.ArcGIS.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ESRI.ArcGIS.Client;

    public static class ArcGISWebClientExtensions
    {
        public static Task<ArcGISWebClient.DownloadStringCompletedEventArgs> DownloadStringTaskAsync(
            this ArcGISWebClient client,
            string url,
            Dictionary<string, string> parameters,
            ArcGISWebClient.HttpMethods httpMethod = ArcGISWebClient.HttpMethods.Post)
        {
            var tcs = new TaskCompletionSource<ArcGISWebClient.DownloadStringCompletedEventArgs>();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                    return;
                }

                tcs.SetResult(e);
            };

            client.DownloadStringAsync(new Uri(url), parameters, ArcGISWebClient.HttpMethods.Post);
            return tcs.Task;
        }

        public static Task<ArcGISWebClient.PostMultipartCompletedEventArgs> PostMultipartTaskAsync(
            this ArcGISWebClient client,
            string url,
            Dictionary<string, string> parameters,
            IEnumerable<ArcGISWebClient.StreamContent> contentStream,
            ArcGISWebClient.HttpMethods httpMethod = ArcGISWebClient.HttpMethods.Post)
        {
            var tcs = new TaskCompletionSource<ArcGISWebClient.PostMultipartCompletedEventArgs>();

            client.PostMultipartCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        tcs.SetException(e.Error);
                        return;
                    }

                    tcs.SetResult(e);
                };

            client.PostMultipartAsync(new Uri(url), parameters, contentStream, ArcGISWebClient.HttpMethods.Post);
            return tcs.Task;
        }
    }
}