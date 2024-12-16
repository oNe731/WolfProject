using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Item.TYPE m_itemMapType;
    [SerializeField] private ItemData.TYPE[] m_itemType;
    [SerializeField] private int[] m_itemCount;

    private BoxCollider2D m_boxCollider;

    private void Start()
    {
        m_boxCollider = GetComponent<BoxCollider2D>();

        Spawn_Items();
    }

    private void Spawn_Items()
    {
        for(int i = 0; i < m_itemType.Length; ++i)
        {
            GameObject gameObject = null;
            switch(m_itemType[i])
            {
                case ItemData.TYPE.IT_HP:
                    gameObject = GameManager.Ins.Load<GameObject>("4_Prefab/4_Item/Recovery");
                    break;
                case ItemData.TYPE.IT_BUFF:
                    gameObject = GameManager.Ins.Load<GameObject>("4_Prefab/4_Item/Speed");
                    break;
                case ItemData.TYPE.IT_SHIELD:
                    gameObject = GameManager.Ins.Load<GameObject>("4_Prefab/4_Item/Defense");
                    break;
            }

            for (int j = 0; j < m_itemCount[i]; j++)
            {
                Vector2 spawnPosition = Get_RandomPosition();
                GameObject Items = Instantiate(gameObject, spawnPosition, Quaternion.identity, transform);

                if(m_itemMapType != Item.TYPE.TYPE_END)
                    Items.GetComponent<Item>().Set_MapType(m_itemMapType);
            }
        }
    }

    private Vector2 Get_RandomPosition()
    {
        // 박스 콜라이더의 바운드 영역
        Bounds bounds = m_boxCollider.bounds;

        // X, Y의 랜덤 위치 계산
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }
}
