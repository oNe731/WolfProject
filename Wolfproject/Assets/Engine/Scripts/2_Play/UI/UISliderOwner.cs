using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UISliderOwner : MonoBehaviour
{
    private Slider m_slider;
    private TMP_Text m_text;
    private Image m_fillImage;

    public Image FillImage => m_fillImage;

    public void Initialize()
    {
        m_slider = GetComponent<Slider>();

        m_text = transform.GetComponentInChildren<TMP_Text>();
        m_fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    public void Set_Slider(float value)
    {
        m_slider.value = value;

        int currentValue = (int)m_slider.value;
        int maxValue = (int)m_slider.maxValue;
        m_text.text = currentValue.ToString() + "/ " + maxValue.ToString();
    }
}
