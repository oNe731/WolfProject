using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum TYPE { TYPE_END, TYPE_FOREST, TYPE_MARSH }
    [SerializeField] private TYPE m_itemMapType;
    [SerializeField] private Sprite[] m_spriteForestImg;
    [SerializeField] private Sprite[] m_spriteMarshImg;
    private SpriteRenderer m_sr;

    private void Start()
    {
        if(m_itemMapType != TYPE.TYPE_END)
            Set_MapType(m_itemMapType);
    }

    public void Set_MapType(TYPE itemMapType)
    {
        if(m_sr == null)
            m_sr = GetComponent<SpriteRenderer>();

        m_itemMapType = itemMapType;
        if (m_itemMapType == TYPE.TYPE_FOREST)
            m_sr.sprite = m_spriteForestImg[0];
        else if (m_itemMapType == TYPE.TYPE_MARSH)
            m_sr.sprite = m_spriteMarshImg[0];
    }

    protected abstract void Triger_Event();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") == true)
        {
            if (GameManager.Ins.Play.Player == null)
                return;

            Triger_Event();

            //Destroy(gameObject);
            if (m_itemMapType == TYPE.TYPE_FOREST)
                m_sr.sprite = m_spriteForestImg[1];
            else if (m_itemMapType == TYPE.TYPE_MARSH)
                m_sr.sprite = m_spriteMarshImg[1];
            else
                m_sr.sprite = m_spriteForestImg[1];
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
