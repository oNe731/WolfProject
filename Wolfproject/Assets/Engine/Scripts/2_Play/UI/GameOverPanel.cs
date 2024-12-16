using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    public void Button_Main()
    {
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene(GameManager.SCENE.SCENE_MAIN), 0.2f, false);
    }

    public void Button_Retry()
    {
        //GameManager.Ins.Change_Scene(GameManager.SCENE.SCENE_PLAY);
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Play.Restart_Game(), 0.2f, false);
    }
}
