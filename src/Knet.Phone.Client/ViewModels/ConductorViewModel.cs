namespace Knet.Phone.Client.ViewModels
{
    using System.Threading.Tasks;

    using global::Caliburn.Micro;

    /// <summary>
    /// The base ViewModel that derives from the <see cref="Conductor{T}"/> with <see cref="ScreenViewModel"/>s. 
    /// </summary>
    public abstract class ConductorViewModel : Conductor<ContentScreenViewModel>.Collection.OneActive
    {
        #region Variables

        private bool isBusy;

        private string busyText;

        private bool initializationExecuted;

        #endregion // Variables

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the ViewModel is busy like initializing or searching.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                if (value.Equals(this.isBusy))
                {
                    return;
                }

                this.isBusy = value;
                this.NotifyOfPropertyChange(() => this.IsBusy);
            }
        }

        /// <summary>
        /// Gets or sets the text for the busy indication
        /// </summary>
        public string BusyText
        {
            get
            {
                return this.busyText;
            }

            set
            {
                if (value == this.busyText)
                {
                    return;
                }

                this.busyText = value;
                this.NotifyOfPropertyChange(() => this.BusyText);
            }
        }

        #endregion

        #region Lifecycle events

        protected override void OnInitialize()
        {
            if (!this.initializationExecuted)
            {
                this.ExecuteInitialization();
            }

            base.OnInitialize();
        }

        #endregion // Lifecyce events

        protected virtual async void ExecuteInitialization()
        {
            this.initializationExecuted = true;
        }

        public virtual async Task LoadDataAsync()
        {
            if (this.ActiveItem == null)
            {
                return;
            }

            this.LoadActiveItemsContentAsync("Loading content...");
        }

        private async void LoadActiveItemsContentAsync(string message)
        {
            this.BusyText = message;
            this.IsBusy = true;

            await this.ActiveItem.LoadContentAsync();

            this.IsBusy = false;
            this.BusyText = string.Empty; 
        }
    }
}



