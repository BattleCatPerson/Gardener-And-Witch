using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public UpgradeType upgradeType;
    [SerializeField] List<int> upgradeCosts;
    [SerializeField] int upgradeCount;
    [SerializeField] Button button;
    void Start()
    {
        
    }
    public void SetUpgrades(int n) => upgradeCount = n;
    public void Upgrade()
    {
        ResourceTracker.bones -= upgradeCosts[upgradeCount];
        StatManager.Instance.Upgrade(upgradeType);
    }
    void Update()
    {
        button.interactable = ResourceTracker.bones > upgradeCosts[upgradeCount];
    }
}
