using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net;
using TMPro;

public class ShowIPv4 : MonoBehaviour
{
    public string preString = "Local IPv4: ";
    public TextMeshProUGUI textMesh;
    void Start()
    {
        textMesh.text = preString + IPv4Manager.GetLocalIPv4();
    }
    public void ChangeDisplayTextColor()
    {
        textMesh.color = Color.red;
    }
}
