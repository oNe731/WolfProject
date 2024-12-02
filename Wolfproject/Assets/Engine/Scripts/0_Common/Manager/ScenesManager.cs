using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ScenesManager
{
    protected GameManager.SCENE m_sceneLevel = GameManager.SCENE.SCENE_END;
    protected string m_sceneName;

    protected bool m_isVisit = false;

    public GameManager.SCENE StageLevel => m_sceneLevel;
    public string SceneName => m_sceneName;

    public ScenesManager()
    {
        Load_Resource();
    }

    protected abstract void Load_Resource();

    public virtual void Enter_Game()
    {
        SceneManager.LoadScene(m_sceneName);
        GameManager.Ins.StartCoroutine(Load_SceneAndRun());
    }

    protected IEnumerator Load_SceneAndRun()
    {
        // �񵿱�� ���� �ε�
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_sceneName);

        // ���� ������ �ε�� ������ ���
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // �� ��ȯ �� �ʱ�ȭ ����
        GameManager.Ins.Sound.Update_AllAudioSources();

        Load_Scene();
    }

    protected abstract void Load_Scene();

    protected virtual void Start_Game()
    {
        GameManager.Ins.IsGame = true;
    }

    public virtual void Update_Game()
    {
    }

    public virtual void LateUpdate_Game()
    {
    }

    public virtual void Exit_Game()
    {
        GameManager.Ins.IsGame = false;
    }

    public virtual void Over_Game()
    {
    }
}
