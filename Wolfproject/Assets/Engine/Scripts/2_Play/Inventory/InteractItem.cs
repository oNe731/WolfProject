using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem : MonoBehaviour
{
    public bool HaveKey = false;

    public void SetKey()
    {
        HaveKey = true;
    }
    public void LoseKey()
    {
        HaveKey = false;
    }

   
}
