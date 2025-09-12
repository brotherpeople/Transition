using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SimpleDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Serializable]
    public enum DragType { DragToTarget, Swipe, Collect };
    public float threshold = 100f;
    public DragType dragType;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPos;
    private Vector2 dragTotal;
    public GameObject dragHere;
    public GameObject swipeHere;
    public GameObject collectThis;
    private int collectedCount;
    public LevelManager levelManager;
    [Header("Events")]
    public UnityEvent<bool> OnResult;
    private bool isAnimating = false;
    public float moveSpeed = 1f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isAnimating) return;

        dragTotal = Vector2.zero;
        Debug.Log("Drag begins");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isAnimating) return;

        Vector2 delta = eventData.delta / canvas.scaleFactor;
        dragTotal += delta;

        switch (dragType)
        {
            case DragType.DragToTarget:
                rectTransform.anchoredPosition += delta;
                break;
            case DragType.Swipe:
                rectTransform.anchoredPosition += delta;
                break;
            case DragType.Collect:
                Vector2 newPos = rectTransform.anchoredPosition + delta;
                newPos = ClampBoundary(newPos);
                rectTransform.anchoredPosition = newPos;
                break;
        }
    }

    private Vector2 ClampBoundary(Vector2 pos)
    {
        float clampedX = Mathf.Clamp(pos.x, -430, 530);
        float clampedY = Mathf.Clamp(pos.y, -1000, 700);
        return new Vector2(clampedX, clampedY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isAnimating) return;

        Debug.Log("Drag finished");

        bool success = false;

        switch (dragType)
        {
            case DragType.DragToTarget:
                success = HandleDragToTarget();
                break;
            case DragType.Swipe:
                HandleSwipe();
                break;
            case DragType.Collect:
                rectTransform.anchoredPosition = ClampBoundary(rectTransform.anchoredPosition);
                success = (collectedCount >= 20);
                // rectTransform.anchoredPosition = originalPos;
                break;
        }
        OnResult?.Invoke(success);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dragType == DragType.Collect && other.CompareTag("DotPrefab"))
        {
            other.gameObject.SetActive(false);
            collectedCount++;
        }
    }

    private bool HandleDragToTarget()
    {
        if (isAnimating) return false;

        Vector2 currentPos = rectTransform.anchoredPosition;
        Vector2 targetPos = dragHere.GetComponent<RectTransform>().anchoredPosition;
        float dist = Vector2.Distance(currentPos, targetPos);

        if (dist < threshold)
        {
            rectTransform.anchoredPosition = targetPos;
            // levelManager.OnDotMoved();
            return true;
        }
        else
        {
            rectTransform.anchoredPosition = originalPos;
            return false;
        }
    }

    private void HandleSwipe()
    {
        float swipeDist = dragTotal.magnitude;
        float swipeX = dragTotal.x;
        Vector2 targetPos = swipeHere.GetComponent<RectTransform>().anchoredPosition;
        Debug.Log($"Threshold: {threshold}");


        bool isLeftSwipe = swipeX < -threshold;
        bool isLongEnough = swipeDist > threshold;

        if (isLeftSwipe && isLongEnough)
        {
            StartCoroutine(SmoothMove(targetPos, true));
        }
        else
        {
            StartCoroutine(SmoothMove(originalPos, false));
        }
    }

    IEnumerator SmoothMove(Vector2 targetPos, bool success)
    {
        isAnimating = true;
        Vector2 startPos = rectTransform.anchoredPosition;
        Debug.Log($"Animation started");

        float moveTime = 0;
        while (moveTime < 1f)
        {
            moveTime += Time.deltaTime * moveSpeed;
            float curveValue = moveCurve.Evaluate(moveTime);
            Vector2 currentPosition = Vector2.Lerp(startPos, targetPos, curveValue);
            rectTransform.anchoredPosition = currentPosition;

            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
        isAnimating = false;
        Debug.Log($"Animation finished: Current position: {targetPos}");

        OnResult?.Invoke(success);
    }

}
