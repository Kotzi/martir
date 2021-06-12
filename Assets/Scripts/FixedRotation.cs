using UnityEngine;

public class FixedRotation: MonoBehaviour
{
    private Quaternion fixedRotation;

    void Awake()
    {
        this.fixedRotation = this.transform.rotation;
    }

    void LateUpdate()
    {
        this.transform.rotation = this.fixedRotation;
    }
}
