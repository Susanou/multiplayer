using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public enum Keys{ 
        None, 
        W, 
        A,
        S,
        D
    
    }

    public Keys pressedKey;

    // Start is called before the first frame Update
    void Start()
    {
        pressedKey = Keys.None; //we always start with no keys pressed
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput(){

        if (Input.GetKeyDown(KeyCode.W))
        {
            pressedKey = Keys.W;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            pressedKey = Keys.A;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            pressedKey = Keys.S;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            pressedKey = Keys.D;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            pressedKey = Keys.None;
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            pressedKey = Keys.None;
        }
        else if(Input.GetKeyUp(KeyCode.S))
        {
            pressedKey = Keys.None;
        }
        else if(Input.GetKeyUp(KeyCode.D))
        {
            pressedKey = Keys.None;
        }

        NetworkSend.SendKeyInput(pressedKey);
    }
}
