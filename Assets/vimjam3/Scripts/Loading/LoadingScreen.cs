using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject m_Screen = null;

    [SerializeField] private TMP_Text m_Details = null;

    public bool IsShowing
    {
        get => m_Screen.activeSelf;
    }

    private static LoadingScreen m_Instance = null;
    public static LoadingScreen Instance
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

    private void Awake()
    {
        Instance = this;
    }

    public void Show()
    {
        m_Screen.SetActive(true);
    }

    public void Hide()
    {
        m_Screen.SetActive(false);
    }

    public void SetText(string newText)
    {
        m_Details.text = newText;
    }
}
