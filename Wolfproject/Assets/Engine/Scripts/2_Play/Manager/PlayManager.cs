using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Game());
    }

    private void Start_Game()
    {

    }

    private void Update()
    {
        
    }
}
