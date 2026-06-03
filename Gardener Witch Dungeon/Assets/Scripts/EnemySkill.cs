using UnityEngine;

public abstract class EnemySkill : MonoBehaviour
{
    public float skillCooldown;
    public string skillName;
    public abstract void Use();
}
