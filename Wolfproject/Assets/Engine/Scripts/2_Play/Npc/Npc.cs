using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public enum TYPE { TYPE_END, TYPE_TREE1, TYPE_TREE2, TYPE_TRAVEL }

    [SerializeField] private TYPE m_type;
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

            if (m_type == TYPE.TYPE_TREE1)
                GameManager.Ins.Play.LevelState = PlayManager.LEVELSTATE.STATE_TREENPC1;
            else if(m_type == TYPE.TYPE_TREE2)
                GameManager.Ins.Play.LevelState = PlayManager.LEVELSTATE.STATE_TREENPC2;
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
