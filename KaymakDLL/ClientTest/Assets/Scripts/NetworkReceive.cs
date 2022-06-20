using System;
using KaymakNetwork;
using UnityEngine;


enum ServerPackets
{
    SWelcome = 1,
    SInstantiatePlayer = 2,
    SPlayerMove
}
internal static class NetworkReceive
{
    internal static void PacketRouter()
    {
        
        NetworkConfig.socket.PacketId[(int)ServerPackets.SWelcome] = new KaymakNetwork.Network.Client.Client.DataArgs(Packet_WelcomeMsg);
        NetworkConfig.socket.PacketId[(int)ServerPackets.SInstantiatePlayer] = new KaymakNetwork.Network.Client.Client.DataArgs(Packet_InstantiateNetworkPlayer);
        NetworkConfig.socket.PacketId[(int)ServerPackets.SPlayerMove] = new KaymakNetwork.Network.Client.Client.DataArgs(Packet_PlayerMove);
    }

    private static void Packet_WelcomeMsg(ref byte[] data)
    {   
        ByteBuffer buffer = new ByteBuffer(data);
        int connectionID = buffer.ReadInt32();
        string msg = buffer.ReadString();
        buffer.Dispose();

        NetworkManager.instance.myConnectionID = connectionID;

        NetworkSend.SendPing();
    }

    private static void Packet_InstantiateNetworkPlayer(ref byte[] data){

        ByteBuffer buffer = new ByteBuffer(data);
        int connectionID = buffer.ReadInt32();

        if (connectionID == NetworkManager.instance.myConnectionID)
            NetworkManager.instance.InstantiateNetworkPlayer(connectionID, true);
        else
            NetworkManager.instance.InstantiateNetworkPlayer(connectionID, false);
    }

    private static void Packet_PlayerMove(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int connectionID = buffer.ReadInt32();
        float x = buffer.ReadSingle();
        float y = buffer.ReadSingle();
        float z = buffer.ReadSingle();

        buffer.Dispose();

        if (!GameManager.instance.playerList.ContainsKey(connectionID)) return;

        GameManager.instance.playerList[connectionID].transform.position = new Vector3(x, y, z);
    }
}
