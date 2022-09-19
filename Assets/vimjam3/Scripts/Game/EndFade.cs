using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

public class EndFade : MonoBehaviour
{
    [SerializeField] private List<Image> m_Images;
    [SerializeField] private List<TMP_Text> m_Texts;

    private float m_FadeSpeed = 0.1f;

    private float m_Alpha = 0.0f;

    public IEnumerator Fade()
    {
        while (m_Alpha < 1.0f)
        {
            foreach (var image in m_Images)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, m_Alpha);
            }

            foreach (var text in m_Texts)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, m_Alpha);
            }

            m_Alpha += m_FadeSpeed;
            yield return new WaitForSeconds(0.1f);
        }
        foreach (var image in m_Images)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b);
        }

        foreach (var text in m_Texts)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b);
        }
        yield return new WaitForSecondsRealtime(3.0f);
        GetComponent<StarterAssetsInputs>().SetCursorState(false);
        SceneLoader.Instance.UnloadScene(Scenes.GameScene);
        SceneLoader.Instance.LoadSceneAdditive(Scenes.MainMenu);
    }
}
