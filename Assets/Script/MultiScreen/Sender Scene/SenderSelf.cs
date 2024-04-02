using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using TMPro;
public class SenderSelf : MonoBehaviour
{
    public string MyIP;
    public bool ServerIPReceived = false;
    public TextMeshProUGUI myText;
    public SenderCenter center;
    public OSCTransmitter myTransmitter;
    public GameObject cameraRange;
    public Color visualizeColor;
    public Vector3 cameraPos;
    public void Remove()
    {
        try
        {
            center.transmitterList.Remove(myTransmitter);
            center.transmitters_Dic.Remove(myTransmitter);
        }
        catch
        {}
        
        Destroy(cameraRange);

        Destroy(this.gameObject);
    }
}
