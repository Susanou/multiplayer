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
    public float rotation;

    // Start is called before the first frame Update
    void Start()
    {
        pressedKey = Keys.None; //we always start with no keys pressed
    }

    // Update is called once per frame
    void Update()
    {
        CheckRotation();
        CheckCamera();
        CheckInput();

        rotation = UDPManager.instance.UnwrapEulerAngles(
            transform.localEulerAngles.y);
    }

    void FixedUpdate() {
        
    }

    private void CheckRotation(){
        gameObject.transform.localEulerAngles = new Vector3(
            gameObject.transform.localEulerAngles.x,
            UDPManager.instance.WrapEulerAngles(rotation),
            gameObject.transform.localEulerAngles.z
        );

        UDPSend.SendPlayerRotation(rotation);
    }

    private void CheckCamera(){
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength)){
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
            
            transform.LookAt(new Vector3(pointToLook.x, pointToLook.y, pointToLook.z));
        }
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

        UDPSend.SendKeyInput(pressedKey);
    }
}
