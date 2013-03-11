namespace Knet.Phone.Client.ViewModels
{
    using global::Caliburn.Micro;

    public abstract class ScreenViewModel : Screen
    {
        private bool _isBusy;

        private string _busyText;

        private bool _backNavSkipOne;

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
        /// Gets or sets the text for the busy indication.
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

        /// <summary>
        /// Gets or sets a value indicating whether previous page is removed the source page from the back stack.
        /// </summary>
        /// <remarks>
        /// Usage :  
        /// <code>
        /// this.navigationService.UriFor{ViewModelWhereToNavigate}().WithParam(p => p.BackNavSkipOne, true).Navigate();
        /// </code>
        /// </remarks>
        public bool BackNavSkipOne
        {
            get
            {
                return this._backNavSkipOne;
            }

            set
            {
                if (value.Equals(this._backNavSkipOne))
                {
                    return;
                }
                this._backNavSkipOne = value;
                this.NotifyOfPropertyChange(() => this.BackNavSkipOne);
            }
        }
    }
}
