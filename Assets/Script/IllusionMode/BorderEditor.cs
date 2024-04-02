using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderEditor : MonoBehaviour
{
    public MyInputMap Input;
    public bool state = true;
    public GameObject[] marks;
    List<Collider2D> markColliders = new List<Collider2D>();
    List<SpriteRenderer> markSprites = new List<SpriteRenderer>();
    void Awake()
    {
        if(Input == null)
        {
            Input = new MyInputMap();
        }
        
        Input.Player.BorderEdit.started += ctx => ShowBorderMark();
    }
    void Start()
    {
        foreach(var mark in marks)
        {
            markColliders.Add(mark.GetComponent<Collider2D>());
            markSprites.Add(mark.GetComponent<SpriteRenderer>());
        }
        foreach(var collider in markColliders)
        {
            collider.enabled = state;
        }
        foreach(var sprite in markSprites)
        {
            sprite.enabled = state;
        }
    }
    public void ShowBorderMark()
    {
        state = !state;
        foreach(var collider in markColliders)
        {
            collider.enabled = state;
        }
        foreach(var sprite in markSprites)
        {
            sprite.enabled = state;
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
