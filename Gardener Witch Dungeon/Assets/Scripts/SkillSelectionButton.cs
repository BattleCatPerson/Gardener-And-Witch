using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionButton : MonoBehaviour
{
    [SerializeField] PlantSkill skill;
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillCost;
    [SerializeField] Button button;
    public void SetValues(PlantSkill s)
    {
        skill = s;
        skillName.text = skill.skillName;
        skillCost.text = skill.energyCost.ToString();
        button.onClick.AddListener(skill.AttemptUse);
    }
}
