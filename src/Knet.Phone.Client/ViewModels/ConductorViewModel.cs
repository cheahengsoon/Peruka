namespace Knet.Phone.Client.ViewModels
{
    using System.Threading.Tasks;

    using global::Caliburn.Micro;

    /// <summary>
    /// The base ViewModel that derives from the <see cref="Conductor{T}"/> with <see cref="ScreenViewModel"/>s. 
    /// </summary>
    public abstract class ConductorViewModel : Conductor<ContentScreenViewModel>.Collection.OneActive
    {
        private bool _isBusy;

        private string _busyText;

        private bool _initializationExecuted;

        /// <summary>
        /// Gets or sets a value indicating whether the ViewModel is busy like initializing or searching.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }

            set
            {
                if (value.Equals(this._isBusy))
                {
                    return;
                }

                this._isBusy = value;
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
                return this._busyText;
            }

            set
            {
                if (value == this._busyText)
                {
                    return;
                }

                this._busyText = value;
                this.NotifyOfPropertyChange(() => this.BusyText);
            }
        }

        protected override void OnInitialize()
        {
            if (!this._initializationExecuted)
            {
                this.ExecuteInitialization();
            }

            base.OnInitialize();
        }

        protected virtual async void ExecuteInitialization()
        {
            this._initializationExecuted = true;
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