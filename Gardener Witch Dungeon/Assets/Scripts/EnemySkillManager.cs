using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillManager : MonoBehaviour
{
    [SerializeField] List<EnemySkill> skillList;
    [SerializeField] float timeBetweenSkills;
    void Start()
    {
        
    }

    void Update()
    {
        SelectRandomSkill();   
    }
    public IEnumerator SelectSkill()
    {
        yield return new WaitForSeconds(timeBetweenSkills);
    }
    public void SelectRandomSkill()
    {
        EnemySkill skill = skillList[Random.Range(0, skillList.Count)];
        skill.Use();
        timeBetweenSkills = skill.skillCooldown;
        StartCoroutine(SelectSkill());
    }
}
