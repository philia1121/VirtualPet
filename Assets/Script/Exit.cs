using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Exit : MonoBehaviour
{
    // PlayerInput Input;
    
    // void Awake()
    // {
    //     Input = new PlayerInput();
    //     Input.Shortcut.ESC.started += ctx => Exit();
    // }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
            Debug.Log("Escape key was pressed");
        } 
    }

    void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication .isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // void OnEnable()
    // {
    //     Input.Shortcut.Enable();
    // }
    // void OnDisable()
    // {
    //     Input.Shortcut.Disable();
    // }

}
