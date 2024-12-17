using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public enum TYPE { TYPE_NONE, TYPE_DIALOG, TYPE_END }
    [SerializeField] private TYPE m_type;
    private bool m_isEvent = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (m_isEvent == true)
            return;

        switch(m_type)
        {
            case TYPE.TYPE_DIALOG: // �����ӵ� ���� �׿��� �� ���� ���̾�α� ���
                if (transform.childCount == 0)
                {
                    m_isEvent = true;
                    GameManager.Ins.Play.Dialog.Start_Dialog();
                }
                break;
        }
    }
}
