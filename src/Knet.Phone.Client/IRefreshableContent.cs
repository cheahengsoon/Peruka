namespace Knet.Phone.Client
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides interface for the refreshable content.
    /// </summary>
    public interface IRefreshableContent : IContent
    {
        /// <summary>
        /// Refreshes the content.
        /// </summary>
        /// <returns>Returns <see cref="Task"/></returns>
        Task RefreshContentAsync();
    }
}
