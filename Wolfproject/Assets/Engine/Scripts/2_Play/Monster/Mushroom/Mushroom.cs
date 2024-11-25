using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Monster
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize_Monster();

        m_hpMax = 5f;
        m_hp = m_hpMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     *2. ����

�⺻ ����
����: ȭ�� �Ӽ��� ����.
����: ���� �Ӽ��� ����.
���ط�: ���� �� �÷��̾�� 15�� ����.


FSM ����

Idle (��� ����)
�ൿ: ���ڸ��� ����� �̵��� �ݺ��ϸ� ��ȸ.
��ȯ ����: �÷��̾ ���� ���� ���� ������ **Stealth(�ẹ ����)**�� ��ȯ.

Stealth (�ẹ ����)
�ൿ: ������ 2�� ���� �ẹ�� ����ȭ�ǰ� �÷��̾ ���.
��ȯ ����: ���� ���� ���� �� Hit ���·� ��ȯ, ���� �� Chase ���·� ��ȯ.

Chase (�߰� ����)
�ൿ: ������ �÷��̾ õõ�� �߰��ϸ� ���� ��ȸ�� ����.
��ȯ ����: �ٰŸ� ���� �� �ٽ� Stealth ���·� ���ư�.

Hit (�ǰ� ����)
�ൿ: ü���� ���� ���Ϸ� �����ϸ� �ٸ� ������ �շ��� ��� ���߰� ü���� ȸ��.
��ȯ ����: ȸ�� �� Chase ���·� ����.
     */
}
