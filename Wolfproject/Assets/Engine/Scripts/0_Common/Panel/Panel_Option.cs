using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Option : MonoBehaviour
{
    private bool m_isOpen = false;
    private bool m_preIsGame = false;

    private Slider m_soundBgm;
    private Slider m_soundEffect;

    private void Start()
    {
        m_soundBgm = transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<Slider>();
        m_soundBgm.onValueChanged.AddListener(Change_BGMSliderValue);
        m_soundBgm.value = GameManager.Ins.Sound.BgmSound;

        m_soundEffect = transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<Slider>();
        m_soundEffect.onValueChanged.AddListener(Change_EffectSliderValue);
        m_soundEffect.value = GameManager.Ins.Sound.EffectSound;

        GameManager.Ins.Sound.Update_AllAudioSources();
    }

    public void Button_Active()
    {
        Active_Panel(!m_isOpen);
    }

    private void Active_Panel(bool active)
    {
        if (active == true)
        {
            m_isOpen = true;

            m_preIsGame = GameManager.Ins.IsGame;
            GameManager.Ins.Set_Pause(true);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            m_isOpen = false;

            GameManager.Ins.Set_Pause(!m_preIsGame);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void Change_BGMSliderValue(float value)
    {
        GameManager.Ins.Sound.BgmSound = value;
        GameManager.Ins.Sound.Update_AllAudioSources();
    }

    public void Change_EffectSliderValue(float value)
    {
        GameManager.Ins.Sound.EffectSound = value;
        GameManager.Ins.Sound.Update_AllAudioSources();
    }
}
