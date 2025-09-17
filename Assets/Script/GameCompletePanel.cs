using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCompletePanel : MonoBehaviour
{
    public GameObject completePanel;
    public TextMeshProUGUI completeText;
    public Button restartButton;
    public Button exitButton;
    public UIManager uIManager;

    // Start is called before the first frame update
    void Start()
    {

        completePanel.SetActive(true);
        Image panelImage = completePanel.GetComponent<Image>();
        panelImage.raycastTarget = false;

        Color transparent = panelImage.color;
        transparent.a = 0f;
        panelImage.color = transparent;

        completeText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void ShowCompletePanel()
    {
        StartCoroutine(ShowCompletePanelSeq());
    }

    IEnumerator ShowCompletePanelSeq()
    {
        completeText.text = "Congratulations!\nNow you know how to use smartphone";

        yield return StartCoroutine(uIManager.FadeInImage(completePanel, 1f));
        yield return new WaitForSeconds(0.5f);
        completeText.gameObject.SetActive(true);
        yield return StartCoroutine(uIManager.FadeInText(completeText));
        yield return new WaitForSeconds(1f);
        restartButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
