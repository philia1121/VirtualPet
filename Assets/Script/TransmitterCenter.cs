using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using TMPro;

public class TransmitterCenter : MonoBehaviour
{
    public Transform transmitterList;
    public GameObject transmitterTemplate;

    public List<OSCTransmitter> transmitters = new List<OSCTransmitter>();
    public void AddNewPort(string value)
    {
        if(value  == null)
            return;
        
        
        var item = Instantiate(transmitterTemplate);
        item.name = "Transmitter " + value;
        item.transform.SetParent(transmitterList);
        item.transform.localScale = new Vector3(1, 1, 1);
        item.GetComponentInChildren<TMP_Text>().text = "Port: " + value;
        var temp = item.gameObject.AddComponent<OSCTransmitter>();
        temp.RemoteHost = value;
        transmitters.Add(temp);
    }

    public void AddNewIP(string value)
    {
        if(value  == null)
            return;
        
        
        var item = Instantiate(transmitterTemplate);
        item.name = "Transmitter " + value;
        item.transform.SetParent(transmitterList);
        item.transform.localScale = new Vector3(1, 1, 1);
        item.GetComponentInChildren<TMP_Text>().text = "IP: " + value;
        var temp = item.gameObject.AddComponent<OSCTransmitter>();
        temp.RemoteHost = value;
        transmitters.Add(temp);
    }
    public void AddTransmitter()
    {

    }
    public void RemoveTransmitter()
    {

    }
    public void SendSomething(string address)
    {
        var message = new OSCMessage(address);
        message.AddValue(OSCValue.String("do the thing"));

        foreach(var sender in transmitters)
        {
            sender.Send(message);
        }
    }
}
