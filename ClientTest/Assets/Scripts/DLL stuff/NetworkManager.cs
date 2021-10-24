using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaymakNetwork;

public class NetworkManager : MonoBehaviour
{

    public GameObject playerPref;

    public static NetworkManager instance;

    private void Awake(){
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        NetworkConfig.InitNetwork();
        NetworkConfig.ConnectToServer();
    }

    private void OnApplicationQuit(){
        NetworkConfig.DisconnectFromServer();
    }

    public void InstantiateNetworkPlayer(int connectionID){
        GameObject go = Instantiate(playerPref);
        go.name = "Player: "+connectionID;

        GameManager.instance.playerList.Add(connectionID, go);
    }
}
