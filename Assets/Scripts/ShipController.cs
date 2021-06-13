using UnityEngine;
using DG.Tweening;

public class ShipController: MonoBehaviour
{
    const float MAX_TIMER = 1.1f;
    public PlayerController player;
    public GameObject talisman;
    public SpriteRenderer lightsSpriteRenderer;
    public bool isMoving = false;

    private bool isDestroying = false;
    private float animationTimer = 0f;

    void Update()
    {
        if (this.isMoving)
        {
            this.animationTimer -= Time.deltaTime;

            if(animationTimer <= 0)
            {
                this.transform.DOPunchPosition(new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), 1f);
                this.animationTimer = MAX_TIMER;
            }
        }
    }

    public void talismanPickedUp()
    {
        this.isMoving = true;

        this.talisman.SetActive(true);
        this.lightsSpriteRenderer.gameObject.SetActive(true);

        DOTween.Sequence()
            .Append(this.lightsSpriteRenderer.DOFade(0.7f, 0.15f))
            .Append(this.lightsSpriteRenderer.DOFade(1f, 0.1f))
            .SetLoops(-1);
    }

    public void destroyShip()
    {
        if (!this.isDestroying)
        {
            this.isDestroying = true;
            Destroy(this.gameObject);
        }
    }
}