using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudLayerController : MonoBehaviour
{
    const float VELOCITY = 2f;
    void LateUpdate()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - VELOCITY * Time.deltaTime);       
    }
}
