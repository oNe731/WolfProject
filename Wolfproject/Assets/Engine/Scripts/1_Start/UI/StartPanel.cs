using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private string[] m_titleName;
    [SerializeField] private Sprite[] m_methodSprite;
    [SerializeField] private Sprite[] m_buttonSprite;
    [SerializeField] private GameObject m_methodPanel;

    private Image[] m_indexButton;
    private TMP_Text m_titleText;

    private int m_currentIndex = 0;
    private Image m_methodImage;

    private void Start()
    {
        m_methodImage = m_methodPanel.transform.GetChild(0).GetComponent<Image>();
        m_titleText = m_methodPanel.transform.GetChild(1).GetComponent<TMP_Text>();

        m_indexButton = new Image[2];
        m_indexButton[0] = m_methodPanel.transform.GetChild(2).GetComponent<Image>();
        m_indexButton[1] = m_methodPanel.transform.GetChild(3).GetComponent<Image>();
    }

    public void Button_Start()
    {
        GameManager.Ins.Change_Scene(GameManager.SCENE.SCENE_CUT);
    }

    public void Button_Method(bool start)
    {
        if (start == true)
            m_currentIndex = 0;

        m_methodImage.sprite = m_methodSprite[m_currentIndex];
        m_methodPanel.SetActive(true);

        if (m_methodSprite.Length == 1) // 1장일 때
        {
            Set_ButtonsActive(false, false);
        }
        else if(m_methodSprite.Length == 2) // 2장일 때
        {
            if(m_currentIndex == 0)
                Set_ButtonsActive(false, true);
            else if(m_currentIndex == 1)
                Set_ButtonsActive(true, false);
        }
        else if (m_methodSprite.Length >= 3) // 3장 이상일 때
        {
            if(m_currentIndex == 0) // 첫장일 때
                Set_ButtonsActive(false, true);
            else if(m_currentIndex == m_methodSprite.Length - 1) // 마지막장일 때
                Set_ButtonsActive(true, false);
            else
                Set_ButtonsActive(true, true);
        }

        m_titleText.text = m_titleName[m_currentIndex];
    }

    private void Set_ButtonsActive(bool leftActive, bool rightActive)
    {
        if (m_indexButton.Length < 2)
            return;

        if (leftActive == true) // 왼쪽 버튼
            m_indexButton[0].sprite = m_buttonSprite[1];
        else
            m_indexButton[0].sprite = m_buttonSprite[0];

        if (rightActive == true) // 오른쪽 버튼
            m_indexButton[1].sprite = m_buttonSprite[1];
        else
            m_indexButton[1].sprite = m_buttonSprite[0];
    }

    public void Button_NextMethod()
    {
        if (m_currentIndex < m_methodSprite.Length - 1)
        {
            m_currentIndex++;
            Button_Method(false);
        }
    }

    public void Button_PreviousMethod()
    {
        if (m_currentIndex > 0)
        {
            m_currentIndex--;
            Button_Method(false);
        }
    }

    public void Button_CloseMethod()
    {
        m_methodPanel.SetActive(false);
    }

    public void Button_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
