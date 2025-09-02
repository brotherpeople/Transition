using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public StageManager stageManager;
    public UIManager uiManager;

    [Header("Level 1")]
    public GameObject clickHere;
    [Header("Level 2")]
    public GameObject dragHere;
    public GameObject dragCircle;
    private RectTransform dragRectTransform;

    void Start()
    {
        dragHere.SetActive(false);
        dragCircle.SetActive(false);

    }

    // level 1 -> level 2
    public void OnDotClicked()
    {
        stageManager.SetDotClicked(true);
        Debug.Log("Dot Clicked!");
        clickHere.GetComponent<Button>().enabled = false;
        StartCoroutine(OnDotClickedSeq());
    }
    IEnumerator OnDotClickedSeq()
    {
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(uiManager.FadeInImage(dragHere, 0.3f));
        yield return StartCoroutine(uiManager.FadeInImage(dragCircle, 1f));


        SimpleDrag dragScript = dragCircle.GetComponent<SimpleDrag>() ?? dragCircle.AddComponent<SimpleDrag>();

        dragScript.Initialize(this, dragHere);
        Debug.Log("Drag enabled");
    }

    // level 2 -> level 3
    public void OnDotMoved()
    {
        stageManager.SetDotMoved(true);
        Debug.Log("Dot moved!");
    }

}