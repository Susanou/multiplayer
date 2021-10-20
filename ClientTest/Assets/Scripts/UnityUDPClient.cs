using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine;

public class UnityUDPClient : MonoBehaviour
{

    public string host;
    public int port;

    private bool recvloop;

    private UdpClient client;
    private Thread threadRecv;

    // Start is called before the first frame update
    void Start()
    {
        client = new UdpClient();
        client.Connect(host, port);

        threadRecv = new Thread(ReceiveDataThread);
        threadRecv.IsBackground = true;
        threadRecv.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReceiveDataThread()
    {
        recvloop = true;
        while (recvloop)
        {
            IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                byte[] response = Encoding.ASCII.GetBytes("Hello server");
                client.Send(response, response.Length);
                recvloop = false;
                byte[] receivedData = client.Receive(ref RemoteIP);
                
                Debug.Log("Received data from server "+RemoteIP.Address.ToString() + " on port "+ RemoteIP.Port.ToString());
                Debug.Log("message: " + Encoding.ASCII.GetString(receivedData));
            }
            catch (Exception ex){
                Debug.LogError(ex.ToString());
            }
        }
    }

    private void OnApplicationQuit(){
        byte[] response = Encoding.ASCII.GetBytes("Thank you server goodbye");
        client.Send(response, response.Length);
        client.Dispose();
    }
}
