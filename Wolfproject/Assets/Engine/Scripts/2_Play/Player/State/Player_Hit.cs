using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hit : Player_Base
{
    float m_time = 0f;

    protected Coroutine m_colorCorutine = null;
    protected Coroutine m_fadeCorutine = null;

    public Player_Hit(StateMachine<Player> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_owner.AM.SetTrigger("IsHit");

        // ÀÌÆåÆ® »ý¼º
        GameObject obj = GameManager.Ins.LoadCreate("4_Prefab/5_Effect/Hit");
        if (obj != null)
        {
            obj.transform.position = new Vector3(m_owner.transform.position.x + Random.Range(-0.2f, 0.2f), m_owner.transform.position.y + Random.Range(-0.2f, 0.2f), m_owner.transform.position.z);
        }

        if (m_colorCorutine != null)
            m_owner.StopCoroutine(m_colorCorutine);
        m_colorCorutine = m_owner.StartCoroutine(Change_Color(new Color(1f, 1f, 1f, 1f), new Color(1f, 0f, 0f, 1f), 0.3f));
    }

    public override void Update_State()
    {
        m_time += Time.deltaTime;
        if(m_time > 0.1f)
        {
            m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }

    protected IEnumerator Change_Color(Color startColor, Color changeColor, float fadespeed)
    {
        Start_Fade(startColor, changeColor, fadespeed);

        while (true)
        {
            if (m_fadeCorutine == null)
                break;
            yield return null;
        }

        Start_Fade(changeColor, startColor, fadespeed);
        yield break;
    }

    public void Start_Fade(Color startColor, Color changeColor, float duration)
    {
        if (m_fadeCorutine != null)
            m_owner.StopCoroutine(m_fadeCorutine);
        m_fadeCorutine = m_owner.StartCoroutine(FadeCoroutine(startColor, changeColor, duration));
    }

    private IEnumerator FadeCoroutine(Color startColor, Color changeColor, float duration)
    {
        float currentTime = 0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float fadeProgress = currentTime / duration;
            Set_Color(new Color(
                Mathf.Lerp(startColor.r, changeColor.r, fadeProgress),
                Mathf.Lerp(startColor.g, changeColor.g, fadeProgress),
                Mathf.Lerp(startColor.b, changeColor.b, fadeProgress),
                Mathf.Lerp(startColor.a, changeColor.a, fadeProgress)));

            yield return null;
        }

        if (m_fadeCorutine != null)
        {
            m_owner.StopCoroutine(m_fadeCorutine);
            m_fadeCorutine = null;
        }

        yield break;
    }

    protected void Set_Color(Color color)
    {
        m_owner.Sr.material.color = color;
    }
}
