using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
public class PropsBroadcaster : MonoBehaviour
{
    public static PropsBroadcaster instance { get; private set;}
    OSCReceiver myReceiver;
    const string allProps_Address = "/Props/*";
    public const string poo_Address = "/Props/Poo";
    public const string cushion_Address = "/Props/Cushion";
    const string food_Address = "/Props/Food";
    const string ball_Address = "/Props/Ball";
    const string teaser_Address = "/Props/Teaser";
    public Transform PooParent, CushionParent, FoodParent, BallParent, TeaserParent;
    public GameObject PooPrefab, CushionPrefab, FoodPrefab, BallPrefab, TeaserPrefab; 
    public Dictionary<string, GameObject> pooDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> cushionDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> foodDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> ballDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> teaserDic = new Dictionary<string, GameObject>();
    const int propsPort = 7567;

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
        instance = this;
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
        myReceiver.LocalPort = propsPort;

        myReceiver.Bind(allProps_Address, BroadcastPropsInfo);
        myReceiver.Bind(allProps_Address, SyncAllProps); // for server sync
        myReceiver.Bind(poo_Address, SyncPoo); // poo
        myReceiver.Bind(cushion_Address, SyncCushion); // cushion
        myReceiver.Bind(food_Address, SyncFood); // food
        myReceiver.Bind(ball_Address, SyncBall); // ball
        myReceiver.Bind(teaser_Address, SyncTeaser); // teaser
    }
    void BroadcastPropsInfo(OSCMessage message)
    {
        Debug.Log("Get Props Info from client: IP " + message.Ip);
        // just broadcast whatever client offer back to all clients
        SenderCenter.instance.SendMessageToAll(message.Address, message, propsPort);
    }

    void SyncAllProps(OSCMessage message)
    {
        var matchPattern = new OSCMatchPattern(OSCValueType.String, OSCValueType.True); // the format while teaser get spawned or destroyed
        if(message.IsMatch(matchPattern))
        {
            var propsType = message.Address;
            var propsId = message.Values[0].StringValue;
            var status = message.Values[1].BoolValue;
            switch(propsType)
            {
                case cushion_Address:
                    if(status)
                        SpawnCushion(propsId);
                    else
                        DestroyCushion(propsId);
                    break;
                case food_Address:
                    if(status)
                        SpawnFood(propsId);
                    else
                        DestroyFood(propsId);
                    break;
                case ball_Address:
                    if(status)
                        SpawnBall(propsId);
                    else
                        DestroyBall(propsId);
                    break;
                case teaser_Address:
                    if(status)
                        SpawnTeaser(propsId);
                    else
                        DestroyTeaser(propsId);
                    break;
            }
        }
    }
    void SyncPoo(OSCMessage message)
    {}
    void SyncCushion(OSCMessage message)
    {}
    void SyncFood(OSCMessage message)
    {}
    void SyncBall(OSCMessage message)
    {}
    void SyncTeaser(OSCMessage message)
    {}
    
    void SpawnCushion(string id)
    {

    }
    void DestroyCushion(string id)
    {
        
    }
    void SpawnFood(string id)
    {

    }
    void DestroyFood(string id)
    {
        
    }
    void SpawnBall(string id)
    {

    }
    void DestroyBall(string id)
    {
        
    }
    void SpawnTeaser(string id)
    {

    }
    void DestroyTeaser(string id)
    {

    }
}
