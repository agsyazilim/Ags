using Ags.Data.Domain.Stores;

namespace Ags.Data.Common
{
    /// <summary>
    /// Store context
    /// </summary>
    public interface IStoreContext
    {
        /// <summary>
        /// Gets the current store
        /// </summary>
        Store CurrentStore { get; }


    }
}
