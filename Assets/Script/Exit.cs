using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Exit : MonoBehaviour
{
    MyInputMap Input;
    
    void Awake()
    {
        Input = new MyInputMap();
        Input.Player.Exit.started += ctx => ExitGame();
        Input.Player.Refresh.started += ctx => RefreshScene();
    }

    void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication .isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
