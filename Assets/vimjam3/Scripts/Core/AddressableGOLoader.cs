using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using StarterAssets;

public class AddressableGOLoader : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject m_Prefab = null;
    private AsyncOperationHandle m_PrefabHandle;
    [SerializeField] private Transform m_Spawn = null;

    // Start is called before the first frame update
    void Start()
    {
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
    }
}