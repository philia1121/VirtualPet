using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class CameraReceiver : MonoBehaviour
{
    public OSCReceiver myReceiver;
    public const int myPort = 7567;
    public const string cameraPos_Address = "/Camera/Position";
    void Start()
    {
        if(!myReceiver)
        {
            if(this.gameObject.TryGetComponent<OSCReceiver>(out OSCReceiver receiver))
            {
                myReceiver = receiver;
            }
            else
            {
                myReceiver = this.gameObject.AddComponent<OSCReceiver>();
            }
        }
        myReceiver.LocalPort = myPort;
        myReceiver.Bind(cameraPos_Address, ReceivePosition);
    }

    void ReceivePosition(OSCMessage message)
    {
        if(message.ToVector3(out var value))
        {
            this.transform.position = new Vector3(value.x, value.y, this.transform.position.z);
        }
    }
}
