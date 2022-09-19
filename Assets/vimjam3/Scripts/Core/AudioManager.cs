using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> m_AudioClips = new List<AudioClip>();
    [SerializeField] private AudioMixerGroup m_SFXMixer = null;

    private List<AudioSource> m_VoiceSources = new List<AudioSource>();

    private static AudioManager m_Instance;
    public static AudioManager Instance
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
        DontDestroyOnLoad(gameObject);
    }

    public void PlayVoiceLine(VoiceLines voiceLine)
    {
        foreach (var vc in m_VoiceSources)
        {
            if (vc != null)
            {
                vc.Stop();
            }
        }
        m_VoiceSources.Clear();

        GameObject go = new GameObject("voiceLine");
        go.transform.SetParent(ShooterController.m_Instance.transform);
        var src = go.AddComponent<AudioSource>();
        src.outputAudioMixerGroup = m_SFXMixer;
        src.clip = m_AudioClips[(int)voiceLine];
        src.Play();
        m_VoiceSources.Add(src);
        Destroy(go, src.clip.length);
    }

    public void PlayAudioAtPoint(AudioClip clip, Vector3 position)
    {
        GameObject go = new GameObject("Audio");
        var src = go.AddComponent<AudioSource>();
        src.outputAudioMixerGroup = m_SFXMixer;
        src.clip = clip;
        src.Play();
        Destroy(go, clip.length);
    }
}

public enum VoiceLines
{
    Welcome,
    Welcome_Mature,
    Release_Low,
    Release_High,
    Release_High_Mature,
    Release_Random,
    Goodbye,
    Goodbye_Mature,
}
