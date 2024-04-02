using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net;
public static class IPv4Manager
{
    public static string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First( f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
    }
}
