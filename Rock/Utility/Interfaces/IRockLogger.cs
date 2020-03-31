using System;

namespace Rock.Utility
{
    public interface IRockLogger
    {
        IRockLogConfiguration LogConfiguration { get; }

        void Close();
        void Debug( Exception exception, string messageTemplate );
        void Debug( Exception exception, string messageTemplate, params object[] propertyValues );
        void Debug( string messageTemplate );
        void Debug( string domain, Exception exception, string messageTemplate );
        void Debug( string domain, Exception exception, string messageTemplate, params object[] propertyValues );
        void Debug( string messageTemplate, params object[] propertyValues );
        void Debug( string domain, string messageTemplate );
        void Debug( string domain, string messageTemplate, params object[] propertyValues );
        void Error( Exception exception, string messageTemplate );
        void Error( Exception exception, string messageTemplate, params object[] propertyValues );
        void Error( string messageTemplate );
        void Error( string domain, Exception exception, string messageTemplate );
        void Error( string domain, Exception exception, string messageTemplate, params object[] propertyValues );
        void Error( string messageTemplate, params object[] propertyValues );
        void Error( string domain, string messageTemplate );
        void Error( string domain, string messageTemplate, params object[] propertyValues );
        void Fatal( Exception exception, string messageTemplate );
        void Fatal( Exception exception, string messageTemplate, params object[] propertyValues );
        void Fatal( string messageTemplate );
        void Fatal( string domain, Exception exception, string messageTemplate );
        void Fatal( string domain, Exception exception, string messageTemplate, params object[] propertyValues );
        void Fatal( string messageTemplate, params object[] propertyValues );
        void Fatal( string domain, string messageTemplate );
        void Fatal( string domain, string messageTemplate, params object[] propertyValues );
        void Information( Exception exception, string messageTemplate );
        void Information( Exception exception, string messageTemplate, params object[] propertyValues );
        void Information( string messageTemplate );
        void Information( string domain, Exception exception, string messageTemplate );
        void Information( string domain, Exception exception, string messageTemplate, params object[] propertyValues );
        void Information( string messageTemplate, params object[] propertyValues );
        void Information( string domain, string messageTemplate );
        void Information( string domain, string messageTemplate, params object[] propertyValues );
        void Verbose( Exception exception, string messageTemplate );
        void Verbose( Exception exception, string messageTemplate, params object[] propertyValues );
        void Verbose( string messageTemplate );
        void Verbose( string domain, Exception exception, string messageTemplate );
        void Verbose( string domain, Exception exception, string messageTemplate, params object[] propertyValues );
        void Verbose( string messageTemplate, params object[] propertyValues );
        void Verbose( string domain, string messageTemplate );
        void Verbose( string domain, string messageTemplate, params object[] propertyValues );
        void Warning( Exception exception, string messageTemplate );
        void Warning( Exception exception, string messageTemplate, params object[] propertyValues );
        void Warning( string messageTemplate );
        void Warning( string domain, Exception exception, string messageTemplate );
        void Warning( string domain, Exception exception, string messageTemplate, params object[] propertyValues );
        void Warning( string messageTemplate, params object[] propertyValues );
        void Warning( string domain, string messageTemplate );
        void Warning( string domain, string messageTemplate, params object[] propertyValues );
        void WriteToLog( RockLogLevel logLevel, Exception exception, string messageTemplate );
        void WriteToLog( RockLogLevel logLevel, Exception exception, string messageTemplate, params object[] propertyValues );
        void WriteToLog( RockLogLevel logLevel, Exception exception, string domain, string messageTemplate );
        void WriteToLog( RockLogLevel logLevel, Exception exception, string domain, string messageTemplate, params object[] propertyValues );
        void WriteToLog( RockLogLevel logLevel, string messageTemplate );
        void WriteToLog( RockLogLevel logLevel, string messageTemplate, params object[] propertyValues );
        void WriteToLog( RockLogLevel logLevel, string domain, string messageTemplate );
        void WriteToLog( RockLogLevel logLevel, string domain, string messageTemplate, params object[] propertyValues );
    }
}