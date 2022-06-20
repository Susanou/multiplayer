using System;
using KaymakNetwork;

namespace CsServer
{
    enum ClientPackets
    {
        CPing = 1,
        CKeyInput
    }

    internal static class NetworkReceive
    {
        internal static void PacketRouter()
        {
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPing] = Packet_Ping;
            NetworkConfig.socket.PacketId[(int)ClientPackets.CKeyInput] = Packet_KeyInput;
        }

        private static void Packet_Ping(int connectionID, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            string msg = buffer.ReadString();

            Console.WriteLine(msg);
            GameManager.createPlayer(connectionID);
            buffer.Dispose();
        }

        private static void Packet_KeyInput(int connectionID, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            byte key = buffer.ReadByte();

            buffer.Dispose();

            InputManager.TryToMove(connectionID, (InputManager.Keys)key);
        }
    }
}
