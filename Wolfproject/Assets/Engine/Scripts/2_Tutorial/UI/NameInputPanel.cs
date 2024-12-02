using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameInputPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_name;

    public void Button_InputName()
    {
        GameManager.Ins.Set_PlayerName(m_name.text);

        // 게임 시작 (UI 업데이트)
        //GameManager.Ins.

        Destroy(gameObject);
    }
}
