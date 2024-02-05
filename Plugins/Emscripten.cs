using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGalaxy
{
    public class Emscripten
    {
        [DllImport("emscripten_wrap", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void call_emscripten_set_main_loop(IntPtr callback, int fps, int simulateInfiniteLoop);
    }
}
