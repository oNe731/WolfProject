using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHp : Item
{
    protected override void Triger_Event()
    {
        //ItemData itemData = new ItemData();
        //itemData.itemType = ItemData.TYPE.IT_HP;
        //itemData.count = 1;
        // GameManager.Ins.Play.Player.Inven.Add_Item(itemData);
        GameManager.Ins.Play.Player.Recover_Player(5f);
    }
}
