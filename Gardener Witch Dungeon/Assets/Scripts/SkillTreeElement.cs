using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Skill Tree/Skill")]
public class SkillTreeElement : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public int boneCost;
    public List<SkillTreeElement> requirements;
    public Sprite sprite;

    public PlantSkill unlockedSkill;
    public float healthBonus;
    public float turnCooldownReduction;
    public float energyIncrease;
}
