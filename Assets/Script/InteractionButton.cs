using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InteractionButton : MonoBehaviour
{
    [Header("CSV Writer")]
    public string fileName;
    string fileType = ".csv";
    string filePath; 

    [Header("Cat")]
    public CatStateMachine Cat;

    [Header("Interactive Props")]
    public Transform PooParent;
    public Transform FoodParent;
    public Transform BallParent;
    public GameObject FoodPrefab;
    public GameObject BallPrefab;
    GameObject foodAwaitInCenter;
    GameObject ballOnStage;
    string ballStatues;
    public Vector3 FoodOffest = new Vector3(-0.6f, -1.94f, 0);

    public void Start()
    {
        // data writer config
        if(fileName == null)
        {
            fileName = "test";
        }
        string fileFullName = fileName + fileType;
        filePath = Path.Combine(Application.persistentDataPath, fileFullName);
    }

    public void Clear()
    {
        RecordButton("Clear");
        try
        {  
            var food = FoodParent.GetChild(0).gameObject;
            if(!food.GetComponent<CatFood>().willBeEaten)
            {
                Destroy(food);
                return;
            }
        }
        catch
        {}
        
        try
        {
            var poo = PooParent.GetChild(0).gameObject;
            Destroy(poo);
        }
        catch
        {}
        
    }

    public void Feed()
    {
        RecordButton("Feed");
        GameObject food;
        if(Cat.WaitForFood)
        {
            food = Instantiate(FoodPrefab, Cat.CatTransform.position + FoodOffest, Quaternion.identity, FoodParent);
        }
        else
        {
            if(foodAwaitInCenter)
            {
                var oldFood = foodAwaitInCenter;
                Destroy(oldFood);
            }
            food = Instantiate(FoodPrefab, new Vector3(0, -2f, 0), Quaternion.identity, FoodParent);
            foodAwaitInCenter = food;
        }

        var foodComp = food.GetComponent<CatFood>();
        Cat.GetFed(foodComp);
    }

    public void Play()
    {
        RecordButton("Play");
        GameObject ball;
        if(!ballOnStage)
        {   
            var generatePos = new Vector3(Random.Range(-8.8f, 8.8f), Random.Range(-5f, 4.7f), 0);
            while(Vector3.Distance(generatePos, Cat.transform.position) < 10f)
            {
                Debug.Log("too close");
                generatePos = new Vector3(Random.Range(-8.8f, 8.8f), Random.Range(-5f, 4.7f), 0);
            } 
            ball = Instantiate(BallPrefab, new Vector3(Random.Range(-8.8f, 8.8f), Random.Range(-5f, 4.7f), 0), Quaternion.identity, BallParent);
            Cat.MyBall = ball.GetComponent<Ball>();
            Cat.BallTransform = ball.transform;
            ballOnStage = ball;
            Cat.GetBallPlay();
            ballStatues = "create new ball";
        }
        else
        {
            ballStatues = "nothing happens";
        }
        
    }

    public void Call()
    {
        RecordButton("Call");
        Cat.GetCalled();
    }

    public void RecordButton(string button)
    {
        TextWriter tw = new StreamWriter(filePath, true);
        string content = button + "," + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        if(button == "Feed")
        {
            if(Cat.WaitForFood)
            {
                content = content + "," + "is waiting for food";
            }
            else
            {
                content = content + "," + "user proactively fed the cat";
            }
        }
        if(button == "Play")
        {
            content = content + "," + ballStatues;
        }
        tw.WriteLine(content);
        tw.Close(); //end writing file function
    }
}
