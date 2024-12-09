using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Monster.TYPE[] m_monsterType;
    [SerializeField] private int[] m_monsterCount;

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
                        monster.spawner = this;
                    }
                }
            }
        }
    }

    public Vector2 Get_RandomPosition()
    {
        // 박스 콜라이더의 바운드 영역
        Bounds bounds = m_boxCollider.bounds;

        // X, Y의 랜덤 위치 계산
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }
}
