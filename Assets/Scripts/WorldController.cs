using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    const float POWER_UP_MAX_TIME = 5f;
    public GameObject shootPowerUpPrefab;
    public GameObject cooldownPowerUpPrefab;
    public GameObject[] enemySpawnZones;
    public Transform cameraTransform;
    public GameUICanvasController gameUICanvasController;
    public GameOverCanvas gameOverCanvas;

    private bool gameActive = true;
    private float powerUpTime = 5f;

    void Update()
    {
        if (gameActive)
        {
            this.powerUpTime -= Time.deltaTime;

            if (this.powerUpTime <= 0)
            {
                var powerUp = Instantiate((Random.value >= 0.5 ? this.shootPowerUpPrefab : this.cooldownPowerUpPrefab), this.cameraTransform.position, this.cameraTransform.rotation, this.cameraTransform);
                powerUp.transform.localPosition = new Vector3(Random.Range(-3f, 3f), Random.Range(0.5f, 5f), 10f);
                this.powerUpTime = POWER_UP_MAX_TIME;
            }
        }
    }

    public void updateLives(int lives)
    {
        this.gameUICanvasController.updateLives(lives);
    }

    public void updateCountdown(int countdown)
    {
        this.gameUICanvasController.updateCountdown(countdown);
    }

    public void playerDied(bool runOutOfTime)
    {
        this.gameActive = false;
        for (int i = 0; i < this.enemySpawnZones.Length; i++)
        {
            this.enemySpawnZones[i].SetActive(false);
        }
        
        this.gameOverCanvas.gameObject.SetActive(true);
    }
}
