using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaymakNetwork;

public class NetworkManager : MonoBehaviour
{
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
}
