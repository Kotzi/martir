using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    const float POWER_UP_MAX_TIME = 5f;
    public GameObject shootPowerUpPrefab;
    public GameObject cooldownPowerUpPrefab;
    public Transform cameraTransform;

    private float powerUpTime = 5f;

    void Update()
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
