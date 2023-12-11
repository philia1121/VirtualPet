using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CatFood : MonoBehaviour
{
    public RawImage RawImg;
    public Texture[] FoodVoluem;
    float duration = 60f;
    int currentVolume = 2;
    bool eating = false;
    public bool durationCheck = true;
    public bool willBeEaten = false;

    public void Start()
    {
        Invoke("SelfDestroy", duration);
    }
    public void EatCatFood()
    {
        durationCheck = false;
        eating = true;
        currentVolume --;
        if(currentVolume >= 0)
        {
            RawImg.texture = FoodVoluem[currentVolume];
            if(currentVolume == 0)
            {
                Debug.Log("invoke");
                Invoke("SelfDestroy", 1.5f);
            }
        }
    }

    public void SelfDestroy()
    {
        if((durationCheck & !eating) | (!durationCheck & currentVolume <= 0))
        {
            Destroy(this.gameObject);
        }
        
    }
}
