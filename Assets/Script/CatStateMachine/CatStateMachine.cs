using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using extOSC;

[RequireComponent(typeof(LoadCatAssets))]
public class CatStateMachine : MonoBehaviour
{
    [Header("StateMachine")]
    CatBaseState _currentState;
    CatStateFactory _states;

    [Header("Testing")]
    public bool ManualControl = false;
    public float ManualStateNum = 0;
    public bool ShorterSwitchDuration = false;
    public float[] ShorterDuration = new float[]{ 3f, 10f};

    [Header("OSC Settings")]
    public bool IllusionMode = false;
    public bool SendOSCMessage = true;
    public string catInfo_Address = "/Cat/Info";

    [Header("Manual Animation")]
    public bool FastAssetsMode = false;
    public LoadCatAssets MyLoadAssets;
    [SerializeField]private RawImage catAppearance;
    RectTransform catTransform;
    Texture[] currentAnimation;
    string currentAnimation_str, nextAnimation_str;
    public Dictionary<string, Texture[]> AnimationDic = new Dictionary<string, Texture[]>();
    bool nextAnimationAwait = false;
    bool transitioning = false;
    int currentFrame = 0;
    float frameDuration = 0.015f; // should be set to 0.015f
    bool useTransitionSpeedUp = false;
    float transitionSpeedUp = 0.1f;
    bool useAdditionalSpeedAdjust = false;
    float additionalSpeedAdjust;
    IEnumerator animationPlayer;
    bool timeUp = false;
    bool simpleTimeUp = false;

    [Header("Movement")]
    public Transform[] Boundary = new Transform[2];
    public float WalkSpeed = 1f;

    [Header("Audio")]
    [SerializeField]private AudioSource myAudioSource;
    [SerializeField]private AudioMixer myAudioMixer;
    [SerializeField]private AudioClip[] myAudioClips;

    [Header("Interactive Props")]
    public GameObject Poo;
    public GameObject Cushion;
    public Transform[] ObjectParents = new Transform[2];
    public Transform[] ObjectBoundary = new Transform[2];
    [HideInInspector]public CatFood MyCatFood;
    bool waitForFood = false;
    bool getFood = false;
    bool foodOnStage = false;
    bool calling = false;
    public Vector3 AnswerCallPos = new Vector3(0, -2f, 0);
    [HideInInspector]public Transform BallTransform;
    [HideInInspector]public Ball MyBall;
    bool ballAppears = false;

    [Header("Interact")]
    [SerializeField]private bool beDragged = false;
    public bool DragBanned = false;

    #region Variables
    public CatBaseState CurrentState { get{ return _currentState;} set{ _currentState = value;}}
    public bool NextAnimationAwait { get{ return nextAnimationAwait;} set{ nextAnimationAwait = value;}}
    public bool Transitioning { get{ return transitioning;} set{ transitioning = value;}}
    public bool UseTransitionSpeedUp { get{ return useTransitionSpeedUp;} set{ useTransitionSpeedUp = value;}}
    public bool UseAdditionalSpeedAdjust { get{ return useAdditionalSpeedAdjust;} set{ useAdditionalSpeedAdjust = value;}}
    public float AdditionalSpeedAdjust { get{ return additionalSpeedAdjust;} set{ additionalSpeedAdjust = value;}}
    public bool WaitForFood { get{ return waitForFood;} set{ waitForFood = value;}}
    public bool GetFood { get{ return getFood;} set{ getFood = value;}}
    public bool Calling { get{ return calling;} set{ calling = value;}}
    public bool BallApears { get{ return ballAppears;} set{ ballAppears = value;}}
    public bool FoodOnStage { get{ return foodOnStage;} set{ foodOnStage = value;}}
    public string NextAnimation_str { get{ return nextAnimation_str;} set{ nextAnimation_str = value;}}

    
    // Readable only
    public RawImage CatAppearance { get{ return catAppearance;}}
    public RectTransform CatTransform { get{ return catTransform;}}
    public int SimpleCurrentAnimationProgress { get{ return currentFrame/currentAnimation.Length;}} // result will only be 0 or 1 since it's integer division, add (float) in front of one of the operands then you will get result between 0 and 1
    public float CurrentAnimationProgress { get{ return (float)currentFrame/currentAnimation.Length;}}
    public bool TimeUp { get{ return timeUp;}}
    public bool SimpleTimeUp { get{ return simpleTimeUp;}}
    public AudioSource MyAudioSource { get{ return myAudioSource;}}
    public AudioClip[] MyAudioClips { get{ return myAudioClips;}}
    public float AudioPlayingProgress { get{ return myAudioSource.time/myAudioSource.clip.length;}}
    public bool BeDragged { get{ return beDragged;}}

