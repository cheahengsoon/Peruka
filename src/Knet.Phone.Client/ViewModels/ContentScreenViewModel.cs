namespace Knet.Phone.Client.ViewModels
{
    using System.Threading.Tasks;

    public abstract class ContentScreenViewModel : ScreenViewModel, IRefreshableContent
    {
        /// <summary>
        /// Used to define is the <see cref="ExecuteInitialization"/> is called when initialization occurs.
        /// </summary>
        private bool _initializationExecuted;

        /// <summary>
        /// Occurs when the view model is initialized
        /// </summary>
        protected override void OnInitialize()
        {
            if (!this._initializationExecuted)
            {
                this.ExecuteInitialization();
            }

            base.OnInitialize();
        }

        /// <summary>
        /// Occurs when the view model is initialized.
        /// </summary>
        protected virtual async void ExecuteInitialization()
        {
            this._initializationExecuted = true;
        }

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
        public bool IsContentLoaded { get; private set; }

        /// <summary>
        /// Loads content.
        /// </summary>
        /// <returns>Returns <see cref="Task"/></returns>
        public async Task LoadContentAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}