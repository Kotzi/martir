using UnityEngine;
using DG.Tweening;

public class PowerUpController: MonoBehaviour
{
    public bool cooldown;
    public bool shoots;

    private float lifetime;

    void Awake()
    {
        this.lifetime = Random.Range(2f, 4f);
        this.transform.localScale *= 0.5f;
    }

    void Start()
    {
        this.transform.DOScale(Vector3.one, 0.1f);
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
        var player = collider.GetComponent<PlayerController>();
        if (player != null) 
        {
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

    void destroy(bool pickedUp)
    {
        this.transform
                .DOScale(this.transform.localScale * (pickedUp ? 1.25f : 0.25f), 0.1f)
                .OnComplete(() => {
                    Destroy(this.gameObject);
                });
    }
}
