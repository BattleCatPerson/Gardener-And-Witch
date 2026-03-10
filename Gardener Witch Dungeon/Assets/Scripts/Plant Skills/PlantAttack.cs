using UnityEngine;
using UnityEngine.InputSystem;

public class PlantAttack : PlantSkill
{
    [SerializeField] float damage;
    public override void Use(float success)
    {
        Debug.Log("ouch!");
        SkillHolder.Instance.targetedEnemy.TakeDamage(damage * success);
    }
}
