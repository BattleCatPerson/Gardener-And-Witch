using UnityEngine;

public abstract class EnemySkill : MonoBehaviour
{
    public float skillCooldown;
    public string skillName;
    public float distanceFromPlayer;
    public abstract void Use();
}
