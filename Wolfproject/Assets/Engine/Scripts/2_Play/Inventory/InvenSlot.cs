using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour, IPointerDownHandler
{
    private bool m_empty = true;

    private GameObject m_uIItem = null;
    private ItemData m_item = null;
    private Inventory inventory = null;

    public bool EMPTY => m_empty;
    public ItemData Item => m_item;
    public Inventory Inventory { set => inventory = value; }

    public void Add_Item(ItemData noteItem, bool reset)
    {
        if (m_item == null || reset)
            m_item = noteItem;
        else
        {
            // 개수 증가
            if (m_item.itemType == noteItem.itemType)
                m_item.count += noteItem.count;
        }

        // 이미지 교체
        transform.GetChild(0).transform.gameObject.SetActive(true);
        transform.GetChild(0).transform.GetComponent<Image>().sprite = inventory.Sprite[(int)noteItem.itemType];

        // 텍스트 변경
        if (m_item.count <= 1)
            transform.GetChild(1).gameObject.SetActive(false);
        else
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).GetComponent<TMP_Text>().text = m_item.count.ToString();
        }

        m_empty = false;
    }

    public void Use_Item()
    {
        m_item.count -= 1;

        transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = m_item.count.ToString();
        if (m_item.count == 1)
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (m_item.count <= 0)
        {
            Reset_Slot();
            inventory.Sort_Inventory();
        }
    }

    public void Reset_Slot()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        m_empty = true;
        m_item = null;

        if (m_uIItem != null) { Destroy(m_uIItem); }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_empty == true)
            return;

        switch (m_item.itemType) // 아이템 스포너 제작
        {
            case ItemData.TYPE.IT_HP:
                // 체력을 25% 회복.
                // 쿨다운: 3초
                break;

            case ItemData.TYPE.IT_BUFF:
                // 10초 동안 이동 및 공격 속도 20% 증가.
                // 쿨다운: 20초
                break;

            case ItemData.TYPE.IT_SHIELD:
                // 10초 동안 방어막이 체력보다 우선 적용.
                // 쿨다운: 15초
                break;
        }

        Use_Item();
    }
}
