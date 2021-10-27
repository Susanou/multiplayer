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

    public int myConnectionID;

    public GameObject PlayerPrefab;

    private bool recvloop;

    private UnityUDPClient instance;

    internal static UdpClient client;
    private Thread threadRecv;
    //private bool spawn = false; 

    private static Queue<Action> _executionQueue = new Queue<Action>();
    private IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 0);

    private void Awake()
    {
        instance = this;
    }


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
        lock(_executionQueue){
            while(_executionQueue.Count > 0){
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    void ReceiveDataThread()
    {
        recvloop = true;

        UDPSend.SendPing();


        while (recvloop)
        {
            try
            {
                string[] message = RecvPacket();

                Debug.Log("message: " + message.ToString());

                if(message[0] == "W"){
                    myConnectionID = Int32.Parse(message[1]);
                }

                if(message[0] == "I"){
                    int connectionID = Int32.Parse(message[1]);
                    _executionQueue.Enqueue((()=>PacketHandler.instance.InstantiatePlayer(connectionID, PlayerPrefab))); // find a way to not have to use the lambda to wrap everything
                    UDPSend.SendInstantiated();
                }
            }
            catch (Exception ex){
                Debug.LogError(ex.ToString());
            }
        }
    }

    private void OnApplicationQuit(){
        recvloop = false;
        byte[] response = Encoding.ASCII.GetBytes("Disconnected");
        client.Send(response, response.Length);
        client.Dispose();
    }

    private string[] RecvPacket(){
        byte[] receivedData = client.Receive(ref RemoteIP);
        Debug.Log("Received data from server "+RemoteIP.Address.ToString() + " on port "+ RemoteIP.Port.ToString());
        Debug.Log("message: " + Encoding.ASCII.GetString(receivedData));
        string rcvData = Encoding.ASCII.GetString(receivedData);
        string[] message = rcvData.Split(':');

        return message;
    }
}
