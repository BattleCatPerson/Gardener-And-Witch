using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreePanel : MonoBehaviour
{
    [SerializeField] SkillTreeElement skill;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject canPurchaseText;
    [SerializeField] Button purchaseButton;
    [SerializeField] TextMeshProUGUI purchaseButtonText;
    [SerializeField] Animator animator;
    public void SetStuff(SkillTreeElement skill, string title, string description, int cost, bool purchased)
    {
        this.skill = skill;
        titleText.text = title;
        descriptionText.text = description;
        costText.text = "Bone Cost: " + cost.ToString();
        if (purchased)
        {
            purchaseButtonText.text = "Unlocked";
            purchaseButton.interactable = false;
        }
        else
        {
            purchaseButtonText.text = "Unlock";
            bool enoughBones = cost <= ResourceTracker.bones;
            canPurchaseText.SetActive(!enoughBones);
            purchaseButton.interactable = enoughBones;
        }
    }
    public void OpenPanel() => animator.SetTrigger("Open");
    public void ClosePanel () => animator.SetTrigger("Close");
    public void Unlock()
    {
        SkillTreeManager.Instance.UnlockSkill(skill);
        ClosePanel();
    }
}
