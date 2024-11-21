using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public VirtualJoystick joystick;
    public WebSocketController webSocketController;
    public string lastCommand = "";

    void Start()
    {
        joystick.OnJoystickMove += HandleJoystickMove;    
    }

    void HandleJoystickMove(float horizontal, float vertical)
    {
        string command = "";
        if (vertical > 0.4f)
        {
            command = "forward";
        }
        else if (vertical < -0.4f)
        {
            command = "backward";
        }
        else if (horizontal > 0.4f)
        {
            command = "left";
        }
        else if (horizontal < -0.4f)
        {
            command = "right";
        }
        else
        {
            command = "stop";
        }

        if (command != lastCommand)
        {
            webSocketController.SendMessage(command);
            lastCommand = command;
        }
    }
}
