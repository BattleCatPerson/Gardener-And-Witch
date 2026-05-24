using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillEquipManager : MonoBehaviour
{
    [Serializable]
    public class UpgradeTypeToSkillList
    {
        public UpgradeType upgradeType;
        public List<PlantSkill> skills;
    }

    [SerializeField] Transform skillPanel;
    [SerializeField] List<UpgradeTypeToSkillList> skillLists;
    // skill slots
    void Start()
    {
        // access unlocked skills from statmanager
        foreach (var type in StatManager.upgrades.Keys)
        {

        }
    }

    void Update()
    {
        
    }
}
