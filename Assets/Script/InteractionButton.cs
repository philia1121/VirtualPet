using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InteractionButton : MonoBehaviour
{
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
    public Transform[] Boundary = new Transform[2];
    public Vector3 FoodOffest = new Vector3(-0.6f, -1.94f, 0);

    public void Clear()
    {
        // try
        // {  
        //     var food = FoodParent.GetChild(0).gameObject;
        //     if(!food.GetComponent<CatFood>().willBeEaten)
        //     {
        //         Destroy(food);
        //         RecordCSVWriter.CSV_Write("Clear", "clear food");
        //         return;
        //     }
        // }
        // catch
        // {}
        
        try
        {
            var poo = PooParent.GetChild(0).gameObject;
            Destroy(poo);
            RecordCSVWriter.CSV_Write("Clear", "clear a poo");
        }
        catch
        {
            RecordCSVWriter.CSV_Write("Clear", "there's nothing to clear");
        }
        
    }

    public void Feed()
    {
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

        string extra = (Cat.WaitForFood)? "is waiting for food": "user proactively fed the cat";
        RecordCSVWriter.CSV_Write("Feed", extra);
    }

    public void Play()
    {
        GameObject ball;
        if(!ballOnStage)
        {   
            var generatePos = new Vector3(Random.Range(Boundary[0].position.x, Boundary[1].position.x), Random.Range(Boundary[0].position.y, Boundary[1].position.y), 0);
            while(Vector3.Distance(generatePos, Cat.transform.position) < 10f) // re-generate if it's too close to the cat
            {
                generatePos = new Vector3(Random.Range(Boundary[0].position.x, Boundary[1].position.x), Random.Range(Boundary[0].position.y, Boundary[1].position.y), 0);
            } 
            ball = Instantiate(BallPrefab, generatePos, Quaternion.identity, BallParent);
            Cat.MyBall = ball.GetComponent<Ball>();
            Cat.BallTransform = ball.transform;
            ballOnStage = ball;
            Cat.GetBallPlay();
            RecordCSVWriter.CSV_Write("Play", "create new ball");
        }
        else
        {
            RecordCSVWriter.CSV_Write("Play", "there's still ball on stage");
        }
    }

    public void Call()
    {
        Cat.GetCalled();
        // Record Button data and cat's random respond result in CatStateMachine.GetCall();
    }
}
