using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
    [SerializeField] SkillTreeElement skillTreeElement;
    [SerializeField] Button button;
    [SerializeField] Image image;
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (skillTreeElement != null)
        {
            button.interactable = SkillTreeManager.Instance.CanUnlock(skillTreeElement);
        }
    }
    public void SetButton(SkillTreeElement skill)
    {
        skillTreeElement = skill;
        image.sprite = skill.sprite;
    }
    public void OpenPanel()
    {
        SkillTreeManager.Instance.OpenPanel(skillTreeElement);
    }
}
