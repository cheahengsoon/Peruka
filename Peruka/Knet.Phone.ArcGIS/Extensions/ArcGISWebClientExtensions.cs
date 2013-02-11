namespace Knet.Phone.Client.ArcGIS.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ESRI.ArcGIS.Client;

    public static class ArcGISWebClientExtensions
    {
        /// <summary>
        /// Executes <see cref="ArcGISWebClient.DownloadStringAsync"/> as a Task.
        /// </summary>
        /// <param name="client"> The client. </param>
        /// <param name="url"> The url. </param>
        /// <param name="parameters"> The parameters. </param>
        /// <param name="httpMethod"> The http Method. Default is POST. </param>
        /// <returns>
        /// Returns <see cref="ArcGISWebClient.DownloadStringCompletedEventArgs"/> as via <see cref="Task{T}"/>
        /// </returns>
        public static Task<ArcGISWebClient.DownloadStringCompletedEventArgs> DownloadStringAsync(
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
    }
}
