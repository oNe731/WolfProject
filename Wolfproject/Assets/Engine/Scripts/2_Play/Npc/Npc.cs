using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private string m_path;
    [SerializeField] private float m_dist;
    private bool m_talk = false;

    private void Update()
    {
        if(GameManager.Ins.Play == null || GameManager.Ins.Play.Player == null)
            return;

        if (m_talk == true)
            return;

        if (Vector3.Distance(transform.position, GameManager.Ins.Play.Player.transform.position) <= m_dist)
        {
            m_talk = true;
            GameManager.Ins.Play.Dialog.Start_Dialog(m_path);
        }
    }

    private void OnDrawGizmos()
    {
        if (m_talk == true)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_dist);
    }
}
