using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;

    private UIManager m_uIManager = null;

    public static GameManager Ins => m_instance;
    public UIManager UI => m_uIManager;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;

            m_uIManager = gameObject.AddComponent<UIManager>();

            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        
    }

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
}
