using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Dissolve Animation")]
    public float dissolveSpeed = 2f;
    public AnimationCurve dissolveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public IEnumerator FadeInText(TextMeshProUGUI text)
    {
        Color originalColor = text.color;
        originalColor.a = 1f;

        float dissolveTime = 0f;
        while (dissolveTime < 1f)
        {
            dissolveTime += Time.deltaTime * dissolveSpeed;
            float curveValue = dissolveCurve.Evaluate(dissolveTime);

            Color color = originalColor;
            color.a = originalColor.a * curveValue;
            text.color = color;

            yield return null;
        }

        text.color = originalColor;
    }

    public IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        Color originalColor = text.color;

        float dissolveTime = 0f;
        while (dissolveTime < 1f)
        {
            dissolveTime += Time.deltaTime * dissolveSpeed;
            float curveValue = dissolveCurve.Evaluate(dissolveTime);

            Color color = originalColor;
            color.a = originalColor.a * (1f - curveValue);
            text.color = color;

            yield return null;
        }
        Color transparentColor = originalColor;
        transparentColor.a = 0f;
        text.color = transparentColor;
    }
    public IEnumerator FadeInImage(GameObject obj, float opacity)
    {
        yield return new WaitForSeconds(1f);

        obj.SetActive(true);

        Color originalColor = obj.GetComponent<Image>().color;

        float dissolveTime = 0f;
        while (dissolveTime < 1f)
        {
            dissolveTime += Time.deltaTime * dissolveSpeed;
            float curveValue = dissolveCurve.Evaluate(dissolveTime);

            Color color = originalColor;
            color.a = opacity * curveValue;
            obj.GetComponent<Image>().color = color;

            yield return null;
        }

        // obj.GetComponent<Image>().color = originalColor;
    }

    public IEnumerator FadeOutImage(GameObject obj, float opacity)
    {
        Color originalColor = obj.GetComponent<Image>().color;

        float dissolveTime = 0f;
        while (dissolveTime < 1f)
        {
            dissolveTime += Time.deltaTime * dissolveSpeed;
            float curveValue = dissolveCurve.Evaluate(dissolveTime);

            Color color = originalColor;
            color.a = opacity * (1f - curveValue);
            obj.GetComponent<Image>().color = color;

            yield return null;
        }
        Color transparentColor = originalColor;
        transparentColor.a = 0f;
        obj.GetComponent<Image>().color = transparentColor;

        obj.SetActive(false);
    }

    public IEnumerator DissolveEffect(TextMeshProUGUI roundText, string newText)
    {
        yield return StartCoroutine(FadeOutText(roundText));
        roundText.text = newText;
        yield return StartCoroutine(FadeInText(roundText));
    }

}