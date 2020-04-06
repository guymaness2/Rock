using System.Collections.Generic;

namespace Rock.Logging
{
    public interface IRockLogReader
    {
        int RecordCount { get; }

        List<RockLogEvent> GetEvents( int startIndex, int count );
    }
}