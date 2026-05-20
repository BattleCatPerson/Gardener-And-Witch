using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public UpgradeType upgradeType;
    [SerializeField] List<int> upgradeCosts;
    [SerializeField] int upgradeCount;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI upgradeCostText;
    [SerializeField] TextMeshProUGUI upgradeLevelText;

    void Start()
    {
    }
    public void SetUpgrades(int n)
    {
        upgradeCount = n;
        SetText();
    }
    public void Upgrade()
    {
        ResourceTracker.bones -= upgradeCosts[upgradeCount];
        StatManager.Instance.Upgrade(upgradeType);
    }
    public void SetText()
    {
        if (upgradeCount >= upgradeCosts.Count)
        {
            upgradeCostText.text = "Max Upgrade";
        }
        else
        {
            upgradeCostText.text = upgradeCosts[upgradeCount].ToString();
        }
        upgradeLevelText.text = "Level: " + upgradeCount.ToString();
    }
    void Update()
    {
        button.interactable = upgradeCount < upgradeCosts.Count && ResourceTracker.bones > upgradeCosts[upgradeCount];
    }
}
