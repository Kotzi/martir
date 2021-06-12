using UnityEngine;
using DG.Tweening;

public class ShipController: MonoBehaviour
{
    public GameObject talisman;
    public SpriteRenderer lightsSpriteRenderer;
    public Rigidbody2D lastChainJoint;

    public void talismanPickedUp()
    {
        this.talisman.SetActive(true);
        this.lightsSpriteRenderer.gameObject.SetActive(true);

        DOTween.Sequence()
            .Append(this.lightsSpriteRenderer.DOFade(0.7f, 0.15f))
            .Append(this.lightsSpriteRenderer.DOFade(1f, 0.1f))
            .SetLoops(-1);
    }
}
