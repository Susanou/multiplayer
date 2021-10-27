using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;


internal static class UDPSend
{
    internal static void SendPing(){
        byte[] response = Encoding.ASCII.GetBytes("Ping");
        UnityUDPClient.client.Send(response, response.Length);
    }

    internal static void SendInstantiated()
    {
        byte[] response = Encoding.ASCII.GetBytes("Player Instantiated");
        UnityUDPClient.client.Send(response, response.Length);
    }

    public static void SendKeyInput(InputManager.Keys pressedKey)
    {
        byte[] response = Encoding.ASCII.GetBytes("P:" + (byte) pressedKey);
        UnityUDPClient.client.Send(response, response.Length);
    }
}
