using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;

using System.ServiceModel;
using System.Management;
using System.Windows;
using Microsoft.Win32;

using System.Reflection;



namespace imggen
{

   
    
class Query_SelectQuery {
    
    string UName; //user name prop

    List<string> IPAddress;

    

   


       public string UserName()
    {

        SelectQuery selectQuery = new
               SelectQuery("Win32_ComputerSystem");
        ManagementObjectSearcher searcher =
            new ManagementObjectSearcher(selectQuery);




        foreach (ManagementObject obj in searcher.Get())
        {

            foreach (PropertyData prop in obj.Properties)
            {

                // Console.WriteLine("{0}: {1}", prop.Name, prop.Value);

                if (prop.Name == "PrimaryOwnerName")
                {

                   // Console.WriteLine(prop.Value);
                     this.UName = prop.Value.ToString();
                    
                }

            }
        }


        return UName;
    }


       public string OSVersion()
       {

           ManagementClass wmi = new ManagementClass("Win32_OperatingSystem");
           ManagementObjectCollection allConfigs = wmi.GetInstances();
           string OSver = string.Empty;

           foreach (ManagementObject configuration in allConfigs)
           {
               OSver = configuration["Caption"] == null ? string.Empty : configuration["Caption"].ToString();

               if (OSver.Length > 0)
                   break;
           }

           return OSver;
       }

       public string HostName()
       {

           ManagementClass wmi = new ManagementClass("Win32_OperatingSystem");
           ManagementObjectCollection allConfigs = wmi.GetInstances();
           string HostN = string.Empty;

           foreach (ManagementObject configuration in allConfigs)
           {
               HostN = configuration["Description"] == null ? string.Empty : configuration["Description"].ToString();

               if (HostN.Length > 0)
                   break;
           }

           return HostN;
       }




       public List<string> IpAddress()
       {



           ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
           List<string> allIPs = new List<string>();
           ManagementObjectCollection allConfigs = wmi.GetInstances();
           foreach (ManagementObject configuration in allConfigs)
           {
               if (configuration["IPAddress"] != null)
               {
                   if (configuration["IPAddress"] is Array)
                   {
                       string[] addresses = (string[])configuration["IPAddress"];
                       allIPs.AddRange(addresses);
                   }
                   else
                   {
                       allIPs.Add(configuration["IPAddress"].ToString());
                   }
               }
           }

          //Console.WriteLine(allIPs[0].ToString());

           IPAddress = allIPs;
        //  return allIPs;
           return IPAddress;
           
       }


     public void BgReplace(string path)
    {
         
          
         
    }
    



    }



	


    class CreateImageBg
    {
        string HostName;
        string   OSVersion;
        string UserName;
        List<string> IpAddress;
        int displayWidth;
        int displayHeight;
        string toBg;

        public const int SetDesktopWallpaper = 20;
        public const int UpdateIniFile = 0x01;
        public const int SendWinIniChange = 0x02;
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        
      

        private Bitmap CreateImage(string text, string PathToBg)
        {

             
            Bitmap myimg = new Bitmap(this.displayWidth, this.displayHeight);

            Graphics graph = Graphics.FromImage((Image)myimg);
            graph.Clear(Color.Gray);
            
            System.Drawing.Bitmap logo = imggen.Properties.Resources.logo_win;
            graph.DrawImage(logo, (float)this.displayWidth / 2, (float)this.displayHeight / 2, 137, 137);

            float LeftSideX = ((float)this.displayWidth) * (float)0.74;
            float LeftSideY = ((float)this.displayHeight) * (float)0.80;
            graph.DrawString(text,
                new Font("Arial", 12, FontStyle.Bold), Brushes.White, new PointF(LeftSideX, LeftSideY));
            myimg.Save(PathToBg, System.Drawing.Imaging.ImageFormat.Jpeg);
            
            logo.Dispose();
            graph.Dispose();
            myimg.Dispose();
            return myimg;



        }
        static void Main(string[] args)
        {

            CreateImageBg CiBg = new CreateImageBg();
            CiBg.displayHeight = SystemInformation.PrimaryMonitorSize.Height;
            CiBg.displayWidth = SystemInformation.PrimaryMonitorSize.Width;

            Query_SelectQuery qs = new Query_SelectQuery();
            CiBg.HostName = qs.HostName();
            CiBg.UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //Environment.UserName;
            CiBg.OSVersion = qs.OSVersion();
            CiBg.IpAddress = qs.IpAddress();

            //"%SystemRoot%\\System32\\RUNDLL32.EXE user32.dll, UpdatePerUserSystemParameters"

            //C:\Users\\AppData\Local\Temp\BGInfo.bmp

            string windowsPath = Environment.GetEnvironmentVariable("windir");
            //Console.WriteLine(windowsPath[0]);
            CiBg.toBg = windowsPath[0] + ":\\Users\\" +Environment.UserName+ "\\AppData\\Local\\Temp\\background.jpeg";

            CiBg.CreateImage("Имя ПК: " + CiBg.HostName + "\nОС: " + CiBg.OSVersion + "\nИмя пользователя: " + CiBg.UserName + "\nIP адрес: " + CiBg.IpAddress[0] + "\n", CiBg.toBg);

            SystemParametersInfo(SetDesktopWallpaper, 0, CiBg.toBg, UpdateIniFile | SendWinIniChange);
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
            key.Close();

            Console.WriteLine("background generated !!!");
            Console.WriteLine("PATH: " + CiBg.toBg);


            CiBg = null;








            Console.ReadLine();




          //  const string userRoot = "HKEY_CURRENT_USER";
         //   const string subkey = "Control Panel\\Desktop";
        //    const string keyName = userRoot + "\\" + subkey;

         //   Registry.SetValue(keyName, "Wallpaper", CiBg.toBg);
           
            

            //System.Diagnostics.Process.Start("cmd.exe", "/C " + "%SystemRoot%\\System32\\RUNDLL32.EXE user32.dll, UpdatePerUserSystemParameters");

          //  Console.WriteLine(keyName);

            //   Console.WriteLine(qs.IpAddress());
            
            
           
        }
   
     
    }
}
