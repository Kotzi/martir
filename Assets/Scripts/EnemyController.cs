using UnityEngine;
using DG.Tweening;

public class EnemyController: MonoBehaviour, IDamageable
{
    public WorldController worldController;
    public float health = 10f;
    public string animationName = "FlyEnemy";
    public bool isActive = true;

    private ShipController ship;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource mainAudio;
    private bool isAlive = true;
    private Vector3 lastShipPosition = Vector3.zero;

    void Start() 
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.sr = this.GetComponent<SpriteRenderer>();
        this.mainAudio = this.GetComponent<AudioSource>();
        this.GetComponent<Animator>().Play(this.animationName);
        this.ship = Object.FindObjectOfType<ShipController>();
    }

    void FixedUpdate()
    {
        if(this.isActive && this.isAlive)
        {
            if (this.ship)
            {
                this.lastShipPosition = this.ship.transform.position;
            }

            if(this.ship && Mathf.Abs(this.ship.transform.position.y - this.transform.position.y) > 1f) 
            {
                this.transform.position -= new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(5.5f, 2.5f), 0) * Time.deltaTime;
            }
            else
            {
                this.transform.position = this.lastShipPosition;
                this.worldController.increaseAttachedEnemies(this.gameObject);
                this.isActive = false;
            }
        }
    }

    void OnBecameInvisible()
    {
        this.destroyed();
    }

    public bool takeDamage(float damageTaken)
    {
        if (this.isAlive)
        {
            this.mainAudio.Play();
            
            DOTween.Sequence()
                    .Append(this.sr.DOFade(0.65f, 0.15f))
                    .Append(this.sr.DOFade(1f, 0.05f));
            
            this.health -= damageTaken;

            if(this.health <= 0)
            {
                this.destroyed();
                return true;
            }
        }
        
        return false;
    }

    public void destroyByPlayerImpact()
    {
        this.destroyed();
    }

    private void destroyed()
    {
        this.isAlive = false;

        // To avoid pushing them
        Destroy(this.rb);
        
        DOTween.Sequence()
                .Append(this.sr.DOFade(0.25f, 0.15f))
                .Append(this.transform.DOScale(this.transform.localScale * 0.25f, 0.3f))
                .OnComplete(() => {
                    Destroy(gameObject);
                });
    }
}
