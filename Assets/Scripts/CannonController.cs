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

    public void fire(float baseSpeed, float multiplier)
    {
        ShotController shot = Instantiate(this.shot, transform.position, transform.rotation, this.worldController.transform.parent).GetComponent<ShotController>();
        shot.transform.localScale *= multiplier;
        shot.fire(baseSpeed, this.damage * multiplier);
    }
}