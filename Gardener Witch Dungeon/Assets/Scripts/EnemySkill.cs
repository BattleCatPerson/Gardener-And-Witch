using UnityEngine;

public abstract class EnemySkill : MonoBehaviour
{
    public float skillCooldown;
    public abstract void Use();
}
