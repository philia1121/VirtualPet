using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BackgroundImageManager : MonoBehaviour
{
    public MyInputMap Input;
    public Texture[] BGImages;
    void Awake()
    {
        if(Input == null)
        {
            Input = new MyInputMap();
        }
        
        // Input.Player.Exit.started += ctx => ExitGame();
        // Input.Player.Refresh.started += ctx => RefreshScene();
    }
}
