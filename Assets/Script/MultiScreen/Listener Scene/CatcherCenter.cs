using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using UnityEngine.Events;
using TMPro;
public class CatcherCenter : MonoBehaviour
{
    public TextMeshProUGUI IP_text;
    public static CatcherCenter instance { get; private set;}
    public OSCTransmitter myTransmitter;
    public OSCReceiver myReceiver;
    public bool localTesting = false;
    string myRemoteHost;
    const int myPort = 7567;
    const string twoWay_address = "/TwoWay/Info";
    public UnityEvent ReceivedRemoteIP_Event = new UnityEvent();

    void Awake() 
    {
        if (instance != null) 
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    void Start()
    {
        if(!myTransmitter)
        {
            if(this.TryGetComponent<OSCTransmitter>(out var transmitter))
            {
                myTransmitter = transmitter;
            }
            else
            {
                myTransmitter = this.gameObject.AddComponent<OSCTransmitter>();
            }
        }
        myTransmitter.RemotePort = myPort;

        if(!myReceiver)
        {
            if(this.TryGetComponent<OSCReceiver>(out var receiver))
            {
                myReceiver = receiver;
            }
            else
            {
                myReceiver = this.gameObject.AddComponent<OSCReceiver>();
            }
        }
        myReceiver.LocalPort = myPort;

        myReceiver.Bind(twoWay_address, GetRemoteTransmitterIP);
    }

    void GetRemoteTransmitterIP(OSCMessage message)
    {
        // get the remote server's IP
        myTransmitter.RemoteHost = message.Values[0].StringValue;
        myRemoteHost = message.Values[0].StringValue;
        ReceivedRemoteIP_Event.Invoke();
        IP_text.color = Color.green;

        // send message back to server as confirmation
        var confirmation = new OSCMessage(twoWay_address);
        confirmation.AddValue(OSCValue.String(IPv4Manager.GetLocalIPv4()));
        myTransmitter.Send(confirmation);
    }

    public void SendBackToServer(string address, OSCMessage message, int thePort = myPort)
    {
        if(myRemoteHost != null | localTesting) // check if the server IP is revealed
        {
            myTransmitter.RemotePort = thePort;
            myTransmitter.Send(message);
        }
    }
}
