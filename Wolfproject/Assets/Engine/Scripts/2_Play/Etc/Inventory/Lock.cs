using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

public class Lock : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            InteractItem interactitem = collision.gameObject.GetComponent<InteractItem>();

            if (interactitem != null && interactitem.HaveKey)
            {
                InventoryController.instance.InventoryClear("Interact");
                Destroy(transform.parent.gameObject);
            }
        }

        
    }
    
}
