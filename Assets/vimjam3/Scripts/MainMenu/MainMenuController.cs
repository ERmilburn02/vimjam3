using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button[] m_MenuButtons;

    [SerializeField] private GameObject m_RotationPoint = null;

    private bool m_HasLoaded = false;

    private void Start()
    {
        // Unload Bootstrap
        if (SceneManager.GetSceneByBuildIndex(0).isLoaded)
        {
            SceneManager.UnloadSceneAsync(0);
        }

#if UNITY_WEBGL || UNITY_IOS || UNITY_ANDROID
        // On these platforms, we don't quit, the system does.
        m_MenuButtons[m_MenuButtons.Length - 1].gameObject.SetActive(false);
#endif
    }

    private void Update()
    {
        if (!m_HasLoaded)
        {
            if (SceneLoader.Instance != null)
            {
                LoadingScreen.Instance.Hide();
                if (!LoadingScreen.Instance.IsShowing)
                    m_HasLoaded = true;
            }
        }

        m_RotationPoint.transform.Rotate(new Vector3(0, 0.15f, 0));
    }

    public void OnPlayButtonClicked()
    {
        DisableButtons();
        LoadingScreen.Instance.SetText("Loading Game Scene...");
        LoadingScreen.Instance.Show();
        SceneLoader.Instance.UnloadScene(Scenes.MainMenu);
        SceneLoader.Instance.LoadSceneAdditive(Scenes.GameScene);
    }

    public void OnExitButtonClicked()
    {
        DisableButtons();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }

    public void DisableButtons()
    {
        foreach (var button in m_MenuButtons)
        {
            button.interactable = false;
        }
    }

    public void EnableButtons()
    {
        foreach (var button in m_MenuButtons)
        {
            button.interactable = true;
        }
    }

}
