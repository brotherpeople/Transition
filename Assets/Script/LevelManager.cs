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
    public GameObject dragThis;
    public SimpleDrag simpleDrag;
    [Header("Level 3")]
    public GameObject swipeThis;
    public GameObject swipeHere;
    [Header("Level 4")]
    public GameObject dotPrefab;
    public GameObject collectThis;
    public GameObject level4Container;


    void Start()
    {
        dragHere.SetActive(false);
        dragThis.SetActive(false);
        swipeThis.SetActive(false);
        swipeHere.SetActive(false);
        collectThis.SetActive(false);

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

    // level 3: Dot Swiped -> level 4: Dot Clicked
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

        // level 4: Dot Clicked setting
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
    }
}
