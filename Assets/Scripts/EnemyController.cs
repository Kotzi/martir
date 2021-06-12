using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;

public class EnemyController: MonoBehaviour, IDamageable
{
    public float health = 10f;
    public float shootPower = 0.5f;
    public float maxShootCooldown = 1.5f;
    public string animationName = "FlyEnemy";

    public GameObject shot;
    public GameObject lookAhead;

    private ShipController ship;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float shotCooldown = 1.5f;
    private bool isActive = true;

    void Start() 
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.sr = GetComponent<SpriteRenderer>();
        this.GetComponent<Animator>().Play(this.animationName);
        this.ship = Object.FindObjectOfType<ShipController>();
    }

    void FixedUpdate()
    {
        if(this.isActive)
        {
            if(Mathf.Abs(this.ship.transform.position.y - this.transform.position.y) > 1f) 
            {
                this.transform.position -= new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(5.5f, 2.5f), 0) * Time.deltaTime;
            }
            else
            {
                this.transform.position = this.ship.transform.position;
                this.isActive = false;
            }
        }
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
