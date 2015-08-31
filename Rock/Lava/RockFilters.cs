﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using DDay.iCal;
using DotLiquid;
using DotLiquid.Util;
using Humanizer;
using Humanizer.Localisation;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;

namespace Rock.Lava
{
    /// <summary>
    /// 
    /// </summary>
    public static class RockFilters
    {
        #region String Filters

        /// <summary>
        /// obfuscate a given email
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ObfuscateEmail( string input )
        {
            if ( input == null )
            {
                return null;
            }
            else
            {
                string[] emailParts = input.Split('@');

                if ( emailParts.Length != 2 )
                {
                    return input;
                }
                else
                {
                    return string.Format( "{0}xxxxx@{1}", emailParts[0].Substring( 0, 1 ), emailParts[1] );
                }
            }
        }

        /// <summary>
        /// pluralizes string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Pluralize( string input )
        {
            return input == null
                ? input
                : input.Pluralize();
        }

        /// <summary>
        /// Possessives the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string Possessive( string input )
        {
            if ( input == null )
            {
                return input;
            }

            if ( input.EndsWith( "s" ) )
            {
                return input + "'";
            }
            else
            {
                return input + "'s";
            }
        }

        /// <summary>
        /// pluralizes string based on the value for quantity
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="quantity">The quantity.</param>
        /// <returns></returns>
        public static string PluralizeForQuantity( string input, int quantity )
        {
            if ( input == null )
            {
                return input;
            }

            if ( quantity > 1 )
            {
                return input.Pluralize();
            }
            else {
                return input;
            }
        }

        /// <summary>
        /// convert a integer to a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToString( int input )
        {
            return input.ToString();
        }

        /// <summary>
        /// singularize string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Singularize( string input )
        {
            return input == null
                ? input
                : input.Singularize();
        }

        /// <summary>
        /// takes computer-readible-formats and makes them human readable
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Humanize( string input )
        {
            return input == null
                ? input
                : input.Humanize();
        }

        /// <summary>
        /// returns sentence in 'Title Case'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TitleCase( string input )
        {
            return input == null
                ? input
                : input.Titleize();
        }

        /// <summary>
        /// returns sentence in 'PascalCase'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToPascal( string input )
        {
            return input == null
                ? input
                : input.Dehumanize();
        }

        /// <summary>
        /// returns sentence in 'PascalCase'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToCssClass( string input )
        {
            return input == null
                ? input
                : input.ToLower().Replace( " ", "-" );
        }

        /// <summary>
        /// returns sentence in 'Sentence case'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SentenceCase( string input )
        {
            return input == null
                ? input
                : input.Transform( To.SentenceCase );
        }

