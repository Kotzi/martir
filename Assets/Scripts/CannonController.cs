using UnityEngine;
//using DG.Tweening;

public class CannonController : MonoBehaviour
{
    public GameObject shot;
    public float damage = 10f;
    
    private WorldController worldController;

    void Start()
    {
        this.worldController = Object.FindObjectOfType<WorldController>();
    }

    public void fire(int count, float baseSpeed, float multiplier)
    {
        for (int i = 0; i < count; i++)
        {
            ShotController shot = Instantiate(this.shot, new Vector3(this.transform.position.x + 0.7f * this.index(i, count), this.transform.position.y, this.transform.position.z), transform.rotation, this.worldController.transform.parent).GetComponent<ShotController>();
            shot.transform.localScale *= multiplier;
            shot.fire(Vector2.up, baseSpeed, this.damage * multiplier);
        }
    }

    private float index(int i, int count) 
    {
        switch (count)
        {
            case 1: return 0;
            case 2:
                switch (i)
                {
                    case 0: return -0.5f;
                    case 1: return 0.5f;
                    default: return 0f;
                }
            case 3:
                switch (i)
                {
                    case 0: return -0.5f;
                    case 1: return 0f;
                    case 2: return 0.5f;
                    default: return 0f;
                }
            case 4:
                switch (i)
                {
                    case 0: return -1f;
                    case 1: return -0.5f;
                    case 2: return 0.5f;
                    case 3: return 1f;
                    default: return 0f;
                }
            default: return 0;
        }
    }
}