using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private float m_leverRange = 100f;

    private bool m_isInput;
    private Vector2 m_inputVector = new Vector2(0f, -1f);

    private RectTransform m_lever;
    private RectTransform m_rectTransform;
    private Vector2 m_startPosition;

    public bool IsInput => m_isInput;
    public Vector2 InputVector => m_inputVector;

    private void Awake()
    {
        m_rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        m_startPosition = m_rectTransform.anchoredPosition;

        m_lever = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }

    // Ŭ������ ��
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Ins.IsGame == false)
            return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPoint);
        m_rectTransform.anchoredPosition = localPoint;
    }

    // Ŭ������ ��
    public void OnPointerUp(PointerEventData eventData)
    {
        m_rectTransform.anchoredPosition = m_startPosition;
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
        m_rectTransform.anchoredPosition = m_startPosition;

        m_lever.anchoredPosition = Vector2.zero;
        m_isInput = false;
    }

    public void Control_JoystickLever(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // rectTransform�� �߽��� �������� inputDir�� ���
        var inputDir = localPoint;

        // �Է� ���͸� Ŭ����
        var clampedDir = inputDir.magnitude < m_leverRange ? inputDir
            : inputDir.normalized * m_leverRange;

        // ������ ��ġ�� ����
        m_lever.anchoredPosition = clampedDir;

        // ���̽�ƽ�� �Է� ���͸� ����
        m_inputVector = clampedDir / m_leverRange;
    }
}
