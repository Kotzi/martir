using UnityEngine;
using DG.Tweening;

public class FinalBossController : MonoBehaviour
{
    const float MAX_ATTACK_COOLDOWN = 3f;

    public PlayerController player;

    private bool shouldAttack = false;
    private float attackCooldown = 1f;

    public void fightStarted()
    {
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
                for (int i = 0; i < Mathf.Min(2, enemies.Length); i++)
                {
                    enemies[i].transform.parent = container.transform;    
                }
                container.transform.DORotate(new Vector3(0f, 0f, 359f), 1f).SetLoops(-1);
                var direction = container.transform.position - this.player.transform.position;
                container.transform.DOMove(direction * 1.5f, 1f);
        
                this.attackCooldown = MAX_ATTACK_COOLDOWN + Random.Range(-1f, 1f);
            }
        }
    }
}
