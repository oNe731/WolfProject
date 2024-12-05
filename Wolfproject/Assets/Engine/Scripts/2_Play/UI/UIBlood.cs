using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlood : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private float m_blinkDuration = 1f; // 알파값이 0에서 1로, 다시 1에서 0으로 변하는 데 걸리는 시간

    private bool m_isBlick = false;
    private Coroutine m_coroutine = null;

    public void Start_Blood()
    {
        if (m_isBlick == true)
            return;

        m_isBlick = true;
        gameObject.SetActive(true);
        m_coroutine = StartCoroutine(Start_Blink());
    }

    public void Stop_Blood()
    {
        if (m_isBlick == false || GameManager.Ins.IsGame == false)
            return;

        if (m_coroutine != null)
            StopCoroutine(m_coroutine);

        m_isBlick = false;
        gameObject.SetActive(false);
    }

    private IEnumerator Start_Blink()
    {
        while (m_isBlick)
        {
            yield return StartCoroutine(Fade_Image(0f, 1f, m_blinkDuration / 2)); // 알파값이 0에서 1로 증가
            yield return StartCoroutine(Fade_Image(1f, 0f, m_blinkDuration / 2)); // 알파값이 1에서 0으로 감소
        }
        yield break;
    }

    private IEnumerator Fade_Image(float start, float end, float duration)
    {
        float elapsed = 0f;

        Color StartColor = m_image.color;
        while (elapsed < duration)
        {
            if(GameManager.Ins.IsGame == true)
            {
                m_image.color = new Color(StartColor.r, StartColor.g, StartColor.b, Mathf.Lerp(start, end, elapsed / duration));
                elapsed += Time.deltaTime;
            }

            yield return null;
        }

        m_image.color = new Color(StartColor.r, StartColor.g, StartColor.b, end);
    }
}

