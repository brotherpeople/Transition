using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public TextMeshProUGUI roundText;
    public GameManager gameManager;
    public Image background;
    private int roundNum;
    [Header("Dissolve Animation")]
    public float dissolveSpeed = 2f;
    public AnimationCurve dissolveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Level Progress")]
    private bool dotClicked = false;
    private bool dotMoved = false;
    private bool screenSwiped = false;
    private bool hasCompleted = false;
    [SerializeField]
    private string[] toDoText = {
        "1. Click The Dot",
        "2. Move The Dot",
        "3. Swipe The Screen"
    };

    void Start()
    {
        UpdateRoundText();
    }

    void Update()
    {
        CheckLevelComplete();
    }

    private void CheckLevelComplete()
    {
        if (hasCompleted) return;

        bool levelComplete = false;

        switch (roundNum)
        {
            case 1:
                levelComplete = dotClicked;
                break;
            case 2:
                levelComplete = dotMoved;
                break;
            case 3:
                levelComplete = screenSwiped;
                break;
        }

        if (levelComplete)
        {
            hasCompleted = true;
            Debug.Log($"Round {roundNum} Completed");
            StartCoroutine(CompleteRoundSequence());


        }
    }

    IEnumerator CompleteRoundSequence()
    {
        yield return StartCoroutine(FadeOutText());
        yield return new WaitForSeconds(1f);
        gameManager.NextRound();
        ResetProgress();
        UpdateRoundText();
    }

    void ResetProgress()
    {
        dotClicked = false;
        dotMoved = false;
        screenSwiped = false;
        hasCompleted = false;
    }

    IEnumerator FadeOutText()
    {
        Color originalColor = roundText.color;
        // Color afterColor = afterText.color;

        float dissolveTime = 0f;
        while (dissolveTime < 1f)
        {
            dissolveTime += Time.deltaTime * dissolveSpeed;
            float curveValue = dissolveCurve.Evaluate(dissolveTime);

            Color color = originalColor;
            // Color aColor = afterColor;
            color.a = originalColor.a * (1f - curveValue);
            // aColor.a = 1f - curveValue;
            roundText.color = color;
            // afterText.color = aColor;

            yield return null;
        }
        Color transparentColor = originalColor;
        transparentColor.a = 0f;
        roundText.color = transparentColor;
    }
    IEnumerator FadeInText()
    {
        Color originalColor = roundText.color;
        originalColor.a = 1f;
        // afterText.text = newText;

        float dissolveTime = 0f;
        while (dissolveTime < 1f)
        {
            dissolveTime += Time.deltaTime * dissolveSpeed;
            float curveValue = dissolveCurve.Evaluate(dissolveTime);

            Color color = originalColor;
            // Color aColor = afterColor;
            color.a = originalColor.a * curveValue;
            // aColor.a = curveValue;
            roundText.color = color;
            // afterText.color = aColor;

            yield return null;
        }

        roundText.color = originalColor;
    }
    IEnumerator DissolveEffect(string newText)
    {
        yield return StartCoroutine(FadeOutText());
        roundText.text = newText;
        yield return StartCoroutine(FadeInText());
    }
    private void UpdateRoundText()
    {
        roundNum = gameManager.currentRound;
        Debug.Log($"Current Round: {roundNum}");

        if (roundNum <= toDoText.Length && roundNum > 0)
        {
            if (roundNum == 1)
                {
                    StartCoroutine(DissolveEffect(toDoText[roundNum - 1]));
                }
                else
                {
                    roundText.text = toDoText[roundNum - 1];
                    StartCoroutine(FadeInText());
                }
        }
    }

    public void OnDotClicked()
    {
        dotClicked = true;
        Debug.Log("Dot Clicked!");
        StartCoroutine(WaitSecCoroutine());
    }

    private IEnumerator WaitSecCoroutine()
    {
        yield return new WaitForSeconds(1f);
        background.color = Color.white;
    }

}
