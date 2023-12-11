using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HotKey : MonoBehaviour
{
    public KeyCode key;
    public UnityEvent KeyEvent = new UnityEvent();

    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            KeyEvent?.Invoke();
        }   
    }
}
