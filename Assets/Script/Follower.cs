using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Follower : MonoBehaviour
{
    public GameObject Effect;
    public float speed = 1;
    private void Start()
    {
        // Effect.SetActive(false);    
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.X))
        {
            // Effect.SetActive(true);
            Effect.transform.position = UtilsClass.GetMouseWorldPosition();
        } 
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Effect.SetActive(!Effect.activeSelf);
        }
        if(Input.GetKey(KeyCode.W))
        {
            Effect.transform.position += Vector3.up* speed* Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            Effect.transform.position += Vector3.down* speed* Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A))
        {
            Effect.transform.position += Vector3.left* speed* Time.deltaTime;
            Effect.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if(Input.GetKey(KeyCode.D))
        {
            Effect.transform.position += Vector3.right* speed* Time.deltaTime;
            Effect.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        // if(Input.GetMouseButtonUp(0))
        // {
        //     Effect.SetActive(false);
        // }
    }
}
