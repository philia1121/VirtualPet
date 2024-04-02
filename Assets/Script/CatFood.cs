using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CatFood : MonoBehaviour
{
    public Transform CatParent;
    public RawImage RawImg;
    public Texture[] FoodVoluem;
    float duration = 60f;
    int currentVolume = 2;
    bool eating = false;
    public bool durationCheck = true;
    public bool willBeEaten = false;
    public int eatingCount = 0;
    public Vector3 FoodOffset = new Vector3(-0.6f, -1.94f, 0);
    CatStateMachine catInRange;

    public void EatCatFood() //old Function 
    {
        durationCheck = false;
        eating = true;
        currentVolume --;
        if(currentVolume >= 0)
        {
            RawImg.texture = FoodVoluem[currentVolume];
            if(currentVolume == 0 && eatingCount == 0)
            {
                Finished();
            }
        }
    }
    public void EatCatFood(int amount)
    {
        durationCheck = false;
        eating = true;
        currentVolume = amount;
        if(amount == 0)
        {
           eatingCount --; 
        }
        if(currentVolume > 0)
        {
            RawImg.texture = FoodVoluem[currentVolume];
        }
        else if(currentVolume == 0)
        {
            if(eatingCount == 0)
            {
                RawImg.texture = FoodVoluem[0];
                Finished();
            }
        }
    }
    void Finished()
    {
        if(TryGetComponent<DragAndDrop>(out DragAndDrop temp))
        {
            Destroy(temp);
        }
        Invoke("SelfDestroy", 1.5f);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "CatRange")
        {
            catInRange = collider.transform.parent.GetComponent<CatStateMachine>();
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "CatRange")
        {
            catInRange = null;
        }
    }
    public void OnMouseDown()
    {
        if(!eating)
        {
            CancelInvoke("SelfDestroy");
        }
        if(!willBeEaten)
        {
            var allCats = CatParent.GetComponentsInChildren<CatStateMachine>();
            foreach(var cat in allCats)
            {
                cat.GetFed(this);
            }
        }
    }
    public void OnMouseUp()
    {
        if(!eating)
        {
            Invoke("SelfDestroy", duration);
        }
        if(catInRange != null && catInRange.WaitForFood)
        {
            this.transform.position = catInRange.CatTransform.position + FoodOffset;
            catInRange.GetFed(this);
            if(TryGetComponent<DragAndDrop>(out DragAndDrop temp))
            {
                Destroy(temp);
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
