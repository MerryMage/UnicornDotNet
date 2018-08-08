using System;
using System.Runtime.InteropServices;

namespace UnicornDotNet.Native {

public class Interface {
    [DllImport("unicorn", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint uc_version(out uint major, out uint minor);
}

}