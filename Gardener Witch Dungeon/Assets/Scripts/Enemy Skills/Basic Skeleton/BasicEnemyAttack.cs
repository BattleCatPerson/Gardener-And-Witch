using UnityEngine;

public class BasicEnemyAttack : EnemySkill
{
    [SerializeField] float damage;
    public override void Use()
    {
        EnemyManager.Instance.playerHealth.TakeDamage(damage);
    }
}
