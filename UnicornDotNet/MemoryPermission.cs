using System;

namespace UnicornDotNet
{
    public enum MemoryPermission
    {
        NONE = 0,
        READ = 1,
        WRITE = 2,
        EXEC = 4,
        ALL = 7,
    }
}
