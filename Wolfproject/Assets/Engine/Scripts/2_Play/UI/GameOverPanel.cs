using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    public void Button_Main()
    {
        GameManager.Ins.Change_Scene(GameManager.SCENE.SCENE_MAIN);
    }

    public void Button_Retry()
    {
        GameManager.Ins.Change_Scene(GameManager.SCENE.SCENE_PLAY);
    }
}
