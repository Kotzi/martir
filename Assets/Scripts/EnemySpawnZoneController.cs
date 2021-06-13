using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnZoneController: MonoBehaviour
{  
    public GameObject enemyPrefab;
    public GameObject smallEnemyPrefab;
    public WorldController worldController;

    private float enemySpawnCooldown = 2f;
    private float maxEnemySpawnCooldown = 2f;
    
    void Update()
    {
        this.enemySpawnCooldown -= Time.deltaTime;

        if(this.enemySpawnCooldown <= 0f)
        {
            var prefab = Random.value >= 0.5f ? this.smallEnemyPrefab : this.enemyPrefab;
            var enemy = Instantiate(prefab, this.transform.position, this.transform.rotation, this.transform.parent).GetComponent<EnemyController>();
            enemy.worldController = this.worldController;
            this.enemySpawnCooldown = this.maxEnemySpawnCooldown + Random.Range(-1f, 1f);
        }
    }
}
