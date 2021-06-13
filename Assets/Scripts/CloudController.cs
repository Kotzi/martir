using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public Sprite[] sprites;

    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.sprites[Random.Range(0, this.sprites.Length)];
        this.transform.localPosition = new Vector3(this.transform.localPosition.x + Random.Range(-4f, 4f), this.transform.localPosition.y, this.transform.localPosition.z);
        this.transform.localScale = Vector3.one * Random.Range(0.45f, 0.65f);
    }
    
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
