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
    private float introCountdown = 3f;
    private bool shouldCountdown = true;

    void Start()
    {
        this.sceneManager.currentSceneIndex = 1;
        this.mainImage.sprite = this.story1Sprite;
        this.mainText.text = "Humanity needs a hero";
    }

    void Update()
    {
        if (this.shouldCountdown)
        {
            this.introCountdown -= Time.deltaTime;
            if (this.introCountdown <= 0)
            {
                this.nextStage();
                this.introCountdown = 3f;
            }
        }
    }

    void nextStage()
    {
        this.stage += 1;

        if (this.stage == 1)
        {
            this.mainImage.sprite = this.story2Sprite;
            this.mainText.text = "Our Earth's core has been stolen by evil creatures";
        }
        else if (this.stage == 2)
        {
            this.mainImage.sprite = this.story3Sprite;
            this.mainText.text = "Recover the core or insects will rule the world!!";
        }
        else 
        {
            this.shouldCountdown = false;
            this.sceneManager.goToNextScene();
        }
    }
}
