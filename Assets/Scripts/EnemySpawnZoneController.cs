using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnZoneController: MonoBehaviour
{  
    public GameObject enemyPrefab;
    public GameObject smallEnemyPrefab;
    private float enemySpawnCooldown = 2f;
    private float maxEnemySpawnCooldown = 2f;
    
    void Update()
    {
        this.enemySpawnCooldown -= Time.deltaTime;

        if(this.enemySpawnCooldown <= 0f)
        {
            var prefab = Random.value >= 0.5f ? this.smallEnemyPrefab : this.enemyPrefab;
            Instantiate(prefab, this.transform.position, this.transform.rotation, this.transform.parent);
            this.enemySpawnCooldown = this.maxEnemySpawnCooldown;
        }
    }
}
