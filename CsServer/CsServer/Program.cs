using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CsServer
{
    class Program
    {
        private static Thread threadConsole;

        static void Main(string[] args)
        {
            threadConsole = new Thread(new ThreadStart(ConsoleThread));
            threadConsole.Start();

            NetworkConfig.InitNetwork();
            NetworkConfig.socket.StartListening(7777, 5, 1);
            Console.WriteLine("Network initialized and listening");
        }

        private static void ConsoleThread()
        {
            while (true)
            {

            }
        }
    }
}
