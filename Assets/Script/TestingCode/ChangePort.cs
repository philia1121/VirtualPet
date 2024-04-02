using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
public class ChangePort : MonoBehaviour
{
    public OSCTransmitter myTransmitter;
    public string myAddress = "test";
    public string info = "random";
    public int[] myPorts = { 2040, 4080};

    void Start()
    {
        SendToAllPort();
    }
    public void SendToAllPort()
    {
        foreach(var port in myPorts)
        {
            myTransmitter.RemotePort = port;

            var message = new OSCMessage(myAddress);
            message.AddValue(OSCValue.Int(port));
            message.AddValue(OSCValue.String(info));
            myTransmitter.Send(message); 
            Debug.Log("Send: from port " + port);
        }
    }
}
