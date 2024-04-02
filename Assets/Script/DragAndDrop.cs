using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public bool dragging = false;
    private float posZ;

    void Awake()
    {
        posZ = transform.position.z;
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
