using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    const float POWER_UP_MAX_TIME = 5f;
    const float NEEDS_MORE_ENEMIES_TIME = 3f;
    const int MAX_ATTACHED_ENEMIES = 20;

    public AudioClip stage1Clip;
    public AudioClip stage2Clip;
    public AudioClip stage3Clip;
    public AudioClip youLostClip;
    public AudioClip youWonClip;
    public AudioSource mainAudio;
    public AudioSource oneShotAudio;
    public GameObject shootPowerUpPrefab;
    public GameObject cooldownPowerUpPrefab;
    public SceneManagerController sceneManager;
    public GameObject[] enemySpawnZones;
    public Transform cameraTransform;
    public GameUICanvasController gameUICanvasController;
    public GameOverCanvas gameOverCanvas;
    public YouWonCanvas youWonCanvas;
    public CloudsSpawnerController cloudsSpawnerController;
    public CameraController cameraController;
    public ShipController ship;
    public FinalBossController finalBoss;
    public TalismanController finalTalisman;
    public PlayerController player;

    private bool gameActive = true;
    private bool gameFinished = false;
    private float powerUpTime = 5f;
    private int needsMoreEnemies = 2;
    private float addMoreEnemiesTime = NEEDS_MORE_ENEMIES_TIME;
    private int attachedEnemies = 0;

    void Start()
    {
        this.sceneManager.currentSceneIndex = 1;
        
        if (Platform.isMobileBrowser())
        {
            this.player.mobileGameUICanvasController = this.gameUICanvasController;
        }

        for (int i = 0; i < this.enemySpawnZones.Length; i++)
        {
            this.enemySpawnZones[i].GetComponent<EnemySpawnZoneController>().worldController = this;
        }
        
        this.resetActive(false, 5);
    }

    void Update()
    {
        if (gameActive)
        {
            this.powerUpTime -= Time.deltaTime;

            if (this.powerUpTime <= 0)
            {
                var finalPosition = new Vector3(Random.Range(-3f, 3f), Random.Range(0.5f, 5f), 10f);
                var powerUp = Instantiate((Random.value >= 0.5 ? this.shootPowerUpPrefab : this.cooldownPowerUpPrefab), this.cameraTransform.position, this.cameraTransform.rotation, this.cameraTransform);
                powerUp.GetComponent<PowerUpController>().destinationY = finalPosition.y;
                powerUp.transform.localPosition = new Vector3(finalPosition.x, 7f, finalPosition.z);
                this.powerUpTime = POWER_UP_MAX_TIME;
            }

            if (this.needsMoreEnemies > 0)
            {
                this.addMoreEnemiesTime -= Time.deltaTime;

                if (this.addMoreEnemiesTime <= 0)
                {
                    if (this.needsMoreEnemies == 2)
                    {
                        this.resetSpawnZonesActivate(true, 3);
                    }
                    else
                    {
                        this.resetSpawnZonesActivate(true, 5);
                    }

                    this.needsMoreEnemies -= 1;
                    this.addMoreEnemiesTime = NEEDS_MORE_ENEMIES_TIME;
                }
            }
        }
    }

    void resetActive(bool active, int countForSpawnZonesActivation)
    {
        this.gameActive = active;
        this.resetSpawnZonesActivate(active, countForSpawnZonesActivation);
    }

    void resetSpawnZonesActivate(bool active, int countForActive)
    {
        if (!active || countForActive == this.enemySpawnZones.Length)
        {
            for (int i = 0; i < this.enemySpawnZones.Length; i++)
            {
                this.enemySpawnZones[i].SetActive(active);
            } 
        }
        else
        {
            switch (countForActive)
            {
                case 1:
                    this.enemySpawnZones[2].SetActive(active);
                    break;
                case 2: 
                case 3:
                    this.enemySpawnZones[1].SetActive(active);
                    this.enemySpawnZones[2].SetActive(active);
                    this.enemySpawnZones[3].SetActive(active);
                    break;
                case 4: 
                case 5:
                    this.enemySpawnZones[0].SetActive(active);
                    this.enemySpawnZones[1].SetActive(active);
                    this.enemySpawnZones[2].SetActive(active);
                    this.enemySpawnZones[3].SetActive(active);
                    this.enemySpawnZones[4].SetActive(active);
                    break;
            }
        }
    }

    public void talismanCaptured()
    {
        this.mainAudio.clip = this.stage2Clip;
        this.mainAudio.Play();
        this.resetActive(true, 1);
        this.cloudsSpawnerController.shouldSpawnClouds = true;
        this.cameraController.shouldScroll = true;
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
        if (!this.gameFinished)
        {
            this.gameFinished = true;

            this.resetActive(false, 5);

            this.mainAudio.Stop();
            this.oneShotAudio.clip = this.youLostClip;
            this.oneShotAudio.Play();
            this.gameOverCanvas.gameObject.SetActive(true);
        }
    }

    public void playerWon()
    {
        if (!this.gameFinished)
        {
            this.gameFinished = true;

            this.resetActive(false, 5);
            
            this.mainAudio.Stop();
            this.oneShotAudio.clip = this.youWonClip;
            this.oneShotAudio.Play();
            this.youWonCanvas.gameObject.SetActive(true);
        }
    }

    public void increaseAttachedEnemies(GameObject enemy)
    {
        enemy.transform.parent = this.finalBoss.transform;

        this.attachedEnemies += 1;

        if (this.attachedEnemies >= MAX_ATTACHED_ENEMIES)
        {
            this.mainAudio.clip = this.stage3Clip;
            this.mainAudio.Play();

            this.finalTalisman = this.ship.talisman.GetComponent<TalismanController>();
            this.finalTalisman.transform.parent = this.finalBoss.transform;
            this.ship.destroyShip();
            this.resetActive(false, 5);

            var powerUps = Object.FindObjectsOfType<PowerUpController>();
            for (int i = 0; i < powerUps.Length; i++)
            {
                powerUps[i].destroy(false);
            }

            this.finalBoss.fightStarted(this.finalTalisman);
            this.player.finalBattleStarted();
        }
    }
}
