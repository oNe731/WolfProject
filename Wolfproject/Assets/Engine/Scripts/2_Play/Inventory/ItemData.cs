using System;
using UnityEngine;

[Serializable]
public class ItemData
{
    public enum TYPE { IT_HP, IT_BUFF, IT_SHIELD, IT_END }

    public TYPE itemType;
    public int count;
}