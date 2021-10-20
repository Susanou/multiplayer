using System;
using KaymakNetwork;
using UnityEngine;


enum ServerPackets
{
    SWelcome = 1,
}
internal static class NetworkReceive
{
    internal static void PacketRouter()
    {
        Debug.Log("we are here");
        //NetworkSend.SendPing();
        
        NetworkConfig.socket.PacketId[(int)ServerPackets.SWelcome] = new KaymakNetwork.Network.Client.Client.DataArgs(Packet_WelcomeMsg);
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
}
