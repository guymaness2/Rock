using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

namespace Rock.Logging
{
    internal class RockSerilogReader : IRockLogReader
    {
        private readonly IRockLogger rockLogger;
        private readonly string rockLogDirectory;
        private readonly string searchPattern;
        private readonly JsonSerializer jsonSerializer;

        public int RecordCount
        {
            get
            {
                /*
	                04/05/2020 - MSB 
	                The number of log records could be large and span multiple files we don't want to
                    incur the cost of counting the number of records in the log file because that would
                    mean reading every line in all files. So we are setting this value to a high value, but
                    we can't use int.MaxValue because that appears to cause some sort of issue in
                    the GridView.PageCount property and it would always return 1.
                */
                return 2000000000;
            }
        }

        public RockSerilogReader( IRockLogger logger )
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

        public List<RockLogEvent> GetEvents( int startIndex, int count )
        {
            rockLogger.Close();
            var results = new List<RockLogEvent>();
            var rockLogFiles = System.IO.Directory.GetFiles( rockLogDirectory, searchPattern ).OrderByDescending( s => s ).ToList();

            var currentFileIndex = 0;
            var logs = System.IO.File.ReadAllLines( rockLogFiles[currentFileIndex] );
            //var logs = firstLogFile.Reverse();

            while ( ( startIndex >= logs.Length || startIndex + count >= logs.Length ) && currentFileIndex < ( rockLogFiles.Count - 1 ) )
            {
                currentFileIndex++;
                var additionalLogs = System.IO.File.ReadAllLines( rockLogFiles[currentFileIndex] );
                var temp = new string[additionalLogs.Length + logs.Length];
                additionalLogs.CopyTo( temp, 0 );
                logs.CopyTo( temp, additionalLogs.Length );

                logs = temp;
            }

            var reversedStartIndex = ( logs.Length - 1 ) - startIndex;
            var reversedEndIndex = reversedStartIndex - count;
            for ( var i = reversedStartIndex; i > reversedEndIndex && i >= 0; i-- )
            {
                var evt = LogEventReader.ReadFromString( logs.ElementAt( i ), jsonSerializer );
                var domain = evt.Properties["domain"].ToString();
                evt.RemovePropertyIfPresent( "domain" );
                var message = evt.RenderMessage().Replace( "{domain}", "" ).Trim();
                domain = domain.Replace( "\"", "" );

                results.Add( new RockLogEvent
                {
                    DateTime = evt.Timestamp.DateTime,
                    Exception = evt.Exception,
                    Level = GetRockLogLevelFromSerilogLevel( evt.Level ),
                    Domain = domain,
                    Message = message
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
