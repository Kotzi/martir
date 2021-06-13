using UnityEngine;
//using DG.Tweening;

public class CannonController : MonoBehaviour
{
    public GameObject shot;
    public GameObject bigShot;
    public AudioClip sfx1;
    public AudioClip sfx2;
    public AudioClip sfx3;
    public float damage = 10f;
    
    private WorldController worldController;
    private AudioSource audioSource;
    private int lastSound = 0;

    void Start()
    {
        this.worldController = Object.FindObjectOfType<WorldController>();
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void fire(int count, float baseSpeed, float multiplier)
    {
        this.playSound();
        
        for (int i = 0; i < count; i++)
        {
            var prefab = multiplier == 1 ? this.shot : this.bigShot;
            ShotController shot = Instantiate(prefab, new Vector3(this.transform.position.x + 0.7f * this.index(i, count), this.transform.position.y, this.transform.position.z), transform.rotation, this.worldController.transform.parent).GetComponent<ShotController>();
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

    private void playSound()
    {
        switch (this.lastSound)
        {
            case 0:
                this.audioSource.clip = this.sfx1;
                break;
            case 1:
                this.audioSource.clip = this.sfx2;
                break;
            case 2:
                this.audioSource.clip = this.sfx3;
                break;
        }

        this.audioSource.Play();

        this.lastSound += 1;

        if (this.lastSound > 2)
        {
            this.lastSound = 0;
        }
    }
}