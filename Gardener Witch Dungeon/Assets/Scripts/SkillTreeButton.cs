using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
    [SerializeField] SkillTreeElement skillTreeElement;
    [SerializeField] Button button;
    void Start()
    {
        
    }

    void Update()
    {
        button.interactable = SkillTreeManager.Instance.CanUnlock(skillTreeElement);
    }
}
