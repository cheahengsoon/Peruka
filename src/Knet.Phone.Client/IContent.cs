namespace Knet.Phone.Client
{
    using System.Threading.Tasks;

    public interface IContent
    {
        /// <summary>
        /// Gets a value indicating whether the content is loaded.
        /// </summary>
        bool IsContentLoaded { get; }

        /// <summary>
        /// Loads content.
        /// </summary>
        /// <returns>Returns <see cref="Task"/></returns>
        Task LoadContentAsync();
    }
}