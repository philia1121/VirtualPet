using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBeSeen : MonoBehaviour
{
    void OnBecameInvisible()
    {
        enabled = false;
    }

    // Enable this script when the GameObject moves into the camera's view
    void OnBecameVisible()
    {
        enabled = true;
    }

    void Update()
    {
        Debug.Log("Script is enabled");
    }
}
