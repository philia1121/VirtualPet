using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class QuickTest : MonoBehaviour
{
    public OSCReceiver Receiver;
    void Start()
    {
        Debug.Log(Receiver.LocalHost);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
