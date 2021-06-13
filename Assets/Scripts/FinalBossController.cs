using UnityEngine;
using DG.Tweening;

public class FinalBossController : MonoBehaviour
{
    const float MAX_ATTACK_COOLDOWN = 3f;

    public PlayerController player;

    private bool shouldAttack = false;
    private float attackCooldown = 1f;

    public void fightStarted(TalismanController finalTalisman)
    {
        var enemies = this.GetComponentsInChildren<EnemyController>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].health *= 2f;
            enemies[i].isMovingToBoss = true;
        }

        var originalScale = this.transform.localScale;
        var enemyLayer = LayerMask.NameToLayer("Enemy");
        var playerLayer = LayerMask.NameToLayer("Player");
        var talismanLayer = LayerMask.NameToLayer("Talisman");

        Physics.IgnoreLayerCollision(enemyLayer, playerLayer, true);
        Physics.IgnoreLayerCollision(talismanLayer, playerLayer, true);
        DOTween.Sequence()
            .Join(this.transform.DOLocalMoveY(8f, 1f))
            .Join(this.transform.DOScale(originalScale * 0.5f, 1f))
            .Append(this.transform.DOScale(originalScale, 1f))
            .OnComplete(() => {
                Physics.IgnoreLayerCollision(enemyLayer, playerLayer, false);
                Physics.IgnoreLayerCollision(talismanLayer, playerLayer, false);
                var enemies = this.GetComponentsInChildren<EnemyController>();
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].isMovingToBoss = false;
                }
                finalTalisman.isFinalBattle = true;
                this.shouldAttack = true;
            });
    }

    void Update()
    {
        if (this.shouldAttack && this.player)
        {
            this.attackCooldown -= Time.deltaTime;

            if (this.attackCooldown <= 0)
            {
                var enemies = this.GetComponentsInChildren<EnemyController>();
                var container = new GameObject();
                container.transform.parent = this.transform.parent;
                var firstPosition = Vector3.zero;
                for (int i = 0; i < Mathf.Min(3+Random.Range(0, 3), enemies.Length); i++)
                {
                    enemies[i].transform.parent = container.transform;   
                    if (i == 0)
                    {
                        firstPosition = enemies[i].transform.position;
                    }
                    else
                    {
                        var newPosition = new Vector3(firstPosition.x + 0.5f, firstPosition.y, firstPosition.z);
                        enemies[i].transform.position = newPosition;
                    }
                }
                container.transform.DOMove(this.player.transform.position * -1.5f, 0.5f);
        
                this.attackCooldown = MAX_ATTACK_COOLDOWN + Random.Range(-1f, 1f);
            }
        }
    }
}
