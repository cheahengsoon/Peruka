namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using System.Threading.Tasks;

    using ESRI.ArcGIS.Client;

    public abstract class TaskBase<TParameters, TResults>
    {
        protected TaskBase()
        {
            this.WebClient = new ArcGISWebClient();
        }

        /// <summary>
        ///     Gets or sets the target service url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Gets the <see cref="ArcGISWebClient" /> used.
        /// </summary>
        protected ArcGISWebClient WebClient { get; private set; }

        /// <summary>
        ///     Executes task.
        /// </summary>
        /// <returns>
        ///     Returns <see cref="Task" />
        /// </returns>
        public abstract Task<TResults> ExecuteAsync(TParameters parameters);
    }
}