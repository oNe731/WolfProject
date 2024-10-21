using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Sprite[] m_sprite;

    private List<InvenSlot> m_slots = new List<InvenSlot>();
    private int m_slotCount;

    public Sprite[] Sprite => m_sprite;

    private void Awake()
    {
        // �κ��丮 ���� �Ҵ�
        m_slotCount = transform.childCount;
        for (int i = 0; i < m_slotCount; i++)
        {
            GameObject slot = transform.GetChild(i).gameObject;
            if(slot != null)
            {
                InvenSlot script = slot.GetComponent<InvenSlot>();
                if(script != null)
                {
                    script.Inventory = this;
                    m_slots.Add(script);
                }
            }
        }
    }

    public void Add_Item(ItemData itemData)
    {
        // �ߺ� ������ �˻�
        bool sameItem = false;
        for (int i = 0; i < m_slotCount; i++)
        {
            if (m_slots[i].EMPTY == false)
            {
                if (m_slots[i].Item.itemType == itemData.itemType)
                {
                    m_slots[i].Add_Item(itemData, false);
                    sameItem = true;
                    break;
                }
            }
        }
        if (sameItem == true)
            return;

        // ������ �߰�
        for (int i = 0; i < m_slotCount; i++)
        {
            if (m_slots[i].EMPTY == true)
            {
                m_slots[i].Add_Item(itemData, true);
                break;
            }
        }
    }

    public void Sort_Inventory()
    {
        List<InvenSlot> sortedSlots = new List<InvenSlot>();

        foreach (InvenSlot slot in m_slots) { if (!slot.EMPTY) { sortedSlots.Add(slot); } }
        for (int i = sortedSlots.Count; i < m_slotCount; i++) { sortedSlots.Add(null); }

        // ���ĵ� ���� ����Ʈ�� �ٽ� ����
        for (int i = 0; i < m_slotCount; i++)
        {
            if (sortedSlots[i] != null && sortedSlots[i].EMPTY == false) { m_slots[i].Add_Item(sortedSlots[i].Item, true); }
            else { m_slots[i].Reset_Slot(); }
        }
    }
}