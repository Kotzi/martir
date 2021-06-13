using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class IntroController : MonoBehaviour
{
    public Sprite story1Sprite;
    public Sprite story2Sprite;
    public Sprite story3Sprite;
    public Image mainImage;
    public TMP_Text mainText;
    public SceneManagerController sceneManager;
    private int stage = 0;

    void Start()
    {
        this.sceneManager.currentSceneIndex = 0;
        this.mainImage.sprite = this.story1Sprite;
    }

    public void onContinueButtonPressed()
    {
        this.stage += 1;

        if (this.stage == 1)
        {
            this.mainImage.sprite = this.story2Sprite;
        }
        else if (this.stage == 2)
        {
            this.mainImage.sprite = this.story3Sprite;
        }
        else 
        {
            this.sceneManager.goToNextScene();
        }
    }
}
