using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
public class CatcherTest : MonoBehaviour
{
    public OSCReceiver myReceiver;
    public const string all_address = "/Test/*";
    public const string a_address = "/Test/a";
    public const string b_address = "/Test/b";
    void Start()
    {
        myReceiver.Bind(all_address, Listen_All);
        myReceiver.Bind(a_address, Listen_A);
        myReceiver.Bind(b_address, Listen_B);
    }

    public void Listen_All(OSCMessage message)
    {
        var matchPattern = new OSCMatchPattern(OSCValueType.String, OSCValueType.True);
        if(message.IsMatch(matchPattern))
        {
            Debug.Log("pattern match: " + message.Address);
        }
        else
        {
            Debug.Log("WARNING: pattern unmatch: " + message.Address);
        }
        Debug.Log(message.Values[2].FloatValue);
    }
    public void Listen_A(OSCMessage message)
    {
        
    }
    public void Listen_B(OSCMessage message)
    {

    }
    
}
