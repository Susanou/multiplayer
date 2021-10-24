using System;
using KaymakNetwork;
using UnityEngine;


enum ClientPackets
{
    CPing = 1,
}

internal static class NetworkSend
{
    public static void SendPing()
    {
        Debug.Log("Sending Ping to server");
        ByteBuffer buffer = new ByteBuffer(4);
        buffer.WriteInt32((int)ClientPackets.CPing);
        buffer.WriteString("Client sending some data during connection");
        NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
        buffer.Dispose();
        Debug.Log("Client data sent");
    }
}
