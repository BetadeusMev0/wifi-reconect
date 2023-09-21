using SimpleWifi;
using System.Net.NetworkInformation;

namespace wifi_reconect 
{
    class Program 
    {
        static string path = "E:\\CS_LOGS\\wifiLog.txt";
        static Wifi wf = new Wifi();
        static string wf_name;

        static AccessPoint acl;

        static void Main(string[] args) 
        {
            TimerCallback tm = new TimerCallback(loging);
            Timer timer = new Timer(tm, 2, 500, 300000); //600000

            refresh_wifi_status();
            wf_name = acl.Name;
            Console.WriteLine(acl.Name);
            Console.WriteLine(wf.ConnectionStatus);
            Console.WriteLine(acl.SignalStrength);
            Console.Read();
        }
        public static void loging(object obj) 
        {
            string status = getstatus();
            Console.WriteLine(status);
            File.AppendAllTextAsync(path, status);
        }
        public static string getstatus() 
        {
            bool conecct =  IsHostReachable("google.com");;
            refresh_wifi_status();
            string status = "";
            status += DateTime.Now;
            status += "-";
            status += wf.ConnectionStatus;
            status += "-";
            status += acl.SignalStrength;
            status += "-";
            status += conecct;
            status += "-";
            if (!conecct) 
            {
                if (wf_name == acl.Name)
                {
                    status += "reconect";
                    wf.Disconnect();
                    Thread.Sleep(10000);
                    acl.Connect(new AuthRequest(acl));
                }
            }
            else status += "stay";
            status += '-';
            if (wf_name == acl.Name) status += '1';
            else status += '0';
            status += '\n';
            
            return status;
        }
        public static void refresh_wifi_status() 
        {
            acl= wf.GetAccessPoints()[0];
        }

        // успешно своровано у chatgpt 
        // вроде понимаю сам принцип работы
        static bool IsHostReachable(string host)
        {
            bool isHostReachable = false;

            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(host);

                    if (reply != null && reply.Status == IPStatus.Success)
                    {
                        isHostReachable = true;
                    }
                }
            }
            catch (PingException)
            {
                isHostReachable = false;
            }

            return isHostReachable;
        }

    }
}