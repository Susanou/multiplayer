using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPManager : MonoBehaviour
{
    public Dictionary<int, GameObject> playerList = new Dictionary<int, GameObject>();

    public static UDPManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float WrapEulerAngles(float rotation){
        rotation %= 360;

        if (rotation > 180){
            return -360;
        }
        return rotation;
    }

    public float UnwrapEulerAngles(float rotation){
        
        if (rotation >= 0)
            return rotation;
        
        rotation = -rotation % 360;

        return 360 - rotation;
    }
}
