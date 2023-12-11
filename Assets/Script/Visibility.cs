using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visibility : MonoBehaviour
{
    public RawImage image;
    public SpriteRenderer sprite;
    public Color visibleColor, invisibleColor;
    public void SetVisibleVaiColr(bool visible)
    {
        if(visible)
        {
            if(image)
            {
                image.color = visibleColor;
            }
            if(sprite)
            {
                sprite.color = visibleColor;
            }
            
        }
        else
        {
            if(image)
            {
                image.color = invisibleColor;
            }
            if(sprite)
            {
                sprite.color = invisibleColor;
            }
        }
        
    }
}
