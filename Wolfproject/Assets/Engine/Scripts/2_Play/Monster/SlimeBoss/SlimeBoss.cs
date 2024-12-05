using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : Monster
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
        if (GameManager.Ins.IsGame == false)
            return;
    }

    /*
     * 3. 대왕 슬라임 (보스)
기본 정보

녹색 상태: 화염에 약하고 번개에 강함.
파란색 상태: 번개에 약하고 체력을 회복.
광폭화 상태: 모든 속성에 중간 저항.
피해량: 접촉 시 20의 피해.


FSM 구조

Idle (대기 상태)
행동: 천천히 배회하며 플레이어를 탐색.
전환 조건: 플레이어가 일정 범위 내에 들어오면 Chase 상태로 전환.

Chase (추격 상태)
행동: 느린 속도로 플레이어를 추격하며, 주기적으로 점프해 거리 좁힘.
전환 조건: 일정 시간 후 Ground Slam 상태로 전환.

Ground Slam (충격파 상태)
행동: 대왕 슬라임이 공중으로 뛰어올라 넓은 범위로 충격파를 발생.
효과: 충격파에 맞은 플레이어는 2초 스턴.
전환 조건: 충격파 사용 후 **Phase Shift(색상 변화 상태)**로 전환.

Phase Shift (색상 변화 상태)
녹색 상태: 화염에 약함. 작은 슬라임 2~3마리 소환.
파란색 상태: 번개에 약함. 3단계 체력 회복 시도.
1단계: 체력 10% 회복 + 슬라임 조각 1개 분출
2단계: 체력 15% 회복 + 슬라임 조각 2개 분출
3단계: 체력 20% 회복 + 슬라임 조각 3개 분출

Berserk (광폭화 상태)
행동: 붉은색으로 변하며 공격 속도와 이동 속도가 크게 증가.

추가 패턴:
자폭 슬라임 소환
넓은 범위의 충격파 발생
전환 조건: 플레이어를 일정 시간 압박 후 다시 Chase 상태로 돌아감.


# 적 간의 차별화된 특징 요약
슬라임
기본 공격형 적으로, 점프 공격과 광폭화 패턴이 특징.
번개에 약하지만 화염에 강함.

버섯
은신과 돌진 공격을 사용하는 교묘한 적.
화염에 약하지만 번개에 강함.
체력이 줄어들면 다른 버섯과 합류해 회복.

대왕 슬라임 (보스)
다양한 색상 변화와 체력 회복 패턴을 사용하는 강력한 보스.
광폭화 상태에서는 속성 상성이 무력화되며, 자폭 슬라임과 충격파로 플레이어를 압박.
     */
}
