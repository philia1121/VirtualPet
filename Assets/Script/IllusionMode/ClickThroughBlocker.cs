using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ClickThroughBlocker : MonoBehaviour
{
    public MyInputMap Input;
    public bool state = false; //block
    public UnityEvent Blocker_On, Blocker_Off = new UnityEvent();
    void Awake()
    {
        if(Input == null)
        {
            Input = new MyInputMap();
        }
        
        Input.Player.Blocker.started += ctx => OpenBlocker();
    }
    void Start()
    {
        if(state)
        {
            Blocker_Off.Invoke();
        }
        else
        {
            Blocker_On.Invoke();
        }
    }
    public void OpenBlocker()
    {
        state = !state;
        if(state)
        {
            Blocker_Off.Invoke();
        }
        else
        {
            Blocker_On.Invoke();
        }
    }
    
    void OnEnable()
    {
        Input.Player.Enable();
    }
    void OnDisable()
    {
        Input.Player.Disable();
    }
}
