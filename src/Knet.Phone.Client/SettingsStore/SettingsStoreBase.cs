namespace Knet.Phone.Client.SettingsStore
{
    using System.Runtime.CompilerServices;

    public abstract class SettingsStoreBase
    {
        /// <summary>
        /// Adds or updates the setting.
        /// </summary>
        /// <param name="value">The value of the setting.</param>
        /// <param name="key">The setting.</param>
        protected abstract void AddOrUpdateValue(object value, [CallerMemberName] string key = "key");

        /// <summary>
        /// Gets a value for the setting if it has been stored. 
        /// </summary>
        /// <typeparam name="T">The type of the setting.</typeparam>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="key">The name of the setting.</param>
        /// <returns>Returns the stored value if it that was stored, if not returns provided default value.</returns>
        protected abstract T GetValueOrDefault<T>(T defaultValue, [CallerMemberName] string key = "key");
    }
}