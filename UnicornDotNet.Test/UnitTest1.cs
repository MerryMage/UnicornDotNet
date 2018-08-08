using System;
using Xunit;
using UnicornDotNet;

namespace UnicornDotNet.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Native.Interface.uc_version(out uint major, out uint minor);
            Console.WriteLine("{0}", major);
            Console.WriteLine("{0}", minor);
        }
    }
}
