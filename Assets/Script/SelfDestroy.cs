using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float lasting = 5f;
    void Start()
    {
        StartCoroutine(countdown());
    }
    IEnumerator countdown()
    {
        yield return new WaitForSeconds(lasting);
        Destroy(this.gameObject);
    }
}
