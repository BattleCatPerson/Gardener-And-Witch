using System.Collections.Generic;
using System;
using UnityEngine;
using System.Resources;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    [Serializable]
    public class SkillTreeUnlocked
    {
        public SkillTreeElement skill;
        public bool isUnlocked;
    }
    public static SkillTreeManager Instance;
    [SerializeField] List<SkillTreeUnlocked> unlockStatuses;
    [SerializeField] TextMeshProUGUI boneText;
    [SerializeField] SkillTreePanel panel;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ResourceTracker.bones = 100;
    }
    private void Update()
    {
        boneText.text = "Bones: " + ResourceTracker.bones.ToString();
    }
    public bool SkillUnlocked(SkillTreeElement skill)
    {
        foreach (var v in unlockStatuses)
        {
            if (v.skill == skill && v.isUnlocked) return true;
        }
        return false;
    }
    public bool CanUnlock(SkillTreeElement skill)
    {
        if (SkillUnlocked(skill) || ResourceTracker.bones < skill.boneCost) return false;
        foreach (SkillTreeElement s in skill.requirements)
        {
            if (!SkillUnlocked(s)) return false;
        }
        return true;
    }
    public void UnlockSkill(SkillTreeElement skill)
    {
        foreach (var v in unlockStatuses)
        {
            if (v.skill == skill)
            {
                v.isUnlocked = true;
                ResourceTracker.bones -= skill.boneCost;
                return;
            }
        }
    }
    public void OpenPanel(SkillTreeElement skill)
    {
        panel.SetStuff(skill.skillName, skill.skillDescription, skill.boneCost);
        panel.OpenPanel();
    }
}
