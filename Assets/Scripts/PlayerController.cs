using UnityEngine;

public class PlayerController: MonoBehaviour
{
    const float MAX_SPEED = 5f;
    const float ACCELERATION = 7.5f;

    public CannonController cannon;
    public float shotCooldownTime = 0.2f;

    private Rigidbody2D rb;
    private float horizontal = 0f;
    private float vertical = 0f;
    private float moveLimiter = 0.7f;
    private float shootCooldown = 0f;
    private float shootPower = 4f;
    private bool shoot = false;
    private Vector2 speed = Vector2.zero;

    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        this.horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        this.vertical = Input.GetAxisRaw("Vertical"); // -1 is down
        this.shoot = Input.GetButton("Shoot");
    }

    void FixedUpdate()
    {
        if (this.horizontal != 0 && this.vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
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
            this.cannon.fire(Vector2.up, this.rb.velocity);
            //Rigidbody.AddForce(BaseRecoil * Mathf.Exp(this.shootPower) * direction);

            this.shake();

            this.shootCooldown = this.shotCooldownTime;
        }
    }

    void shake()
    {

    }
}
