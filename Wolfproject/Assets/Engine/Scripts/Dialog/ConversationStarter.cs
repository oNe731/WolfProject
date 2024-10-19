using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    private void OnTriggerStay2D(Collider2D collision)
    {
            if (collision.CompareTag("Player"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                ConversationManager.Instance.StartConversation(myConversation);
                
                }
            }
    }

}
