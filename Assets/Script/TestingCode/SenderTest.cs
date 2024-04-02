using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
public class SenderTest : MonoBehaviour
{
    public OSCTransmitter myTransmitter;
    public const string a_address = "/Test/a";
    public const string b_address = "/Test/b";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SendMessageA()
    {
        var message = new OSCMessage(a_address);
        message.AddValue(OSCValue.String("A"));
        message.AddValue(OSCValue.Bool(true));
        message.AddValue(OSCValue.Array(OSCValue.Float(34f), OSCValue.Float(13456.2f), OSCValue.Float(4562.4545f)));
        myTransmitter.Send(message);
    }
    public void SendMessageB()
    {
        var message = new OSCMessage(b_address);
        message.AddValue(OSCValue.String("B"));
        message.AddValue(OSCValue.Bool(true));
        message.AddValue(OSCValue.Float(1.3254f));
        myTransmitter.Send(message);
    }
}
