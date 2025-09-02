using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private LevelManager levelManager;
    private Canvas canvas;
    private Vector2 originalPos;
    private GameObject dragHere;
    public void Initialize(LevelManager manager, GameObject dragTarget)
    {
        levelManager = manager;
        dragHere = dragTarget;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag begins");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag finished");

        float dist = Vector2.Distance(rectTransform.anchoredPosition, dragHere.GetComponent<RectTransform>().anchoredPosition);

        if (dist < 10f)
        {
            rectTransform.anchoredPosition = dragHere.GetComponent<RectTransform>().anchoredPosition;
            levelManager.OnDotMoved();
            Destroy(this);
        }
        else
        {
            rectTransform.anchoredPosition = originalPos;
        }
    }
}
