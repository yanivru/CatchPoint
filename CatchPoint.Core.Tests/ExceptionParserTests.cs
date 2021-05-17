using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CatchPoint.Core.Tests
{
    public class ExceptionParserTests
    {
        [Test]
        public void Parse_SystemExceptionThrown_SystemExceptionReturned()
        {
            Exception ex = CreateException();

            var exceptionParser = new ExceptionParser();

            var parsedEx = exceptionParser.Parse(ex.ToString());

            Assert.AreEqual("System.Exception", parsedEx.Type);
        }

        [Test]
        public void Parse_AsyncAwaitInStack_()
        {
            Exception ex = CreateExceptionWithAsync().Result;
        }

        [Test]
        public void Parse_ExceptionWithInner()
        {
            Exception ex = CreateExceptionWithInner();
        }

        private Exception CreateExceptionWithInner()
        {
            Exception res = null;
            try
            {
                try
                {
                    throw new Exception("This is the inner");
                }
                catch (Exception ex)
                {
                    throw new Exception("outer ex", ex);
                }
            }
            catch (Exception ex)
            {
                res = ex;
            }

            return res;
        }

        private async Task<Exception> CreateExceptionWithAsync()
        {
            Exception res = null;
            try
            {
                AsyncThrowingFun().Wait();
            }
            catch (Exception ex)
            {
                res = ex;
            }
            return res;
        }

        private static async Task AsyncThrowingFun()
        {
            await Task.Delay(1);
            throw new Exception("Exception with async");
        }

        [Test]
        public void Parse_MessageIsSet_MessageIsReturned()
        {
            Exception ex = CreateException();

            var exceptionParser = new ExceptionParser();

            var parsedEx = exceptionParser.Parse(ex.ToString());

            Assert.AreEqual("my message", parsedEx.Message);
        }

        [Test]
        public void Parse_NormalException_StackTraceIsReturned()
        {
            Exception ex = CreateException();

            var exceptionParser = new ExceptionParser();

            var parsedEx = exceptionParser.Parse(ex.ToString());

            var expectedFirstStackTrace = @"CatchPoint.Core.Tests.ExceptionParserTests.ThrowException() in C:\Users\Yaniv\source\repos\CatchPoint\CatchPoint.Core.Tests\ExceptionParserTests.cs:line";
            var expectedSecondStackTrace = @"CatchPoint.Core.Tests.ExceptionParserTests.CreateException() in C:\Users\Yaniv\source\repos\CatchPoint\CatchPoint.Core.Tests\ExceptionParserTests.cs:line";

            Assert.True(parsedEx.StackTrace.First().Contains(expectedFirstStackTrace));
            Assert.True(parsedEx.StackTrace.ElementAt(1).Contains(expectedSecondStackTrace));
        }
        
        private static Exception CreateException()
        {
            Exception ex = null;
            try
            {
                ThrowException();
            }
            catch (Exception e)
            {
                ex = e;
            }

            return ex;
        }

        private static void ThrowException()
        {
            throw new Exception("my message");
        }
    }
}