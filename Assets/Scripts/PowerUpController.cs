using UnityEngine;
using DG.Tweening;

public class PowerUpController: MonoBehaviour
{
    public bool cooldown;
    public bool shoots;
    public float destinationY;

    private float lifetime;
    private Vector3 originalScale;
    private bool isDestroying = false;

    void Awake()
    {
        this.lifetime = Random.Range(5f, 8f);
        this.originalScale = this.transform.localScale;
        this.transform.localScale = this.originalScale * 0.5f;
    }

    void Start()
    {
        DOTween.Sequence()
                .Append(this.transform.DOScale(this.originalScale, 0.1f))
                .Append(this.transform.DOLocalMoveY(destinationY, 0.1f));
    }

    void Update()
    {
        this.lifetime -= Time.deltaTime;
        if (this.lifetime <= 0)
        {
            this.destroy(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!this.isDestroying)
        {
            var player = collider.GetComponent<PlayerController>();
            if (player != null) 
            {
                this.GetComponent<AudioSource>().Play();

                if (cooldown) 
                {
                    player.reduceShootCooldown();
                }

                if (shoots) 
                {
                    player.addMoreShoots();
                }

                this.destroy(true);
            }
        }
    }

    public void destroy(bool pickedUp)
    {
        if (!this.isDestroying) 
        {
            this.isDestroying = true;

            this.transform
                .DOScale(this.transform.localScale * (pickedUp ? 1.25f : 0.25f), 0.1f)
                .OnComplete(() => {
                    if (pickedUp)
                    {
                        Destroy(this.GetComponent<SpriteRenderer>());
                        Destroy(this.gameObject, 1.05f);
                    }
                    else
                    {
                        Destroy(this.gameObject);
                    }
                });
        }
    }
}
