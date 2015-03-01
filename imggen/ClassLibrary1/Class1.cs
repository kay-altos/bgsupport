using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Management;
using System.Windows;
using System.ServiceModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace ReplaceBackgrounLibrary1
{
    public class ReplaceBackgroun
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        public static extern int SystemParametersInfo(int uAction, int uParam, IntPtr lpvParam, int fuWinIni);

        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 0x1;
        public const int SPIF_SENDWININICHANGE = 0x2;

        public void f1()
        {

            Console.WriteLine("this is lib");
            return;
        }



    }
}