    #endregion


    void Start()
    {
        if(FastAssetsMode)
        {
            GetLoadedAssets();
        }
        _states = new CatStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();

        catTransform = catAppearance.GetComponent<RectTransform>();

        // currentAnimation = Idle;
        currentAnimation_str = "idle";
        currentAnimation = AnimationDic[currentAnimation_str];
        animationPlayer = ManualAnimationPlayer();
        StartCoroutine(animationPlayer);
    }

    void Update()
    {
        _currentState.UpdateState();

        Send_CatInfo();
    }
    void OnMouseUp()
    {
        beDragged = false;
    }
    void OnMouseDown()
    {
        Debug.Log("cat on click");
        beDragged = true;
        if(!DragBanned)
        {
            CurrentState.ExitState();
            _currentState = _states.Dragged();
            _currentState.EnterState();
        }
    }

    public void GetLoadedAssets()
    {
        AnimationDic.Clear();

        //Initialize AnimationDic
        string[] allAniState = new string[]{ "eat", "idle", "jump", "knead", 
                "observe", "postEat", "postKnead", "postScratch", "postWaitForEat", "preEat", "preKnead", "preScratch",
                "preWaitForEat", "scratch", "sit", "sitDown", "sitUp", "sleep", "stretch", "waitForEat", "walk"};
        foreach(var item in allAniState)
        {
            AnimationDic.Add(item, new Texture[0]);
        }

        // Load Asset
        if(FastAssetsMode)
        {
            var temp = new Texture[]{catAppearance.texture};
            foreach(var item in allAniState)
            {
                AnimationDic[item] = temp;
            }
        }
        else
        {
            foreach(var item in MyLoadAssets.AnimationArrayDic)
            {
                AnimationDic[item.Key] = item.Value;
            }
        }
        catAppearance.texture = AnimationDic["idle"][0];
    }

    public IEnumerator ManualAnimationPlayer()
    {
        while(true)
        {
            if(currentFrame < currentAnimation.Length)
            {
                catAppearance.texture = currentAnimation[currentFrame];
                currentFrame++;
            }
            else
            {
                if(nextAnimationAwait)
                {
                    // currentAnimation = nextAnimation;
                    currentAnimation = AnimationDic[nextAnimation_str];
                    currentAnimation_str = nextAnimation_str;
                    nextAnimationAwait = false;
                    transitioning = false;
                }
                currentFrame = 0;
                catAppearance.texture = currentAnimation[currentFrame];
                currentFrame ++;
            }

            var playingSpeed = (useTransitionSpeedUp && transitioning) ? frameDuration* transitionSpeedUp : frameDuration; //speed up the transition
            playingSpeed = useAdditionalSpeedAdjust ? playingSpeed* additionalSpeedAdjust : playingSpeed; //speed up the playing speed with additional adjustment value 
            

            yield return new WaitForSecondsRealtime(playingSpeed);
        }
    }

