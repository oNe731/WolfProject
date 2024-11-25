using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public void Initialize_Character()
    {
        Collider2D collider1 = GetComponent<BoxCollider2D>();
        Collider2D collider2 = transform.GetChild(0).GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(collider1, collider2);
    }
}
