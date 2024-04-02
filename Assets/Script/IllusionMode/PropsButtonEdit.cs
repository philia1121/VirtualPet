using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsButtonEdit : MonoBehaviour
{
    public MyInputMap Input;
    public bool state = true;
    public GameObject buttonPanel;
    void Awake()
    {
        if(Input == null)
        {
            Input = new MyInputMap();
        }
        
        Input.Player.PropsEdit.started += ctx => EditPropsButton();
    }
    void Start()
    {
        buttonPanel.SetActive(state);
    }
    public void EditPropsButton()
    {
        state = !state;
        buttonPanel.SetActive(state);
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
