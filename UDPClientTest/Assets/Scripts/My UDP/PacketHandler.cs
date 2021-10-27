using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketHandler : MonoBehaviour 
{
    
    public static PacketHandler instance;


    private void Awake()
    {
        instance = this;
    }

    //Packet Methods

    public void InstantiatePlayer(int connectionID, GameObject PlayerPrefab){
        GameObject go = Instantiate(PlayerPrefab);
        go.name = "Player: "+connectionID;

        UDPManager.instance.playerList.Add(connectionID, go);
    }
}
