using System;
using UnityEngine;
using UnityEngine.InputSystem;
public enum TargetType
{
    singleEnemy, allEnemies, player
}
public enum SkillResult
{
    Great, Good, Bad, Ok, Fail
}

public abstract class PlantSkill : MonoBehaviour
{
    public string skillName;
    public float energyCost;
    public TargetType targetType;
    //public float cooldown;
    //public Sprite sprite;
    public string sceneName;
    public string triggerName;
    public float greatValue;
    public float goodValue;
    public float okValue;
    public float badValue;
    public void AttemptUse(/*InputAction.CallbackContext context*/)
    {
        if (SkillHolder.Instance.CanUse(this, energyCost))
        {
            SkillHolder.Instance.skillEvent += Use;
            SkillHolder.Instance.StartTargeting(this);
            //SkillHolder.Instance.StartMinigame(this);
            //SkillHolder.Instance.UseEnergy(energyCost);
            //SkillHolder.Instance.StartTimer(this);
        }
    }
    public abstract void Use(SkillResult success);
    public SkillResult GetSkillResult(float success)
    {
        if (success >= greatValue) return SkillResult.Great;
        else if (success >= goodValue) return SkillResult.Good;
        else if (success >= okValue) return SkillResult.Ok;
        else if (success >= badValue) return SkillResult.Bad;
        return SkillResult.Fail;
    }
}
