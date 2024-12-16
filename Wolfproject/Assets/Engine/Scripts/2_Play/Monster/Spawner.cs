using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum ITEMTYPE { TYPE_END, TYPE_MONSTER, TYPE_GROUP }

    [SerializeField] private Item.TYPE m_mapType;
    [SerializeField] private Monster.TYPE[] m_monsterType;
    [SerializeField] private int[] m_monsterCount;

    [SerializeField] private ITEMTYPE m_itemCreateType;
    [SerializeField] private ItemData.TYPE[] m_itemTypes;
    [SerializeField] private float[] m_itemProbability;

    private BoxCollider2D m_boxCollider;

    private void Start()
    {
        m_boxCollider = GetComponent<BoxCollider2D>();

        Spawn_Monsters();
    }

    private void Spawn_Monsters()
    {
        for(int i = 0; i < m_monsterType.Length; ++i)
        {
            GameObject gameObject = null;
            switch(m_monsterType[i])
            {
                case Monster.TYPE.TYPE_SLIME:
                    gameObject = GameManager.Ins.Load<GameObject>("4_Prefab/2_Monster/Slime");
                    break;
                case Monster.TYPE.TYPE_MUSHROOM:
                    gameObject = GameManager.Ins.Load<GameObject>("4_Prefab/2_Monster/Mushroom");
                    break;
                case Monster.TYPE.TYPE_SLIMEBOSS:
                    gameObject = GameManager.Ins.Load<GameObject>("4_Prefab/2_Monster/SlimeBoss");
                    break;
                case Monster.TYPE.TYPE_SLIMERED:
                    gameObject = GameManager.Ins.Load<GameObject>("4_Prefab/2_Monster/SlimeRed");
                    break;
            }

            for (int j = 0; j < m_monsterCount[i]; j++)
            {
                Vector2 spawnPosition = Get_RandomPosition();
                GameObject obj = Instantiate(gameObject, spawnPosition, Quaternion.identity, transform);
                if (obj != null)
                {
                    Monster monster = obj.GetComponent<Monster>();
                    if (monster != null)
                    {
                        monster.MapType = m_mapType;
                        monster.spawner = this;

                        // ���� �� ����� �� �ִ� ������ ���� 0 - 1��
                        if(m_itemCreateType == ITEMTYPE.TYPE_MONSTER)
                        {
                            // Ư�� �� óġ �� + Ȯ��
                            int randomChance = Random.Range(0, 100);
                            if (randomChance <= m_itemProbability[i])
                                monster.ItemType = m_itemTypes[i];
                        }
                        else if (m_itemCreateType == ITEMTYPE.TYPE_GROUP)
                        {
                            // Ư�� ���� �� óġ �� + Ȯ��
                            int RandomIndex = Random.Range(0, m_itemTypes.Length);

                            int randomChance = Random.Range(0, 100);
                            if (randomChance <= m_itemProbability[RandomIndex])
                                monster.ItemType = m_itemTypes[RandomIndex];
                        }
                    }
                }
            }
        }
    }

    public Vector2 Get_RandomPosition()
    {
        // �ڽ� �ݶ��̴��� �ٿ�� ����
        Bounds bounds = m_boxCollider.bounds;

        // X, Y�� ���� ��ġ ���
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }
}
