using UnityEngine;
using UnityEngine.InputSystem;

public class PlantAttack : PlantSkill
{
    [SerializeField] float damage;
    [SerializeField] float goodDamageMult;
    [SerializeField] float okDamageMult;
    [SerializeField] float badDamageMult;
    private void Start()
    {
        skillHolder = SkillHolder.Instance;
    }
    public void Use()
    {
        Debug.Log("ouch!");
        float mult = 1.0f;
        Debug.Log(result.ToString());
        if (result == SkillResult.Good)
        {
            mult = goodDamageMult;
        }
        else if (result == SkillResult.Ok)
        {
            mult = okDamageMult;
        }
        else if (result == SkillResult.Bad)
        {
            mult = badDamageMult;
        }
        else if (result == SkillResult.Fail)
        {
            mult = 0;
        }
        SkillHolder.Instance.targetedEnemy.TakeDamage(damage * mult);
    }

    public override void PositiveListener()
    {
        if (SkillHolder.Instance.canInput && skillHolder.activeSkill == this)
        {
            result = GetSkillResult(skillHolder.success);
            if (result == SkillResult.Fail)
            {
                skillHolder.plantSkillAnimator.SetTrigger("Fail");
            }
            else
            {
                skillHolder.plantSkillAnimator.SetTrigger("Use");
            }
        }
    }

    public override void NegativeListener()
    {
    }

    public override void AnimationListener()
    {
        Use();
    }
}
