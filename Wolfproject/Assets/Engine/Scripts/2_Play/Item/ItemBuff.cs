using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuff : Item
{
    protected override void Triger_Event()
    {
        ItemData itemData = new ItemData();
        itemData.itemType = ItemData.TYPE.IT_BUFF;
        itemData.count = 1;

        GameManager.Ins.Play.Player.Inven.Add_Item(itemData);
    }
}
