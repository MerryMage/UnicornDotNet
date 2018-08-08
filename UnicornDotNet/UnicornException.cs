using System;
using System.Runtime.InteropServices;

namespace UnicornDotNet
{
    public class UnicornException : Exception
    {
        public readonly UnicornError Error;

        internal UnicornException(UnicornError error)
        {
            Error = error;
        }

        public override string Message
        {
            get
            {
                return Marshal.PtrToStringAnsi(Native.Interface.uc_strerror(Error));
            }
        }
    }
}
