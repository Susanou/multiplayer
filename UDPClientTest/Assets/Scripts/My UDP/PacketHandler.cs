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

    public void InstantiatePlayer(int connectionID, GameObject PlayerPrefab, bool isMyPlayer){
        GameObject go = Instantiate(PlayerPrefab);
        go.name = "Player: "+connectionID;

        if (isMyPlayer){
            go.AddComponent<InputManager>();
        }

        UDPManager.instance.playerList.Add(connectionID, go);
    }

    public void PlayerMove(int connectionID, Vector3 position){
        Debug.Log("Moving player "+connectionID);
        UDPManager.instance.playerList[connectionID].transform.position = position;
    }

}
