using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualGIF : MonoBehaviour
{
    public SpriteRenderer GIF_Player;
    public Sprite[] run, sleep, currentGIF;
    public int frame = 0;
    public float interval;
    void Start()
    {
        currentGIF = run;
        GIF_Player = GetComponent<SpriteRenderer>();
        StartCoroutine(ManualGIFPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            currentGIF = run;
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            currentGIF = sleep;
        }
    }
    IEnumerator ManualGIFPlayer()
    {
        while(true)
        {
            if(frame < currentGIF.Length)
            {
                GIF_Player.sprite = currentGIF[frame];
                frame++;
            }
            else
            {
                frame = 0;
                GIF_Player.sprite = currentGIF[frame];
                frame ++;
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