    void Send_CatInfo() //general info
    {
        if(SendOSCMessage)
        {
            // Debug.Log("send--------------------------------");
            var message = new OSCMessage(catInfo_Address);

            message.AddValue(OSCValue.String(currentAnimation_str)); // 0: currentAnimation
            message.AddValue(OSCValue.Int(currentFrame)); // 1: currentFrame
            
            var currentPos = CatTransform.position; // 2: current Position
            message.AddValue(OSCValue.Array(OSCValue.Float(currentPos.x), OSCValue.Float(currentPos.y), OSCValue.Float(currentPos.z)));

            var currentFacing = CatTransform.localScale; // 3: current LocalScale
            message.AddValue(OSCValue.Array(OSCValue.Float(currentFacing.x), OSCValue.Float(currentFacing.y), OSCValue.Float(currentFacing.z)));


            SenderCenter.instance.SendMessageToAll(catInfo_Address, message);
        }
    }

    public IEnumerator RandomTimer(float min, float max, bool auto)
    {
        float randomTime = Random.Range(min, max);
        // Debug.Log("will get random state after " + randomTime + " sec.");
        yield return new WaitForSeconds(randomTime);

        timeUp = true;
        if(auto)
        {
            _currentState.CheckSwitchStates();
        }
    }

    public void CallRandomSwitchState(bool autoSwitchState = false, float minDuration = 180, float maxDuration = 900)
    {
        timeUp = false;
        if(ShorterSwitchDuration)
        {
            minDuration = ShorterDuration[0];
            maxDuration = ShorterDuration[1];
        }
        StartCoroutine(RandomTimer(minDuration, maxDuration, autoSwitchState));
    }
    public void StopRandomTimer()
    {
        StopCoroutine("RandomTimer");
    }
    
    public IEnumerator RandomSimpleTimer(float min, float max)
    {
        float randomTime = Random.Range(min, max);
        yield return new WaitForSeconds(randomTime);

        simpleTimeUp = true;
    }
    public void CallRandomSimpleTimer(float minDuration = 3, float maxDuration = 10)
    {
        simpleTimeUp = false;
        StartCoroutine(RandomSimpleTimer(minDuration, maxDuration));
    }

    public void GetFed(CatFood food)
    {
        if(waitForFood)
        {
            getFood = true;
            food.willBeEaten = true;
            food.durationCheck = false;
            food.eatingCount++;
        }
        else
        {
            Debug.Log("Get a Food");
            if(Random.Range(0, 1f) <= 0.7f) // might ignore the food
            {
                foodOnStage = true;
                food.willBeEaten = true;
                food.durationCheck = false;
                food.eatingCount++;
            }
        }
        MyCatFood = food;
    }
    public void GetCalled()
    {   
        Audio_AnswerCall();

        if(!calling) // if cat has decide to answer your call, it WILL answer to you (eventually)
        {
            if(Random.Range(0, 1f) <= 0.4f & (Vector3.Distance(CatTransform.position, AnswerCallPos) > 0.05f)) // will respond to the call
            {
                calling = true;
                RecordCSVWriter.CSV_Write("Call", "successful call");
            }
            else
            {
                RecordCSVWriter.CSV_Write("Call", "no response");
            }
        }
        else
        {
            RecordCSVWriter.CSV_Write("Call", "invalid call");
        }
    }
    public void GetBallPlay()
    {   
        if(Random.Range(0, 1f) <= 0.9f) // will observe the ball, otherwise ignore it
        {
            Debug.Log("will play");
            ballAppears = true;
            MyBall.ObserveBall();
        }
        else
        {
            Debug.Log("ignore ball");
            MyBall.SelfDestroy(true);
        }
    }

