using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EasyTriggerEvent : MonoBehaviour
{
    public bool everyOther = false;
    public bool checkTag = true;
    public bool checkName = false;
    public string target = "Player";

    public bool onEnter = true;
    public UnityEvent OnEnter = new UnityEvent();
    public bool onExit = false;
    public UnityEvent OnExit = new UnityEvent();

    void OnTriggerEnter(Collider other)
    {
        if(onEnter)
        {
            if(everyOther | (checkTag && other.tag == target) | (checkName && other.name == target))
            {
                OnEnter.Invoke();
                Debug.Log("enter");
            }
            else
            {
                Debug.Log("other.tag: " + other.tag + ", other.name: " + other.name);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(onEnter)
        {
            if(everyOther | (checkTag && other.tag == target) | (checkName && other.name == target))
            {
                OnEnter.Invoke();
                Debug.Log("enter");
            }
            else
            {
                Debug.Log("other.tag: " + other.tag + ", other.name: " + other.name);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(onExit)
        {
            if(everyOther | (checkTag && other.tag == target) | (checkName && other.name == target))
            {
                OnExit.Invoke();
                Debug.Log("exit");
            }
            else
            {
                Debug.Log("other.tag: " + other.tag + ", other.name: " + other.name);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(onExit)
        {
            if(everyOther | (checkTag && other.tag == target) | (checkName && other.name == target))
            {
                OnExit.Invoke();
                Debug.Log("exit");
            }
            else
            {
                Debug.Log("other.tag: " + other.tag + ", other.name: " + other.name);
            }
        }
    }
}
