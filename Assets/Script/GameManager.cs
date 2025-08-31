using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int currentRound = 1;

    void Awake()
    {
        if (instance == null) { instance = this; }
    }

    public void NextRound()
    {
        currentRound++;
        PlayerPrefs.SetInt("CurrentRound", currentRound);
        PlayerPrefs.Save();
    }

    void ResetGame()
    {
        currentRound = 1;
        PlayerPrefs.SetInt("CurrentRound", currentRound);
        PlayerPrefs.Save();
    }
}
