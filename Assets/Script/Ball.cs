using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    float duration = 60f;
    bool durationCheck = true;
    float speed = 15f;
    float rotateSpeed = 800f;
    public void Start()
    {
        Invoke("SelfDestroy", duration);
    }

    public void StartRolling()
    {
        StartCoroutine(Rolling());
    }

    public IEnumerator Rolling()
    {
        var target = new Vector3(Random.Range(9.5f, 15.8f), Random.Range(5.5f, 11.5f), 0);
        target = (Random.Range(-1,1f) > 0) ? target : new Vector3(target.x* -1, target.y, target.z);
        target = (Random.Range(-1,1f) > 0) ? target : new Vector3(target.x, target.y* -1, target.z);
        while(true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed* Time.deltaTime);
            this.transform.Rotate(new Vector3(0, 0, rotateSpeed)* Time.deltaTime);
            yield return null;   
        }
    }

    public void ObserveBall()
    {
        durationCheck = false;
    }
    public void SelfDestroy(bool force = false)
    {
        if(force)
        {
            StartCoroutine(RandomSimpleTimer(3, 5)); // cat ignore the ball, remove the ball from stage within random time duration
            return;
        }

        if(durationCheck)
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }     
    }
    public IEnumerator RandomSimpleTimer(float min, float max)
    {
        float randomTime = Random.Range(min, max);
        yield return new WaitForSeconds(randomTime);

        Destroy(this.gameObject);
        StopAllCoroutines();
        
    }
}
