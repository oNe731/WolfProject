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

    private float m_maxValueString = 0f;

    public Image FillImage => m_fillImage;

    public void Initialize(float MaxValueString = 0f)
    {
        m_slider = GetComponent<Slider>();

        m_text = transform.GetComponentInChildren<TMP_Text>();
        m_fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();

        m_maxValueString = MaxValueString;
    }

    public void Set_Slider(float value)
    {
        if (value < 0)
            value = 0f;
        m_slider.value = value;

        int currentValue = (int)m_slider.value;

        int maxValue;
        if (m_maxValueString == 0)
            maxValue = (int)m_slider.maxValue;
        else
            maxValue = (int)m_maxValueString;

        m_text.text = currentValue.ToString() + "/ " + maxValue.ToString();
    }
}
