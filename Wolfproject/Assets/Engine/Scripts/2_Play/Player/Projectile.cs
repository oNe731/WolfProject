using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public void Start_Projectile(Player.ATTRIBUTETYPE type, Vector3 direct)
    {
        // Ÿ�Կ� ���� ó��
        if (type == Player.ATTRIBUTETYPE.AT_FIRE)
        {
            // ȭ�� �Ӽ�: ������ ���� ���ظ� ����.
        }
        else if (type == Player.ATTRIBUTETYPE.AT_THUNDER)
        {
            // ���� �Ӽ�: ���� �̵� �ӵ��� ������ ����.
        }


    }

    void Update()
    {
        
    }
}
