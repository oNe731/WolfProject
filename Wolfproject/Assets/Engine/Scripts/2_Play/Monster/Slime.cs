using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    // Start is called before the first frame update
    void Start()
    {
        Start_Monster();

        m_hpMax = 5f;
        m_hp = m_hpMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
 * 1. 슬라임

기본 정보
약점: 번개 속성에 약함.
저항: 화염 속성에 강함.
피해량: 접촉 시 플레이어에게 10의 피해.


FSM 구조
Idle (대기 상태)

행동: 무작위로 천천히 맵을 배회하며 플레이어를 탐색.
전환 조건: 플레이어가 일정 거리 내에 들어오면 **Chase(추격 상태)**로 전환.
Chase (추격 상태)

행동: 슬라임이 직선으로 느리게 플레이어를 추격하며, 장애물을 넘기 위해 주기적으로 점프함.
전환 조건: 일정 시간 후 플레이어와의 거리 좁힘 시 **Jump Attack(점프 공격 상태)**으로 전환.
Jump Attack (점프 공격 상태)

행동: 슬라임이 근접한 플레이어를 향해 점프 공격.
공격 성공 시 0.5초 스턴 부여.
전환 조건: 공격 후 다시 Chase 상태로 돌아감.
Hit (피격 상태)

행동: 체력이 절반 이하로 떨어지면 속도가 빨라지고 공격 빈도가 높아짐.
전환 조건: 일정 확률로 광폭화(Berserk) 상태로 전환.
Berserk (광폭화 상태 - 옵션)

행동: 이동 속도와 공격 속도가 증가하며, 플레이어에게 접촉 시 큰 피해를 입힘.
전환 조건: 일정 시간 후 다시 Idle 상태로 돌아감.

 */
}
