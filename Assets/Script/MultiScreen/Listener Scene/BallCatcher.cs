using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCatcher : MonoBehaviour
{
    bool dragging = false;
    float posZ = 0;
    void Start()
    {
        
    }
    void OnMouseDown()
    {
        dragging = true;
        this.enabled = true;
    }
 
    void OnMouseUp()
    {
        dragging = false;
        this.enabled = false;
    }

    void Update()
    {
        if (dragging)
        {
            Vector3 mousePos = Input.mousePosition;
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x, Camera.main.ScreenToWorldPoint(mousePos).y, posZ);
        }
    }
}
