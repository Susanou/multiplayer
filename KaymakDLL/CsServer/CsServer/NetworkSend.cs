using System;
using KaymakNetwork;
using System.Numerics;

namespace CsServer
{
    enum ServerPackets
    {
        SWelcome = 1,
        SInstantiatePlayer = 2,
        SPlayerMove
    }

    internal static class NetworkSend
    {

        public static void WelcomeMsg(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SWelcome);
            buffer.WriteInt32(connectionID);
            buffer.WriteString(msg);           
            NetworkConfig.socket.SendDataTo(connectionID, buffer.Data, buffer.Head);

            buffer.Dispose();
        }

        private static ByteBuffer PlayerData(int connectionID, Player player)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SInstantiatePlayer);
            buffer.WriteInt32(connectionID);

            return buffer;
        }

        public static void InstantiateNetworkPlayer(int connectionID, Player player)
        {


            for (int i = 1; i < GameManager.playerList.Count; i++)
            {
                if(GameManager.playerList[i] != null)
                {
                    if(GameManager.playerList[i].inGame)
                    {
                        if(i != connectionID)
                        {
                            NetworkConfig.socket.SendDataTo(connectionID, PlayerData(i, player).Data, PlayerData(i, player).Head);
                        }
                    }
                }
            }

            NetworkConfig.socket.SendDataToAll(PlayerData(connectionID, player).Data, PlayerData(connectionID, player).Head);
        }

        public static void SendPlayerMove(int connectionID, float x, float y, float z)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerMove);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(x);
            buffer.WriteSingle(y);
            buffer.WriteSingle(z);

            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);

            buffer.Dispose();
        }

    }
}
