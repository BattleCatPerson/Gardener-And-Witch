using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlantSkill : MonoBehaviour
{
    public float energyCost;
    public float cooldown;
    public Sprite sprite;
    public void AttemptUse(InputAction.CallbackContext context)
    {
        if (SkillHolder.Instance.CanUse(this, energyCost))
        {
            Use();
            SkillHolder.Instance.UseEnergy(energyCost);
            SkillHolder.Instance.StartTimer(this);
        }
    }
    public abstract void Use();
}
