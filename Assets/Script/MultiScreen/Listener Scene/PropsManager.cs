using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using extOSC;

public class PropsManager : MonoBehaviour
{
    public OSCReceiver myReceiver;

    [Header("Props Parents")]
    public Transform PooParent;
    public Transform CushionParent, FoodParent, BallParent, TeaserParent;
    [Header("Props Prefabs")]
    public GameObject PooPrefab;
    public GameObject CushionPrefab, FoodPrefab, BallPrefab, TeaserPrefab; 
    public Dictionary<string, GameObject> pooDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> cushionDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> foodDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> ballDic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> teaserDic = new Dictionary<string, GameObject>();
    public const string allProps_Address = "/Props/*";
    public const string poo_Address = "/Props/Poo";
    public const string cushion_Address = "/Props/Cushion";
    public const string food_Address = "/Props/Food";
    public const string ball_Address = "/Props/Ball";
    public const string teaser_Address = "/Props/Teaser";
    public const int propsPort = 7567;
    void Start()
    {
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
        myReceiver.LocalPort = propsPort;

        myReceiver.Bind(allProps_Address, GetPropsInfo); // All in one
        myReceiver.Bind(poo_Address, GetPooInfo); // poo
        myReceiver.Bind(cushion_Address, GetCushionInfo); // cushion
        myReceiver.Bind(food_Address, GetFoodInfo); // food
        myReceiver.Bind(ball_Address, GetBallInfo); // ball
        myReceiver.Bind(teaser_Address, GetTeaserInfo); // teaser
    }

    #region Receiver Bind Function
    void GetPropsInfo(OSCMessage message)
    {}
    void GetPooInfo(OSCMessage message)
    {
        var propsId = message.Values[0].StringValue;
        var status = message.Values[1].BoolValue;
        var posArray = message.Values[2].ArrayValue;
        Vector3 pos = new Vector3(posArray[0].FloatValue, posArray[1].FloatValue, posArray[2].FloatValue);

        if(status)
            SpawnPoo(propsId, pos);
        else
            DestroyPoo(propsId);
    }
    void GetCushionInfo(OSCMessage message)
    {}
    void GetFoodInfo(OSCMessage message)
    {}
    void GetBallInfo(OSCMessage message)
    {}
    void GetTeaserInfo(OSCMessage message)
    {
        var propsId = message.Values[0].StringValue;
        var status = message.Values[1].BoolValue;
        if(status)
            SpawnTeaser(propsId);
        else
            DestroyTeaser(propsId);
    }
    #endregion

    #region Poo
    // only get to native spawn by cat in server
    void SpawnPoo(string id, Vector3 pos) // pasive spawn
    {
        // spawn the gameObject
        var item = Instantiate(PooPrefab);
        item.name = "Poo_" + id;
        item.transform.SetParent(PooParent);
        pooDic.Add(id, item);
        PooCatcher poo = item.GetComponent<PooCatcher>();
        poo.propsManager = this;
        poo.PooId = id;
    }
    public void DestroyPoo(PooCatcher pooScript) // native destroy
    {
        // destroy the gameObject
        Destroy(pooScript.gameObject);
        teaserDic.Remove(pooScript.PooId);

        // tell the server that this poo is destroyed
        var message = new OSCMessage(poo_Address);
        message.AddValue(OSCValue.String(pooScript.PooId)); // 1: poo ID
        message.AddValue(OSCValue.Bool(false)); // 2: poo removed
        CatcherCenter.instance.SendBackToServer(poo_Address, message);
    }
    void DestroyPoo(string id) // passive destroy
    {
        if(pooDic.ContainsKey(id))
        {
            Destroy(pooDic[id]);
            pooDic.Remove(id);
        }
    }
    #endregion

    #region Cushion
    public void SpawnCushion()
    {

    }
    void SpawnCushion(string id)
    {

    }
    public void DestroyCushion()
    {

    }
    void DestroyCushion(string id)
    {

    }
    #endregion

    #region Food
    public void SpawnFood()
    {

    }
    void SpawnFood(string id)
    {

    }
    public void DestroyFood()
    {

    }
    void DestroyFood(string id)
    {

    }
    #endregion

    #region Ball
    public void SpawnBall()
    {

    }
    void SpawnBall(string id)
    {

    }
    public void DestroyBall()
    {

    }
    void DestroyBall(string id)
    {

    }
    #endregion

    #region Teaser
    public void SpawnTeaser() // native spawn
    {
        // spawn the gameObject
        var item = Instantiate(TeaserPrefab);
        var id = System.Guid.NewGuid().ToString();
        item.name = "Teaser_" + id;
        item.transform.SetParent(TeaserParent);
        teaserDic.Add(id, item);
        TeaserCatcher teaser = item.GetComponent<TeaserCatcher>();
        teaser.propsManager = this;
        teaser.enabled = true;
        teaser.dragging = true;
        teaser.TeaserId = id;
        
        // tell the server that this teaser is spawned
        var message = new OSCMessage(teaser_Address);
        message.AddValue(OSCValue.String(id)); // 1: teaser ID
        message.AddValue(OSCValue.Bool(true)); // 2: new teaser spawned
        CatcherCenter.instance.SendBackToServer(teaser_Address, message);
    }
    void SpawnTeaser(string id) // passive spawn
    {
        if(!teaserDic.ContainsKey(id))
        {
            var item = Instantiate(TeaserPrefab);
            item.name = "Teaser " + id;
            item.transform.SetParent(TeaserParent);
            teaserDic.Add(id, item);
            TeaserCatcher teaser = item.GetComponent<TeaserCatcher>();
            teaser.propsManager = this;
            teaser.enabled = false;
            teaser.dragging = false;
            teaser.TeaserId = id;
        }
    }
    public void DestroyTeaser(TeaserCatcher teaserScript) // native destroy
    {
        // destroy the gameObject
        Destroy(teaserScript.gameObject);
        teaserDic.Remove(teaserScript.TeaserId);

        // tell the server that this teaser is destroyed
        var message = new OSCMessage(teaser_Address);
        message.AddValue(OSCValue.String(teaserScript.TeaserId)); // 1: teaser ID
        message.AddValue(OSCValue.Bool(false)); // 2: teaser removed
        CatcherCenter.instance.SendBackToServer(teaser_Address, message);
    }
    void DestroyTeaser(string id) // passive destroy
    {
        if(teaserDic.ContainsKey(id))
        {
            Destroy(teaserDic[id]);
            teaserDic.Remove(id);
        }
    }
    #endregion
}
