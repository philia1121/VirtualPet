using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class CatStateMachine : MonoBehaviour
{
    [Header("StateMachine")]
    CatBaseState _currentState;
    CatStateFactory _states;

    [Header("Testing")]
    public bool ManualControl = false;
    public float ManualStateNum = 0;
    public bool shorterSwitchDuration = false;

    [Header("Manual Animation")]
    [SerializeField]private RawImage catAppearance;
    RectTransform catTransform;
    Texture[] currentAnimation, nextAnimation;
    public Texture[] Eat, Idle, Jump, Knead, Observe, PostEat, PostKnead, PostScratch, PostWaitForEat,
        PreEat, PreKnead, PreScratch, PreWaitForEat, Scratch, Sit, SitDown, SitUp, Sleep, Stretch, WaitForEat, Walk;
    bool nextAnimationAwait = false;
    bool transitioning = false;
    int currentFrame = 0;
    float frameDuration = 0.015f;
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


    #region Variables
    public CatBaseState CurrentState { get{ return _currentState;} set{ _currentState = value;}}
    public bool NextAnimationAwait { get{ return nextAnimationAwait;} set{ nextAnimationAwait = value;}}
    public bool Transitioning { get{ return transitioning;} set{ transitioning = value;}}
    public Texture[] NextAnimation { get{ return nextAnimation;} set{ nextAnimation = value;}}
    public bool UseTransitionSpeedUp { get{ return useTransitionSpeedUp;} set{ useTransitionSpeedUp = value;}}
    public bool UseAdditionalSpeedAdjust { get{ return useAdditionalSpeedAdjust;} set{ useAdditionalSpeedAdjust = value;}}
    public float AdditionalSpeedAdjust { get{ return additionalSpeedAdjust;} set{ additionalSpeedAdjust = value;}}
    public bool WaitForFood { get{ return waitForFood;} set{ waitForFood = value;}}
    public bool GetFood { get{ return getFood;} set{ getFood = value;}}
    public bool Calling { get{ return calling;} set{ calling = value;}}
    public bool BallApears { get{ return ballAppears;} set{ ballAppears = value;}}
    public bool FoodOnStage { get{ return foodOnStage;} set{ foodOnStage = value;}}   

    
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

    #endregion


    void Awake()
    {
        _states = new CatStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();

        catTransform = catAppearance.GetComponent<RectTransform>();

        currentAnimation = Idle;
        animationPlayer = ManualAnimationPlayer();
        StartCoroutine(animationPlayer);
    }

    void Update()
    {
        _currentState.UpdateState();
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
                    currentAnimation = nextAnimation;
                    nextAnimation = null;
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
        if(shorterSwitchDuration)
        {
            minDuration = 3;
            maxDuration = 10;
        }
        StartCoroutine(RandomTimer(minDuration, maxDuration, autoSwitchState));
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
        }
        else
        {
            if(Random.Range(0, 1f) <= 0.7f) // might ignore the food
            {
                foodOnStage = true;
                food.willBeEaten = true;
            }
        }
        MyCatFood = food;
        food.durationCheck = false;
    }
    public void GetCalled()
    {   
        Audio_AnswerCall();

        if(!calling) // if cat has decide to answer your call, it WILL answer your (eventually)
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
            item.transform.SetParent(ObjectParents[0]);
            item.transform.position = catTransform.position;
            RecordCSVWriter.CSV_Write("Cat", "take a shit");
        }
    }

    public void PlaceCushion(Vector3 pos)
    {
        var item = Instantiate(Cushion);
        item.transform.SetParent(ObjectParents[1]);
        item.transform.position = pos;
    }
    public void RemoveCushion()
    {
        Destroy(ObjectParents[1].GetChild(0).gameObject);
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
