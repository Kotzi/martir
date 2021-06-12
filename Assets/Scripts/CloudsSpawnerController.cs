using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsSpawnerController: MonoBehaviour
{
    public GameObject cloudPrefab;
    public GameObject cloudLayer;

    private float cloudSpawnCooldown = 0f;
    private float maxCloudSpawnCooldown = 3f;
    
    void Update()
    {
        this.cloudSpawnCooldown -= Time.deltaTime;

        if(this.cloudSpawnCooldown <= 0f)
        {
            Instantiate(this.cloudPrefab, this.transform.position, this.transform.rotation, this.cloudLayer.transform);
            this.cloudSpawnCooldown = this.maxCloudSpawnCooldown + Random.Range(-0.5f, 0.5f);
        }
    }
}
