namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using System.Threading.Tasks;

    using ESRI.ArcGIS.Client;

    public abstract class TaskBase<T>
    {
        #region Variables

        #endregion // Variables

        #region Constructor

        protected TaskBase()
        {
            this.WebClient = new ArcGISWebClient();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the target service url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets the <see cref="ArcGISWebClient"/> used.
        /// </summary>
        protected ArcGISWebClient WebClient { get;  private set; }

        #endregion 

        /// <summary>
        /// Executes task.
        /// </summary>
        /// <returns>Returns <see cref="Task"/></returns>
        public abstract Task ExecuteAsync(T parameters);
    }
}
