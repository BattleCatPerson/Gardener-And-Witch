using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Health, Damage, Sunlight, Block, Recovery, SkillSlots
}

public class StatManager : MonoBehaviour
{
    public static Dictionary<UpgradeType, int> upgrades;
    public static StatManager Instance;
    [SerializeField] List<UpgradeType> upgradeTypes;
    [SerializeField] List<UpgradeButton> upgradeButtons;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (upgrades == null)
        {
            upgrades = new Dictionary<UpgradeType, int>();
            foreach (var s in upgradeTypes)
            {
                upgrades.Add(s, 0);
                GetButton(s, upgradeButtons).SetUpgrades(0);
            }
        }
        else
        {
            foreach (var s in upgrades.Keys)
            {
                GetButton(s, upgradeButtons).SetUpgrades(upgrades[s]);
            }
        }
        ResourceTracker.bones = 1000;
    }
    public void Upgrade(UpgradeType type)
    {
        upgrades[type]++;
        GetButton(type, upgradeButtons).SetUpgrades(upgrades[type]); 
    }
    public static UpgradeButton GetButton(UpgradeType type, List<UpgradeButton> upgradeButtons)
    {
        foreach (var u in upgradeButtons)
        {
            if (type == u.upgradeType) return u;
        }
        return null;
    }
}
