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
    public UIManager uiManager;
    public Image background;
    private int roundNum;

    [Header("Level Progress")]
    private bool dotClicked = false; // level 1
    private bool dotMoved = false; // level 2
    private bool screenSwiped = false; // level 3
    private bool hasCompleted = false; // level 3
    private bool dotCollected = false; // level 4
    private bool dotPressed = false; // level 5
    private bool dotDoubleTapped = false; // level 6
    [SerializeField]
    private string[] toDoText = {
        "1. Click The Dot",
        "2. Move The Dot",
        "3. Swipe The Screen",
        "4. Collect The Dots"
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
            case 4:
                levelComplete = dotCollected;
                break;
            case 5:
                levelComplete = dotPressed;
                break;
            case 6:
                levelComplete = dotDoubleTapped;
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
        yield return StartCoroutine(uiManager.FadeOutText(roundText));
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
        dotCollected = false;
        hasCompleted = false;
        dotPressed = false;
        dotDoubleTapped = false;
    }


    private void UpdateRoundText()
    {
        roundNum = gameManager.currentRound;
        Debug.Log($"Current Round: {roundNum}");

        if (roundNum <= toDoText.Length && roundNum > 0)
        {
            if (roundNum == 1)
            {
                StartCoroutine(uiManager.DissolveEffect(roundText, toDoText[roundNum - 1]));
            }
            else
            {
                roundText.text = toDoText[roundNum - 1];
                StartCoroutine(uiManager.FadeInText(roundText));
            }
        }
    }

    internal int GetCurrentRound()
    {
        return roundNum;
    }

    internal void SetDotClicked(bool v)
    {
        dotClicked = v;
        Debug.Log("SetDotClicked");
    }

    internal void SetDotMoved(bool v)
    {
        dotMoved = v;
    }

    internal void SetSwiped(bool v)
    {
        screenSwiped = v;
    }
    internal void SetCollected(bool v)
    {
        dotCollected = v;
    }
    internal void SetLongPressed(bool v)
    {
        dotPressed = v;
    }

    internal void SetDoubleTapped(bool v)
    {
        dotDoubleTapped = v;
    }

}
