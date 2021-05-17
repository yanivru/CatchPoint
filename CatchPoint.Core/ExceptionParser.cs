using System;
using System.Collections.Generic;

namespace CatchPoint.Core
{
    public class ExceptionParser
    {
        private const string startOfStackTrace = "   at ";

        public ExceptionDetails Parse(string exception)
        {
            int indexOfSemicolon = exception.IndexOf(":");
            int indexOfStartOfStackTrace = exception.IndexOf(startOfStackTrace);

            string type = exception.Substring(0, indexOfSemicolon);
            string message = exception.Substring(indexOfSemicolon + 1, indexOfStartOfStackTrace - indexOfSemicolon);

            var stackTrace = exception.Substring(indexOfStartOfStackTrace);

            var stackTraces = stackTrace.Split(startOfStackTrace, StringSplitOptions.RemoveEmptyEntries);

            return new ExceptionDetails { Type = type, Message = message.Trim(), StackTrace = stackTraces };
        }
    }
}
