using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class LoadCatAssets : MonoBehaviour
{
	public string MainFolderPath = @"D:\NTUT\XRLab\Virtual Pet\AICat";
    public string CatFolderName = "purple_striped_cat"; 
	public string[] ClipFolderName = new string[]{ "eat", "idle", "jump", "knead", "observe", "postEat", "postKnead", "postScratch", "postWaitForEat",
        "preEat", "preKnead", "preScratch", "preWaitForEat", "scratch", "sit", "sitDown", "sitUp", "sleep", "stretch", "waitForEat", "walk"};
    string[] foundResults;
    public float finishedCount = 0;
    public TextMeshProUGUI loadingProgressText;
    public RawImage previewImage;
    string pathPreFix = @"file://"; 
    [HideInInspector]public List<Texture> Eat, Idle, Jump, Knead, Observe, PostEat, PostKnead, PostScratch, PostWaitForEat,
        PreEat, PreKnead, PreScratch, PreWaitForEat, Scratch, Sit, SitDown, SitUp, Sleep, Stretch, WaitForEat, Walk;
    public Dictionary<string, List<Texture>> myDic = new Dictionary<string, List<Texture>>();
    public Dictionary<string, Texture[]> AnimationArrayDic = new Dictionary<string, Texture[]>();
    public UnityEvent DoneLoading = new UnityEvent();
    
    void Start()
    {
        InitializeAnimationDic();
        foreach(var clip in ClipFolderName)
        {
            string path = Path.Combine(Path.Combine(MainFolderPath, CatFolderName), clip);
            foundResults = System.IO.Directory.GetFiles(path);

            StartCoroutine(LoadImages(foundResults, clip));
        }
    }
    public void ResetAssets()
    {
        foreach(var pair in myDic)
            pair.Value.Clear();
        AnimationArrayDic.Clear();
        finishedCount = 0;
        this.enabled = false;
    }

    private IEnumerator LoadImages(string[] files, string clipName)
    {
        foreach(var item in files)
        {
            var url = pathPreFix + item;
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError | request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                // Debug.Log("Successfully download at " + Time.time);

                var myImg = DownloadHandlerTexture.GetContent(request);
                myDic[clipName].Add(myImg);
            }
        }

        finishedCount++; //done downloading this clip
        if(loadingProgressText != null)
        {
            loadingProgressText.text = finishedCount.ToString() + "/" + ClipFolderName.Length.ToString();
        }
        if(finishedCount >= ClipFolderName.Length) // done downloading every clip
        {
            if(previewImage != null)
            {
                previewImage.texture = Idle[0];
            }
            Debug.Log("All Animation Clip Assets Done Downloading at : " + Time.time);
            foreach(var item in myDic)
            {
                AnimationArrayDic.Add(item.Key, item.Value.ToArray());
            }
            DoneLoading?.Invoke();
        }
	}

    public float LoadingProgress()
    {
        return finishedCount/ClipFolderName.Length;
    }
    void InitializeAnimationDic()
    {
        myDic.Clear();
        myDic.Add("eat", Eat);
        myDic.Add("idle", Idle);
        myDic.Add("jump", Jump);
        myDic.Add("knead", Knead);
        myDic.Add("observe", Observe);
        myDic.Add("postEat", PostEat);
        myDic.Add("postKnead", PostKnead);
        myDic.Add("postScratch", PostScratch);
        myDic.Add("postWaitForEat", PostWaitForEat);
        myDic.Add("preEat", PreEat);
        myDic.Add("preKnead", PreKnead);
        myDic.Add("preScratch", PreScratch);
        myDic.Add("preWaitForEat", PreWaitForEat);
        myDic.Add("scratch", Scratch);
        myDic.Add("sit", Sit);
        myDic.Add("sitDown", SitDown);
        myDic.Add("sitUp", SitUp);
        myDic.Add("sleep", Sleep);
        myDic.Add("stretch", Stretch);
        myDic.Add("waitForEat", WaitForEat);
        myDic.Add("walk", Walk);
    }

    public void ChangePath_MainFolder(string value)
    {
        if(value  == null)
            return;
        
        MainFolderPath = value;
    }
    public void ChangePath_Cat(string value)
    {
        if(value  == null)
            return;
        
        CatFolderName = value;
    }
}
