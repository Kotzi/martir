using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController: MonoBehaviour
{
    const int MAX_COUNTDOWN = 15;
    const int MAX_LIVES = 5;
    const int MAX_SHOOTS = 4;
    const float MAX_SPEED = 5f;
    const float MIN_SHOOT_COOLDOWN = 0.01f;
    const float ACCELERATION = 7.5f;

    public GameUICanvasController mobileGameUICanvasController;
    public WorldController worldController;
    public CannonController cannon;
    public ShipController ship;
    public AudioClip chainConnectedAudio;
    public AudioClip chainDisconnectedAudio;
    public float shotCooldownTime = 0.05f;
    public bool finalBattle = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private Camera mainCamera;
    private AudioSource mainAudio;
    private float horizontal = 0f;
    private float vertical = 0f;
    private float moveLimiter = 0.7f;
    private float shootCooldown = 0f;
    private bool shoot = false;
    private Vector2 speed = Vector2.zero;
    private float cameraWidth;
    private float halfSpriteWidth;
    private float cameraHeight;
    private float halfSpriteHeight;
    private int maxShoots = 1;
    private int countdown = MAX_COUNTDOWN;
    private float countdownTimer = 1f;
    private int lives = MAX_LIVES;
    private bool facingRight = true;
    private bool isInSafeArea = true;
    private bool firstConnection = true;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.sr = this.GetComponent<SpriteRenderer>();
        this.animator = this.GetComponent<Animator>();
        this.mainAudio = this.GetComponent<AudioSource>();
        this.mainCamera = Camera.main;

        var spriteSize = this.GetComponent<SpriteRenderer>().bounds.size;
        this.halfSpriteWidth = spriteSize.x * .5f;
        this.halfSpriteHeight = spriteSize.y * .5f;

        this.cameraHeight = this.mainCamera.orthographicSize;
        this.cameraWidth = this.mainCamera.orthographicSize * this.mainCamera.aspect;
    }

    void Update()
    {
        if (this.mobileGameUICanvasController)
        {
            this.horizontal = this.mobileGameUICanvasController.joystick.Horizontal;
            this.vertical = this.mobileGameUICanvasController.joystick.Vertical;
            this.shoot = this.mobileGameUICanvasController.shootButton.isPressed;
        }
        else
        {
            this.horizontal = Input.GetAxisRaw("Horizontal");
            this.vertical = Input.GetAxisRaw("Vertical");
            this.shoot = Input.GetButton("Shoot");

        }
    
        if (!this.isConnected())
        {
            this.countdownTimer -= Time.deltaTime;
            if (this.countdownTimer <= 0)
            {
                this.countdown -= 1;
                this.countdownTimer = 1f;
                this.worldController.updateCountdown(this.countdown);

                if (this.countdown <= 0)
                {
                    this.playerDied(true);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (this.horizontal != 0 && this.vertical != 0)
        {
            this.horizontal *= moveLimiter;
            this.vertical *= moveLimiter;
        } 

        if (this.horizontal != 0)
        {
            this.speed.x = Mathf.Clamp(this.speed.x + ACCELERATION * Time.deltaTime, -MAX_SPEED, MAX_SPEED);
        }

        if (this.vertical != 0)
        {
            this.speed.y = Mathf.Clamp(this.speed.y + ACCELERATION * Time.deltaTime, -MAX_SPEED, MAX_SPEED);
        }

        if(this.horizontal > 0 && !this.facingRight) 
        {
            this.flip();
        }
        else if(this.horizontal < 0 && this.facingRight)
        {
            this.flip();
        }

        this.rb.velocity = new Vector2(this.horizontal, this.vertical) * this.speed;

        this.shootCooldown -= Time.deltaTime;
        if(this.shootCooldown <= 0 && this.shoot)
        {
            this.cannon.fire(this.maxShoots, this.speed.y, this.isConnected() ? 2f : 1f);
            this.animator.SetBool("IsAttacking", true);
            this.rb.velocity += Mathf.Exp(this.maxShoots) * Vector2.down;

            this.shake();

            this.shootCooldown = this.shotCooldownTime;
        }
        else
        {
            this.animator.SetBool("IsAttacking", false);
        }
    }

    void LateUpdate()
    {
        var yMin = this.mainCamera.transform.position.y - this.cameraHeight + this.halfSpriteHeight + (this.finalBattle ? 0f : 2f); // lower bound
        var yMax = this.mainCamera.transform.position.y + this.cameraHeight - this.halfSpriteHeight - 1f; // upper bound
                 
        var xMin = -(this.cameraWidth * this.mainCamera.rect.width) + this.halfSpriteWidth; // left bound
        var xMax = (this.cameraWidth * this.mainCamera.rect.width) - this.halfSpriteWidth; // right bound 

        var xValidPosition = Mathf.Clamp(transform.position.x, xMin, xMax);
        var yValidPosition = Mathf.Clamp(transform.position.y, yMin, yMax);
 
        this.transform.position = new Vector3(xValidPosition, yValidPosition, 10f);
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        if (enemy && !enemy.isMovingToBoss)
        {
            this.lives = Mathf.Clamp(this.lives - 1, 0, MAX_LIVES);

            DOTween.Sequence()
                    .Append(this.sr.DOFade(0.65f, 0.15f))
                    .Append(this.sr.DOFade(1f, 0.05f));

            this.worldController.updateLives(this.lives);
            enemy.destroyByPlayerImpact();

            if (this.lives <= 0)
            {
                this.playerDied(false);
            }
        }
    }

    public void addMoreShoots()
    {
        this.maxShoots = Mathf.Clamp(this.maxShoots + 1, 0, MAX_SHOOTS);
    }

    public void reduceShootCooldown()
    {
        this.shotCooldownTime = Mathf.Clamp(this.shotCooldownTime - 0.01f, MIN_SHOOT_COOLDOWN, 1f);
    }

    public void talismanPickedUp()
    {
        this.ship.talismanPickedUp();
        this.worldController.talismanCaptured();
    }

    void shake()
    {

    }

    void flip()
    {
        this.facingRight = !this.facingRight;
        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
    }

    public void setIsConnected(bool isConnected)
    {
        this.isInSafeArea = isConnected;

        if (this.firstConnection)
        {
            this.firstConnection = false;
        }
        else
        {
            if (isConnected)
            {
                this.mainAudio.clip = this.chainConnectedAudio;
            }
            else
            {
                this.mainAudio.clip = this.chainDisconnectedAudio;
            }

            this.mainAudio.Play();
        }

        this.countdown = isConnected ? -1 : MAX_COUNTDOWN;
        this.worldController.updateCountdown(this.countdown);
    }
    
    public bool isConnected()
    {
        if (this.finalBattle)
        {
            return false;
        }
        else
        {
            return this.isInSafeArea;
        }
    }

    public void finalBattleStarted()
    {
        this.finalBattle = true;
        this.isInSafeArea = false;
        this.countdown = MAX_COUNTDOWN * 2;
        this.worldController.updateCountdown(this.countdown);
    }

    void playerDied(bool runOutOfTime)
    {
        this.worldController.playerDied(runOutOfTime);
        this.transform.DOScale(this.transform.localScale * 0.5f, 0.15f).OnComplete(() => {
            Destroy(this.gameObject);
        });
    }
}
