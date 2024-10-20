using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public void Start_Projectile(Player.ATTRIBUTETYPE type, Vector3 direct)
    {
        // 타입에 따른 처리
        if (type == Player.ATTRIBUTETYPE.AT_FIRE)
        {
            // 화염 속성: 적에게 지속 피해를 입힘.
        }
        else if (type == Player.ATTRIBUTETYPE.AT_THUNDER)
        {
            // 번개 속성: 적의 이동 속도를 느리게 만듦.
        }


    }

    void Update()
    {
        
    }
}
