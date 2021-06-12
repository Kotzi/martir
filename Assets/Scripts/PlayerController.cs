using UnityEngine;

public class PlayerController: MonoBehaviour
{
    const float MAX_SPEED = 5f;
    const float ACCELERATION = 7.5f;

    public CannonController cannon;
    public float shotCooldownTime = 0.05f;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private float horizontal = 0f;
    private float vertical = 0f;
    private float moveLimiter = 0.7f;
    private float shootCooldown = 0f;
    private float shootPower = 4f;
    private float baseRecoil = -5f;
    private bool shoot = false;
    private Vector2 speed = Vector2.zero;
    private float cameraWidth;
    private float halfSpriteWidth;
    private float cameraHeight;
    private float halfSpriteHeight;

    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.mainCamera = Camera.main;

        var spriteSize = GetComponent<SpriteRenderer>().bounds.size;
        this.halfSpriteWidth = spriteSize.x * .5f;
        this.halfSpriteHeight = spriteSize.y * .5f;

        this.cameraHeight = this.mainCamera.orthographicSize;
        this.cameraWidth = this.mainCamera.orthographicSize * this.mainCamera.aspect;
    }

    void Update()
    {
        this.horizontal = Input.GetAxisRaw("Horizontal");
        this.vertical = Input.GetAxisRaw("Vertical");
        this.shoot = Input.GetButton("Shoot");
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

        this.rb.velocity = new Vector2(this.horizontal, this.vertical) * this.speed;

        this.shootCooldown -= Time.deltaTime;
        if(this.shootCooldown <= 0 && this.shoot)
        {
            this.cannon.fire(true, this.speed.y);
            //this.rb.AddForce(this.baseRecoil * Mathf.Exp(this.shootPower) * Vector2.down);

            this.shake();

            this.shootCooldown = this.shotCooldownTime;
        }
    }

    void LateUpdate()
    {
        var yMin = this.mainCamera.transform.position.y - this.cameraHeight + this.halfSpriteHeight; // lower bound
        var yMax = this.mainCamera.transform.position.y + this.cameraHeight - this.halfSpriteHeight; // upper bound
         
        var xMin = -this.cameraWidth + this.halfSpriteWidth; // left bound
        var xMax = this.cameraWidth - this.halfSpriteWidth; // right bound 

        var xValidPosition = Mathf.Clamp(transform.position.x, xMin, xMax);
        var yValidPosition = Mathf.Clamp(transform.position.y, yMin, yMax);
 
        this.transform.position = new Vector3(xValidPosition, yValidPosition, 10f);
    }

    void shake()
    {

    }
}
