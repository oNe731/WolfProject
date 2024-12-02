using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutDialog : MonoBehaviour
{
    [SerializeField] private Image m_backgroundImg;
    [SerializeField] private TMP_Text m_dialogTxt;

    private List<CutData> m_dialogs;
    private Coroutine m_dialogTextCoroutine = null;

    private bool m_isUpdate = false;
    private bool m_isTyping = false;
    private bool m_cancelTyping = false;
    private int m_dialogIndex = 0;
    private float m_typeSpeed = 0.05f;

    private void Update()
    {
        if (GameManager.Ins.IsGame == false)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Input.mousePosition;
            float screenHeight = Screen.height;
            if (touchPosition.y < screenHeight * 0.75f)
                Update_Dialog();
        }
    }

    private void Update_Dialog()
    {
        if (m_isTyping)
            m_cancelTyping = true;
        else if (!m_isTyping)
        {
            if (m_isUpdate == false)
                return;

            // 다이얼로그 진행
            if (m_dialogIndex < m_dialogs.Count)
            {
                switch (m_dialogs[m_dialogIndex].m_dataType)
                {
                    case CutData.TYPE.TYPE_DIALOG:
                        Update_None();
                        break;

                    case CutData.TYPE.TYPE_FADE:
                        Update_Fade();
                        break;
                }
            }
            else // 다이얼로그 종료
            {
                m_isUpdate = false;
                switch (GameManager.Ins.PreScene)
                {
                    case (int)GameManager.SCENE.SCENE_MAIN: // 인트로 컷씬
                        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(GameManager.SCENE.SCENE_NAME), 0.2f, false);
                        break;

                    case (int)GameManager.SCENE.SCENE_PLAY: // 선, 혼돈 점수에 따른 엔딩 컷씬
                        break;

                    default:
                        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(GameManager.SCENE.SCENE_NAME), 0.2f, false);
                        break;
                }
            }
        }
    }

    #region Update
    private void Update_Basic(int index)
    {
        // 리소스 업데이트
        //* 배경 이미지 업데이트
        if (!string.IsNullOrEmpty(m_dialogs[index].cutSpr))
            m_backgroundImg.sprite = GameManager.Ins.Cut.CutSpr[m_dialogs[index].cutSpr];
    }

    private void Update_None()
    {
        Update_Basic(m_dialogIndex);

        if (m_dialogTextCoroutine != null)
            StopCoroutine(m_dialogTextCoroutine);
        m_dialogTextCoroutine = StartCoroutine(Type_Text(m_dialogIndex));

        m_dialogIndex++;
    }

    private void Update_Fade()
    {
        switch (m_dialogs[m_dialogIndex].fade)
        {
            case CutData.FADE.FADE_IN:
                Update_Basic(m_dialogIndex + 1);
                m_dialogTxt.gameObject.transform.parent.gameObject.SetActive(false);

                m_isUpdate = false;
                GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Next_FadeIn());
                break;
        }
    }

    private void Next_FadeIn()
    {
        m_isUpdate = true;

        m_dialogIndex++;
        Update_Dialog();
    }
    #endregion

    #region Common
    public void Start_Dialog(string path)
    {
        m_dialogs = GameManager.Ins.Load_JsonData<CutData>(path);

        m_isUpdate = true;
        m_isTyping = false;
        m_cancelTyping = false;
        m_dialogIndex = 0;

        Update_Dialog();
    }

    IEnumerator Type_Text(int dialogIndex)
    {
        m_isTyping = true;
        m_cancelTyping = false;

        m_dialogTxt.gameObject.transform.parent.gameObject.SetActive(true);
        m_dialogTxt.text = "";
        foreach (char letter in m_dialogs[dialogIndex].dialogText.ToCharArray())
        {
            if (m_cancelTyping)
            {
                m_dialogTxt.text = m_dialogs[dialogIndex].dialogText;
                break;
            }

            m_dialogTxt.text += letter;
            yield return new WaitForSeconds(m_typeSpeed);
        }

        m_isTyping = false;
        yield break;
    }
    #endregion
}