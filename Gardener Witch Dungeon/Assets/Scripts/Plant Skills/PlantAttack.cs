using UnityEngine;
using UnityEngine.InputSystem;

public class PlantAttack : PlantSkill
{
    [SerializeField] float damage;
    [SerializeField] float goodDamageMult;
    [SerializeField] float okDamageMult;
    [SerializeField] float badDamageMult;
    public override void Use(SkillResult success)
    {
        Debug.Log("ouch!");
        float mult = 1.0f;
        Debug.Log(success.ToString());
        if (success == SkillResult.Good)
        {
            mult = goodDamageMult;
        }
        else if (success == SkillResult.Ok)
        {
            mult = okDamageMult;
        }
        else if (success == SkillResult.Bad)
        {
            mult = badDamageMult;
        }
        else if (success == SkillResult.Fail)
        {
            mult = 0;
        }
        SkillHolder.Instance.targetedEnemy.TakeDamage(damage * mult);
    }
}
