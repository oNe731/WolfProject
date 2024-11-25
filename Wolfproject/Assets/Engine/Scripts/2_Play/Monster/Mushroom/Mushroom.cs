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
     *2. 버섯

기본 정보
약점: 화염 속성에 약함.
저항: 번개 속성에 강함.
피해량: 접촉 시 플레이어에게 15의 피해.


FSM 구조

Idle (대기 상태)
행동: 제자리에 멈췄다 이동을 반복하며 배회.
전환 조건: 플레이어가 일정 범위 내에 들어오면 **Stealth(잠복 상태)**로 전환.

Stealth (잠복 상태)
행동: 버섯이 2초 동안 잠복해 투명화되고 플레이어를 기습.
전환 조건: 돌진 공격 성공 시 Hit 상태로 전환, 실패 시 Chase 상태로 전환.

Chase (추격 상태)
행동: 버섯이 플레이어를 천천히 추격하며 돌진 기회를 엿봄.
전환 조건: 근거리 접근 시 다시 Stealth 상태로 돌아감.

Hit (피격 상태)
행동: 체력이 절반 이하로 감소하면 다른 버섯과 합류해 잠시 멈추고 체력을 회복.
전환 조건: 회복 후 Chase 상태로 복귀.
     */
}
