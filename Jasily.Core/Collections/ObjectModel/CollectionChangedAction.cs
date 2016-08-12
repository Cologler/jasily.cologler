namespace System.Collections.ObjectModel
{
    public enum CollectionChangedAction
    {
        /// <summary>
        /// collection was clear.
        /// </summary>
        Reset = 0,

        /// <summary>
        /// collection was rebuild.
        /// </summary>
        Initialized = 1,

        /// <summary>
        /// collection was added one or more items.
        /// </summary>
        Add = 2,

        /// <summary>
        /// collection was removed one or more items.
        /// </summary>
        Remove = 3,
    }
}