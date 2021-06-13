using UnityEngine;
using DG.Tweening;

public class TalismanController: MonoBehaviour
{
    public bool canBePickedUp = true;
    public bool isFinalBattle = false;

    void Start()
    {
        DOTween.Sequence()
                .Append(this.transform.DOPunchScale(Vector3.one * 0.05f, 0.75f))
                .AppendInterval(0.5f)
                .SetLoops(-1);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (this.canBePickedUp)
        {
            var player = collider.GetComponent<PlayerController>();
            if (player != null) 
            {
                this.transform.DOLocalMove(player.ship.transform.position, 1f).OnComplete(() => {
                    player.talismanPickedUp();
                    Destroy(this.gameObject);
                });
            }
        }
        else if (this.isFinalBattle)
        {
            var player = collider.GetComponent<PlayerController>();
            if (player != null) 
            {
                player.worldController.playerWon();
            }
        }
    }

}
