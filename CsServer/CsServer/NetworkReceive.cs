using System;
using KaymakNetwork;

namespace CsServer
{
    enum ClientPackets
    {
        CPing = 1,
    }

    internal static class NetworkReceive
    {
        internal static void PacketRouter()
        {
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPing] = Packet_Ping;
        }

        private static void Packet_Ping(int connectionID, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            string msg = buffer.ReadString();

            Console.WriteLine(msg);

            buffer.Dispose();
        }
    }
}
