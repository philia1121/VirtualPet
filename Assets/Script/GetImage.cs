using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetImage : MonoBehaviour
{
	public Sprite[] sprites;
	public SpriteRenderer mySpriteComp;
	public RawImage rawImage;
	public string path = @"D:\NTUT\XRLab";
	
	string[] files;
	string pathPreFix; 
	
	// Use this for initialization
	void Start () {
		//Change this to change pictures folder
		
		pathPreFix = @"file://";

		files = System.IO.Directory.GetFiles(path);
		
		// StartCoroutine(LoadImages());
	}
	
	private IEnumerator LoadImages()
    {
		var url = pathPreFix + files[0];
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
		Debug.Log("Send Request at" + Time.time);
        yield return request.SendWebRequest();

		if(request.result == UnityWebRequest.Result.ConnectionError | request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.LogError(request.error);
		}
		else
		{
			Debug.Log("Successfully download at " + Time.time);
			var myImg = DownloadHandlerTexture.GetContent(request);
			
			// for Raw Image
			rawImage.texture = myImg;

			// for SpriteRenderer
			// var temp = Sprite.Create(myImg, new Rect(0, 0, myImg.width, myImg.height), new Vector2(.5f, .5f));
			// mySpriteComp.sprite = temp;
		}
	}
}
