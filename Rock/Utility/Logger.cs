using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Rock.Utility
{
    public class Logger
    {
        private static Logger log;
        public static Logger Log {
            get
            {
                if(log == null )
                {
                    log = new Logger();
                }
                return log;
            }
        }

        private Logger()
        {
           Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel
                .Verbose()
                .WriteTo.File( "logs\\rock.log",
                    rollingInterval: RollingInterval.Day,
                    buffered: true,
                    shared: false,                    
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                    retainedFileCountLimit: 1,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 5000000)
                .CreateLogger();
        }

        ~Logger()
        {
            Serilog.Log.CloseAndFlush();
        }

        public void Information(string message)
        {
            Serilog.Log.Logger.Information( message );
            Debug.WriteLine( message );
        }

        public void Information( string domain, string message )
        {
            Serilog.Log.Logger.Information("{domain} {message}", domain, message );
            Debug.WriteLine( $"{domain} {message}" );
        }
    }
}
