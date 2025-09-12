using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SimplePress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float pressDuration = 5f;
    public float shakeSpeed = 20f;
    public TextMeshProUGUI progressText;
    public UnityEvent OnPressComplete;
    private bool isPressed = false;
    private float pressTime = 0f;
    private RectTransform rectTransform;
    private Vector2 originalPos;
    private Vector2 originalScale;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
        originalScale = rectTransform.sizeDelta;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPressed)
        {
            isPressed = true;
            pressTime = 0f;
            StartCoroutine(LongPressRoutine());
            StartCoroutine(ShakeEffect());
        }
    }

    IEnumerator ShakeEffect()
    {
        float elapsedTime = 0f;

        while (isPressed)
        {
            float offsetX = UnityEngine.Random.Range(-5f, 5f);
            float offsetY = UnityEngine.Random.Range(-5f, 5f);
            Vector2 shakePos = originalPos + new Vector2(offsetX, offsetY);

            elapsedTime += Time.deltaTime;
            Vector2 enlargedSize = new Vector2(
                originalScale.x + (elapsedTime * 30f),
                originalScale.y + (elapsedTime * 30f)
            );
            rectTransform.anchoredPosition = shakePos;
            rectTransform.sizeDelta = enlargedSize;

            yield return new WaitForSeconds(1f / shakeSpeed);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPressed)
        {
            progressText.text = $"0 / {pressDuration}";
            isPressed = false;
            pressTime = 0f;
            StopCoroutine(LongPressRoutine());
            StopCoroutine(ShakeEffect());
            rectTransform.sizeDelta = originalScale;
        }
    }

    IEnumerator LongPressRoutine()
    {
        while (isPressed && pressTime < pressDuration)
        {
            pressTime += Time.deltaTime;
            progressText.text = $"{pressTime:F2} / {pressDuration}";
            yield return null;
        }

        if (isPressed && pressTime >= pressDuration)
        {
            isPressed = false;
            StopCoroutine(ShakeEffect());
            OnPressComplete?.Invoke();
            Debug.Log("Long Press completed!");
        }
    }

}