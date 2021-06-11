using UnityEngine;

public class FixedRotation: MonoBehaviour
{
    private Quaternion fixedRotation;

    void Start()
    {
        this.fixedRotation = this.transform.rotation;
    }

    void FixedUpdate()
    {
        this.transform.rotation = this.fixedRotation;
    }
}
