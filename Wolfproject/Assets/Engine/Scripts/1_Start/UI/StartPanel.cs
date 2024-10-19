using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private Sprite[] m_methodSprite;
    [SerializeField] private GameObject m_methodPanel;
    [SerializeField] private GameObject[] m_indexButton;

    private int m_currentIndex = 0;
    private Image m_methodImage;

    private void Start()
    {
        m_methodImage = m_methodPanel.transform.GetChild(0).GetComponent<Image>();
    }

    public void Button_Start()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Button_Method()
    {
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
    }

    private void Set_ButtonsActive(bool leftActive, bool rightActive)
    {
        if (m_indexButton.Length < 2)
            return;

        m_indexButton[0].SetActive(leftActive);  // 왼쪽 버튼
        m_indexButton[1].SetActive(rightActive); // 오른쪽 버튼
    }

    public void Button_NextMethod()
    {
        if (m_currentIndex < m_methodSprite.Length - 1)
        {
            m_currentIndex++;
            Button_Method();
        }
    }

    public void Button_PreviousMethod()
    {
        if (m_currentIndex > 0)
        {
            m_currentIndex--;
            Button_Method();
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
