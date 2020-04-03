using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

namespace Rock.Logging
{
    public class RockLogReader
    {
        private readonly IRockLogger rockLogger;
        private readonly string rockLogDirectory;
        private readonly string searchPattern;
        private readonly JsonSerializer jsonSerializer;

        public RockLogReader(IRockLogger logger )
        {
            rockLogger = logger;

            rockLogDirectory = System.IO.Path.GetFullPath( System.IO.Path.GetDirectoryName( rockLogger.LogConfiguration.LogPath ) );

            searchPattern = System.IO.Path.GetFileNameWithoutExtension( rockLogger.LogConfiguration.LogPath );
            searchPattern += "*";
            searchPattern += System.IO.Path.GetExtension( rockLogger.LogConfiguration.LogPath );

            jsonSerializer = JsonSerializer.Create( new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None,
                Culture = CultureInfo.InvariantCulture
            } );
        }

        public List<RockLogEvent> GetEvents(int startIndex, int count )
        {
            rockLogger.Close();
            var results = new List<RockLogEvent>();
            var rockLogFiles = System.IO.Directory.GetFiles( rockLogDirectory, searchPattern ).OrderByDescending( s => s ).ToList();

            var currentFileIndex = 0;
            var logs = System.IO.File.ReadAllLines( rockLogFiles[currentFileIndex] );
            //var logs = firstLogFile.Reverse();

            while((startIndex >= logs.Length || startIndex + count >= logs.Length) && currentFileIndex < (rockLogFiles.Count - 1) )
            {
                currentFileIndex++;
                var additionalLogs = System.IO.File.ReadAllLines( rockLogFiles[currentFileIndex] );
                var temp = new string[additionalLogs.Length + logs.Length];
                additionalLogs.CopyTo( temp, 0 );
                logs.CopyTo( temp, additionalLogs.Length );

                logs = temp;
            }

            var reversedStartIndex = (logs.Length - 1) - startIndex;
            var reversedEndIndex = reversedStartIndex - count;
            for (var i = reversedStartIndex; i > reversedEndIndex && i >= 0; i-- )
            {
                var evt = LogEventReader.ReadFromString( logs.ElementAt( i ), jsonSerializer );

                results.Add( new RockLogEvent
                {
                    DateTime = evt.Timestamp.DateTime,
                    Exception = evt.Exception,
                    Level = GetRockLogLevelFromSerilogLevel( evt.Level ),
                    Message = evt.RenderMessage()
                } );
            }
            //using ( var clef = File.OpenText( rockConfiguration.LogPath ) )
            //{
            //    var reader = new LogEventReader( clef );

            //    LogEvent evt;
            //    while ( reader.TryRead( out evt ) )
            //    {
            //        results.Add( new RockLogEvent
            //        {
            //            DateTime = evt.Timestamp.DateTime,
            //            Exception = evt.Exception,
            //            Level = GetRockLogLevelFromSerilogLevel(evt.Level),
            //            Message = evt.RenderMessage()
            //        } );
            //    }   
            //}

            return results;
        }
        private RockLogLevel GetRockLogLevelFromSerilogLevel( LogEventLevel logLevel )
        {
            switch ( logLevel )
            {
                case ( LogEventLevel.Error ):
                    return RockLogLevel.Error;
                case ( LogEventLevel.Warning ):
                    return RockLogLevel.Warning;
                case ( LogEventLevel.Information ):
                    return RockLogLevel.Info;
                case ( LogEventLevel.Debug ):
                    return RockLogLevel.Debug;
                case ( LogEventLevel.Verbose ):
                    return RockLogLevel.All;
                default:
                    return RockLogLevel.Fatal;
            }
        }

    }
}
