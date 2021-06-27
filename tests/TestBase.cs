using System;

namespace tests
{
    internal class TestBase
    {
        internal const string SYNC_TESTS = "Synchronous";
        internal const string ASYNC_TESTS = "Asynchronous";
        internal const string SERIAL_TESTS = "Serialization";

        internal void Log(object obj)
            => Console.WriteLine(obj);
        internal void Log(string format, params object[] args)
            => Console.WriteLine(format, args);
    }
}