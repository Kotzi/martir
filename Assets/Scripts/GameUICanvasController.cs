using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameUICanvasController: MonoBehaviour
{
    public TMP_Text livesText;
    public TMP_Text countdownText;
    public CanvasRenderer mobileCanvas;
    public ShootButton shootButton;
    public Joystick joystick;

    void Awake()
    {
        this.mobileCanvas.gameObject.SetActive(Platform.isMobileBrowser());
    }

    public void updateLives(int lives)
    {
        this.livesText.text = "Lives: " + lives.ToString();
    }

    public void updateCountdown(int countdown)
    {
        if (countdown != -1)
        {
            this.countdownText.enabled = true;
            this.countdownText.transform.localScale = Vector3.one;
            this.countdownText.text = countdown.ToString();
            this.countdownText.transform.DOPunchScale(Vector3.one * 1.15f, 0.1f);
        }
        else 
        {
            this.countdownText.enabled = false;
        }
    }
}
