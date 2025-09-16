using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public StageManager stageManager;
    public UIManager uiManager;
    public SimplePress simplePress;

    [Header("Level 1")]
    public GameObject clickHere;
    [Header("Level 2")]
    public GameObject dragHere;
    public GameObject dragThis;
    public SimpleDrag simpleDrag;
    [Header("Level 3")]
    public GameObject swipeThis;
    public GameObject swipeHere;
    [Header("Level 4")]
    public GameObject dotPrefab;
    public GameObject collectThis;
    public GameObject level4Container;
    public GameObject squareTransition;
    [Header("Level 5")]
    public GameObject pressThis;
    public float pressDuration = 5f;
    [Header("Level 6")]
    public GameObject doubleTapThis;
    [Header("Level 7")]
    public GameObject zoomButton1;
    public GameObject zoomButton2;
    public TextMeshProUGUI zoomText;

    void Start()
    {
        // level 2
        dragHere.SetActive(false);
        dragThis.SetActive(false);

        // level 3
        swipeThis.SetActive(false);
        swipeHere.SetActive(false);

        // level 4
        collectThis.SetActive(false);
        squareTransition.SetActive(false);

        // level 5
        pressThis.SetActive(false);

        // level 6
        doubleTapThis.SetActive(false);

        // level 7
        zoomButton1.SetActive(false);
        zoomButton2.SetActive(false);
        zoomText.gameObject.SetActive(false);
    }

    // level 1: Dot Clicked -> level 2: Dot Moved
    public void OnDotClicked()
    {
        stageManager.SetDotClicked(true);
        clickHere.GetComponent<Button>().enabled = false;
        StartCoroutine(OnDotClickedSeq());

        Debug.Log("Level 1: Dot Clicked!");
    }
    IEnumerator OnDotClickedSeq()
    {
        yield return new WaitForSeconds(1.5f);

        // level 2: Dot Moved setting
        yield return StartCoroutine(uiManager.FadeInImage(dragHere, 0.3f));
        yield return StartCoroutine(uiManager.FadeInImage(dragThis, 1f));

        SimpleDrag drag = dragThis.GetComponent<SimpleDrag>();
        drag.dragType = SimpleDrag.DragType.DragToTarget;
        drag.dragHere = dragHere;
        drag.threshold = 50f;
        drag.OnResult.AddListener((success) =>
        {
            if (success) OnDotMoved();
        });
        Debug.Log("Drag enabled");
    }

    // level 2: Dot Moved -> level 3: Dot Swiped
    public void OnDotMoved()
    {

        stageManager.SetDotMoved(true);

        Animator animator = dragThis.GetComponent<Animator>();
        animator.SetTrigger("isMoved");
        StartCoroutine(OnDotMovedSeq());

        Debug.Log("Level 2: Move Clear!");
    }

    IEnumerator OnDotMovedSeq()
    {
        yield return new WaitForSeconds(1.5f);

        // destroy unnecessary objects
        Destroy(dragHere);
        Destroy(clickHere);
        Image dragThisPos = dragThis.GetComponent<Image>();
        dragThisPos.raycastTarget = false;

        // level 3: Dot Swiped setting
        yield return StartCoroutine(uiManager.FadeInImage(swipeThis, 1f));

        SimpleDrag swipe = swipeThis.GetComponent<SimpleDrag>();
        swipe.dragType = SimpleDrag.DragType.Swipe;
        swipe.threshold = 150f;
        swipe.OnResult.AddListener((success) =>
        {
            if (success) onSwipe();
            else Debug.Log("Swipe failed");
        });

    }

    // level 3: Dot Swiped -> level 4: Dot Collect
    public void onSwipe()
    {
        stageManager.SetSwiped(true);

        StartCoroutine(OnDotSwipedSeq());
        Debug.Log("Level 3: Swipe Clear!");
    }

    IEnumerator OnDotSwipedSeq()
    {
        yield return new WaitForSeconds(1.5f);

        // destroy unnecessary objects
        Image swipeThisPos = swipeThis.GetComponent<Image>();
        swipeThisPos.raycastTarget = false;

        // level 4: Dot Collect setting
        Destroy(dragThis);

        for (int i = 0; i < 20; i++)
        {
            float randX = Random.Range(-500, 500);
            float randY = Random.Range(-900f, 600f);

            GameObject newDot = Instantiate(dotPrefab);
            newDot.transform.SetParent(level4Container.transform, false);
            RectTransform dotRect = newDot.GetComponent<RectTransform>();
            dotRect.anchoredPosition = new Vector2(randX, randY);
            newDot.SetActive(false);
            newDot.tag = "DotPrefab";
            StartCoroutine(uiManager.FadeInImage(newDot, 1f));
        }

        yield return new WaitForSeconds(1f);
        collectThis.SetActive(true);

        SimpleDrag drag = collectThis.GetComponent<SimpleDrag>();
        drag.dragType = SimpleDrag.DragType.Collect;
        drag.OnResult.AddListener((success) =>
        {
            if (success)
            {
                OnCollectComplete();
            }
            else Debug.Log("Swipe failed");
        });

    }

    private void OnCollectComplete()
    {
        stageManager.SetCollected(true);

        squareTransition.SetActive(true);
        Animator animator = squareTransition.GetComponent<Animator>();
        animator.SetTrigger("isCollected");
        StartCoroutine(OnCollectedSeq());

        Debug.Log("Level 4: Collect Clear!");
    }

    // level 4: Dot Collect -> level 5: Dot Pressed
    IEnumerator OnCollectedSeq()
    {
        yield return new WaitForSeconds(1.5f);

        // destroy level 3 objects
        Destroy(swipeThis);

        // level 5: Dot Pressed setting
        yield return StartCoroutine(uiManager.FadeInImage(pressThis, 1f));
        SimplePress longPress = pressThis.GetComponent<SimplePress>();
        if (longPress != null)
        {
            longPress.OnPressComplete.AddListener(() =>
            {
                OnPressComplete();
            });
        }
    }

    // level 5: Dot Pressed -> level 6: Dot Double Tap
    private void OnPressComplete()
    {
        stageManager.SetLongPressed(true);

        Animator animator = pressThis.GetComponent<Animator>();
        animator.SetTrigger("isLongPressed");
        StartCoroutine(OnLongPressedSeq());

        Debug.Log("Level 5: Long Press Clear!");
    }

    IEnumerator OnLongPressedSeq()
    {
        yield return new WaitForSeconds(1.5f);

        // destroy level 4 objects
        Destroy(collectThis);
        Destroy(squareTransition);

        // level 6: Dot Double Tap setting
        yield return StartCoroutine(uiManager.FadeInImage(doubleTapThis, 1f));
        SimpleDoubleTap doubleTap = doubleTapThis.GetComponent<SimpleDoubleTap>();
        if (doubleTap != null)
        {
            doubleTap.OnDoubleTapComplete.AddListener(() =>
            {
                OnDoubleTapComplete();
            });
        }

    }

    // level 6: Dot Double Tap -> level 7: Zoom Text
    private void OnDoubleTapComplete()
    {
        stageManager.SetDoubleTapped(true);

        Animator animator = pressThis.GetComponent<Animator>();
        animator.SetTrigger("isDoubleTapped");
        StartCoroutine(OnDoubleTappedSeq());
    }

    IEnumerator OnDoubleTappedSeq()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(pressThis);
        yield return StartCoroutine(uiManager.FadeOutImage(doubleTapThis, 1f));

        yield return StartCoroutine(uiManager.FadeInText(zoomText));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(uiManager.FadeInImage(zoomButton1, 1f));
        yield return StartCoroutine(uiManager.FadeInImage(zoomButton2, 1f));

        SimpleDrag zoomThis = zoomButton1.GetComponent<SimpleDrag>();
        if (zoomThis != null)
        {
            zoomThis.OnResult.AddListener((success) =>
            {
                if (success)
                {
                    OnZoomComplete();
                }
            });
        }
    }

    // level 7: Zoom Text -> level 8
    private void OnZoomComplete()
    {
        stageManager.SetTextZoomed(true);

        squareTransition.SetActive(true);
        Animator animator = squareTransition.GetComponent<Animator>();
        animator.SetTrigger("isZoomCompleted");
        StartCoroutine(OnZoomCompleteSeq());

        Debug.Log("Level 7: Zoom Complete!");
    }

    IEnumerator OnZoomCompleteSeq()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
