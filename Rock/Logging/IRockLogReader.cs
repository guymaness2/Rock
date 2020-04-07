using System.Collections.Generic;

namespace Rock.Logging
{
    /// <summary>
    /// The interface used by the RockLogger for the log reader.
    /// </summary>
    public interface IRockLogReader
    {
        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <value>
        /// The record count.
        /// </value>
        int RecordCount { get; }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        List<RockLogEvent> GetEvents( int startIndex, int count );
    }
}