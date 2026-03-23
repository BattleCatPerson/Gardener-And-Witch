using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject canPurchaseText;
    [SerializeField] Button purchaseButton;
    [SerializeField] Animator animator;
    public void SetStuff(string title, string description, int cost)
    {
        titleText.text = title;
        descriptionText.text = description;
        costText.text = "Bone Cost: " + cost.ToString();
        bool enoughBones = cost <= ResourceTracker.bones;
        canPurchaseText.SetActive(!enoughBones);
        purchaseButton.interactable = enoughBones;
    }
    public void OpenPanel() => animator.SetTrigger("Open");
    public void ClosePanel () => animator.SetTrigger("Close");
}
