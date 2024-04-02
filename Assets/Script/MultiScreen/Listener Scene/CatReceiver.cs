using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using UnityEngine.UI;

public class CatReceiver : MonoBehaviour
{
    public LoadCatAssets MyLoadAssets;
    public OSCReceiver myReceiver;
    const int myPort = 7567;
    [SerializeField]private string catInfo_Address = "/Cat/Info";
    [SerializeField]private RawImage catAppearance;
    Texture[] currentAnimation;
    int currentFrame;
    public Texture[] Eat, Idle, Jump, Knead, Observe, PostEat, PostKnead, PostScratch, PostWaitForEat,
        PreEat, PreKnead, PreScratch, PreWaitForEat, Scratch, Sit, SitDown, SitUp, Sleep, Stretch, WaitForEat, Walk;
    public Dictionary<string, Texture[]> AnimationDic = new Dictionary<string, Texture[]>();
    void Start()
    {
        currentAnimation = Idle;
        currentFrame = 0;

        if(!myReceiver)
        {
            myReceiver = this.gameObject.AddComponent<OSCReceiver>();
        }
        myReceiver.LocalPort = myPort;
        myReceiver.Bind(catInfo_Address, ReceiveCatInfo);
    }
    public void SetReceiverAddress(string value)
    {
        catInfo_Address = value;
        myReceiver.Bind(catInfo_Address, ReceiveCatInfo);
    }
    
    void ReceiveCatInfo(OSCMessage message)
    {
        // Get Animation Info
        var ani = message.Values[0].StringValue;
        var frame = message.Values[1].IntValue;
        currentAnimation = AnimationDic[ani];
        currentFrame = frame;
        ManualAnimation();

        // Get Position and Facing Info
        var posArray = message.Values[2].ArrayValue;
        var tempPos = new Vector3(posArray[0].FloatValue, posArray[1].FloatValue, posArray[2].FloatValue);
        this.transform.position = tempPos;
        var scaleArray = message.Values[3].ArrayValue;
        this.transform.localScale = new Vector3(scaleArray[0].FloatValue, scaleArray[1].FloatValue, scaleArray[2].FloatValue);
    }

    public void GetLoadedAssets()
    {
        AnimationDic.Clear();
        AnimationDic.Add("eat", new Texture[0]);
        AnimationDic.Add("idle", new Texture[0]);
        AnimationDic.Add("jump", new Texture[0]);
        AnimationDic.Add("knead", new Texture[0]);
        AnimationDic.Add("observe", new Texture[0]);
        AnimationDic.Add("postEat", new Texture[0]);
        AnimationDic.Add("postKnead", new Texture[0]);
        AnimationDic.Add("postScratch", new Texture[0]);
        AnimationDic.Add("postWaitForEat", new Texture[0]);
        AnimationDic.Add("preEat", new Texture[0]);
        AnimationDic.Add("preKnead", new Texture[0]);
        AnimationDic.Add("preScratch", new Texture[0]);
        AnimationDic.Add("preWaitForEat", new Texture[0]);
        AnimationDic.Add("scratch", new Texture[0]);
        AnimationDic.Add("sit", new Texture[0]);
        AnimationDic.Add("sitDown", new Texture[0]);
        AnimationDic.Add("sitUp", new Texture[0]);
        AnimationDic.Add("sleep", new Texture[0]);
        AnimationDic.Add("stretch", new Texture[0]);
        AnimationDic.Add("waitForEat", new Texture[0]);
        AnimationDic.Add("walk", new Texture[0]);

        foreach(var item in MyLoadAssets.AnimationArrayDic)
        {
            AnimationDic[item.Key] = item.Value;
            Debug.Log("Get Animation Assets: " + AnimationDic[item.Key].Length);
        }
        catAppearance.texture = AnimationDic["idle"][0];
    }

    void ManualAnimation()
    {
        try
        {
            catAppearance.texture = currentAnimation[currentFrame];
        }
        catch
        {}
    }
}
