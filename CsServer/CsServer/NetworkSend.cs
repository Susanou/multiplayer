using System;
using KaymakNetwork;

namespace CsServer
{
    enum ServerPackets
    {
        SWelcome = 1,
        SInstantiatePlayer = 2,
    }

    internal static class NetworkSend
    {

        public static void WelcomeMsg(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SWelcome);
            buffer.WriteString(msg);           
            NetworkConfig.socket.SendDataTo(connectionID, buffer.Data, buffer.Head);
            Console.WriteLine("Buffer Content: " + buffer.ToString());
            buffer.Dispose();
        }

        public static void InstantiateNetworkPlayer(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SInstantiatePlayer);
            NetworkConfig.socket.SendDataTo(connectionID, buffer.Data, buffer.Head);

            buffer.Dispose();
        }

    }
}