    public void RandomTakeShit(float chance)
    {
        var posX = CatTransform.position.x;
        var posY = CatTransform.position.y;
        var minX = (ObjectBoundary[0].position.x < ObjectBoundary[1].position.x)? ObjectBoundary[0].position.x : ObjectBoundary[1].position.x;
        var maxX = (ObjectBoundary[0].position.x > ObjectBoundary[1].position.x)? ObjectBoundary[0].position.x : ObjectBoundary[1].position.x;
        var minY = (ObjectBoundary[0].position.y < ObjectBoundary[1].position.y)? ObjectBoundary[0].position.y : ObjectBoundary[1].position.y;
        var maxY = (ObjectBoundary[0].position.y > ObjectBoundary[1].position.y)? ObjectBoundary[0].position.y : ObjectBoundary[1].position.y;
        
        if((posX > minX & posX < maxX) & (posY > minY & posY < maxY) && Random.Range(0, 1f) <= chance)
        {
            var item = Instantiate(Poo);
            var id = System.Guid.NewGuid().ToString();
            item.name = "Poo_" + id;
            item.transform.SetParent(ObjectParents[0]);
            item.transform.position = catTransform.position;
            // RecordCSVWriter.CSV_Write("Cat", "take a shit");

            Send_PooInfo(id, item);
        }
    }
    void Send_PooInfo(string pooId, GameObject poo)
    {
        if(SendOSCMessage)
        {
            PropsBroadcaster.instance.pooDic.Add(pooId, poo);

            var message = new OSCMessage(PropsBroadcaster.poo_Address);
            message.AddValue(OSCValue.String(pooId)); // 1: poo ID
            message.AddValue(OSCValue.Bool(true)); // 2: poo spawned
            var pooPos = CatTransform.position; // 3: poo position
            message.AddValue(OSCValue.Array(OSCValue.Float(pooPos.x), OSCValue.Float(pooPos.y), OSCValue.Float(pooPos.z)));
            SenderCenter.instance.SendMessageToAll(PropsBroadcaster.poo_Address, message);
        }
    }

    public void PlaceCushion(Vector3 pos) // cat itself place its cushion
    {
        var item = Instantiate(Cushion);
        item.name = "CushionSpawnedByCat"; // cat itself 
        item.transform.SetParent(ObjectParents[1]);
        item.transform.position = pos;

        Send_CushionInfo(item.name, pos);
    }
    void Send_CushionInfo(string cushionId, Vector3 cushionPos)
    {
        if(SendOSCMessage)
        {
            var message = new OSCMessage(PropsBroadcaster.cushion_Address);
            message.AddValue(OSCValue.String(cushionId)); // 1: cushion id
            message.AddValue(OSCValue.Bool(true)); // 2: cushion spawned
            message.AddValue(OSCValue.Array(OSCValue.Float(cushionPos.x), OSCValue.Float(cushionPos.y), OSCValue.Float(cushionPos.z))); // 3: cushion position
            SenderCenter.instance.SendMessageToAll(PropsBroadcaster.cushion_Address, message);
        }
    }
    public void RemoveCushionByName(string name)
    {
        Destroy(ObjectParents[1].Find(name).gameObject);
    }

    public void Audio_AnswerCall()
    {
        var clip = (Random.Range(0,1f) <= 0.5f)? myAudioClips[0] : myAudioClips[1];
        myAudioSource.clip = clip;
        myAudioSource.loop = false;
        myAudioSource.Play();
    } 
    public void Audio_CallForFood()
    {
        var clip = (Random.Range(0,1f) <= 0.5f)? myAudioClips[2] : myAudioClips[8];
        myAudioSource.clip = clip;
        myAudioSource.loop = false;
        myAudioSource.Play();
    } 
    public void Audio_SleepingPurr()
    {
        var clip = (Random.Range(0,1f) <= 0.5f)? myAudioClips[6] : myAudioClips[7];
        myAudioSource.clip = clip;
        myAudioSource.loop = true;
        myAudioSource.Play();
    }
    public void Audio_Eating()
    {
        var clip = (Random.Range(0,1f) <= 0.5f)? myAudioClips[3] : myAudioClips[4];
        myAudioSource.clip = clip;
        myAudioSource.loop = true;
        myAudioSource.Play();
    }
    public void Audio_KneadingPurr()
    {
        myAudioSource.clip = myAudioClips[5];
        myAudioSource.loop = true;
        myAudioSource.Play();
    }
}
