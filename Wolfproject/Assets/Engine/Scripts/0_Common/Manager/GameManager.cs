using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.EventSystems;

public enum DIRECTION { DT_UP, DT_DOWN, DT_LEFT, DT_RIGHT, DT_END }

public class GameManager : MonoBehaviour
{
    public enum SCENE { SCENE_MAIN, SCENE_CUT, SCENE_NAME, SCENE_TUTORIAL, SCENE_PLAY, SCENE_END }

    private bool m_isGame = false;
    private int m_curScene = -1;
    private int m_preScene = -1;
    private List<ScenesManager> m_scenes;

    private string m_playerName = "";
    private int m_zenScore = 0;
    private int m_chaosScore = 0;
    private AudioSource m_audioSource;

    private static GameManager m_instance = null;
    private UIManager m_uIManager = null;
    private SoundManager m_soundManager = null;
    private Panel_Option m_option;

    public bool IsGame { get => m_isGame; set => m_isGame = value; }
    public int CurScene { get => m_curScene; }
    public int PreScene { get => m_preScene; }
    public CutManager Cut => (CutManager)m_scenes[(int)SCENE.SCENE_CUT];
    public TutorialManager Tutorial => (TutorialManager)m_scenes[(int)SCENE.SCENE_TUTORIAL];
    public PlayManager Play => (PlayManager)m_scenes[(int)SCENE.SCENE_PLAY];

    public string PlayerName => m_playerName;
    public int ZenScore => m_zenScore;
    public int ChaosScore => m_chaosScore;
    public AudioSource AudioSource => m_audioSource;

    public static GameManager Ins => m_instance;
    public UIManager UI => m_uIManager;
    public SoundManager Sound => m_soundManager;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            m_uIManager = gameObject.AddComponent<UIManager>();
            m_soundManager = gameObject.AddComponent<SoundManager>();

            m_option = gameObject.transform.GetChild(0).GetComponent<Panel_Option>();
            m_audioSource = gameObject.GetComponent<AudioSource>();

            // 매니저
            m_scenes = new List<ScenesManager>();
            m_scenes.Add(new StartManager());
            m_scenes.Add(new CutManager());
            m_scenes.Add(new NameManager());
            m_scenes.Add(new TutorialManager());
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
        {
            m_preScene = m_curScene;
            m_scenes[(int)m_curScene].Exit_Game();
        }
    
        m_curScene = (int)sceneType;
        m_scenes[(int)m_curScene].Enter_Game();
    }

    public void Set_Pause(bool pause)
    {
        m_isGame = !pause;
        if (m_curScene != -1)
            m_scenes[(int)m_curScene].Set_Pause(pause);
    }

    public void Set_PlayerName(string name)
    {
        if (m_playerName != "")
            return;

        m_playerName = name;
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

    public GameObject Create(GameObject obj, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null)
    {
        return Instantiate(obj, position, rotation, parent);
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        Object.Destroy(gameObject);
    }

    public List<T> Load_JsonData<T>(string filePath)
    {
        TextAsset jsonAsset = Load<TextAsset>(filePath);

        if (jsonAsset != null)
            return JsonConvert.DeserializeObject<List<T>>(jsonAsset.text);
        else
            Debug.LogError($"Failed to load Jsondata : {filePath}");

        return null;
    }
    #endregion
}
