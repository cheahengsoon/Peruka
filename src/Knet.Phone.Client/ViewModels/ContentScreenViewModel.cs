namespace Knet.Phone.Client.ViewModels
{
    using System.Threading.Tasks;

    public abstract class ContentScreenViewModel : ScreenViewModel, IRefreshableContent
    {
        #region Variables

        /// <summary>
        /// Used to define is the <see cref="ExecuteInitialization"/> is called when initialization occurs.
        /// </summary>
        private bool initializationExecuted;

        #endregion // Variables

        #region Properties

        #endregion

        #region Lifecycle events

        /// <summary>
        /// Occurs when the view model is initialized
        /// </summary>
        protected override void OnInitialize()
        {
            if (!this.initializationExecuted)
            {
                this.ExecuteInitialization();
            }

            base.OnInitialize();
        }

        #endregion // Lifecyce events

        /// <summary>
        /// Occurs when the view model is initialized.
        /// </summary>
        protected virtual async void ExecuteInitialization()
        {
            this.initializationExecuted = true;
        }

        #region IContentRefrashable memebers

        /// <summary>
        /// Refreshes content.
        /// </summary>
        /// <returns>Returns <see cref="Task"/></returns>
        public async Task RefreshContentAsync()
        {
            await this.LoadContentAsync();
        }

        /// <summary>
        /// Gets a value indicating whether the content is loaded.
        /// </summary>
        public bool IsContentLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Loads content.
        /// </summary>
        /// <returns>Returns <see cref="Task"/></returns>
        public async Task LoadContentAsync()
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}
