namespace StellarDuelist.Core.Collections
{
    /// <summary>
    /// An interface for objects that can be reset and reused in an object pool.
    /// </summary>
    public interface IPoolableObject
    {
        /// <summary>
        /// Resets the object to its initial state, preparing it for reuse.
        /// </summary>
        void Reset();
    }
}
