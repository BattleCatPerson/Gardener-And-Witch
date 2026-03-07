using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillManager : MonoBehaviour
{
    [SerializeField] List<EnemySkill> skillList;
    [SerializeField] float timeBetweenSkills;
    [SerializeField] float variance;
    void Start()
    {
        StartCoroutine(SelectSkill());
    }

    void Update()
    {
    }
    public IEnumerator SelectSkill()
    {
        yield return new WaitForSeconds(timeBetweenSkills);
        SelectRandomSkill();
    }
    public void SelectRandomSkill()
    {
        EnemySkill skill = skillList[Random.Range(0, skillList.Count)];
        skill.Use();
        timeBetweenSkills = skill.skillCooldown;
        timeBetweenSkills += Random.Range(-variance, variance);
        StartCoroutine(SelectSkill());
    }
}
