namespace Peruka.Phone.Client.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using Caliburn.Micro;

    using ESRI.ArcGIS.Client;
    using ESRI.ArcGIS.Client.Portal;
    using ESRI.ArcGIS.Client.WebMap;

    using Knet.Phone.Client.ArcGIS.Extensions;

    public class PortalService : PropertyChangedBase
    {
        private readonly ArcGISPortal portal;

        private IdentityManager.Credential credentials;

        private ArcGISPortalUser userInformation;

        public PortalService()
        {
            this.portal = new ArcGISPortal();
        }

        #region Properties

        /// <summary>
        /// Gets the users information
        /// </summary>
        public ArcGISPortalUser UserInformation
        {
            get
            {
                return this.userInformation;
            }

            private set
            {
                if (Equals(value, this.userInformation))
                {
                    return;
                }

                this.userInformation = value;
                this.NotifyOfPropertyChange(() => this.UserInformation);
            }
        }

        #endregion // Properties

        /// <summary>
        /// Initializes the portal.
        /// </summary>
        public async Task InitializeAsync(string username, string password)
        {
            // If already initialized, dont do anything
            if (this.portal.IsInitialized)
            {
                return;
            }

            try
            {
                var credential = await IdentityManager.Current.LoginAsync(username, password);

                if (credential != null)
                {
                    this.credentials = credential;
                    this.StartTokenRefresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Debug.WriteLine(ex);
                throw;
            }

            await this.portal.InitializeAsync();
            this.UserInformation = this.portal.CurrentUser;
        }

        public async Task<IEnumerable<ArcGISPortalItem>> LoadBasemapGalleryAsync(int limit = 10)
        {
            var parameters = new SearchParameters() { Limit = limit };

            // Load ArcGISPortalItems from the Online that contains basemaps.
            var items = await this.portal.LoadBasemapGalleryAsync(parameters);

            // Filter away Bing maps since we don't have a token.
            return items.Where(l => !l.Name.Contains("Bing"));
        }

        /// <summary>
        /// Get items from ArcGIS Portal by query clause.
        /// </summary>
        /// <param name="query">The clause that is used in the query.</param>
        /// <returns>Returns items that were found./></returns>
        public async Task<IEnumerable<ArcGISPortalItem>> GetItemsAsync(string query)
        {
            var parameters = new SearchParameters
            {
                QueryString = query
            };

            return await this.portal.SearchItemsAsync(parameters);
        }

        public async Task<GetMapCompletedEventArgs> LoadWebmapByIdAsync(string id)
        {
            var webMapDocument = new Document();

            try
            {
                webMapDocument.Token = this.credentials.Token;
                return await webMapDocument.LoadWebMapAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ArcGISPortalItem>> GetNewestOrganizationalItemsAsync(int limit)
        {
            if (!this.portal.IsInitialized)
            {
                return null;
            }

            var parameters = new SearchParameters
            {
                Limit = limit,
                QueryString = string.Format("accountid:\"{0}\" AND type:\"Web Map\" AND -type:\"Web Mapping Application\"", this.UserInformation.OrgId),
                SortField = "modified",
                SortOrder = QuerySortOrder.Descending
            };

            try
            {
                return await this.portal.SearchItemsAsync(parameters);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ArcGISPortalItem>> GetMostUsedWebMapsFromOrganizationAsync(int limit)
        {
            if (!this.portal.IsInitialized)
            {
                return null;
            }

            var parameters = new SearchParameters
            {
                Limit = limit,
                QueryString = string.Format("accountid:\"{0}\" AND type:\"Web Map\" AND -type:\"Web Mapping Application\"", this.UserInformation.OrgId),
                SortField = "numviews",
                SortOrder = QuerySortOrder.Descending
            };

            try
            {
                return await this.portal.SearchItemsAsync(parameters);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ArcGISPortalItem>> GetFeaturedWebMapsAsync(int limit)
        {
            var parameters = new SearchParameters
            {
                Limit = limit,
                QueryString = string.Format("type:\"Web Map\" AND -type:\"Web Mapping Application\""),
                SortField = "modified",
                SortOrder = QuerySortOrder.Descending
            };

            try
            {
                return await this.portal.ArcGISPortalInfo.SearchFeaturedItemsAsync(parameters);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Starts handling refreshing Token.
        /// </summary>
        private void StartTokenRefresh()
        {
            this.credentials.PropertyChanged += this.HandleTokenRefreshed;
        }

        /// <summary>
        /// Stops handling token refreshing.
        /// </summary>
        private void StopTokenRefresh()
        {
            this.credentials.PropertyChanged -= this.HandleTokenRefreshed;
        }

        /// <summary>
        /// Handles token refreshing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The event args.</param>
        private void HandleTokenRefreshed(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Token")
            {
                if (this.credentials.Token != null)
                {
                    this.portal.Token = this.credentials.Token;
                }
            }
        }
    }
}
