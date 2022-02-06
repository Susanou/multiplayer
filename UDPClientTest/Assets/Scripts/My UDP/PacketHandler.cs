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
        //Debug.Log("Moving player "+connectionID);
        UDPManager.instance.playerList[connectionID].transform.position = position;
    }

    public void PlayerRotation(int connectionID, float rotation, bool isMyPlayer){
        UDPManager.instance.WrapEulerAngles(rotation);

        

        if (isMyPlayer) return;
        if (!UDPManager.instance.playerList.ContainsKey(connectionID)) return;
        
        Debug.Log(isMyPlayer);

        UDPManager.instance.playerList[connectionID].transform.rotation = new Quaternion(0, rotation, 0, 0);
    }

}
