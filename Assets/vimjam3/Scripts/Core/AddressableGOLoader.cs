using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using StarterAssets;

public class AddressableGOLoader : MonoBehaviour
{
    /*[SerializeField] private List<AssetReferenceGameObject> m_AssetGroup1 = new List<AssetReferenceGameObject>();
    [SerializeField] private List<AssetReferenceGameObject> m_AssetGroup2 = new List<AssetReferenceGameObject>();
    [SerializeField] private List<AssetReferenceGameObject> m_AssetGroup3 = new List<AssetReferenceGameObject>();
    [SerializeField] private List<AssetReferenceGameObject> m_AssetGroup4 = new List<AssetReferenceGameObject>();
    [SerializeField] private List<AssetReferenceGameObject> m_AssetGroup5 = new List<AssetReferenceGameObject>();

    private List<AssetReferenceGameObject> m_CurrentGroup = null;
    private int m_CurrentGroupIndex = -1;
    private bool m_IsCurrentlyLoading = false;

    private bool m_Finished = false;

    private void Start()
    {
        m_CurrentGroup = m_AssetGroup1;
        m_CurrentGroupIndex = 1;

        StartCoroutine(LoadAllGroups());
    }

    IEnumerator LoadAllGroups()
    {
        while (true)
        {
            while (!m_Finished)
            {
                if (m_CurrentGroup != null)
                {
                    if (!m_IsCurrentlyLoading)
                    {
                        LoadAssetGroup();
                        m_IsCurrentlyLoading = true;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        Debug.Log("Loading done!");
    }

    private void LoadAssetGroup()
    {
        if (m_CurrentGroup.Count == 0)
        {
            CheckForNextGroup();
            return;
        }

        foreach (var asset in m_CurrentGroup)
        {
            asset.LoadAssetAsync().Completed += (op) =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    Instantiate(asset.Asset, Vector3.zero, Quaternion.identity);
                    m_CurrentGroup.Remove(asset);

                    CheckForNextGroup();
                }
            };
        }
    }

    private void CheckForNextGroup()
    {
        if (m_CurrentGroup.Count == 0)
        {
            switch (m_CurrentGroupIndex)
            {
                case 1:
                    m_CurrentGroup = m_AssetGroup2;
                    break;
                case 2:
                    m_CurrentGroup = m_AssetGroup3;
                    break;
                case 3:
                    m_CurrentGroup = m_AssetGroup4;
                    break;
                case 4:
                    m_CurrentGroup = m_AssetGroup5;
                    break;
                case 5:
                    m_Finished = true;
                    break;
                default:
                    break;
            }
            m_CurrentGroupIndex++;
            m_IsCurrentlyLoading = false;
        }
    }*/

    /*[SerializeField] private AssetReferenceGameObject m_Prefab = null;
    private AsyncOperationHandle m_PrefabHandle;
    [SerializeField] private Transform m_Spawn = null;
    [SerializeField] private string m_ObjectName = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_ObjectName != null)
        {
            if (LoadingScreen.Instance != null)
            {
                LoadingScreen.Instance.SetText($"Loading {m_ObjectName}...");
            }
        }

        m_PrefabHandle = m_Prefab.LoadAssetAsync();
        m_PrefabHandle.Completed += M_PlayerPrefabHandle_Completed;
    }

    private void M_PlayerPrefabHandle_Completed(AsyncOperationHandle obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject playerGO = (GameObject)Instantiate(m_Prefab.Asset, m_Spawn.position, Quaternion.identity);
            StarterAssetsInputs inputs = playerGO.GetComponentInChildren<StarterAssetsInputs>();
            inputs.SetCursorState(true);

            LoadingScreen.Instance.Hide();
        }
        else
        {
            Debug.LogError($"AssetReference {m_Prefab.RuntimeKey} failed to load.");
        }
    }

    public void UnloadAssets()
    {
        m_Prefab.ReleaseAsset();
    }

    private void OnDestroy()
    {
        UnloadAssets();
    }*/

    private static AddressableGOLoader m_CurrentInstance = null;
    private static AddressableGOLoader m_NextInstance = null;

    [SerializeField] private List<AssetReferenceGameObject> m_Assets = new List<AssetReferenceGameObject>();

    private void Awake()
    {
        if (m_CurrentInstance == null)
        {
            m_CurrentInstance = this;
        }
        else
        {
            m_NextInstance = this;
        }
    }

    private void Start()
    {
        if (m_CurrentInstance == this)
        {
            LoadAssets();
        }
    }

    public void LoadAssets()
    {
        if (m_Assets.Count == 0)
        {
            FinishedLoading();
            return;
        }

        foreach (var asset in m_Assets)
        {
            asset.LoadAssetAsync().Completed += (op) =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    Instantiate(asset.Asset);
                    m_Assets.Remove(asset);

                    if (m_Assets.Count == 0)
                    {
                        FinishedLoading();
                        return;
                    }
                }
            };
        }
    }

    private void FinishedLoading()
    {
        if (m_NextInstance != null)
        {
            m_CurrentInstance = m_NextInstance;
            m_NextInstance = null;
            m_CurrentInstance.LoadAssets();
        }
        else
        {
            m_CurrentInstance = null;
            if (LoadingScreen.Instance != null)
            {
                if (LoadingScreen.Instance.IsShowing)
                {
                    LoadingScreen.Instance.Hide();
                }
            }
        }
    }
}