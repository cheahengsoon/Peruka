namespace Knet.Phone.Client.SettingsStore
{
    using System;
    using System.Collections.Generic;
    using System.IO.IsolatedStorage;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides base class for settings store that uses <see cref="IsolatedStorageSettings.ApplicationSettings"/> as a store.
    /// </summary>
    /// <remarks>
    /// Example usage:
    /// <code>
    /// public class PerukaSettingsStore : SettingsStore
    /// {
    ///     public bool Username
    /// {
    ///     get { return this.GetValueOrDefault(true); }
    ///     set { this.AddOrUpdateValue(value); }
    /// }
    /// </code>
    /// </remarks>
    public abstract class IsolatedSettingsStore : SettingsStoreBase
    {
        private readonly IsolatedStorageSettings isolatedStore;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedSettingsStore"/> class. 
        /// </summary>
        protected IsolatedSettingsStore()
        {
            this.isolatedStore = IsolatedStorageSettings.ApplicationSettings;
        }

        #endregion

        /// <summary>
        /// Adds or updates the setting.
        /// </summary>
        /// <param name="value">The value of the setting.</param>
        /// <param name="key">The setting.</param>
        protected override void AddOrUpdateValue(object value, [CallerMemberName]string key = "key")
        {
            var valueChanged = false;

            lock (this)
            {
                try
                {
                    if (value == null)
                    {
                        // Nothing to remove
                        if (!this.isolatedStore.Contains(key))
                        {
                            return;
                        }

                        this.isolatedStore.Remove(key);
                        this.Save();
                    }

                    // If the new value is different, set the new value.
                    if (this.isolatedStore[key] != value)
                    {
                        this.isolatedStore[key] = value;
                        valueChanged = true;
                    }
                }
                catch (KeyNotFoundException)
                {
                    this.isolatedStore.Add(key, value);
                    valueChanged = true;
                }
                catch (ArgumentException)
                {
                    this.isolatedStore.Add(key, value);
                    valueChanged = true;
                }

                if (valueChanged)
                {
                    this.Save();
                }
            }
        }

        /// <summary>
        /// Gets a value for the setting if it has been stored. 
        /// </summary>
        /// <typeparam name="T">The type of the setting.</typeparam>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="key">The name of the setting.</param>
        /// <returns>Returns the stored value if it that was stored, if not returns provided default value.</returns>
        protected override T GetValueOrDefault<T>(T defaultValue, [CallerMemberName]string key = "key")
        {
            lock (this)
            {
                T value;

                try
                {
                    value = (T)this.isolatedStore[key];
                }
                catch (KeyNotFoundException)
                {
                    value = defaultValue;
                }
                catch (ArgumentException)
                {
                    value = defaultValue;
                }

                return value;
            }
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        private void Save()
        {
            try
            {
                this.isolatedStore.Save();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
