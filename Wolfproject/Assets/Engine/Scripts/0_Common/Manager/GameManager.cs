using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum SCENE { SCENE_MAIN, SCENE_PLAY, SCENE_END }

    private bool m_isGame = false;
    private int m_curScene = -1;
    private List<ScenesManager> m_scenes;

    private static GameManager m_instance = null;
    private UIManager m_uIManager = null;

    public bool IsGame { get => m_isGame; set => m_isGame = value; }
    public static GameManager Ins => m_instance;
    public PlayManager Play => (PlayManager)m_scenes[(int)SCENE.SCENE_PLAY];
    public UIManager UI => m_uIManager;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            m_uIManager = gameObject.AddComponent<UIManager>();

            // 매니저
            m_scenes = new List<ScenesManager>();
            m_scenes.Add(new StartManager());
            m_scenes.Add(new PlayManager());

            Scene currentScene = SceneManager.GetActiveScene();
            for (int i = 0; i < m_scenes.Count; ++i)
            {
                if (m_scenes[i].SceneName == currentScene.name)
                {
                    Change_Scene(m_scenes[i].StageLevel);
                    break;
                }
            }

            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if (m_curScene == -1)
            return;

        m_scenes[(int)m_curScene].Update_Game();
    }

    private void LateUpdate()
    {
        if (m_curScene == -1)
            return;

        m_scenes[(int)m_curScene].LateUpdate_Game();
    }

    public void Change_Scene(SCENE sceneType)
    {
        if (m_curScene != -1)
            m_scenes[(int)m_curScene].Exit_Game();

        m_curScene = (int)sceneType;
        m_scenes[(int)m_curScene].Enter_Game();
    }

    public void Set_Pause(bool pause)
    {
        m_isGame = !pause;
    }

    #region 라이브러리
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }

    public GameObject LoadCreate(string path, Transform transform = null)
    {
        GameObject prefab = Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return Instantiate(prefab, transform);
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        Object.Destroy(gameObject);
    }
    #endregion
}
