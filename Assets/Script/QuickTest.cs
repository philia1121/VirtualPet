using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net;
using UnityEngine.UI;

public class QuickTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("LocalIPv4: " + GetLocalIPv4());
    }
    public void Log()
    {
        Debug.Log("do something");
    }

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First( f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
    }
}
