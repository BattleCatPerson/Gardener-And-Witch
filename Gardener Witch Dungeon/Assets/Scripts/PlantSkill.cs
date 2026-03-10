using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlantSkill : MonoBehaviour
{
    public float energyCost;
    public float cooldown;
    public Sprite sprite;
    public string sceneName;
    public void AttemptUse(InputAction.CallbackContext context)
    {
        if (SkillHolder.Instance.CanUse(this, energyCost))
        {
            SkillHolder.Instance.minigameReturnEvent += Use;
            SkillHolder.Instance.StartMinigame(this);
            SkillHolder.Instance.UseEnergy(energyCost);
            SkillHolder.Instance.StartTimer(this);
        }
    }
    public abstract void Use(float success);
}
