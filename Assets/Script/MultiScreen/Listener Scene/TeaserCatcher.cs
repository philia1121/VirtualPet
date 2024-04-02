using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class TeaserCatcher : MonoBehaviour
{
    public string TeaserId;
    [HideInInspector]public PropsManager propsManager;
    public bool dragging = true;
    float posZ = 0f;
    public bool durationCheck = true;
    float duration = 5f;

    [Header("Receiver Settings")]
    public OSCReceiver myReceiver;

    void Start()
    {
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
        myReceiver.LocalPort = PropsManager.propsPort;

        myReceiver.Bind(PropsManager.teaser_Address, GetTeaserInfo);
    }
    void OnMouseDown()
    {
        dragging = true;
        this.enabled = true;
        this.transform.SetSiblingIndex(-1); // show on top (if they share the same parent)
        CancelInvoke("SelfDestroy");
    }
 
    void OnMouseUp()
    {
        dragging = false;
        this.enabled = false;
        Invoke("SelfDestroy", duration);
    }

    void Update()
    {
        if (dragging)
        {
            // draggable function
            Vector3 mousePos = Input.mousePosition;
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x, Camera.main.ScreenToWorldPoint(mousePos).y, posZ);
            
            // client will send the position back to server, then server will re-send to every other client
            var message = new OSCMessage(PropsManager.teaser_Address);
            message.AddValue(OSCValue.String(TeaserId)); // 1: teaser ID
            var pos = transform.position;
            message.AddValue(OSCValue.Array(OSCValue.Float(pos.x), OSCValue.Float(pos.y), OSCValue.Float(pos.z))); // 2: teaser position
            CatcherCenter.instance.SendBackToServer(PropsManager.teaser_Address, message);
        }
    }
    void SelfDestroy()
    {
        if(durationCheck & !dragging)
        {
            propsManager.DestroyTeaser(this);
        }
    }

    void GetTeaserInfo(OSCMessage message)
    {
        var matchPattern = new OSCMatchPattern(OSCValueType.String, OSCValueType.True); // the format while teaser get spawned or destroyed
        if(!message.IsMatch(matchPattern) && message.Values[0].StringValue == TeaserId)
        {
            if(!dragging) // only update those passive teaser
            {
                var posArray = message.Values[1].ArrayValue;
                var tempPos = new Vector3(posArray[0].FloatValue, posArray[1].FloatValue, posArray[2].FloatValue);
                this.transform.position = tempPos;
            }
        }
    }
}