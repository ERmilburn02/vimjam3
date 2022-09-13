using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private AssetReference m_LoadingScreen = null;

    [SerializeField] private AssetReferenceGameObject[] m_PreloadedAssets;

    private int m_RemainingAssets;
    private int m_TotalAssets;

    private void Start()
    {
        Addressables.LoadSceneAsync(m_LoadingScreen, LoadSceneMode.Additive).Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                m_RemainingAssets = m_PreloadedAssets.Length;
                m_TotalAssets = m_PreloadedAssets.Length;
                LoadingScreen.Instance.SetText($"Preloading assets ({m_TotalAssets - m_RemainingAssets}/{m_TotalAssets})");
                PreloadAssets();
            }
        };
    }

    private void PreloadAssets()
    {
        foreach (var asset in m_PreloadedAssets)
        {
            asset.LoadAssetAsync().Completed += LoadAssetCompleted;
        }
    }

    private void LoadAssetCompleted(AsyncOperationHandle<GameObject> obj)
    {
        m_RemainingAssets--;
        LoadingScreen.Instance.SetText($"Preloading assets ({m_TotalAssets - m_RemainingAssets}/{m_TotalAssets})");
        if (m_RemainingAssets >= 0)
        {
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        SceneLoader.Instance.LoadSceneAdditive(Scenes.MainMenu);
    }
}