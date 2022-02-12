using ServerCore;
using System;
using System.Net;
using System.Threading;

namespace Server
{

    internal class Program
    {
        static Listener _listener = new Listener();
        public static GameRoom Room = new GameRoom();

        static void FlushRoom()
        {
            Room.Push(() => Room.Flush());
            JobTimer.Instance.push(FlushRoom, 250);
        }
        static void Main()
        {
            //DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);


            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");

            JobTimer.Instance.push(FlushRoom);

            while (true)
            {
                JobTimer.Instance.Flush();
            }

        }
    }
}
