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

    private bool m_isButton = false;
    private List<GameObject> m_choice_Button = new List<GameObject>();

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
            if (m_isUpdate == false || m_isButton == true)
                return;

            // 다이얼로그 진행
            if (m_dialogIndex < m_dialogs.Count)
            {
                switch(m_dialogs[m_dialogIndex].dataType)
                {
                    case PlayData.TYPE.TYPE_DIALOG:
                        Update_None();
                        break;

                    case PlayData.TYPE.TYPE_BUTTON:
                        Update_Button();
                        break;

                    case PlayData.TYPE.TYPE_RESCUE:
                        Update_Rescue();
                        break;

                    case PlayData.TYPE.TYPE_NEGLECT:
                        Update_Neglect();
                        break;

                    case PlayData.TYPE.TYPE_POTION:
                        Update_Potion();
                        break;
                }
   
            }
            else // 다이얼로그 종료
            {
                Close_Dialog();
            }
        }
    }

    #region Update
    private void Update_None()
    {
        if (!string.IsNullOrEmpty(m_dialogs[m_dialogIndex].profileSpr))
        {
            m_profileImg.gameObject.SetActive(true);
            m_profileImg.sprite = GameManager.Ins.Play.ProfileSpr[m_dialogs[m_dialogIndex].profileSpr];
        }

        if (m_dialogTextCoroutine != null)
            StopCoroutine(m_dialogTextCoroutine);
        m_dialogTextCoroutine = StartCoroutine(Type_Text(m_dialogIndex));

        m_dialogIndex++;
    }

    #region Button
    private void Update_Button()
    {
        m_isButton = true;
        m_profileImg.gameObject.SetActive(false);
        m_dialogTxt.text = "";

        // 선택지 버튼 생성
        float startHeight = 145f;
        for (int i = 0; i < m_dialogs[m_dialogIndex].choiceText.Count; ++i)
        {
            int ButtonIndex = i; // 버튼 고유 인덱스

            GameObject Clone = GameManager.Ins.LoadCreate("4_Prefab/6_UI/Button");
            if (Clone != null)
            {
                Clone.transform.SetParent(gameObject.transform);
                Clone.transform.localPosition = new Vector3(0f, (startHeight + (i * -140)), 0f);
                Clone.transform.localScale = new Vector3(1f, 1f, 1f);

                TMP_Text TextCom = Clone.GetComponentInChildren<TMP_Text>();
                if (TextCom)
                {
                    TextCom.text = m_dialogs[m_dialogIndex].choiceText[i];

                    Button button = Clone.GetComponent<Button>();
                    if (button != null) // 이벤트 핸들러 추가
                        button.onClick.AddListener(() => Click_Button(ButtonIndex));

                    m_choice_Button.Add(Clone);
                }
            }
        }
    }

    private void Click_Button(int index)
    {
        if(m_dialogs[m_dialogIndex].choicePoint[index] == PlayData.POINT.TYPE_ZEN)
            GameManager.Ins.ZenScore += m_dialogs[m_dialogIndex].choiceValue[index];
        else if (m_dialogs[m_dialogIndex].choicePoint[index] == PlayData.POINT.TYPE_CHAOS)
            GameManager.Ins.ChaosScore += m_dialogs[m_dialogIndex].choiceValue[index];

        Start_Dialog(m_dialogs[m_dialogIndex].choiceDialog[index]);
    }
    #endregion

    private void Update_Rescue()
    {
        GameObject traveler = GameObject.FindGameObjectWithTag("Traveler");

        Transform firstChild = traveler.transform.GetChild(0);
        for (int i = 0; i < firstChild.childCount; i++)
            firstChild.GetChild(i).transform.GetComponent<Monster>().StateMachine.Change_State((int)Slime.STATE.ST_CHASE);

        Close_Dialog();
    }

    private void Update_Neglect()
    {
        // 페이드 아웃
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Next_Neglect(), 0f, false);
    }

    private void Next_Neglect()
    {
        // 슬라임 흔적으로 여행자 근처에 뿌려놓기
        GameObject obj = GameManager.Ins.LoadCreate("4_Prefab/5_Effect/SlimeBubble");
        if (obj != null)
            obj.transform.position = new Vector3(75.8f, -26.7f, 0f);

        // 여행자, 몬스터 삭제
        GameObject traveler = GameObject.FindGameObjectWithTag("Traveler");
        Destroy(traveler);

        // 페이드 인 후 다음 대사 출력
        Close_Dialog();
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Dialog());
    }

    private void Update_Potion()
    {
        GameObject traveler = GameObject.FindGameObjectWithTag("Traveler");

        // 여행자 위치에 포션3개 생성
        GameObject gameObject = GameManager.Ins.Load<GameObject>("4_Prefab/4_Item/Recovery");
        for (int i = 0; i < 3; ++i)
        {
            Vector2 spawnPosition = traveler.transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0f);
            GameObject Items = Instantiate(gameObject, spawnPosition, Quaternion.identity);
            Items.GetComponent<Item>().Set_MapType(Item.TYPE.TYPE_FOREST);
        }

        Destroy(traveler);
        Close_Dialog();
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

        m_isButton = false;
        for (int i = 0; i < m_choice_Button.Count; ++i)
            Destroy(m_choice_Button[i]);
        m_choice_Button.Clear();

        GameManager.Ins.Set_Pause(true);
        gameObject.SetActive(true);
        Update_Dialog();
    }

    public void Start_Dialog()
    {
        m_isUpdate = true;
        m_isTyping = false;
        m_cancelTyping = false;

        m_isButton = false;
        for (int i = 0; i < m_choice_Button.Count; ++i)
            Destroy(m_choice_Button[i]);
        m_choice_Button.Clear();

        GameManager.Ins.Set_Pause(true);
        gameObject.SetActive(true);

        m_dialogIndex++;
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

    private void Close_Dialog()
    {
        m_isUpdate = false;
        m_audioSource.Stop();

        GameManager.Ins.Set_Pause(false);
        gameObject.SetActive(false);
    }
}
