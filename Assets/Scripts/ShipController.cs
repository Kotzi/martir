using UnityEngine;
using DG.Tweening;

public class ShipController: MonoBehaviour
{
    const float JOINT_VELOCITY = 5f;
    public PlayerController player;
    public GameObject talisman;
    public SpriteRenderer lightsSpriteRenderer;
    public Rigidbody2D lastChainJoint;

    void Update()
    {
        if (!this.player.isConnected())
        {
            this.lastChainJoint.velocity = new Vector2(Random.Range(-1.5f, 1.5f), 1f) * JOINT_VELOCITY;
        }
        else
        {
            this.lastChainJoint.velocity = Vector2.zero;
        }
    }

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
