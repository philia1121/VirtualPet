using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
public class CameraRangeModifier : MonoBehaviour
{
    public OSCTransmitter myTransmitter;
    public const string address = "/Camera/Position";
    private bool dragging = false;
    private float posZ;

    void Awake()
    {
        posZ = transform.position.z;
    }
 
    void OnMouseDown()
    {
        dragging = true;
        this.enabled = true;
    }
 
    void OnMouseUp()
    {
        dragging = false;
        this.enabled = false;
    }
 
    void Update()
    {
        if (dragging)
        {
            Vector3 mousePos = Input.mousePosition;
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x, Camera.main.ScreenToWorldPoint(mousePos).y, posZ);
            SendPosition(address, transform.position);
        }
    }
    void SendPosition(string address, Vector3 pos)
    {
        var message = new OSCMessage(address);

		message.AddValue(OSCValue.Float(pos.x));
		message.AddValue(OSCValue.Float(pos.y));
		message.AddValue(OSCValue.Float(pos.z));
        myTransmitter.Send(message);
    }
}
