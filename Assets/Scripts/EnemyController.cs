using UnityEngine;
using DG.Tweening;

public class EnemyController: MonoBehaviour, IDamageable
{
    public float health = 10f;
    public string animationName = "FlyEnemy";
    public bool isActive = true;

    private ShipController ship;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

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
        DOTween.Sequence()
                .Append(this.sr.DOFade(0.65f, 0.15f))
                .Append(this.sr.DOFade(1f, 0.05f));
        
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

    public void destroyByPlayerImpact()
    {
        this.destroyed();
    }

    private void destroyed()
    {
        Destroy(gameObject);
    }
}
