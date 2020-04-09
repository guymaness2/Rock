// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using Serilog;
using Serilog.Configuration;

namespace Rock.Logging
{
    /// <summary>
    /// This class is used to add custom Rock code to the serilog implementations.
    /// </summary>
    internal static class RockSerilogExtensions
    {
        /// <summary>
        /// Gets set the serilog minimum log level from configuration the RockLogLevel.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="logLevel">The log level.</param>
        /// <returns></returns>
        public static LoggerConfiguration GetFromConfiguration( this LoggerMinimumLevelConfiguration min, RockLogLevel logLevel )
        {
            switch ( logLevel )
            {
                case RockLogLevel.All:
                    return min.Verbose();
                case RockLogLevel.Debug:
                    return min.Debug();
                case RockLogLevel.Error:
                    return min.Error();
                case RockLogLevel.Info:
                    return min.Information();
                case RockLogLevel.Warning:
                    return min.Warning();
                default:
                    return min.Fatal();
            }
        }
    }
}
