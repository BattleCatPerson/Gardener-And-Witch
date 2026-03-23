using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillManager : MonoBehaviour
{
    [SerializeField] List<EnemySkill> skillList;
    [SerializeField] float timeBetweenSkills;
    [SerializeField] float timer;
    [SerializeField] float variance;
    void Start()
    {
        //StartCoroutine(SelectSkill());
    }

    void Update()
    {
        float multiplier = TurnManager.Instance.timePaused || VictoryDefeatManager.Instance.conditionChosen ? 0 : 1;
        timer += Time.deltaTime * multiplier;
        if (timer >=  timeBetweenSkills)
        {
            SelectRandomSkill();
            timer = 0;
        }
    }
    //public IEnumerator SelectSkill()
    //{
    //    float timer = 0;
    //    while (timer < timeBetweenSkills)
    //    {
    //        float multiplier = 1;
    //        //float multiplier = TurnManager.Instance.timePaused ? 0 : 1;
    //        timer += Time.deltaTime * multiplier;
    //        yield return null;
    //    }
    //}
    public void SelectRandomSkill()
    {
        //TurnManager.Instance.timePaused = true;
        EnemySkill skill = skillList[Random.Range(0, skillList.Count)];
        skill.Use();
        timeBetweenSkills = skill.skillCooldown;
        timeBetweenSkills += Random.Range(-variance, variance);
        //StartCoroutine(SelectSkill());
    }
}
