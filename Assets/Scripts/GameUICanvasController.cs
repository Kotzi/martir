using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUICanvasController: MonoBehaviour
{
    public TMP_Text livesText;
    public Image countdownImage;
    public TMP_Text countdownText;
    public CanvasRenderer mobileCanvas;
    public ShootButton shootButton;
    public Joystick joystick;

    void Awake()
    {
        this.mobileCanvas.gameObject.SetActive(Platform.isMobileBrowser());
        this.updateCountdown(-1);
    }

    public void updateLives(int lives)
    {
        this.livesText.text = "Lives: " + lives.ToString();
    }

    public void updateCountdown(int countdown)
    {
        if (countdown != -1)
        {
            var currentNumber = int.Parse(this.countdownText.text);
            if (currentNumber != countdown) 
            {
                this.countdownImage.enabled = true;
                this.countdownText.enabled = true;
                this.countdownText.transform.localScale = Vector3.one;
                this.countdownText.text = countdown.ToString();
                if (currentNumber != -1)
                {
                    this.countdownText.transform.DOPunchScale(Vector3.one * 1.15f, 0.1f);
                }
            }
        }
        else 
        {
            this.countdownText.text = "-1";
            this.countdownText.transform.localScale = Vector3.one;
            this.countdownImage.enabled = false;
            this.countdownText.enabled = false;
        }
    }
}
