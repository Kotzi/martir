using UnityEngine;
using DG.Tweening;

public class ShipController: MonoBehaviour
{
    public SpriteRenderer lightsSpriteRenderer;

    void Start()
    {
        DOTween.Sequence()
            .Append(this.lightsSpriteRenderer.DOFade(0.7f, 0.15f))
            .Append(this.lightsSpriteRenderer.DOFade(1f, 0.1f))
            .SetLoops(-1);
    }
}
