using UnityEngine;

public class BasicEnemyAttack : EnemySkill
{
    [SerializeField] float damage;
    [SerializeField] float timer;
    public override void Use()
    {
        EnemyManager.Instance.playerHealth.TakeDamage(damage);
    }
}
