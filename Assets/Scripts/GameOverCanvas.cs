using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverCanvas: MonoBehaviour
{
    public SceneManagerController sceneManager;

    public void onRetryButtonTapped()
    {
        this.sceneManager.reloadCurrentScene();
    }
}
