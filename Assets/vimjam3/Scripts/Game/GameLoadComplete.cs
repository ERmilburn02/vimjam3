using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadComplete : MonoBehaviour
{
    private void Start()
    {
        LoadingScreen.Instance.SetText("Loading player...");
    }
}
