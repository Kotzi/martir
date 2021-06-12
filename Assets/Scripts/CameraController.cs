using UnityEngine;

public class CameraController: MonoBehaviour
{
    const float VELOCITY = 0.75f;
    void LateUpdate()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + VELOCITY * Time.deltaTime);       
    }
}