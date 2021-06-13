using UnityEngine;

public class CameraController: MonoBehaviour
{
    const float VELOCITY = 4f;
    public bool shouldScroll = false;
    void LateUpdate()
    {
        if (this.shouldScroll)
        {
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + VELOCITY * Time.deltaTime);
        }
    }
}
