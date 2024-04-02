using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooCatcher : MonoBehaviour
{
    public string PooId;
    [HideInInspector]public PropsManager propsManager;
    void OnMouseUp()
    {
        SelfDestroy();
    }
    public void SelfDestroy()
    {
        propsManager.DestroyPoo(this);
    }
}
