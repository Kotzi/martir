using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;

public class EnemyController: MonoBehaviour, IDamageable
{
    public float health = 10f;
    public float shootPower = 0.5f;
    public float maxShootCooldown = 1.5f;

    public GameObject shot;
    public GameObject lookAhead;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float shotCooldown = 1.5f;

    void Start() 
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        this.transform.position -= new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f), 0) * Time.deltaTime;
        this.transform.rotation = Quaternion.identity;
    }

    public bool takeDamage(float damageTaken)
    {
        this.health -= damageTaken;

        if(this.health <= 0)
        {
            this.destroyed();
            return true;
        } 
        else 
        {
            return false;
        }
    }

    public bool isEnemy()
    {
        return true;
    }

    private void destroyed()
    {
        Destroy(gameObject);
    }
}
