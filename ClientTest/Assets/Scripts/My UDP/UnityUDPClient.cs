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

    public GameObject PlayerPrefab;

    private bool recvloop;

    private UdpClient client;
    private Thread threadRecv;
    //private bool spawn = false; 

    private static Queue<Action> _executionQueue = new Queue<Action>();


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
/*         if (spawn){
            InstantiatePlayer();
            spawn = false;
        } */

        lock(_executionQueue){
            while(_executionQueue.Count > 0){
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    void ReceiveDataThread()
    {
        recvloop = true;

        IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 0);

        byte[] response = Encoding.ASCII.GetBytes("Hello server");
        client.Send(response, response.Length);
        byte[] receivedData = client.Receive(ref RemoteIP);
        
        Debug.Log("Received data from server "+RemoteIP.Address.ToString() + " on port "+ RemoteIP.Port.ToString());
        Debug.Log("message: " + Encoding.ASCII.GetString(receivedData));

        while (recvloop)
        {
            
            try
            {
                receivedData = client.Receive(ref RemoteIP);
                Debug.Log("Received data from server "+RemoteIP.Address.ToString() + " on port "+ RemoteIP.Port.ToString());
                Debug.Log("message: " + Encoding.ASCII.GetString(receivedData));
                string rcvData = Encoding.ASCII.GetString(receivedData);
                string[] message = rcvData.Split('\n');

                Debug.Log("message: " + message.ToString());

                if(message[0] == "I"){
                    int connectionID = Int32.Parse(message[1]);
                    _executionQueue.Enqueue((()=>InstantiatePlayer(connectionID))); // find a way to not have to use the lambda to wrap everything
                    response = Encoding.ASCII.GetBytes("Player Instantiated");
                    client.Send(response, response.Length);
                }
            }
            catch (Exception ex){
                Debug.LogError(ex.ToString());
            }
        }
    }

    private void OnApplicationQuit(){
        recvloop = false;
        byte[] response = Encoding.ASCII.GetBytes("Thank you server goodbye");
        client.Send(response, response.Length);
        client.Dispose();
    }

    private void InstantiatePlayer(int connectionID){
        GameObject go = Instantiate(PlayerPrefab);
        go.name = "Player: "+connectionID;

        UDPManager.instance.playerList.Add(connectionID, go);
    }
}
