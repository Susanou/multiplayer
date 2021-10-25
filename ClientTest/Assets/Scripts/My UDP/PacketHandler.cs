using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PacketHandler : MonoBehaviour
{

    public static readonly Queue<Action> _executionQueue = new Queue<Action>();


    // Start is called before the first frame update
    void Start()
    {
        
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
}
