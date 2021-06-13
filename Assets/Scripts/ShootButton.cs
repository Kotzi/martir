using UnityEngine;

public class ShootButton: MonoBehaviour
{
    const float MAX_TIMEOUT = 0.2f;
    public bool isPressed = false;
    private float isPressedTimeout = MAX_TIMEOUT;

    void Update()
    {
        if (this.isPressed)
        {
            this.isPressedTimeout -= Time.deltaTime;
            if (this.isPressedTimeout <= 0f)
            {
                this.isPressed = false;
            }
        }
    }

    public void onButtonClicked()
    {
        this.isPressed = true;
        this.isPressedTimeout = MAX_TIMEOUT;
    }
}