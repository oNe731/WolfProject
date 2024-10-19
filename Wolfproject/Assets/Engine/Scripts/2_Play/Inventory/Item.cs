using UnityEngine;
using InventorySystem;
namespace InventorySampleScene
{
    public class Item : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        string ItemName;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Player")
            {
                if (!InventoryController.instance.InventoryFull("Bar", ItemName))
                {
                    InventoryController.instance.AddItem("Bar", ItemName);
                    Destroy(gameObject);

                }
                else
                {
                    Debug.Log("Inventory Cannot Fit Item");
                }

            }
        }
    }
}
