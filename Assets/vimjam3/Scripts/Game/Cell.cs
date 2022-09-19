using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cell : Interactable
{
    [SerializeField] private GameObject m_Door = null;
    [SerializeField] private TMP_Text m_Duration = null;

    private static bool m_HasPlayedLow = false;
    private static bool m_HasPlayedRandom = false;
    private static bool m_HasPlayedHigh = false;

    private int m_Value = 0;

    private void Start()
    {
        m_Value = Random.Range(1, 9999);
        m_Duration.text = $"{m_Value} years\nremaining";
    }

    public override void Interact()
    {
        if (ShooterController.m_Instance.GetKeys() > 0)
        {
            m_Door.SetActive(false);
            ShooterController.m_Instance.RemoveKey();
            ShooterController.m_Instance.UpdateKeyCount();
            PlayVoiceLine();
            m_Duration.text = string.Empty;
            Destroy(gameObject);
        }

        if (ShooterController.m_Instance.GetKeys() <= 0)
        {
            foreach (Cell cell in FindObjectsOfType<Cell>())
            {
                Destroy(cell.gameObject);
            }

            AudioManager.Instance.PlayVoiceLine(VoiceLines.Goodbye);
        }
    }

    private void PlayVoiceLine()
    {
        if (m_Value < 1000 && !m_HasPlayedLow)
        {
            m_HasPlayedLow = true;
            AudioManager.Instance.PlayVoiceLine(VoiceLines.Release_Low);
        }
        else if (m_Value > 8000 && !m_HasPlayedHigh)
        {
            m_HasPlayedHigh = true;
            AudioManager.Instance.PlayVoiceLine(VoiceLines.Release_High);
        }
        else if (m_Value <= Random.Range(1, 9999) && !m_HasPlayedRandom)
        {
            m_HasPlayedRandom = true;
            AudioManager.Instance.PlayVoiceLine(VoiceLines.Release_Random);
        }

    }
}
