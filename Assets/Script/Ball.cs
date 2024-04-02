using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    bool durationCheck = true;
    float speed = 15f;
    float rotateSpeed = 800f;
    public Vector3[] boundary = new Vector3[2];
    // public void Start()
    // {
    //     Invoke("SelfDestroy", duration);
    // }

    public void StartRolling()
    {
        StartCoroutine(Rolling());
    }

    public IEnumerator Rolling()
    {
        // now will only move inside the boundary
        var target = new Vector3(Random.Range(boundary[0].x, boundary[1].x), Random.Range(boundary[0].y, boundary[1].y), Random.Range(boundary[0].z, boundary[1].z));
        // target = (Random.Range(-1,1f) > 0) ? target : new Vector3(target.x* -1, target.y, target.z);
        // target = (Random.Range(-1,1f) > 0) ? target : new Vector3(target.x, target.y* -1, target.z);
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
