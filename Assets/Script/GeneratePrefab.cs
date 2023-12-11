using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class GeneratePrefab : MonoBehaviour
{
    
    public GameObject prefab;
    public Transform point;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
        }
    }
}