        /// <summary>
        /// takes 1, 2 and returns 1st, 2nd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NumberToOrdinal( string input )
        {
            if ( input == null )
                return input;

            int number;

            if ( int.TryParse( input, out number ) )
            {
                return number.Ordinalize();
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// takes 1,2 and returns one, two
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NumberToWords( string input )
        {
            if ( input == null )
                return input;

            int number;

            if ( int.TryParse( input, out number ) )
            {
                return number.ToWords();
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// takes 1,2 and returns first, second
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NumberToOrdinalWords( string input )
        {
            if ( input == null )
                return input;

            int number;

            if ( int.TryParse( input, out number ) )
            {
                return number.ToOrdinalWords();
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// takes 1,2 and returns I, II, IV
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NumberToRomanNumerals( string input )
        {
            if ( input == null )
                return input;

            int number;

            if ( int.TryParse( input, out number ) )
            {
                return number.ToRoman();
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// formats string to be appropriate for a quantity
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="quantity">The quantity.</param>
        /// <returns></returns>
        public static string ToQuantity( string input, int quantity )
        {
            return input == null
                ? input
                : input.ToQuantity( quantity );
        }

        /// <summary>
        /// Replace occurrences of a string with another - this is a Rock version on this filter which takes any object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="string"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string Replace( object input, object @string, object replacement = null )
        {
            if ( input == null )
            {
                return string.Empty;
            }
            
            string inputAsString = input.ToString();

            string replacementString = replacement.ToString();
            string pattern = Regex.Escape( @string.ToString() );

            /*// escape common regex meta characters
            var listOfRegExChars = new List<string> { ".", "$", "{", "}", "^", "[", "]", "*", @"\", "+", "|", "?", "<", ">" };
            if ( listOfRegExChars.Contains( @string ) )
            {
                @string = @"\" + @string;
            }*/

            if ( string.IsNullOrEmpty( inputAsString ) || string.IsNullOrEmpty( pattern ) )
                return inputAsString;

            return string.IsNullOrEmpty( inputAsString )
                ? inputAsString
                : Regex.Replace( inputAsString, pattern, replacementString );
        }

        /// <summary>
        /// Replace the first occurence of a string with another - this is a Rock version on this filter which takes any object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="string"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceFirst( object input, string @string, string replacement = "" )
        {
            if ( input == null )
            {
                return string.Empty;
            }
            
            string inputAsString = input.ToString();

            if ( string.IsNullOrEmpty( inputAsString ) || string.IsNullOrEmpty( @string ) )
                return inputAsString;

            // escape common regex meta characters
            var listOfRegExChars = new List<string> { ".", "$", "{", "}", "^", "[", "]", "*", @"\", "+", "|", "?", "<", ">" };
            if ( listOfRegExChars.Contains( @string ) )
            {
                @string = @"\" + @string;
            }

            bool doneReplacement = false;
            return Regex.Replace( inputAsString, @string, m =>
            {
                if ( doneReplacement )
                    return m.Value;

                doneReplacement = true;
                return replacement;
            } );
        }

        /// <summary>
        /// Replace the last occurence of a string with another - this is a Rock version on this filter which takes any object
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="search">The search.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string ReplaceLast( object input, string search, string replacement = "" )
        {
            if ( input == null )
            {
                return string.Empty;
            }

            string inputAsString = input.ToString();

            if ( string.IsNullOrEmpty( inputAsString ) || string.IsNullOrEmpty( search ) )
                return inputAsString;

            int place = inputAsString.LastIndexOf( search );
            if ( place > 0 )
            {
                return inputAsString.Remove( place, search.Length ).Insert( place, replacement );
            }
            else
            {
                return input.ToString();
            }
            
        }

        /// <summary>
        /// Remove a substring - this is a Rock version on this filter which takes any object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string Remove( object input, string @string )
        {
            if ( input == null )
            {
                return string.Empty;
            }
            
            string inputAsString = input.ToString();

            return string.IsNullOrWhiteSpace( inputAsString )
                ? inputAsString
                : inputAsString.Replace( @string, string.Empty );
        }

        /// <summary>
        /// Trims the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string Trim( object input )
        {
            if ( input == null )
            {
                return string.Empty;
            }

            return input.ToString().Trim();
        }

        /// <summary>
        /// Remove the first occurrence of a substring - this is a Rock version on this filter which takes any object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string RemoveFirst( object input, string @string )
        {
            if ( input == null )
            {
                return string.Empty;
            }
            
            string inputAsString = input.ToString();

            return string.IsNullOrWhiteSpace( inputAsString )
                ? inputAsString
                : ReplaceFirst( inputAsString, @string, string.Empty );
        }

        /// <summary>
        /// Appends the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="string">The string.</param>
        /// <returns></returns>
        public static string Append( object input, string @string )
        {
            if ( input == null )
            {
                return string.Empty;
            }
            
            string inputAsString = input.ToString();

            return inputAsString == null
                ? inputAsString
                : inputAsString + @string;
        }

        /// <summary>
        /// Prepend a string to another - this is a Rock version on this filter which takes any object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string Prepend( object input, string @string )
        {
            if ( input == null )
            {
                return string.Empty;
            }
            
            string inputAsString = input.ToString();

            return inputAsString == null
                ? inputAsString
                : @string + inputAsString;
        }

        /// <summary>
        /// Returns the passed default value if the value is undefined or empty, otherwise the value of the variable
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultString">The default string.</param>
        /// <returns></returns>
        public static string Default( object input, string defaultString )
        {

            if ( input == null ) {
                return defaultString;
            }

            string inputAsString = input.ToString();

            return string.IsNullOrWhiteSpace( inputAsString )
                ? defaultString
                : inputAsString;
        }

        /// <summary>
        /// Decodes an HTML string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string HtmlDecode( string input )
        {
            if ( input == null )
            {
                return null;
            }
            else
            {
                return HttpUtility.HtmlDecode( input );
            }
        }

        #endregion

        #region DateTime Filters

        /// <summary>
        /// Formats a date using a .NET date format string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Date( object input, string format )
        {
            if ( input == null )
                return null;

            if ( input.ToString() == "Now" )
            {
                input = RockDateTime.Now.ToString();
            }

            if ( string.IsNullOrWhiteSpace( format ) )
                return input.ToString();

            // if format string is one character add a space since a format string can't be a single character http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx#UsingSingleSpecifiers
            if ( format.Length == 1 )
            {
                format = " " + format;
            }

            DateTime date;

            return DateTime.TryParse( input.ToString(), out date )
                ? Liquid.UseRubyDateFormat ? date.ToStrFTime( format ).Trim() : date.ToString( format ).Trim()
                : input.ToString().Trim();
        }


        /// <summary>
        /// Dateses from i cal.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="returnCount">The return count.</param>
        /// <returns></returns>
        public static List<DateTime> DatesFromICal( object input, int returnCount = 1 )
        {
            List<DateTime> nextOccurrences = new List<DateTime>();
            
            if ( input is string )
            {
                nextOccurrences = GetOccurrenceDates( (string)input, returnCount );
            }
            else if ( input is IList )
            {
                foreach ( var item in input as IList )
                {
                    if ( item is string )
                    {
                        nextOccurrences.AddRange( GetOccurrenceDates( (string)item, returnCount ) );
                    }
                }
            }

            nextOccurrences.Sort((a,b) => a.CompareTo(b));

            return nextOccurrences.Take( returnCount ).ToList();
        }

        /// <summary>
        /// Gets the occurrence dates.
        /// </summary>
        /// <param name="iCalString">The i cal string.</param>
        /// <param name="returnCount">The return count.</param>
        /// <returns></returns>
        private static List<DateTime> GetOccurrenceDates( string iCalString, int returnCount )
        {
            iCalendar calendar = iCalendar.LoadFromStream( new StringReader( iCalString ) ).First() as iCalendar;
            DDay.iCal.Event calendarEvent = calendar.Events[0] as Event;

            if ( calendarEvent.DTStart != null )
            {
                List<Occurrence> dates = calendar.GetOccurrences( RockDateTime.Now, RockDateTime.Now.AddYears( 1 ) ).Take( returnCount ).ToList();
                return dates.Select( d => d.Period.StartTime.Value ).ToList();
            }
            else
            {
                return new List<DateTime>();
            }
        }

        /// <summary>
        /// Adds a time interval to a date
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="interval">The interval.</param>
        /// <returns></returns>
        public static DateTime? DateAdd( object input, int amount, string interval = "d" )
        {
            DateTime? date = null;
            
            if ( input == null )
                return null;

            if ( input.ToString() == "Now" )
            {
                date = RockDateTime.Now;
            }
            else
            {
                DateTime d;
                bool success = DateTime.TryParse( input.ToString(), out d );
                if ( success )
                {
                    date = d;
                }
            }

            if ( date.HasValue )
            {
                TimeSpan timeInterval = new TimeSpan();
                switch ( interval )
                {
                    case "d":
                        timeInterval = new TimeSpan(amount, 0, 0, 0);
                        break;
                    case "h":
                        timeInterval = new TimeSpan( 0, amount, 0, 0 );
                        break;
                    case "m":
                        timeInterval = new TimeSpan(0, 0, amount, 0);
                        break;
                    case "s":
                        timeInterval = new TimeSpan(0, 0, 0, amount);
                        break;
                }
                
                date = date.Value.Add(timeInterval);
            }

            return date;
        }

        /// <summary>
        /// takes a date time and compares it to RockDateTime.Now and returns a human friendly string like 'yesterday' or '2 hours ago'
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns></returns>
        public static string HumanizeDateTime( object input, object compareDate = null )
        {
            if ( input == null )
                return string.Empty;

            DateTime dtInput;
            DateTime dtCompare;

            if ( input != null && input is DateTime )
            {
                dtInput = (DateTime)input;
            }
            else
            {
                if ( input == null || !DateTime.TryParse( input.ToString(), out dtInput ) )
                {
                    return string.Empty;
                }
            }

            if ( compareDate == null || !DateTime.TryParse( compareDate.ToString(), out dtCompare ) )
            {
                dtCompare = RockDateTime.Now;
            }

            return dtInput.Humanize( false, dtCompare );

        }

        /// <summary>
        /// takes two datetimes and humanizes the difference like '1 day'. Supports 'Now' as end date
        /// </summary>
        /// <param name="sStartDate">The s start date.</param>
        /// <param name="sEndDate">The s end date.</param>
        /// <param name="precision">The precision.</param>
        /// <returns></returns>
        public static string HumanizeTimeSpan( object sStartDate, object sEndDate, int precision = 1 )
        {
            DateTime startDate = GetDateFromObject(sStartDate);
            DateTime endDate = GetDateFromObject(sEndDate);

            if ( startDate != DateTime.MinValue && endDate != DateTime.MinValue )
            {
                TimeSpan difference = endDate - startDate;
                return difference.Humanize( precision );
            }
            else
            {
                return "Could not parse one or more of the dates provided into a valid DateTime";
            }
        }

        /// <summary>
        /// Humanizes the time span.
        /// </summary>
        /// <param name="sStartDate">The s start date.</param>
        /// <param name="sEndDate">The s end date.</param>
        /// <param name="unit">The minimum unit.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public static string HumanizeTimeSpan( object sStartDate, object sEndDate, string unit, string direction )
        {
            DateTime startDate = GetDateFromObject( sStartDate );
            DateTime endDate = GetDateFromObject( sEndDate );

            TimeUnit minUnitValue = TimeUnit.Day;

            switch(unit)
            {
                case "Year":
                    minUnitValue = TimeUnit.Year;
                    break;
                case "Month":
                    minUnitValue = TimeUnit.Month;
                    break;
                case "Week":
                    minUnitValue = TimeUnit.Week;
                    break;
                case "Day":
                    minUnitValue = TimeUnit.Day;
                    break;
                case "Hour":
                    minUnitValue = TimeUnit.Hour;
                    break;
                case "Minute":
                    minUnitValue = TimeUnit.Minute;
                    break;
                case "Second":
                    minUnitValue = TimeUnit.Second;
                    break;
            }

            if ( startDate != DateTime.MinValue && endDate != DateTime.MinValue )
            {
                TimeSpan difference = endDate - startDate;
                
                if (direction.ToLower() == "max") {
                    return difference.Humanize( maxUnit: minUnitValue );
                } else {
                    return difference.Humanize( minUnit: minUnitValue );
                }
            }
            else
            {
                return "Could not parse one or more of the dates provided into a valid DateTime";
            }
        }

        /// <summary>
        /// Gets the date from object.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private static DateTime GetDateFromObject( object date )
        {
            DateTime oDateTime = DateTime.MinValue;
            
            if ( date is String )
            {
                if ( (string)date == "Now" )
                {
                    return RockDateTime.Now;
                }
                else
                {
                    if ( DateTime.TryParse( (string)date, out oDateTime ) )
                    {
                        return oDateTime;
                    }
                    else
                    {
                        return DateTime.MinValue;
                    }
                }
            }
            else if ( date is DateTime )
            {
                return (DateTime)date;
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// takes two datetimes and returns the difference in the unit you provide
        /// </summary>
        /// <param name="sStartDate">The s start date.</param>
        /// <param name="sEndDate">The s end date.</param>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Int64? DateDiff( object sStartDate, object sEndDate, string unit )
        {
            DateTime startDate = GetDateFromObject( sStartDate );
            DateTime endDate = GetDateFromObject( sEndDate );

            if ( startDate != DateTime.MinValue && endDate != DateTime.MinValue )
            {
                TimeSpan difference = endDate - startDate;

                switch ( unit )
                {
                    case "d":
                        return (Int64)difference.TotalDays;
                    case "h":
                        return (Int64)difference.TotalHours;
                    case "m":
                        return (Int64)difference.TotalMinutes;
                    case "M":
                        return (Int64)GetMonthsBetween( startDate, endDate );
                    case "Y":
                        return (Int64)( endDate.Year - startDate.Year );
                    case "s":
                        return (Int64)difference.TotalSeconds;
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }

        private static int GetMonthsBetween( DateTime from, DateTime to )
        {
            if ( from > to ) return GetMonthsBetween( to, from );

            var monthDiff = Math.Abs( ( to.Year * 12 + ( to.Month - 1 ) ) - ( from.Year * 12 + ( from.Month - 1 ) ) );

            if ( from.AddMonths( monthDiff ) > to || to.Day < from.Day )
            {
                return monthDiff - 1;
            }
            else
            {
                return monthDiff;
            }
        }

        #endregion

        #region Number Filters

        /// <summary>
        /// Formats the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string Format( object input, string format )
        {
            if ( input == null )
                return null;
            else if ( string.IsNullOrWhiteSpace( format ) )
                return input.ToString();

            return string.Format( "{0:" + format + "}", input );
        }

        /// <summary>
        /// Formats the specified input as currency using the CurrencySymbol from Global Attributes
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string FormatAsCurrency( object input )
        {
            if ( input == null )
            {
                return null;
            }

            if (input is string)
            {
                // if the input is a string, just append the currency symbol to the front, even if it can't be converted to a number
                var currencySymbol = GlobalAttributesCache.Value( "CurrencySymbol" );
                return string.Format("{0}{1}", currencySymbol, input);
            }
            else
            {
                // if the input an integer, decimal, double or anything else that can be parsed as a decimal, format that
                decimal? inputAsDecimal = input.ToString().AsDecimalOrNull();
                return inputAsDecimal.FormatAsCurrency();
            }
        }

        /// <summary>
        /// Addition - Overriding this to change the logic. The default filter will concat if the type is 
        /// string. This one does the math if the input can be parsed as a int
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static object Plus( object input, object operand )
        {
            if ( input == null )
            {
                return input;
            }

            decimal iInput = -1;
            decimal iOperand = -1;

            if ( decimal.TryParse( input.ToString(), out iInput ) && decimal.TryParse( operand.ToString(), out iOperand ) )
            {
                return iInput + iOperand;
            }
            else
            {
                return string.Concat( input, operand );
            }
        }

        /// <summary>
        /// Minus - Overriding this to change the logic. This one does the math if the input can be parsed as a int
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static object Minus( object input, object operand )
        {
            if ( input == null )
            {
                return input;
            }

            decimal iInput = -1;
            decimal iOperand = -1;

            if ( decimal.TryParse( input.ToString(), out iInput ) && decimal.TryParse( operand.ToString(), out iOperand ) )
            {
                return iInput - iOperand;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Times - Overriding this to change the logic. This one does the math if the input can be parsed as a int
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static object Times( object input, object operand )
        {
            if ( input == null )
            {
                return input;
            }

            decimal iInput = -1;
            decimal iOperand = -1;

            if ( decimal.TryParse( input.ToString(), out iInput ) && decimal.TryParse( operand.ToString(), out iOperand ) )
            {
                return iInput * iOperand;
            }
            else
            {
                return Enumerable.Repeat( (string)input, (int)operand );
            }
        }

        /// <summary>
        /// Divideds the by.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="operand">The operand.</param>
        /// <param name="precision">The precision.</param>
        /// <returns></returns>
        public static object DividedBy( object input, object operand, int precision = 2 )
        {
            if ( input == null || operand == null )
                return null;

            try
            {
                decimal dInput = 0;
                decimal dOperand = 0;

                if ( decimal.TryParse( input.ToString(), out dInput ) && decimal.TryParse( operand.ToString(), out dOperand ) )
                {
                    decimal result = ( dInput / dOperand );
                    return decimal.Round( result, precision );
                }

                return "Could not convert input to number";
                
            } catch (Exception ex){
                return ex.Message;
            }
        }

        /// <summary>
        /// Floors the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static object Floor( object input)
        {
            if ( input == null )
            {
                return input;
            }

            decimal iInput = -1;

            if ( decimal.TryParse( input.ToString(), out iInput ) )
            {
                    return decimal.Floor( iInput );                
            }
            else
            {
                return "Could not convert input to number to round";
            }
        }

        /// <summary>
        /// Ceilings the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static object Ceiling( object input )
        {
            if ( input == null )
            {
                return input;
            }

            decimal iInput = -1;

            if ( decimal.TryParse( input.ToString(), out iInput ) )
            {
                return decimal.Ceiling( iInput );
            }
            else
            {
                return "Could not convert input to number to round";
            }
        }

        #endregion

        #region Attribute Filters

        /// <summary>
        /// DotLiquid Attribute Filter
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="attributeKey">The attribute key.</param>
        /// <param name="qualifier">The qualifier.</param>
        /// <returns></returns>
        public static object Attribute( DotLiquid.Context context, object input, string attributeKey, string qualifier = "" )
        {
            IHasAttributes item = null;
            
            if ( input == null || attributeKey == null )
            {
                return string.Empty;
            }

            // Try to get RockContext from the dotLiquid context
            var rockContext = GetRockContext(context);

            AttributeCache attribute = null;
            string rawValue = string.Empty;

            // If Input is "Global" then look for a global attribute with key
            if (input.ToString().Equals( "Global", StringComparison.OrdinalIgnoreCase ) )
            {
                var globalAttributeCache = Rock.Web.Cache.GlobalAttributesCache.Read( rockContext );
                attribute = globalAttributeCache.Attributes
                    .FirstOrDefault( a => a.Key.Equals(attributeKey, StringComparison.OrdinalIgnoreCase));
                if (attribute != null )
                {
                    // Get the value
                    string theValue = globalAttributeCache.GetValue( attributeKey );
                    if ( theValue.HasMergeFields() )
                    {
                        // Global attributes may reference other global attributes, so try to resolve this value again
                        rawValue = theValue.ResolveMergeFields( new Dictionary<string, object>() );
                    }
                    else
                    {
                        rawValue = theValue;
                    }
                }
            }

            // If input is an object that has attributes, find its attribute value
            else
            { 
                
                if ( input is IHasAttributes)
                {
                    item = (IHasAttributes)input;
                }
                else if ( input is IHasAttributesWrapper )  
                {
                    item = ((IHasAttributesWrapper)input).HasAttributesEntity;
                }

                if ( item != null )
                {
                    if ( item.Attributes == null )
                    {
                        item.LoadAttributes( rockContext );
                    }

                    if ( item.Attributes.ContainsKey( attributeKey ) )
                    {
                        attribute = item.Attributes[attributeKey];
                        rawValue = item.AttributeValues[attributeKey].Value;
                    }
                }
            }

            // If valid attribute and value were found
            if ( attribute != null && !string.IsNullOrWhiteSpace( rawValue ) )
            {
                Person currentPerson = null;

                // First check for a person override value included in lava context
                if ( context.Scopes != null )
                {
                    foreach ( var scopeHash in context.Scopes )
                    {
                        if ( scopeHash.ContainsKey( "CurrentPerson" ) )
                        {
                            currentPerson = scopeHash["CurrentPerson"] as Person;
                        }
                    }
                }

                if ( currentPerson == null )
                {
                    var httpContext = System.Web.HttpContext.Current;
                    if ( httpContext != null && httpContext.Items.Contains( "CurrentPerson" ) )
                    {
                        currentPerson = httpContext.Items["CurrentPerson"] as Person;
                    }
                }

                if ( attribute.IsAuthorized( Authorization.VIEW, currentPerson ) )
                {
                    // Check qualifier for 'Raw' if present, just return the raw unformatted value
                    if ( qualifier.Equals( "RawValue", StringComparison.OrdinalIgnoreCase ) )
                    {
                        return rawValue;
                    }

                    // Check qualifier for 'Url' and if present and attribute's field type is a ILinkableFieldType, then return the formatted url value
                    var field = attribute.FieldType.Field;
                    if ( qualifier.Equals( "Url", StringComparison.OrdinalIgnoreCase ) && field is Rock.Field.ILinkableFieldType )
                    {
                        return ( (Rock.Field.ILinkableFieldType)field ).UrlLink( rawValue, attribute.QualifierValues );
                    }

                    // check if attribute is a key value list and return a collection of key/value pairs
                    if ( field is Rock.Field.Types.KeyValueListFieldType )
                    {
                        var keyValueField = (Rock.Field.Types.KeyValueListFieldType)field;

                        return keyValueField.GetValuesFromString( null, rawValue, attribute.QualifierValues, false );
                    }

                    // If qualifier was specified, and the attribute field type is an IEntityFieldType, try to find a property on the entity
                    if ( !string.IsNullOrWhiteSpace( qualifier ) && field is Rock.Field.IEntityFieldType )
                    {
                        IEntity entity = ( (Rock.Field.IEntityFieldType)field ).GetEntity( rawValue );
                        if ( entity != null )
                        {
                            if ( qualifier.Equals( "object", StringComparison.OrdinalIgnoreCase ) )
                            {
                                return entity;
                            }
                            else
                            {
                                return entity.GetPropertyValue( qualifier ).ToStringSafe();
                            }
                        }
                    }

                    // Otherwise return the formatted value
                    return field.FormatValue( null, rawValue, attribute.QualifierValues, false );
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Properties the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="propertyKey">The property key.</param>
        /// <param name="qualifier">The qualifier.</param>
        /// <returns></returns>
        public static object Property( DotLiquid.Context context, object input, string propertyKey, string qualifier = "" )
        {
            if ( input != null )
            {
                return input.GetPropertyValue(propertyKey);
            }
            return string.Empty;
        }

        #endregion

        #region Person Filters

        /// <summary>
        /// Persons the by identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static Person PersonById( DotLiquid.Context context, object input )
        {
            if ( input == null )
            {
                return null;
            }

            int personId = -1;

            if (! Int32.TryParse(input.ToString(), out personId)) {
                return null;
            }

            var rockContext = new RockContext();

            return new PersonService( rockContext ).Get( personId );

        }

        /// <summary>
        /// Persons the by alias identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static Person PersonByAliasId( DotLiquid.Context context, object input )
        {
            if ( input == null )
            {
                return null;
            }

            int personAliasId = -1;

            if ( !Int32.TryParse( input.ToString(), out personAliasId ) )
            {
                return null;
            }

            var rockContext = new RockContext();

            return new PersonAliasService( rockContext ).Get( personAliasId ).Person;

        }

        /// <summary>
        /// Gets an address for a person object
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="addressType">Type of the address.</param>
        /// <param name="qualifier">The qualifier.</param>
        /// <returns></returns>
        public static string Address( DotLiquid.Context context, object input, string addressType, string qualifier = "" )
        {
            var person = GetPerson( input );

            if ( person != null )
            {
                Guid familyGuid = Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuid();

                Location location = null;

                switch ( addressType )
                {
                    case "Mailing":
                        location = new GroupMemberService( GetRockContext( context ) )
                            .Queryable( "GroupLocations.Location" )
                            .AsNoTracking()
                            .Where( m =>
                                m.PersonId == person.Id &&
                                m.Group.GroupType.Guid == familyGuid )
                            .SelectMany( m => m.Group.GroupLocations )
                            .Where( gl =>
                                gl.IsMailingLocation == true )
                            .Select( gl => gl.Location )
                            .FirstOrDefault();
                        break;
                    case "MapLocation":
                        location = new GroupMemberService( GetRockContext( context ) )
                            .Queryable( "GroupLocations.Location" )
                            .AsNoTracking()
                            .Where( m =>
                                m.PersonId == person.Id &&
                                m.Group.GroupType.Guid == familyGuid )
                            .SelectMany( m => m.Group.GroupLocations )
                            .Where( gl =>
                                gl.IsMappedLocation == true )
                            .Select( gl => gl.Location )
                            .FirstOrDefault();
                        break;
                    default:
                        location = new GroupMemberService( GetRockContext( context ) )
                            .Queryable( "GroupLocations.Location" )
                            .AsNoTracking()
                            .Where( m =>
                                m.PersonId == person.Id &&
                                m.Group.GroupType.Guid == familyGuid )
                            .SelectMany( m => m.Group.GroupLocations )
                            .Where( gl =>
                                gl.GroupLocationTypeValue.Value == addressType )
                            .Select( gl => gl.Location )
                            .FirstOrDefault();
                        break;
                }

                if (location != null)
                {
                    if ( qualifier == "" )
                    {
                        return location.GetFullStreetAddress();
                    }
                    else
                    {
                        var matches = Regex.Matches(qualifier, @"\[\[([^\]]+)\]\]");
                        foreach ( var match in matches )
                        {
                            string propertyKey = match.ToString().Replace("[", "");
                            propertyKey = propertyKey.ToString().Replace( "]", "" );
                            propertyKey = propertyKey.ToString().Replace( " ", "" );

                            switch ( propertyKey )
                            {
                                case "Street1":
                                    qualifier = qualifier.Replace( match.ToString(), location.Street1 );
                                    break;
                                case "Street2":
                                    qualifier = qualifier.Replace( match.ToString(), location.Street2 );
                                    break;
                                case "City":
                                    qualifier = qualifier.Replace( match.ToString(), location.City );
                                    break;
                                case "State":
                                    qualifier = qualifier.Replace( match.ToString(), location.State );
                                    break;
                                case "PostalCode":
                                case "Zip":
                                    qualifier = qualifier.Replace( match.ToString(), location.PostalCode );
                                    break;
                                case "Country":
                                    qualifier = qualifier.Replace( match.ToString(), location.Country );
                                    break;
                                case "GeoPoint":
                                    if ( location.GeoPoint != null )
                                    {
                                        qualifier = qualifier.Replace( match.ToString(), string.Format("{0},{1}", location.GeoPoint.Latitude.ToString(), location.GeoPoint.Longitude.ToString()) );
                                    }
                                    else
                                    {
                                        qualifier = qualifier.Replace( match.ToString(), "" );
                                    }
                                    break;
                                case "Latitude":
                                    if ( location.GeoPoint != null )
                                    {
                                        qualifier = qualifier.Replace( match.ToString(), location.GeoPoint.Latitude.ToString() );
                                    }
                                    else
                                    {
                                        qualifier = qualifier.Replace( match.ToString(), "" );
                                    }
                                    break;
                                case "Longitude":
                                    if ( location.GeoPoint != null )
                                    {
                                        qualifier = qualifier.Replace( match.ToString(), location.GeoPoint.Longitude.ToString() );
                                    }
                                    else
                                    {
                                        qualifier = qualifier.Replace( match.ToString(), "" );
                                    }
                                    break;
                                case "FormattedAddress":
                                    qualifier = qualifier.Replace( match.ToString(), location.FormattedAddress );
                                    break;
                                case "FormattedHtmlAddress":
                                    qualifier = qualifier.Replace( match.ToString(), location.FormattedHtmlAddress );
                                    break;
                                default:
                                    qualifier = qualifier.Replace( match.ToString(), "" );
                                    break;
                            }
                        }

                        return qualifier;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the groups of selected type that person is a member of
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="groupTypeId">The group type identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static List<Rock.Model.GroupMember> Groups( DotLiquid.Context context, object input, string groupTypeId, string status = "Active" )
        {
            var person = GetPerson( input );
            int? numericalGroupTypeId = groupTypeId.AsIntegerOrNull();

            if ( person != null && numericalGroupTypeId.HasValue )
            {
                var groupQuery =  new GroupMemberService( GetRockContext( context ) )
                    .Queryable("Group, GroupRole").AsNoTracking()
                    .Where( m =>
                        m.PersonId == person.Id &&
                        m.Group.GroupTypeId == numericalGroupTypeId.Value &&
                        m.Group.IsActive );
                
                if ( status != "All" )
                {
                    GroupMemberStatus queryStatus = GroupMemberStatus.Active;
                    queryStatus = (GroupMemberStatus)Enum.Parse( typeof( GroupMemberStatus ), status, true );

                    groupQuery = groupQuery.Where( m => m.GroupMemberStatus == queryStatus );
                }

                return groupQuery.ToList();
            }

            return new List<Model.GroupMember>();
        }

        /// <summary>
        /// Gets the groups of selected type that person is a member of which they have attended at least once
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="groupTypeId">The group type identifier.</param>
        /// <returns></returns>
        public static List<Rock.Model.Group> GroupsAttended( DotLiquid.Context context, object input, string groupTypeId )
        {
            var person = GetPerson( input );
            int? numericalGroupTypeId = groupTypeId.AsIntegerOrNull();

            if ( person != null && numericalGroupTypeId.HasValue )
            {
                return new AttendanceService( GetRockContext( context ) ).Queryable().AsNoTracking()
                    .Where(a => a.Group.GroupTypeId == numericalGroupTypeId && a.PersonAlias.PersonId == person.Id && a.DidAttend == true)
                    .Select(a => a.Group).Distinct().ToList();
            }

            return new List<Model.Group>();
        }

        /// <summary>
        /// Gets the last attendance item for a given person in a group of type provided
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="groupTypeId">The group type identifier.</param>
        /// <returns></returns>
        public static Attendance LastAttendedGroupOfType( DotLiquid.Context context, object input, string groupTypeId )
        {
            var person = GetPerson( input );
            int? numericalGroupTypeId = groupTypeId.AsIntegerOrNull();

            if ( person != null && numericalGroupTypeId.HasValue )
            {
                var attendance =  new AttendanceService( GetRockContext( context ) ).Queryable("Group").AsNoTracking()
                    .Where( a => a.Group.GroupTypeId == numericalGroupTypeId && a.PersonAlias.PersonId == person.Id && a.DidAttend == true )
                    .OrderByDescending( a => a.StartDateTime ).FirstOrDefault();

                return attendance;
            }

            return new Attendance();
        }

        /// <summary>
        /// Gets the groups of selected type that geofence the selected person
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="groupTypeId">The group type identifier.</param>
        /// <returns></returns>
        public static List<Rock.Model.Group> GeofencingGroups( DotLiquid.Context context, object input, string groupTypeId )
        {
            var person = GetPerson( input );
            int? numericalGroupTypeId = groupTypeId.AsIntegerOrNull();

            if ( person != null && numericalGroupTypeId.HasValue )
            {
                return new GroupService( GetRockContext( context ) )
                    .GetGeofencingGroups( person.Id, numericalGroupTypeId.Value )
                    .ToList();
            }

            return new List<Model.Group>();
        }

        /// <summary>
        /// Gets the groups of selected type that geofence the selected person 
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="groupTypeId">The group type identifier.</param>
        /// <param name="groupTypeRoleId">The group type role identifier.</param>
        /// <returns></returns>
        public static List<Rock.Model.Person> GeofencingGroupMembers( DotLiquid.Context context, object input, string groupTypeId, string groupTypeRoleId )
        {
            var person = GetPerson( input );
            int? numericalGroupTypeId = groupTypeId.AsIntegerOrNull();
            int? numericalGroupTypeRoleId = groupTypeRoleId.AsIntegerOrNull();

            if ( person != null && numericalGroupTypeId.HasValue && numericalGroupTypeRoleId.HasValue )
            {
                return new GroupService( GetRockContext( context ) )
                    .GetGeofencingGroups( person.Id, numericalGroupTypeId.Value )
                    .SelectMany( g => g.Members.Where( m => m.GroupRole.Id == numericalGroupTypeRoleId ) )
                    .Select( m => m.Person )
                    .ToList();
            }

            return new List<Model.Person>();
        }

        /// <summary>
        /// Returnes the nearest group of a specific type.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="groupTypeId">The group type identifier.</param>
        /// <returns></returns>
        public static Rock.Model.Group NearestGroup( DotLiquid.Context context, object input, string groupTypeId )
        {
            var person = GetPerson( input );
            int? numericalGroupTypeId = groupTypeId.AsIntegerOrNull();

            if ( person != null && numericalGroupTypeId.HasValue )
            {
                return new GroupService( GetRockContext( context ) )
                    .GetNearestGroup( person.Id, numericalGroupTypeId.Value );
            }

            return null;
        }
        
        /// <summary>
        /// Gets the rock context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private static RockContext GetRockContext( DotLiquid.Context context)
        {
            if ( context.Registers.ContainsKey("rock_context"))
            {
                return context.Registers["rock_context"] as RockContext;
            }
            else
            {
                var rockContext = new RockContext();
                context.Registers.Add( "rock_context", rockContext );
                return rockContext;
            }
        }

        private static Person GetPerson( object input )
        {
            if ( input != null )
            {
                var person = input as Person;
                if ( person != null )
                {
                    return person;
                }
                var checkinPerson = input as CheckIn.CheckInPerson;
                if ( checkinPerson != null )
                {
                    return checkinPerson.Person;
                }
            }
            return null;
        }

        #endregion

        #region Misc Filters

        /// <summary>
        /// creates a postback javascript function
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static string Postback( object input, string command )
        {
            if ( input != null )
            {
                return string.Format( "javascript:__doPostBack('[ClientId]','{0}^{1}'); return false;", command, input.ToString() );
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ToJSON (object input)
        {
            return input.ToJson();
        }

        /// <summary>
        /// adds a meta tag to the head of the document
        /// </summary>
        /// <param name="input">The input to use for the content attribute of the tag.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns></returns>
        public static string AddMetaTagToHead( string input, string attributeName, string attributeValue )
        {
            RockPage page = HttpContext.Current.Handler as RockPage;

            if ( page != null )
            {
                HtmlMeta metaTag = new HtmlMeta();
                metaTag.Attributes.Add( attributeName, attributeValue );
                metaTag.Content = input;
                page.Header.Controls.Add( metaTag );
            }

            return null;
        }

        /// <summary>
        /// adds a link tag to the head of the document
        /// </summary>
        /// <param name="input">The input to use for the href of the tag.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns></returns>
        public static string AddLinkTagToHead( string input, string attributeName, string attributeValue )
        {
            RockPage page = HttpContext.Current.Handler as RockPage;

            if ( page != null )
            {
                HtmlLink imageLink = new HtmlLink();
                imageLink.Attributes.Add( attributeName, attributeValue );
                imageLink.Attributes.Add( "href", input );
                page.Header.Controls.Add( imageLink );
            }

            return null;
        }

        /// <summary>
        /// adds a link tag to the head of the document
        /// </summary>
        /// <param name="input">The input to use for the href of the tag.</param>
        /// <returns></returns>
        public static string SetPageTitle( string input )
        {
            RockPage page = HttpContext.Current.Handler as RockPage;

            if ( page != null )
            {
                page.BrowserTitle = input;
                page.PageTitle = input;
                page.Header.Title = input;
            }

            return null;
        }


        /// <summary>
        /// Pages the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="parm">The parm.</param>
        /// <returns></returns>
        public static string Page( string input, string parm )
        {
            RockPage page = HttpContext.Current.Handler as RockPage;

            if ( page != null )
            {
                switch ( parm )
                {
                    case "Title": 
                        {
                            return page.BrowserTitle;
                        }
                    case "Url": 
                        {
                            return HttpContext.Current.Request.Url.AbsoluteUri;
                        }
                    case "Id":
                        {
                            return page.PageId.ToString();
                        }
                    case "Host":
                        {
                            return HttpContext.Current.Request.Url.Host;
                        }
                    case "Path":
                        {
                            return HttpContext.Current.Request.Url.AbsolutePath;
                        }
                    case "SiteName":
                        {
                            return page.Site.Name;
                        }
                    case "SiteId":
                        {
                            return page.Site.Id.ToString();
                        }
                    case "Theme":
                        {
                            if ( page.Theme != null )
                            {
                                return page.Theme;
                            }
                            else
                            {
                                return page.Site.Theme;
                            }
                            
                        }
                    case "Layout":
                        {
                            return page.Layout.Name;
                        }
                    case "Scheme":
                        {
                            return HttpContext.Current.Request.Url.Scheme;
                        }
                }
            }

            return null;
        }


        /// <summary>
        /// Converts a lava property to a key value pair
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static Dictionary<string, object> PropertyToKeyValue(object input)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            var type = input.GetType();

            if ( type == typeof(KeyValuePair<string,object>) )
            {
                var key = type.GetProperty("Key");
                var value = type.GetProperty("Value");

                result.Add("Key", key.GetValue(input, null).ToString());
                result.Add("Value", value.GetValue(input, null));
            }

            return result;
        }

        #endregion

        #region Array Filters

        /// <summary>
        /// Rearranges an array in a random order
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static object Shuffle( object input )
        {
            if ( input == null )
            {
                return input;
            }

            if ( !(input is IList) )
            {
                return input;
            }

            var inputList = input as IList;
            Random rng = new Random();
            int n = inputList.Count;
            while ( n > 1 )
            {
                n--;
                int k = rng.Next( n + 1 );
                var value = inputList[k];
                inputList[k] = inputList[n];
                inputList[n] = value;
            }

            return inputList;
        }

        /// <summary>
        /// Wheres the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="filterKey">The filter key.</param>
        /// <param name="filterValue">The filter value.</param>
        /// <returns></returns>
        public static object Where( object input, string filterKey, object filterValue )
        {
            if ( input == null )
            {
                return input;
            }

            if ( input is IEnumerable )
            {
                var result = new List<object>();
                
                foreach ( var value in ( (IEnumerable)input ) )
                {
                    if ( value is ILiquidizable )
                    {
                        var liquidObject = value as ILiquidizable;
                        if (liquidObject.ContainsKey(filterKey) && liquidObject[filterKey].Equals(filterValue)) {
                            result.Add(liquidObject);
                        }
                    }
                }

                return result;
            }

            return input;
        }

        /// <summary>
        /// Selects the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="selectKey">The select key.</param>
        /// <returns></returns>
        public static object Select( object input, string selectKey )
        {
            if ( input == null )
            {
                return input;
            }

            if ( input is IEnumerable )
            {
                var result = new List<object>();

                foreach ( var value in ( (IEnumerable)input ) )
                {
                    if ( value is ILiquidizable )
                    {
                        var liquidObject = value as ILiquidizable;
                        if ( liquidObject.ContainsKey( selectKey ) )
                        {
                            result.Add( liquidObject[selectKey] );
                        }
                    }
                }

                return result;
            }

            return input;
        }

        #endregion
    }
}
