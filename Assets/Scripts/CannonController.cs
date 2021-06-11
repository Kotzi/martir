using UnityEngine;
//using DG.Tweening;

public class CannonController : MonoBehaviour
{
    public GameObject shot;
    public float damage = 10f;
    
    public void fire(Vector2 direction, Vector2 baseVelocity)
    {
        print("cannon FIRE");
        ShotController shot = Instantiate(this.shot, transform.position, transform.rotation, transform.parent).GetComponent<ShotController>();
        //shot.WorldController = WorldController;
        shot.fire(direction, baseVelocity, this.damage);
    }
}