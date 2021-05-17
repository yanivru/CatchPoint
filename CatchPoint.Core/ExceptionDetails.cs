using System.Collections.Generic;

namespace CatchPoint.Core
{
    public class ExceptionDetails
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> StackTrace { get; set; }

        public ExceptionDetails[] InnerException { get; set; }
    }
}