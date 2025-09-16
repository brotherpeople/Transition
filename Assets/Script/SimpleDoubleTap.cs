using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SimpleDoubleTap : MonoBehaviour, IPointerClickHandler
{
    public float doubleTapTimeLimit = 0.1f;
    public TextMeshProUGUI indicatorText;
    public UnityEvent OnDoubleTapComplete;
    private int tapCount = 0;
    private bool isAnimating = false;
    private float lastTapTime;


    void Start()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAnimating) return;
        float currentTime = Time.time;
        float actualTapTime = currentTime - lastTapTime;

        // first tap
        if (tapCount == 0 || actualTapTime > doubleTapTimeLimit)
        {
            tapCount = 1;
            lastTapTime = currentTime;
            indicatorText.text = "Tap faster!";
        }
        // second tap
        else if (tapCount == 1 || actualTapTime <= doubleTapTimeLimit)
        {
            tapCount = 0;
            OnDoubleTapComplete?.Invoke();
            indicatorText.text = "You made it!";
            Debug.Log("Double Tap Completed!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
