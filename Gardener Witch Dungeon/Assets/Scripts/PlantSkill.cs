using System;
using UnityEngine;
using UnityEngine.InputSystem;
public enum TargetType
{
    singleEnemy, allEnemies, player
}

public abstract class PlantSkill : MonoBehaviour
{
    public string skillName;
    public float energyCost;
    public TargetType targetType;
    //public float cooldown;
    //public Sprite sprite;
    public string sceneName;
    public void AttemptUse(/*InputAction.CallbackContext context*/)
    {
        if (SkillHolder.Instance.CanUse(this, energyCost))
        {
            SkillHolder.Instance.minigameReturnEvent += Use;
            SkillHolder.Instance.StartTargeting(this);
            //SkillHolder.Instance.StartMinigame(this);
            //SkillHolder.Instance.UseEnergy(energyCost);
            //SkillHolder.Instance.StartTimer(this);
        }
    }
    public abstract void Use(float success);
}
