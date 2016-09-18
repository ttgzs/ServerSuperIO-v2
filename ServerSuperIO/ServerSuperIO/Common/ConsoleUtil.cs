using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ServerSuperIO.Common
{
    public class ConsoleUtil
    {
        /// <summary>
        ///      0工具被强制关闭 //Ctrl+C关闭  
        ///      2工具被强制关闭
        /// </summary>
        /// <param name="ctrlType"></param>
        /// <returns></returns>
        public delegate bool ControlCtrlDelegate(int ctrlType);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);

    }
}
