namespace Peruka.Phone.Client.Core.Services
{
    using System.Device.Location;

    using Knet.Phone.Client.SettingsStore;

    public class SettingsService : IsolatedSettingsStore
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get
            {
                return GetValueOrDefault(string.Empty);
            }
            set
            {
                AddOrUpdateValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the password. 
        /// </summary>
        /// <remarks>
        /// TODO: This solution is not very secure, so check this later.
        /// </remarks>
        public string Password
        {
            get
            {
                return GetValueOrDefault(string.Empty);
            }
            set
            {
                AddOrUpdateValue(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether using location is allowed.
        /// </summary>
        public bool IsLocationServiceAllowed
        {
            get
            {
                return GetValueOrDefault(true);
            }
            set
            {
                AddOrUpdateValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the used accuracy for GPS position. Default is High even it uses more battery.
        /// </summary>
        public GeoPositionAccuracy PositionAccuracy
        {
            get
            {
                return GetValueOrDefault(GeoPositionAccuracy.High);
            }
            set
            {
                AddOrUpdateValue(value);
            }
        }
    }
}