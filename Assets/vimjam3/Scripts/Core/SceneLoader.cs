using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader m_Instance = null;
    public static SceneLoader Instance
    {
        get => m_Instance;
        set
        {
            if (m_Instance == null)
            {
                m_Instance = value;
            }
            else if (m_Instance != value)
            {
                Destroy(value.gameObject);
            }
        }
    }

    [SerializeField] private AssetReference[] m_Scenes;

    private Dictionary<int, AsyncOperationHandle<SceneInstance>> handles = new Dictionary<int, AsyncOperationHandle<SceneInstance>>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void LoadScene(Scenes scene)
    {
        Addressables.LoadSceneAsync(m_Scenes[(int)scene], LoadSceneMode.Single).Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                handles.Add((int)scene, op);
                SceneManager.SetActiveScene(op.Result.Scene);
            }
        };
    }

    public void LoadSceneAdditive(Scenes scene)
    {
        Addressables.LoadSceneAsync(m_Scenes[(int)scene], LoadSceneMode.Additive).Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                handles.Add((int)scene, op);
                SceneManager.SetActiveScene(op.Result.Scene);
            }
        };
    }

    public void UnloadScene(Scenes scene)
    {
        if (handles.ContainsKey((int)scene))
        {
            Addressables.UnloadSceneAsync(handles[(int)scene], true).Completed += (op) =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    handles.Remove((int)scene);
                }
            };
        }
        else
        {
            Debug.LogWarning($"Attempted to unload a scene that isn't loaded: {scene}");
        }
    }
}

public enum Scenes
{
    MainMenu,
    GameScene
}
/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoader : MonoBehaviour
{
    public AssetReference Scene;
    private AsyncOperationHandle<SceneInstance> handle;
    private bool unloaded;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Addressables.LoadSceneAsync(Scene,UnityEngine.SceneManagement.LoadSceneMode.Additive).Completed += SceneLoadCompleted;
    }

    private void SceneLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Successfully loaded Scene.");
            handle = obj;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !unloaded)
        {
            unloaded = true;
            UnloadScene();
        }
    }

    void UnloadScene()
    {
        Addressables.UnloadSceneAsync(handle, true).Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
                Debug.Log("Successfully unloaded Scene.");
        };
    }
}
*/