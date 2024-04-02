using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CatSelfPanel : MonoBehaviour
{
    public CatStateMachine CSM;
    public LoadCatAssets loader;
    public TextMeshProUGUI myText;
    public RawImage image;

    public void RemoveCat()
    {
        Destroy(CSM.gameObject);
        Destroy(this.gameObject);
    }
    public void LoadAssets()
    {
        loader.enabled = true;
    }
    public void ClearAssets()
    {
        loader.ResetAssets();
    }
    public void ChangePath_Main(string value)
    {
        loader.ChangePath_MainFolder(value);
    }
    public void ChangePath_Cat(string value)
    {
        loader.ChangePath_Cat(value);
    }

}
