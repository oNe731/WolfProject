using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler// IPointerUpHandler, IPointerDownHandler
{
    private float m_leverRange = 50f;

    private bool m_isInput;
    private Vector2 m_inputVector = new Vector2(-1f, 0f);

    private RectTransform m_lever;
    private RectTransform m_rectTransform;

    public bool IsInput => m_isInput;
    public Vector2 InputVector => m_inputVector;

    private void Awake()
    {
        m_lever = transform.GetChild(0).GetComponent<RectTransform>();
        m_rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_isInput = true;
        Control_JoystickLever(eventData);
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        Control_JoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_lever.anchoredPosition = Vector2.zero;
        m_isInput = false;
    }

    public void Control_JoystickLever(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // rectTransform의 중심을 기준으로 inputDir을 계산
        var inputDir = localPoint;

        // 입력 벡터를 클램핑
        var clampedDir = inputDir.magnitude < m_leverRange ? inputDir
            : inputDir.normalized * m_leverRange;

        // 레버의 위치를 설정
        m_lever.anchoredPosition = clampedDir;

        // 조이스틱의 입력 벡터를 저장
        m_inputVector = clampedDir / m_leverRange;
    }
}
