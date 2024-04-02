using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatEditor : MonoBehaviour
{
    public MyInputMap Input;
    public bool assetPanel = true;
    public bool statePanel = false;
    public GameObject assetEditorPanel, stateEditorPanel;
    public GameObject catTemplate, selfTemplate;
    public Transform catParent, selfParent;
    [Header("CatStateMaschine Settings")]
    public Transform[] catBorderMarks = new Transform[2];
    public Transform[] objectBorderMarks = new Transform[2];
    public Transform[] objectParents = new Transform[2];
    void Awake()
    {
        if(Input == null)
        {
            Input = new MyInputMap();
        }
        
        Input.Player.AssetEdit.started += ctx => ShowAssetEditorPanel();
        Input.Player.StateEdit.started += ctx => ShowStateEditorPanel();
    }
    void Start()
    {
        assetEditorPanel.SetActive(assetPanel);
        stateEditorPanel.SetActive(statePanel);
    }
    void ShowAssetEditorPanel()
    {
        assetPanel = !assetPanel;
        assetEditorPanel.SetActive(assetPanel);
    }
    public void AddNewCat()
    {
        var selfPanel = Instantiate(selfTemplate, selfParent);
        var cat = Instantiate(catTemplate, catParent);
        
        var self = selfPanel.GetComponent<CatSelfPanel>();
        self.CSM = cat.GetComponent<CatStateMachine>();
        self.CSM.Boundary[0] = catBorderMarks[0]; 
        self.CSM.Boundary[1] = catBorderMarks[1];
        self.CSM.ObjectBoundary[0] = objectBorderMarks[0]; 
        self.CSM.ObjectBoundary[1] = objectBorderMarks[1];  
        self.CSM.ObjectParents[0] = objectParents[0];
        self.CSM.ObjectParents[1] = objectParents[1];
        self.loader = cat.GetComponent<LoadCatAssets>();
        self.loader.previewImage = self.image;
        self.loader.loadingProgressText = self.myText;
    }
    void ShowStateEditorPanel()
    {
        statePanel = !statePanel;
        stateEditorPanel.SetActive(statePanel);
    }
    public void ChangeMinDuration(string value)
    {
        var allCatSelfs = selfParent.GetComponentsInChildren<CatSelfPanel>();
        foreach(var self in allCatSelfs)
        {
            self.CSM.ShorterDuration[0] = float.Parse(value);
        }
    }
    public void ChangeMaxDuration(string value)
    {
        var allCatSelfs = selfParent.GetComponentsInChildren<CatSelfPanel>();
        foreach(var self in allCatSelfs)
        {
            self.CSM.ShorterDuration[1] = float.Parse(value);
        }
    }
    void OnEnable()
    {
        Input.Player.Enable();
    }
    void OnDisable()
    {
        Input.Player.Disable();
    }
}
