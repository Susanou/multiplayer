using System;
using KaymakNetwork;
using UnityEngine;


enum ServerPackets
{
    SWelcome = 1,
    SInstantiatePlayer = 2,
}
internal static class NetworkReceive
{
    internal static void PacketRouter()
    {
        Debug.Log("we are here");
        //NetworkSend.SendPing();
        
        NetworkConfig.socket.PacketId[(int)ServerPackets.SWelcome] = new KaymakNetwork.Network.Client.Client.DataArgs(Packet_WelcomeMsg);
        NetworkConfig.socket.PacketId[(int)ServerPackets.SInstantiatePlayer] = new KaymakNetwork.Network.Client.Client.DataArgs(Packet_InstantiateNetworkPlayer);
    }

    private static void Packet_WelcomeMsg(ref byte[] data)
    {   
        Debug.Log("Trying to read a pakcet");
        ByteBuffer buffer = new ByteBuffer(data);
        string msg = buffer.ReadString();
        buffer.Dispose();

        Debug.Log(msg);

        NetworkSend.SendPing();
    }

    private static void Packet_InstantiateNetworkPlayer(ref byte[] data){

        ByteBuffer buffer = new ByteBuffer(data);
        int connectionID = buffer.ReadInt32();

        NetworkManager.instance.InstantiateNetworkPlayer(connectionID);
    }
}
