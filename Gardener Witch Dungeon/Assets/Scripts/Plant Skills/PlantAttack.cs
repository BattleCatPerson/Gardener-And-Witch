using UnityEngine;
using UnityEngine.InputSystem;

public class PlantAttack : PlantSkill
{
    [SerializeField] int damage;
    public override void Use()
    {
        Debug.Log("ouch!");
        SkillHolder.Instance.targetedEnemy.TakeDamage(damage);
    }
}
