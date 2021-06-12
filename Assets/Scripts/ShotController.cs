using UnityEngine;
//using DG.Tweening;

public class ShotController: MonoBehaviour
{
    private const float destructionDuration = 1f;

    public float speed = 20f;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private float hitPower = 0f;

    void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.sp = GetComponent<SpriteRenderer>();
    }

    public void fire(float baseVelocity, float hitPower) 
    {
        this.transform.eulerAngles = new Vector3(0f, 0f, 90f);

        this.hitPower = hitPower;

        this.rb.angularVelocity = 0f;
        this.rb.velocity = Vector2.up * (this.speed + baseVelocity);
    }
    
    void OnCollisionEnter2D(Collision2D collision) 
    {
        IDamageable damageable = collision.collider.gameObject.GetComponent<IDamageable>();
        if(damageable != null) 
        {
            bool killed = damageable.takeDamage(this.hitPower);
        }

        this.destroyed(true);
    }

    void OnBecameInvisible()
    {
        this.destroyed(false);
    }

    void destroyed(bool animated)
    {
        if(!animated) {
            Destroy(gameObject);
            return;
        }

        this.rb.velocity = Vector2.zero;
        // SpriteRenderer.enabled = false;
        // Light.enabled = false;
        // Sequence sequence = DOTween.Sequence();
        // foreach (var explosion in ExplosionsSpriteRenderers)
        // {   
        //     explosion.enabled = true;
        //     Transform explosionTransform = explosion.transform;
        //     Vector3 newScale = explosionTransform.localScale * 0.25f;
        //     Vector3 newPosition = explosionTransform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
        //     Color newColor = explosion.color;
        //     newColor.a = 0f;
        //     sequence.Join(explosion.DOColor(newColor, DestructionDuration));
        //     sequence.Join(explosionTransform.DOScale(newScale, DestructionDuration));
        //     sequence.Join(explosionTransform.DOMove(newPosition, DestructionDuration));
        // }
        // sequence.Play().OnComplete(() => {
            Destroy(gameObject);
        // });
    }
}
