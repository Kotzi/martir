using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController: MonoBehaviour
{
    const int MAX_COUNTDOWN = 15;
    const float DISCONNECTION_TIMER = 2f;
    const float DISCONNECTION_SPEED = 30f;
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
    private Animator animator;
    private HingeJoint2D chainConnector;
    private Camera mainCamera;
    private AudioSource mainAudio;
    private float horizontal = 0f;
    private float vertical = 0f;
    private float moveLimiter = 0.7f;
    private float shootCooldown = 0f;
    private bool shoot = false;
    private bool connect = false;
    private Vector2 speed = Vector2.zero;
    private float cameraWidth;
    private float halfSpriteWidth;
    private float cameraHeight;
    private float halfSpriteHeight;
    private int maxShoots = 1;
    private int countdown = MAX_COUNTDOWN;
    private float countdownTimer = 1f;
    private int lives = MAX_LIVES;
    private float isDesconnectingTimer = 2f;
    private bool facingRight = true;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.mainAudio = this.GetComponent<AudioSource>();
        this.chainConnector = this.GetComponent<HingeJoint2D>();
        this.chainConnector.enabled = false;
        this.chainConnector.connectedBody = this.ship.lastChainJoint;
        this.mainCamera = Camera.main;

        var spriteSize = this.GetComponent<SpriteRenderer>().bounds.size;
        this.halfSpriteWidth = spriteSize.x * .5f;
        this.halfSpriteHeight = spriteSize.y * .5f;

        this.cameraHeight = this.mainCamera.orthographicSize;
        this.cameraWidth = this.mainCamera.orthographicSize * this.mainCamera.aspect;
    }

    void Update()
    {
        this.horizontal = ( this.mobileGameUICanvasController ? this.mobileGameUICanvasController.joystick.Horizontal : Input.GetAxisRaw("Horizontal"));
        this.vertical = ( this.mobileGameUICanvasController ? this.mobileGameUICanvasController.joystick.Vertical : Input.GetAxisRaw("Vertical"));
        this.shoot = ( this.mobileGameUICanvasController ? this.mobileGameUICanvasController.shootButton.isPressed : Input.GetButton("Shoot"));
        this.connect = Input.GetButton("Connect");
    
        if (this.isDesconnectingTimer > 0)
        {
            this.isDesconnectingTimer -= Time.deltaTime;
        }
        else if (!this.isConnected())
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

        if(this.connect && !this.finalBattle)
        {
            if(!this.isConnected())
            {
                if(Vector2.Distance(this.chainConnector.transform.position, this.ship.lastChainJoint.transform.position) < 1f) 
                {
                    this.setIsConnected(true);
                }
            }
            else
            {
                this.setIsConnected(false);
                this.isDesconnectingTimer = DISCONNECTION_TIMER;
                this.rb.velocity += Vector2.up * DISCONNECTION_SPEED;
            }
        }
    }

    void LateUpdate()
    {
        var yMin = this.mainCamera.transform.position.y - this.cameraHeight + this.halfSpriteHeight; // lower bound
        var yMax = this.mainCamera.transform.position.y; // upper bound
        
        if(this.isConnected())
        {
            yMax -= this.cameraHeight * 0.4f;
        } 
        else 
        {
            yMax += this.cameraHeight - this.halfSpriteHeight;
        }
         
        var xMin = -this.cameraWidth + this.halfSpriteWidth; // left bound
        var xMax = this.cameraWidth - this.halfSpriteWidth; // right bound 

        var xValidPosition = Mathf.Clamp(transform.position.x, xMin, xMax);
        var yValidPosition = Mathf.Clamp(transform.position.y, yMin, yMax);
 
        this.transform.position = new Vector3(xValidPosition, yValidPosition, 10f);
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        if (enemy)
        {
            this.lives -= 1;
            this.worldController.updateLives(this.lives);
            enemy.destroyByPlayerImpact();

            if (this.lives <= 0)
            {
                this.playerDied(false);
            }
        }
        else if(!this.chainConnector.enabled && this.isDesconnectingTimer <= 0)
        {
            ShipController ship = collision.collider.GetComponentInParent<ShipController>();
            if(ship != null) 
            {
                this.setIsConnected(true);
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

    void setIsConnected(bool isConnected)
    {
        if (isConnected)
        {
            this.ship.lastChainJoint.velocity = (this.ship.lastChainJoint.position - (Vector2)this.chainConnector.transform.position);
            this.mainAudio.clip = this.chainConnectedAudio;
        }
        else
        {
            this.mainAudio.clip = this.chainDisconnectedAudio;
        }

        this.mainAudio.Play();

        this.chainConnector.enabled = isConnected;
        this.countdown = isConnected ? -1 : MAX_COUNTDOWN;
        this.worldController.updateCountdown(this.countdown);
    }
    
    public bool isConnected()
    {
        if (this.finalBattle)
        {
            return false;
        }
        else if (this.chainConnector) 
        {
            return this.chainConnector.enabled;
        }
        else
        {
            return false;
        }
    }

    public void finalBattleStarted()
    {
        this.finalBattle = true;
        this.setIsConnected(false);
    }

    void playerDied(bool runOutOfTime)
    {
        this.worldController.playerDied(runOutOfTime);
        this.transform.DOScale(this.transform.localScale * 0.5f, 0.15f).OnComplete(() => {
            Destroy(this.gameObject);
        });
    }
}
