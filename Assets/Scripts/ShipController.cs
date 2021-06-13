using UnityEngine;
using DG.Tweening;

public class ShipController: MonoBehaviour
{
    public PlayerController player;
    public GameObject talisman;
    public SpriteRenderer lightsSpriteRenderer;
    private bool isDestroying = false;

    public void talismanPickedUp()
    {
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