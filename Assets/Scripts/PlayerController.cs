using UnityEngine;

public class PlayerController: MonoBehaviour
{
    public CannonController cannon;
    public float runSpeed = 20.0f;
    public float shotCooldownTime = 0.2f;

    private Rigidbody2D rb;
    private float horizontal = 0f;
    private float vertical = 0f;
    private float moveLimiter = 0.7f;
    private float shootCooldown = 0f;
    private float shootPower = 4f;
    private bool shoot = false;

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

        this.rb.velocity = new Vector2(this.horizontal * this.runSpeed, this.vertical * this.runSpeed);

        this.shootCooldown -= Time.deltaTime;
        if(this.shootCooldown <= 0 && this.shoot)
        {
            Vector2 direction = Vector2Utils.Vector2FromAngle(this.rb.rotation);
            this.cannon.fire(direction, this.rb.velocity);
            //Rigidbody.AddForce(BaseRecoil * Mathf.Exp(this.shootPower) * direction);

            this.shake();

            this.shootCooldown = this.shotCooldownTime;
        }
    }

    void shake()
    {

    }
}
