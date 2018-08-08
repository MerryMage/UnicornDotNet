using System;
using UnicornDotNet;
using Xunit;

namespace UnicornDotNet.Test
{
    public class SanityCheck
    {
        [Fact]
        public void PrintVersion()
        {
            Native.Interface.uc_version(out uint major, out uint minor);
            Console.WriteLine("Unicorn Version: {0}.{1}", major, minor);
        }

        [Fact]
        public void Add()
        {
            UnicornAArch64 unicorn = new UnicornAArch64();

            unicorn.MemoryMap(0x1000, 0x4000000, MemoryPermission.READ | MemoryPermission.EXEC | MemoryPermission.WRITE);

            unicorn.X[1] = 1;
            unicorn.X[2] = 2;
            unicorn.PC = 0x1004;

            Assert.True(unicorn.X[1] == 1);
            Assert.True(unicorn.X[2] == 2);
            Assert.True(unicorn.PC == 0x1004);

            unicorn.DumpMemoryInformation();

            unicorn.MemoryWrite32(0x1004, 0x8b020020); // ADD X0, X1, X2
            unicorn.MemoryWrite32(0x1008, 0x14000000); // B .

            unicorn.Step();

            Assert.True(unicorn.X[0] == 3);
            Assert.True(unicorn.PC == 0x1008);
        }
    }
}
