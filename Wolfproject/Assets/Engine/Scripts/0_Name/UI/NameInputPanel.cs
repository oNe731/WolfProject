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
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(GameManager.SCENE.SCENE_TUTORIAL), 0.2f, false);
    }
}
