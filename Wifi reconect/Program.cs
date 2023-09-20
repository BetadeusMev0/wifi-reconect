using SimpleWifi;
using System.Net.NetworkInformation;

namespace wifi_reconect 
{
    class Program 
    {
        static string path = "C:\\Users\\wlade\\Documents\\CS_Logs\\wifiLog.txt";
        static Wifi wf = new Wifi();


        static AccessPoint acl;

        static void Main(string[] args) 
        {
            TimerCallback tm = new TimerCallback(loging);
            Timer timer = new Timer(tm, 2, 500, 300000); //600000

            refresh_wifi_status();

            Console.WriteLine(wf.ConnectionStatus);
            Console.WriteLine(acl.SignalStrength);
            Console.Read();
        }
        public static void loging(object obj) 
        {
            string status = getstatus();
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
                status += "reconect";
                wf.Disconnect();
                Thread.Sleep(10000);
                acl.Connect(new AuthRequest(acl));
                Console.WriteLine(status);
            }
            else status += "stay";
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