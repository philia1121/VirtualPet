using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MultiCatInteractionButton : MonoBehaviour
{
    [Header("Cat")]
    public Transform CatParent;
    public CatStateMachine[] allCats;

    [Header("Interactive Props")]
    public Transform FoodParent;
    public Transform BallParent;
    public GameObject FoodPrefab;
    public GameObject BallPrefab;
    GameObject foodAwaitInCenter;
    GameObject ballOnStage;
    public Transform[] Boundary = new Transform[2];
    public Vector3 FoodOffest = new Vector3(-0.6f, -1.94f, 0);
    bool showOption = false;
    GameObject allOptions;
    void showToysOption()
    {
        showOption = !showOption;
        allOptions.SetActive(showOption);
    }

    public void Feed()
    {
        var newFood = Instantiate(FoodPrefab, FoodParent);
        Vector3 mousePos = Input.mousePosition;
        newFood.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x, Camera.main.ScreenToWorldPoint(mousePos).y, 0);
        newFood.GetComponent<DragAndDrop>().dragging = true;
        newFood.GetComponent<CatFood>().CatParent = CatParent;
    }
    public void Ball()
    {

    }
    public void Fish()
    {

    }
}
