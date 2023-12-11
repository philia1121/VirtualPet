using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratingAnimation : MonoBehaviour
{
    public AnimationClip clip;
    public Sprite[] newSprites;
    private void Start()
    {
        AnimationCurve curve;
        Keyframe[] keys;
        keys = new Keyframe[newSprites.Length];
        for(int i = 0; i < newSprites.Length; i++)
        {
            var sprite = newSprites[i];
            // keys[i] = new Keyframe(0f, sprite);
        }
        curve= new AnimationCurve();
        clip.SetCurve( "", typeof(Sprite), "sprite", curve);
    }
}
