namespace Knet.Phone.Client.ViewModels
{
    using global::Caliburn.Micro;

    public abstract class ScreenViewModel : Screen
    {
        #region Variables

        private bool isBusy;

        private string busyText;

        private bool backNavSkipOne;

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
        /// Gets or sets the text for the busy indication.
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
                return this.backNavSkipOne;
            }

            set
            {
                if (value.Equals(this.backNavSkipOne))
                {
                    return;
                }
                this.backNavSkipOne = value;
                this.NotifyOfPropertyChange(() => this.BackNavSkipOne);
            }
        }

        #endregion
    }
}
