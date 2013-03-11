namespace Knet.Phone.Client.ArcGIS.Extensions
{
    using System.Threading.Tasks;

    using ESRI.ArcGIS.Client.Tasks;

    public static class QueryTaskExtensions
    {
        public static Task<QueryEventArgs> ExecuteTaskAsync(this QueryTask task, Query query)
        {
            var tcs = new TaskCompletionSource<QueryEventArgs>();

            task.ExecuteCompleted += (sender, e) => { tcs.SetResult(e); };
            task.Failed += (sender, args) => { tcs.SetException(args.Error); };

            task.ExecuteAsync(query);
            return tcs.Task;
        }
    }
}