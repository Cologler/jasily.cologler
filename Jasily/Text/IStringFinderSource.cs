using System;

namespace Jasily.Text
{
    public interface IStringFinderSource
    {
        /// <summary>
        /// cannot change in finder life.
        /// </summary>
        string OriginalString { get; }

        /// <summary>
        /// cannot change in finder life.
        /// </summary>
        StringComparison Comparison { get; }

        /// <summary>
        /// can change in finder life.
        /// </summary>
        int StartIndex { get; }
    }
}