using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayDialog : MonoBehaviour
{
    [SerializeField] private Image m_profileImg;
    [SerializeField] private TMP_Text m_dialogTxt;

    private List<PlayData> m_dialogs;
    private Coroutine m_dialogTextCoroutine = null;

    private bool m_isUpdate = false;
    private bool m_isTyping = false;
    private bool m_cancelTyping = false;
    private int m_dialogIndex = 0;
    private float m_typeSpeed = 0.05f;

    private AudioSource m_audioSource;

    public void Initialize()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (m_isUpdate == false)
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
                Update_None();
            }
            else // 다이얼로그 종료
            {
                m_isUpdate = false;
                m_audioSource.Stop();

                GameManager.Ins.Set_Pause(false);
                gameObject.SetActive(false);
            }
        }
    }

    #region Update
    private void Update_None()
    {
        if (!string.IsNullOrEmpty(m_dialogs[m_dialogIndex].profileSpr))
            m_profileImg.sprite = GameManager.Ins.Play.ProfileSpr[m_dialogs[m_dialogIndex].profileSpr];

        if (m_dialogTextCoroutine != null)
            StopCoroutine(m_dialogTextCoroutine);
        m_dialogTextCoroutine = StartCoroutine(Type_Text(m_dialogIndex));

        m_dialogIndex++;
    }
    #endregion

    #region Common
    public void Start_Dialog(string path)
    {
        m_dialogs = GameManager.Ins.Load_JsonData<PlayData>(path);

        m_isUpdate = true;
        m_isTyping = false;
        m_cancelTyping = false;
        m_dialogIndex = 0;

        GameManager.Ins.Set_Pause(true);
        gameObject.SetActive(true);
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

    public void Play_AudioSource(string audioClip, bool loop, float speed, float volume)
    {
        m_audioSource.Stop();

        m_audioSource.clip = GameManager.Ins.Play.Effect[audioClip];
        m_audioSource.loop = loop;
        m_audioSource.pitch = speed; // 기본1f
        m_audioSource.volume = volume;
        m_audioSource.Play();
    }
}
