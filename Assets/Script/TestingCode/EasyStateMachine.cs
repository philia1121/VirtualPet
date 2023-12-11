using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EasyStateMachine : MonoBehaviour
{
    public SpriteRenderer catRenderer;
    public Sprite[] currentAnimation;
    public Sprite[] eat, idle, jump, knead, observe, postEat, postKnead, postScratch, postWaitForEat,
        preEat, preKnead, preScratch, preWaitForEat, scratch, sit, sitDown, sitUp, sleep, stretch, waitForEat, walk;

    public Dictionary<int, Sprite[]> animationDic = new Dictionary<int, Sprite[]>();
    public int frame = 0;
    public float interval = 0.02f;
    public float min_randomTime;
    public float max_randomTime;
    IEnumerator playing;
    void Start()
    {
        animationDic[0] = eat;
        animationDic[1] = idle;
        animationDic[2] = jump;
        animationDic[3] = knead;
        animationDic[4] = observe;
        animationDic[5] = scratch;
        animationDic[6] = sit;
        animationDic[7] = sleep;
        animationDic[8] = stretch;
        animationDic[9] = waitForEat;
        animationDic[10] = walk;

        currentAnimation = idle;
        playing = ManualAnimationPlayer();
        StartCoroutine(playing);
        StartCoroutine(RandomChange());
    }

    IEnumerator ManualAnimationPlayer()
    {
        while(true)
        {
            if(frame < currentAnimation.Length)
            {
                catRenderer.sprite = currentAnimation[frame];
                frame++;
            }
            else
            {
                frame = 0;
                catRenderer.sprite = currentAnimation[frame];
                frame ++;
            }
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator RandomChange()
    {
        while(true)
        {
            float randomTime = Random.Range(min_randomTime, max_randomTime);
            Debug.Log("Next Random will happend after " + randomTime + "sec.");

            int randomAnimation = Random.Range(0, 11);
            Debug.Log("Switch to Animation Clip: " + animationDic[randomAnimation]);
            StopCoroutine(playing);
            currentAnimation = animationDic[randomAnimation];
            frame = 1;
            playing = ManualAnimationPlayer();
            StartCoroutine(playing);
            
            yield return new WaitForSeconds(randomTime);
        }
    }

}
