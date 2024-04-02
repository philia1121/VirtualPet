using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using TMPro;
using System;

public class SenderCenter : MonoBehaviour
{
    public static SenderCenter instance { get; private set;}
    public Transform transmitterListUI;
    public GameObject transmitterTemplate;
    public GameObject cameraRangeVisualizer;
    public List<OSCTransmitter> transmitterList = new List<OSCTransmitter>();
    public Dictionary<OSCTransmitter, SenderSelf> transmitters_Dic = new Dictionary<OSCTransmitter, SenderSelf>();
    public OSCReceiver myReceiver;
    const int myPort = 7567;
    const string twoWay_Address = "/TwoWay/Info";
    

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
        // setup receiver
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
        myReceiver.Bind(twoWay_Address, ReceiveClientConfirmation); // make sure all client know the IP of server
    }
    public void AddNewIP(string value)
    {
        if(value  == null)
            return;

        var item = Instantiate(transmitterTemplate);

        var trsmtr = item.gameObject.AddComponent<OSCTransmitter>();
        trsmtr.RemoteHost = value;
        trsmtr.RemotePort = myPort;

        item.name = "Transmitter " + value;
        item.transform.SetParent(transmitterListUI);
        item.transform.localScale = new Vector3(1, 1, 1);
        item.GetComponentInChildren<TMP_Text>().text = "IP: " + value;

        var info = item.GetComponent<SenderSelf>();
        info.MyIP = value;
        info.center = this;
        info.myTransmitter = trsmtr;
        info.visualizeColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        info.cameraRange = Instantiate(cameraRangeVisualizer, Vector3.zero, Quaternion.identity);
        info.cameraRange.GetComponent<SpriteRenderer>().color = new Color(info.visualizeColor.r, info.visualizeColor.g, info.visualizeColor.b, 0.4f);
        info.cameraPos = Vector3.zero;

        info.cameraRange.GetComponent<CameraRangeModifier>().myTransmitter = trsmtr;
        
        
        transmitterList.Add(trsmtr);
        transmitters_Dic.Add(trsmtr, info);
        
        SendServerIP(trsmtr); // send server IP to client
    }

    public void SendMessageToAll(string address, OSCMessage message, int thePort = myPort)
    {
        foreach(var sender in transmitterList)
        {
            sender.RemotePort = thePort;
            sender.Send(message);
        }
    }

    //for two way communication
    public void SendServerIP(OSCTransmitter sender)
    {
        var message = new OSCMessage(twoWay_Address);
        message.AddValue(OSCValue.String(IPv4Manager.GetLocalIPv4()));
        sender.Send(message);
    }
    public void SendServerIPToAll() //send serve IP to all client
    {
        foreach(var sender in transmitterList)
        {
            // if(!transmitters_Dic[sender].ServerIPReceived) // if client didn't know server's IP, send it to client
            // {
                var message = new OSCMessage(twoWay_Address);
                message.AddValue(OSCValue.String(IPv4Manager.GetLocalIPv4()));
                sender.Send(message);
            // }
        }
    }
    public void ReceiveClientConfirmation(OSCMessage message)
    {
        Debug.Log("on receive client confirmation");
        var value = message.Values[0].StringValue;
        Debug.Log(value);
        try
        {
            foreach(var item in transmitters_Dic)
            {
                if(item.Value.MyIP == value)
                {
                    item.Value.ServerIPReceived = true;
                    item.Value.myText.color = Color.white;
                    Debug.Log("Get confirmation from IP:" + value);
                }
            }
        }
        catch
        {}
    }
}
